using System;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Remove Edges

        protected static void RemoveEdge_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                Assert.That(v, Is.Not.Null);
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edgeNotInGraph = new Edge<int>(3, 4);
            var edgeWithVertexNotInGraph1 = new Edge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new Edge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new Edge<int>(10, 11);
            var edgeNotEquatable = new Edge<int>(1, 2);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edgeNotInGraph), Is.False);
            CheckCounters(0);

            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph1), Is.False);
            CheckCounters(0);

            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph2), Is.False);
            CheckCounters(0);

            Assert.That(graph.RemoveEdge(edgeWithVerticesNotInGraph), Is.False);
            CheckCounters(0);

            Assert.That(graph.RemoveEdge(edgeNotEquatable), Is.False);
            CheckCounters(0);

            Assert.That(graph.RemoveEdge(edge13Bis), Is.True);
            CheckCounters(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge13, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edge31), Is.True);
            CheckCounters(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge13, edge14, edge24, edge33]);

            Assert.That(graph.RemoveEdge(edge12), Is.True);
            Assert.That(graph.RemoveEdge(edge13), Is.True);
            Assert.That(graph.RemoveEdge(edge14), Is.True);
            Assert.That(graph.RemoveEdge(edge24), Is.True);
            Assert.That(graph.RemoveEdge(edge33), Is.True);
            CheckCounters(5);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.That(0, Is.EqualTo(verticesRemoved));
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edgeNotInGraph = new Edge<int>(3, 4);
            var edgeNotEquatable = new Edge<int>(1, 2);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edgeNotInGraph), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edgeNotEquatable), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edge13Bis), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge13, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edge31), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge13, edge14, edge24, edge33]);

            Assert.That(graph.RemoveEdge(edge12), Is.True);
            Assert.That(graph.RemoveEdge(edge13), Is.True);
            Assert.That(graph.RemoveEdge(edge14), Is.True);
            Assert.That(graph.RemoveEdge(edge24), Is.True);
            Assert.That(graph.RemoveEdge(edge33), Is.True);
            CheckCounter(5);
            AssertEmptyGraph(graph); // Vertices removed in the same time as edges

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge13 = new Edge<int>(1, 3);
            var edge20 = new Edge<int>(2, 0);
            var edge22 = new Edge<int>(2, 2);
            var edgeNotInGraph = new Edge<int>(2, 3);
            var edgeWithVertexNotInGraph1 = new Edge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new Edge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new Edge<int>(10, 11);
            var edgeNotEquatable = new Edge<int>(0, 1);
            graph.AddEdgeRange([edge01, edge02, edge03, edge13, edge20, edge22]);

            Assert.That(graph.RemoveEdge(edgeNotInGraph), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph1), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph2), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edgeWithVerticesNotInGraph), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edgeNotEquatable), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [0, 1, 2, 3]);
            AssertHasEdges(graph, [edge02, edge03, edge13, edge20, edge22]);

            Assert.That(graph.RemoveEdge(edge02), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [0, 1, 2, 3]);
            AssertHasEdges(graph, [edge03, edge13, edge20, edge22]);

            Assert.That(graph.RemoveEdge(edge20), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [0, 1, 2, 3]);
            AssertHasEdges(graph, [edge03, edge13, edge22]);

            Assert.That(graph.RemoveEdge(edge03), Is.True);
            Assert.That(graph.RemoveEdge(edge13), Is.True);
            Assert.That(graph.RemoveEdge(edge22), Is.True);
            CheckCounter(3);
            AssertHasVertices(graph, [0, 1, 2, 3]);
            AssertNoEdge(graph);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edgeNotInGraph = new Edge<int>(3, 4);
            var edgeWithVertexNotInGraph1 = new Edge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new Edge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new Edge<int>(10, 11);
            var edgeNotEquatable = new Edge<int>(1, 2);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edgeNotInGraph), Is.False);
            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph1), Is.False);
            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph2), Is.False);
            Assert.That(graph.RemoveEdge(edgeWithVerticesNotInGraph), Is.False);
            Assert.That(graph.RemoveEdge(edgeNotEquatable), Is.False);

            Assert.That(graph.RemoveEdge(edge13Bis), Is.True);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge13, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edge31), Is.True);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge13, edge14, edge24, edge33]);

            Assert.That(graph.RemoveEdge(edge12), Is.True);
            Assert.That(graph.RemoveEdge(edge13), Is.True);
            Assert.That(graph.RemoveEdge(edge14), Is.True);
            Assert.That(graph.RemoveEdge(edge24), Is.True);
            Assert.That(graph.RemoveEdge(edge33), Is.True);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertNoEdge(graph);


            // With cluster
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge14, edge24, edge31]);
            AssertHasEdges(graph, [edge12, edge13, edge14, edge24, edge31]);

            ClusteredAdjacencyGraph<int, Edge<int>> cluster1 = graph.AddCluster();
            ClusteredAdjacencyGraph<int, Edge<int>> cluster2 = graph.AddCluster();
            ClusteredAdjacencyGraph<int, Edge<int>> cluster3 = graph.AddCluster();

            cluster1.AddVerticesAndEdgeRange([edge12, edge13]);
            AssertHasEdges(cluster1, [edge12, edge13]);

            cluster2.AddVerticesAndEdgeRange([edge12, edge14, edge24]);
            AssertHasEdges(cluster2, [edge12, edge14, edge24]);

            cluster3.AddVerticesAndEdge(edge12);
            AssertHasEdges(cluster3, [edge12]);


            graph.RemoveEdge(edge12);
            AssertHasEdges(graph, [edge13, edge14, edge24, edge31]);
            AssertHasEdges(cluster1, [edge13]);
            AssertHasEdges(cluster2, [edge14, edge24]);
            AssertNoEdge(cluster3);

            graph.RemoveEdge(edge13);
            AssertHasEdges(graph, [edge14, edge24, edge31]);
            AssertNoEdge(cluster1);
            AssertHasEdges(cluster2, [edge14, edge24]);
            AssertNoEdge(cluster3);

            graph.RemoveEdge(edge24);
            AssertHasEdges(graph, [edge14, edge31]);
            AssertNoEdge(cluster1);
            AssertHasEdges(cluster2, [edge14]);
            AssertNoEdge(cluster3);

            graph.RemoveEdge(edge14);
            AssertHasEdges(graph, [edge31]);
            AssertNoEdge(cluster1);
            AssertNoEdge(cluster2);
            AssertNoEdge(cluster3);
        }

        protected static void RemoveEdge_EquatableEdge_Test(
            IMutableVertexAndEdgeSet<int, EquatableEdge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                Assert.That(v, Is.Not.Null);
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new EquatableEdge<int>(1, 2);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge13Bis = new EquatableEdge<int>(1, 3);
            var edge14 = new EquatableEdge<int>(1, 4);
            var edge24 = new EquatableEdge<int>(2, 4);
            var edge31 = new EquatableEdge<int>(3, 1);
            var edge33 = new EquatableEdge<int>(3, 3);
            var edgeNotInGraph = new EquatableEdge<int>(3, 4);
            var edgeWithVertexNotInGraph1 = new EquatableEdge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new EquatableEdge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new EquatableEdge<int>(10, 11);
            var edgeEquatable = new EquatableEdge<int>(1, 2);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edgeNotInGraph), Is.False);
            CheckCounters(0);

            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph1), Is.False);
            CheckCounters(0);

            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph2), Is.False);
            CheckCounters(0);

            Assert.That(graph.RemoveEdge(edgeWithVerticesNotInGraph), Is.False);
            CheckCounters(0);

            Assert.That(graph.RemoveEdge(edgeEquatable), Is.True);
            CheckCounters(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edge13Bis), Is.True);
            CheckCounters(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge13, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edge31), Is.True);
            CheckCounters(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge13, edge14, edge24, edge33]);

            Assert.That(graph.RemoveEdge(edge13), Is.True);
            Assert.That(graph.RemoveEdge(edge14), Is.True);
            Assert.That(graph.RemoveEdge(edge24), Is.True);
            Assert.That(graph.RemoveEdge(edge33), Is.True);
            CheckCounters(4);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.That(0, Is.EqualTo(verticesRemoved));
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EquatableEdge_EdgesOnly_Test(
            EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new EquatableEdge<int>(1, 2);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge14 = new EquatableEdge<int>(1, 4);
            var edge24 = new EquatableEdge<int>(2, 4);
            var edge31 = new EquatableEdge<int>(3, 1);
            var edge33 = new EquatableEdge<int>(3, 3);
            var edgeNotInGraph = new EquatableEdge<int>(3, 4);
            var edgeEquatable = new EquatableEdge<int>(1, 2);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edgeNotInGraph), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edgeEquatable), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge13, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edge13), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edge31), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge14, edge24, edge33]);

            Assert.That(graph.RemoveEdge(edge14), Is.True);
            Assert.That(graph.RemoveEdge(edge24), Is.True);
            Assert.That(graph.RemoveEdge(edge33), Is.True);
            CheckCounter(3);
            AssertEmptyGraph(graph); // Vertices removed in the same time as edges

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EquatableEdge_ImmutableVertices_Test(
            BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge01 = new EquatableEdge<int>(0, 1);
            var edge02 = new EquatableEdge<int>(0, 2);
            var edge03 = new EquatableEdge<int>(0, 3);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge20 = new EquatableEdge<int>(2, 0);
            var edge22 = new EquatableEdge<int>(2, 2);
            var edgeNotInGraph = new EquatableEdge<int>(2, 3);
            var edgeWithVertexNotInGraph1 = new EquatableEdge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new EquatableEdge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new EquatableEdge<int>(10, 11);
            var edgeNotEquatable = new EquatableEdge<int>(0, 1);
            graph.AddEdgeRange([edge01, edge02, edge03, edge13, edge20, edge22]);

            Assert.That(graph.RemoveEdge(edgeNotInGraph), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph1), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph2), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edgeWithVerticesNotInGraph), Is.False);
            CheckCounter(0);

            Assert.That(graph.RemoveEdge(edgeNotEquatable), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [0, 1, 2, 3]);
            AssertHasEdges(graph, [edge02, edge03, edge13, edge20, edge22]);

            Assert.That(graph.RemoveEdge(edge02), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [0, 1, 2, 3]);
            AssertHasEdges(graph, [edge03, edge13, edge20, edge22]);

            Assert.That(graph.RemoveEdge(edge20), Is.True);
            CheckCounter(1);
            AssertHasVertices(graph, [0, 1, 2, 3]);
            AssertHasEdges(graph, [edge03, edge13, edge22]);

            Assert.That(graph.RemoveEdge(edge03), Is.True);
            Assert.That(graph.RemoveEdge(edge13), Is.True);
            Assert.That(graph.RemoveEdge(edge22), Is.True);
            CheckCounter(3);
            AssertHasVertices(graph, [0, 1, 2, 3]);
            AssertNoEdge(graph);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdge_EquatableEdge_Clusters_Test(
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            var edge12 = new EquatableEdge<int>(1, 2);
            var edge13 = new EquatableEdge<int>(1, 3);
            var edge13Bis = new EquatableEdge<int>(1, 3);
            var edge14 = new EquatableEdge<int>(1, 4);
            var edge24 = new EquatableEdge<int>(2, 4);
            var edge31 = new EquatableEdge<int>(3, 1);
            var edge33 = new EquatableEdge<int>(3, 3);
            var edgeNotInGraph = new EquatableEdge<int>(3, 4);
            var edgeWithVertexNotInGraph1 = new EquatableEdge<int>(2, 10);
            var edgeWithVertexNotInGraph2 = new EquatableEdge<int>(10, 2);
            var edgeWithVerticesNotInGraph = new EquatableEdge<int>(10, 11);
            var edgeEquatable = new EquatableEdge<int>(1, 2);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edgeNotInGraph), Is.False);
            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph1), Is.False);
            Assert.That(graph.RemoveEdge(edgeWithVertexNotInGraph2), Is.False);
            Assert.That(graph.RemoveEdge(edgeWithVerticesNotInGraph), Is.False);

            Assert.That(graph.RemoveEdge(edgeEquatable), Is.True);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edge13Bis), Is.True);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge13, edge14, edge24, edge31, edge33]);

            Assert.That(graph.RemoveEdge(edge31), Is.True);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge13, edge14, edge24, edge33]);

            Assert.That(graph.RemoveEdge(edge13), Is.True);
            Assert.That(graph.RemoveEdge(edge14), Is.True);
            Assert.That(graph.RemoveEdge(edge24), Is.True);
            Assert.That(graph.RemoveEdge(edge33), Is.True);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertNoEdge(graph);
        }

        protected static void RemoveEdge_Throws_Test<TVertex, TEdge>(
            IMutableEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdge(null));
        }

        protected static void RemoveEdge_Throws_Clusters_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdge(null));
        }

        protected static void RemoveEdgeIf_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                Assert.That(v, Is.Not.Null);
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVertexRange([1, 2, 3, 4]);
            graph.AddEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(0, Is.EqualTo(graph.RemoveEdgeIf(edge => edge.Target == 5)));
            CheckCounters(0);

            Assert.That(2, Is.EqualTo(graph.RemoveEdgeIf(edge => edge.Source == 3)));
            CheckCounters(2);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge13, edge13Bis, edge14, edge24]);

            Assert.That(5, Is.EqualTo(graph.RemoveEdgeIf(_ => true)));
            CheckCounters(5);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertNoEdge(graph);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.That(0, Is.EqualTo(verticesRemoved));
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdgeIf_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(0, Is.EqualTo(graph.RemoveEdgeIf(edge => edge.Target == 5)));
            CheckCounter(0);

            Assert.That(2, Is.EqualTo(graph.RemoveEdgeIf(edge => edge.Source == 3)));
            CheckCounter(2);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge13, edge13Bis, edge14, edge24]);

            Assert.That(5, Is.EqualTo(graph.RemoveEdgeIf(_ => true)));
            CheckCounter(5);
            AssertEmptyGraph(graph); // Vertices removed in the same time as edges

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveEdgeIf_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVertexRange([1, 2, 3, 4]);
            graph.AddEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(0, Is.EqualTo(graph.RemoveEdgeIf(edge => edge.Target == 5)));

            Assert.That(2, Is.EqualTo(graph.RemoveEdgeIf(edge => edge.Source == 3)));
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge13, edge13Bis, edge14, edge24]);

            Assert.That(5, Is.EqualTo(graph.RemoveEdgeIf(_ => true)));
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertNoEdge(graph);
        }

        protected static void RemoveEdgeIf_Throws_Test<TVertex, TEdge>(
            IMutableEdgeListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdgeIf(null));
        }

        protected static void RemoveEdgeIf_Throws_Clusters_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveEdgeIf(null));
        }

        protected static void RemoveOutEdgeIf_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                Assert.That(v, Is.Not.Null);
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.That(0, Is.EqualTo(graph.RemoveOutEdgeIf(1,_ => true)));
            CheckCounters(0);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(3, Is.EqualTo(graph.RemoveOutEdgeIf(1, edge => edge.Target >= 3)));
            CheckCounters(3);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge24, edge31, edge33]);

            Assert.That(0, Is.EqualTo(graph.RemoveOutEdgeIf(3, edge => edge.Target > 5)));
            CheckCounters(0);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge24, edge31, edge33]);

            Assert.That(2, Is.EqualTo(graph.RemoveOutEdgeIf(3, _ => true)));
            CheckCounters(2);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge24]);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.That(0, Is.EqualTo(verticesRemoved));
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveOutEdgeIf_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.That(0, Is.EqualTo(graph.RemoveOutEdgeIf(6, _ => true)));
            CheckCounter(0);
            AssertNoEdge(graph);

            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge13 = new Edge<int>(1, 3);
            var edge20 = new Edge<int>(2, 0);
            var edge22 = new Edge<int>(2, 2);
            graph.AddEdgeRange([edge01, edge02, edge03, edge13, edge20, edge22]);

            Assert.That(2, Is.EqualTo(graph.RemoveOutEdgeIf(0, edge => edge.Target >= 2)));
            CheckCounter(2);
            AssertHasEdges(graph, [edge01, edge13, edge20, edge22]);

            Assert.That(0, Is.EqualTo(graph.RemoveOutEdgeIf(2, edge => edge.Target > 4)));
            CheckCounter(0);
            AssertHasEdges(graph, [edge01, edge13, edge20, edge22]);

            Assert.That(2, Is.EqualTo(graph.RemoveOutEdgeIf(2, _ => true)));
            CheckCounter(2);
            AssertHasEdges(graph, [edge01, edge13]);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveOutEdgeIf_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            Assert.That(0, Is.EqualTo(graph.RemoveOutEdgeIf(1,_ => true)));
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(3, Is.EqualTo(graph.RemoveOutEdgeIf(1,edge => edge.Target >= 3)));
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge24, edge31, edge33]);

            Assert.That(0, Is.EqualTo(graph.RemoveOutEdgeIf(3,edge => edge.Target > 5)));
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge24, edge31, edge33]);

            Assert.That(2, Is.EqualTo(graph.RemoveOutEdgeIf(3,_ => true)));
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge24]);
        }

        protected static void RemoveOutEdgeIf_Throws_Test<TEdge>(
            BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(default, null));
        }

        protected static void RemoveOutEdgeIf_Throws_Test<TVertex, TEdge>(
            IMutableIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, _ => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(new TVertex(), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void RemoveOutEdgeIf_Throws_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, _ => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(new TVertex(), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveOutEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void RemoveInEdgeIf_Test(
            IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                Assert.That(v, Is.Not.Null);
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.That(0, Is.EqualTo(graph.RemoveInEdgeIf(1,_ => true)));
            CheckCounters(0);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(2, Is.EqualTo(graph.RemoveInEdgeIf(3,edge => edge.Source == 1)));
            CheckCounters(2);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge14, edge24, edge31, edge33]);

            Assert.That(0, Is.EqualTo(graph.RemoveInEdgeIf(3,edge => edge.Target > 5)));
            CheckCounters(0);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge14, edge24, edge31, edge33]);

            Assert.That(1, Is.EqualTo(graph.RemoveInEdgeIf(2,_ => true)));
            CheckCounters(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge14, edge24, edge31, edge33]);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.That(0, Is.EqualTo(verticesRemoved));
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveInEdgeIf_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.That(0, Is.EqualTo(graph.RemoveInEdgeIf(6,_ => true)));
            CheckCounter(0);
            AssertNoEdge(graph);

            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge13 = new Edge<int>(1, 3);
            var edge20 = new Edge<int>(2, 0);
            var edge22 = new Edge<int>(2, 2);
            graph.AddEdgeRange([edge01, edge02, edge03, edge13, edge20, edge22]);

            Assert.That(1, Is.EqualTo(graph.RemoveInEdgeIf(2,edge => edge.Source == 0)));
            CheckCounter(1);
            AssertHasEdges(graph, [edge01, edge03, edge13, edge20, edge22]);

            Assert.That(0, Is.EqualTo(graph.RemoveInEdgeIf(2,edge => edge.Target > 4)));
            CheckCounter(0);
            AssertHasEdges(graph, [edge01, edge03, edge13, edge20, edge22]);

            Assert.That(1, Is.EqualTo(graph.RemoveInEdgeIf(1,_ => true)));
            CheckCounter(1);
            AssertHasEdges(graph, [edge03, edge13, edge20, edge22]);

            #region Local function

            void CheckCounter(int expectedRemovedEdges)
            {
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveInEdgeIf_Throws_Test<TEdge>(
            BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(default, null));
        }

        protected static void RemoveInEdgeIf_Throws_Test(
            IMutableBidirectionalGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(null, _ => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveInEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void RemoveAdjacentEdgeIf_Test(
            IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            int verticesRemoved = 0;
            int edgesRemoved = 0;

            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexRemoved += v =>
            {
                Assert.That(v, Is.Not.Null);
                ++verticesRemoved;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeRemoved += e =>
            {
                Assert.That(e, Is.Not.Null);
                // ReSharper disable once AccessToModifiedClosure
                ++edgesRemoved;
            };

            Assert.That(0, Is.EqualTo(graph.RemoveAdjacentEdgeIf(1,_ => true)));
            CheckCounters(0);
            AssertEmptyGraph(graph);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge13Bis = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            graph.AddVerticesAndEdgeRange([edge12, edge13, edge13Bis, edge14, edge24, edge31, edge33]);

            Assert.That(4, Is.EqualTo(graph.RemoveAdjacentEdgeIf(1,edge => edge.Source >= 3 || edge.Target >= 3)));
            CheckCounters(4);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge24, edge33]);

            Assert.That(0, Is.EqualTo(graph.RemoveAdjacentEdgeIf(3,edge => edge.Target > 5)));
            CheckCounters(0);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge24, edge33]);

            Assert.That(1, Is.EqualTo(graph.RemoveAdjacentEdgeIf(3,_ => true)));
            CheckCounters(1);
            AssertHasVertices(graph, [1, 2, 3, 4]);
            AssertHasEdges(graph, [edge12, edge24]);

            #region Local function

            void CheckCounters(int expectedRemovedEdges)
            {
                Assert.That(0, Is.EqualTo(verticesRemoved));
                Assert.That(expectedRemovedEdges, Is.EqualTo(edgesRemoved));
                edgesRemoved = 0;
            }

            #endregion
        }

        protected static void RemoveAdjacentEdgeIf_Throws_Test(
            IMutableUndirectedGraph<TestVertex, Edge<TestVertex>> graph)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.RemoveAdjacentEdgeIf(null, _ => true));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveAdjacentEdgeIf(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => graph.RemoveAdjacentEdgeIf(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion
    }
}