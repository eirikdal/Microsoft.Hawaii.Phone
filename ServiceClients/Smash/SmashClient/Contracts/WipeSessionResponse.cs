// ----------------------------------------------------------
// <copyright file="WipeSessionResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    /// <summary>
    /// The wire representation of the WipeSessionAsync result.
    /// Applications do not use this directly.
    /// </summary>
    public class WipeSessionResponse : AbortableServiceResult
    {
    }
}