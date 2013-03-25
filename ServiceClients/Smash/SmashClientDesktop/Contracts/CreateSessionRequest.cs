// ----------------------------------------------------------
// <copyright file="CreateSessionRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    /// <summary>
    /// The wire representation of the CreateSessionAsync call.
    /// Applications do not use this directly.
    /// </summary>
    public class CreateSessionRequest
    {
        /// <summary>
        /// Gets or sets the meeting token of the session.
        /// </summary>
        public Guid MeetingToken { get; set; }

        /// <summary>
        /// Gets or sets the name of the session.
        /// </summary>
        public string SessionName { get; set; }

        /// <summary>
        /// Gets or sets the organizer of the session.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets email address of organizer of the session.
        /// </summary>
        public string OwnerEmail { get; set; }

        /// <summary>
        /// Gets or sets list of user names allowed to join a session. A wildcard '*' matches all users attempting to join.
        /// </summary>
        public string[] Attendees { get; set; }

        /// <summary>
        /// Gets or sets the lifetime of the session. Can be up to 30 days. The session will automatically be erased after expiration of this timespan.
        /// </summary>
        public TimeSpan Lifetime { get; set; }

        /// <summary>
        /// Gets or sets the owner's management secret required to enumerate, modify, wipe sessions.
        /// </summary>
        public Guid ManagementID { get; set; }
    }
}
