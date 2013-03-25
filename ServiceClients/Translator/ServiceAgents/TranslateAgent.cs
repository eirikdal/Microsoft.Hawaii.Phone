// ----------------------------------------------------------
// <copyright file="TranslateAgent.cs" company="Microsoft">
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
    /// Class agent to translate the text.
    /// </summary>
    internal class TranslateAgent : ServiceAgent<TranslateResult>
    {
        /// <summary>
        /// The text to be translated.
        /// </summary>
        private string text;

        /// <summary>
        /// The language translate from.
        /// </summary>
        private string from;

        /// <summary>
        /// The language translate to.
        /// </summary>
        private string to;

        /// <summary>
        /// Initializes a new instance of the TranslateAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="identity">Specifies client identity.</param>
        /// <param name="text">The text to be translated.</param>
        /// <param name="to">The language translate to.</param>
        /// <param name="from">The language translate from.</param>
        public TranslateAgent(string hostName, ClientIdentity identity, string text, string to, string from = "") :
            this(hostName, identity, text, to, null, from)
        {
        }

        /// <summary>
        /// Initializes a new instance of the TranslateAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="identity">Specifies client identity.</param>
        /// <param name="text">The text to be translated.</param>
        /// <param name="to">The language translate to.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="from">The language translate from.</param>
        public TranslateAgent(string hostName, ClientIdentity identity, string text, string to, object stateObject, string from = "") :
            base(HttpMethod.Get, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException("to");
            }

            this.text = text;
            this.from = from;
            this.to = to;

            // Set the client identity.
            this.ClientIdentity = identity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName);
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
            this.Result.TextTranslated = DeserializeResponseJson<TranslateResult>(responseStream).TextTranslated;
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
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName)
        {
            string signature = string.Format("{0}/{1}", TranslatorService.TranslatorSignature, TranslatorService.TranslateKey);
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(signature));

            uri.AddQueryString(TranslatorService.TextKey, Uri.EscapeDataString(this.text));
            uri.AddQueryString(TranslatorService.FromKey, Uri.EscapeDataString(this.from));
            uri.AddQueryString(TranslatorService.ToKey, Uri.EscapeDataString(this.to));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
