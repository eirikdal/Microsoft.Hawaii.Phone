// ----------------------------------------------------------
// <copyright file="DeleteByKeysRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------
namespace Microsoft.Hawaii.KeyValue.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The request for Delete() operation
    /// </summary>
    [DataContract]
    public class DeleteByKeysRequest
    {
        /// <summary>
        /// Gets or sets the key collection
        /// </summary>
        [DataMember]
        public string[] Keys { get; set; }
    }
}
