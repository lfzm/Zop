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
    /// IEntity接口的基本实现
    /// 一个实体可以继承这个类直接实现到IEntity接口。
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// ID是否生成ID
        /// </summary>
        private bool IsGenerateId;
        public Entity()
        {
     
        }
        /// <summary>
        /// 此实体的唯一标识符
        /// </summary>
        [Key]
        [Required]
        [JsonProperty]
        public virtual TPrimaryKey Id { get; protected set; }

        /// <summary>
        ///检查这个实体是否是临时对象的。
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        [NotMapped]
        [JsonIgnore]
        public virtual bool IsTransient
        {
            get
            {
                if (this.IsGenerateId)
                    return true;

                TPrimaryKey defaultValue = default(TPrimaryKey);
                if (Id == null)
                    return true;
                if (defaultValue.Equals(Id))
                    return true;
                else
                    return false;
            }
        }
   
        /// <summary>
        /// 设置唯一标识符
        /// </summary>
        /// <param name="id">唯一标识符</param>
        public virtual void SetId(TPrimaryKey id)
        {
            this.IsGenerateId = true;
            this.Id = id;
        }
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TPrimaryKey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (Entity<TPrimaryKey>)obj;
            if (IsTransient && other.IsTransient)
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) && !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }
        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{GetType().Name} {Id}]";
        }
        /// <inheritdoc/>
        public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }
        /// <inheritdoc/>
        public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return !(left == right);
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
    }


}

