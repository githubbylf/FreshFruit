using FreshFruit.Common.ToolsUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshFruit.Common.Components.MsgCode
{
    public class MessageCodesConfig
    {
        private static System.Timers.Timer redisConfigTimer = new System.Timers.Timer(600000);//间隔为10分钟

        private static MessageCodesConfigInfo m_configinfo;

        static MessageCodesConfig()
        {
            m_configinfo = MessageCodesConfigFileManager.LoadConfig();
            redisConfigTimer.AutoReset = true;
            redisConfigTimer.Enabled = true;
            redisConfigTimer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            redisConfigTimer.Start();
        }

        public static string GetMessageContent(string errorCode)
        {
            string result = string.Empty;
            if (m_configinfo != null && errorCode.IsNotEmpty())
            {
                var item = m_configinfo.MessageCodes.FirstOrDefault(u => u.Name == errorCode);
                if (item != null)
                {
                    result = item.Value ?? "系统出错，请联系管理员！";
                }
            }
            return result;

        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetConfig();
        }


        /// <summary>
        /// 重设配置类实例
        /// </summary>
        public static void ResetConfig()
        {
            m_configinfo = MessageCodesConfigFileManager.LoadConfig();
        }

        /// <summary>
        /// 获取配置类实例
        /// </summary>
        /// <returns></returns>
        public static MessageCodesConfigInfo GetConfig()
        {
            return m_configinfo;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <param name="configinfo"></param>
        /// <returns></returns>
        public static bool SaveConfig(MessageCodesConfigInfo configinfo)
        {
            MessageCodesConfigFileManager rcfm = new MessageCodesConfigFileManager();
            MessageCodesConfigFileManager.ConfigInfo = configinfo;
            return rcfm.SaveConfig();
        }
    }
}
