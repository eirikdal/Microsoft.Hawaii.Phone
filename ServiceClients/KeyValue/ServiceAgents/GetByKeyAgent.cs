// ----------------------------------------------------------
// <copyright file="GetByKeyAgent.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.KeyValue.Client
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Class agent to get key value item by key.
    /// </summary>
    internal class GetByKeyAgent : ServiceAgent<GetByKeyResult>
    {
        /// <summary>
        /// Initializes a new instance of the GetByKeyAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="key">Specifies the key to search</param>
        public GetByKeyAgent(string hostName, ClientIdentity clientIdentity, string key) :
            this(hostName, clientIdentity, key, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GetByKeyAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="key">Specifies the key to search</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public GetByKeyAgent(string hostName, ClientIdentity clientIdentity, string key, object stateObject) :
            base(HttpMethod.Get, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, key);
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
            this.Result.KeyValueItem = DeserializeResponseJson<GetByKeyResult>(responseStream).KeyValueItem;
            this.Result.Status = Status.Success;
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="key">Specifies the key to search</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName, string key)
        {
            string signature = string.Format("{0}/{1}", KeyValueService.KeyValueSignature, key);
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(signature));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
