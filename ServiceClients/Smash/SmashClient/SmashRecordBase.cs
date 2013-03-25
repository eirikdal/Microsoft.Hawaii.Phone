// ----------------------------------------------------------
// <copyright file="SmashRecordBase.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
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
    /// The base class for all record types. Application record types used in SmashTable must derive from this.
    /// See ChatRecord of the SmashSampleApp for an example.
    /// </summary>
    /// <typeparam name="T">The derived record type. Usage: public class MyRecord: SmashRecordBase&lt;MyRecord&gt; { ... }.</typeparam>
    [DataContract]
    public class SmashRecordBase<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        private SendRowAction sendAction;

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        private Guid recordID;

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        private Guid creatorClient;

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        private long timeStamp;

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        private bool frozen;

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        private bool modified;

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        private int modificationFlag;

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        private object userData;

        /// <summary>
        /// Initializes a new instance of the SmashRecordBase class.
        /// </summary>
        protected SmashRecordBase()
        {
            this.sendAction = SendRowAction.Add;
            this.frozen = false;
            this.modified = false;
            this.timeStamp = 0;
            this.modificationFlag = 0;
            this.recordID = Guid.Empty;
        }

        /// <summary>
        /// Associates user state with a record. This member is not synchronized with other session participants.
        /// </summary>
        [IgnoreDataMember]
        public object UserData
        {
            get
            {
                return this.userData;
            }

            set
            {
                this.userData = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        internal SendRowAction SendAction
        {
            get
            {
                return this.sendAction;
            }

            set
            {
                this.sendAction = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        internal Guid RecordID
        {
            get
            {
                return this.recordID;
            }

            set
            {
                this.recordID = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        internal Guid CreatorClient
        {
            get
            {
                return this.creatorClient;
            }

            set
            {
                this.creatorClient = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        internal long TimeStamp
        {
            get
            {
                return this.timeStamp;
            }

            set
            {
                this.timeStamp = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        internal int ModificationFlag
        {
            get
            {
                return this.modificationFlag;
            }

            set
            {
                this.modificationFlag = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        internal bool NeedsUpdate
        {
            get
            {
                if (this.sendAction == SendRowAction.Add)
                {
                    return true;
                }
                else if (this.sendAction == SendRowAction.Delete)
                {
                    return true;
                }
                else if (this.sendAction == SendRowAction.Update && this.modified)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a mutable copy of the record.
        /// </summary>
        /// <param name="context">The change context to associate the mutable copy with.</param>
        /// <returns>A mutable copy of the record</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202", Justification = "Do not dispose objects multiple times")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "the using statement does dispose")]
        public T GetUnfrozen(ISmashTableChangeContext context)
        {
            if (!this.frozen)
            {
                throw new InvalidOperationException("Attempt to unfreeze editable record");
            }

            T unfrozen = (context as ISmashTableChangeContextInternal).GetUnfrozen(this) as T;

            if (unfrozen == null)
            {
                Exception error = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    try
                    {
                        DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(T));
                        ds.WriteObject(ms, this);
                        ms.Position = 0;
                        unfrozen = ds.ReadObject(ms) as T;
                        (unfrozen as SmashRecordBase<T>).TimeStamp = this.TimeStamp;
                        (unfrozen as SmashRecordBase<T>).RecordID = this.RecordID;
                        (unfrozen as SmashRecordBase<T>).CreatorClient = this.CreatorClient;
                        (unfrozen as SmashRecordBase<T>).ModificationFlag = this.ModificationFlag;
                        (unfrozen as SmashRecordBase<T>).SendAction = SendRowAction.Update;
                        (unfrozen as SmashRecordBase<T>).Thaw();
                        ms.Close();
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                }

                if (error != null)
                {
                    throw error;
                }

                (context as ISmashTableChangeContextInternal).AddUnfrozen(this, unfrozen);
            }

            return unfrozen;
        }

        /// <summary>
        /// Get a unique identifier for the record, stable for the lifetime of the record.
        /// </summary>
        /// <returns>a unique identifier.</returns>
        public Guid GetRecordID()
        {
            return this.recordID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal bool IsLatest()
        {
            return this.ModificationFlag == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Freeze()
        {
            this.frozen = true;
            this.modified = false;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Thaw()
        {
            this.frozen = false;
            this.modified = false;
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Delete()
        {
            this.OnChange();
            this.SendAction = SendRowAction.Delete;
        }

        /// <summary>
        /// Derived record types must call this upon set operations on their public members.
        /// </summary>
        protected void OnChange()
        {
            if (this.frozen)
            {
                throw new MemberAccessException("Attempt to change frozen record");
            }
            else if (this.SendAction == SendRowAction.Delete)
            {
                throw new MemberAccessException("Attempt to change record marked for deletion");
            }

            this.modified = true;
        }
    }
}
