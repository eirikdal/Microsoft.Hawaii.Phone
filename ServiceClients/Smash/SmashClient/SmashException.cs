// ----------------------------------------------------------
// <copyright file="SmashException.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Threading;
    using System.Windows.Threading;

    /// <summary>
    /// Base class for Smash exception classes. Applications to not use this directly.
    /// </summary>
    [Serializable]
    public class SmashException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the SmashException class.
        /// </summary>
        public SmashException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SmashException class.
        /// </summary>
        /// <param name="message">A descriptive message.</param>
        public SmashException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SmashException class.
        /// </summary>
        /// <param name="message">A descriptive message.</param>
        /// <param name="inner">The inner exception.</param>
        public SmashException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
