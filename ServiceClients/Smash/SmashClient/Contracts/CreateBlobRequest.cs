// ----------------------------------------------------------
// <copyright file="CreateBlobRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    /// <summary>
    /// The wire representation of the CreateBlobAsync call.
    /// Applications do not use this directly.
    /// </summary>
    public class CreateBlobRequest
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
        /// Gets or sets the file name extension for the blob.
        /// </summary>
        public string FileExtension { get; set; }
    }
}
