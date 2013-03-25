// -
// <copyright file="SendMessageAgent.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System;
    using System.Net;
    using System.Text;
    
    /// <summary>
    /// Class agent to send message to recipients.
    /// </summary>
    internal class SendMessageAgent : ServiceAgent<MessagingResult>
    {
        /// <summary>
        /// Byte array to hold the message
        /// </summary>
        private byte[] message;

        /// <summary>
        /// Initializes a new instance of the SendMessageAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="fromEndPoint">Specifies an from end point.</param>
        /// <param name="recipientIds">Specifies recipient (either endpoint or group) ids.</param>
        /// <param name="message">Specifies an message data to be sent.</param>
        /// <param name="timeout">The message time-to-live.</param>
        public SendMessageAgent(
            string hostName,
             ClientIdentity clientIdentity, 
            Endpoint fromEndPoint, 
            string recipientIds, 
            byte[] message, 
            TimeSpan timeout) :
            this(hostName, clientIdentity, fromEndPoint, recipientIds, message, timeout, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SendMessageAgent class.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="clientIdentity">Specifies the client identity.</param>
        /// <param name="fromEndPoint">Specifies an from end point.</param>
        /// <param name="recipientIds">Specifies recipient (either endpoint or group) ids.</param>
        /// <param name="message">Specifies an message data to be sent.</param>
        /// <param name="timeout">The message time-to-live.</param>
        /// <param name="stateObject">Specifies a user-defined object.</param>
        public SendMessageAgent(
            string hostName,
            ClientIdentity clientIdentity, 
            Endpoint fromEndPoint, 
            string recipientIds, 
            byte[] message, 
            TimeSpan timeout, 
            object stateObject) :
            base(HttpMethod.Post, stateObject)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }
            
            if (fromEndPoint == null || 
                string.IsNullOrEmpty(fromEndPoint.RegistrationId))
            {
                throw new ArgumentNullException("endpoint");
            }

            if (string.IsNullOrEmpty(recipientIds))
            {
                throw new ArgumentNullException("recipientIds");
            }

            if (message == null || message.Length == 0)
            {
                throw new ArgumentNullException("message");
            }

            this.message = message;

            clientIdentity.SecretKey = fromEndPoint.SecretKey;
            clientIdentity.RegistrationId = fromEndPoint.RegistrationId;

            // Set the client identity.
            this.ClientIdentity = clientIdentity;

            // Create the service uri.
            this.Uri = this.CreateServiceUri(hostName, fromEndPoint, recipientIds, timeout);
        }

        /// <summary>
        /// An overriden method to get the data for POST service call.
        /// </summary>
        /// <returns>Return the byte array of the Post data.</returns>
        protected override byte[] GetPostData()
        {
            return this.message;
        }

        /// <summary>
        /// Method creates the service uri.
        /// </summary>
        /// <param name="hostName">Specifies a host name of the service.</param>
        /// <param name="fromEndPoint">Specifies an from end point.</param>
        /// <param name="recipientIds">Specifies recipient (either endpoint or group) ids.</param>
        /// <param name="timeout">The message time-to-live.</param>
        /// <returns>A valid service uri object.</returns>
        private Uri CreateServiceUri(
                                     string hostName,
                                     Endpoint fromEndPoint,
                                     string recipientIds,
                                     TimeSpan timeout)
        {
            // Create the service Uri.
            string signature = string.Format(
                "{0}/{1}", 
                RelayService.EndPointSignature, 
                fromEndPoint.RegistrationId);
            ServiceUri uri = new ServiceUri(hostName, signature);
            uri.AddQueryString(RelayService.ToKey, Uri.EscapeUriString(recipientIds));
            uri.AddQueryString(RelayService.TtlKey, Uri.EscapeUriString(timeout.ToString()));

            // Return the URI object.
            return new Uri(uri.ToString());
        }
    }
}
