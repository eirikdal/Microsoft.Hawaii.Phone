// -
// <copyright file="LeaveGroupAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System;
    using System.Net;
    using System.Text;
    
    /// <summary>
    /// Class agent to leave a group.
    /// </summary>
    internal class LeaveGroupAgent : ServiceAgent<GroupResult> 
    {
        /// <summary>
        /// Initializes a new instance of the LeaveGroupAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="endpoint">Specifies an endpoint to leave a group.</param>
        /// <param name="group">Specifies a target group.</param>
        public LeaveGroupAgent(string hostName, ClientIdentity clientIdentity, Endpoint endpoint, Group group) :
            this(hostName, clientIdentity, endpoint, group, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LeaveGroupAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="endpoint">Specifies an endpoint to leave a group.</param>
        /// <param name="group">Specifies a target group.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public LeaveGroupAgent(
            string hostName,
            ClientIdentity clientIdentity, 
            Endpoint endpoint, 
            Group group, 
            object stateObject) :
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

            if (group == null || 
                string.IsNullOrEmpty(group.RegistrationId))
            {
                throw new ArgumentNullException("group");
            }

            clientIdentity.SecretKey = endpoint.SecretKey;
            clientIdentity.RegistrationId = endpoint.RegistrationId;

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, endpoint, group);
        }
      
        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="endpoint">Specifies an endpoint to join a group.</param>
        /// <param name="group">Specifies a target group.</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName, Endpoint endpoint, Group group)
        {
            // Create the service Uri.
            string signature = string.Format(
                                             "{0}/{1}/{2}", 
                                             RelayService.GroupSignature,
                                             Uri.EscapeUriString(group.RegistrationId),
                                             endpoint.RegistrationId);

            ServiceUri uri = new ServiceUri(hostName, signature);

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
