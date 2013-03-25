// ----------------------------------------------------------
// <copyright file="UploadCompletedArgs.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading;
    using System.Windows.Threading;

    /// <summary>
    /// Argument class passed to the completion handler for UploadAsync.
    /// </summary>
    public class UploadCompletedArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the UploadCompletedArgs class.
        /// </summary>
        /// <param name="error">null or an Exception if an error occurred.</param>
        /// <param name="cancelled">true if the call has been cancelled programmatically.</param>
        /// <param name="userState">The userState argument passed on the call to UploadAsync.</param>
        internal UploadCompletedArgs(Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
        }

        /// <summary>
        /// A string containing the URI of the uploaded blob.
        /// </summary>
        public string BlobAddress { get; internal set; }
    }
}
