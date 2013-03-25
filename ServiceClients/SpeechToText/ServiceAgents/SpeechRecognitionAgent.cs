// -
// <copyright file="SpeechRecognitionAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Speech.Client
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// This class provides helper methods used to communicate with the Hawaii Speech-to-Text service.
    /// It has helper methods for contacting Hawaii Speech-to-Text Service to execute the speech-to-text translation.
    /// It accepts an audio stream as input, sends it to the speech service and receives a list of potential texts corresponding to the audio input.
    /// </summary>
    internal partial class SpeechRecognitionAgent : ServiceAgent<SpeechServiceResult>
    {
        /// <summary>
        /// Initializes a new instance of the SpeechRecognitionAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the speech service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="grammar">Specifies a grammar name loaded in the server.</param>
        /// <param name="speechBuffer">
        /// Specifies a buffer containing the audio data to be translated to text.
        /// The audio buffer should have the following characteristics:
        /// 'SamplesPerSecond=16000', 'AudioBitsPerSample=16' and 'AudioChannel=Mono'.
        /// </param>
        public SpeechRecognitionAgent(
                                    string hostName,
                                    ClientIdentity clientIdentity,
                                    string grammar,
                                    byte[] speechBuffer) :
            this(hostName, clientIdentity, grammar, speechBuffer, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SpeechRecognitionAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the speech service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="grammar">Specifies a grammar name loaded in the server.</param>
        /// <param name="speechBuffer">
        /// Specifies a buffer containing the audio data to be translated to text.
        /// The audio buffer should have the following characteristics:
        /// 'SamplesPerSecond=16000', 'AudioBitsPerSample=16' and 'AudioChannel=Mono'.
        /// </param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public SpeechRecognitionAgent(
                                    string hostName,
                                    ClientIdentity clientIdentity,
                                    string grammar,
                                    byte[] speechBuffer,
                                    object stateObject) :
            base(HttpMethod.Post, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (string.IsNullOrEmpty(grammar))
            {
                throw new ArgumentNullException("grammar");
            }

            if (speechBuffer == null || speechBuffer.Length == 0)
            {
                throw new ArgumentNullException("speechBuffer");
            }

            // Set the speech buffer to recognize.
            this.SpeechBuffer = speechBuffer;

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, grammar);
        }

        /// <summary>
        /// Gets the speech buffer to be translated to text. 
        /// The audio buffer should have the following characteristics:
        /// 'SamplesPerSecond=16000', 'AudioBitsPerSample=16' and 'AudioChannel=Mono'.
        /// </summary>
        public byte[] SpeechBuffer { get; private set; }

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
        /// An overridden abstract method. It provides the POST (as in Http POST) data that has to be sent to the service.
        /// This method will be called by the base class when it needs data during a Http POST method call.
        /// </summary>
        /// <returns>
        /// The POST data as an array of bytes.
        /// </returns>
        protected override byte[] GetPostData()
        {
            // This service client is for Speech, so the method returns the image buffer. 
            return this.SpeechBuffer;
        }

        /// <summary>
        /// An overridden abstract method. This method is called after the response sent by the server is received by the client.
        /// </summary>
        /// <param name="responseStream">
        /// The response stream containing response data that is received from the server.
        /// </param>
        protected override void ParseOutput(Stream responseStream)
        {
            if (responseStream == null)
            {
                return;
            }

            Debug.Assert(this.Result != null, "result is null");
            this.Result.SpeechResult = DeserializeResponseJson<SpeechResult>(responseStream);
        }

        /// <summary>
        /// An overriden method. 
        /// It is invoked after completing the service request. It does some processing of the Speech-to-Text service call result 
        /// and it calls the client's "on complete" callback method.
        /// </summary>
        protected override void OnCompleteRequest()
        {
            // Call all client event handling methods.
            if (this.OnComplete != null)
            {
                // Create the event argument object based on success/failure.
                if (this.Result.Exception == null)
                {
                    SpeechServiceResult speechResult = this.Result as SpeechServiceResult;
                    Debug.Assert(speechResult != null, "speechResult is null");

                    // A successful recognition.
                    if (speechResult.SpeechResult.InternalErrorMessage != null)
                    {
                        this.Result.Status = Status.InternalServerError;
                        this.Result.Exception = new Exception("A server side error occured while processing the speech request.");
                    }
                }

                // Call the UI's on completion method.
                this.OnComplete(this.Result);
            }
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="grammar">Specifies a grammar name.</param>
        /// <returns>A valid service uri object.</returns>
        private Uri CreateServiceUri(string hostName, string grammar)
        {
            // Create the service Uri.
            string signature = string.Format("{0}/{1}", SpeechService.Signature, grammar);
            ServiceUri uri = new ServiceUri(hostName, signature);

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
