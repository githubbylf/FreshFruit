using FreshFruit.Common.BaseCommon;
using FreshFruit.Common.ToolsUtils;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace FreshFruit.Common.Utility
{
    public static class Utils
    {
        private static IdWorker m_IdWorker = new IdWorker(31L, 31L, 0L);

        private static Regex m_HtmlTagsRegex = new Regex("<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static string m_IP = string.Empty;

        private static string[] browerNames = new string[]
        {
            "MSIE",
            "Firefox",
            "Opera",
            "Netscape",
            "Safari",
            "Lynx",
            "Konqueror",
            "Mozilla"
        };

        public static long GenerateId()
        {
            return Utils.m_IdWorker.NextId();
        }

        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length *= -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex -= length;
                    }
                }
                if (startIndex > str.Length)
                {
                    return "";
                }
            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                if (length + startIndex <= 0)
                {
                    return "";
                }
                length += startIndex;
                startIndex = 0;
            }
            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }
            return str.Substring(startIndex, length);
        }

        public static string CutString(string str, int startIndex)
        {
            return Utils.CutString(str, startIndex, str.Length);
        }

        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            strPath = strPath.Replace("/", "\\");
            if (strPath.StartsWith("\\"))
            {
                strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart(new char[]
                {
                    '\\'
                });
            }
            return System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, strPath);
        }

        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        public static void ResponseFile(string filepath, string filename, string filetype)
        {
            System.IO.Stream stream = null;
            byte[] buffer = new byte[10000];
            try
            {
                stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                long num = stream.Length;
                HttpContext.Current.Response.ContentType = filetype;
                if (HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].IndexOf("MSIE") > -1)
                {
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Utils.UrlEncode(filename.Trim()).Replace("+", " "));
                }
                else
                {
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename.Trim());
                }
                while (num > 0L)
                {
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        int num2 = stream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, num2);
                        HttpContext.Current.Response.Flush();
                        buffer = new byte[10000];
                        num -= (long)num2;
                    }
                    else
                    {
                        num = -1L;
                    }
                }
            }
            catch (System.Exception ex)
            {
                HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            HttpContext.Current.Response.End();
        }

        public static bool IsImgFilename(string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
            {
                return false;
            }
            string a = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            return a == "jpg" || a == "jpeg" || a == "png" || a == "bmp" || a == "gif";
        }

        public static string MD5(string str)
        {
            byte[] array = System.Text.Encoding.UTF8.GetBytes(str);
            array = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(array);
            string text = "";
            for (int i = 0; i < array.Length; i++)
            {
                text += array[i].ToString("x").PadLeft(2, '0');
            }
            return text;
        }

        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return Utils.GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        public static string GetUnicodeSubString(string str, int len, string p_TailString)
        {
            str = str.TrimEnd(new char[0]);
            string result = string.Empty;
            int byteCount = System.Text.Encoding.Default.GetByteCount(str);
            int length = str.Length;
            int num = 0;
            int num2 = 0;
            if (byteCount > len)
            {
                for (int i = 0; i < length; i++)
                {
                    if (System.Convert.ToInt32(str.ToCharArray()[i]) > 255)
                    {
                        num += 2;
                    }
                    else
                    {
                        num++;
                    }
                    if (num > len)
                    {
                        num2 = i;
                        break;
                    }
                    if (num == len)
                    {
                        num2 = i + 1;
                        break;
                    }
                }
                if (num2 >= 0)
                {
                    result = str.Substring(0, num2) + p_TailString;
                }
            }
            else
            {
                result = str;
            }
            return result;
        }

        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string text = p_SrcString;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(p_SrcString);
            char[] chars = System.Text.Encoding.UTF8.GetChars(bytes);
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                if ((c > 'ࠀ' && c < '一') || (c > '가' && c < '힣'))
                {
                    string result;
                    if (p_StartIndex >= p_SrcString.Length)
                    {
                        result = "";
                    }
                    else
                    {
                        result = p_SrcString.Substring(p_StartIndex, (p_Length + p_StartIndex > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                    }
                    return result;
                }
            }
            if (p_Length >= 0)
            {
                byte[] bytes2 = System.Text.Encoding.Default.GetBytes(p_SrcString);
                if (bytes2.Length > p_StartIndex)
                {
                    int num = bytes2.Length;
                    if (bytes2.Length > p_StartIndex + p_Length)
                    {
                        num = p_Length + p_StartIndex;
                    }
                    else
                    {
                        p_Length = bytes2.Length - p_StartIndex;
                        p_TailString = "";
                    }
                    int num2 = p_Length;
                    int[] array = new int[p_Length];
                    int num3 = 0;
                    for (int j = p_StartIndex; j < num; j++)
                    {
                        if (bytes2[j] > 127)
                        {
                            num3++;
                            if (num3 == 3)
                            {
                                num3 = 1;
                            }
                        }
                        else
                        {
                            num3 = 0;
                        }
                        array[j] = num3;
                    }
                    if (bytes2[num - 1] > 127 && array[p_Length - 1] == 1)
                    {
                        num2 = p_Length + 1;
                    }
                    byte[] array2 = new byte[num2];
                    System.Array.Copy(bytes2, p_StartIndex, array2, 0, num2);
                    text = System.Text.Encoding.Default.GetString(array2);
                    text += p_TailString;
                }
            }
            return text;
        }

        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        public static string GetSpacesString(int spacesCount)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < spacesCount; i++)
            {
                stringBuilder.Append(" &nbsp;&nbsp;");
            }
            return stringBuilder.ToString();
        }

        public static string GetEmailHostName(string strEmail)
        {
            if (strEmail.IndexOf("@") < 0)
            {
                return "";
            }
            return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
        }

        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, "[A-Za-z0-9\\+\\/\\=]");
        }

        public static string GetFilename(string url)
        {
            if (url == null)
            {
                return "";
            }
            string[] array = url.Split(new char[]
            {
                '/'
            });
            return array[array.Length - 1].Split(new char[]
            {
                '?'
            })[0];
        }

        public static string GetDate(string datetimestr, string replacestr)
        {
            if (datetimestr == null)
            {
                return replacestr;
            }
            if (datetimestr.Equals(""))
            {
                return replacestr;
            }
            try
            {
                datetimestr = System.Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;
        }

        public static string GetTime()
        {
            return System.DateTime.Now.ToString("HH:mm:ss");
        }

        public static string GetDateTime()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetDateTime(int relativeday)
        {
            return System.DateTime.Now.AddDays((double)relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!strContent.IsNotEmpty())
            {
                return new string[0];
            }
            if (strContent.IndexOf(strSplit) < 0)
            {
                return new string[]
                {
                    strContent
                };
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] array = new string[count];
            string[] array2 = Utils.SplitString(strContent, strSplit);
            for (int i = 0; i < count; i++)
            {
                if (i < array2.Length)
                {
                    array[i] = array2[i];
                }
                else
                {
                    array[i] = string.Empty;
                }
            }
            return array;
        }

        public static string[] PadStringArray(string[] strArray, int minLength, int maxLength)
        {
            if (minLength > maxLength)
            {
                int num = maxLength;
                maxLength = minLength;
                minLength = num;
            }
            int num2 = 0;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (minLength > -1 && strArray[i].Length < minLength)
                {
                    strArray[i] = null;
                }
                else
                {
                    if (strArray[i].Length > maxLength)
                    {
                        strArray[i] = strArray[i].Substring(0, maxLength);
                    }
                    num2++;
                }
            }
            string[] array = new string[num2];
            int num3 = 0;
            int num4 = 0;
            while (num3 < strArray.Length && num4 < array.Length)
            {
                if (strArray[num3] != null && strArray[num3] != string.Empty)
                {
                    array[num4] = strArray[num3];
                    num4++;
                }
                num3++;
            }
            return array;
        }

        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem, int maxElementLength)
        {
            string[] array = Utils.SplitString(strContent, strSplit);
            if (!ignoreRepeatItem)
            {
                return array;
            }
            return Utils.DistinctStringArray(array, maxElementLength);
        }

        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem, int minElementLength, int maxElementLength)
        {
            string[] strArray = Utils.SplitString(strContent, strSplit);
            if (ignoreRepeatItem)
            {
                strArray = Utils.DistinctStringArray(strArray);
            }
            return Utils.PadStringArray(strArray, minElementLength, maxElementLength);
        }

        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem)
        {
            return Utils.SplitString(strContent, strSplit, ignoreRepeatItem, 0);
        }

        public static string[] DistinctStringArray(string[] strArray, int maxElementLength)
        {
            System.Collections.Hashtable hashtable = new System.Collections.Hashtable();
            for (int i = 0; i < strArray.Length; i++)
            {
                string text = strArray[i];
                string text2 = text;
                if (maxElementLength > 0 && text2.Length > maxElementLength)
                {
                    text2 = text2.Substring(0, maxElementLength);
                }
                hashtable[text2.Trim()] = text;
            }
            string[] array = new string[hashtable.Count];
            hashtable.Keys.CopyTo(array, 0);
            return array;
        }

        public static string[] DistinctStringArray(string[] strArray)
        {
            return Utils.DistinctStringArray(strArray, 0);
        }

        public static string StrFilter(string str, string bantext)
        {
            string[] array = Utils.SplitString(bantext, "\r\n");
            for (int i = 0; i < array.Length; i++)
            {
                string oldValue = array[i].Substring(0, array[i].IndexOf("="));
                string newValue = array[i].Substring(array[i].IndexOf("=") + 1);
                str = str.Replace(oldValue, newValue);
            }
            return str;
        }

        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        public static bool InIPArray(string ip, string[] iparray)
        {
            string[] array = Utils.SplitString(ip, ".");
            for (int i = 0; i < iparray.Length; i++)
            {
                string[] array2 = Utils.SplitString(iparray[i], ".");
                int num = 0;
                for (int j = 0; j < array2.Length; j++)
                {
                    if (array2[j] == "*")
                    {
                        return true;
                    }
                    if (array.Length <= j || !(array2[j] == array[j]))
                    {
                        break;
                    }
                    num++;
                }
                if (num == 4)
                {
                    return true;
                }
            }
            return false;
        }

        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[strName];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(strName);
            }
            httpCookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }

        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[strName];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(strName);
            }
            httpCookie[key] = strValue;
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }

        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[strName];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(strName);
            }
            httpCookie.Value = strValue;
            httpCookie.Expires = System.DateTime.Now.AddMinutes((double)expires);
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }

        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();
            }
            return "";
        }

        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
            {
                return HttpContext.Current.Request.Cookies[strName][key].ToString();
            }
            return "";
        }

        public static string RemoveHtml(string content)
        {
            return Utils.m_HtmlTagsRegex.Replace(content, string.Empty);
        }

        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, "(\\<|\\s+)o([a-z]+\\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "(script|frame|form|meta|behavior|style)([\\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }

        public static string GetRootUrl(string forumPath)
        {
            int port = HttpContext.Current.Request.Url.Port;
            return string.Format("{0}://{1}{2}{3}", new object[]
            {
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Host.ToString(),
                (port == 80 || port == 0) ? "" : (":" + port),
                forumPath
            });
        }

        public static string GetFileExtName(string fileName)
        {
            if (!fileName.IsNotEmpty() || fileName.IndexOf('.') <= 0)
            {
                return "";
            }
            fileName = fileName.ToLower().Trim();
            return fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
        }

        public static string GetHttpWebResponse(string url)
        {
            return Utils.GetHttpWebResponse(url, string.Empty);
        }

        public static string GetHttpWebResponse(string url, string postData)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)postData.Length;
            httpWebRequest.Timeout = 20000;
            HttpWebResponse httpWebResponse = null;
            string result;
            try
            {
                System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(postData);
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
            }
            return result;
        }

        public static T GetEnum<T>(string value, T defValue)
        {
            T result;
            try
            {
                result = (T)((object)System.Enum.Parse(typeof(T), value, true));
            }
            catch (System.ArgumentException)
            {
                result = defValue;
            }
            return result;
        }

        public static string GetClientBrower()
        {
            string text = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (!string.IsNullOrEmpty(text))
            {
                string[] array = Utils.browerNames;
                for (int i = 0; i < array.Length; i++)
                {
                    string text2 = array[i];
                    if (text.Contains(text2))
                    {
                        return text2;
                    }
                }
            }
            return "Other";
        }

        public static string GetClientOS()
        {
            string result = string.Empty;
            string text = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (text == null)
            {
                return "Other";
            }
            if (text.IndexOf("Win") > -1)
            {
                result = "Windows";
            }
            else if (text.IndexOf("Mac") > -1)
            {
                result = "Mac";
            }
            else if (text.IndexOf("Linux") > -1)
            {
                result = "Linux";
            }
            else if (text.IndexOf("FreeBSD") > -1)
            {
                result = "FreeBSD";
            }
            else if (text.IndexOf("SunOS") > -1)
            {
                result = "SunOS";
            }
            else if (text.IndexOf("OS/2") > -1)
            {
                result = "OS/2";
            }
            else if (text.IndexOf("AIX") > -1)
            {
                result = "AIX";
            }
            else if (Regex.IsMatch(text, "(Bot|Crawl|Spider)"))
            {
                result = "Spiders";
            }
            else
            {
                result = "Other";
            }
            return result;
        }

        public static void RestartIISProcess()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(Utils.GetMapPath("~/web.config"));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(Utils.GetMapPath("~/web.config"), null);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlDocument.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            catch
            {
            }
        }

        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            for (int i = 0; i < strNumber.Length; i++)
            {
                string expression = strNumber[i];
                if (!Utils.IsNumeric(expression))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsNumeric(string expression)
        {
            return expression != null && expression.Length > 0 && expression.Length <= 11 && Regex.IsMatch(expression, "^[-]?[0-9]*[.]?[0-9]*$") && (expression.Length < 10 || (expression.Length == 10 && expression[0] == '1') || (expression.Length == 11 && expression[0] == '-' && expression[1] == '1'));
        }

        public static void SendEmail(string from, string password, string to, string host, int port, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(from);
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Body = body;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.Normal;
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Credentials = new NetworkCredential(from, password);
                smtpClient.Port = port;
                smtpClient.Host = host;
                smtpClient.Send(mailMessage);
            }
        }

        public static string GetLocalIPAddress()
        {
            if (string.IsNullOrEmpty(Utils.m_IP))
            {
                ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection instances = managementClass.GetInstances();
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ManagementObject managementObject = (ManagementObject)enumerator.Current;
                        if ((bool)managementObject["IPEnabled"])
                        {
                            string[] array = (string[])managementObject["IPAddress"];
                            if (array != null && array.Length > 0)
                            {
                                Utils.m_IP = array[0];
                                break;
                            }
                        }
                    }
                }
                instances.Dispose();
                managementClass.Dispose();
            }
            return Utils.m_IP;
        }
    }
}
