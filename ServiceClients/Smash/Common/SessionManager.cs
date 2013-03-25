// ----------------------------------------------------------
// <copyright file="SessionManager.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Threading;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// Delegate type for completion handler of JoinSessionAsync.
    /// </summary>
    /// <param name="sender">The SessionManager instance calling the completion handler.</param>
    /// <param name="e">The completion arguments.</param>
    public delegate void JoinSessionCompletedHandler(object sender, JoinSessionCompletedArgs e);

    /// <summary>
    /// Delegate type for completion handler of CreateBlobAsync.
    /// </summary>
    /// <param name="sender">The SessionManager instance calling the completion handler.</param>
    /// <param name="e">The completion arguments.</param>
    public delegate void CreateBlobCompletedHandler(object sender, CreateBlobCompletedArgs e);

    /// <summary>
    /// Delegate type for completion handler of CreateSessionAsync.
    /// </summary>
    /// <param name="sender">The SessionManager instance calling the completion handler.</param>
    /// <param name="e">The completion arguments.</param>
    public delegate void CreateSessionCompletedHandler(object sender, CreateSessionCompletedArgs e);

    /// <summary>
    /// Delegate type for completion handler of ModifySessionAsync.
    /// </summary>
    /// <param name="sender">The SessionManager instance calling the completion handler.</param>
    /// <param name="e">The completion arguments.</param>
    public delegate void ModifySessionCompletedHandler(object sender, ModifySessionCompletedArgs e);

    /// <summary>
    /// Delegate type for completion handler of EnumSessionsAsync.
    /// </summary>
    /// <param name="sender">The SessionManager instance calling the completion handler.</param>
    /// <param name="e">The completion arguments.</param>
    public delegate void EnumSessionsCompletedHandler(object sender, EnumSessionCompletedArgs e);

    /// <summary>
    /// Delegate type for completion handler of GetSessionInfoAsync.
    /// </summary>
    /// <param name="sender">The SessionManager instance calling the completion handler.</param>
    /// <param name="e">The completion arguments.</param>
    public delegate void GetSessionInfoCompletedHandler(object sender, GetSessionInfoCompletedArgs e);

    /// <summary>
    /// Delegate type for completion handler of WipeSessionAsync.
    /// </summary>
    /// <param name="sender">The SessionManager instance calling the completion handler.</param>
    /// <param name="e">The completion arguments.</param>
    public delegate void WipeSessionCompletedHandler(object sender, WipeSessionCompletedArgs e);

    /// <summary>
    /// Manager class for Smash sessions.
    /// </summary>
    public sealed class SessionManager
    {
        /// <summary>
        /// Constructor for SessionManager.
        /// </summary>
        public SessionManager()
        {
        }

        /// <summary>
        /// Event raised upon completion of JoinSessionAsync.
        /// </summary>
        public event JoinSessionCompletedHandler JoinSessionCompleted;

        /// <summary>
        /// Event raised upon completion of CreateBlobAsync.
        /// </summary>
        public event CreateBlobCompletedHandler CreateBlobCompleted;

        /// <summary>
        /// Event raised upon completion of CreateSessionAsync.
        /// </summary>
        public event CreateSessionCompletedHandler CreateSessionCompleted;

        /// <summary>
        /// Event raised upon completion of ModifySessionAsync.
        /// </summary>
        public event ModifySessionCompletedHandler ModifySessionCompleted;

        /// <summary>
        /// Event raised upon completion of EnumSessionsAsync.
        /// </summary>
        public event EnumSessionsCompletedHandler EnumSessionsCompleted;

        /// <summary>
        /// Event raised upon completion of GetSessionInfoAsync.
        /// </summary>
        public event GetSessionInfoCompletedHandler GetSessionInfoCompleted;

        /// <summary>
        /// Event raised upon completion of WipeSessionAsync.
        /// </summary>
        public event WipeSessionCompletedHandler WipeSessionCompleted;

        

        /// <summary>
        /// Joins a Smash session. User+email+device must be unique across all joinees of a session.
        /// </summary>
        /// <param name="admClientId">Azure Data Market client id used for authentication.</param>
        /// <param name="admClientSecret">Azure Data Market client secret used for authentication.</param>
        /// <param name="dispatcher">The Dispatcher object. a dispatcher must be specified in order to use data binding with Smash.</param>
        /// <param name="meetingToken">The meeting token of the session to join.</param>
        /// <param name="user">User name joining the session.</param>
        /// <param name="email">email address of the user joining the session.</param>
        /// <param name="device">Device name of the user joining the session.</param>
        /// <param name="tables">Array of ISmashTable for all SmashTable objects to be associated with the joined session.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void JoinSessionAsync(string admClientId, string admClientSecret, Dispatcher dispatcher, Guid meetingToken, string user, string email, string device, ISmashTable[] tables, object state)
        {
            if (string.IsNullOrEmpty(admClientId))
            {
                throw new ArgumentNullException("admClientId");
            }

            if (string.IsNullOrEmpty(admClientSecret))
            {
                throw new ArgumentNullException("admClientSecret");
            }

            this.JoinSessionAsync(new AdmAuthClientIdentity(admClientId, admClientSecret, SmashClientREST.ServiceScope), dispatcher, meetingToken, user, email, device, false, tables, state);
        }

        /// <summary>
        /// Joins a Smash session. User+email+device must be unique across all joinees of a session.
        /// </summary>
        /// <param name="admClientId">Azure Data Market client id used for authentication.</param>
        /// <param name="admClientSecret">Azure Data Market client secret used for authentication.</param>
        /// <param name="dispatcher">The Dispatcher object. a dispatcher must be specified in order to use data binding with Smash.</param>
        /// <param name="meetingToken">The meeting token of the session to join.</param>
        /// <param name="user">User name joining the session.</param>
        /// <param name="email">email address of the user joining the session.</param>
        /// <param name="device">Device name of the user joining the session.</param>
        /// <param name="forceRejoin">A value of 'true' overrides user+email+device uniqueness requirement for joining a session.</param>
        /// <param name="tables">Array of ISmashTable for all SmashTable objects to be associated with the joined session.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void JoinSessionAsync(string admClientId, string admClientSecret, Dispatcher dispatcher, Guid meetingToken, string user, string email, string device, bool forceRejoin, ISmashTable[] tables, object state)
        {
            if (string.IsNullOrEmpty(admClientId))
            {
                throw new ArgumentNullException("admClientId");
            }

            if (string.IsNullOrEmpty(admClientSecret))
            {
                throw new ArgumentNullException("admClientSecret");
            }

            this.JoinSessionAsync(new AdmAuthClientIdentity(admClientId, admClientSecret, SmashClientREST.ServiceScope), dispatcher, meetingToken, user, email, device, forceRejoin, tables, state);
        }

        /// <summary>
        /// Creates a new smash session.
        /// </summary>
        /// <param name="admClientId">Azure Data Market client id used for authentication.</param>
        /// <param name="admClientSecret">Azure Data Market client secret used for authentication.</param>
        /// <param name="meetingToken">The meeting token of the session to create. Needs to be shared to allow others to join the session.</param>
        /// <param name="subject">The name of the session.</param>
        /// <param name="organizer">The organizer of the session.</param>
        /// <param name="organizerEmail">email address of organizer of session.</param>
        /// <param name="attendees">List of user names allowed to join a session. A wildcard '*' matches all users attempting to join.</param>
        /// <param name="lifetime">The lifetime of the session. Can be up to 30 days. The session will automatically be erased after expiration of this timespan.</param>
        /// <param name="managementID">The owner's management secret required to enumerate, modify, wipe sessions.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void CreateSessionAsync(string admClientId, string admClientSecret, Guid meetingToken, string subject, string organizer, string organizerEmail, IEnumerable<string> attendees, TimeSpan lifetime, Guid managementID, object state)
        {
            if (string.IsNullOrEmpty(admClientId))
            {
                throw new ArgumentNullException("admClientId");
            }

            if (string.IsNullOrEmpty(admClientSecret))
            {
                throw new ArgumentNullException("admClientSecret");
            }

            this.CreateSessionAsync(new AdmAuthClientIdentity(admClientId, admClientSecret, SmashClientREST.ServiceScope), meetingToken, subject, organizer, organizerEmail, attendees, lifetime, managementID, state);
        }

        /// <summary>
        /// Enumerates all sessions created with a management secret.
        /// </summary>
        /// <param name="admClientId">Azure Data Market client id used for authentication.</param>
        /// <param name="admClientSecret">Azure Data Market client secret used for authentication.</param>
        /// <param name="managementID">The owner's management secret required to enumerate, modify, wipe sessions.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void EnumSessionsAsync(string admClientId, string admClientSecret, Guid managementID, object state)
        {
            if (string.IsNullOrEmpty(admClientId))
            {
                throw new ArgumentNullException("admClientId");
            }

            if (string.IsNullOrEmpty(admClientSecret))
            {
                throw new ArgumentNullException("admClientSecret");
            }

            this.EnumSessionsAsync(new AdmAuthClientIdentity(admClientId, admClientSecret, SmashClientREST.ServiceScope), managementID, state);
        }

        /// <summary>
        /// Gets info for a specific Smash session.
        /// </summary>
        /// <param name="admClientId">Azure Data Market client id used for authentication.</param>
        /// <param name="admClientSecret">Azure Data Market client secret used for authentication.</param>
        /// <param name="meetingToken">The meeting token of the session to get info for.</param>
        /// <param name="sessionID">The session ID of the session to get info for.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void GetSessionInfoAsync(string admClientId, string admClientSecret, Guid meetingToken, Guid sessionID, object state)
        {
            if (string.IsNullOrEmpty(admClientId))
            {
                throw new ArgumentNullException("admClientId");
            }

            if (string.IsNullOrEmpty(admClientSecret))
            {
                throw new ArgumentNullException("admClientSecret");
            }

            this.GetSessionInfoAsync(new AdmAuthClientIdentity(admClientId, admClientSecret, SmashClientREST.ServiceScope), meetingToken, sessionID, state);
        }

        /// <summary>
        /// Wipes a Smash session.
        /// </summary>
        /// <param name="admClientId">Azure Data Market client id used for authentication.</param>
        /// <param name="admClientSecret">Azure Data Market client secret used for authentication.</param>
        /// <param name="meetingToken">The meeting token of the session to wipe.</param>
        /// <param name="sessionID">The session ID of the session to wipe.</param>
        /// <param name="managementID">The owner's management secret required to enumerate, modify, wipe sessions.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void WipeSessionAsync(string admClientId, string admClientSecret, Guid meetingToken, Guid sessionID, Guid managementID, object state)
        {
            if (string.IsNullOrEmpty(admClientId))
            {
                throw new ArgumentNullException("admClientId");
            }

            if (string.IsNullOrEmpty(admClientSecret))
            {
                throw new ArgumentNullException("admClientSecret");
            }

            this.WipeSessionAsync(new AdmAuthClientIdentity(admClientId, admClientSecret, SmashClientREST.ServiceScope), meetingToken, sessionID, managementID, state);
        }

        /// <summary>
        /// Creates a new smash session.
        /// </summary>
        /// <param name="identity">ClientIdentity object used for authentication.</param>
        /// <param name="meetingToken">The meeting token of the session to create. Needs to be shared to allow others to join the session.</param>
        /// <param name="subject">The name of the session.</param>
        /// <param name="organizer">The organizer of the session.</param>
        /// <param name="organizerEmail">email address of organizer of session.</param>
        /// <param name="attendees">List of user names allowed to join a session. A wildcard '*' matches all users attempting to join.</param>
        /// <param name="lifetime">The lifetime of the session. Can be up to 30 days. The session will automatically be erased after expiration of this timespan.</param>
        /// <param name="managementID">The owner's management secret required to enumerate, modify, wipe sessions.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void CreateSessionAsync(ClientIdentity identity, Guid meetingToken, string subject, string organizer, string organizerEmail, IEnumerable<string> attendees, TimeSpan lifetime, Guid managementID, object state)
        {
            IAsyncResult asyncResult = SmashClientREST.CreateSessionAsync(
                identity,
                meetingToken,
                subject,
                organizer,
                organizerEmail,
                attendees,
                lifetime,
                managementID,
                new ServiceAgent<Contracts.CreateSessionResponse>.OnCompleteDelegate(
                    (response) =>
                    {
                        CreateSessionCompletedArgs e = new CreateSessionCompletedArgs(response.Exception, response.Aborted, response.StateObject);
                        if (response.Exception == null && !response.Aborted)
                        {
                            e.MeetingToken = meetingToken;
                            e.SessionID = response.SessionID;
                        }
                        OnCreateSessionCompleted(e);
                    }),
                state);

            SmashClientREST.HandleCompletion(asyncResult, state);
        }

        /// <summary>
        /// Enumerates all sessions created with a management secret.
        /// </summary>
        /// <param name="identity">ClientIdentity object used for authentication.</param>
        /// <param name="managementID">The owner's management secret required to enumerate, modify, wipe sessions.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void EnumSessionsAsync(ClientIdentity identity, Guid managementID, object state)
        {
            IAsyncResult asyncResult = SmashClientREST.EnumSessionsAsync(
                identity,
                managementID,
                new ServiceAgent<Contracts.EnumSessionsResponse>.OnCompleteDelegate(
                    (response) =>
                    {
                        EnumSessionCompletedArgs e = new EnumSessionCompletedArgs(response.Exception, response.Aborted, response.StateObject);
                        if (response.Exception == null && !response.Aborted)
                        {
                            e.SessionInfos = response.Sessions;
                        }
                        OnEnumSessionsCompleted(e);
                    }),
                state);

            SmashClientREST.HandleCompletion(asyncResult, state);
        }

        /// <summary>
        /// Modifies a Smash session's attendee list.
        /// </summary>
        /// <param name="session">Smash session object.</param>
        /// <param name="managementID">The owner's management secret required to enumerate, modify, wipe sessions.</param>
        /// <param name="attendeesAdd">List of user names to add.</param>
        /// <param name="attendeesRemove">List of user names to delete.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void ModifySessionAsync(SmashSession session, Guid managementID, IEnumerable<string> attendeesAdd, IEnumerable<string> attendeesRemove, object state)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            IAsyncResult asyncResult = SmashClientREST.ModifySessionAsync(
                session.ClientIdentity,
                session.MeetingToken,
                session.SessionID,
                managementID,
                attendeesAdd,
                attendeesRemove,
                new ServiceAgent<Contracts.ModifySessionResponse>.OnCompleteDelegate(
                    (response) =>
                    {
                        ModifySessionCompletedArgs e = new ModifySessionCompletedArgs(response.Exception, response.Aborted, response.StateObject);
                        OnModifySessionCompleted(e);
                    }),
                state);

            SmashClientREST.HandleCompletion(asyncResult, state);
        }

        /// <summary>
        /// Creates a blob associated with a session.
        /// </summary>
        /// <param name="session">A Smash session object.</param>
        /// <param name="fileExtension">File extension for the blob.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void CreateBlobAsync(SmashSession session, string fileExtension, object state)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            IAsyncResult asyncResult = SmashClientREST.CreateBlobAsync(
                session.ClientIdentity,
                session.MeetingToken,
                session.SessionID,
                session.ClientID,
                fileExtension,
                new ServiceAgent<Contracts.CreateBlobResponse>.OnCompleteDelegate(
                    (response) =>
                    {
                        CreateBlobCompletedArgs e = new CreateBlobCompletedArgs(response.Exception, response.Aborted, response.StateObject);
                        if (response.Exception == null && !response.Aborted)
                        {
                            SmashBlobUploader blobUploader = new SmashBlobUploader(session.ClientIdentity, session.MeetingToken, session.SessionID, session.ClientID, response.BlobAddress, response.BlobSharedSignature);

                            e.BlobUploader = blobUploader;
                        }
                        OnCreateBlobCompleted(e);
                    }),
                state);

            SmashClientREST.HandleCompletion(asyncResult, state);
        }

        /// <summary>
        /// Joins a Smash session. User+email+device must be unique across all joinees of a session.
        /// </summary>
        /// <param name="identity">ClientIdentity object used for authentication.</param>
        /// <param name="dispatcher">The Dispatcher object. a dispatcher must be specified in order to use data binding with Smash.</param>
        /// <param name="meetingToken">The meeting token of the session to join.</param>
        /// <param name="user">User name joining the session.</param>
        /// <param name="email">email address of the user joining the session.</param>
        /// <param name="device">Device name of the user joining the session.</param>
        /// <param name="tables">Array of ISmashTable for all SmashTable objects to be associated with the joined session.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void JoinSessionAsync(ClientIdentity identity, Dispatcher dispatcher, Guid meetingToken, string user, string email, string device, ISmashTable[] tables, object state)
        {
            JoinSessionAsync(identity, dispatcher, meetingToken, user, email, device, false, tables, state);
        }

        /// <summary>
        /// Joins a Smash session. User+email+device must be unique across all joinees of a session.
        /// </summary>
        /// <param name="identity">ClientIdentity object used for authentication.</param>
        /// <param name="dispatcher">The Dispatcher object. a dispatcher must be specified in order to use data binding with Smash.</param>
        /// <param name="meetingToken">The meeting token of the session to join.</param>
        /// <param name="user">User name joining the session.</param>
        /// <param name="email">email address of the user joining the session.</param>
        /// <param name="device">Device name of the user joining the session.</param>
        /// <param name="forceRejoin">A value of 'true' overrides user+email+device uniqueness requirement for joining a session.</param>
        /// <param name="tables">Array of ISmashTable for all SmashTable objects to be associated with the joined session.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void JoinSessionAsync(ClientIdentity identity, Dispatcher dispatcher, Guid meetingToken, string user, string email, string device, bool forceRejoin, ISmashTable[] tables, object state)
        {
            if (tables == null)
            {
                throw new ArgumentNullException("tables");
            }

            if (tables.Length == 0)
            {
                throw new ArgumentException("tables.Length==0");
            }

            Dictionary<int, ISmashTable> uniquenessCheck = new Dictionary<int, ISmashTable>();
            foreach (var table in tables)
            {
                int hash = table.TypeHash;

                if (uniquenessCheck.ContainsKey(hash))
                {
                    throw new ArgumentException("tables: Two tables with identical types and names provided");
                }

                uniquenessCheck.Add(hash,table);
            }

            IAsyncResult asyncResult = SmashClientREST.JoinSessionAsync(
                identity,
                meetingToken,
                user,
                email,
                device,
                forceRejoin,
                new ServiceAgent<Contracts.JoinSessionResponse>.OnCompleteDelegate(
                    (response) =>
                    {
                        JoinSessionCompletedArgs e = new JoinSessionCompletedArgs(response.Exception, response.Aborted, response.StateObject);
                        if (response.Exception == null && !response.Aborted)
                        {
                            SmashSession session = new SmashSession(identity, dispatcher, meetingToken, response.SessionID, response.ClientID, tables);

                            e.Session = session;
                        }
                        OnJoinSessionCompleted(e);
                    }),
                state);

            SmashClientREST.HandleCompletion(asyncResult, state);
        }

        /// <summary>
        /// Gets info for a specific Smash session.
        /// </summary>
        /// <param name="identity">ClientIdentity object used for authentication.</param>
        /// <param name="meetingToken">The meeting token of the session to get info for.</param>
        /// <param name="sessionID">The session ID of the session to get info for.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void GetSessionInfoAsync(ClientIdentity identity, Guid meetingToken, Guid sessionID, object state)
        {
            IAsyncResult asyncResult = SmashClientREST.GetSessionInfoAsync(
                identity,
                meetingToken,
                sessionID,
                new ServiceAgent<Contracts.GetSessionInfoResponse>.OnCompleteDelegate(
                    (response) =>
                    {
                        GetSessionInfoCompletedArgs e = new GetSessionInfoCompletedArgs(response.Exception, response.Aborted, response.StateObject);
                        if (response.Exception == null && !response.Aborted)
                        {
                            e.SessionInfo = response.Session;
                        }
                        OnGetSessionInfoCompleted(e);
                    }),
                state);

            SmashClientREST.HandleCompletion(asyncResult, state);
        }

        /// <summary>
        /// Wipes (erases) all contents of a Smash session and prevents any further operation on the session.
        /// </summary>
        /// <param name="identity">ClientIdentity object used for authentication.</param>
        /// <param name="meetingToken">The meeting token of the session to join.</param>
        /// <param name="sessionID">The session ID of the session to be wiped.</param>
        /// <param name="managementID">The owner's management secret required to enumerate, modify, wipe sessions.</param>
        /// <param name="state">State to be passed as userState in the completion event args.</param>
        public void WipeSessionAsync(ClientIdentity identity, Guid meetingToken, Guid sessionID, Guid managementID, object state)
        {
            IAsyncResult asyncResult = SmashClientREST.WipeSessionAsync(
                identity,
                meetingToken,
                sessionID,
                managementID,
                new ServiceAgent<Contracts.WipeSessionResponse>.OnCompleteDelegate(
                    (response) =>
                    {
                        WipeSessionCompletedArgs e = new WipeSessionCompletedArgs(response.Exception, response.Aborted, response.StateObject);
                        OnWipeSessionCompleted(e);
                    }),
                state);

            SmashClientREST.HandleCompletion(asyncResult, state);
        }

        /// <summary>
        /// Event raised upon completion of JoinSessionAsync.
        /// </summary>
        /// <param name="e"></param>
        private void OnJoinSessionCompleted(JoinSessionCompletedArgs e)
        {
            if (this.JoinSessionCompleted != null)
            {
                this.JoinSessionCompleted(this, e);
            }
        }

        /// <summary>
        /// Event raised upon completion of CreateBobAsync.
        /// </summary>
        /// <param name="e"></param>
        private void OnCreateBlobCompleted(CreateBlobCompletedArgs e)
        {
            if (this.CreateBlobCompleted != null)
            {
                this.CreateBlobCompleted(this, e);
            }
        }

        /// <summary>
        /// Event raised upon completion of CreateSessionAsync.
        /// </summary>
        /// <param name="e"></param>
        private void OnCreateSessionCompleted(CreateSessionCompletedArgs e)
        {
            if (this.CreateSessionCompleted != null)
            {
                this.CreateSessionCompleted(this, e);
            }
        }

        /// <summary>
        /// Event raised upon completion of ModifySessionAsync.
        /// </summary>
        /// <param name="e"></param>
        private void OnModifySessionCompleted(ModifySessionCompletedArgs e)
        {
            if (this.ModifySessionCompleted != null)
            {
                this.ModifySessionCompleted(this, e);
            }
        }

        /// <summary>
        /// Event raised upon completion of EnumSessionsAsync.
        /// </summary>
        /// <param name="e"></param>
        private void OnEnumSessionsCompleted(EnumSessionCompletedArgs e)
        {
            if (this.EnumSessionsCompleted != null)
            {
                this.EnumSessionsCompleted(this, e);
            }
        }

        /// <summary>
        /// Event raised upon completion of GetSessionInfoAsync.
        /// </summary>
        /// <param name="e"></param>
        private void OnGetSessionInfoCompleted(GetSessionInfoCompletedArgs e)
        {
            if (this.GetSessionInfoCompleted != null)
            {
                this.GetSessionInfoCompleted(this, e);
            }
        }

        /// <summary>
        /// Event raised upon completion of WipeSessionAsync.
        /// </summary>
        /// <param name="e"></param>
        private void OnWipeSessionCompleted(WipeSessionCompletedArgs e)
        {
            if (this.WipeSessionCompleted != null)
            {
                this.WipeSessionCompleted(this, e);
            }
        }
    }
}