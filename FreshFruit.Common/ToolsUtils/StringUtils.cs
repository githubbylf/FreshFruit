using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FreshFruit.Common.ToolsUtils
{
    public class StringUtils
    {
        private static System.Text.Encoding gb2312 = System.Text.UnicodeEncoding.GetEncoding("gb2312");

        private static char[] strChar = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZ".ToCharArray();//去掉0和O
        public static List<int> ChangeIntList(string str)
        {
            List<int> ss = new List<int>();
            MatchCollection mc = Regex.Matches(str, "\\d+");
            foreach (Match m in mc)
            {
                ss.Add(Convert.ToInt32(m.Value));
            }
            return ss;
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static KeyValueConfigurationCollection ReadConfig(string configfilename)
        {
            ExeConfigurationFileMap file = new ExeConfigurationFileMap();
            file.ExeConfigFilename = configfilename;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(file, ConfigurationUserLevel.None);
            return config.AppSettings.Settings;
            //Console.WriteLine(config.AppSettings.Settings["bonus"].Value);
        }
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string RndString(int len)
        {

            StringBuilder str = new StringBuilder();
            Random rnd = new Random(GetRandomSeed());
            for (int i = 0; i < len; i++)
            {
                str.Append(strChar[rnd.Next() % 34]);
            }
            return str.ToString();
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RndInteger(int max)
        {
            Random rnd = new Random(GetRandomSeed());
            return rnd.Next(1, max);
        }
        /// <summary>
        /// 生成随机种子
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 检测手机号码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(string mobile)
        {
            return Regex.IsMatch(mobile, @"^((13\d{9})|(14\d{9})|(15\d{9})|(18\d{9}))$", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 是否是移动的手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsChinaMobile(string mobile)
        {
            return Regex.IsMatch(mobile, @"^(13[4-9]\d{8})|159\d{8}$", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 是否是联通的手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsUnionMobile(string mobile)
        {
            return Regex.IsMatch(mobile, @"^13[0-3]\d{8}$", RegexOptions.IgnoreCase);
        }
        //        public static bool IsMobile(string mobile)
        //        {
        //            return Regex.IsMatch(mobile,@"(13\d{9})|(1060d{10,11})$",RegexOptions.IgnoreCase);
        //        }
        /// <summary>
        /// 获取文件名后缀
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSuffix(string str)
        {
            string s = Regex.Replace(str, @".+\.", string.Empty, RegexOptions.IgnoreCase);
            if (s == str)
            {
                return "";
            }
            return s;
        }
        /// <summary>
        /// 获取页面名称
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPageName(string str)
        {
            return Regex.Replace(str, @".+/", string.Empty, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 检测合法的账户名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckLegalUName(string name)
        {
            return IsMatch(new Regex(@"^[a-zA-Z][a-zA-Z0-9]{4,19}$"), name, 0);
        }
        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="sinput"></param>
        /// <returns></returns>
        public static bool IsInteger(string sinput)
        {
            return IsMatch(new Regex(@"^\d+(\.\d+)?$"), sinput, 0);
        }
        /// <summary>
        /// 是不是邮件地址
        /// </summary>
        /// <param name="sinput"></param>
        /// <returns></returns>
        public static bool IsEmail(string sinput)
        {
            return Regex.IsMatch(sinput, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 是否是ZipCode
        /// </summary>
        /// <param name="sinput"></param>
        /// <returns></returns>
        public static bool IsZipCode(string sinput)
        {
            return Regex.IsMatch(sinput, @"^\d{6}$", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 转换EnCode码
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static string PackParm(string parms)
        {
            return HttpUtility.UrlEncode(parms, gb2312);

        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static string UnPackParm(string parms)
        {
            return HttpUtility.UrlDecode(parms, gb2312);
        }
        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="sinput"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private static bool IsMatch(Regex reg, string sinput, int pos)
        {
            if (sinput == String.Empty || sinput == null) return false;
            return reg.IsMatch(sinput, pos);
        }
        /// <summary>
        /// 换算文件大小
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ConvertFileSize(Decimal d)
        {
            return ConvertFileSize(Convert.ToInt32(d));
        }
        /// <summary>
        /// 返回文件大小
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string ConvertFileSize(int i)
        {
            int m = 0, k = 0;
            m = i / 1048576;
            if (m > 0)
            {
                return String.Format("{0:n2}M", (double)i / 1048576);
            }
            k = i / 1024;
            if (k > 0)
            {
                return String.Format("{0:n2}K", (double)i / 1024);
            }
            return String.Format("{0}Bytes", i);

        }
        /// <summary>
        /// 将Ip地址转换为Long
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long IpToLong(string ip)
        {
            string[] strs = ip.Split('.');
            long lip = 0;
            for (int i = 0; i < 4; i++)
            {
                lip = Convert.ToByte(strs[i]) | lip << 8;
            }
            return lip;
        }

        /// <summary>
        /// 转换HTML代码，将尖括号转换为&lt;形式，回车换行转为等
        /// </summary>
        /// <param name="exp1">要转换的字符串</param>
        /// <returns>返回过滤后的字符串</returns>
        public static String TextToHtml(string exp1)
        {
            String exp2;
            exp2 = exp1.Replace("<", "&lt;");
            exp2 = exp2.Replace(">", "&gt;");
            exp2 = exp2.Replace("'", "''");
            //exp2=exp2.Replace("\"","\\\"");
            exp2 = exp2.Replace(" ", "&nbsp;");
            exp2 = exp2.Replace("\n", "<br>");
            exp2 = exp2.Replace("\r", "");
            return exp2;
        }
        /// <summary>
        /// 转换HTML代码，将尖括号转换为&lt;形式，但不转换换行符
        /// </summary>
        /// <param name="exp1">要转换的字符串</param>
        /// <returns>返回过滤后的字符串</returns>
        public static string HtmlToText(string exp1)
        {
            String exp2;
            exp2 = exp1.Replace("&lt;", "<");
            exp2 = exp2.Replace("&gt;", ">");
            exp2 = exp2.Replace("''", "'");
            //exp2=exp2.Replace("\\\"","\"");    
            exp2 = exp2.Replace("&nbsp;", " ");
            exp2 = exp2.Replace("<br>", "\n");
            exp2 = exp2.Replace("\r", "");
            return exp2;
        }
        /// <summary>
        /// 将英文星期转为中文
        /// </summary>
        /// <param name="englishWeek"></param>
        /// <returns></returns>
        public static string GetChineseWeek(string englishWeek)
        {
            string cn = string.Empty;
            switch (englishWeek.ToLower())
            {
                case "monday":
                    cn = "星期一";
                    break;
                case "tuesday":
                    cn = "星期二";
                    break;
                case "wednesday":
                    cn = "星期三";
                    break;
                case "thursday":
                    cn = "星期四";
                    break;
                case "friday":
                    cn = "星期五";
                    break;
                case "saturday":
                    cn = "星期六";
                    break;
                case "sunday":
                    cn = "星期日";
                    break;
            }
            return cn;
        }
        /// <summary>
        /// 清除HTML元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>结果</returns>
        public static string ClearHtml(string str)
        {
            return Regex.Replace(str, @"\<[^\>]*?\>", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 获取初始密码
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public static string GetInitPassWord(string cardid)
        {
            uint key = 0;
            char[] chars = cardid.ToCharArray();
            int len = chars.Length;
            for (int i = 0; i < len; i++)
            {
                switch (i % 4)
                {
                    case 0:
                        key += (uint)chars[len - i - 1];
                        break;
                    case 1:
                        key = (uint)(key * (int)chars[len - i - 1]);
                        break;
                    case 2:
                        key = key << (Convert.ToInt32(chars[len - i - 1]) - 48);
                        break;
                    case 3:
                        key = key >> (Convert.ToInt32(chars[len - i - 1]) - 48);
                        break;
                }
            }
            string returnvalue = String.Format("{0:D6}", key);
            len = returnvalue.Length;
            if (len > 6)
            {
                return returnvalue.Substring(len - 6, 6);
            }
            return returnvalue;
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        public static bool IsNullOrEmpty(string str)
        {
            if (str == null) return true;
            if (str.Trim().Length == 0) return true;

            return false;
        }
    }
}
