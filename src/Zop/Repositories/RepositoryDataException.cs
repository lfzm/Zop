using System;
using System.Runtime.Serialization;

namespace Zop.Repositories
{
    [Serializable]
    public class RepositoryDataException:ZopException
    {
        /// <summary>
        /// Creates a new <see cref="RepositoryDataException"/> object.
        /// </summary>
        public RepositoryDataException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="RepositoryDataException"/> object.
        /// </summary>
        public RepositoryDataException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="RepositoryDataException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public RepositoryDataException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="RepositoryDataException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public RepositoryDataException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
