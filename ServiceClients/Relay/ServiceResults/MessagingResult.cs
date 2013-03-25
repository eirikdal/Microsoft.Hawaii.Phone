// -
// <copyright file="MessagingResult.cs" company="Microsoft Corporation">
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
    public class MessagingResult : ServiceResult
    {
        /// <summary>
        ///  Gets the messages.
        /// </summary>
        public List<Message> Messages { get; internal set; }
    }
}
