using System;
using System.Runtime.Serialization;

namespace Zop
{
    /// <summary>
    /// Zop系统为Zop特定异常所抛出的基异常类型。
    /// </summary>
    [Serializable]
    public class ZopException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="ZopException"/> object.
        /// </summary>
        public ZopException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="ZopException"/> object.
        /// </summary>
        public ZopException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="ZopException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public ZopException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="ZopException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public ZopException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
