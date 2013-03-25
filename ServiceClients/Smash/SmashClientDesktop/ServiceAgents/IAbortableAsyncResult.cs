// ----------------------------------------------------------
// <copyright file="IAbortableAsyncResult.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Threading;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Contracts;

    /// <summary>
    /// 
    /// </summary>
    internal interface IAbortableAsyncResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timedOut"></param>
        void Abort(bool timedOut);
    }
}