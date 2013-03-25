// -
// <copyright file="EndpointResult.cs" company="Microsoft Corporation">
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
    public class EndpointResult : ServiceResult
    {
        /// <summary>
        /// Gets the endpoint.
        /// </summary>
        public Endpoint EndPoint { get; internal set; }
    }
}
