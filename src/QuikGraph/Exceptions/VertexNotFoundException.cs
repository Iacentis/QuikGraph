using System;

using System.Runtime.Serialization;



namespace QuikGraph
{
    /// <summary>
    /// Exception raised when trying to use a vertex that is not inside the manipulated graph.
    /// </summary>

    [Serializable]

    public class VertexNotFoundException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="VertexNotFoundException"/> class.
        /// </summary>
        public VertexNotFoundException()
            : base("Vertex is not present in the graph.")
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="VertexNotFoundException"/> class.
        /// </summary>
        public VertexNotFoundException( string message,  Exception innerException = null)
            : base(message, innerException)
        {
        }


        /// <summary>
        /// Constructor used during runtime serialization.
        /// </summary>
        [Obsolete("Obsolete")]
        protected VertexNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}