using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Condensation;

namespace QuikGraph.Tests.Algorithms.Condensation
{
    /// <summary>
    /// Base class for condensation algorithms.
    /// </summary>
    internal abstract class CondensationGraphAlgorithmTestsBase
    {
        #region Test helpers

        protected static void CheckVertexCount<TVertex, TEdge>(
            IVertexSet<TVertex> graph,
            IVertexSet<AdjacencyGraph<TVertex, TEdge>> condensedGraph)
            where TEdge : IEdge<TVertex>
        {
            int count = 0;
            foreach (AdjacencyGraph<TVertex, TEdge> vertices in condensedGraph.Vertices)
                count += vertices.VertexCount;
            Assert.That(graph.VertexCount, Is.EqualTo(count), $"{nameof(graph.VertexCount)} does not match.");
        }

        protected static void CheckEdgeCount<TVertex, TEdge>(
            IEdgeSet<TVertex, TEdge> graph,
            IEdgeListGraph<AdjacencyGraph<TVertex, TEdge>,
                CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph)
            where TEdge : IEdge<TVertex>
        {
            // Check edge count
            int count = 0;
            foreach (CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>> edges in condensedGraph.Edges)
                count += edges.Edges.Count;
            foreach (AdjacencyGraph<TVertex, TEdge> vertices in condensedGraph.Vertices)
                count += vertices.EdgeCount;
            Assert.That(graph.EdgeCount, Is.EqualTo(count), $"{nameof(graph.EdgeCount)} does not match.");
        }

        protected static void CheckDAG<TVertex, TEdge>(
            IVertexAndEdgeListGraph<AdjacencyGraph<TVertex, TEdge>,
                CondensedEdge<TVertex, TEdge, AdjacencyGraph<TVertex, TEdge>>> condensedGraph)
            where TEdge : IEdge<TVertex>
        {
            Assert.That(condensedGraph.IsDirectedAcyclicGraph(), Is.True);
        }

        #endregion
    }
}