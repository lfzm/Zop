using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Json格式转换扩展类
    /// </summary>
    public static class JsonExtention
    {
        /// <summary>
        /// 对象转换为String
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None);
        }
        /// <summary>
        /// 对象转换为String 去掉null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonIgnoreNullString(this object obj)
        {
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return JsonConvert.SerializeObject(obj, Formatting.None, jSetting);
        }
        /// <summary>
        /// 对象转换为String
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonFormatString(this object obj)
        {
            List<JsonConverter> Converters = new List<JsonConverter>();
            Converters.Add(new CustomDecimalConverter());
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Converters = Converters };
            return JsonConvert.SerializeObject(obj, Formatting.Indented, jSetting);

        }
        /// <summary>
        /// Json转换为对应对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToFromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }


    /// <summary>
    /// decimal格式化
    /// </summary>
    public class CustomDecimalConverter : CustomCreationConverter<decimal>
    {
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        public override decimal Create(Type objectType)
        {
            return 0.0M;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null || (decimal)value == 0)
                writer.WriteValue(0.0M);
            else
            {
                var formatted = Convert.ToDouble(value);
                writer.WriteValue(formatted);
            }

        }
    }

    /// <summary>
    /// DateTime 格式化
    /// </summary>
    public class CustomDateTimeConverter : CustomCreationConverter<DateTime>
    {
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        public override DateTime Create(Type objectType)
        {
            return DateTime.Now;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null || (DateTime)value == DateTime.MinValue)
                writer.WriteValue(DateTime.Now);
            else
            {
                var formatted = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss:fff");
                writer.WriteValue(formatted);
            }

        }
    }
}
