using System;
using System.Collections.Generic;
using System.Diagnostics;
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// A distance recorder for <see cref="ITreeBuilderAlgorithm{TVertex,TEdge}"/> algorithms.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public sealed class VertexDistanceRecorderObserver<TVertex, TEdge> : IObserver<ITreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexDistanceRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeWeights"/> is <see langword="null"/>.</exception>
        public VertexDistanceRecorderObserver( Func<TEdge, double> edgeWeights)
            : this(edgeWeights, DistanceRelaxers.EdgeShortestDistance, new Dictionary<TVertex, double>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexDistanceRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        /// <param name="distances">Distances per vertex.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="edgeWeights"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="distanceRelaxer"/> is <see langword="null"/>.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="distances"/> is <see langword="null"/>.</exception>
        public VertexDistanceRecorderObserver(
             Func<TEdge, double> edgeWeights,
             IDistanceRelaxer distanceRelaxer,
             IDictionary<TVertex, double> distances)
        {
            EdgeWeights = edgeWeights ?? throw new ArgumentNullException(nameof(edgeWeights));
            DistanceRelaxer = distanceRelaxer ?? throw new ArgumentNullException(nameof(distanceRelaxer));
            Distances = distances ?? throw new ArgumentNullException(nameof(distances));
        }

        /// <summary>
        /// Distance relaxer.
        /// </summary>

        public IDistanceRelaxer DistanceRelaxer { get; }

        /// <summary>
        /// Function that computes the weight for a given edge.
        /// </summary>

        public Func<TEdge, double> EdgeWeights { get; }

        /// <summary>
        /// Distances per vertex.
        /// </summary>

        public IDictionary<TVertex, double> Distances { get; }

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(ITreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            ArgumentNullException.ThrowIfNull(algorithm);

            algorithm.TreeEdge += OnEdgeDiscovered;
            return Finally(() => algorithm.TreeEdge -= OnEdgeDiscovered);
        }

        #endregion

        private void OnEdgeDiscovered( TEdge edge)
        {
            Debug.Assert(edge != null);

            if (!Distances.TryGetValue(edge.Source, out double sourceDistance))
            {
                Distances[edge.Source] = sourceDistance = DistanceRelaxer.InitialDistance;
            }
            Distances[edge.Target] = DistanceRelaxer.Combine(sourceDistance, EdgeWeights(edge));
        }
    }
}