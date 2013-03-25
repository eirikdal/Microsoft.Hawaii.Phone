// -
// <copyright file="Group.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
   using System.Runtime.Serialization; 
    
    /// <summary>
    /// Represents a multicast group of communications endpoints.
    /// </summary>
    [DataContract]
    public class Group
    {
        /// <summary>
        /// Gets or sets the registration id assigned to this group by the relay service.
        /// </summary>
        [DataMember]
        public string RegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the secret key used to authenticate requests operating on this group
        /// to the relay service.
        /// </summary>
        [DataMember]
        public string SecretKey { get; set; }
    }
}