// ----------------------------------------------------------
// <copyright file="GetLanguagesForTranslateAgent.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Translator.Client
{
    using System;
    using System.Diagnostics;
    using System.Net;

    using Microsoft.Hawaii;

    /// <summary>
    /// Class agent to get supported languages for Translate method.
    /// </summary>
    internal class GetLanguagesForTranslateAgent : ServiceAgent<GetLanguagesForTranslateResult>
    {
        /// <summary>
        /// Initializes a new instance of the GetLanguagesForTranslateAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="identity">Specifies client identity.</param>
        /// <param name="locale">The local system locale</param>
        public GetLanguagesForTranslateAgent(string hostName, ClientIdentity identity, string locale = "en") :
            this(hostName, identity, null, locale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GetLanguagesForTranslateAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="identity">Specifies client identity.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="locale">The local system locale</param>
        public GetLanguagesForTranslateAgent(string hostName, ClientIdentity identity, object stateObject, string locale = "en") :
            base(HttpMethod.Get, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (string.IsNullOrEmpty(locale))
            {
                throw new ArgumentNullException("locale");
            }

            // Set the client identity.
            this.ClientIdentity = identity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, locale);
        }

        /// <summary>
        /// An overriden property to set the request content type to be Json.
        /// </summary>
        protected override string RequestContentType
        {
            get
            {
                return "application/json";
            }
        }

        /// <summary>
        /// An overriden method to parse the result from the service.
        /// </summary>
        /// <param name="responseStream">
        /// A valid response stream.
        /// </param>
        protected override void ParseOutput(System.IO.Stream responseStream)
        {
            if (responseStream == null)
            {
                throw new ArgumentException("Response stream is null");
            }

            Debug.Assert(this.Result != null, "result is null");
            this.Result.SupportedLanguages = DeserializeResponseJson<GetLanguagesForTranslateResult>(responseStream).SupportedLanguages;
            this.Result.Status = Status.Success;
        }

        /// <summary>
        /// An overriden method to get the data for POST service call.
        /// </summary>
        /// <returns>Return the byte array of the Post data.</returns>
        protected override byte[] GetPostData()
        {
            return null;
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="locale">The system locale.</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName, string locale)
        {
            string signature = string.Format(
                "{0}/{1}?{2}={3}", 
                TranslatorService.TranslatorSignature, 
                TranslatorService.GetLanguagesForTranslateKey, 
                TranslatorService.LocaleKey,
                Uri.EscapeDataString(locale));
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeDataString(signature));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
