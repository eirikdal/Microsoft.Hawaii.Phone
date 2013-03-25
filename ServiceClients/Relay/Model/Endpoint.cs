// -
// <copyright file="EndPoint.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Represents a communications endpoint of the relay service.
    /// </summary>
    /// <remarks>
    /// An endpoint is used to send and receive messages through the relay
    /// service.  It is uniquely identified by its
    /// <see cref="RegistrationId" />, which is automatically generated upon
    /// its creation.
    /// </remarks>
    [DataContract]
    public class Endpoint
    {
        /// <summary>
        /// Initializes a new instance of the Endpoint class.
        /// </summary>
        public Endpoint()
        {
            this.Groups = new Groups();
        }

        /// <summary>
        /// Gets or sets the registration id assigned to this endpoint by the relay service.
        /// </summary>
        [DataMember]
        public string RegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the secret key used to authenticate requests operating on this endpoint
        /// to the relay service.
        /// </summary>
        [DataMember]
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the group container to hold groups of this end point.
        /// </summary>
        [IgnoreDataMember]
        public Groups Groups { get; set; }
    }
}
