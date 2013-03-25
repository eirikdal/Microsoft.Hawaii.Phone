// -
// <copyright file="PredictLocationAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.PathPrediction.Client
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.Hawaii;

    /// <summary>
    /// Class agent to create call PredictLocation from Path PredictionService
    /// </summary>
    internal class PredictLocationAgent : ServiceAgent<PredictLocationResult>
    {
        /// <summary>
        /// Initializes a new instance of the PredictLocationAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>        
        /// <param name="request">Request to the Path Prediction Service.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public PredictLocationAgent(string hostName, ClientIdentity clientIdentity, PredictLocationRequest request, object stateObject) :
            base(HttpMethod.Post, stateObject)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Path == null)
            {
                throw new ArgumentNullException("request.Path");
            }

            this.Request = request;

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = new Uri(new ServiceUri(hostName, "PredictLocation").ToString());
        }

        /// <summary>
        /// Gets location prediction request
        /// </summary>
        public PredictLocationRequest Request { get; private set; }

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
        /// Converts the request into bytes to be posted
        /// </summary>
        /// <returns>
        /// The POST data as an array of bytes.
        /// </returns>
        protected override byte[] GetPostData()
        {
            return this.Request.SerializeToJsonBytes();
        }

        /// <summary>
        /// An overriden method to parse the result from the service.
        /// </summary>
        /// <param name="responseStream">
        /// A valid response stream.
        /// </param>
        protected override void ParseOutput(Stream responseStream)
        {
            if (responseStream == null)
            {
                throw new ArgumentException("Response stream is null");
            }

            Debug.Assert(this.Result != null, "result is null");
            this.Result.PossibleDestinations = ServiceAgent<PredictLocationResult>.DeserializeResponseJson<PossibleDestination[]>(responseStream);
        }
    }
}