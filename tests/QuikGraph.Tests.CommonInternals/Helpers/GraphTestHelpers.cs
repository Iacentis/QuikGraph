using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace QuikGraph.Tests
{
    /// <summary>
    /// Test helpers for graphs.
    /// </summary>
    internal static class GraphTestHelpers
    {
        public static void AssertVertexCountEqual<TVertex>(
            this IVertexSet<TVertex> left,
            IVertexSet<TVertex> right)
        {
            Assert.That(left, Is.Not.Null);
            Assert.That(right, Is.Not.Null);
            Assert.That(left.VertexCount, Is.EqualTo(right.VertexCount));
        }

        public static void AssertEdgeCountEqual<TVertex, TEdge>(
            this IEdgeSet<TVertex, TEdge> left,
            IEdgeSet<TVertex, TEdge> right)
            where TEdge : IEdge<TVertex>
        {
            Assert.That(left, Is.Not.Null);
            Assert.That(right, Is.Not.Null);
            Assert.That(left.EdgeCount, Is.EqualTo(right.EdgeCount));
        }

        public static bool InVertexSet<TVertex>(
            IVertexSet<TVertex> graph,
            TVertex vertex)
        {
            return graph.ContainsVertex(vertex);
        }

        public static bool InVertexSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph,
            TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            return InVertexSet(graph, edge.Source)
                   && InVertexSet(graph, edge.Target);
        }

        public static bool InEdgeSet<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph,
            TEdge edge)
            where TEdge : IEdge<TVertex>
        {
            return InVertexSet(graph, edge) && graph.ContainsEdge(edge);
        }

        [Pure]
        public static bool IsDescendant<TValue>(
            Dictionary<TValue, TValue> parents,
            TValue u,
            TValue v)
        {
            TValue t;
            TValue current = u;
            do
            {
                t = current;
                current = parents[t];
                if (current.Equals(v))
                    return true;
            } while (!t.Equals(current));

            return false;
        }

        #region Vertices helpers

        public static void AssertNoVertex<TVertex>(IVertexSet<TVertex> graph)
        {
            Assert.That(graph.IsVerticesEmpty, Is.True);
            Assert.That(0, Is.EqualTo(graph.VertexCount));
            CollectionAssert.IsEmpty(graph.Vertices);
        }

        public static void AssertHasVertices<TVertex>(
            IVertexSet<TVertex> graph,
            IEnumerable<TVertex> vertices)
        {
            TVertex[] vertexArray = vertices.ToArray();
            CollectionAssert.IsNotEmpty(vertexArray);

            Assert.That(graph.IsVerticesEmpty, Is.False);
            Assert.That(vertexArray.Length, Is.EqualTo(graph.VertexCount));
            CollectionAssert.AreEquivalent(vertexArray, graph.Vertices);
        }

        public static void AssertNoVertices<TVertex>(
            IImplicitVertexSet<TVertex> graph,
            IEnumerable<TVertex> vertices)
        {
            AssertImplicitHasVertices(graph, vertices, false);
        }

        public static void AssertHasVertices<TVertex>(
            IImplicitVertexSet<TVertex> graph,
            IEnumerable<TVertex> vertices)
        {
            AssertImplicitHasVertices(graph, vertices, true);
        }

        private static void AssertImplicitHasVertices<TVertex>(
            IImplicitVertexSet<TVertex> graph,
            IEnumerable<TVertex> vertices,
            bool expectedContains)
        {
            TVertex[] vertexArray = vertices.ToArray();
            CollectionAssert.IsNotEmpty(vertexArray);

            foreach (TVertex vertex in vertexArray)
            {
                Assert.That(expectedContains, Is.EqualTo(graph.ContainsVertex(vertex)));
            }
        }

        #endregion

        #region Edges helpers

        public static void AssertNoEdge<TVertex, TEdge>(IEdgeSet<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            Assert.That(graph.IsEdgesEmpty, Is.True);
            Assert.That(0, Is.EqualTo(graph.EdgeCount));
            CollectionAssert.IsEmpty(graph.Edges);
        }

        public static void AssertHasEdges<TVertex, TEdge>(
            IEdgeSet<TVertex, TEdge> graph,
            IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.That(graph.IsEdgesEmpty, Is.False);
            Assert.That(edgeArray.Length, Is.EqualTo(graph.EdgeCount));
            CollectionAssert.AreEquivalent(edgeArray, graph.Edges);
        }

        public static void AssertHasEdges<TVertex, TEdge>(
            IEdgeSet<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            AssertHasEdges(
                graph,
                edges.Select(edge => new SReversedEdge<TVertex, TEdge>(edge)));
        }

        public static void AssertSameReversedEdge(
            Edge<int> edge,
            SReversedEdge<int, Edge<int>> reversedEdge)
        {
            Assert.That(new SReversedEdge<int, Edge<int>>(edge), Is.EqualTo(reversedEdge));
            Assert.That(edge, Is.SameAs(reversedEdge.OriginalEdge));
        }

        public static void AssertSameReversedEdges(
            IEnumerable<Edge<int>> edges,
            IEnumerable<SReversedEdge<int, Edge<int>>> reversedEdges)
        {
            var edgesArray = edges.ToArray();
            var reversedEdgesArray = reversedEdges.ToArray();
            Assert.That(edgesArray.Length, Is.EqualTo(reversedEdgesArray.Length));
            for (int i = 0; i < edgesArray.Length; ++i)
                AssertSameReversedEdge(edgesArray[i], reversedEdgesArray[i]);
        }

        #endregion

        #region Graph helpers

        public static void AssertEmptyGraph<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            AssertNoVertex(graph);
            AssertNoEdge(graph);
        }

        public static void AssertEmptyGraph<TVertex>(
            CompressedSparseRowGraph<TVertex> graph)
        {
            AssertNoVertex(graph);
            AssertNoEdge(graph);
        }

        public static void AssertNoInEdge<TVertex, TEdge>(IBidirectionalIncidenceGraph<TVertex, TEdge> graph,
            TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            Assert.That(graph.IsInEdgesEmpty(vertex), Is.True);
            Assert.That(0, Is.EqualTo(graph.InDegree(vertex)));
            CollectionAssert.IsEmpty(graph.InEdges(vertex));
        }

        public static void AssertHasInEdges<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, TEdge> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.That(graph.IsInEdgesEmpty(vertex), Is.False);
            Assert.That(edgeArray.Length, Is.EqualTo(graph.InDegree(vertex)));
            CollectionAssert.AreEquivalent(edgeArray, graph.InEdges(vertex));
        }

        public static void AssertHasReversedInEdges<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.That(graph.IsInEdgesEmpty(vertex), Is.False);
            Assert.That(edgeArray.Length, Is.EqualTo(graph.InDegree(vertex)));
            CollectionAssert.AreEquivalent(
                edgeArray.Select(edge => new SReversedEdge<TVertex, TEdge>(edge)),
                graph.InEdges(vertex));
        }

        public static void AssertNoOutEdge<TVertex, TEdge>(IImplicitGraph<TVertex, TEdge> graph, TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            Assert.That(graph.IsOutEdgesEmpty(vertex), Is.True);
            Assert.That(0, Is.EqualTo(graph.OutDegree(vertex)));
            CollectionAssert.IsEmpty(graph.OutEdges(vertex));
        }

        public static void AssertHasOutEdges<TVertex, TEdge>(
            IImplicitGraph<TVertex, TEdge> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.That(graph.IsOutEdgesEmpty(vertex), Is.False);
            Assert.That(edgeArray.Length, Is.EqualTo(graph.OutDegree(vertex)));
            CollectionAssert.AreEquivalent(edgeArray, graph.OutEdges(vertex));
        }

        public static void AssertHasReversedOutEdges<TVertex, TEdge>(
            IImplicitGraph<TVertex, SReversedEdge<TVertex, TEdge>> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges)
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.That(graph.IsOutEdgesEmpty(vertex), Is.False);
            Assert.That(edgeArray.Length, Is.EqualTo(graph.OutDegree(vertex)));
            CollectionAssert.AreEquivalent(
                edgeArray.Select(edge => new SReversedEdge<TVertex, TEdge>(edge)),
                graph.OutEdges(vertex));
        }


        public static void AssertNoAdjacentEdge<TVertex, TEdge>(IImplicitUndirectedGraph<TVertex, TEdge> graph,
            TVertex vertex)
            where TEdge : IEdge<TVertex>
        {
            Assert.That(graph.IsAdjacentEdgesEmpty(vertex), Is.True);
            Assert.That(0, Is.EqualTo(graph.AdjacentDegree(vertex)));
            CollectionAssert.IsEmpty(graph.AdjacentEdges(vertex));
        }

        public static void AssertHasAdjacentEdges<TVertex, TEdge>(
            IImplicitUndirectedGraph<TVertex, TEdge> graph,
            TVertex vertex,
            IEnumerable<TEdge> edges,
            int degree = -1) // If not set => equals the count of edges
            where TEdge : IEdge<TVertex>
        {
            TEdge[] edgeArray = edges.ToArray();
            CollectionAssert.IsNotEmpty(edgeArray);

            Assert.That(graph.IsAdjacentEdgesEmpty(vertex), Is.False);
            Assert.That(degree < 0 ? edgeArray.Length : degree, Is.EqualTo(graph.AdjacentDegree(vertex)));
            CollectionAssert.AreEquivalent(edgeArray, graph.AdjacentEdges(vertex));
        }


        public static void AssertEquivalentGraphs<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> expected,
            IEdgeListGraph<TVertex, TEdge> actual)
            where TEdge : IEdge<TVertex>
        {
            Assert.That(expected.IsDirected, Is.EqualTo(actual.IsDirected));
            Assert.That(expected.AllowParallelEdges, Is.EqualTo(actual.AllowParallelEdges));

            if (expected.IsVerticesEmpty)
            {
                AssertNoVertex(actual);
            }
            else
            {
                AssertHasVertices(actual, expected.Vertices);
            }

            if (expected.IsEdgesEmpty)
            {
                AssertNoEdge(actual);
            }
            else
            {
                AssertHasEdges(actual, expected.Edges);
            }
        }

        #endregion
    }
}