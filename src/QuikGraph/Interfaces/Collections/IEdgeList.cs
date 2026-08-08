
using System;

using System.Collections.Generic;
using System.Diagnostics.Contracts;


namespace QuikGraph.Collections
{
    /// <summary>
    /// Represents a cloneable list of edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IEdgeList<TVertex, TEdge> : IList<TEdge>

        , ICloneable

        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Trims excess allocated space.
        /// </summary>
        void TrimExcess();

        /// <summary>
        /// Gets a clone of this list.
        /// </summary>
        /// <returns>Cloned list.</returns>
        [Pure]


        new

        IEdgeList<TVertex, TEdge> Clone();
    }
}