// ----------------------------------------------------------
// <copyright file="SmashSession.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading;
    using System.Windows.Threading;
    using Microsoft.Hawaii;
    using Microsoft.Hawaii.Smash.Client.Contracts;
    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// A Smash session object. This holds all the state of a joined session.
    /// </summary>
    public sealed class SmashSession : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private const int ThreadEnded = 0;
        
        /// <summary>
        /// 
        /// </summary>
        private const int ThreadStopped = 1;
        
        /// <summary>
        /// 
        /// </summary>
        private const int ThreadPrepareToStop = 2;
        
        /// <summary>
        /// 
        /// </summary>
        private const int ThreadActive = 3;

#if WINDOWS_PHONE
        /// <summary>
        /// 
        /// </summary>
        private const string SessionStateKey = "SmashSessionState";
#endif

        /// <summary>
        /// 
        /// </summary>
        private SmashClient client;

        /// <summary>
        /// The client identity
        /// </summary>
        private ClientIdentity identity;

        /// <summary>
        /// 
        /// </summary>
        private Guid meetingToken;
        
        /// <summary>
        /// 
        /// </summary>
        private Guid sessionID;
        
        /// <summary>
        /// 
        /// </summary>
        private Guid clientID;
        
        /// <summary>
        /// 
        /// </summary>
        private long lastKnownRowTime;
        
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, ISmashTable> tables;
        
        /// <summary>
        /// 
        /// </summary>
        private int isActive;
        
        /// <summary>
        /// 
        /// </summary>
        private Thread sessionThread;
        
        /// <summary>
        /// 
        /// </summary>
        private SmashClient sessionThreadClient;
        
        /// <summary>
        /// 
        /// </summary>
        private Dispatcher dispatcher;
        
        /// <summary>
        /// 
        /// </summary>
        private RowDeduper deduper;
        
        /// <summary>
        /// 
        /// </summary>
        private int refreshInterval;
        
        /// <summary>
        /// 
        /// </summary>
        private AutoResetEvent triggerRefresh;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="dispatcher"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="clientID"></param>
        /// <param name="tables"></param>
        internal SmashSession(ClientIdentity identity, Dispatcher dispatcher, Guid meetingToken, Guid sessionID, Guid clientID, ISmashTable[] tables)
        {
            this.identity = identity.Copy();
            this.client = new SmashClient(this.identity);
            this.dispatcher = dispatcher;
            this.meetingToken = meetingToken;
            this.sessionID = sessionID;
            this.clientID = clientID;
            this.LastKnownRowTime = 0;
            this.IsActive = ThreadEnded;
            this.tables = new Dictionary<int, ISmashTable>();

            if (tables != null)
            {
                foreach (ISmashTable table in tables)
                {
                    this.tables.Add(table.TypeHash, table);
                    table.SetSession(this);
                }
            }

            this.deduper = new RowDeduper(this.clientID.ToString(), false);

            this.triggerRefresh = new AutoResetEvent(false);

            // RefreshInterval 0 by default, meaning we use long poll
            this.StartSessionThread();
        }

