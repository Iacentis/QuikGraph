using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of vertices predecessors paths.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>

    [Serializable]

    public sealed class VertexPredecessorPathRecorderObserver<TVertex, TEdge> :
        IObserver<IVertexPredecessorRecorderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPredecessorPathRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        public VertexPredecessorPathRecorderObserver()
            : this(new Dictionary<TVertex, TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPredecessorPathRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="verticesPredecessors">Vertices predecessors.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesPredecessors"/> is <see langword="null"/>.</exception>
        public VertexPredecessorPathRecorderObserver(
             IDictionary<TVertex, TEdge> verticesPredecessors)
        {
            VerticesPredecessors = verticesPredecessors ?? throw new ArgumentNullException(nameof(verticesPredecessors));
        }

        /// <summary>
        /// Vertices predecessors.
        /// </summary>

        public IDictionary<TVertex, TEdge> VerticesPredecessors { get; }

        /// <summary>
        /// Path ending vertices.
        /// </summary>

        public ICollection<TVertex> EndPathVertices { get; } = new List<TVertex>();

        /// <summary>
        /// Gets all paths.
        /// </summary>
        /// <returns>Enumerable of paths.</returns>
        [Pure]

        public IEnumerable<IEnumerable<TEdge>> AllPaths()
        {
            return EndPathVertices
                .Select(vertex => VerticesPredecessors.TryGetPath(vertex, out IEnumerable<TEdge> path) ? path : null)
                .Where(path => path != null);
        }

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(IVertexPredecessorRecorderAlgorithm<TVertex, TEdge> algorithm)
        {
            ArgumentNullException.ThrowIfNull(algorithm);

            algorithm.TreeEdge += OnEdgeDiscovered;
            algorithm.FinishVertex += OnVertexFinished;
            return Finally(() =>
            {
                algorithm.TreeEdge -= OnEdgeDiscovered;
                algorithm.FinishVertex -= OnVertexFinished;
            });
        }

        #endregion

        private void OnEdgeDiscovered( TEdge edge)
        {
            Debug.Assert(edge != null);

            VerticesPredecessors[edge.Target] = edge;
        }

        private void OnVertexFinished( TVertex vertex)
        {
            Debug.Assert(vertex != null);

            if (VerticesPredecessors.Values.Any(edge => EqualityComparer<TVertex>.Default.Equals(edge.Source, vertex)))
                return;

            EndPathVertices.Add(vertex);
        }
    }
}