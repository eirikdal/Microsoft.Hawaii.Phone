// ----------------------------------------------------------
// <copyright file="JoinSessionAgent.cs" company="Microsoft">
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
    internal class JoinSessionAgent : AbortableServiceAgent<JoinSessionResponse>
    {
        /// <summary>
        /// 
        /// </summary>
        private JoinSessionRequest request;

        /// <summary>
        /// Initializes a new instance of the JoinSessionAgent class.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="clientIdentity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="userName"></param>
        /// <param name="userEmail"></param>
        /// <param name="deviceName"></param>
        /// <param name="forceRejoin"></param>
        public JoinSessionAgent(
            string hostName,
            ClientIdentity clientIdentity,
            Guid meetingToken,
            string userName,
            string userEmail,
            string deviceName,
            bool forceRejoin)
            : this(hostName, clientIdentity, meetingToken, userName, userEmail, deviceName, forceRejoin, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the JoinSessionAgent class.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="clientIdentity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="userName"></param>
        /// <param name="userEmail"></param>
        /// <param name="deviceName"></param>
        /// <param name="forceRejoin"></param>
        /// <param name="stateObject"></param>
        public JoinSessionAgent(
            string hostName,
            ClientIdentity clientIdentity,
            Guid meetingToken,
            string userName,
            string userEmail,
            string deviceName,
            bool forceRejoin,
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

            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            if (userEmail == null)
            {
                throw new ArgumentNullException("userEmail");
            }

            if (deviceName == null)
            {
                throw new ArgumentNullException("deviceName");
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            this.request = new JoinSessionRequest()
            {
                MeetingToken = meetingToken,
                UserName = userName,
                UserEmail = userEmail,
                DeviceName = deviceName,
                ForceRejoin = forceRejoin
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
            this.Result = AbortableServiceAgent<JoinSessionResponse>.DeserializeResponseJson<JoinSessionResponse>(responseStream);
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
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(JoinSessionRequest));
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
            string signature = string.Format("{0}", SmashClientREST.JoinSessionSignature);
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(signature));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
