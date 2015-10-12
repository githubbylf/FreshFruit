using FreshFruit.Common.BaseCommon.Extended;
using System;

namespace FreshFruit.Common.Components.MsgCode
{
    public class DefaultConfigFileManager
    {
        /// <summary>
        /// 文件所在路径变量
        /// </summary>
        private static string m_configfilepath;

        /// <summary>
        /// 临时配置对象变量
        /// </summary>
        private static IConfigInfo m_configinfo = null;

        /// <summary>
        /// 锁对象
        /// </summary>
        private static object m_lockHelper = new object();

        /// <summary>
        /// 文件是否被修改，默认是true
        /// </summary>
        public static bool m_IsChanged = true;

        /// <summary>
        /// 文件所在路径
        /// </summary>
        public static string ConfigFilePath
        {
            get { return m_configfilepath; }
            set { m_configfilepath = value; }
        }

        public static bool IsChanged
        {
            get { return m_IsChanged; }
            set { m_IsChanged = value; }
        }

        /// <summary>
        /// 临时配置对象
        /// </summary>
        public static IConfigInfo ConfigInfo
        {
            get { return m_configinfo; }
            set { m_configinfo = value; }
        }

        /// <summary>
        /// 加载(反序列化)指定对象类型的配置对象
        /// </summary>
        /// <param name="fileoldChangeData">文件加载时间</param>
        /// <param name="configFilePath">配置文件所在路径</param>
        /// <param name="configinfo">相应的变量 注:该参数主要用于设置m_configinfo变量 和 获取类型.GetType()</param>
        /// <returns></returns>
        protected static IConfigInfo LoadConfig(ref DateTime fileoldChangeData, string configFilePath, IConfigInfo configinfo)
        {
            return LoadConfig(ref fileoldChangeData, configFilePath, configinfo, true);
        }


        /// <summary>
        /// 加载(反序列化)指定对象类型的配置对象
        /// </summary>
        /// <param name="fileoldChangeData">文件加载时间</param>
        /// <param name="configFilePath">配置文件所在路径(包括文件名)</param>
        /// <param name="configinfo">相应的变量 注:该参数主要用于设置m_configinfo变量 和 获取类型.GetType()</param>
        /// <param name="checkTime">是否检查并更新传递进来的"文件加载时间"变量</param>
        /// <returns></returns>
        protected static IConfigInfo LoadConfig(ref DateTime fileoldChangeData, string configFilePath, IConfigInfo configinfo, bool checkTime)
        {
            lock (m_lockHelper)
            {
                m_configfilepath = configFilePath;
                m_configinfo = configinfo;

                if (checkTime)
                {
                    DateTime m_fileNewChangeData = System.IO.File.GetLastWriteTime(configFilePath);

                    //当程序运行中config文件发生变化时则对config重新赋值
                    if (fileoldChangeData != m_fileNewChangeData)
                    {
                        fileoldChangeData = m_fileNewChangeData;
                        m_configinfo = DeserializeInfo(configFilePath, configinfo.GetType());
                        m_IsChanged = true;
                    }
                }
                else
                {
                    m_configinfo = DeserializeInfo(configFilePath, configinfo.GetType());
                    m_IsChanged = false;
                }

                return m_configinfo;
            }
        }


        /// <summary>
        /// 反序列化指定的类
        /// </summary>
        /// <param name="configfilepath">config 文件的路径</param>
        /// <param name="configtype">相应的类型</param>
        /// <returns></returns>
        public static IConfigInfo DeserializeInfo(string configfilepath, Type configtype)
        {
            return (IConfigInfo)SerializationHelper.Load(configtype, configfilepath);
        }

        /// <summary>
        /// 保存配置实例(虚方法需继承)
        /// </summary>
        /// <returns></returns>
        public virtual bool SaveConfig()
        {
            return true;
        }

        /// <summary>
        /// 保存(序列化)指定路径下的配置文件
        /// </summary>
        /// <param name="configFilePath">指定的配置文件所在的路径(包括文件名)</param>
        /// <param name="configinfo">被保存(序列化)的对象</param>
        /// <returns></returns>
        public bool SaveConfig(string configFilePath, IConfigInfo configinfo)
        {
            return SerializationHelper.Save(configinfo, configFilePath);
        }
    }
}
