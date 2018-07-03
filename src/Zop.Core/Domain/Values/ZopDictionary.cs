using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Runtime.Serialization;

namespace Zop.Domain.Values
{
    /// <summary>
    /// 存储JSON字典
    /// </summary>
    /// <typeparam name="TKey">key type</typeparam>
    /// <typeparam name="TValue">value type</typeparam>
    [Serializable]
    public class ZopDictionary<TKey, TValue> : Dictionary<TKey, TValue>,IValueObject
    {
        protected ZopDictionary(SerializationInfo info, StreamingContext context):base(info, context)
        {

        }
        public ZopDictionary() { }

        [MaxLength(20000)]
        [JsonIgnore]
        public string Json
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
            private set
            {
                if (value.IsNull())
                    return;
                var data = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(value);
                this.Clear();
                foreach (var item in data)
                {
                    this.Add(item.Key, item.Value);
                }
            }
        }

    }
}
