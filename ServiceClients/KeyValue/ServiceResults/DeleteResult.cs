// ----------------------------------------------------------
// <copyright file="DeleteResult.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Response for the DeleteByKeys() and Delete() operations
    /// </summary>
    public class DeleteResult : ServiceResult
    {
        /// <summary>
        /// Gets or sets the number of the items deleted by this request
        /// </summary>
        public int DeletedItemNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is more items that can be deleted by this request
        /// </summary>
        public bool MoreItemsToDelete { get; set; }

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
