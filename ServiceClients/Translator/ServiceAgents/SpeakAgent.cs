// ----------------------------------------------------------
// <copyright file="SpeakAgent.cs" company="Microsoft">
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
    /// Class agent to convert text to speech.
    /// </summary>
    internal class SpeakAgent : ServiceAgent<SpeakResult>
    {
        /// <summary>
        /// The text to be translated.
        /// </summary>
        private string text;

        /// <summary>
        /// The language of the speech.
        /// </summary>
        private string language;

        /// <summary>
        /// The stream format of the content type. Currently "audio/wav" and "audio/mp3" are available. The default value is "audio/wav".
        /// </summary>
        private string format;

        /// <summary>
        /// Specifies the quality of the audio signals. Currently "MaxQuality" and "MinSize" are available. The default value is "MinSize".
        /// </summary>
        private string options;

        /// <summary>
        /// Initializes a new instance of the SpeakAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="identity">Specifies client identity.</param>
        /// <param name="text">The text to be converted to speech.</param>
        /// <param name="language">The language of the speech.</param>
        /// <param name="format">The stream format of the content type. Currently "audio/wav" and "audio/mp3" are available. The default value is "audio/wav".</param>
        /// <param name="options">Specifies the quality of the audio signals. Currently "MaxQuality" and "MinSize" are available. The default value is "MinSize".</param>
        public SpeakAgent(string hostName, ClientIdentity identity, string text, string language = "", string format = "audio/wav", string options = "MinSize") :
            this(hostName, identity, text, null, language, format, options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SpeakAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="identity">Specifies client identity.</param>
        /// <param name="text">The text to be converted to speech.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        /// <param name="language">The language of the speech.</param>
        /// <param name="format">The stream format of the content type. Currently "audio/wav" and "audio/mp3" are available. The default value is "audio/wav".</param>
        /// <param name="options">Specifies the quality of the audio signals. Currently "MaxQuality" and "MinSize" are available. The default value is "MinSize".</param>
        public SpeakAgent(string hostName, ClientIdentity identity, string text, object stateObject, string language = "", string format = "audio/wav", string options = "MinSize") :
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

            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }

            if (string.IsNullOrEmpty(options))
            {
                throw new ArgumentNullException("options");
            }

            this.text = text;
            this.language = language;
            this.format = format;
            this.options = options;

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
            this.Result.Audio = DeserializeResponseJson<SpeakResult>(responseStream).Audio;
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
            string signature = string.Format("{0}/{1}", TranslatorService.TranslatorSignature, TranslatorService.SpeakKey);
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(signature));

            uri.AddQueryString(TranslatorService.TextKey, Uri.EscapeDataString(this.text));
            uri.AddQueryString(TranslatorService.SpeakLanguageKey, Uri.EscapeDataString(this.language));
            uri.AddQueryString(TranslatorService.SpeakFormatKey, Uri.EscapeDataString(this.format));
            uri.AddQueryString(TranslatorService.SpeakOptionsKey, Uri.EscapeDataString(this.options));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
