// -
// <copyright file="SpeechService.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Speech.Client
{
    using System;
    using System.Security;

    /// <summary>
    /// Helper class that provides access to the Speech-to-Text service.
    /// </summary>
    public static class SpeechService
    {
        /// <summary>
        /// Specifies the default service host name. This will be used to create the service url.
        /// </summary>
        public const string DefaultHostName = @"http://api.hawaii-services.net/Stt/V1";

        /// <summary>
        /// Name of the default speech grammar.
        /// </summary>
        public const string DefaultGrammar = "Dictation";

        /// <summary>
        /// Specifies a signature for the REST methods that are part of the speech-to-text service.
        /// </summary>
        public const string Signature = "SpeechRecognition";

        /// <summary>
        /// The name of the config file that indicates where the SpeechToText staging service is located.  Used only as a test hook.
        /// </summary>
        private static readonly string stagingConfigFileName = @"C:\AzureStagingDeploymentConfig\SpeechToTextStagingHostConfig.ini";

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
                return SpeechService.GetHostName();
            }
        }

        /// <summary>
        /// Gets the service scope to be used when accessing the adm OAuth service.
        /// </summary>
        private static string ServiceScope
        {
            get
            {
                if (string.IsNullOrEmpty(SpeechService.serviceScope))
                {
                    SpeechService.serviceScope = AdmAuthClientIdentity.GetServiceScope(SpeechService.HostName);
                }

                return SpeechService.serviceScope;
            }
        }

        /// <summary>
        /// Helper method to initiate the service call that retrieves all grammars available on the server.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate handler.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void GetGrammarsAsync(
            string hawaiiAppId,
            ServiceAgent<SpeechServiceResult>.OnCompleteDelegate onComplete, 
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            GetGrammarsAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the service call that executes the speech-to-text translation.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="grammar">Specifies a grammar name.</param>
        /// <param name="speechBuffer">
        /// Specifies a buffer containing the audio data to be translated to text.
        /// The audio buffer should have the following characteristics:
        /// 'SamplesPerSecond=16000', 'AudioBitsPerSample=16' and 'AudioChannel=Mono'.
        /// </param>
        /// <param name="onComplete">Specifies an "on complete" delegate handler.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public static void RecognizeSpeechAsync(
            string hawaiiAppId, 
            string grammar, 
            byte[] speechBuffer,
            ServiceAgent<SpeechServiceResult>.OnCompleteDelegate onComplete, 
            object stateObject = null)
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            RecognizeSpeechAsync(
                new GuidAuthClientIdentity(hawaiiAppId), 
                grammar, 
                speechBuffer, 
                onComplete,
                stateObject);
        }

        /// <summary>
        /// Helper method to initiate the service call that retrieves all grammars available on the server.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate handler.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void GetGrammarsAsync(
            ClientIdentity clientIdentity,
            ServiceAgent<SpeechServiceResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            SpeechGrammarsAgent client = new SpeechGrammarsAgent(
                SpeechService.HostName,
                clientIdentity,
                stateObject);

            client.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the service call that executes the speech-to-text translation.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="grammar">Specifies a grammar name.</param>
        /// <param name="speechBuffer">
        /// Specifies a buffer containing the audio data to be translated to text.
        /// The audio buffer should have the following characteristics:
        /// 'SamplesPerSecond=16000', 'AudioBitsPerSample=16' and 'AudioChannel=Mono'.
        /// </param>
        /// <param name="onComplete">Specifies an "on complete" delegate handler.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        private static void RecognizeSpeechAsync(
            ClientIdentity clientIdentity,
            string grammar,
            byte[] speechBuffer,
            ServiceAgent<SpeechServiceResult>.OnCompleteDelegate onComplete,
            object stateObject = null)
        {
            SpeechRecognitionAgent client = new SpeechRecognitionAgent(
                SpeechService.HostName,
                clientIdentity,
                grammar,
                speechBuffer,
                stateObject);

            client.ProcessRequest(onComplete);
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
            if (string.IsNullOrEmpty(SpeechService.hostName))
            {
                SpeechService.hostName = ClientLibraryUtils.LookupHostNameFromConfig(stagingConfigFileName, SpeechService.DefaultHostName);
            }

            return SpeechService.hostName;
        }
    }
}
