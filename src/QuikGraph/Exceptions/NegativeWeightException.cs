using System;

using System.Runtime.Serialization;



namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm find or computed a negative weight in a graph.
    /// </summary>

    [Serializable]

    public class NegativeWeightException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NegativeWeightException"/> class.
        /// </summary>
        public NegativeWeightException()
            : base("The graph contains at least one negative weight.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NegativeWeightException"/> class.
        /// </summary>
        public NegativeWeightException( string message,  Exception innerException = null)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        [Obsolete("Obsolete")]
        protected NegativeWeightException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}