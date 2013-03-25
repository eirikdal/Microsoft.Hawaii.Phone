// -
// <copyright file="PathPredictionService.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.PathPrediction.Client
{
    using System;
    using Microsoft.Hawaii;

    /// <summary>
    /// Mobile client to the Path Prediction Web Service
    /// </summary>
    public static partial class PathPredictionService
    {
        /// <summary>
        /// Path to the service access point
        /// </summary>
        public const string DefaultServicePath = @"http://api.hawaii-services.net/PathPrediction/V1";

        /// <summary>
        /// The name of the config file that indicates where the Relay staging service is located.  Used only for testing
        /// </summary>
        private static readonly string stagingConfigFileName = @"C:\AzureStagingDeploymentConfig\PathPredictionStagingHostConfig.ini";

        /// <summary>
        /// path of the service
        /// </summary>
        private static string servicePath;

        /// <summary>
        /// The service scope.  This is the private variable that is initialized on first
        /// access via the ServiceScope get accessor.  If a config file is present to point to a
        /// staging server, that host will be stored.  Otherwise, the default is used.
        /// </summary>
        private static string serviceScope;

        /// <summary>
        /// Gets the service path
        /// </summary>
        public static string ServicePath
        {
            get
            {
                if (string.IsNullOrEmpty(servicePath))
                {
                    servicePath = ClientLibraryUtils.LookupHostNameFromConfig(stagingConfigFileName, PathPredictionService.DefaultServicePath);
                }

                return servicePath;
            }
        }

        /// <summary>
        /// Gets the service scope to be used when accessing the adm OAuth service.
        /// </summary>
        private static string ServiceScope
        {
            get
            {
                if (string.IsNullOrEmpty(PathPredictionService.serviceScope))
                {
                    PathPredictionService.serviceScope = AdmAuthClientIdentity.GetServiceScope(PathPredictionService.ServicePath);
                }

                return PathPredictionService.serviceScope;
            }
        }

        /// <summary>
        /// Calls the PredictLocation service method
        /// </summary>
        /// <param name="hawaiiAppId">login to use</param>
        /// <param name="request">request parameter to the method</param>
        /// <param name="onComplete">on complete callback</param>
        /// <param name="stateObject">Optional correlation user state object</param>
        public static void PredictLocationAsync(
            string hawaiiAppId,
            PredictLocationRequest request,
            ServiceAgent<PredictLocationResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            PredictLocationAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                request,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Calls the PredictLocation service method
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="request">request parameter to the method</param>
        /// <param name="onComplete">on complete callback</param>
        /// <param name="stateObject">Optional correlation user state object</param>
        public static void PredictLocationAsync(
            string clientId,
            string clientSecret,
            PredictLocationRequest request,
            ServiceAgent<PredictLocationResult>.OnCompleteDelegate onComplete,
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

            PredictLocationAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, PathPredictionService.ServiceScope),
                request,
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Calls the PredictLocation service method
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="request">request parameter to the method</param>
        /// <param name="onComplete">on complete callback</param>
        /// <param name="stateObject">Optional correlation user state object</param>
        private static void PredictLocationAsync(
            ClientIdentity clientIdentity,
            PredictLocationRequest request,
            ServiceAgent<PredictLocationResult>.OnCompleteDelegate onComplete,
            object stateObject)
        {
            PredictLocationAgent agent = new PredictLocationAgent(
                PathPredictionService.ServicePath,
                clientIdentity,
                request,
                stateObject);

            agent.ProcessRequest(onComplete);
        }
    }
}