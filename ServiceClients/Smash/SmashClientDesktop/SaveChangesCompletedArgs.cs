// ----------------------------------------------------------
// <copyright file="SaveChangesCompletedArgs.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading;
    using System.Windows.Threading;

    using Microsoft.Hawaii.Smash.Client.Common;

    /// <summary>
    /// Argument class passed to the completion handler for SaveChangesAsync.
    /// </summary>
    public class SaveChangesCompletedArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the SaveChangesCompletedArgs class.
        /// </summary>
        /// <param name="error">null or an Exception if an error occurred.</param>
        /// <param name="cancelled">true if the call has been cancelled programmatically.</param>
        /// <param name="userState">The userState argument passed on the call to SaveChangesAsync.</param>
        internal SaveChangesCompletedArgs(Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
        }
    }
}
