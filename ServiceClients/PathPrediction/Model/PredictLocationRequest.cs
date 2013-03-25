// -----------------------------------------------------------------------
// <copyright file="PredictLocationRequest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Hawaii.PathPrediction.Client
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;

    /// <summary>
    /// Request for the Predict Location method
    /// </summary>
    [DataContract]
    public class PredictLocationRequest
    {
        /// <summary>
        /// Gets or sets the the latlong crumbs of the current trip whose destinations we are attempting to predict.
        /// </summary>
        [DataMember]
        public LatLong[] Path { get; set; }

        /// <summary>
        /// Gets or sets the top N results to retrieve.  
        /// </summary>
        /// <example>Calling this function with the value 500 will return the top 500 most likely destinations</example>
        [DataMember]
        public int MaxDestinations { get; set; }

        /// <summary>
        /// Converts this object to JSon string
        /// </summary>
        /// <returns>String containing the requiest serialized to JSON</returns>
        public string SerializeToJson()
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PredictLocationRequest));
                serializer.WriteObject(stream, this);

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Converts this object to JSon bytes
        /// </summary>
        /// <returns>Bytes containing the requiest serialized to JSON</returns>
        public byte[] SerializeToJsonBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PredictLocationRequest));
                serializer.WriteObject(stream, this);
                return stream.ToArray();
            }
        }
    }
}
