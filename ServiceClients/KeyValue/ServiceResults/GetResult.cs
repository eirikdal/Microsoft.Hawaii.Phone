// ----------------------------------------------------------
// <copyright file="GetResult.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Response for Get operation
    /// </summary>
    public class GetResult : ServiceResult
    {
        /// <summary>
        /// Gets or sets KeyValueItem collection
        /// </summary>
        public KeyValueItem[] KeyValueCollection { get; set; }

        /// <summary> 
        /// Gets or sets the continuation token. 
        /// Continuation token is used by client developers to get the next batch of results 
        /// </summary> 
        public string ContinuationToken { get; set; }
    }
}
