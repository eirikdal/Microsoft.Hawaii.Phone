// ----------------------------------------------------------
// <copyright file="SetResult.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Response for the Set or Create operation
    /// </summary>
    public class SetResult : ServiceResult
    {
        /// <summary>
        /// Gets or sets a number of bytes available to the current application
        /// </summary>
        public long AvailableByteCount { get; set; }

        /// <summary>
        /// Gets or sets the current count of key-value pair for a partition
        /// </summary>
        public int TotalKeyValuePairCount { get; set; }
    }
}
