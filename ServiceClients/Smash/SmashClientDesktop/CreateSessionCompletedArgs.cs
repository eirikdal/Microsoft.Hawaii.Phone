// ----------------------------------------------------------
// <copyright file="CreateSessionCompletedArgs.cs" company="Microsoft">
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
    /// Argument class passed to the completion handler for CreateSessionAsync.
    /// </summary>
    public class CreateSessionCompletedArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the CreateSessionCompletedArgs class.
        /// Applications do not call this directly.
        /// </summary>
        /// <param name="error">null or an Exception if an error occurred.</param>
        /// <param name="cancelled">true if the call has been cancelled programmatically.</param>
        /// <param name="userState">The userState argument passed on the call to CreateSessionAsync.</param>
        internal CreateSessionCompletedArgs(Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
        }

        /// <summary>
        /// The MeetingToken for the new session.
        /// </summary>
        public Guid MeetingToken { get; internal set; }

        /// <summary>
        /// The SessionID for the new session.
        /// </summary>
        public Guid SessionID { get; internal set; }
    }
}