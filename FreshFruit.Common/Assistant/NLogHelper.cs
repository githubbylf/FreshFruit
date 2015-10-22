using NLog;
using NLog.Config;
using System;
using System.IO;
using System.Management;

namespace FreshFruit.Common.Assistant
{
    public static class NLogHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static string _ip = string.Empty;
        private static readonly string ApplicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"];

        static NLogHelper()
        {
            string logConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\NLog.config");
            if (!File.Exists(logConfigFilePath))
            {
                logConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\NLog.config");
            }
            LogManager.Configuration = new XmlLoggingConfiguration(logConfigFilePath);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="source"></param>
        /// <param name="exception"></param>
        public static void Info(string exception, string source = "", string message = "")
        {
            WriteLog(LogLevel.Info, message, source, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="source"></param>
        /// <param name="exception"></param>
        public static void Trace(string exception, string source = "", string message = "")
        {
            WriteLog(LogLevel.Trace, message, source, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="source"></param>
        /// <param name="exception"></param>
        public static void Error(string exception, string source = "", string message = "")
        {
            WriteLog(LogLevel.Error, message, source, exception);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="source"></param>
        /// <param name="exception"></param>
        public static void Debug(string exception, string source = "", string message = "")
        {
            WriteLog(LogLevel.Debug, message, source, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="source"></param>
        /// <param name="exception"></param>
        public static void Fatal(string exception, string source = "", string message = "")
        {
            WriteLog(LogLevel.Fatal, message, source, exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="source"></param>
        /// <param name="exception"></param>
        public static void Warn(string exception, string source = "", string message = "")
        {
            WriteLog(LogLevel.Warn, message, source, exception);
        }

        private static void WriteLog(LogLevel level, string message, string source, string exception)
        {
            var eventInfo = new LogEventInfo(level, string.Empty, message ?? string.Empty);
            eventInfo.Properties["applicationName"] = ApplicationName ?? string.Empty;
            eventInfo.Properties["ip"] = IP ?? string.Empty;
            eventInfo.Properties["source"] = source ?? string.Empty;
            eventInfo.Properties["exception"] = exception ?? string.Empty;
            Logger.Log(eventInfo);
        }

        /// <summary>
        /// 服务器IP地址（非客户端请求的IP地址）
        /// </summary>
        public static string IP
        {
            get
            {
                if (string.IsNullOrEmpty(_ip))
                {
                    var managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
                    foreach (var o in managementObjectCollection)
                    {
                        var managementObject = (ManagementObject)o;
                        if ((bool)managementObject["IPEnabled"] == true)
                        {
                            string[] ipAddresses = (string[])managementObject["IPAddress"];
                            if (ipAddresses != null && ipAddresses.Length > 0)
                            {
                                _ip = ipAddresses[0];
                                break;
                            }
                        }
                    }
                    managementObjectCollection.Dispose();
                    managementClass.Dispose();
                }
                return _ip;
            }
            set { _ip = value; }
        }
    }
}
