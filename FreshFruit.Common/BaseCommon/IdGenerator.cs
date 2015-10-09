using System;
using System.Runtime.CompilerServices;

namespace FreshFruit.Common.BaseCommon
{
    public class IdGenerator
    {
        private static readonly IdWorker IdWorker = new IdWorker(31L, 31L, 0L);

        public static long GenerateId()
        {
            return IdGenerator.IdWorker.NextId();
        }

        [STAThread]
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static string CreateSixteenRandomString()
        {
            double totalSeconds = DateTime.Now.Subtract(new DateTime(2010, 1, 1)).TotalSeconds;
            long num = (long)(totalSeconds * 10000000.0);
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int num2 = random.Next(1000000, 9999999);
            return (num + (long)num2).ToString().Substring(0, 16);
        }
    }
}
