using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of vertices predecessors (undirected).
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>

    [Serializable]

    public sealed class UndirectedVertexPredecessorRecorderObserver<TVertex, TEdge> :
        IObserver<IUndirectedTreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedVertexPredecessorRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        public UndirectedVertexPredecessorRecorderObserver()
            : this(new Dictionary<TVertex, TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedVertexPredecessorRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="verticesPredecessors">Vertices predecessors.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="verticesPredecessors"/> is <see langword="null"/>.</exception>
        public UndirectedVertexPredecessorRecorderObserver(
             IDictionary<TVertex, TEdge> verticesPredecessors)
        {
            VerticesPredecessors = verticesPredecessors ?? throw new ArgumentNullException(nameof(verticesPredecessors));
        }

        /// <summary>
        /// Vertices predecessors.
        /// </summary>

        public IDictionary<TVertex, TEdge> VerticesPredecessors { get; }

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(IUndirectedTreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            ArgumentNullException.ThrowIfNull(algorithm);

            algorithm.TreeEdge += OnEdgeDiscovered;
            return Finally(() => algorithm.TreeEdge -= OnEdgeDiscovered);
        }

        #endregion

        private void OnEdgeDiscovered( object sender,  UndirectedEdgeEventArgs<TVertex, TEdge> args)
        {
            Debug.Assert(sender != null);
            Debug.Assert(args != null);

            VerticesPredecessors[args.Target] = args.Edge;
        }

        /// <summary>
        /// Tries to get the predecessor path, if reachable.
        /// </summary>
        /// <param name="vertex">Path ending vertex.</param>
        /// <param name="path">Path to the ending vertex.</param>
        /// <returns>True if a path was found, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="vertex"/> is <see langword="null"/>.</exception>
        [Pure]

        public bool TryGetPath( TVertex vertex,  out IEnumerable<TEdge> path)
        {
            return VerticesPredecessors.TryGetPath(vertex, out path);
        }
    }
}