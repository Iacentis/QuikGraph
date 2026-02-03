

namespace QuikGraph
{
    /// <summary>
    /// Delegate to create a vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <returns>The created vertex.</returns>

    public delegate TVertex VertexFactory<out TVertex>();
}