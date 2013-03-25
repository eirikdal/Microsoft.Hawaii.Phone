// ----------------------------------------------------------
// <copyright file="ISmashTableChangeContext.cs" company="Microsoft">
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
    /// Base interface for SmashTableChangeContext&lt;T&gt;.
    /// Application classes do not implement this interface.
    /// </summary>
    public interface ISmashTableChangeContext
    {
        /// <summary>
        /// Event raised upon completion of SaveChangesAsync
        /// </summary>
        event SaveChangesCompletedHandler SaveChangesCompleted;

        /// <summary>
        /// Adds a record
        /// </summary>
        /// <param name="r">record object. Type of record must be generic type T of SmashTable from which ISmashTableChangeContext was obtained.</param>
        void Add(object r);

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="r">record object. Type of record must be generic type T of SmashTable from which ISmashTableChangeContext was obtained.</param>
        void Delete(object r);

        /// <summary>
        /// Commit pending changes on change context
        /// </summary>
        /// <param name="state">User state. Will be passed as userState in completion event args.</param>
        void SaveChangesAsync(object state);
    }
}
