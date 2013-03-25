// -
// <copyright file="GroupResult.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.Hawaii;
    
    /// <summary>
    /// Class to carry the results of relay service invocation.
    /// </summary>
    public class GroupResult : ServiceResult
    {
        /// <summary>
        ///  Gets the group object.
        /// </summary>
        public Group Group { get; internal set; }
    }
}
