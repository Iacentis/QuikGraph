using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Degree

        protected static void Degree_Test(
             IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            graph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);
            graph.AddVertex(5);

            Assert.That(3,Is.EqualTo(graph.Degree(1)));
            Assert.That(3,Is.EqualTo(graph.Degree(2)));
            Assert.That(4,Is.EqualTo(graph.Degree(3))); // Self edge
            Assert.That(2,Is.EqualTo(graph.Degree(4)));
            Assert.That(0,Is.EqualTo(graph.Degree(5)));
        }

        protected static void Degree_ImmutableGraph_Test(
             IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
             Func<IBidirectionalIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);
            wrappedGraph.AddVertex(5);
            IBidirectionalIncidenceGraph<int, Edge<int>> graph = createGraph();

            Assert.That(3,Is.EqualTo(graph.Degree(1)));
            Assert.That(3,Is.EqualTo(graph.Degree(2)));
            Assert.That(4,Is.EqualTo(graph.Degree(3))); // Self edge
            Assert.That(2,Is.EqualTo(graph.Degree(4)));
            Assert.That(0,Is.EqualTo(graph.Degree(5)));
        }

        protected static void Degree_ImmutableVertices_Test(
             BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            graph.AddEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);

            Assert.That(0,Is.EqualTo(graph.Degree(0)));
            Assert.That(3,Is.EqualTo(graph.Degree(1)));
            Assert.That(3,Is.EqualTo(graph.Degree(2)));
            Assert.That(4,Is.EqualTo(graph.Degree(3))); // Self edge
            Assert.That(2,Is.EqualTo(graph.Degree(4)));
        }

        protected static void Degree_ImmutableGraph_ReversedTest(
             IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
             Func<IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(1, 4);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 2);
            var edge6 = new Edge<int>(3, 3);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);
            wrappedGraph.AddVertex(5);
            IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            Assert.That(3,Is.EqualTo(graph.Degree(1)));
            Assert.That(3,Is.EqualTo(graph.Degree(2)));
            Assert.That(4,Is.EqualTo(graph.Degree(3))); // Self edge
            Assert.That(2,Is.EqualTo(graph.Degree(4)));
            Assert.That(0,Is.EqualTo(graph.Degree(5)));
        }

        protected static void Degree_Throws_Test<TVertex, TEdge>(
             IBidirectionalIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class, IEquatable<TVertex>, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.Degree(null));
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(new TVertex()));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void Degree_Throws_Matrix_Test<TEdge>(
             BidirectionalMatrixGraph<TEdge> graph)
            where TEdge : class, IEdge<int>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(-1));
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(10));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion
    }
}