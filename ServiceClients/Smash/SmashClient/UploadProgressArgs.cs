// ----------------------------------------------------------
// <copyright file="UploadProgressArgs.cs" company="Microsoft">
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
    /// Argument class passed to the handler for upload progress.
    /// </summary>
    public class UploadProgressArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the UploadProgressArgs class.
        /// </summary>
        /// <param name="userState">The userState argument passed on the call to UploadAsync.</param>
        internal UploadProgressArgs(object userState)
            : base(null, false, userState)
        {
        }

        /// <summary>
        /// Number of bytes already uploaded
        /// </summary>
        public long UploadedSize { get; internal set; }

        /// <summary>
        /// The event handler can set this to 'true' to abort upload of a blob.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
