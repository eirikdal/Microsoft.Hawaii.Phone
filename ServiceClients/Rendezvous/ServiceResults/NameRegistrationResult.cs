// -
// <copyright file="NameRegistrationResult.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Rendezvous.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.Hawaii;
    
    /// <summary>
    /// Class to carry the results of Rendezvous service invocation.
    /// </summary>
    public class NameRegistrationResult : ServiceResult
    {
        /// <summary>
        /// Gets or sets registration id.
        /// </summary>
        public NameRegistration NameRegistration { get; set; }
    }
}
