using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


#nullable disable
namespace QuikGraph.Collections;

/// <summary>
/// Stores associations of vertices to their edges.
/// </summary>
/// <typeparam name="TVertex">Vertex type.</typeparam>
/// <typeparam name="TEdge">Edge type.</typeparam>
[Serializable]
public sealed class VertexEdgeDictionary<TVertex, TEdge>
    : Dictionary<TVertex, IEdgeList<TVertex, TEdge>>, IVertexEdgeDictionary<TVertex, TEdge>
    where TEdge : IEdge<TVertex>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexEdgeDictionary{TVertex,TEdge}"/> class.
    /// </summary>
    public VertexEdgeDictionary()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VertexEdgeDictionary{TVertex,TEdge}"/> class.
    /// </summary>
    /// <param name="capacity">Dictionary capacity.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="capacity"/> is negative.</exception>
    public VertexEdgeDictionary(int capacity)
        : base(capacity)
    {
    }


    [Obsolete("Obsolete")]
    private VertexEdgeDictionary(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    #region ICloneable

    /// <summary>
    /// Clones this vertex/edges dictionary.
    /// </summary>
    /// <returns>Cloned dictionary.</returns>
    public VertexEdgeDictionary<TVertex, TEdge> Clone()
    {
        var clone = new VertexEdgeDictionary<TVertex, TEdge>(Count);
        foreach (KeyValuePair<TVertex, IEdgeList<TVertex, TEdge>> pair in this)
            clone.Add(pair.Key, pair.Value.Clone());
        return clone;
    }

    /// <inheritdoc />
    IVertexEdgeDictionary<TVertex, TEdge> IVertexEdgeDictionary<TVertex, TEdge>.Clone()
    {
        return Clone();
    }


    /// <inheritdoc />
    object ICloneable.Clone()
    {
        return Clone();
    }

    #endregion
}