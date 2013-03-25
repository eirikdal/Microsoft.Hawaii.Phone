// ----------------------------------------------------------
// <copyright file="GetRowsRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// The wire representation of the method retrieving SmashTable record adds/updates/deletes to the Smash service.
    /// Applications do not use this directly.
    /// </summary>
    public class GetRowsRequest
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
        /// Gets or sets the last known row timestamp (in ticks UTC) for the client making the request.
        /// </summary>
        public long LastKnownRowTime { get; set; }

        /// <summary>
        /// Gets or sets a timeout value (in ms) the service should hold the call for in a long poll.
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// Gets or sets a value used to indicate if the rows read should be retrieved from short-lived cache state in the service (true), or read directly from the Azure table.
        /// </summary>
        public bool Cached { get; set; }
    }
}
