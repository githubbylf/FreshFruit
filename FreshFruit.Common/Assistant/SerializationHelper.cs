using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace FreshFruit.Common.Assistant
{
    public class SerializationHelper
    {
        private static Dictionary<int, XmlSerializer> serializer_dict = new Dictionary<int, XmlSerializer>();

        private SerializationHelper()
        {
        }

        public static XmlSerializer GetSerializer(System.Type t)
        {
            int hashCode = t.GetHashCode();
            if (!SerializationHelper.serializer_dict.ContainsKey(hashCode))
            {
                SerializationHelper.serializer_dict.Add(hashCode, new XmlSerializer(t));
            }
            return SerializationHelper.serializer_dict[hashCode];
        }

        public static object Load(System.Type type, string filename)
        {
            System.IO.FileStream fileStream = null;
            object result;
            try
            {
                fileStream = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                result = xmlSerializer.Deserialize(fileStream);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            return result;
        }

        public static bool Save(object obj, string filename)
        {
            bool result = false;
            System.IO.FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(fileStream, obj);
                result = true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            return result;
        }

        public static string Serialize(object obj)
        {
            string result = "";
            XmlSerializer serializer = SerializationHelper.GetSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = null;
            System.IO.StreamReader streamReader = null;
            try
            {
                xmlTextWriter = new XmlTextWriter(memoryStream, System.Text.Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                serializer.Serialize(xmlTextWriter, obj);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                streamReader = new StreamReader(memoryStream);
                result = streamReader.ReadToEnd();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlTextWriter != null)
                {
                    xmlTextWriter.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                memoryStream.Close();
            }
            return result;
        }

        public static string Serialize(object obj, XmlSerializerNamespaces ns)
        {
            string result = "";
            XmlSerializer serializer = SerializationHelper.GetSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = null;
            StreamReader streamReader = null;
            try
            {
                xmlTextWriter = new XmlTextWriter(memoryStream, System.Text.Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                serializer.Serialize(xmlTextWriter, obj, ns);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                streamReader = new StreamReader(memoryStream);
                result = streamReader.ReadToEnd();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlTextWriter != null)
                {
                    xmlTextWriter.Close();
                }
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                memoryStream.Close();
            }
            return result;
        }

        public static object DeSerialize(System.Type type, string s)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            object result;
            try
            {
                XmlSerializer serializer = SerializationHelper.GetSerializer(type);
                result = serializer.Deserialize(new System.IO.MemoryStream(bytes));
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static byte[] SerializeObject(object pObj)
        {
            if (pObj == null)
            {
                return null;
            }
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, pObj);
            memoryStream.Position = 0L;
            byte[] array = new byte[memoryStream.Length];
            memoryStream.Read(array, 0, array.Length);
            memoryStream.Close();
            memoryStream.Dispose();
            return array;
        }

        public static T DeserializeObject<T>(byte[] pBytes)
        {
            T result = default(T);
            if (pBytes == null)
            {
                return result;
            }
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(pBytes);
            memoryStream.Position = 0L;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            result = (T)((object)binaryFormatter.Deserialize(memoryStream));
            memoryStream.Close();
            memoryStream.Dispose();
            return result;
        }
    }
}
