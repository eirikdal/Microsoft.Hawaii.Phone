// ----------------------------------------------------------
// <copyright file="CreateBlobResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    /// <summary>
    /// The wire representation of the CreateBlobAsync result.
    /// Applications do not use this directly.
    /// </summary>
    public class CreateBlobResponse : AbortableServiceResult
    {
        /// <summary>
        /// Gets or sets the tentative blob URI for the new blob.
        /// </summary>
        public string BlobAddress { get; set; }

        /// <summary>
        /// Gets or sets the shared signature for the new blob.
        /// </summary>
        public string BlobSharedSignature { get; set; }
    }
}