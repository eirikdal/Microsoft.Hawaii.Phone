// ----------------------------------------------------------
// <copyright file="GetByKeysRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------
namespace Microsoft.Hawaii.KeyValue.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Request for GetByKeys operation
    /// </summary>
    [DataContract]
    public class GetByKeysRequest
    {
        /// <summary>
        /// Gets or sets the key collection
        /// </summary>
        [DataMember]
        public string[] Keys { get; set; }
    }
}
