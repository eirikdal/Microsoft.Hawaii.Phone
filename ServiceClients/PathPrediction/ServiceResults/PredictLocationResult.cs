// -
// <copyright file="PredictLocationResult.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.PathPrediction.Client
{
    /// <summary>
    /// Class to carry the results of PathPrediction.PredictLocation method invocation.
    /// </summary>
    public class PredictLocationResult : ServiceResult
    {
        /// <summary>
        /// Gets the result of PredictLocation call 
        /// </summary>
        public PossibleDestination[] PossibleDestinations { get; internal set; }
    }
}
