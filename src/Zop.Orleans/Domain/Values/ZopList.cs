using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Zop.Domain.Values
{
    /// <summary>
    /// 存储JSON的List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ZopList<T> : List<T>, IValueObject
    {
        public ZopList() { }
        public ZopList(List<T> collection) : base(collection) { }
        [NotMapped]
        public new int Capacity { get => base.Capacity; set => base.Capacity = value; }

        [MaxLength(20000)]
        [JsonIgnore]
        public virtual string Json
        {
            get
            {
                return JsonConvert.SerializeObject(this);
            }
            private set
            {
                if (value.IsNull())   return;
                this.Clear();

                var list = JsonConvert.DeserializeObject<ZopList<T>>(value);
                foreach (var item in list)
                {
                    this.Add(item);
                }
            }
        }
    }
}
