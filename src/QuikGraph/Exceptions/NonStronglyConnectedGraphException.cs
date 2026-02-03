using System;

using System.Runtime.Serialization;



namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm detected a non-strongly connected graph.
    /// </summary>

    [Serializable]

    public class NonStronglyConnectedGraphException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NonStronglyConnectedGraphException"/> class.
        /// </summary>
        public NonStronglyConnectedGraphException()
            : base("The graph is not strongly connected.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NonStronglyConnectedGraphException"/> class.
        /// </summary>
        public NonStronglyConnectedGraphException( string message,  Exception innerException = null)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        [Obsolete("Obsolete")]
        protected NonStronglyConnectedGraphException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}