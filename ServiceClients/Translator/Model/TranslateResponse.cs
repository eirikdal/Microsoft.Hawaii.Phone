// ----------------------------------------------------------
// <copyright file="TranslateResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Translator.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Response for the Translate method.
    /// </summary>
    [DataContract]
    public class TranslateResponse
    {
        /// <summary>
        /// Gets or sets the text be translated to the target language.
        /// </summary>
        [DataMember]
        public string TextTranslated { get; set; }
    }
}
