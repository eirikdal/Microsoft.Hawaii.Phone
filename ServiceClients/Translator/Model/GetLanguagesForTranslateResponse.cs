// ----------------------------------------------------------
// <copyright file="GetLanguagesForTranslateResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Translator.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Response for the GetLanguagesForTranslate method.
    /// </summary>
    [DataContract]
    public class GetLanguagesForTranslateResponse
    {
        /// <summary>
        /// Gets or sets the supported languages for translate.
        /// </summary>
        [DataMember]
        public Language[] SupportedLanguages { get; set; }
    }
}
