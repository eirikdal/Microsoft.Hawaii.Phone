// -
// <copyright file="CreateGroupAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    
    /// <summary>
    /// Class agent to create a new group.
    /// </summary>
    internal class CreateGroupAgent : ServiceAgent<GroupResult>
    {
        /// <summary>
        /// Initializes a new instance of the CreateGroupAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="timeout">Specifies a time span to live.</param>
        public CreateGroupAgent(string hostName, ClientIdentity clientIdentity, TimeSpan timeout) :
            this(hostName, clientIdentity, timeout, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CreateGroupAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="timeout">Specifies a time span to live.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public CreateGroupAgent(string hostName, ClientIdentity clientIdentity, TimeSpan timeout, object stateObject) :
            base(HttpMethod.Post, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, timeout);
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
            this.Result.Group = DeserializeResponse<Group>(responseStream);
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="timeout">Specifies a time span to live.</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName, TimeSpan timeout)
        {
            // Create the service Uri.
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(RelayService.GroupSignature));
            uri.AddQueryString(RelayService.TtlKey, Uri.EscapeUriString(timeout.ToString()));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
