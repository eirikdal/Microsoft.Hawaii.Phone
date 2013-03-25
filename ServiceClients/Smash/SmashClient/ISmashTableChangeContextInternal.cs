// ----------------------------------------------------------
// <copyright file="ISmashTableChangeContextInternal.cs" company="Microsoft">
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
    /// 
    /// </summary>
    internal interface ISmashTableChangeContextInternal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frozen"></param>
        /// <returns></returns>
        object GetUnfrozen(object frozen);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frozen"></param>
        /// <param name="unfrozen"></param>
        void AddUnfrozen(object frozen, object unfrozen);
    }
}
