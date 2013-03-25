// -
// <copyright file="OcrServiceResult.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

using Microsoft.Hawaii.Ocr.Client.Model;

namespace Microsoft.Hawaii.Ocr.Client.ServiceResults
{
    /// <summary>
    /// This class represents the result of the OCR processing.
    /// </summary>
    public class OcrServiceResult : ServiceResult
    {
        /// <summary>
        /// Gets the result of the OCR processing.
        /// </summary>
        public OcrResult OcrResult { get; internal set; }
    }
}
