using System;


using System.Runtime.Serialization;


namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm detected a parallel edge that is not allowed.
    /// </summary>

    [Serializable]

    public class ParallelEdgeNotAllowedException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ParallelEdgeNotAllowedException"/> class.
        /// </summary>
        public ParallelEdgeNotAllowedException()
            : base("Parallel edges are not allowed in the graph.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ParallelEdgeNotAllowedException"/> class.
        /// </summary>
        public ParallelEdgeNotAllowedException( string message,  Exception innerException = null)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        [Obsolete("Obsolete")]
        protected ParallelEdgeNotAllowedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}