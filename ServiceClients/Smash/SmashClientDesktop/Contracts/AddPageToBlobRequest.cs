// ----------------------------------------------------------
// <copyright file="AddPageToBlobRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    /// <summary>
    /// The wire representation of the method used to add incremental data from a stream to a blob.
    /// Applications do not use this directly.
    /// </summary>
    public class AddPageToBlobRequest
    {
        /// <summary>
        /// Gets or sets the meeting token of the session.
        /// </summary>
        public Guid MeetingToken { get; set; }

        /// <summary>
        /// Gets or sets the session ID of the session.
        /// </summary>
        public Guid SessionID { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the client.
        /// </summary>
        public Guid ClientID { get; set; }

        /// <summary>
        /// Gets or sets the URI of the blob.
        /// </summary>
        public string BlobAddress { get; set; }

        /// <summary>
        /// Gets or sets the blob shared signature.
        /// </summary>
        public string BlobSharedSignature { get; set; }

        /// <summary>
        /// Gets or sets a chunk of data to be added to the blob.
        /// </summary>
        public byte[] Chunk { get; set; }

        /// <summary>
        /// Gets or sets an incremental id for the chunk of data to be added.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates that this is the last chunk if set to true.
        /// </summary>
        public bool Done { get; set; }
    }
}
