// ----------------------------------------------------------
// <copyright file="ChangeSetLimitException.cs" company="Microsoft">
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
    /// This exception is thrown if too many changes have been added to a single table change context.
    /// The total limit is 100 'change units', whereas each record add accounts for 1 change units, and each record delete / update accounts for 3 change units.
    /// For example, 70 adds, 8 updates and 2 deletes amount to a total of 100 change units. 
    /// </summary>
    [Serializable]
    public class ChangeSetLimitException : SmashException
    {
        /// <summary>
        /// Initializes a new instance of the ChangeSetLimitException class.
        /// </summary>
        internal ChangeSetLimitException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ChangeSetLimitException class.
        /// </summary>
        /// <param name="message">A descriptive message.</param>
        internal ChangeSetLimitException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ChangeSetLimitException class.
        /// </summary>
        /// <param name="message">A descriptive message.</param>
        /// <param name="inner">The inner exception.</param>
        internal ChangeSetLimitException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
