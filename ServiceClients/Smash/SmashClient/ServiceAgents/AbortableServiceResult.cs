// ----------------------------------------------------------
// <copyright file="AbortableServiceResult.cs" company="Microsoft">
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
    public class AbortableServiceResult : ServiceResult
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Aborted { get; set; }
    }
}