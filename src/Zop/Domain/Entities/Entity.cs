using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// IEntity接口的基本实现  一个实体可以继承这个类直接实现到IEntity接口。
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 此实体的唯一标识符
        /// </summary>
        [Key]
        [Required]
        [JsonProperty]
        public virtual TPrimaryKey Id { get; protected set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// 设置唯一标识符
        /// </summary>
        /// <param name="id">唯一标识符</param>
        public virtual void SetId(TPrimaryKey id)
        {
            this.Id = id;
        }
        /// <summary>
        /// 克隆实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity Clone<TEntity>()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return (TEntity)formatter.Deserialize(stream);
        }
        /// <summary>
        /// 获取唯一标示
        /// </summary>
        /// <returns></returns>
        public object GetPrimaryKey()
        {
            TPrimaryKey defaultValue = default(TPrimaryKey);
            if (Id == null)
                return null;
            if (Id.Equals(defaultValue))
                return defaultValue;
            else
                return Id;
        }
       
    }


}

