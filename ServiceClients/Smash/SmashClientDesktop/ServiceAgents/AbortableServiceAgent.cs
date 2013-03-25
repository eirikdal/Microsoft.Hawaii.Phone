// ----------------------------------------------------------
// <copyright file="AbortableServiceAgent.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Threading;

    using Microsoft.Hawaii;

    using Microsoft.Hawaii.Smash.Client.Contracts;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class AbortableServiceAgent<T> : ServiceAgent<T> where T : AbortableServiceResult, new()
    {
        /// <summary>
        /// Initializes a new instance of the AbortableServiceAgent class
        /// </summary>
        /// <param name="requestMethod"></param>
        /// <param name="stateObject"></param>
        public AbortableServiceAgent(HttpMethod requestMethod, object stateObject)
            : base(requestMethod, stateObject)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onComplete"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "returning the IAsyncResult to the caller. Caller needs to call .Close on AsyncWaitHandle")]
        public IAsyncResult AbortableProcessRequest(OnCompleteDelegate onComplete, object asyncState)
        {
            AsyncResult asyncResult = new AsyncResult(onComplete, asyncState);

            this.ProcessRequest(new OnCompleteDelegate(asyncResult.OnCompleted));

            return asyncResult;
        }

        /// <summary>
        /// 
        /// </summary>
        private class AsyncResult : IAsyncResult, IAbortableAsyncResult, IDisposable
        {
            /// <summary>
            /// 
            /// </summary>
            private object myLock = new object();

            /// <summary>
            /// 
            /// </summary>
            private OnCompleteDelegate onComplete;

            /// <summary>
            /// 
            /// </summary>
            private object asyncState;

            /// <summary>
            /// 
            /// </summary>
            private ManualResetEvent asyncWaitHandle;

            /// <summary>
            /// Initializes a new instance of the AsyncResult class
            /// </summary>
            /// <param name="onComplete"></param>
            /// <param name="asyncState"></param>
            public AsyncResult(OnCompleteDelegate onComplete, object asyncState)
            {
                this.onComplete = onComplete;
                this.asyncState = asyncState;
            }

            /// <summary>
            /// 
            /// </summary>
            public object AsyncState
            {
                get
                {
                    return this.asyncState;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public System.Threading.WaitHandle AsyncWaitHandle
            {
                get
                {
                    lock (this.myLock)
                    {
                        if (this.asyncWaitHandle == null)
                        {
                            this.asyncWaitHandle = new ManualResetEvent(this.IsCompleted);
                        }

                        return this.asyncWaitHandle;
                    }
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public bool CompletedSynchronously
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public bool IsCompleted
            {
                get
                {
                    lock (this.myLock)
                    {
                        return this.onComplete == null ? true : false;
                    }
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="response"></param>
            public void OnCompleted(T response)
            {
                lock (this.myLock)
                {
                    if (this.onComplete != null)
                    {
                        this.onComplete(response);
                        this.onComplete = null;

                        if (this.asyncWaitHandle != null)
                        {
                            this.asyncWaitHandle.Set();
                        }
                    }
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="timedOut"></param>
            public void Abort(bool timedOut)
            {
                lock (this.myLock)
                {
                    T response = new T();

                    response.Aborted = true;
                    response.StateObject = AsyncState;
                    if (timedOut)
                    {
                        response.Exception = new TimeoutException();
                    }

                    this.OnCompleted(response);
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public void Dispose()
            {
                if (this.asyncWaitHandle != null)
                {
                    using (ManualResetEvent dispose = this.asyncWaitHandle)
                    {
                        this.asyncWaitHandle = null;
                    }
                }
            }
        }
    }
}