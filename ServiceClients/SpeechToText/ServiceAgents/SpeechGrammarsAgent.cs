// -
// <copyright file="SpeechGrammarsAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Speech.Client
{
    using System;
    using System.Diagnostics;
    using System.IO;
    
    /// <summary>
    /// This class provides helper methods used to communicate with the Hawaii Speech-to-Text service.
    /// It has helper methods for contacting Hawaii Speech-to-Text Service to receive all available grammars in the server. 
    /// Currently, only the 'Dictation' grammar is available for a general purpose speech-to-text translation.
    /// </summary>
    internal partial class SpeechGrammarsAgent : ServiceAgent<SpeechServiceResult>
    {
        /// <summary>
        /// Initializes a new instance of the SpeechGrammarsAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the speech service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        public SpeechGrammarsAgent(string hostName, ClientIdentity clientIdentity)
            : this(hostName, clientIdentity, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SpeechGrammarsAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the speech service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public SpeechGrammarsAgent(string hostName, ClientIdentity clientIdentity, object stateObject)
            : base(HttpMethod.Get, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

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
        /// An overridden abstract method. This method is called after the response sent by the server is received by the client.
        /// speech service.
        /// </summary>
        /// <param name="responseStream">
        /// The response stream containing response data that is received from the server.
        /// </param>
        protected override void ParseOutput(Stream responseStream)
        {
            if (responseStream == null)
            {
                throw new ArgumentException("Response stream is null");
            }

            Debug.Assert(this.Result != null, "result is null");
            this.Result.SpeechResult = DeserializeResponseJson<SpeechResult>(responseStream);
        }

        /// <summary>
        /// It creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <returns>A valid service uri object.</returns>
        private Uri CreateServiceUri(string hostName)
        {
            // Create the service Uri.
            ServiceUri uri = new ServiceUri(hostName, SpeechService.Signature);

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