#if WINDOWS_PHONE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="dispatcher"></param>
        /// <param name="state"></param>
        /// <param name="rows"></param>
        /// <param name="tables"></param>
        internal SmashSession(ClientIdentity identity, Dispatcher dispatcher, SessionState state, List<DataRow_Wire> rows, ISmashTable[] tables)
        {
            this.identity = identity.Copy();
            this.client = new SmashClient(this.identity);
            this.dispatcher = dispatcher;
            this.meetingToken = state.MeetingToken;
            this.sessionID = state.SessionID;
            this.clientID = state.ClientID;
            this.LastKnownRowTime = state.LastKnownRowTime;
            this.IsActive = ThreadEnded;
            this.tables = new Dictionary<int, ISmashTable>();

            if (tables != null)
            {
                foreach (ISmashTable table in tables)
                {
                    this.tables.Add(table.TypeHash, table);
                    table.SetSession(this);
                }
            }

            this.deduper = new RowDeduper(this.clientID.ToString(), false);

            this.triggerRefresh = new AutoResetEvent(false);

            // RefreshInterval 0 by default, meaning we use long poll
            this.ProcessDataRows(rows);

            this.StartSessionThread();
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        internal ClientIdentity ClientIdentity
        {
            get
            {
                return this.identity;
            }
        }

        /// <summary>
        /// Gets or sets the refresh interval (in ms) for data in all SmashTable joined to the session.
        /// A value of 0 indicates that the Session will use 'long poll' for almost instantaneous refresh of session data.
        /// A non-0 value specifies the number of ms between refresh operations.
        /// </summary>
        public int RefreshInterval
        {
            get
            {
                return Interlocked.CompareExchange(ref this.refreshInterval, 0, 0);
            }

            set
            {
                Interlocked.Exchange(ref this.refreshInterval, value);
                this.triggerRefresh.Set();
            }
        }

        /// <summary>
        /// Gets the meeting token of the session.
        /// </summary>
        public Guid MeetingToken
        {
            get
            {
                return this.meetingToken;
            }
        }

        /// <summary>
        /// Gets the session ID of the session.
        /// </summary>
        public Guid SessionID
        {
            get
            {
                return this.sessionID;
            }
        }

        /// <summary>
        /// Gets the unique identifier of the current client.
        /// </summary>
        public Guid ClientID
        {
            get
            {
                return this.clientID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Dispatcher Dispatcher
        {
            get
            {
                return this.dispatcher;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private long LastKnownRowTime
        {
            get
            {
                lock (this)
                {
                    return this.lastKnownRowTime;
                }
            }

            set
            {
                lock (this)
                {
                    this.lastKnownRowTime = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private SmashClient Client
        {
            get
            {
                return this.client;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int IsActive
        {
            get
            {
                return Interlocked.CompareExchange(ref this.isActive, 0, 0);
            }

            set
            {
                Interlocked.Exchange(ref this.isActive, value);
            }
        }

#if WINDOWS_PHONE
        /// <summary>
        /// Re-activates a session when application comes back from tombstone state.
        /// </summary>
        /// <param name="clientId">Azure Data Market client id used for authentication.</param>
        /// <param name="clientSecret">Azure Data Market client secret used for authentication.</param>
        /// <param name="dispatcher">The Dispatcher object. a dispatcher must be specified in order to use data binding with Smash.</param>
        /// <param name="stateDictionary">The tombstoned state.</param>
        /// <param name="tables">The set of SmashTable objects to join to the re-activated session.</param>
        /// <returns>the Smash session object.</returns>
        public static SmashSession JoinSessionFromState(string clientId, string clientSecret, Dispatcher dispatcher, IDictionary<string, object> stateDictionary, ISmashTable[] tables)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("admClientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("admClientSecret");
            }

            return JoinSessionFromState(new AdmAuthClientIdentity(clientId, clientSecret, SmashClientREST.ServiceScope), dispatcher, stateDictionary, tables);
        }

        private static SmashSession JoinSessionFromState(ClientIdentity identity, Dispatcher dispatcher, IDictionary<string, object> stateDictionary, ISmashTable[] tables)
        {
            SmashSession session = null;
            IsolatedStorageSettings storedState = IsolatedStorageSettings.ApplicationSettings;

            if (stateDictionary.ContainsKey(SessionStateKey) && ((IDictionary<string, object>)storedState).ContainsKey(SessionStateKey))
            {
                SessionState state = stateDictionary[SessionStateKey] as SessionState;
                List<DataRow_Wire> rows = storedState[SessionStateKey] as List<DataRow_Wire>;

                if (state != null)
                {
                    session = new SmashSession(identity, dispatcher, state, rows, tables);
                }
            }

            return session;
        }
#endif

        /// <summary>
        /// Forces a refresh of data in SmashTable associated with session. If session uses long poll, this has no effect.
        /// </summary>
        public void Refresh()
        {
            this.triggerRefresh.Set();
        }

        /// <summary>
        /// Checks if a record has been created by current client.
        /// </summary>
        /// <typeparam name="T">generic T.</typeparam>
        /// <param name="record">A record to be checked for ownership.</param>
        /// <returns>true if record has been created by current client, false otherwise.</returns>
        public bool IsRecordOwned<T>(SmashRecordBase<T> record) where T: class
        {
            return record.CreatorClient.Equals(this.ClientID);
        }

        /// <summary>
        /// Initiates shutdown of a session. The session will disconnect from the service, the session data in the Smash service is preserved.
        /// </summary>
        public void Shutdown()
        {
            Action shutdown = new Action(() =>
            {
                if (this.sessionThread != null)
                {
                    this.IsActive = ThreadPrepareToStop;
                    if (this.sessionThreadClient != null)
                    {
                        this.sessionThreadClient.AbortGetRows();
                    }

                    this.IsActive = ThreadStopped;
                    while (this.IsActive != ThreadEnded)
                    {
                        Thread.Sleep(1);
                    }

                    this.sessionThread = null;
                }
            });

#if WINDOWS_PHONE
            // Needs to be done on Dispatcher on phone, or else hangs
            Dispatcher.BeginInvoke(this.Shutdown);
#else
            shutdown.Invoke();
#endif
        }

        /// <summary>
        /// The IDisposable.Dispose() method.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:Shutdown will signal session thread to end, which will dispose sessionThreadClient")]
        public void Dispose()
        {
            if (this.client != null)
            {
                this.Shutdown();
                this.client.Dispose();
                this.client = null;
            }

            if (this.triggerRefresh != null)
            {
                this.triggerRefresh.Dispose();
                this.triggerRefresh = null;
            }
        }

#if WINDOWS_PHONE
        /// <summary>
        /// Prepares a session for tombstoning.
        /// </summary>
        /// <param name="stateDictionary">Dictionary to receive tombstoned state.</param>
        public void DeactivateSession(IDictionary<string, object> stateDictionary)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
            {
                this.Shutdown();
            }));

            IsolatedStorageSettings storedState = IsolatedStorageSettings.ApplicationSettings;

            stateDictionary[SessionStateKey] = null;
            storedState[SessionStateKey] = null;

            try
            {
                SessionState state = new SessionState();

                state.MeetingToken = this.MeetingToken;
                state.LastKnownRowTime = this.LastKnownRowTime;
                state.ClientID = this.ClientID;
                state.SessionID = this.SessionID;

                List<DataRow_Wire> rows = new List<DataRow_Wire>();
                foreach (KeyValuePair<int, ISmashTable> t in this.tables)
                {
                    t.Value.GetState(rows);
                }

                storedState[SessionStateKey] = rows;

                storedState.Save();

                stateDictionary[SessionStateKey] = state;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Resumes session synchronization after rejoining from tombstone states.
        /// </summary>
        public void ResumeSession()
        {
            this.StartSessionThread();
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rowList"></param>
        /// <param name="typeHash"></param>
        /// <param name="rows"></param>
        /// <param name="changedOnly"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "The Stream gets disposed correctly")]
        internal IEnumerable<DataRow_Wire> PrepareDataRows<T>(IEnumerable<T> rowList, int typeHash, List<DataRow_Wire> rows, bool changedOnly) where T : SmashRecordBase<T>
        {
            if (rows == null)
            {
                rows = new List<DataRow_Wire>();
            }

            foreach (T r in rowList)
            {
                if (!r.NeedsUpdate && changedOnly)
                {
                    continue;
                }

                DataRow_Wire row = new DataRow_Wire();
                row.GUID = r.RecordID;
                row.CreatorClient = r.CreatorClient;
                row.TimeStamp = r.TimeStamp;
                row.Action = r.SendAction;
                row.TypeHash = typeHash;
                rows.Add(row);

                if (row.Action == SendRowAction.Delete)
                {
                    row.Blob = new byte[0];
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        DataContractJsonSerializer ds = new DataContractJsonSerializer(r.GetType());
                        ds.WriteObject(ms, r);

                        row.Blob = ms.ToArray();

                        ms.Close();
                    }

                    if (row.Blob.Length > 0x10000)
                    {
                        throw new ChangeSetLimitException("Maximum data size for record exceeded");
                    }
                }
            }

            return rows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        internal void SendDataRowsAsync(IEnumerable<DataRow_Wire> rows, ServiceAgent<SendRowsResponse>.OnCompleteDelegate onComplete, object state)
        {
            this.Client.SendRowsAsync(this.meetingToken, this.sessionID, this.clientID, rows, onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="timeStampOverride"></param>
        /// <param name="rowGuids"></param>
        /// <param name="changeContextCall"></param>
        internal void ProcessDataRows(IEnumerable<DataRow_Wire> rows, long timeStampOverride, IEnumerable<Guid> rowGuids, bool changeContextCall)
        {
            IEnumerator<Guid> rg = rowGuids != null ? rowGuids.GetEnumerator() : null;
            lock (this.deduper)
            {
                List<Action> actions = new List<Action>();

                foreach (DataRow_Wire row in rows)
                {
                    long rowTimeStamp = row.TimeStamp;
                    Guid rowGUID = row.GUID;

                    if (rg != null && timeStampOverride != -1)
                    {
                        rg.MoveNext();
                        rowGUID = rg.Current;
                        rowTimeStamp = timeStampOverride++; // The service internally increments the timestamp for each row to preserve sort order
                    }

                    if (!this.deduper.Known(rowGUID, rowTimeStamp))
                    {
                        ISmashTable table;
                        bool foundTable;
                        lock (this.tables)
                        {
                            foundTable = this.tables.TryGetValue(row.TypeHash, out table);
                        }

                        if (foundTable)
                        {
                            actions.Add(table.PrepareSendRecord(rowGUID, row.CreatorClient.Equals(Guid.Empty) ? this.clientID : row.CreatorClient, rowTimeStamp, row.Action, row.ModificationFlag, row.Blob));
                            this.deduper.Add(rowGUID, rowTimeStamp);
                        }
                        else
                        {
                            // Ignore this. Simply means we do not know the table. Maybe we are an old client or table has not yet been joined.
                            // Also do not add record to this.deduper, so we catch it upon join.
                        }
                    }

                    // If we received this data from the service, we need to update our lastknownrowtime. If we received it as result of local savechanges, then do not update lastknownrowtime, so we do not miss server changes
                    if (!changeContextCall && row.TimeStamp > this.LastKnownRowTime)
                    {
                        this.LastKnownRowTime = row.TimeStamp;
                    }
                }

                Action work = new Action(() =>
                {
                    foreach (Action action in actions)
                    {
                        action.Invoke();
                    }
                });

                DispatchWork(work);
            }
        }

        internal void DispatchWork(Action work)
        {
            if (Dispatcher != null)
            {
                Dispatcher.BeginInvoke(work);
            }
            else
            {
                work.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        private void ProcessDataRows(IEnumerable<DataRow_Wire> rows)
        {
            this.ProcessDataRows(rows, -1, null, false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartSessionThread()
        {
            this.sessionThread = new Thread(new ThreadStart(() =>
            {
                while (this.IsActive != ThreadEnded)
                {
                    Thread.Sleep(1);
                }

                this.sessionThreadClient = new SmashClient(this.identity);

                try
                {
                    this.IsActive = ThreadActive;
                    while (this.IsActive > ThreadStopped)
                    {
                        try
                        {
                            if (this.IsActive == ThreadActive)
                            {
                                if (this.triggerRefresh.WaitOne(RefreshInterval))
                                {
                                    this.sessionThreadClient.UseLongPoll = RefreshInterval == 0 ? true : false;
                                }

                                DataRow_Wire[] rowsOut;
                                bool aborted = this.sessionThreadClient.GetRows(this.meetingToken, this.sessionID, this.clientID, this.LastKnownRowTime, out rowsOut);

                                if (aborted)
                                {
                                    lock (this.deduper)
                                    {
                                        this.LastKnownRowTime = 0;
                                    }
                                }
                                else
                                {
                                    this.ProcessDataRows(rowsOut);
                                }
                            }
                            else if (this.IsActive == ThreadPrepareToStop)
                            {
                                Thread.Sleep(1);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                finally
                {
                    this.sessionThreadClient.Dispose();
                    this.sessionThreadClient = null;
                    this.IsActive = ThreadEnded;
                }
            }));

            this.sessionThread.Start();
        }

#if WINDOWS_PHONE
        /// <summary>
        /// Tombstone session state. Applications do not use this directly.
        /// </summary>
        [DataContract]
        public class SessionState
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public SessionState()
            {
            }

            /// <summary>
            /// Gets or sets the meeting token of the tombstoned session state.
            /// </summary>
            [DataMember]
            public Guid MeetingToken { get; set; }

            /// <summary>
            /// Gets or sets the session ID of the tombstoned session state.
            /// </summary>
            [DataMember]
            public Guid SessionID { get; set; }

            /// <summary>
            /// Gets or sets the client identifier of the tombstoned session state.
            /// </summary>
            [DataMember]
            public Guid ClientID { get; set; }

            /// <summary>
            /// Gets or sets the last known synchronization time of the tombstoned session state.
            /// </summary>
            [DataMember]
            public long LastKnownRowTime { get; set; }
        }
#endif
    }
}
