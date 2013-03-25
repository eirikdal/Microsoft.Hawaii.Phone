// ----------------------------------------------------------
// <copyright file="GetByKeysResult.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Response for GetByKeys operation
    /// </summary>
    public class GetByKeysResult : ServiceResult
    {
        /// <summary>
        /// Gets or sets KeyValueItem collection
        /// </summary>
        public KeyValueItem[] KeyValueCollection { get; set; }
    }
}
