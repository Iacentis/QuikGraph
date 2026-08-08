using System;

using System.Runtime.Serialization;



namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm detected a negative cycle in a graph.
    /// </summary>

    [Serializable]

    public class NegativeCycleGraphException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCycleGraphException"/> class.
        /// </summary>
        public NegativeCycleGraphException()
            : base("The graph contains at least one negative cycle.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCycleGraphException"/> class.
        /// </summary>
        public NegativeCycleGraphException( string message,  Exception innerException = null)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        [Obsolete("Obsolete")]
        protected NegativeCycleGraphException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}