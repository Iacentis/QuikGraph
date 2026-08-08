using System;
using System.Linq;
using NUnit.Framework;
using static QuikGraph.Tests.AssertHelpers;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region In Edges

        protected static void InEdge_Test(
            IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge13 = new Edge<int>(1, 3);
            var edge21 = new Edge<int>(2, 1);
            var edge41 = new Edge<int>(4, 1);

            graph.AddVerticesAndEdgeRange([edge11, edge13, edge21, edge41]);

            Assert.That(edge11, Is.SameAs(graph.InEdge(1, 0)));
            Assert.That(edge41, Is.SameAs(graph.InEdge(1, 2)));
            Assert.That(edge13, Is.SameAs(graph.InEdge(3, 0)));
        }

        protected static void InEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IBidirectionalIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge13 = new Edge<int>(1, 3);
            var edge21 = new Edge<int>(2, 1);
            var edge41 = new Edge<int>(4, 1);

            wrappedGraph.AddVerticesAndEdgeRange([edge11, edge13, edge21, edge41]);
            IBidirectionalIncidenceGraph<int, Edge<int>> graph = createGraph();

            Assert.That(edge11, Is.SameAs(graph.InEdge(1, 0)));
            Assert.That(edge41, Is.SameAs(graph.InEdge(1, 2)));
            Assert.That(edge13, Is.SameAs(graph.InEdge(3, 0)));
        }

        protected static void InEdge_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge14 = new Edge<int>(1, 4);
            var edge21 = new Edge<int>(2, 1);
            var edge31 = new Edge<int>(3, 1);
            var edge33 = new Edge<int>(3, 3);
            var edge42 = new Edge<int>(4, 2);

            graph.AddEdgeRange([edge11, edge14, edge21, edge31, edge33, edge42]);

            Assert.That(edge11, Is.SameAs(graph.InEdge(1, 0)));
            Assert.That(edge31, Is.SameAs(graph.InEdge(1, 2)));
            Assert.That(edge42, Is.SameAs(graph.InEdge(2, 0)));
            Assert.That(edge33, Is.SameAs(graph.InEdge(3, 0)));
            Assert.That(edge14, Is.SameAs(graph.InEdge(4, 0)));
        }

        protected static void InEdge_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge31 = new Edge<int>(3, 1);
            var edge32 = new Edge<int>(3, 2);
            var edge34 = new Edge<int>(3, 4);

            wrappedGraph.AddVerticesAndEdgeRange([edge11, edge31, edge32, edge34]);
            IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            AssertSameReversedEdge(edge11, graph.InEdge(1, 0));
            AssertSameReversedEdge(edge31, graph.InEdge(3, 0));
            AssertSameReversedEdge(edge34, graph.InEdge(3, 2));
        }

        protected static void InEdge_NullThrows_Test<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.InEdge(null, 0));
        }

        protected static void InEdge_Throws_Test(
            IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph.InEdge(vertex1, 0));

            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);
            AssertIndexOutOfRange(() => graph.InEdge(vertex1, 0));

            graph.AddEdge(new Edge<int>(1, 2));
            AssertIndexOutOfRange(() => graph.InEdge(vertex1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void InEdge_Throws_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IBidirectionalIncidenceGraph<int, Edge<int>>> createGraph)
        {
            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IBidirectionalIncidenceGraph<int, Edge<int>> graph1 = createGraph();
            Assert.Throws<VertexNotFoundException>(() => graph1.InEdge(vertex1, 0));

            wrappedGraph.AddVertex(vertex1);
            wrappedGraph.AddVertex(vertex2);
            graph1 = createGraph();
            AssertIndexOutOfRange(() => graph1.InEdge(vertex1, 0));

            wrappedGraph.AddEdge(new Edge<int>(1, 2));
            graph1 = createGraph();
            AssertIndexOutOfRange(() => graph1.InEdge(vertex1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void InEdge_Throws_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph.InEdge(-1, 0));
            Assert.Throws<VertexNotFoundException>(() => graph.InEdge(4, 0));

            graph.AddEdge(new Edge<int>(2, 1));
            AssertIndexOutOfRange(() => graph.InEdge(1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void InEdge_Throws_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            const int vertex1 = 1;
            const int vertex2 = 2;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();
            Assert.Throws<VertexNotFoundException>(() => graph.InEdge(vertex1, 0));

            wrappedGraph.AddVertex(vertex1);
            wrappedGraph.AddVertex(vertex2);
            graph = createGraph();
            AssertIndexOutOfRange(() => graph.InEdge(vertex1, 0));

            wrappedGraph.AddEdge(new Edge<int>(1, 2));
            graph = createGraph();
            AssertIndexOutOfRange(() => graph.InEdge(vertex1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void InEdges_Test(
            IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge32 = new Edge<int>(3, 2);
            var edge33 = new Edge<int>(3, 3);

            graph.AddVertex(1);
            AssertNoInEdge(graph, 1);
            AssertNoOutEdge(graph, 1);

            graph.AddVerticesAndEdgeRange([edge12, edge13, edge14, edge24, edge32, edge33]);

            AssertHasOutEdges(graph, 1, [edge12, edge13, edge14]);
            AssertHasOutEdges(graph, 2, [edge24]);
            AssertHasOutEdges(graph, 3, [edge32, edge33]);
            AssertNoOutEdge(graph, 4);

            AssertNoInEdge(graph, 1);
            AssertHasInEdges(graph, 2, [edge12, edge32]);
            AssertHasInEdges(graph, 3, [edge13, edge33]);
            AssertHasInEdges(graph, 4, [edge14, edge24]);
        }

        protected static void InEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IBidirectionalIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge24 = new Edge<int>(2, 4);
            var edge32 = new Edge<int>(3, 2);
            var edge33 = new Edge<int>(3, 3);

            wrappedGraph.AddVertex(1);
            IBidirectionalIncidenceGraph<int, Edge<int>> graph = createGraph();
            AssertNoInEdge(graph, 1);
            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVerticesAndEdgeRange([edge12, edge13, edge14, edge24, edge32, edge33]);
            graph = createGraph();

            AssertHasOutEdges(graph, 1, [edge12, edge13, edge14]);
            AssertHasOutEdges(graph, 2, [edge24]);
            AssertHasOutEdges(graph, 3, [edge32, edge33]);
            AssertNoOutEdge(graph, 4);

            AssertNoInEdge(graph, 1);
            AssertHasInEdges(graph, 2, [edge12, edge32]);
            AssertHasInEdges(graph, 3, [edge13, edge33]);
            AssertHasInEdges(graph, 4, [edge14, edge24]);
        }

        protected static void InEdges_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge02 = new Edge<int>(0, 2);
            var edge10 = new Edge<int>(1, 0);
            var edge20 = new Edge<int>(2, 0);
            var edge22 = new Edge<int>(2, 2);
            var edge30 = new Edge<int>(3, 0);
            var edge31 = new Edge<int>(3, 1);

            AssertNoInEdge(graph, 1);

            graph.AddEdgeRange([edge02, edge10, edge20, edge22, edge30, edge31]);

            AssertHasInEdges(graph, 0, [edge10, edge20, edge30]);
            AssertHasInEdges(graph, 1, [edge31]);
            AssertHasInEdges(graph, 2, [edge02, edge22]);
            AssertNoInEdge(graph, 3);
        }

        protected static void InEdges_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge33 = new Edge<int>(3, 3);
            var edge34 = new Edge<int>(3, 4);
            var edge43 = new Edge<int>(4, 3);

            wrappedGraph.AddVertex(1);
            IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();
            AssertNoInEdge(graph, 1);
            AssertNoOutEdge(graph, 1);

            wrappedGraph.AddVerticesAndEdgeRange([edge12, edge13, edge14, edge33, edge34, edge43]);
            graph = createGraph();

            AssertNoOutEdge(graph, 1);
            AssertHasReversedOutEdges(graph, 2, [edge12]);
            AssertHasReversedOutEdges(graph, 3, [edge13, edge33, edge43]);
            AssertHasReversedOutEdges(graph, 4, [edge14, edge34]);

            AssertHasReversedInEdges(graph, 1, [edge12, edge13, edge14]);
            AssertNoInEdge(graph, 2);
            AssertHasReversedInEdges(graph, 3, [edge33, edge34]);
            AssertHasReversedInEdges(graph, 4, [edge43]);
        }

        protected static void InEdges_NullThrows_Test<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.IsInEdgesEmpty(null));
            Assert.Throws<ArgumentNullException>(() => graph.InDegree(null));
            Assert.Throws<ArgumentNullException>(() => graph.InEdges(null).ToArray());
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void InEdges_Throws_Test<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : IEquatable<TVertex>, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var vertex = new TVertex();
            Assert.Throws<VertexNotFoundException>(() => graph.IsInEdgesEmpty(vertex));
            Assert.Throws<VertexNotFoundException>(() => graph.InDegree(vertex));
            Assert.Throws<VertexNotFoundException>(() => graph.InEdges(vertex).ToArray());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void InEdges_Throws_Matrix_Test<TEdge>(
            BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            const int vertex = 10;
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<VertexNotFoundException>(() => graph.IsInEdgesEmpty(vertex));
            Assert.Throws<VertexNotFoundException>(() => graph.InDegree(vertex));
            Assert.Throws<VertexNotFoundException>(() => graph.InEdges(vertex).ToArray());
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion
    }
}