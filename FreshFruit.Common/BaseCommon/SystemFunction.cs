using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshFruit.Common.BaseCommon
{
    public static class SystemFunction
    {
        public static Func<long> currentTimeFunc = new Func<long>(SystemFunction.InternalCurrentTimeMillis);

        private static readonly System.DateTime Jan1st1970 = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return SystemFunction.currentTimeFunc();
        }

        public static System.IDisposable StubCurrentTime(Func<long> func)
        {
            SystemFunction.currentTimeFunc = func;
            return new DisposableAction(delegate
            {
                SystemFunction.currentTimeFunc = new Func<long>(SystemFunction.InternalCurrentTimeMillis);
            });
        }

        public static System.IDisposable StubCurrentTime(long millis)
        {
            SystemFunction.currentTimeFunc = (() => millis);
            return new DisposableAction(delegate
            {
                SystemFunction.currentTimeFunc = new Func<long>(SystemFunction.InternalCurrentTimeMillis);
            });
        }

        private static long InternalCurrentTimeMillis()
        {
            return (long)(System.DateTime.UtcNow - SystemFunction.Jan1st1970).TotalMilliseconds;
        }
    }
}
