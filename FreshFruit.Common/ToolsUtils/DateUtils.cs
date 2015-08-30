using System;

namespace FreshFruit.Common.ToolsUtils
{
    public class DateUtils
    {
        /// <summary>  
        /// Get current timestamp
        /// </summary>  
        /// <param name="bflag">Get the 10 time stamp for the truth, and get 13 time stamps for the false</param>  
        /// <returns></returns>  
        public string GetTimeStamp(bool bflag)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
            {
                return ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            }
            else
            {
                return ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            }
        }


    }
}
