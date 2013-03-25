// -
// <copyright file="CreateEndPointAgent.cs" company="Microsoft Corporation">
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
    /// Class agent to create a new end point.
    /// </summary>
    internal class CreateEndPointAgent : ServiceAgent<EndpointResult>
    {
        /// <summary>
        /// Initializes a new instance of the CreateEndPointAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="name">Specifies a name of the test client.</param>
        /// <param name="timeout">Specifies a timeout value.</param>
        public CreateEndPointAgent(string hostName, ClientIdentity clientIdentity, string name, TimeSpan timeout) :
            this(hostName, clientIdentity, name, timeout, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CreateEndPointAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="name">Specifies a name of the test client.</param>
        /// <param name="timeout">Specifies a timeout value.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public CreateEndPointAgent(string hostName, ClientIdentity clientIdentity, string name, TimeSpan timeout, object stateObject) :
            base(HttpMethod.Post, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, name, timeout);
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
            this.Result.EndPoint = DeserializeResponse<Endpoint>(responseStream);
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="name">Specifies a name of the test client.</param>
        /// <param name="timeout">Specifies a timeout value.</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName, string name, TimeSpan timeout)
        {
            // Create the service Uri.
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(RelayService.EndPointSignature));
            uri.AddQueryString(RelayService.NameKey, Uri.EscapeUriString(name));
            uri.AddQueryString(RelayService.TtlKey, Uri.EscapeUriString(timeout.ToString()));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
