// -
// <copyright file="Message.cs" company="Microsoft Corporation">
//    Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -

namespace Microsoft.Hawaii.Relay.Client
{
    using System;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Represents a message received from the relay service.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Initializes a new instance of the Message class from params.
        /// </summary>
        public Message()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Message class from params.
        /// </summary>
        /// <param name="from">Sender of the message.</param>
        /// <param name="to">Recepient of the message.</param>
        /// <param name="body">Body of the message.</param>
        public Message(string from, string to, byte[] body)
        {
            this.From = from;
            this.To = to;
            this.Body = body;
        }

        /// <summary>
        /// Initializes a new instance of the Message class from data
        /// read from an XML stream.
        /// </summary>
        /// <param name="reader">The XmlReader for the XML data.</param>
        public Message(XmlReader reader)
        {
            MemoryStream growableBuffer = null;

            try
            {
                reader.ReadToFollowing("From");
                this.From = reader.ReadElementContentAsString();

                reader.ReadToFollowing("To");
                this.To = reader.ReadElementContentAsString();

                reader.ReadToFollowing("Body");
                int got = 0;
                byte[] buffer = new byte[100];
                growableBuffer = new MemoryStream();
                while ((got = reader.ReadElementContentAsBase64(buffer, 0, buffer.Length)) > 0)
                {
                    growableBuffer.Write(buffer, 0, got);
                }

                this.Body = growableBuffer.ToArray();

                this.Valid = true;
            }
            catch
            {
                return;
            }
            finally
            {
                if (growableBuffer != null)
                {
                    growableBuffer.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the message was constructed properly.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets a registration id of the sender of this message.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets a registration id(s) of the recipient(s) of this message.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        public byte[] Body { get; set; }
    }
}
