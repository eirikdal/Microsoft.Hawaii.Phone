// ----------------------------------------------------------
// <copyright file="GetLanguagesForSpeakResult.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Translator.Client
{
    using Microsoft.Hawaii;

    /// <summary>
    /// Result for the GetLanguagesForSpeak method.
    /// </summary>
    public class GetLanguagesForSpeakResult : ServiceResult
    {
        /// <summary>
        /// Gets or sets the supported languages for speak.
        /// </summary>
        public Language[] SupportedLanguages { get; set; }
    }
}
