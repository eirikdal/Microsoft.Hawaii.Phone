// -
// <copyright file="ReceiveMessagesAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml;

    /// <summary>
    /// Class agent to received all endpoint messages from service.
    /// </summary>
    internal class ReceiveMessagesAgent : ServiceAgent<MessagingResult>
    {
        /// <summary>
        /// Initializes a new instance of the ReceiveMessagesAgent class.
        /// </summary>
        /// <param name="hostName">
        /// Specifies a host name of the service.
        /// </param>
        /// <param name="clientIdentity">
        /// Specifies the client identity.
        /// </param>
        /// <param name="endpoint">
        /// Specifies an endpoint to leave a group.
        /// </param>
        /// <param name="filter">
        /// Specifies a list of registration ids  for Endpoints and/or Groups that
        /// identify senders and/or group recipients of desired messages.
        /// </param>
        /// <param name="wait">
        /// The time to wait for a message.
        /// </param>
        public ReceiveMessagesAgent(string hostName, ClientIdentity clientIdentity, Endpoint endpoint, string filter, TimeSpan wait) :
            this(hostName, clientIdentity, endpoint, filter, wait, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ReceiveMessagesAgent class.
        /// </summary>
        /// <param name="hostName">
        /// Specifies a host name of the service.
        /// </param>
        /// <param name="clientIdentity">
        /// Specifies the client identity.
        /// </param>
        /// <param name="endpoint">
        /// Specifies an endpoint to leave a group.
        /// </param>
        /// <param name="filter">
        /// Specifies a list of registration ids  for Endpoints and/or Groups that
        /// identify senders and/or group recipients of desired messages.
        /// </param>
        /// <param name="wait">
        /// The time to wait for a message.
        /// </param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public ReceiveMessagesAgent(string hostName, ClientIdentity clientIdentity, Endpoint endpoint, string filter, TimeSpan wait, object stateObject) :
            base(HttpMethod.Get, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (endpoint == null ||
                string.IsNullOrEmpty(endpoint.RegistrationId))
            {
                throw new ArgumentNullException("endpoint");
            }

            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            clientIdentity.SecretKey = endpoint.SecretKey;
            clientIdentity.RegistrationId = endpoint.RegistrationId;

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, endpoint, filter, wait);
        }

        /// <summary>
        /// An overriden method to parse the result from the service.
        /// </summary>
        /// <param name="responseStream">
        /// A valid response stream.
        /// </param>
        protected override void ParseOutput(Stream responseStream)
        {
            List<Message> messages = new List<Message>();
            try
            {
                // Read the returned message(s).
                XmlReader reader = XmlReader.Create(responseStream);
                while (reader.ReadToFollowing("Message"))
                {
                    Message message = new Message(reader);
                    if ((message != null) && message.Valid)
                    {
                        messages.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("The Server returned an invalid response", ex);
            }

            if (messages.Count != 0)
            {
                Debug.Assert(this.Result != null, "result is null");
                this.Result.Messages = messages;
            }
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="endpoint">Specifies an endpoint to join a group.</param>
        /// <param name="filter">
        /// Specifies a list of registration ids  for Endpoints and/or Groups that
        /// identify senders and/or group recipients of desired messages.
        /// </param>
        /// <param name="wait">The time to wait for a message.</param>
        /// <returns>
        /// A valid service uri object.
        /// </returns>
        private Uri CreateServiceUri(
                                     string hostName,
                                     Endpoint endpoint,
                                     string filter,
                                     TimeSpan wait)
        {
            // Create the service Uri.
            string signature = string.Format("{0}/{1}", RelayService.EndPointSignature, endpoint.RegistrationId);
            ServiceUri uri = new ServiceUri(hostName, signature);
            uri.AddQueryString(RelayService.FilterKey, Uri.EscapeUriString(filter));
            uri.AddQueryString(RelayService.WaitKey, Uri.EscapeUriString(wait.ToString()));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
