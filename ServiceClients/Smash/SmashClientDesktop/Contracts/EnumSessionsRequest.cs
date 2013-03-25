// ----------------------------------------------------------
// <copyright file="EnumSessionsRequest.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// The wire representation of the EnumSessionsAsync call.
    /// Applications do not use this directly.
    /// </summary>
    public class EnumSessionsRequest
    {
        /// <summary>
        /// Gets or sets the owner's management secret required to enumerate, modify, wipe sessions.
        /// </summary>
        public Guid ManagementID { get; set; }
    }
}
