// ----------------------------------------------------------
// <copyright file="SpeakResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Translator.Client
{
    using System.IO;
    using System.Runtime.Serialization;

    /// <summary>
    /// Response for the Speak method.
    /// </summary>
    [DataContract]
    public class SpeakResponse
    {
        /// <summary>
        /// Gets or sets the byte array of the audio signals for speech. 
        /// </summary>
        [DataMember]
        public byte[] Audio { get; set; } 
    }
}
