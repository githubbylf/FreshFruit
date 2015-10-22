using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace FreshFruit.Common.Assistant
{
    public class JsonHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(Object obj)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, timeFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static Object DeserializeObject(string jsonString, Type dataType)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
            return JsonConvert.DeserializeObject(jsonString, dataType, settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
