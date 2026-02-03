using System;

using System.Runtime.Serialization;



namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm could not find a path in a graph.
    /// </summary>

    [Serializable]

    public class NoPathFoundException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NoPathFoundException"/> class.
        /// </summary>
        public NoPathFoundException()
            : base("No path found to join vertices in the graph.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NoPathFoundException"/> class.
        /// </summary>
        public NoPathFoundException( string message,  Exception innerException = null)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        [Obsolete("Obsolete")]
        protected NoPathFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}