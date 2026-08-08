using System;

using System.Runtime.Serialization;



namespace QuikGraph
{
    /// <summary>
    /// QuikGraph base exception.
    /// </summary>

    [Serializable]

    public abstract class QuikGraphException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="QuikGraphException"/> with the given message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        protected QuikGraphException( string message,  Exception innerException = null)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        [Obsolete("Obsolete")]
        protected QuikGraphException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}