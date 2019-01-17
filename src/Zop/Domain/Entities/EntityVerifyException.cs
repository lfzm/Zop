using System;
using System.Runtime.Serialization;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// 实体验证异常
    /// </summary>
    [Serializable]
    public class EntityVerifyException : ZopException
    {
        /// <summary>
        /// Type of the entity.
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// Id of the Entity.
        /// </summary>
        public object Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="EntityVerifyException"/> object.
        /// </summary>
        public EntityVerifyException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityVerifyException"/> object.
        /// </summary>
        public EntityVerifyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityVerifyException"/> object.
        /// </summary>
        public EntityVerifyException(Type entityType, object id)
            : this(entityType, id, null)
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityVerifyException"/> object.
        /// </summary>
        public EntityVerifyException(Type entityType, object id, Exception innerException)
            : base($"There is no such an entity. Entity type: {entityType.FullName}, id: {id}", innerException)
        {
            EntityType = entityType;
            Id = id;
        }

        /// <summary>
        /// Creates a new <see cref="EntityVerifyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public EntityVerifyException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityVerifyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public EntityVerifyException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
