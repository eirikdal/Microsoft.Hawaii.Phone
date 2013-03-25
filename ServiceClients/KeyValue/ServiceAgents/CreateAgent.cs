// ----------------------------------------------------------
// <copyright file="CreateAgent.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// Class agent to get key value item by key.
    /// </summary>
    internal class CreateAgent : ServiceAgent<SetResult>
    {
        /// <summary>
        /// The KeyValue items to create.
        /// </summary>
        private KeyValueItem[] items;

        /// <summary>
        /// Initializes a new instance of the CreateAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="items">Specifies the items to create.</param>
        public CreateAgent(string hostName, ClientIdentity clientIdentity, KeyValueItem[] items) :
            this(hostName, clientIdentity, items, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CreateAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="items">Specifies the items to create.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public CreateAgent(string hostName, ClientIdentity clientIdentity, KeyValueItem[] items, object stateObject) :
            base(HttpMethod.Post, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (items == null || items.Length == 0)
            {
                throw new ArgumentNullException("items");
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;
            this.items = items;

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

            SetResult result = DeserializeResponseJson<SetResult>(responseStream);

            this.Result.AvailableByteCount = result.AvailableByteCount;
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
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SetRequest));
                serializer.WriteObject(stream, new SetRequest() { KeyValueCollection = this.items });

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
            ServiceUri uri = new ServiceUri(hostName, KeyValueService.KeyValueSignature);

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
