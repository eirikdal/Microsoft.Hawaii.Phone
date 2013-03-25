// ----------------------------------------------------------
// <copyright file="ModifySessionRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    /// <summary>
    /// The wire representation of the ModifySessionAsync call.
    /// Applications do not use this directly.
    /// </summary>
    public class ModifySessionRequest
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
        /// Gets or sets the owner's management secret required to enumerate, modify, wipe sessions.
        /// </summary>
        public Guid ManagementID { get; set; }

        /// <summary>
        /// Gets or sets a list of user names to be added to the list of users allowed to join the session. Wildcard '*' matches all users.
        /// </summary>
        public string[] AttendeesAdd { get; set; }

        /// <summary>
        /// Gets or sets a list of user names to be removed to the list of users allowed to join the session.
        /// </summary>
        public string[] AttendeesRemove { get; set; }
    }
}
