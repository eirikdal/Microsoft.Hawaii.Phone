// ----------------------------------------------------------
// <copyright file="ISmashTable.cs" company="Microsoft">
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
    /// Base interface for SmashTable&lt;T&gt;.
    /// Application classes do not implement this interface.
    /// </summary>
    public interface ISmashTable
    {
        /// <summary>
        /// Unique identifying hash of the derived SmashTable's type and name.
        /// This is used internally by Smash and applications should not call this API.
        /// </summary>
        int TypeHash { get; }

        /// <summary>
        /// Attaches a SmashSession to a SmashTable.
        /// This is used internally by Smash and applications should not call this API.
        /// </summary>
        /// <param name="s">the SmashSession to attach the SmashTable to.</param>
        void SetSession(SmashSession s);

#if WINDOWS_PHONE
        /// <summary>
        /// Gets serialable state from the derived SmashTable for application hibernation.
        /// This is used internally by Smash and applications should not call this API.
        /// </summary>
        /// <param name="rows">A List to which serialized record data is appended.</param>
        void GetState(List<DataRow_Wire> rows);
#else
        /// <summary>
        /// Gets a synchronization object. 
        /// Applications use this to lock access to a SmashTable when using it from multiple threads.
        /// Multi-threaded access to SmashTable object cannot be used in conjunction with data binding.
        /// </summary>
        object SyncRoot { get; }
#endif

        /// <summary>
        /// Internal method used by Smash to synchronize table state with state shared by the Smash service.
        /// This is used internally by Smash and applications should not call this API.
        /// </summary>
        /// <param name="recordID">Record ID</param>
        /// <param name="creatorClient">ID of the creator client</param>
        /// <param name="timeStamp">Time stamp</param>
        /// <param name="action">action for rows</param>
        /// <param name="modificationFlag">modification flag</param>
        /// <param name="blob">blob to prepare</param>
        /// <returns>Action to take</returns>
        Action PrepareSendRecord(Guid recordID, Guid creatorClient, long timeStamp, SendRowAction action, int modificationFlag, byte[] blob);
    }
}
