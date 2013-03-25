// ----------------------------------------------------------
// <copyright file="KeyValueItem.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System;

    /// <summary>
    /// Entity class to represent a key-value pair
    /// </summary>
    public class KeyValueItem
    {
        /// <summary>
        /// Gets or sets the key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the last modified date
        /// </summary>
        public DateTime LastModifiedDate { get; set; }
    }
}
