// ----------------------------------------------------------
// <copyright file="SmashTable.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading;
    using System.Windows.Threading;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// Generic class for a SmashTable.
    /// </summary>
    /// <typeparam name="T">Type of records. T must derive from SmashRecordBase.</typeparam>
    public sealed class SmashTable<T> : ReadOnlyObservableCollection<T>, ISmashTable where T : SmashRecordBase<T>
    {
        /// <summary>
        /// 
        /// </summary>
        private const int ModHash = 65521;

        /// <summary>
        /// 
        /// </summary>
        private SmashSession session;

        /// <summary>
        /// 
        /// </summary>
        private int typeHash;

#if WINDOWS_PHONE
        /// <summary>
        /// 
        /// </summary>
        private object SyncRoot = new object();
#endif

        /// <summary>
        /// Initializes a new instance of the SmashTable class.
        /// (T,name) must be unique for all SmashTable joined to a single session.
        /// </summary>
        /// <param name="name">The name of the table.</param>
        public SmashTable(string name)
            : base(new ObservableCollection<T>())
        {
            Type t = typeof(T);
            StringBuilder sb = new StringBuilder();
            sb.Append("Namespace=");
            sb.Append(t.Namespace);

            sb.Append(";Type=");
            sb.Append(t.Name);

            sb.Append(";ChannelName=");
            sb.Append(name);

            foreach (MemberInfo m in t.GetMembers())
            {
                if (m.MemberType == MemberTypes.Property)
                {
                    sb.Append(";MemberType=");
                    sb.Append(m.MemberType.ToString());
                    sb.Append(";MemberName=");
                    sb.Append(m.Name);
                }
            }

            this.typeHash = this.GetNameHash(sb.ToString());
        }

        /// <summary>
        /// Gets the hash of (T,name) of the SmashTable. Applications do not use this directly.
        /// </summary>
        int ISmashTable.TypeHash
        {
            get
            {
                return this.typeHash;
            }
        }

        /// <summary>
        /// Get a change context
        /// </summary>
        /// <returns>The change context used to modify the table</returns>
        public ISmashTableChangeContext GetTableChangeContext()
        {
            return new SmashTableChangeContext<T>(this.typeHash, this.session);
        }

        /// <summary>
        /// Smash uses this internally to associate a SmashTable with a session. Applications do not use this directly.
        /// </summary>
        /// <param name="session"></param>
        void ISmashTable.SetSession(SmashSession session)
        {
            this.session = session;
            this.session.DispatchWork(new Action(() =>
                {
                    lock (this.SyncRoot)
                    {
                        base.Items.Clear();
                    }
                }));
        }

#if WINDOWS_PHONE
        /// <summary>
        /// Smash uses this internally to capture tombstone state for a session. Applications do not use this directly.
        /// </summary>
        /// <param name="rows"></param>
        void ISmashTable.GetState(List<DataRow_Wire> rows)
        {
            this.session.PrepareDataRows(this, this.typeHash, rows, false);
        }
#else
        /// <summary>
        /// Gets a synchronization object. 
        /// Applications use this to lock access to a SmashTable when using it from multiple threads.
        /// Multi-threaded access to SmashTable object cannot be used in conjunction with data binding.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return base.Items;
            }
        }
#endif

        /// <summary>
        /// Internal method used by Smash to synchronize table state with state shared by the Smash service.
        /// This is used internally by Smash and applications should not call this API.
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="creatorClient"></param>
        /// <param name="timeStamp"></param>
        /// <param name="action"></param>
        /// <param name="modificationFlag"></param>
        /// <param name="blob"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202", Justification = "Do not dispose objects multiple times")]
        Action ISmashTable.PrepareSendRecord(Guid recordID, Guid creatorClient, long timeStamp, SendRowAction action, int modificationFlag, byte[] blob)
        {
            Action work = new Action(() =>
            {
                lock (this.SyncRoot)
                {
                    if (action == SendRowAction.Delete)
                    {
                        List<T> todelete = new List<T>(from record in base.Items where record.RecordID.Equals(recordID) select record);

                        foreach (T delete in todelete)
                        {
                            base.Items.Remove(delete);
                            break;
                        }
                    }
                    else
                    {
                        using (MemoryStream ms = new MemoryStream(blob))
                        {
                            DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
                            T t = ds.ReadObject(ms) as T;
                            t.RecordID = recordID;
                            t.CreatorClient = creatorClient;
                            t.TimeStamp = timeStamp;
                            t.ModificationFlag = modificationFlag;
                            t.Freeze();
                            ms.Close();

                            switch (action)
                            {
                                case SendRowAction.Add:
                                    base.Items.Add(t);
                                    break;
                                case SendRowAction.Update:
                                    List<T> toreplace = new List<T>(from record in base.Items where record.RecordID.Equals(recordID) select record);

                                    foreach (T replace in toreplace)
                                    {
                                        base.Items[base.Items.IndexOf(replace)] = t;
                                        break;
                                    }

                                    break;
                            }
                        }
                    }
                }
            });

            return work;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int GetNameHash(string s)
        {
            uint a = 1, b = 0;

            foreach (char c in s)
            {
                a = (a + c) % ModHash;
                b = (b + a) % ModHash;
            }

            return (int)((b << 16) | a);
        }
    }
}
