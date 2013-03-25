// ----------------------------------------------------------
// <copyright file="SetRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------
namespace Microsoft.Hawaii.KeyValue.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The request for Create() or Set() operation
    /// </summary>
    [DataContract]
    public class SetRequest
    {
        /// <summary>
        /// Gets or sets KeyValueItem collection
        /// </summary>
        [DataMember]
        public KeyValueItem[] KeyValueCollection { get; set; }
    }
}