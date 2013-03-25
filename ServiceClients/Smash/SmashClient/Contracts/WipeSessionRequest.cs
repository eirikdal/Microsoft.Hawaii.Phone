// ----------------------------------------------------------
// <copyright file="WipeSessionRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    /// <summary>
    /// The wire representation of the WipeSessionAsync call.
    /// Applications do not use this directly.
    /// </summary>
    public class WipeSessionRequest
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
    }
}
