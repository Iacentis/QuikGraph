

namespace QuikGraph
{
    /// <summary>
    /// Represents a directed edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    public interface IEdge<out TVertex>
    {
        /// <summary>
        /// Gets the source vertex.
        /// </summary>

        TVertex Source { get; }

        /// <summary>
        /// Gets the target vertex.
        /// </summary>

        TVertex Target { get; }
    }
}