// ----------------------------------------------------------
// <copyright file="GetByKeyResult.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Response for GetByKey operation
    /// </summary>
    public class GetByKeyResult : ServiceResult
    {
        /// <summary>
        /// Gets or sets KeyValueItem
        /// </summary>
        public KeyValueItem KeyValueItem { get; set; }
    }
}
