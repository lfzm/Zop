using System;
using System.Runtime.Serialization;

namespace Zop.DTO
{
    /// <summary>
    /// DTO验证异常
    /// </summary>
    public class DTOVerifyException : ZopException
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
        /// Creates a new <see cref="DTOVerifyException"/> object.
        /// </summary>
        public DTOVerifyException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="DTOVerifyException"/> object.
        /// </summary>
        public DTOVerifyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="DTOVerifyException"/> object.
        /// </summary>
        public DTOVerifyException(Type entityType, object id)
            : this(entityType, id, null)
        {

        }

        /// <summary>
        /// Creates a new <see cref="DTOVerifyException"/> object.
        /// </summary>
        public DTOVerifyException(Type entityType, object id, Exception innerException)
            : base($"There is no such an entity. Entity type: {entityType.FullName}, id: {id}", innerException)
        {
            EntityType = entityType;
            Id = id;
        }

        /// <summary>
        /// Creates a new <see cref="DTOVerifyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public DTOVerifyException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="DTOVerifyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public DTOVerifyException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
