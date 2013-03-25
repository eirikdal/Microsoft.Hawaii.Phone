// -
// <copyright file="DeleteGroupAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System;
    using System.Net;
    using System.Text;
    
    /// <summary>
    /// Class agent to delete a group.
    /// </summary>
    internal class DeleteGroupAgent : ServiceAgent<GroupResult> 
    {
        /// <summary>
        /// Initializes a new instance of the DeleteGroupAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="group">Specifies a group to be deleted.</param>
        public DeleteGroupAgent(string hostName, ClientIdentity clientIdentity, Group group) :
            this(hostName, clientIdentity, group, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DeleteGroupAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="group">Specifies a group to be deleted.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public DeleteGroupAgent(string hostName, ClientIdentity clientIdentity, Group group, object stateObject) :
            base(HttpMethod.Delete, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (group == null || 
                string.IsNullOrEmpty(group.RegistrationId))
            {
                throw new ArgumentNullException("group");
            }

            clientIdentity.SecretKey = group.SecretKey;
            clientIdentity.RegistrationId = group.RegistrationId;

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, group);
        }
   
        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="group">Specifies a group to be deleted.</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName, Group group)
        {
            // Create the service Uri.
            string signature = string.Format("{0}/{1}", RelayService.GroupSignature, group.RegistrationId);
            ServiceUri uri = new ServiceUri(hostName, signature);

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
