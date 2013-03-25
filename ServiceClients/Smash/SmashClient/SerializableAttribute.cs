// ----------------------------------------------------------
// <copyright file="SerializableAttribute.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// Indicates that a class can be serialized. 
    /// This class cannot be inherited. 
    /// This is a mock-up class used only for Windows Phone build. 
    /// Applications do not use this class directly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false)]
    public sealed class SerializableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the System.SerializableAttribute class.
        /// </summary>
        public SerializableAttribute()
        {
        }
    }
}
