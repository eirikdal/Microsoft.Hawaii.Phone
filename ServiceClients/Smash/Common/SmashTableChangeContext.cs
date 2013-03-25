// ----------------------------------------------------------
// <copyright file="SmashTableChangeContext.cs" company="Microsoft">
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

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Contracts;
    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// Delegate type for completion handler of SaveChangesAsync.
    /// </summary>
    /// <param name="sender">The SmashTableChangeContext instance calling the completion handler.</param>
    /// <param name="e">The completion arguments.</param>
    public delegate void SaveChangesCompletedHandler(object sender, SaveChangesCompletedArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The record type for the SmashTable from which the SmashTableChangeContext was obtained.</typeparam>
    internal class SmashTableChangeContext<T> : ISmashTableChangeContext, ISmashTableChangeContextInternal where T : SmashRecordBase<T>
    {
        /// <summary>
        /// 
        /// </summary>
        private const int ChangesPerAdd = 1;

        /// <summary>
        /// 
        /// </summary>
        private const int ChangesPerUpdate = 3;    // 1 for lock, 1 for update, 1 for setting ModificationFlag in old row

        /// <summary>
        /// 
        /// </summary>
        private const int MaxBatchSize = 100;      // Max 100 changes in change set for Azure table

        /// <summary>
        /// 
        /// </summary>
        private SmashSession session;

        /// <summary>
        /// 
        /// </summary>
        private int typeHash;

        /// <summary>
        /// 
        /// </summary>
        private List<T> records;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<object, object> unfrozens;

        /// <summary>
        /// 
        /// </summary>
        private int batchSize;

        /// <summary>
        /// Initializes a new instance of the SmashTableChangeContext class.
        /// </summary>
        /// <param name="typeHash"></param>
        /// <param name="session"></param>
        internal SmashTableChangeContext(int typeHash, SmashSession session)
        {
            this.typeHash = typeHash;
            this.session = session;
            this.records = new List<T>();
            this.unfrozens = new Dictionary<object, object>();
            this.batchSize = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public event SaveChangesCompletedHandler SaveChangesCompleted;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frozen"></param>
        /// <returns></returns>
        object ISmashTableChangeContextInternal.GetUnfrozen(object frozen)
        {
            if (unfrozens == null)
            {
                throw new InvalidOperationException("ISmashTableChangeContext not reusable after SaveChangesAsync.");
            }

            object unfrozen;
            this.unfrozens.TryGetValue(frozen, out unfrozen);
            return unfrozen;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frozen"></param>
        /// <param name="unfrozen"></param>
        void ISmashTableChangeContextInternal.AddUnfrozen(object frozen, object unfrozen)
        {
            if (unfrozens == null)
            {
                throw new InvalidOperationException("ISmashTableChangeContext not reusable after SaveChangesAsync.");
            }

            if (this.batchSize + ChangesPerUpdate > MaxBatchSize)
            {
                throw new ChangeSetLimitException("The maximum number of add/update/delete operations for the ISmashTableChangeContext has been exceeded");
            }

            this.unfrozens.Add(frozen, unfrozen);
            this.records.Add(unfrozen as T);
            this.batchSize += ChangesPerUpdate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void SaveChangesAsync(object state)
        {
            if (unfrozens == null)
            {
                throw new InvalidOperationException("ISmashTableChangeContext not reusable after SaveChangesAsync.");
            }

            IEnumerable<DataRow_Wire> rows = this.session.PrepareDataRows(this.records, this.typeHash, null, true);

            ThreadPool.QueueUserWorkItem(new WaitCallback((object o) =>
                {
                    ServiceAgent<SendRowsResponse>.OnCompleteDelegate userHandler = new ServiceAgent<SendRowsResponse>.OnCompleteDelegate((response)=>
                        {
                            SaveChangesCompletedArgs e = new SaveChangesCompletedArgs(response.Exception, response.Aborted, state);

                            if (response.Exception == null && !response.Aborted)
                            {
                                this.session.ProcessDataRows(rows, response.TimeStampStart, response.RowGuids, true);
                            }

                            OnSaveChangesCompleted(e);
                        });
                    this.session.SendDataRowsAsync(rows, userHandler, null);
                }));

            foreach (SmashRecordBase<T> r in this.records)
            {
                r.Freeze();
            }

            this.records = null;
            this.unfrozens = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void Add(object r)
        {
            if (unfrozens == null)
            {
                throw new InvalidOperationException("ISmashTableChangeContext not reusable after SaveChangesAsync.");
            }

            if (this.batchSize + ChangesPerAdd > MaxBatchSize)
            {
                throw new ChangeSetLimitException("The maximum number of add/update/delete operations for the ISmashTableChangeContext has been exceeded");
            }

            if (this.records.Contains(r as T))
            {
                throw new InvalidOperationException("Cannot add record more than once or add a record marked for deletion");
            }

            this.records.Add(r as T);
            this.batchSize += ChangesPerAdd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        public void Delete(object r)
        {
            if (unfrozens == null)
            {
                throw new InvalidOperationException("ISmashTableChangeContext not reusable after SaveChangesAsync.");
            }

            T u = (r as T).GetUnfrozen(this);

            u.Delete();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSaveChangesCompleted(SaveChangesCompletedArgs e)
        {
            if (this.SaveChangesCompleted != null)
            {
                this.SaveChangesCompleted(this, e);
            }
        }
    }
}
