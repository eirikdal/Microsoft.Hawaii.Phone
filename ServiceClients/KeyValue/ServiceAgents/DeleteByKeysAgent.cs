// ----------------------------------------------------------
// <copyright file="DeleteByKeysAgent.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization.Json;

    /// <summary>
    /// Class agent to delete key value item by keys.
    /// </summary>
    internal class DeleteByKeysAgent : ServiceAgent<DeleteResult>
    {
        /// <summary>
        /// The keys array to search.
        /// </summary>
        private string[] keys;

        /// <summary>
        /// Initializes a new instance of the DeleteByKeysAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="keys">Specifies the key to search</param>
        public DeleteByKeysAgent(string hostName, ClientIdentity clientIdentity, string[] keys) :
            this(hostName, clientIdentity, keys, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DeleteByKeysAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="keys">Specifies the key to search</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public DeleteByKeysAgent(string hostName, ClientIdentity clientIdentity, string[] keys, object stateObject) :
            base(HttpMethod.Delete, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (keys == null || keys.Length == 0)
            {
                throw new ArgumentNullException("keys");
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;
            this.keys = keys;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName);
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
        /// An overriden method to get the data for POST service call.
        /// </summary>
        /// <returns>Return the byte array of the Post data.</returns>
        protected override byte[] GetPostData()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DeleteByKeysRequest));
                serializer.WriteObject(stream, new DeleteByKeysRequest() { Keys = this.keys });

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName)
        {
            string signature = string.Format("{0}/{1}", KeyValueService.KeyValueSignature, KeyValueService.GetBatchKey);
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(signature));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
