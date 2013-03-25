// ----------------------------------------------------------
// <copyright file="TranslateResult.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Translator.Client
{
    using Microsoft.Hawaii;

    /// <summary>
    /// Response for the Translate method.
    /// </summary>
    public class TranslateResult : ServiceResult
    {
        /// <summary>
        /// Gets or sets the text be translated to the target language.
        /// </summary>
        public string TextTranslated { get; set; }
    }
}
