// ----------------------------------------------------------
// <copyright file="CreateSessionResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    /// <summary>
    /// The wire representation of the CreateSessionAsync result.
    /// Applications do not use this directly.
    /// </summary>
    public class CreateSessionResponse : AbortableServiceResult
    {
        /// <summary>
        /// Gets or sets the session ID of the created session.
        /// </summary>
        public Guid SessionID { get; set; }
    }
}