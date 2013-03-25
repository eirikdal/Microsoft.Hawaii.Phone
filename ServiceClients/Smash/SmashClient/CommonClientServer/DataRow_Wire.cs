// ----------------------------------------------------------
// <copyright file="DataRow_Wire.cs" company="Microsoft">
//     Copyright (tableClient) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.Text;

    /// <summary>
    /// This class represents per-record data used to synchronize SmashTable records between sessions and the Smash service.
    /// Applications do not use this class directly.
    /// </summary>
    public class DataRow_Wire
    {
        /// <summary>
        /// Gets or sets the type of operation this data record represents
        /// </summary>
        public SendRowAction Action { get; set; }

        /// <summary>
        /// Gets or set the unique record identifier for this row
        /// </summary>
        public Guid GUID { get; set; }

        /// <summary>
        /// Gets or sets the timestamp (UTC ticks) for last modification of this record
        /// </summary>
        public long TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the client id for the creator of this row
        /// </summary>
        public Guid CreatorClient { get; set; }

        /// <summary>
        /// Gets or sets a flag used to keep track of modification state
        /// </summary>
        public int ModificationFlag { get; set; }

        /// <summary>
        /// Gets or sets the type hash for the SmashTable this row belongs to
        /// </summary>
        public int TypeHash { get; set; }

        /// <summary>
        /// Gets or sets the JSON serialized SmashRecord payload for this row
        /// </summary>
        public byte[] Blob { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal long GetEstimatedEgressSize()
        {
            // Todo: Need better estimates without actual serialization. Look at JSON result...
            long size = 0;

            // 2 characters for SendRowAction
            size += 2;

            // 36 characters for GUID
            size += 36;

            // 16 characters for TimeStamp
            size += 16;

            // 36 characters for CreatorClient
            size += 36;

            // 2 characters for ModificationFlag
            size += 2;

            // 2 characters for TypeHash
            size += 2;

            size += this.Blob.Length;

            return size;
        }
    }
}
