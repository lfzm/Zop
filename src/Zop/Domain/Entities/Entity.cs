using System;
using System.ComponentModel.DataAnnotations;
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
        public virtual TPrimaryKey Id { get; protected set; }

        public TEntity Clone<TEntity>()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return (TEntity)formatter.Deserialize(stream);
        }
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
    }


}

