// -
// <copyright file="RegisterNameAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Rendezvous.Client
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    
    /// <summary>
    /// Class to register a Name with the service.
    /// </summary>
    internal class RegisterNameAgent : ServiceAgent<NameRegistrationResult>
    {
        /// <summary>
        /// Initializes a new instance of the RegisterNameAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the rendezvous service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="name">Specifies a name to register.</param>
        public RegisterNameAgent(
            string hostName,
            ClientIdentity clientIdentity,
            string name) :
            this(hostName, clientIdentity, name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RegisterNameAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the rendezvous service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="name">Specifies a name to register.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        public RegisterNameAgent(
            string hostName,
            ClientIdentity clientIdentity,
            string name,
            object stateObject) :
            base(HttpMethod.Get, stateObject)
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
            this.Uri = this.CreateServiceUri(hostName, name);
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
            this.Result.NameRegistration = DeserializeResponse<NameRegistration>(responseStream);
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="name">Specifies a name to register.</param>
        /// <returns>A valid uri service object.</returns>
        private Uri CreateServiceUri(string hostName, string name)
        {
            // Create the service Uri.
            string signature = string.Format("{0}", RendezvousService.NameSignature);
            ServiceUri uri = new ServiceUri(hostName, signature);

            uri.AddQueryString("Name", Uri.EscapeUriString(name));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
