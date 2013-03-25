// ----------------------------------------------------------
// <copyright file="TranslatorService.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Translator.Client
{
    using System;
    using System.Security;
    using Microsoft.Hawaii;

    /// <summary>
    /// Helper class that provides access to the Translator service.
    /// </summary>
    public class TranslatorService
    {
        /// <summary>
        /// Specifies the default service host name. This will be used to create the service url.
        /// </summary>
        internal const string DefaultHostName = "http://api.hawaii-services.net/Translator/v1";

        /// <summary>
        /// Specifies a signature for the REST methods of translator service.
        /// </summary>
        internal const string TranslatorSignature = "translator";

        /// <summary>
        /// Specifies a query string key of GetLanguagesForTranslate.
        /// </summary>
        internal const string GetLanguagesForTranslateKey = "GetLanguagesForTranslate";

        /// <summary>
        /// Specifies a query string key of GetLanguagesForSpeak.
        /// </summary>
        internal const string GetLanguagesForSpeakKey = "GetLanguagesForSpeak";

        /// <summary>
        /// Specifies a query string key of Translate.
        /// </summary>
        internal const string TranslateKey = "Translate";

        /// <summary>
        /// Specifies a query string key of text.
        /// </summary>
        internal const string TextKey = "text";

        /// <summary>
        /// Specifies a query string key of the language to translate from.
        /// </summary>
        internal const string FromKey = "languageFrom";

        /// <summary>
        /// Specifies a query string key of the language to translate to.
        /// </summary>
        internal const string ToKey = "languageTo";

        /// <summary>
        /// Specifies a query string key of Speak.
        /// </summary>
        internal const string SpeakKey = "speak";

        /// <summary>
        /// Specifies a query string key of speak language.
        /// </summary>
        internal const string SpeakLanguageKey = "language";

        /// <summary>
        /// Specifies a query string key of speak format.
        /// </summary>
        internal const string SpeakFormatKey = "format";

        /// <summary>
        /// Specifies a query string key of speak options.
        /// </summary>
        internal const string SpeakOptionsKey = "options";

        /// <summary>
        /// Specifies a query string key of locale.
        /// </summary>
        internal const string LocaleKey = "locale";

        /// <summary>
        /// The name of the config file that indicates where the translater staging service is located.  Used only as a test hook.
        /// </summary>
        private static readonly string StagingConfigFileName = @"C:\AzureStagingDeploymentConfig\TranslatorStagingHostConfig.ini";

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
        private static string HostName
        {
            get
            {
                return TranslatorService.GetHostName();
            }
        }

        /// <summary>
        /// Gets the service scope to be used when accessing the adm OAuth service.
        /// </summary>
        private static string ServiceScope
        {
            get
            {
                if (string.IsNullOrEmpty(TranslatorService.serviceScope))
                {
                    TranslatorService.serviceScope = AdmAuthClientIdentity.GetServiceScope(TranslatorService.HostName);
                }

                return TranslatorService.serviceScope;
            }
        }

        /// <summary>
        /// Helper method to initiate the call that gets the supported languages for translate method.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="locale">The system locale</param>
        public static void GetLanguagesForTranslateAsync(
            string hawaiiAppId,
            ServiceAgent<GetLanguagesForTranslateResult>.OnCompleteDelegate onComplete,
            object stateObject,
            string locale = "en")
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            GetLanguagesForTranslateAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                onComplete,
                stateObject,
                locale);
        }

        /// <summary>
        /// Helper method to initiate the call that gets the supported languages for translate method.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="locale">The system locale</param>
        public static void GetLanguagesForTranslateAsync(
            string clientId,
            string clientSecret,
            ServiceAgent<GetLanguagesForTranslateResult>.OnCompleteDelegate onComplete,
            object stateObject,
            string locale = "en")
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            GetLanguagesForTranslateAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, TranslatorService.ServiceScope),
                onComplete,
                stateObject,
                locale);
        }

        /// <summary>
        /// Helper method to initiate the call that gets the supported languages for speak method.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="locale">The system locale</param>
        public static void GetLanguagesForSpeakAsync(
            string hawaiiAppId,
            ServiceAgent<GetLanguagesForSpeakResult>.OnCompleteDelegate onComplete,
            object stateObject,
            string locale = "en")
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            GetLanguagesForSpeakAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                onComplete,
                stateObject,
                locale);
        }

        /// <summary>
        /// Helper method to initiate the call that gets the supported languages for speak method.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="locale">The system locale</param>
        public static void GetLanguagesForSpeakAsync(
            string clientId,
            string clientSecret,
            ServiceAgent<GetLanguagesForSpeakResult>.OnCompleteDelegate onComplete,
            object stateObject,
            string locale = "en")
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            GetLanguagesForSpeakAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, TranslatorService.ServiceScope),
                onComplete,
                stateObject,
                locale);
        }

        /// <summary>
        /// Helper method to initiate the call that translate method.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="text">The text to be translated.</param>
        /// <param name="to">The language translate to.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="from">The language translate from.</param>        
        public static void TranslateAsync(
            string hawaiiAppId,
            string text,
            string to,
            ServiceAgent<TranslateResult>.OnCompleteDelegate onComplete,
            object stateObject,            
            string from = "")
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            TranslateAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                text,
                to,
                onComplete,
                stateObject,
                from);
        }

        /// <summary>
        /// Helper method to initiate the call that translate method.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="text">The text to be translated.</param>
        /// <param name="to">The language translate to.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="from">The language translate from.</param>        
        public static void TranslateAsync(
            string clientId,
            string clientSecret,
            string text,
            string to,
            ServiceAgent<TranslateResult>.OnCompleteDelegate onComplete,
            object stateObject,
            string from = "")
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            TranslateAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, TranslatorService.ServiceScope),
                text,
                to,
                onComplete,
                stateObject,
                from);
        }

        /// <summary>
        /// Helper method to initiate the call that speak method.
        /// </summary>
        /// <param name="hawaiiAppId">Specifies the Hawaii Application Id.</param>
        /// <param name="text">The text to be converted to speech.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="language">The language of the speech.</param>        
        /// <param name="format">The stream format of the content type. Currently "audio/wav" and "audio/mp3" are available. The default value is "audio/wav".</param>
        /// <param name="options">Specifies the quality of the audio signals. Currently "MaxQuality" and "MinSize" are available. The default value is "MinSize".</param>
        public static void SpeakAsync(
            string hawaiiAppId,
            string text,
            ServiceAgent<SpeakResult>.OnCompleteDelegate onComplete,
            object stateObject,
            string language = "",
            string format = "audio/wav", 
            string options = "MinSize")
        {
            if (string.IsNullOrEmpty(hawaiiAppId))
            {
                throw new ArgumentNullException("hawaiiAppId");
            }

            SpeakAsync(
                new GuidAuthClientIdentity(hawaiiAppId),
                text,
                onComplete,
                stateObject,              
                language,
                format,
                options);
        }

        /// <summary>
        /// Helper method to initiate the call that speak method.
        /// </summary>
        /// <param name="clientId">The adm client Id.</param>
        /// <param name="clientSecret">The adm client secret.</param>
        /// <param name="text">The text to be converted to speech.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="language">The language of the speech.</param>        
        /// <param name="format">The stream format of the content type. Currently "audio/wav" and "audio/mp3" are available. The default value is "audio/wav".</param>
        /// <param name="options">Specifies the quality of the audio signals. Currently "MaxQuality" and "MinSize" are available. The default value is "MinSize".</param>
        public static void SpeakAsync(
            string clientId,
            string clientSecret,
            string text,
            ServiceAgent<SpeakResult>.OnCompleteDelegate onComplete,
            object stateObject,
            string language = "",
            string format = "audio/wav",
            string options = "MinSize")
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }

            SpeakAsync(
                new AdmAuthClientIdentity(clientId, clientSecret, TranslatorService.ServiceScope),
                text,
                onComplete,
                stateObject,
                language,
                format,
                options);
        }

        /// <summary>
        /// Helper method to initiate the call that gets the supported languages for translate method.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="locale">The system locale</param>
        private static void GetLanguagesForTranslateAsync(
            ClientIdentity clientIdentity,
            ServiceAgent<GetLanguagesForTranslateResult>.OnCompleteDelegate onComplete,
            object stateObject,
            string locale = "en")
        {
            GetLanguagesForTranslateAgent agent = new GetLanguagesForTranslateAgent(
                TranslatorService.HostName,
                clientIdentity,
                stateObject,
                locale);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that gets the supported languages for speak method.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="locale">The system locale</param>       
        private static void GetLanguagesForSpeakAsync(
            ClientIdentity clientIdentity,
            ServiceAgent<GetLanguagesForSpeakResult>.OnCompleteDelegate onComplete,
            object stateObject,
            string locale = "en")
        {
            GetLanguagesForSpeakAgent agent = new GetLanguagesForSpeakAgent(
                TranslatorService.HostName,
                clientIdentity,
                stateObject,
                locale);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that translate method.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="text">The text to be translated.</param>
        /// <param name="to">The language translate to.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="from">The language translate from.</param>        
        private static void TranslateAsync(
           ClientIdentity clientIdentity,
           string text,
           string to,
           ServiceAgent<TranslateResult>.OnCompleteDelegate onComplete,
           object stateObject,
           string from = "")
        {
            TranslateAgent agent = new TranslateAgent(
                TranslatorService.HostName,
                clientIdentity,
                text,
                to,
                stateObject,
                from);

            agent.ProcessRequest(onComplete);
        }

        /// <summary>
        /// Helper method to initiate the call that speak method.
        /// </summary>
        /// <param name="clientIdentity">The hawaii client identity.</param>
        /// <param name="text">The text to be converted to speech.</param>
        /// <param name="onComplete">Specifies an "on complete" delegate callback.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="language">The language of the speech.</param>        
        /// <param name="format">The stream format of the content type. Currently "audio/wav" and "audio/mp3" are available. The default value is "audio/wav".</param>
        /// <param name="options">Specifies the quality of the audio signals. Currently "MaxQuality" and "MinSize" are available. The default value is "MinSize".</param>
        private static void SpeakAsync(
            ClientIdentity clientIdentity,
            string text,
            ServiceAgent<SpeakResult>.OnCompleteDelegate onComplete,
            object stateObject,
            string language = "",
            string format = "audio/wav",
            string options = "MinSize")
        {
            SpeakAgent agent = new SpeakAgent(
                TranslatorService.HostName,
                clientIdentity,
                text,
                stateObject,
                language,
                format,
                options);

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
            if (string.IsNullOrEmpty(TranslatorService.hostName))
            {
                TranslatorService.hostName = ClientLibraryUtils.LookupHostNameFromConfig(StagingConfigFileName, TranslatorService.DefaultHostName);
            }

            return TranslatorService.hostName;
        }
    }
}
