using System;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Remove Vertices

        protected static void RemoveVertex_Test(
             IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                Assert.That(v,Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e,Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveVertex(5),Is.False);
            CheckCounters(0, 0);

            Assert.That(graph.RemoveVertex(3),Is.True);
            CheckCounters(1, 3);
            AssertHasVertices(graph, [1, 2, 4]);
            AssertHasEdges(graph, [edge12, edge14, edge24]);

            Assert.That(graph.RemoveVertex(1),Is.True);
            CheckCounters(1, 2);
            AssertHasVertices(graph, [2, 4]);
            AssertHasEdges(graph, [edge24]);

            Assert.That(graph.RemoveVertex(2),Is.True);
            CheckCounters(1, 1);
            AssertHasVertices(graph, [4]);
            AssertNoEdge(graph);

            Assert.That(graph.RemoveVertex(4),Is.True);
            CheckCounters(1, 0);
            AssertEmptyGraph(graph);

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                Assert.That(expectedRemovedVertices,Is.EqualTo(verticesRemoved));
                Assert.That(expectedRemovedEdges,Is.EqualTo(edgesRemoved));
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveVertex_Clusters_Test(
             ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveVertex(5),Is.False);

            Assert.That(graph.RemoveVertex(3),Is.True);
            AssertHasVertices(graph, [1, 2, 4]);
            AssertHasEdges(graph, [edge12, edge14, edge24]);

            Assert.That(graph.RemoveVertex(1),Is.True);
            AssertHasVertices(graph, [2, 4]);
            AssertHasEdges(graph, [edge24]);

            Assert.That(graph.RemoveVertex(2),Is.True);
            AssertHasVertices(graph, [4]);
            AssertNoEdge(graph);

            Assert.That(graph.RemoveVertex(4),Is.True);
            AssertEmptyGraph(graph);


            // With cluster
            ClusteredAdjacencyGraph<int, Edge<int>> cluster1 = graph.AddCluster();
            ClusteredAdjacencyGraph<int, Edge<int>> cluster2 = graph.AddCluster();
            ClusteredAdjacencyGraph<int, Edge<int>> cluster3 = graph.AddCluster();

            cluster1.AddVertexRange([1, 2]);
            AssertHasVertices(cluster1, [1, 2]);

            cluster2.AddVertexRange([1, 2, 4]);
            AssertHasVertices(cluster2, [1, 2, 4]);

            cluster3.AddVertex(2);
            AssertHasVertices(cluster3, [2]);

            graph.AddVertexRange([1, 2, 3, 4]);
            AssertHasVertices(graph, [1, 2, 3, 4]);


            graph.RemoveVertex(2);
            AssertHasVertices(graph, [1, 3, 4]);
            AssertHasVertices(cluster1, [1]);
            AssertHasVertices(cluster2, [1, 4]);
            AssertNoVertex(cluster3);

            graph.RemoveVertex(1);
            AssertHasVertices(graph, [3, 4]);
            AssertNoVertex(cluster1);
            AssertHasVertices(cluster2, [4]);
            AssertNoVertex(cluster3);

            graph.RemoveVertex(3);
            AssertHasVertices(graph, [4]);
            AssertNoVertex(cluster1);
            AssertHasVertices(cluster2, [4]);
            AssertNoVertex(cluster3);

            graph.RemoveVertex(4);
            AssertNoVertex(graph);
            AssertNoVertex(cluster1);
            AssertNoVertex(cluster2);
            AssertNoVertex(cluster3);
        }

        protected static void RemoveVertex_Throws_Test<TVertex>(
             IMutableVertexSet<TVertex> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertex(null));
        }

        protected static void RemoveVertex_Throws_Clusters_Test<TVertex, TEdge>(
             ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertex(null));
        }

        protected static void RemoveVertexIf_Test(
             IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                Assert.That(v,Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e,Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge14, edge24, edge31, edge33]);

            Assert.That(0,Is.EqualTo(graph.RemoveVertexIf(vertex => vertex > 10)));
            CheckCounters(0, 0);

            Assert.That(2,Is.EqualTo(graph.RemoveVertexIf(vertex => vertex > 2)));
            CheckCounters(2, 5);
            AssertHasVertices(graph, [1, 2]);
            AssertHasEdges(graph, [edge12]);

            Assert.That(2,Is.EqualTo(graph.RemoveVertexIf(_ => true)));
            CheckCounters(2, 1);
            AssertEmptyGraph(graph);

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                Assert.That(expectedRemovedVertices,Is.EqualTo(verticesRemoved));
                Assert.That(expectedRemovedEdges,Is.EqualTo(edgesRemoved));
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveVertexIf_Test2(
             IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                Assert.That(v,Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e,Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge11 = new Edge<int>(1, 1);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            var edge34 = new Edge<int>(3, 4);
            graph.AddVerticesAndEdgeRange([edge11, edge13, edge24, edge31, edge32, edge34]);

            Assert.That(2,Is.EqualTo(graph.RemoveVertexIf(vertex => vertex == 1 || vertex  == 3)));
            CheckCounters(2, 5);
            AssertHasVertices(graph, [2, 4]);
            AssertHasEdges(graph, [edge24]);

            #region Local function

            void CheckCounters(int expectedRemovedVertices, int expectedRemovedEdges)
            {
                Assert.That(expectedRemovedVertices,Is.EqualTo(verticesRemoved));
                Assert.That(expectedRemovedEdges,Is.EqualTo(edgesRemoved));
                verticesRemoved = 0;
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveVertexIf_Clusters_Test(
             ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge14, edge24, edge31, edge33]);

            Assert.That(0,Is.EqualTo(graph.RemoveVertexIf(vertex => vertex > 10)));

            Assert.That(2,Is.EqualTo(graph.RemoveVertexIf(vertex => vertex > 2)));
            AssertHasVertices(graph, [1, 2]);
            AssertHasEdges(graph, [edge12]);

            Assert.That(2,Is.EqualTo(graph.RemoveVertexIf(_ => true)));
            AssertEmptyGraph(graph);
        }

        protected static void RemoveVertexIf_Clusters_Test2(
             ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge13 = new Edge<int>(1, 3);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            var edge34 = new Edge<int>(3, 4);
            graph.AddVerticesAndEdgeRange([edge11, edge13, edge24, edge31, edge32, edge34]);

            Assert.That(2,Is.EqualTo(graph.RemoveVertexIf(vertex => vertex == 1 || vertex == 3)));
            AssertHasVertices(graph, [2, 4]);
            AssertHasEdges(graph, [edge24]);
        }

        protected static void RemoveVertexIf_Throws_Test<TVertex>(
             IMutableVertexSet<TVertex> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertexIf(null));
        }

        protected static void RemoveVertexIf_Throws_Clusters_Test<TVertex, TEdge>(
             ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveVertexIf(null));
        }

        #endregion
    }
}