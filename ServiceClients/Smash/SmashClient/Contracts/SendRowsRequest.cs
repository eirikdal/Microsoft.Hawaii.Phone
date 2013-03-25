// ----------------------------------------------------------
// <copyright file="SendRowsRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// The wire representation of the method sending SmashTable record adds/updates/deletes to the Smash service.
    /// Applications do not use this directly.
    /// </summary>
    public class SendRowsRequest
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
        /// Gets or sets a list of DataRows_Wire records sent to the service to reflect client side updates.
        /// </summary>
        public DataRow_Wire[] RowsWire { get; set; }
    }
}
