// ----------------------------------------------------------
// <copyright file="DeleteAgent.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Class agent to delete key value item by key.
    /// </summary>
    internal class DeleteAgent : ServiceAgent<DeleteResult>
    {
        /// <summary>
        /// Initializes a new instance of the DeleteAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="before">Specifies the timpstamp for delete operation.</param>
        public DeleteAgent(string hostName, ClientIdentity clientIdentity, string prefix, DateTime before) :
            this(hostName, clientIdentity, prefix, before, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DeleteAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="before">Specifies the timpstamp for delete operation.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public DeleteAgent(string hostName, ClientIdentity clientIdentity, string prefix, DateTime before, object stateObject) :
            base(HttpMethod.Delete, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, prefix, before);
        }

        /// <summary>
        /// An overriden property to set the request content type to be Json.
        /// </summary>
        protected override string RequestContentType
        {
            get
            {
                return "application/json";
            }
        }

        /// <summary>
        /// An overriden method to parse the result from the service.
        /// </summary>
        /// <param name="responseStream">
        /// A valid response stream.
        /// </param>
        protected override void ParseOutput(System.IO.Stream responseStream)
        {
            if (responseStream == null)
            {
                throw new ArgumentException("Response stream is null");
            }

            Debug.Assert(this.Result != null, "result is null");
            DeleteResult result = DeserializeResponseJson<DeleteResult>(responseStream);

            this.Result.AvailableByteCount = result.AvailableByteCount;
            this.Result.DeletedItemNumber = result.DeletedItemNumber;
            this.Result.MoreItemsToDelete = result.MoreItemsToDelete;
            this.Result.TotalKeyValuePairCount = result.TotalKeyValuePairCount;

            this.Result.Status = Status.Success;
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="before">Specifies the timpstamp for delete operation.</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName, string prefix, DateTime before)
        {
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(KeyValueService.KeyValueSignature));
            uri.AddQueryString(KeyValueService.PrefixKey, prefix);
            uri.AddQueryString(KeyValueService.BeforeKey, before.ToString());

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
