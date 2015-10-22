using FreshFruit.Common.Interface;
using System.Runtime.Serialization.Json;

namespace FreshFruit.Common.Serialization
{
    /// <summary>
	/// 表示一个 JSON 列化器对象。
	/// </summary>
	public class ObjectJsonSerializer : IObjectSerializer
    {
        public virtual byte[] Serialize<TObject>(TObject obj)
        {
            System.Type type = obj.GetType();
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(type);
            byte[] result = null;
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                dataContractJsonSerializer.WriteObject(memoryStream, obj);
                result = memoryStream.ToArray();
                memoryStream.Close();
            }
            return result;
        }

        public virtual TObject Deserialize<TObject>(byte[] stream)
        {
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(TObject));
            TObject result;
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(stream))
            {
                TObject tObject = (TObject)((object)dataContractJsonSerializer.ReadObject(memoryStream));
                memoryStream.Close();
                result = tObject;
            }
            return result;
        }
    }
}
