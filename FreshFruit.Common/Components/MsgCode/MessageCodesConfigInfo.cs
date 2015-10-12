using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FreshFruit.Common.Components.MsgCode
{
    public class MessageCodesConfigInfo : IConfigInfo
    {
        [XmlArray(ElementName = "messageCodes")]
        [XmlArrayItem(ElementName = "messageCode")]
        public List<MessageCodeItem> MessageCodes { get; set; }
    }

    public class MessageCodeItem
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }
}
