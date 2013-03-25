// ----------------------------------------------------------
// <copyright file="CreateBlobCompletedArgs.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Threading;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// Argument class passed to the completion handler for CreateBlobAsync.
    /// </summary>
    public class CreateBlobCompletedArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the CreateBlobCompletedArgs class.
        /// </summary>
        /// <param name="error">null or an Exception if an error occurred.</param>
        /// <param name="cancelled">true if the call has been cancelled programmatically.</param>
        /// <param name="userState">The userState argument passed on the call to CreateBlobAsync.</param>
        internal CreateBlobCompletedArgs(Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
        }

        /// <summary>
        /// The helper object used for async upload of data from a stream to the blob storage.
        /// </summary>
        public SmashBlobUploader BlobUploader { get; internal set; }
    }
}