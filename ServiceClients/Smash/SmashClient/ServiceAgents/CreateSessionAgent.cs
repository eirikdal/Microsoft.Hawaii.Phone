// ----------------------------------------------------------
// <copyright file="CreateSessionAgent.cs" company="Microsoft">
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
    using System.Threading;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Contracts;

    /// <summary>
    /// Class agent to get key value item by key.
    /// </summary>
    internal class CreateSessionAgent : AbortableServiceAgent<CreateSessionResponse>
    {
        /// <summary>
        /// Maximum Session Lifetime In Days
        /// </summary>
        public const int MaximumSessionLifetimeInDays = 30;

        /// <summary>
        /// 
        /// </summary>
        private CreateSessionRequest request;

        /// <summary>
        /// Initializes a new instance of the CreateSessionAgent class.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="clientIdentity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionName"></param>
        /// <param name="ownerName"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="attendees"></param>
        /// <param name="lifetime"></param>
        /// <param name="managementID"></param>
        public CreateSessionAgent(
            string hostName,
            ClientIdentity clientIdentity,
            Guid meetingToken,
            string sessionName,
            string ownerName,
            string ownerEmail,
            IEnumerable<string> attendees,
            TimeSpan lifetime,
            Guid managementID)
            : this(hostName, clientIdentity, meetingToken, sessionName, ownerName, ownerEmail, attendees, lifetime, managementID, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CreateSessionAgent class.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="clientIdentity"></param>
        /// <param name="meetingToken"></param>
        /// <param name="sessionName"></param>
        /// <param name="ownerName"></param>
        /// <param name="ownerEmail"></param>
        /// <param name="attendees"></param>
        /// <param name="lifetime"></param>
        /// <param name="managementID"></param>
        /// <param name="stateObject"></param>
        public CreateSessionAgent(
            string hostName,
            ClientIdentity clientIdentity,
            Guid meetingToken,
            string sessionName,
            string ownerName,
            string ownerEmail,
            IEnumerable<string> attendees,
            TimeSpan lifetime,
            Guid managementID,
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

            if (sessionName == null)
            {
                throw new ArgumentNullException("sessionName");
            }

            if (ownerName == null)
            {
                throw new ArgumentNullException("ownerName");
            }

            if (ownerEmail == null)
            {
                throw new ArgumentNullException("ownerEmail");
            }

            if (attendees == null)
            {
                throw new ArgumentNullException("attendees");
            }

            if (lifetime.Days > MaximumSessionLifetimeInDays)
            {
                throw new ArgumentException(string.Format("Session lifetime cannot exceed {0} days.", MaximumSessionLifetimeInDays));
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            this.request = new CreateSessionRequest()
            {
                MeetingToken = meetingToken,
                SessionName = sessionName,
                OwnerName = ownerName,
                OwnerEmail = ownerEmail,
                Attendees = attendees.ToArray(),
                Lifetime = lifetime,
                ManagementID = managementID
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
            this.Result = AbortableServiceAgent<CreateSessionResponse>.DeserializeResponseJson<CreateSessionResponse>(responseStream);
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
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CreateSessionRequest));
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
            string signature = string.Format("{0}", SmashClientREST.CreateSessionSignature);
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(signature));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
