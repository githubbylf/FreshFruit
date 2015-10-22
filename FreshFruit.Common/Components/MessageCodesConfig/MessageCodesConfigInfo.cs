using FreshFruit.Common.Interface;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FreshFruit.Common.Components.MessageCodesConfig
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
