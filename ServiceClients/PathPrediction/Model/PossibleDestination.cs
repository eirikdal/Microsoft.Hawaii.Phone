// -----------------------------------------------------------------------
// <copyright file="PossibleDestination.cs" company="Microsoft Research">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Hawaii.PathPrediction.Client
{
    using System.Runtime.Serialization;

    /// <summary>
    /// An object that represents a latlong and an associated probability of being a final destination
    /// </summary>
    [DataContract]
    public struct PossibleDestination
    {
        /// <summary>
        /// Gets or sets the location of the potential destination
        /// </summary>
        [DataMember(Name = "Loc")]
        public LatLong Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the probability that this location is a potential destination
        /// </summary>
        [DataMember(Name = "Prob", EmitDefaultValue = false, IsRequired = false)]
        public double Probability
        {
            get;
            set;
        }
    }
}