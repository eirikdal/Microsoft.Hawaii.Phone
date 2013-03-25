// ----------------------------------------------------------
// <copyright file="EnumSessionsResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// The wire representation of the EnumSessionsAsync result.
    /// Applications do not use this directly.
    /// </summary>
    public class EnumSessionsResponse : AbortableServiceResult
    {
        /// <summary>
        /// Gets or sets a list of SessionInfo objects describing the enumerated sessions.
        /// </summary>
        public SessionInfo[] Sessions { get; set; }
    }
}