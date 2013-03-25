// ----------------------------------------------------------
// <copyright file="RowDeduper.cs" company="Microsoft">
//     Copyright (tableClient) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------

namespace Microsoft.Hawaii.Smash.Client.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
    internal class RowDeduper
    {
        /// <summary>
        /// 
        /// </summary>
        private int lastUsedTimeInSeconds;

        /// <summary>
        /// 
        /// </summary>
        private long transferLastTime;

        /// <summary>
        /// 
        /// </summary>
        private long transferMax;

        /// <summary>
        /// 
        /// </summary>
        private long base2010;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<Guid, long> known;

        /// <summary>
        /// 
        /// </summary>
        private Queue<KeyValuePair<long, Guid>> history;

        /// <summary>
        /// Initializes a new instance of the RowDeduper class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keepHistory"></param>
        public RowDeduper(string id, bool keepHistory)
        {
            this.ID = id;
            this.known = new Dictionary<Guid, long>();
            if (keepHistory)
            {
                this.history = new Queue<KeyValuePair<long, Guid>>();
            }

            this.base2010 = (new DateTime(2010, 1, 1)).Ticks;
            this.LastUsedTimeInSeconds = this.NowInSeconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Age
        {
            get
            {
                return this.LastUsedTimeInSeconds - this.NowInSeconds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long TransferLastTime
        {
            set
            {
                this.transferLastTime = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TransferMax
        {
            get
            {
                return (int)Math.Min(Math.Max(this.transferMax, 64000), 2000000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int LastUsedTimeInSeconds
        {
            get
            {
                return Interlocked.CompareExchange(ref this.lastUsedTimeInSeconds, 0, 0);
            }

            set
            {
                Interlocked.Exchange(ref this.lastUsedTimeInSeconds, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int NowInSeconds
        {
            get
            {
                long now = DateTime.UtcNow.Ticks;
                return (int)((now - this.base2010) / 10000000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public bool Known(Guid guid, long timeStamp)
        {
            long t;
            if (this.known.TryGetValue(guid, out t))
            {
                // A better version of same record is known
                if (t >= timeStamp)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="timeStamp"></param>
        public void Add(Guid guid, long timeStamp)
        {
            this.known[guid] = timeStamp;
            if (this.history != null)
            {
                this.history.Enqueue(new KeyValuePair<long, Guid>(timeStamp, guid));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SignalTransferStart()
        {
            if (this.transferLastTime != 0)
            {
                int roundTripTime = (int)(((DateTime.UtcNow.Ticks - this.transferLastTime) / 10000) + 1);
                this.transferMax = this.transferMax * 2000 / roundTripTime;
                this.transferMax = Math.Min(Math.Max(this.transferMax, 256000), 2000000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastKnownRowTime"></param>
        public void ApplyTTL(long lastKnownRowTime)
        {
            if (this.history == null)
            {
                throw new InvalidOperationException("Cannot call ApplyTTL if no history is recorded");
            }

            this.LastUsedTimeInSeconds = this.LastUsedTimeInSeconds - this.NowInSeconds;
            if (lastKnownRowTime == 0)
            {
                // Restarted
                this.known = new Dictionary<Guid, long>();
                this.history = new Queue<KeyValuePair<long, Guid>>();
                this.transferMax = 256000;   // Max 256KB for first request
            }
            else
            {
                while (true)
                {
                    // Keep at least the last 10 items to avoid unnecessary roundtrips
                    if (this.history.Count > 10 && this.history.Peek().Key < lastKnownRowTime)
                    {
                        if (this.known.ContainsKey(this.history.Dequeue().Value))
                        {
                            this.known.Remove(this.history.Dequeue().Value);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
