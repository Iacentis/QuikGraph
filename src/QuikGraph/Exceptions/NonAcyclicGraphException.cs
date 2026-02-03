using System;

using System.Runtime.Serialization;



namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm detected a cyclic graph when required acyclic.
    /// </summary>

    [Serializable]

    public class NonAcyclicGraphException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NonAcyclicGraphException"/> class.
        /// </summary>
        public NonAcyclicGraphException()
            : base("The graph contains at least one cycle.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NonAcyclicGraphException"/> class.
        /// </summary>
        public NonAcyclicGraphException( string message,  Exception innerException = null)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        [Obsolete("Obsolete")]
        protected NonAcyclicGraphException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}