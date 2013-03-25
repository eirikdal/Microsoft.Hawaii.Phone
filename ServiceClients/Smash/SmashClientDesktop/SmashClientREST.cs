// ----------------------------------------------------------
// <copyright file="SmashClientREST.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Threading;
    using System.Windows.Threading;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Contracts;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class SmashClientREST
    {
        /// <summary>
        /// 
        /// </summary>
        private const int DefaultTimeout = 30000;

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultHostName = "http://api.hawaii-services.net/SMASH/V1/SmashService.svc";

        /// <summary>
        /// 
        /// </summary>
        public const string CreateSessionSignature = "CreateSession";

        /// <summary>
        /// 
        /// </summary>
        public const string AddPageToBlobSignature = "AddPageToBlob";

        /// <summary>
        /// 
        /// </summary>
        public const string CreateBlobSignature = "CreateBlob";

        /// <summary>
        /// 
        /// </summary>
        public const string EnumSessionsSignature = "EnumSessions";

        /// <summary>
        /// 
        /// </summary>
        public const string GetRowsSignature = "GetRows";

        /// <summary>
        /// 
        /// </summary>
        public const string GetSessionInfoSignature = "GetSessionInfo";

        /// <summary>
        /// 
        /// </summary>
        public const string JoinSessionSignature = "JoinSession";

        /// <summary>
        /// 
        /// </summary>
        public const string ModifySessionSignature = "ModifySession";

        /// <summary>
        /// 
        /// </summary>
        public const string SendRowsSignature = "SendRows";

        /// <summary>
        /// 
        /// </summary>
        public const string WipeSessionSignature = "WipeSession";

        /// <summary>
        /// 
        /// </summary>
        private static readonly string stagingConfigFileName = @"C:\AzureStagingDeploymentConfig\SmashStagingHostConfig.ini";

        /// <summary>
        /// The name of the config file that indicates what is the translater service scope.  Used only as a test hook.
        /// </summary>
        private static readonly string stagingServiceScopeConfigFileName = @"C:\AzureStagingDeploymentConfig\HawaiiServiceScopeConfig.ini";

        /// <summary>
        /// 
        /// </summary>
        private static string hostName;

        /// <summary>
        /// The service scope.  This is the private variable that is initialized on first
        /// access via the GetServiceScope() method.  If a config file is present to point to a
        /// staging server, that host will be stored.  Otherwise, the default is used.
        /// </summary>
        private static string serviceScope;

        /// <summary>
        /// 
        /// </summary>
        public static string HostName
        {
            get
            {
                return SmashClientREST.GetHostName();
            }

            set
            {
                SmashClientREST.hostName = value;
            }
        }

        /// <summary>
        /// Gets the service scope to be used when accessing the adm OAuth service.
        /// </summary>
        internal static string ServiceScope
        {
            get
            {
                return SmashClientREST.GetServiceScope();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionName"></param>
        /// <param name="ownerName"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="attendees"></param>
        /// <param name="lifetime"></param>
        /// <param name="managementID"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IAsyncResult CreateSessionAsync(
            ClientIdentity identity,
            Guid meetingToken,
            string sessionName,
            string ownerName,
            string ownerEmail,
            IEnumerable<string> attendees,
            TimeSpan lifetime,
            Guid managementID,
            ServiceAgent<CreateSessionResponse>.OnCompleteDelegate onComplete,
            object state)
        {
            CreateSessionAgent agent = new CreateSessionAgent(
                SmashClientREST.HostName,
                identity,
                meetingToken,
                sessionName,
                ownerName,
                ownerEmail,
                attendees,
                lifetime,
                managementID,
                state);

            return agent.AbortableProcessRequest(onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="clientID"></param>
        /// <param name="blobAddress"></param>
        /// <param name="blobSharedSignature"></param>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <param name="close"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IAsyncResult AddPageToBlobAsync(
            ClientIdentity identity,
            Guid meetingToken,
            Guid sessionID,
            Guid clientID,
            string blobAddress,
            string blobSharedSignature,
            byte[] page,
            int id,
            bool close,
            ServiceAgent<AddPageToBlobResponse>.OnCompleteDelegate onComplete,
            object state)
        {
            AddPageToBlobAgent agent = new AddPageToBlobAgent(
                SmashClientREST.HostName,
                identity,
                meetingToken,
                sessionID,
                clientID,
                blobAddress,
                blobSharedSignature,
                page,
                id,
                close,
                state);

            return agent.AbortableProcessRequest(onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="clientID"></param>
        /// <param name="fileExtension"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IAsyncResult CreateBlobAsync(
            ClientIdentity identity,
            Guid meetingToken,
            Guid sessionID,
            Guid clientID,
            string fileExtension,
            ServiceAgent<CreateBlobResponse>.OnCompleteDelegate onComplete,
            object state)
        {
            CreateBlobAgent agent = new CreateBlobAgent(
                SmashClientREST.HostName,
                identity,
                meetingToken,
                sessionID,
                clientID,
                fileExtension,
                state);

            return agent.AbortableProcessRequest(onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="managementID"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IAsyncResult EnumSessionsAsync(
            ClientIdentity identity,
            Guid managementID,
            ServiceAgent<EnumSessionsResponse>.OnCompleteDelegate onComplete,
            object state)
        {
            EnumSessionsAgent agent = new EnumSessionsAgent(
                SmashClientREST.HostName,
                identity,
                managementID,
                state);

            return agent.AbortableProcessRequest(onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="clientID"></param>
        /// <param name="lastKnownRowTime"></param>
        /// <param name="timeOut"></param>
        /// <param name="cached"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IAsyncResult GetRowsAsync(
            ClientIdentity identity,
            Guid meetingToken,
            Guid sessionID,
            Guid clientID,
            long lastKnownRowTime,
            int timeOut,
            bool cached,
            ServiceAgent<GetRowsResponse>.OnCompleteDelegate onComplete,
            object state)
        {
            GetRowsAgent agent = new GetRowsAgent(
                SmashClientREST.HostName,
                identity,
                meetingToken,
                sessionID,
                clientID,
                lastKnownRowTime,
                timeOut,
                cached,
                state);

            return agent.AbortableProcessRequest(onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IAsyncResult GetSessionInfoAsync(
            ClientIdentity identity,
            Guid meetingToken,
            Guid sessionID,
            ServiceAgent<GetSessionInfoResponse>.OnCompleteDelegate onComplete,
            object state)
        {
            GetSessionInfoAgent agent = new GetSessionInfoAgent(
                SmashClientREST.HostName,
                identity,
                meetingToken,
                sessionID,
                state);

            return agent.AbortableProcessRequest(onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="userName"></param>
        /// <param name="userEmail"></param>
        /// <param name="deviceName"></param>
        /// <param name="forceRejoin"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IAsyncResult JoinSessionAsync(
            ClientIdentity identity,
            Guid meetingToken,
            string userName,
            string userEmail,
            string deviceName,
            bool forceRejoin,
            ServiceAgent<JoinSessionResponse>.OnCompleteDelegate onComplete,
            object state)
        {
            JoinSessionAgent agent = new JoinSessionAgent(
                SmashClientREST.HostName,
                identity,
                meetingToken,
                userName,
                userEmail,
                deviceName,
                forceRejoin,
                state);

            return agent.AbortableProcessRequest(onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="managementID"></param>
        /// <param name="attendeesAdd"></param>
        /// <param name="attendeesRemove"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IAsyncResult ModifySessionAsync(
            ClientIdentity identity,
            Guid meetingToken,
            Guid sessionID,
            Guid managementID,
            IEnumerable<string> attendeesAdd,
            IEnumerable<string> attendeesRemove,
            ServiceAgent<ModifySessionResponse>.OnCompleteDelegate onComplete,
            object state)
        {
            if (attendeesAdd == null)
            {
                attendeesAdd = new List<string>();
            }
            if (attendeesRemove == null)
            {
                attendeesRemove = new List<string>();
            }

            ModifySessionAgent agent = new ModifySessionAgent(
                SmashClientREST.HostName,
                identity,
                meetingToken,
                sessionID,
                managementID,
                attendeesAdd,
                attendeesRemove,
                state);

            return agent.AbortableProcessRequest(onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="clientID"></param>
        /// <param name="rows"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IAsyncResult SendRowsAsync(
            ClientIdentity identity,
            Guid meetingToken,
            Guid sessionID,
            Guid clientID,
            IEnumerable<DataRow_Wire> rows,
            ServiceAgent<SendRowsResponse>.OnCompleteDelegate onComplete,
            object state)
        {
            SendRowsAgent agent = new SendRowsAgent(
                SmashClientREST.HostName,
                identity,
                meetingToken,
                sessionID,
                clientID,
                rows,
                state);

            return agent.AbortableProcessRequest(onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="managementID"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IAsyncResult WipeSessionAsync(
            ClientIdentity identity,
            Guid meetingToken,
            Guid sessionID,
            Guid managementID,
            ServiceAgent<WipeSessionResponse>.OnCompleteDelegate onComplete,
            object state)
        {
            WipeSessionAgent agent = new WipeSessionAgent(
                SmashClientREST.HostName,
                identity,
                meetingToken,
                sessionID,
                managementID,
                state);

            return agent.AbortableProcessRequest(onComplete, state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <param name="state"></param>
        public static void HandleCompletion(IAsyncResult asyncResult, object state)
        {
            ThreadPool.RegisterWaitForSingleObject(
                asyncResult.AsyncWaitHandle,
                new WaitOrTimerCallback(
                    (state_, timedOut) =>
                    {
                        if (timedOut)
                        {
                            SmashClientREST.AbortRequest(asyncResult, true);
                        }
                    }),
                    state,
                    DefaultTimeout,
                    true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="timedOut"></param>
        public static void AbortRequest(IAsyncResult result, bool timedOut)
        {
            (result as IAbortableAsyncResult).Abort(timedOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SecuritySafeCritical]
        private static string GetHostName()
        {
            if (string.IsNullOrEmpty(SmashClientREST.hostName))
            {
                SmashClientREST.hostName = ClientLibraryUtils.LookupHostNameFromConfig(stagingConfigFileName, SmashClientREST.DefaultHostName);
            }

            return SmashClientREST.hostName;
        }

        /// <summary>
        /// Returns the service scope to be used when accessing the adm OAuth service. This will generally
        /// be the value generated by the DefaultServiceScope, but it can conditionally be set with
        /// the presence of a config file on first access.
        /// </summary>
        /// <returns>A string containing the service scope.</returns>
        [SecuritySafeCritical]
        private static string GetServiceScope()
        {
            if (string.IsNullOrEmpty(SmashClientREST.serviceScope))
            {
                string defaultServiceScope = AdmAuthClientIdentity.GetServiceScope(SmashClientREST.HostName);
                SmashClientREST.serviceScope = ClientLibraryUtils.LookupServiceScopeFromConfig(stagingServiceScopeConfigFileName, defaultServiceScope);
            }

            return SmashClientREST.serviceScope;
        }
    }
}