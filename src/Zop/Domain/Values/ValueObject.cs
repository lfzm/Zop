using System;
using System.Linq;
using System.Reflection;

namespace Zop.Domain.Values
{
    /// <summary>
    ///值对象基础类
    /// </summary>
    /// <typeparam name="TValueObject">值对象的类型</typeparam>
    [Serializable]
    public abstract class ValueObject<TValueObject> : IEquatable<TValueObject>, IValueObject
        where TValueObject : ValueObject<TValueObject>
    {

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TValueObject other)
        {
            if ((object)other == null)
            {
                return false;
            }

            var publicProperties = GetType().GetTypeInfo().GetProperties();
            if (!publicProperties.Any())
            {
                return true;
            }

            return publicProperties.All(property => Equals(property.GetValue(this, null), property.GetValue(other, null)));
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var item = obj as ValueObject<TValueObject>;
            return (object)item != null && Equals((TValueObject)item);

        }

        public override int GetHashCode()
        {
            return base.GetHashCode(); ;
        }

        public abstract TValueObject Clone();
        public static bool operator ==(ValueObject<TValueObject> x, ValueObject<TValueObject> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (((object)x == null) || ((object)y == null))
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(ValueObject<TValueObject> x, ValueObject<TValueObject> y)
        {
            return !(x == y);
        }
    }
}
