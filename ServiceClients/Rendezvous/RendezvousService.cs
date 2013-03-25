// -
// <copyright file="RendezvousService.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Rendezvous.Client
{
    using System;
    using System.IO;
    using System.Security;
    
    /// <summary>
    /// Helper class that provides access to the Rendezvous service.
    /// </summary>
    public static class RendezvousService
    {
        /// <summary>
        /// Specifies the default service host name. This will be used to create the service url.
        /// </summary>
        public const string DefaultHostName = @"http://api.hawaii-services.net/Rendezvous/V1";

        /// <summary>
        /// Specifies a signature for the REST methods.
        /// </summary>
        public const string NameSignature = "Name";

        /// <summary>
        /// The name of the config file that indicates where the Rendezvous staging service is located.  Used only as a test hook.
        /// </summary>
        private static readonly string stagingConfigFileName = @"C:\AzureStagingDeploymentConfig\RendezvousStagingHostConfig.ini";

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
                return RendezvousService.GetHostName();
            }
        }

        /// <summary>
        /// Gets the service scope to be used when accessing the adm OAuth service.
        /// </summary>
        private static string ServiceScope
        {
            get
            {
                if (string.IsNullOrEmpty(RendezvousService.serviceScope))
                {
                    RendezvousService.serviceScope = AdmAuthClientIdentity.GetServiceScope(RendezvousService.HostName);
                }

                return RendezvousService.serviceScope;
            }
        }

        /// <summary>
        /// Helper method to find a registration id for a name.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="name">Specifies a group name.</param>
        /// <param name="onComplete">Specifiesa an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object which will be provided in the call to the "on complete" calback.</param>
        public static void LookupNameAsync(
            string hawaiiAppId, 
            string name,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete, 
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            LookupNameAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                name, 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to register a Name with the service.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="name">Specifies a name to register.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        public static void RegisterNameAsync(
            string hawaiiAppId, 
            string name,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete, 
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            RegisterNameAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                name, 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to unregister (delete) a Name with the service.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        public static void UnregisterNameAsync(
            string hawaiiAppId, 
            NameRegistration registration,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete, 
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            UnregisterNameAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                registration, 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to asociate a registeration id to a name with the service.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        public static void AssociateIdAsync(
            string hawaiiAppId, 
            NameRegistration registration,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete, 
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            AssociateIdAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                registration, 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to disasociate a registeration id to a name with the service.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        public static void DisassociateIdAsync(
            string hawaiiAppId, 
            NameRegistration registration,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete, 
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            DisassociateIdAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                registration, 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to find a registration id for a name.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="name">Specifies a group name.</param>
        /// <param name="onComplete">Specifiesa an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object which will be provided in the call to the "on complete" calback.</param>
        public static void LookupNameAsync(
            string clientId,
            string clientSecret,
            string name,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete,
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

            LookupNameAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RendezvousService.ServiceScope),
                name,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to register a Name with the service.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="name">Specifies a name to register.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        public static void RegisterNameAsync(
            string clientId,
            string clientSecret,
            string name,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete,
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

            RegisterNameAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RendezvousService.ServiceScope),
                name,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to unregister (delete) a Name with the service.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        public static void UnregisterNameAsync(
            string clientId,
            string clientSecret,
            NameRegistration registration,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete,
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

            UnregisterNameAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RendezvousService.ServiceScope),
                registration,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to asociate a registeration id to a name with the service.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        public static void AssociateIdAsync(
            string clientId,
            string clientSecret,
            NameRegistration registration,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete,
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

            AssociateIdAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RendezvousService.ServiceScope),
                registration,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to disasociate a registeration id to a name with the service.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        public static void DisassociateIdAsync(
            string clientId,
            string clientSecret,
            NameRegistration registration,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete,
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

            DisassociateIdAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, RendezvousService.ServiceScope),
                registration,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to find a registration id for a name.
        /// </summary>
        /// <param name="identity">The hawaii client identity.</param>
        /// <param name="name">Specifies a group name.</param>
        /// <param name="onComplete">Specifiesa an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object which will be provided in the call to the "on complete" calback.</param>
        private static void LookupNameAsync(
            ClientIdentity identity,
            string name,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            LookupNameAgent agent = new LookupNameAgent(
                RendezvousService.HostName,
                identity,
                name,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to register a Name with the service.
        /// </summary>
        /// <param name="identity">The hawaii client identity.</param>
        /// <param name="name">Specifies a name to register.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        private static void RegisterNameAsync(
            ClientIdentity identity,
            string name,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            RegisterNameAgent agent = new RegisterNameAgent(
                RendezvousService.HostName,
                identity,
                name,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to unregister (delete) a Name with the service.
        /// </summary>
        /// <param name="identity">The hawaii client identity.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        private static void UnregisterNameAsync(
            ClientIdentity identity,
            NameRegistration registration,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            UnregisterNameAgent agent = new UnregisterNameAgent(
                RendezvousService.HostName,
                identity,
                registration,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to asociate a registeration id to a name with the service.
        /// </summary>
        /// <param name="identity">The hawaii client identity.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        private static void AssociateIdAsync(
            ClientIdentity identity,
            NameRegistration registration,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            AssociateIdAgent agent = new AssociateIdAgent(
                RendezvousService.HostName,
                identity,
                registration,
                stateObject);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to disasociate a registeration id to a name with the service.
        /// </summary>
        /// <param name="identity">The hawaii client identity.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        private static void DisassociateIdAsync(
            ClientIdentity identity,
            NameRegistration registration,
            ServiceAgent<NameRegistrationResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            DisassociateIdAgent agent = new DisassociateIdAgent(
                RendezvousService.HostName,
                identity,
                registration,
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
            if (string.IsNullOrEmpty(RendezvousService.hostName))
            {
                RendezvousService.hostName = ClientLibraryUtils.LookupHostNameFromConfig(stagingConfigFileName, RendezvousService.DefaultHostName);
            }

            return RendezvousService.hostName;
        }
    }
}
