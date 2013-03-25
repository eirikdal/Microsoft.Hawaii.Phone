// ----------------------------------------------------------
// <copyright file="SendRowsResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// The wire representation of the result of the method used to send client updates of SmashTable to the Smash service.
    /// Applications do not use this directly.
    /// </summary>
    public class SendRowsResponse : AbortableServiceResult
    {
        /// <summary>
        /// Gets or sets a timestamp value used by the service to preserve sort order of row updates.
        /// </summary>
        public long TimeStampStart { get; set; }

        /// <summary>
        /// Gets or sets list of unique row Guids assigned by the service to the rows.
        /// </summary>
        public Guid[] RowGuids { get; set; }
    }
}
