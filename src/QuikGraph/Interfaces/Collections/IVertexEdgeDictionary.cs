using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;


#nullable disable
namespace QuikGraph.Collections
{
    /// <summary>
    /// A cloneable dictionary of vertices associated to their edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IVertexEdgeDictionary<TVertex, TEdge> : IDictionary<TVertex, IEdgeList<TVertex, TEdge>>
        , ICloneable
        , ISerializable
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Gets a clone of the dictionary. The vertices and edges are not cloned.
        /// </summary>
        /// <returns>Cloned dictionary.</returns>
        [Pure]
        new IVertexEdgeDictionary<TVertex, TEdge> Clone();
    }
}