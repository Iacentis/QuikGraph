using System;
using System.Collections.Generic;


namespace QuikGraph.Algorithms.Exploration
{
    /// <summary>
    /// Represents a transition factory.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface ITransitionFactory<in TVertex, out TEdge>
        where TVertex : ICloneable
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Checks if the given <paramref name="vertex"/> is valid or not.
        /// </summary>
        /// <param name="vertex">Vertex to check.</param>
        /// <returns>True if the vertex is valid, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        bool IsValid(TVertex vertex);

        /// <summary>
        /// Applies the transition from the given <paramref name="source"/>.
        /// </summary>
        /// <param name="source">Source vertex.</param>
        /// <returns>Edges resulting of the apply.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        IEnumerable<TEdge> Apply(TVertex source);
    }
}