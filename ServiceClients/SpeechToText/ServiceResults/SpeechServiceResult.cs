// -
// <copyright file="SpeechServiceResult.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Speech.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.Hawaii;
    
    /// <summary>
    /// This class represents the result of the Speec-to-Text processing.
    /// </summary>
    public class SpeechServiceResult : ServiceResult
    {
        /// <summary>
        /// Gets the result of the Speec-to-Text processing.
        /// </summary>
        public SpeechResult SpeechResult { get; internal set; }
    }
}
