// ----------------------------------------------------------
// <copyright file="JoinSessionRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    /// <summary>
    /// The wire representation of the JoinSessionAsync call.
    /// Applications do not use this directly.
    /// </summary>
    public class JoinSessionRequest
    {
        /// <summary>
        /// Gets or sets the meeting token of the session.
        /// </summary>
        public Guid MeetingToken { get; set; }

        /// <summary>
        /// Gets or sets user name joining the session.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets email address of the user joining the session.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets device name of the user joining the session.
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets a value used to override user+email+device uniqueness requirement for joining a session.
        /// </summary>
        public bool ForceRejoin { get; set; }
    }
}
