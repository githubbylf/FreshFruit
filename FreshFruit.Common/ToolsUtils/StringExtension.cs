using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FreshFruit.Common.ToolsUtils
{
    /// <summary>
    /// String Extension 
    /// </summary>
    public static class StringExtension
    {
        private static Regex emailRegex = new Regex("^[\\w\\.]+([-]\\w+)*@[A-Za-z0-9-_]+[\\.][A-Za-z0-9-_]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex urlRegex = new Regex("^http(s)?://([\\w-]+\\.)+[\\w-]+(:\\d+)?(/[\\w- ./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex ipRegex = new Regex("^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex mobilePhoneRegex = new Regex("^1[3,5,7,8][0-9]\\d{8}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex IDRegex = new Regex("^\\d{17}[\\d|X]|\\d{15}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsEmail(this string strEmail)
        {
            bool result = false;
            if (strEmail.IsNotEmpty())
            {
                result = StringExtension.emailRegex.IsMatch(strEmail);
            }
            return result;
        }

        public static bool IsURL(this string strUrl)
        {
            bool result = false;
            if (strUrl.IsNotEmpty())
            {
                result = StringExtension.urlRegex.IsMatch(strUrl);
            }
            return result;
        }

        public static bool IsIP(this string ip)
        {
            bool result = false;
            if (ip.IsNotEmpty())
            {
                result = StringExtension.ipRegex.IsMatch(ip);
            }
            return result;
        }

        public static bool IsPhoneNumber(this string value)
        {
            bool result = false;
            if (value.IsNotEmpty())
            {
                result = StringExtension.mobilePhoneRegex.IsMatch(value);
            }
            return result;
        }

        public static bool IsIdentityCardNumber(this string value)
        {
            bool result = false;
            if (value.IsNotEmpty())
            {
                result = StringExtension.IDRegex.IsMatch(value);
            }
            return result;
        }

        public static int ToSafeInt(this string value)
        {
            int result = 0;
            if (value.IsNotEmpty())
            {
                int.TryParse(value, out result);
            }
            return result;
        }

        public static System.DateTime ToSafeDateTime(this string value)
        {
            System.DateTime minValue = System.DateTime.MinValue;
            if (value.IsNotEmpty())
            {
                System.DateTime.TryParse(value, out minValue);
            }
            return minValue;
        }

        public static long ToSafeLong(this string value)
        {
            long result = 0L;
            if (value.IsNotEmpty())
            {
                long.TryParse(value, out result);
            }
            return result;
        }

        public static double ToSafeDouble(this string value)
        {
            double result = 0.0;
            if (value.IsNotEmpty())
            {
                double.TryParse(value, out result);
            }
            return result;
        }

        public static float ToSafeFloat(this string value)
        {
            float result = 0f;
            if (value.IsNotEmpty())
            {
                float.TryParse(value, out result);
            }
            return result;
        }

        public static decimal ToSafeDecimal(this string value)
        {
            decimal result = 0m;
            if (value.IsNotEmpty())
            {
                decimal.TryParse(value, out result);
            }
            return result;
        }
    }
}
