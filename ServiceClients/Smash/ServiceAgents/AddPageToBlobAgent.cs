// ----------------------------------------------------------
// <copyright file="AddPageToBlobAgent.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Contracts;

    /// <summary>
    /// Class agent to get key value item by key.
    /// </summary>
    internal class AddPageToBlobAgent : AbortableServiceAgent<AddPageToBlobResponse>
    {
        /// <summary>
        /// 
        /// </summary>
        private AddPageToBlobRequest request;

        /// <summary>
        /// Initializes a new instance of the AddPageToBlobAgent class.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="clientIdentity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="clientID"></param>
        /// <param name="blobAddress"></param>
        /// <param name="blobSharedSignature"></param>
        /// <param name="chunk"></param>
        /// <param name="id"></param>
        /// <param name="done"></param>
        public AddPageToBlobAgent(
            string hostName,
            ClientIdentity clientIdentity,
            Guid meetingToken,
            Guid sessionID,
            Guid clientID,
            string blobAddress,
            string blobSharedSignature,
            byte[] chunk,
            int id,
            bool done)
            : this(hostName, clientIdentity, meetingToken, sessionID, clientID, blobAddress, blobSharedSignature, chunk, id, done, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AddPageToBlobAgent class.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="clientIdentity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="clientID"></param>
        /// <param name="blobAddress"></param>
        /// <param name="blobSharedSignature"></param>
        /// <param name="chunk"></param>
        /// <param name="id"></param>
        /// <param name="done"></param>
        /// <param name="stateObject"></param>
        public AddPageToBlobAgent(
            string hostName,
            ClientIdentity clientIdentity,
            Guid meetingToken,
            Guid sessionID,
            Guid clientID,
            string blobAddress,
            string blobSharedSignature,
            byte[] chunk,
            int id,
            bool done,
            object stateObject) :
            base(HttpMethod.Post, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (clientIdentity == null)
            {
                throw new ArgumentNullException("clientIdentity");
            }

            if (blobAddress == null)
            {
                throw new ArgumentNullException("blobAddress");
            }

            if (blobSharedSignature == null)
            {
                throw new ArgumentNullException("blobSharedSignature");
            }

            if (chunk == null)
            {
                throw new ArgumentNullException("chunk");
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            this.request = new AddPageToBlobRequest()
            {
                MeetingToken = meetingToken,
                SessionID = sessionID,
                ClientID = clientID,
                BlobAddress = blobAddress,
                BlobSharedSignature = blobSharedSignature,
                Chunk = chunk,
                Id = id,
                Done = done
            };

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
            object state = this.Result.StateObject;
            this.Result = AbortableServiceAgent<AddPageToBlobResponse>.DeserializeResponseJson<AddPageToBlobResponse>(responseStream);
            this.Result.Status = Status.Success;
            this.Result.StateObject = state;
        }

        /// <summary>
        /// An overriden method to get the data for POST service call.
        /// </summary>
        /// <returns>Return the byte array of the Post data.</returns>
        protected override byte[] GetPostData()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AddPageToBlobRequest));
                serializer.WriteObject(stream, this.request);

                return stream.ToArray();
            }
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
            string signature = string.Format("{0}", SmashClientREST.AddPageToBlobSignature);
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(signature));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
