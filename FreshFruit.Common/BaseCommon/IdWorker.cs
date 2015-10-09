using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshFruit.Common.BaseCommon
{
    public class IdWorker
    {
        public const long Twepoch = 1288834974657L;

        private const int WorkerIdBits = 5;

        private const int DatacenterIdBits = 5;

        private const int SequenceBits = 12;

        private const long MaxWorkerId = 31L;

        private const long MaxDatacenterId = 31L;

        private const int WorkerIdShift = 12;

        private const int DatacenterIdShift = 17;

        public const int TimestampLeftShift = 22;

        private const long SequenceMask = 4095L;

        private long _sequence;

        private long _lastTimestamp = -1L;

        private readonly object _lock = new object();

        public long WorkerId
        {
            get;
            protected set;
        }

        public long DatacenterId
        {
            get;
            protected set;
        }

        public long Sequence
        {
            get
            {
                return this._sequence;
            }
            internal set
            {
                this._sequence = value;
            }
        }

        public IdWorker(long workerId, long datacenterId, long sequence = 0L)
        {
            this.WorkerId = workerId;
            this.DatacenterId = datacenterId;
            this._sequence = sequence;
            if (workerId > 31L || workerId < 0L)
            {
                throw new System.ArgumentException(string.Format("worker Id can't be greater than {0} or less than 0", 31L));
            }
            if (datacenterId > 31L || datacenterId < 0L)
            {
                throw new System.ArgumentException(string.Format("datacenter Id can't be greater than {0} or less than 0", 31L));
            }
        }

        public virtual long NextId()
        {
            long result;
            lock (this._lock)
            {
                long num = this.TimeGen();
                if (num < this._lastTimestamp)
                {
                    throw new InvalidSystemClock(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds", this._lastTimestamp - num));
                }
                if (this._lastTimestamp == num)
                {
                    this._sequence = (this._sequence + 1L & 4095L);
                    if (this._sequence == 0L)
                    {
                        num = this.TilNextMillis(this._lastTimestamp);
                    }
                }
                else
                {
                    this._sequence = 0L;
                }
                this._lastTimestamp = num;
                long num2 = num - 1288834974657L << 22 | this.DatacenterId << 17 | this.WorkerId << 12 | this._sequence;
                result = num2;
            }
            return result;
        }

        protected virtual long TilNextMillis(long lastTimestamp)
        {
            long num;
            for (num = this.TimeGen(); num <= lastTimestamp; num = this.TimeGen())
            {
            }
            return num;
        }

        protected virtual long TimeGen()
        {
            return SystemFunction.CurrentTimeMillis();
        }
    }
}
