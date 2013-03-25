// -
// <copyright file="NameRegistration.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Rendezvous.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a multicast group of communications endpoints.
    /// </summary>
    [DataContract]
    public class NameRegistration
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        [DataMember]
        public string SecretKey { get; set; }
    }
}