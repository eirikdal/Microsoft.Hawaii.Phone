// ----------------------------------------------------------
// <copyright file="SessionInfo.cs" company="Microsoft">
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
    /// 
    /// </summary>
    internal enum SmashServiceResultCodes
    {
        /// <summary>
        /// 
        /// </summary>
        ErrorClient,

        /// <summary>
        /// 
        /// </summary>
        ErrorServer,

        /// <summary>
        /// 
        /// </summary>
        InvalidArgument,

        /// <summary>
        /// 
        /// </summary>
        Timeout,

        /// <summary>
        /// 
        /// </summary>
        SessionAlreadyExists,

        /// <summary>
        /// 
        /// </summary>
        SessionUnknown,

        /// <summary>
        /// 
        /// </summary>
        ClientAlreadyJoined,

        /// <summary>
        /// 
        /// </summary>
        UserUnknown,

        /// <summary>
        /// 
        /// </summary>
        AccessDenied,

        /// <summary>
        /// 
        /// </summary>
        AccessConflict,

        /// <summary>
        /// 
        /// </summary>
        OutOfStorageQuota,

        /// <summary>
        /// 
        /// </summary>
        TablesInitialized,

        /// <summary>
        /// 
        /// </summary>
        SessionCreated,

        /// <summary>
        /// 
        /// </summary>
        SessionModified,

        /// <summary>
        /// 
        /// </summary>
        SessionJoined,

        /// <summary>
        /// 
        /// </summary>
        SessionFinalized,

        /// <summary>
        /// 
        /// </summary>
        SessionWiped,

        /// <summary>
        /// 
        /// </summary>
        SessionsEnumerated,

        /// <summary>
        /// 
        /// </summary>
        RowsRead,

        /// <summary>
        /// 
        /// </summary>
        RowSent,

        /// <summary>
        /// 
        /// </summary>
        BlobCreated,

        /// <summary>
        /// 
        /// </summary>
        BlobPageAdded
    }

    /// <summary>
    /// Type of operation a DataRow_Wire represents.
    /// Applications do not use this enumeration directly.
    /// </summary>
    public enum SendRowAction
    {
        /// <summary>
        /// Addition of a record
        /// </summary>
        Add,

        /// <summary>
        /// Update of a record
        /// </summary>
        Update,

        /// <summary>
        /// Deletion of a record
        /// </summary>
        Delete
    }

    /// <summary>
    /// Details about a Smash Session.
    /// </summary>
    public class SessionInfo
    {
        /// <summary>
        /// Gets or sets the meeting token.
        /// </summary>
        public Guid MeetingToken { get; set; }

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
        public Guid SessionID { get; set; }

        /// <summary>
        /// Gets or sets the management secret.
        /// Non-owners of the session will only see Guid.Empty values for this field.
        /// </summary>
        public Guid ManagementID { get; set; }

        /// <summary>
        /// Gets or sets name of the session.
        /// </summary>
        public string SessionName { get; set; }

        /// <summary>
        /// Gets or sets user name of the session owner.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets email address of the session owner.
        /// </summary>
        public string OwnerEmail { get; set; }

        /// <summary>
        /// Gets or sets time (UTC) this session was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the session lifetime.
        /// </summary>
        public TimeSpan Lifetime { get; set; }

        /// <summary>
        /// Gets or sets the list of user names allowed to join the session.
        /// </summary>
        public string[] Attendees { get; set; }

        /// <summary>
        /// Gets or sets the approximate total number of bytes of table storage used by the session.
        /// </summary>
        public long TotalTableDataStored { get; set; }

        /// <summary>
        /// Gets or sets the approximate total number of storage transactions used by the session.
        /// </summary>
        public int TotalStorageTransactions { get; set; }

        /// <summary>
        /// Gets or sets the approximate total number of bytes of Azure data center egress used by the session.
        /// </summary>
        public long TotalDataEgress { get; set; }

        /// <summary>
        /// Gets or sets the approximate total number of bytes of blob storage used by the session.
        /// </summary>
        public long TotalBlobDataStored { get; set; }
    }
}
