// -
// <copyright file="SpeechResult.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Speech.Client
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;
    
    /// <summary>
    /// This class describes the result obtained after a Hawaii Speech-to-Text call.
    /// </summary>
    [DataContract]
    public class SpeechResult
    {
        /// <summary>
        /// Initializes a new instance of the SpeechResult class.
        /// </summary>
        public SpeechResult()
        {
            this.InternalErrorMessage = null;
            this.Items = new List<string>();
        }

        /// <summary>
        /// Gets or sets the error message if an error occures during the Speech-to-Text translation.
        /// </summary>
        [DataMember]
        public string InternalErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the list of items obtained after the speech-to-text translation. 
        /// </summary>
        [DataMember]
        public List<string> Items { get; set; }
    }
}
