// ----------------------------------------------------------
// <copyright file="GetAgent.cs" company="Microsoft">
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
    internal class GetAgent : ServiceAgent<GetResult>
    {
        /// <summary>
        /// Initializes a new instance of the GetAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="size">Specifies the size of result.</param>
        /// <param name="continuationToken">Specifies the continuation token.</param>
        public GetAgent(string hostName, ClientIdentity clientIdentity, string prefix, int size, string continuationToken) :
            this(hostName, clientIdentity, prefix, size, continuationToken, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the GetAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="size">Specifies the size of result.</param>
        /// <param name="continuationToken">Specifies the continuation token.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public GetAgent(string hostName, ClientIdentity clientIdentity, string prefix, int size, string continuationToken, object stateObject) :
            base(HttpMethod.Get, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, prefix, size, continuationToken);
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
            GetResult result = DeserializeResponseJson<GetResult>(responseStream);
            this.Result.ContinuationToken = result.ContinuationToken;
            this.Result.KeyValueCollection = result.KeyValueCollection;
            this.Result.Status = Status.Success;
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="prefix">Specifies the search prefix keyword.</param>
        /// <param name="size">Specifies the size of result.</param>
        /// <param name="continuationToken">Specifies the continuation token.</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(string hostName, string prefix, int size, string continuationToken)
        {
            ServiceUri uri = new ServiceUri(hostName, Uri.EscapeUriString(KeyValueService.KeyValueSignature));
            uri.AddQueryString(KeyValueService.PrefixKey, prefix);
            uri.AddQueryString(KeyValueService.SizeKey, size.ToString());
            uri.AddQueryString(KeyValueService.ContinuationTokenKey, continuationToken);

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}