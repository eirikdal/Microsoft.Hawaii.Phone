// -
// <copyright file="RelayService.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System;
    using System.IO;
    using System.Security;
    
    /// <summary>
    /// Helper class that provides access to the Relay service.
    /// </summary>
    public static class RelayService
    {
        /// <summary>
        /// Specifies the default service host name. This will be used to create the service url.
        /// </summary>
        public const string DefaultHostName = @"http://api.hawaii-services.net/Relay/V1";

        /// <summary>
        /// Specifies a signature for the REST methods that manage endpoints.
        /// </summary>
        public const string EndPointSignature = "Endpoint";

        /// <summary>
        /// Specifies a signature for the REST methods that manage groups.
        /// </summary>
        public const string GroupSignature = "Group";

        /// <summary>
        /// Query string key for name.
        /// </summary>
        public const string NameKey = "name";

        /// <summary>
        /// Query string key for timeout.
        /// </summary>
        public const string TtlKey = "ttl";

        /// <summary>
        /// Query string key for to.
        /// </summary>
        public const string ToKey = "to";

        /// <summary>
        /// Query string key for filter.
        /// </summary>
        public const string FilterKey = "filter";

        /// <summary>
        /// Query string key for wait.
        /// </summary>
        public const string WaitKey = "wait";

        /// <summary>
        /// The name of the config file that indicates where the Relay staging service is located.  Used only as a test hook.
        /// </summary>
        private static readonly string StagingConfigFileName = @"C:\AzureStagingDeploymentConfig\RelayStagingHostConfig.ini";

        /// <summary>
        /// The service host name.  This is the private variable that is initialized on first
        /// access via the ServiceScope get accessor.  If a config file is present to point to a
        /// staging server, that host will be stored.  Otherwise, the default is used.
        /// </summary>
        private static string hostName;

        /// <summary>
        /// The service scope.  This is the private variable that is initialized on first
        /// access via the GetServiceScope() method.  If a config file is present to point to a
        /// staging server, that host will be stored.  Otherwise, the default is used.
        /// </summary>
        private static string serviceScope;

        /// <summary>
        /// Gets the Host Name to be used when accessing the service.
        /// </summary>
        public static string HostName
        {
            get
            {
                return RelayService.GetHostName();
            }
        }

        /// <summary>
        /// Gets the service scope to be used when accessing the adm OAuth service.
        /// </summary>
        private static string ServiceScope
        {
            get
            {
                if (string.IsNullOrEmpty(RelayService.serviceScope))
                {
                    RelayService.serviceScope = AdmAuthClientIdentity.GetServiceScope(RelayService.HostName);
                }

                return RelayService.serviceScope;
            }
        }

        /// <summary>
        /// Helper method to initiate the call that creates an endpoint.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="name">Specifies a name of the client.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void CreateEndPointAsync(
            string hawaiiAppId,
            string name,
            ServiceAgent<EndpointResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            CreateEndPointAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                name, 
                onComplete,
                TimeSpan.MaxValue);
        }

        /// <summary>
        /// Helper method to initiate the call that creates an endpoint.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="name">Specifies a name of the client.</param>
        /// <param name="ttl">Specifies the time to live on the server</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void CreateEndPointAsync(
            string hawaiiAppId,
            string name, 
            TimeSpan ttl,
            ServiceAgent<EndpointResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            CreateEndPointAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                name,
                ttl,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes an endpoint.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="endpoint">Specifies an endpoint.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void DeleteEndPointAsync(
            string hawaiiAppId,
            Endpoint endpoint,
            ServiceAgent<EndpointResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            DeleteEndPointAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                endpoint, 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that will join an endpoint to a group.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="endpoint">Specifies an endpoint to join a group.</param>
        /// <param name="group">Specifies a target group.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void JoinGroupAsync(
            string hawaiiAppId,
            Endpoint endpoint,
            Group group,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            JoinGroupAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                endpoint, 
                group, 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that will have an endpoint leave from a group.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="endpoint">Specifies an endpoint to leave a group.</param>
        /// <param name="group">Specifies a target group.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void LeaveGroupAsync(
            string hawaiiAppId,
            Endpoint endpoint,
            Group group,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            LeaveGroupAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                endpoint, 
                group, 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that will send a message.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="fromEndPoint">Specifies an from end point.</param>
        /// <param name="recipientIds">Specifies the recipients (either endpoint or group) ids.</param>
        /// <param name="message">Specifies an message data to be sent.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void SendMessageAsync(
            string hawaiiAppId,
            Endpoint fromEndPoint,
            string recipientIds,
            byte[] message,
            ServiceAgent<MessagingResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            SendMessageAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                fromEndPoint,
                recipientIds,
                message,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that will receive a message.
        /// </summary>
        /// <param name="hawaiiAppId">
        /// Specifies the Hawaii Application Id.
        /// </param>
        /// <param name="endpoint">
        /// Specifies an endpoint to leave a group.
        /// </param>
        /// <param name="filter">
        /// Specifies a list of registration ids for Endpoints and/or Groups that
        /// identify senders and/or group recipients of desired messages.
        /// </param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void ReceiveMessagesAsync(
            string hawaiiAppId,
            Endpoint endpoint,
            string filter,
            ServiceAgent<MessagingResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            ReceiveMessagesAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                endpoint, 
                filter,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that creates a new group.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void CreateGroupAsync(
            string hawaiiAppId,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            CreateGroupAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that creates a new group.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="ttl">Specifies the time to live in the service</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void CreateGroupAsync(
            string hawaiiAppId, 
            TimeSpan ttl,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            CreateGroupAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                ttl,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes a group.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="group">Specifies a group to be deleted.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void DeleteGroupAsync(
            string hawaiiAppId,
            Group group,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            DeleteGroupAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                group, 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that creates an endpoint.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="name">Specifies a name of the client.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void CreateEndPointAsync(
            string clientId,
            string clientSecret,
            string name,
            ServiceAgent<EndpointResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            CreateEndPointAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RelayService.ServiceScope),
                name,
                onComplete,
                TimeSpan.MaxValue);
        }

        /// <summary>
        /// Helper method to initiate the call that creates an endpoint.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="name">Specifies a name of the client.</param>
        /// <param name="ttl">Specifies the time to live on the server</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void CreateEndPointAsync(
            string clientId,
            string clientSecret,
            string name,
            TimeSpan ttl,
            ServiceAgent<EndpointResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            CreateEndPointAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RelayService.ServiceScope),
                name,
                ttl,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes an endpoint.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="endpoint">Specifies an endpoint.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void DeleteEndPointAsync(
            string clientId,
            string clientSecret,
            Endpoint endpoint,
            ServiceAgent<EndpointResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            DeleteEndPointAsync(
               new AdmAuthClientIdentity(clientId, clientSecret, RelayService.ServiceScope),
                endpoint,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that will join an endpoint to a group.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="endpoint">Specifies an endpoint to join a group.</param>
        /// <param name="group">Specifies a target group.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void JoinGroupAsync(
            string clientId,
            string clientSecret,
            Endpoint endpoint,
            Group group,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            JoinGroupAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RelayService.ServiceScope),
                endpoint,
                group,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that will have an endpoint leave from a group.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="endpoint">Specifies an endpoint to leave a group.</param>
        /// <param name="group">Specifies a target group.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void LeaveGroupAsync(
            string clientId,
            string clientSecret,
            Endpoint endpoint,
            Group group,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            LeaveGroupAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RelayService.ServiceScope),
                endpoint,
                group,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that will send a message.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="fromEndPoint">Specifies an from end point.</param>
        /// <param name="recipientIds">Specifies the recipients (either endpoint or group) ids.</param>
        /// <param name="message">Specifies an message data to be sent.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void SendMessageAsync(
            string clientId,
            string clientSecret,
            Endpoint fromEndPoint,
            string recipientIds,
            byte[] message,
            ServiceAgent<MessagingResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            SendMessageAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RelayService.ServiceScope),
                fromEndPoint,
                recipientIds,
                message,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that will receive a message.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.
        /// Specifies the Hawaii Application Id.
        /// </param>
        /// <param name="endpoint">
        /// Specifies an endpoint to leave a group.
        /// </param>
        /// <param name="filter">
        /// Specifies a list of registration ids for Endpoints and/or Groups that
        /// identify senders and/or group recipients of desired messages.
        /// </param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void ReceiveMessagesAsync(
            string clientId,
            string clientSecret,
            Endpoint endpoint,
            string filter,
            ServiceAgent<MessagingResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            ReceiveMessagesAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RelayService.ServiceScope),
                endpoint,
                filter,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that creates a new group.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void CreateGroupAsync(
            string clientId,
            string clientSecret,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            CreateGroupAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RelayService.ServiceScope),
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that creates a new group.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="ttl">Specifies the time to live in the service</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void CreateGroupAsync(
            string clientId,
            string clientSecret,
            TimeSpan ttl,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            CreateGroupAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RelayService.ServiceScope),
                ttl,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes a group.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="group">Specifies a group to be deleted.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void DeleteGroupAsync(
            string clientId,
            string clientSecret,
            Group group,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            DeleteGroupAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RelayService.ServiceScope),
                group,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that creates an endpoint.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="name">Specifies a name of the client.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void CreateEndPointAsync(
            ClientIdentity clientIdentity,
            string name,
            ServiceAgent<EndpointResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            CreateEndPointAgent agent = new CreateEndPointAgent(
                RelayService.HostName,
                clientIdentity,
                name,
                TimeSpan.MaxValue);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that creates an endpoint.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="name">Specifies a name of the client.</param>
        /// <param name="ttl">Specifies the time to live on the server</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void CreateEndPointAsync(
            ClientIdentity clientIdentity,
            string name,
            TimeSpan ttl,
            ServiceAgent<EndpointResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            CreateEndPointAgent agent = new CreateEndPointAgent(
                RelayService.HostName,
                clientIdentity,
                name,
                ttl,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes an endpoint.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="endpoint">Specifies an endpoint.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void DeleteEndPointAsync(
            ClientIdentity clientIdentity,
            Endpoint endpoint,
            ServiceAgent<EndpointResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            DeleteEndPointAgent agent = new DeleteEndPointAgent(
                RelayService.HostName,
                clientIdentity,
                endpoint,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that will join an endpoint to a group.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="endpoint">Specifies an endpoint to join a group.</param>
        /// <param name="group">Specifies a target group.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void JoinGroupAsync(
            ClientIdentity clientIdentity,
            Endpoint endpoint,
            Group group,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            JoinGroupAgent agent = new JoinGroupAgent(
                RelayService.HostName,
                clientIdentity,
                endpoint,
                group,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that will have an endpoint leave from a group.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="endpoint">Specifies an endpoint to leave a group.</param>
        /// <param name="group">Specifies a target group.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void LeaveGroupAsync(
            ClientIdentity clientIdentity,
            Endpoint endpoint,
            Group group,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            LeaveGroupAgent agent = new LeaveGroupAgent(
                RelayService.HostName,
                clientIdentity,
                endpoint,
                group,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that will send a message.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="fromEndPoint">Specifies an from end point.</param>
        /// <param name="recipientIds">Specifies the recipients (either endpoint or group) ids.</param>
        /// <param name="message">Specifies an message data to be sent.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void SendMessageAsync(
            ClientIdentity clientIdentity,
            Endpoint fromEndPoint,
            string recipientIds,
            byte[] message,
            ServiceAgent<MessagingResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            SendMessageAgent agent = new SendMessageAgent(
                RelayService.HostName,
                clientIdentity,
                fromEndPoint,
                recipientIds,
                message,
                TimeSpan.MaxValue,
                stateObject);
            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that will receive a message.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="endpoint">Specifies an endpoint to leave a group.</param>
        /// <param name="filter">
        /// Specifies a list of registration ids for Endpoints and/or Groups that
        /// identify senders and/or group recipients of desired messages.
        /// </param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void ReceiveMessagesAsync(
            ClientIdentity clientIdentity,
            Endpoint endpoint,
            string filter,
            ServiceAgent<MessagingResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            ReceiveMessagesAgent agent = new ReceiveMessagesAgent(
                RelayService.HostName,
                clientIdentity,
                endpoint,
                filter,
                TimeSpan.Zero, // Default to return immediately
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that creates a new group.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void CreateGroupAsync(
            ClientIdentity clientIdentity,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            CreateGroupAgent agent = new CreateGroupAgent(
                RelayService.HostName,
                clientIdentity,
                TimeSpan.MaxValue,
                stateObject);
            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that creates a new group.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="ttl">Specifies the time to live in the service</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void CreateGroupAsync(
            ClientIdentity clientIdentity,
            TimeSpan ttl,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            CreateGroupAgent agent = new CreateGroupAgent(
                RelayService.HostName,
                clientIdentity,
                ttl,
                stateObject);
            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes a group.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="group">Specifies a group to be deleted.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void DeleteGroupAsync(
            ClientIdentity clientIdentity,
            Group group,
            ServiceAgent<GroupResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            DeleteGroupAgent agent = new DeleteGroupAgent(
                RelayService.HostName,
                clientIdentity,
                group,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Returns the Host Name to be used when accessing the service.  This will generally
        /// be the value specified in the DefaultHostName, but it can conditionally be set with
        /// the presence of a config file on first access.
        /// </summary>
        /// <returns>A string containing the host name of the service</returns>
        [SecuritySafeCritical]
        private static string GetHostName()
        {
            if (string.IsNullOrEmpty(RelayService.hostName))
            {
                RelayService.hostName = ClientLibraryUtils.LookupHostNameFromConfig(StagingConfigFileName, RelayService.DefaultHostName);
            }

            return RelayService.hostName;
        }
    }
}
