// ----------------------------------------------------------
// <copyright file="JoinSessionResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    /// <summary>
    /// The wire representation of the JoinSessionAsync result.
    /// Applications do not use this directly.
    /// </summary>
    public class JoinSessionResponse : AbortableServiceResult
    {
        /// <summary>
        /// Gets or sets the session ID of the joined session.
        /// </summary>
        public Guid SessionID { get; set; }

        /// <summary>
        /// Gets or sets the client id of the client which joined the session.
        /// </summary>
        public Guid ClientID { get; set; }
    }
}