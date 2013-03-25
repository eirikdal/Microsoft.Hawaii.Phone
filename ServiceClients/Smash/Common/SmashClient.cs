// ----------------------------------------------------------
// <copyright file="SmashClient.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Microsoft.Hawaii;
    using Microsoft.Hawaii.Smash.Client.Common;
    using Microsoft.Hawaii.Smash.Client.Contracts;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class SmashClient : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private const int DefaultTimeout = 30000;

        /// <summary>
        /// 
        /// </summary>
        private const int WaitAfterTimeout = 5000;

        /// <summary>
        /// 
        /// </summary>
        private const int LongPollTimeout = 10000;

        /// <summary>
        /// 
        /// </summary>
        private int useLongPoll;

        /// <summary>
        /// The client identity.
        /// </summary>
        private ClientIdentity identity;

        /// <summary>
        /// 
        /// </summary>
        private AutoResetEvent abortGetRows;

        /// <summary>
        /// 
        /// </summary>
        private AutoResetEvent abortGetRowsAck;

        /// <summary>
        /// Initializes a new instance of the SmashClient class.
        /// </summary>
        private SmashClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SmashClient class.
        /// </summary>
        /// <param name="identity">The application identity.</param>
        public SmashClient(ClientIdentity identity)
            : this()
        {
            this.identity = identity.Copy();
            this.UseLongPoll = true;
            this.abortGetRows = new AutoResetEvent(false);
            this.abortGetRowsAck = new AutoResetEvent(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal bool UseLongPoll
        {
            get
            {
                return Interlocked.CompareExchange(ref this.useLongPoll, 0, 0) == 0;
            }

            set
            {
                Interlocked.Exchange(ref this.useLongPoll, value == true ? 0 : 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="clientID"></param>
        /// <param name="lastKnownRowTime"></param>
        /// <param name="rowsOut"></param>
        /// <returns></returns>
        public bool GetRows(Guid meetingToken, Guid sessionID, Guid clientID, long lastKnownRowTime, out DataRow_Wire[] rowsOut)
        {
            RequestState<GetRowsResponse> state = new RequestState<GetRowsResponse>();
            Exception exception;
            bool aborted = false;

            try
            {
                rowsOut = null;

                IAsyncResult result = SmashClientREST.GetRowsAsync(
                    this.identity,
                    meetingToken,
                    sessionID,
                    clientID,
                    lastKnownRowTime,
                    UseLongPoll ? LongPollTimeout : 0,
                    true,
                    new ServiceAgent<GetRowsResponse>.OnCompleteDelegate(this.SmashServiceClient_GetRowsCompleted),
                    state);

                int signal;
                if ((signal = WaitHandle.WaitAny(new WaitHandle[] { state.Done, this.abortGetRows }, this.Timeout != 0 ? this.Timeout : DefaultTimeout)) != 0)
                {
                    bool timedOut = signal == -1 ? true : false;
                    SmashClientREST.AbortRequest(result, timedOut);

                    if (timedOut)
                    {
                        // This wait will cause the service side adjust the max transfer size due to longer round trip time
                        System.Threading.Thread.Sleep(WaitAfterTimeout);
                    }
                    else
                    {
                        this.abortGetRowsAck.Set();
                    }
                }

                exception = state.Response.Exception;
                aborted = state.Response.Aborted;
                if (exception == null && !aborted)
                {
                    rowsOut = state.Response.RowsOut;
                }
                else
                {
                    rowsOut = new DataRow_Wire[0];
                }
            }
            catch (Exception inner)
            {
                throw new SmashException("Error while getting data from session", inner);
            }
            finally
            {
                state.Dispose();
            }

            if (exception != null)
            {
                throw exception;
            }

            return aborted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="clientID"></param>
        /// <param name="rows"></param>
        /// <param name="onComplete"></param>
        /// <param name="state"></param>
        public void SendRowsAsync(Guid meetingToken, Guid sessionID, Guid clientID, IEnumerable<DataRow_Wire> rows, ServiceAgent<SendRowsResponse>.OnCompleteDelegate onComplete, object state)
        {
            try
            {
                IAsyncResult result = SmashClientREST.SendRowsAsync(
                    this.identity,
                    meetingToken,
                    sessionID,
                    clientID,
                    rows,
                    onComplete,
                    state);
            }
            catch (Exception inner)
            {
                throw new SmashException("Error while joining session", inner);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meetingToken"></param>
        /// <param name="sessionID"></param>
        /// <param name="clientID"></param>
        /// <param name="blobAddress"></param>
        /// <param name="blobSharedSignature"></param>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <param name="close"></param>
        public void AddPageToBlob(Guid meetingToken, Guid sessionID, Guid clientID, string blobAddress, string blobSharedSignature, byte[] page, int id, bool close)
        {
            RequestState<AddPageToBlobResponse> state = new RequestState<AddPageToBlobResponse>();
            Exception exception;

            try
            {
                IAsyncResult result = SmashClientREST.AddPageToBlobAsync(
                    this.identity,
                    meetingToken,
                    sessionID,
                    clientID,
                    blobAddress,
                    blobSharedSignature,
                    page,
                    id,
                    close,
                    new ServiceAgent<AddPageToBlobResponse>.OnCompleteDelegate(this.SmashServiceClient_AddPageToBlobCompleted),
                    state);

                if (!state.Done.WaitOne(this.Timeout != 0 ? this.Timeout : DefaultTimeout))
                {
                    SmashClientREST.AbortRequest(result, true);
                }

                exception = state.Response.Exception;
            }
            catch (Exception inner)
            {
                throw new SmashException("Error while creating blob", inner);
            }
            finally
            {
                state.Dispose();
            }

            if (exception != null)
            {
                throw exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this.abortGetRows != null)
            {
                this.abortGetRows.Dispose();
                this.abortGetRows = null;
            }
            if (this.abortGetRowsAck != null)
            {
                this.abortGetRowsAck.Dispose();
                this.abortGetRowsAck = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void AbortGetRows()
        {
            this.abortGetRows.Set();
            this.abortGetRowsAck.WaitOne();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        private void SmashServiceClient_GetRowsCompleted(GetRowsResponse response)
        {
            RequestState<GetRowsResponse> state = response.StateObject as RequestState<GetRowsResponse>;
            state.Response = response;
            state.SetDone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        private void SmashServiceClient_AddPageToBlobCompleted(AddPageToBlobResponse response)
        {
            RequestState<AddPageToBlobResponse> state = response.StateObject as RequestState<AddPageToBlobResponse>;
            state.Response = response;
            state.SetDone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class RequestState<T> : IDisposable
        {
            /// <summary>
            /// 
            /// </summary>
            private AutoResetEvent done;

            /// <summary>
            /// Initializes a new instance of the RequestState class.
            /// </summary>
            public RequestState()
            {
                this.done = new AutoResetEvent(false);
            }

            /// <summary>
            /// 
            /// </summary>
            public T Response { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public AutoResetEvent Done
            {
                get
                {
                    return this.done;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public void SetDone()
            {
                if (this.done != null)
                {
                    this.done.Set();
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public void Dispose()
            {
                if (this.done != null)
                {
                    this.done.Dispose();
                    this.done = null;
                }
            }
        }
    }
}
