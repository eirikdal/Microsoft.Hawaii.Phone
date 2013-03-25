// ----------------------------------------------------------
// <copyright file="SmashBlobUploader.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading;
    using System.Windows.Threading;

    /// <summary>
    /// Delegate type for completion handler of UploadAsync.
    /// </summary>
    /// <param name="sender">The SmashBlobUploader instance calling the completion handler.</param>
    /// <param name="e">The completion arguments.</param>
    public delegate void UploadCompletedHandler(object sender, UploadCompletedArgs e);

    /// <summary>
    /// Delegate type for progress handler of UploadAsync.
    /// </summary>
    /// <param name="sender">The SmashBlobUploader instance calling the completion handler.</param>
    /// <param name="e">The progress arguments.</param>
    public delegate void UploadProgressHandler(object sender, UploadProgressArgs e);

    /// <summary>
    /// Helper to asynchronously upload blob data from a stream. The object is obtained from the completion args of SessionManager.CreateBlobAsync.
    /// </summary>
    public sealed class SmashBlobUploader
    {
        /// <summary>
        /// 
        /// </summary>
        private Guid meetingToken;

        /// <summary>
        /// 
        /// </summary>
        private Guid sessionID;

        /// <summary>
        /// 
        /// </summary>
        private Guid clientID;

        /// <summary>
        /// 
        /// </summary>
        private string blobAddress;

        /// <summary>
        /// 
        /// </summary>
        private string blobSharedSignature;

        /// <summary>
        /// 
        /// </summary>
        private ClientIdentity identity;

        /// <summary>
        /// Initializes a new instance of the SmashBlobUploader class.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="m"></param>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <param name="address"></param>
        /// <param name="signature"></param>
        internal SmashBlobUploader(ClientIdentity identity, Guid m, Guid s, Guid c, string address, string signature)
        {
            this.meetingToken = m;
            this.sessionID = s;
            this.clientID = c;
            this.blobAddress = address;
            this.blobSharedSignature = signature;
            this.identity = identity;
        }

        /// <summary>
        /// Event raised upon completion of UploadAsync.
        /// </summary>
        public event UploadCompletedHandler UploadCompleted;

        /// <summary>
        /// Event raised indicating progress of UploadAsync.
        /// </summary>
        public event UploadProgressHandler UploadProgress;

        /// <summary>
        /// Initiates async upload of blob data from a stream.
        /// </summary>
        /// <param name="stream">Stream containing data to be uploaded.</param>
        /// <param name="state">State to be passed as userState in the progress and completion event args.</param>
        public void UploadAsync(Stream stream, object state)
        {
            ThreadPool.QueueUserWorkItem(
                new WaitCallback((object o) =>
                {
                    UploadCompletedArgs e = new UploadCompletedArgs(null, false, o);
                    UploadProgressArgs progress = new UploadProgressArgs(o);
                    SmashClient client = new SmashClient(this.identity);
                    try
                    {
                        using (var streamcopy = stream)
                        {
                            stream = null;
                            streamcopy.Position = 0;

                            int id = 0;
                            long length = streamcopy.Length;
                            int multiplier = 1;
                            for (long len = 0; len < length && !progress.Cancel; )
                            {
                                byte[] data = new byte[65536 * multiplier];
                                int read = streamcopy.Read(data, 0, (int)Math.Min(65536 * multiplier, length - len));
                                if (read == 0)
                                {
                                    throw new System.IO.EndOfStreamException("Stream shorter than expected.");
                                }

                                if (read < data.Length)
                                {
                                    byte[] tmp = new byte[read];
                                    Array.Copy(data, tmp, read);
                                    data = tmp;
                                }

                                long duration = DateTime.UtcNow.Ticks;
                                client.AddPageToBlob(this.meetingToken, this.sessionID, this.clientID, this.blobAddress, this.blobSharedSignature, data, id, len + read >= length);
                                duration = ((DateTime.UtcNow.Ticks - duration) / 10000) + 1;

                                multiplier = Math.Max(1, Math.Min(16, 10000 / (int)duration));

                                len += read;
                                id++;

                                progress.UploadedSize = len;
                                OnUploadProgress(progress);
                            }
                        }

                        if (progress.Cancel)
                        {
                            e = new UploadCompletedArgs(null, true, o);
                        }
                        else
                        {
                            e.BlobAddress = this.blobAddress;
                        }
                    }
                    catch (Exception ex)
                    {
                        e = new UploadCompletedArgs(ex, false, o);
                    }
                    finally
                    {
                        client.Dispose();
                    }

                    OnUploadCompleted(e);
                }),
            state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void OnUploadCompleted(UploadCompletedArgs e)
        {
            if (this.UploadCompleted != null)
            {
                this.UploadCompleted(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progress"></param>
        private void OnUploadProgress(UploadProgressArgs progress)
        {
            if (this.UploadProgress != null)
            {
                this.UploadProgress(this, progress);
            }
        }
    }
}
