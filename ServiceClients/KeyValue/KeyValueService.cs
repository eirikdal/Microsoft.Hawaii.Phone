// ----------------------------------------------------------
// <copyright file="KeyValueService.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System;
    using System.Security;

    /// <summary>
    /// Helper class that provides access to the KeyValue service.
    /// </summary>
    public class KeyValueService
    {
        /// <summary>
        /// Specifies the default service host name. This will be used to create the service url.
        /// </summary>
        public const string DefaultHostName = "http://api.hawaii-services.net/keyvalue/v1";

        /// <summary>
        /// Specifies a signature for the REST methods that manage KeyValue item.
        /// </summary>
        public const string KeyValueSignature = "values";

        /// <summary>
        /// Specifies a query string key of prefix.
        /// </summary>
        public const string PrefixKey = "prefix";

        /// <summary>
        /// Specifies a query string key of size.
        /// </summary>
        public const string SizeKey = "size";

        /// <summary>
        /// Specifies a query string key of continuationToken.
        /// </summary>
        public const string ContinuationTokenKey = "continuationToken";

        /// <summary>
        /// Specifies a query string key of batch operation.
        /// </summary>
        public const string GetBatchKey = "$batch";

        /// <summary>
        /// Specifies a query string key of timestamp before.
        /// </summary>
        public const string BeforeKey = "before";

        /// <summary>
        /// The name of the config file that indicates where the KVS staging service is located.  Used only as a test hook.
        /// </summary>
        private static readonly string StagingConfigFileName = @"C:\AzureStagingDeploymentConfig\KeyValueStagingHostConfig.ini";

        /// <summary>
        /// The service host name.  This is the private variable that is initialized on first
        /// access via the GetHostName() method.  If a config file is present to point to a
        /// staging server, that host will be stored.  Otherwise, the default is used.
        /// </summary>
        private static string hostName;

        /// <summary>
        /// The service scope.  This is the private variable that is initialized on first
        /// access via the ServiceScope get accessor.  If a config file is present to point to a
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
                return KeyValueService.GetHostName();
            }
        }

        /// <summary>
        /// Gets the service scope to be used when accessing the adm OAuth service.
        /// </summary>
        private static string ServiceScope
        {
            get
            {
                if (string.IsNullOrEmpty(KeyValueService.serviceScope))
                {
                    KeyValueService.serviceScope = AdmAuthClientIdentity.GetServiceScope(KeyValueService.HostName);
                }

                return KeyValueService.serviceScope;
            }
        }

        /// <summary>
        /// Helper method to initiate the call that gets KeyValue items by key.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="key">Specifies a key to search.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void GetByKeyAsync(
            string hawaiiAppId,
            string key,
            ServiceAgent<GetByKeyResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            GetByKeyAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                key,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that gets KeyValue items by key.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="key">Specifies a key to search.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void GetByKeyAsync(
            string clientId,
            string clientSecret,
            string key,
            ServiceAgent<GetByKeyResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            GetByKeyAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, KeyValueService.ServiceScope),
                key,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that gets KeyValue items by keys.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="keys">Specifies the keys to search.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void GetByKeysAsync(
            string hawaiiAppId,
            string[] keys,
            ServiceAgent<GetByKeysResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            GetByKeysAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                keys,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that gets KeyValue items by keys.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="keys">Specifies the keys to search.</param>        
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void GetByKeysAsync(
            string clientId,
            string clientSecret,
            string[] keys,
            ServiceAgent<GetByKeysResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            GetByKeysAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, KeyValueService.ServiceScope),
                keys,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that gets KeyValue items.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="size">Specifies the size of result.</param>
        /// <param name="continuationToken">Specifies the continuation token.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void GetAsync(
            string hawaiiAppId,
            string prefix, 
            int size, 
            string continuationToken,
            ServiceAgent<GetResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            GetAsync(
                new GuidAuthClientIdentity(hawaiiAppId),                
                prefix,
                size,
                continuationToken,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that gets KeyValue items.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="size">Specifies the size of result.</param>
        /// <param name="continuationToken">Specifies the continuation token.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void GetAsync(
            string clientId,
            string clientSecret,
            string prefix,
            int size,
            string continuationToken,
            ServiceAgent<GetResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            GetAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, KeyValueService.ServiceScope),
                prefix,
                size,
                continuationToken,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that creates KeyValue item.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="items">Specifies the KeyValue items to create.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void CreateAsync(
            string hawaiiAppId,
            KeyValueItem[] items,
            ServiceAgent<SetResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            CreateAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                items,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that creates KeyValue item.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="items">Specifies the KeyValue items to create.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void CreateAsync(
            string clientId,
            string clientSecret,
            KeyValueItem[] items,
            ServiceAgent<SetResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            CreateAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, KeyValueService.ServiceScope),
                items,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that sets KeyValue item.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="items">Specifies the KeyValue items to create.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void SetAsync(
            string hawaiiAppId,
            KeyValueItem[] items,
            ServiceAgent<SetResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            SetAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                items,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that sets KeyValue item.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="items">Specifies the KeyValue items to create.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void SetAsync(
            string clientId,
            string clientSecret,
            KeyValueItem[] items,
            ServiceAgent<SetResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            SetAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, KeyValueService.ServiceScope),
                items,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes KeyValue items.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="before">Specifies the timpstamp for delete operation.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void DeleteAsync(
            string hawaiiAppId,
            string prefix,
            DateTime before,
            ServiceAgent<DeleteResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            DeleteAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                prefix,
                before,
                onComplete,
                stateObject);            
        }

        /// <summary>
        /// Helper method to initiate the call that deletes KeyValue items.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="before">Specifies the timpstamp for delete operation.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void DeleteAsync(
            string clientId,
            string clientSecret,
            string prefix,
            DateTime before,
            ServiceAgent<DeleteResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            DeleteAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, KeyValueService.ServiceScope),
                prefix,
                before,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes KeyValue items by keys.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="keys">Specifies the keys to search.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void DeleteByKeysAsync(
            string hawaiiAppId,
            string[] keys,
            ServiceAgent<DeleteResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            DeleteByKeysAsync(
                new GuidAuthClientIdentity(hawaiiAppId),                
                keys,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes KeyValue items by keys.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="keys">Specifies the keys to search.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void DeleteByKeysAsync(
            string clientId,
            string clientSecret,
            string[] keys,
            ServiceAgent<DeleteResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            DeleteByKeysAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, KeyValueService.ServiceScope),
                keys,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the call that gets KeyValue items by key.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="key">Specifies a key to search.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void GetByKeyAsync(
            ClientIdentity clientIdentity,
            string key,
            ServiceAgent<GetByKeyResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            GetByKeyAgent agent = new GetByKeyAgent(
                KeyValueService.HostName,
                clientIdentity,
                key,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that gets KeyValue items by keys.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="keys">Specifies the keys to search.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void GetByKeysAsync(
            ClientIdentity clientIdentity,
            string[] keys,
            ServiceAgent<GetByKeysResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            GetByKeysAgent agent = new GetByKeysAgent(
                KeyValueService.HostName,
                clientIdentity,
                keys,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that gets KeyValue items.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="size">Specifies the size of result.</param>
        /// <param name="continuationToken">Specifies the continuation token.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void GetAsync(
            ClientIdentity clientIdentity,
            string prefix,
            int size,
            string continuationToken,
            ServiceAgent<GetResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            GetAgent agent = new GetAgent(
                KeyValueService.HostName,
                clientIdentity,
                prefix,
                size,
                continuationToken,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that creates KeyValue item.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="items">Specifies the KeyValue items to create.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void CreateAsync(
            ClientIdentity clientIdentity,
            KeyValueItem[] items,
            ServiceAgent<SetResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            CreateAgent agent = new CreateAgent(
                KeyValueService.HostName,
                clientIdentity,
                items,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that sets KeyValue item.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="items">Specifies the KeyValue items to create.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void SetAsync(
            ClientIdentity clientIdentity,
            KeyValueItem[] items,
            ServiceAgent<SetResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            SetAgent agent = new SetAgent(
                KeyValueService.HostName,
                clientIdentity,
                items,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes KeyValue items.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="before">Specifies the timpstamp for delete operation.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void DeleteAsync(
            ClientIdentity clientIdentity,
            string prefix,
            DateTime before,
            ServiceAgent<DeleteResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            DeleteAgent agent = new DeleteAgent(
                KeyValueService.HostName,
                clientIdentity,
                prefix,
                before,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that deletes KeyValue items by keys.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="keys">Specifies the keys to search.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void DeleteByKeysAsync(
            ClientIdentity clientIdentity,
            string[] keys,
            ServiceAgent<DeleteResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            DeleteByKeysAgent agent = new DeleteByKeysAgent(
                KeyValueService.HostName,
                clientIdentity,
                keys,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Returns the Host Name to be used when accessing the service. This will generally
        /// be the value specified in the DefaultHostName, but it can conditionally be set with
        /// the presence of a config file on first access.
        /// </summary>
        /// <returns>A string containing the host name of the service</returns>
        [SecuritySafeCritical]
        private static string GetHostName()
        {
            if (string.IsNullOrEmpty(KeyValueService.hostName))
            {
                KeyValueService.hostName = ClientLibraryUtils.LookupHostNameFromConfig(StagingConfigFileName, KeyValueService.DefaultHostName);
            }

            return KeyValueService.hostName;
        }
    }
}
