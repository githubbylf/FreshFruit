using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FreshFruit.Common.Components.MsgCode
{
    /// <summary>
    ///配置信息类接口
    /// </summary>
    public interface IConfigInfo
    {
    }

    public class KeyValueItem
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }
}
