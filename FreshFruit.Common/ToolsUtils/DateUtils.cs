using System;

namespace FreshFruit.Common.ToolsUtils
{
    public class DateUtils
    {

        /// <summary>
        /// 日期转换时间戳
        /// </summary>
        /// <param name="time">传入时间</param>
        /// 13 BIT-- return Convert.ToInt32(ts.TotalMilliseconds);
        /// <returns>int - UnixStamp</returns>
        public static int UnixStamp(DateTime time)
        {
            TimeSpan ts = time - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToInt32(ts.TotalSeconds);
        }

        /// <summary>
        /// 当前时间转换时间戳
        /// </summary>
        /// <returns>int - UnixStamp</returns>
        public static int UnixStamp()
        {
            return UnixStamp(DateTime.Now);
        }

        /// <summary>
        /// 时间戳转换日期
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns>Datetime</returns>
        public static DateTime From_UnixTime(long timeStamp)
        {
            return Convert.ToDateTime("1970-01-01 00:00:00").AddSeconds(timeStamp);
        }



        /// <summary>
        /// 根据生日获取星座
        /// </summary>
        /// <param name="birthday">生日</param>
        /// <returns></returns>
        public static string GetSigns(DateTime birthday)
        {
            float birthdayF = 0.00F;
            string strday;

            if (birthday.Day < 10)
            {
                strday = "0" + birthday.Day.ToString();
            }
            else
            {
                strday = birthday.Day.ToString();
            }

            if (birthday.Month == 1 && birthday.Day < 20)
            {
                birthdayF = float.Parse(string.Format("13.{0}", strday));
            }
            else
            {
                birthdayF = float.Parse(string.Format("{0}.{1:2}", birthday.Month, strday));
            }
            float[] signs_scope = { 1.20F, 2.19F, 3.21F, 4.20F, 5.21F, 6.22F, 7.23F, 8.23F, 9.23F, 10.24F, 11.23F, 12.22F, 13.20F };
            string[] signs_list = { "水瓶座", "双鱼座", "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "处女座", "天秤座", "天蝎座", "射手座", "摩羯座" };
            string ret_Signs = "我凌乱了，你发现了一个新的星座";

            for (int i = 0; i < signs_scope.Length - 1; i++)
            {
                if (signs_scope[i] <= birthdayF && signs_scope[i + 1] > birthdayF)
                {
                    ret_Signs = signs_list[i];
                    break;
                }
            }
            return ret_Signs;
        }
    }
}
