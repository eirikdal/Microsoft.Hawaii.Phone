// -
// <copyright file="DeleteEndPointAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System;
    using System.Net;
    using System.Text;
    
    /// <summary>
    /// Class agent to delete and endpoint.
    /// </summary>
    internal class DeleteEndPointAgent : ServiceAgent<EndpointResult>
    {
        /// <summary>
        /// Initializes a new instance of the DeleteEndPointAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="endpoint">Specifies an endpoint to delete.</param>
        public DeleteEndPointAgent(string hostName, ClientIdentity clientIdentity, Endpoint endpoint) :
            this(hostName, clientIdentity, endpoint, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DeleteEndPointAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="endpoint">Specifies an endpoint to delete.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public DeleteEndPointAgent(string hostName, ClientIdentity clientIdentity, Endpoint endpoint, object stateObject) :
            base(HttpMethod.Delete, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (endpoint == null ||
               string.IsNullOrEmpty(endpoint.RegistrationId))
            {
                throw new ArgumentNullException("endpoint");
            }

            clientIdentity.SecretKey = endpoint.SecretKey;
            clientIdentity.RegistrationId = endpoint.RegistrationId;

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, endpoint);
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="endpoint">Specifies an endpoint to delete.</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName, Endpoint endpoint)
        {
            // Create the service Uri.
            string signature = string.Format("{0}/{1}", RelayService.EndPointSignature, endpoint.RegistrationId);
            ServiceUri uri = new ServiceUri(hostName, signature);

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
