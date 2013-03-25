// ----------------------------------------------------------
// <copyright file="GetRowsResponse.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Contracts
{
    using System;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// The wire representation of the result of the method used to retrieve row updates from the Smash service.
    /// Applications do not use this directly.
    /// </summary>
    public class GetRowsResponse : AbortableServiceResult
    {
        /// <summary>
        /// A list of DataRow_Wire objects describing the row updates to be applied to the client's state.
        /// </summary>
        public DataRow_Wire[] RowsOut { get; set; }
    }
}
