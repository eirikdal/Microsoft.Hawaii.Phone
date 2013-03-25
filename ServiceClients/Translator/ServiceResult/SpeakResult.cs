// ----------------------------------------------------------
// <copyright file="SpeakResult.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Translator.Client
{
    using System.IO;
    using Microsoft.Hawaii;

    /// <summary>
    /// Response for the Speak method.
    /// </summary>
    public class SpeakResult : ServiceResult
    {
        /// <summary>
        /// Gets or sets the byte array of the audio signals for speech. 
        /// </summary>
        public byte[] Audio { get; set; } 
    }
}
