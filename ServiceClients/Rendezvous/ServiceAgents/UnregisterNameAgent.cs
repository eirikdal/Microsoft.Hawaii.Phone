// -
// <copyright file="UnregisterNameAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Rendezvous.Client
{
    using System;
    
    /// <summary>
    /// Class to unregister (delete) a Name with the service.
    /// </summary>
    internal class UnregisterNameAgent : ServiceAgent<NameRegistrationResult>
    {
        /// <summary>
        /// Initializes a new instance of the UnregisterNameAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the rendezvous service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        public UnregisterNameAgent(
            string hostName,
            ClientIdentity clientIdentity,
            NameRegistration registration) :
            this(hostName, clientIdentity, registration, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the UnregisterNameAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the rendezvous service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <param name="stateObject">Specifies a user defined object.</param>
        public UnregisterNameAgent(
            string hostName,
            ClientIdentity clientIdentity,
            NameRegistration registration, 
            object stateObject) :
            base(HttpMethod.Delete, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }
            
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            clientIdentity.RegistrationId = registration.Name;
            clientIdentity.SecretKey = registration.SecretKey;

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, registration);
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="registration">Specifies a name registration object.</param>
        /// <returns>A valid uri service object.</returns>
        private Uri CreateServiceUri(string hostName, NameRegistration registration)
        {
            // Create the service Uri.
            string signature = string.Format(
                "{0}/{1}", 
                RendezvousService.NameSignature, 
                Uri.EscapeUriString(registration.Name));

            ServiceUri uri = new ServiceUri(hostName, signature);

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
