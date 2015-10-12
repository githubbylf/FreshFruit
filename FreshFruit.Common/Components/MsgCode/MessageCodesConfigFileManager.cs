using FreshFruit.Common.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshFruit.Common.Components.MsgCode
{
    public class MessageCodesConfigFileManager:DefaultConfigFileManager
    {
        private static MessageCodesConfigInfo m_configinfo;

        /// <summary>
        /// 文件修改时间
        /// </summary>
        private static DateTime m_fileoldchange;

        /// <summary>
        /// 初始化文件修改时间和对象实例
        /// </summary>
        static MessageCodesConfigFileManager()
        {
            if (Utils.FileExists(ConfigFilePath))
            {
                m_fileoldchange = System.IO.File.GetLastWriteTime(ConfigFilePath);
                m_configinfo = (MessageCodesConfigInfo)DefaultConfigFileManager.DeserializeInfo(ConfigFilePath, typeof(MessageCodesConfigInfo));
            }
        }

        /// <summary>
        /// 当前的配置类实例
        /// </summary>
        public new static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = (MessageCodesConfigInfo)value; }
        }

        /// <summary>
        /// 配置文件所在路径
        /// </summary>
        public static string filename = null;


        /// <summary>
        /// 获取配置文件所在路径
        /// </summary>
        public new static string ConfigFilePath
        {
            get
            {
                if (filename == null)
                {
                    filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "messageCodes.config");
                }

                return filename;
            }
        }

        /// <summary>
        /// 返回配置类实例
        /// </summary>
        /// <returns></returns>
        public static MessageCodesConfigInfo LoadConfig()
        {
            if (Utils.FileExists(ConfigFilePath))
            {
                ConfigInfo = DefaultConfigFileManager.LoadConfig(ref m_fileoldchange, ConfigFilePath, ConfigInfo);
                return ConfigInfo as MessageCodesConfigInfo;
            }
            else
                return null;
        }

        /// <summary>
        /// 保存配置类实例
        /// </summary>
        /// <returns></returns>
        public override bool SaveConfig()
        {
            return base.SaveConfig(ConfigFilePath, ConfigInfo);
        }
    }
}
