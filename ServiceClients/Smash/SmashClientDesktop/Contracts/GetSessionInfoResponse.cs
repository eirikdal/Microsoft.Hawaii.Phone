// ----------------------------------------------------------
// <copyright file="GetSessionInfoResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// The wire representation of the GetSessionInfoAsync result.
    /// Applications do not use this directly.
    /// </summary>
    public class GetSessionInfoResponse : AbortableServiceResult
    {
        /// <summary>
        /// Gets or sets a SessionInfo object describing the session.
        /// </summary>
        public SessionInfo Session { get; set; }
    }
}