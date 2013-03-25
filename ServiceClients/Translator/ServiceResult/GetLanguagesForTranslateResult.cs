// ----------------------------------------------------------
// <copyright file="GetLanguagesForTranslateResult.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Translator.Client
{
    using Microsoft.Hawaii;

    /// <summary>
    /// Result for the GetLanguagesForTranslate method.
    /// </summary>
    public class GetLanguagesForTranslateResult : ServiceResult
    {
        /// <summary>
        /// Gets or sets the supported languages for translate.
        /// </summary>
        public Language[] SupportedLanguages { get; set; }
    }
}
