using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Try Get Edges

        protected static void TryGetEdge_Test(
            IIncidenceGraph<int, Edge<int>> graph,
            Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            addVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);

            Assert.That(graph.TryGetEdge(0, 10, out _), Is.False);
            Assert.That(graph.TryGetEdge(0, 1, out _), Is.False);

            Assert.That(graph.TryGetEdge(2, 4, out Edge<int> gotEdge), Is.True);
            Assert.That(edge5, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(2, 2, out gotEdge), Is.True);
            Assert.That(edge4, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(1, 2, out gotEdge), Is.True);
            Assert.That(edge1, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(2, 1, out _), Is.False);
        }

        protected static void TryGetEdge_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            TryGetEdge_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);
            IIncidenceGraph<int, Edge<int>> graph = createGraph();

            Assert.That(graph.TryGetEdge(0, 10, out _), Is.False);
            Assert.That(graph.TryGetEdge(0, 1, out _), Is.False);

            Assert.That(graph.TryGetEdge(2, 4, out Edge<int> gotEdge), Is.True);
            Assert.That(edge5, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(2, 2, out gotEdge), Is.True);
            Assert.That(edge4, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(1, 2, out gotEdge), Is.True);
            Assert.That(edge1, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(2, 1, out _), Is.False);
        }

        protected static void TryGetEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IIncidenceGraph<int, SEquatableEdge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);
            IIncidenceGraph<int, SEquatableEdge<int>> graph = createGraph();

            Assert.That(graph.TryGetEdge(0, 10, out _), Is.False);
            Assert.That(graph.TryGetEdge(0, 1, out _), Is.False);

            Assert.That(graph.TryGetEdge(2, 4, out SEquatableEdge<int> gotEdge), Is.True);
            Assert.That(new SEquatableEdge<int>(2, 4), Is.EqualTo(gotEdge));

            Assert.That(graph.TryGetEdge(2, 2, out gotEdge), Is.True);
            Assert.That(new SEquatableEdge<int>(2, 2), Is.EqualTo(gotEdge));

            Assert.That(graph.TryGetEdge(1, 2, out gotEdge), Is.True);
            Assert.That(new SEquatableEdge<int>(1, 2), Is.EqualTo(gotEdge));

            Assert.That(graph.TryGetEdge(2, 1, out _), Is.False);
        }

        protected static void TryGetEdge_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);

            graph.AddEdgeRange([edge1, edge2, edge3, edge4, edge5]);

            Assert.That(graph.TryGetEdge(6, 10, out _), Is.False);
            Assert.That(graph.TryGetEdge(6, 1, out _), Is.False);

            Assert.That(graph.TryGetEdge(2, 4, out Edge<int> gotEdge), Is.True);
            Assert.That(edge4, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(2, 2, out gotEdge), Is.True);
            Assert.That(edge3, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(1, 2, out gotEdge), Is.True);
            Assert.That(edge1, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(2, 1, out _), Is.False);
        }

        protected static void TryGetEdge_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);
            IIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            Assert.That(graph.TryGetEdge(0, 10, out _), Is.False);
            Assert.That(graph.TryGetEdge(0, 1, out _), Is.False);

            Assert.That(graph.TryGetEdge(4, 2, out SReversedEdge<int, Edge<int>> gotEdge), Is.True);
            AssertSameReversedEdge(edge5, gotEdge);

            Assert.That(graph.TryGetEdge(2, 2, out gotEdge), Is.True);
            AssertSameReversedEdge(edge4, gotEdge);

            Assert.That(graph.TryGetEdge(2, 1, out gotEdge), Is.True);
            AssertSameReversedEdge(edge1, gotEdge);

            Assert.That(graph.TryGetEdge(1, 2, out _), Is.False);
        }

        protected static void TryGetEdge_UndirectedGraph_Test(
            IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 2);

            graph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6, edge7]);

            Assert.That(graph.TryGetEdge(0, 10, out _), Is.False);
            Assert.That(graph.TryGetEdge(0, 1, out _), Is.False);

            Assert.That(graph.TryGetEdge(2, 4, out Edge<int> gotEdge), Is.True);
            Assert.That(edge5, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(1, 2, out gotEdge), Is.True);
            Assert.That(edge1, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(2, 2, out gotEdge), Is.True);
            Assert.That(edge4, Is.SameAs(gotEdge));

            // 1 -> 2 is present in this undirected graph
            Assert.That(graph.TryGetEdge(2, 1, out gotEdge), Is.True);
            Assert.That(edge1, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(5, 2, out gotEdge), Is.True);
            Assert.That(edge7, Is.SameAs(gotEdge));

            // 5 -> 2 is present in this undirected graph
            Assert.That(graph.TryGetEdge(2, 5, out gotEdge), Is.True);
            Assert.That(edge7, Is.SameAs(gotEdge));
        }

        protected static void TryGetEdge_ImmutableGraph_UndirectedGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IImplicitUndirectedGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 2);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6, edge7]);
            IImplicitUndirectedGraph<int, Edge<int>> graph = createGraph();

            Assert.That(graph.TryGetEdge(0, 10, out _), Is.False);
            Assert.That(graph.TryGetEdge(0, 1, out _), Is.False);

            Assert.That(graph.TryGetEdge(2, 4, out Edge<int> gotEdge), Is.True);
            Assert.That(edge5, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(1, 2, out gotEdge), Is.True);
            Assert.That(edge1, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(2, 2, out gotEdge), Is.True);
            Assert.That(edge4, Is.SameAs(gotEdge));

            // 1 -> 2 is present in this undirected graph
            Assert.That(graph.TryGetEdge(2, 1, out gotEdge), Is.True);
            Assert.That(edge1, Is.SameAs(gotEdge));

            Assert.That(graph.TryGetEdge(5, 2, out gotEdge), Is.True);
            Assert.That(edge7, Is.SameAs(gotEdge));

            // 5 -> 2 is present in this undirected graph
            Assert.That(graph.TryGetEdge(2, 5, out gotEdge), Is.True);
            Assert.That(edge7, Is.SameAs(gotEdge));
        }

        protected static void TryGetEdge_Throws_Test<TVertex, TEdge>(
            IIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, new TVertex(), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(new TVertex(), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetEdge_Throws_UndirectedGraph_Test<TVertex, TEdge>(
            IImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, new TVertex(), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(new TVertex(), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdge(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetEdges_Test(
            IIncidenceGraph<int, Edge<int>> graph,
            Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            addVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);

            Assert.That(graph.TryGetEdges(0, 10, out _), Is.False);
            Assert.That(graph.TryGetEdges(0, 1, out _), Is.False);

            Assert.That(graph.TryGetEdges(2, 2, out IEnumerable<Edge<int>> gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge4 }, gotEdges);

            Assert.That(graph.TryGetEdges(2, 4, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.That(graph.TryGetEdges(1, 2, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);

            Assert.That(graph.TryGetEdges(2, 1, out gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);
        }

        protected static void TryGetEdges_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            TryGetEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);
            IIncidenceGraph<int, Edge<int>> graph = createGraph();

            Assert.That(graph.TryGetEdges(0, 10, out _), Is.False);
            Assert.That(graph.TryGetEdges(0, 1, out _), Is.False);

            Assert.That(graph.TryGetEdges(2, 2, out IEnumerable<Edge<int>> gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge4 }, gotEdges);

            Assert.That(graph.TryGetEdges(2, 4, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.That(graph.TryGetEdges(1, 2, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);

            Assert.That(graph.TryGetEdges(2, 1, out gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);
        }

        protected static void TryGetEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IIncidenceGraph<int, SEquatableEdge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);
            IIncidenceGraph<int, SEquatableEdge<int>> graph = createGraph();

            Assert.That(graph.TryGetEdges(0, 10, out _), Is.False);
            Assert.That(graph.TryGetEdges(0, 1, out _), Is.False);

            Assert.That(graph.TryGetEdges(2, 2, out IEnumerable<SEquatableEdge<int>> gotEdges), Is.True);
            CollectionAssert.AreEqual(
                new[] { new SEquatableEdge<int>(2, 2) },
                gotEdges);

            Assert.That(graph.TryGetEdges(2, 4, out gotEdges), Is.True);
            CollectionAssert.AreEqual(
                new[] { new SEquatableEdge<int>(2, 4), },
                gotEdges);

            Assert.That(graph.TryGetEdges(1, 2, out gotEdges), Is.True);
            CollectionAssert.AreEqual(
                new[] { new SEquatableEdge<int>(1, 2), new SEquatableEdge<int>(1, 2) },
                gotEdges);

            Assert.That(graph.TryGetEdges(2, 1, out gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);
        }

        protected static void TryGetEdges_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);

            graph.AddEdgeRange([edge1, edge2, edge3, edge4, edge5]);

            Assert.That(graph.TryGetEdges(6, 10, out _), Is.False);
            Assert.That(graph.TryGetEdges(6, 1, out _), Is.False);

            Assert.That(graph.TryGetEdges(2, 2, out IEnumerable<Edge<int>> gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge3 }, gotEdges);

            Assert.That(graph.TryGetEdges(2, 4, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge4 }, gotEdges);

            Assert.That(graph.TryGetEdges(1, 2, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge1 }, gotEdges);

            Assert.That(graph.TryGetEdges(2, 1, out gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);
        }

        protected static void TryGetEdges_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);
            IIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            Assert.That(graph.TryGetEdges(0, 10, out _), Is.False);
            Assert.That(graph.TryGetEdges(0, 1, out _), Is.False);

            Assert.That(graph.TryGetEdges(2, 2, out IEnumerable<SReversedEdge<int, Edge<int>>> gotEdges), Is.True);
            AssertSameReversedEdges([edge4], gotEdges);

            Assert.That(graph.TryGetEdges(4, 2, out gotEdges), Is.True);
            AssertSameReversedEdges([edge5], gotEdges);

            Assert.That(graph.TryGetEdges(2, 1, out gotEdges), Is.True);
            AssertSameReversedEdges([edge1, edge2], gotEdges);

            Assert.That(graph.TryGetEdges(1, 2, out gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);
        }

        protected static void TryGetEdges_Throws_Test<TEdge>(
            IIncidenceGraph<TestVertex, TEdge> graph)
            where TEdge : IEdge<TestVertex>
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(null, new TestVertex("v2"), out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(new TestVertex("v1"), null, out _));
            Assert.Throws<ArgumentNullException>(() => graph.TryGetEdges(null, null, out _));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        protected static void TryGetOutEdges_Test(
            IImplicitGraph<int, Edge<int>> graph,
            Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(4, 5);

            addVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6, edge7]);

            Assert.That(graph.TryGetOutEdges(0, out _), Is.False);

            Assert.That(graph.TryGetOutEdges(5, out IEnumerable<Edge<int>> gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);

            Assert.That(graph.TryGetOutEdges(3, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge6 }, gotEdges);

            Assert.That(graph.TryGetOutEdges(1, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge3 }, gotEdges);
        }

        protected static void TryGetOutEdges_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            TryGetOutEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetOutEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IImplicitGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(4, 5);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6, edge7]);
            IImplicitGraph<int, Edge<int>> graph = createGraph();

            Assert.That(graph.TryGetOutEdges(0, out _), Is.False);

            Assert.That(graph.TryGetOutEdges(5, out IEnumerable<Edge<int>> gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);

            Assert.That(graph.TryGetOutEdges(3, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge6 }, gotEdges);

            Assert.That(graph.TryGetOutEdges(1, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge3 }, gotEdges);
        }

        protected static void TryGetOutEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IImplicitGraph<int, SEquatableEdge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(4, 5);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6, edge7]);
            IImplicitGraph<int, SEquatableEdge<int>> graph = createGraph();

            Assert.That(graph.TryGetOutEdges(0, out _), Is.False);

            Assert.That(graph.TryGetOutEdges(5, out IEnumerable<SEquatableEdge<int>> gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);

            Assert.That(graph.TryGetOutEdges(3, out gotEdges), Is.True);
            CollectionAssert.AreEqual(
                new[] { new SEquatableEdge<int>(3, 1) },
                gotEdges);

            Assert.That(graph.TryGetOutEdges(1, out gotEdges), Is.True);
            CollectionAssert.AreEqual(
                new[] { new SEquatableEdge<int>(1, 2), new SEquatableEdge<int>(1, 2), new SEquatableEdge<int>(1, 3) },
                gotEdges);
        }

        protected static void TryGetOutEdges_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);
            var edge6 = new Edge<int>(4, 5);

            graph.AddEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);

            Assert.That(graph.TryGetOutEdges(6, out _), Is.False);

            Assert.That(graph.TryGetOutEdges(5, out IEnumerable<Edge<int>> gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);

            Assert.That(graph.TryGetOutEdges(3, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.That(graph.TryGetOutEdges(1, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge1, edge2 }, gotEdges);
        }

        protected static void TryGetOutEdges_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IImplicitGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 4);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6, edge7]);
            IImplicitGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            Assert.That(graph.TryGetOutEdges(0, out _), Is.False);

            Assert.That(graph.TryGetOutEdges(5, out IEnumerable<SReversedEdge<int, Edge<int>>> gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);

            Assert.That(graph.TryGetOutEdges(3, out gotEdges), Is.True);
            AssertSameReversedEdges([edge3], gotEdges);

            Assert.That(graph.TryGetOutEdges(2, out gotEdges), Is.True);
            AssertSameReversedEdges([edge1, edge2, edge4], gotEdges);
        }

        protected static void TryGetOutEdges_Throws_Test<TVertex, TEdge>(
            IImplicitGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetOutEdges(null, out _));
        }

        protected static void TryGetInEdges_Test(
            IBidirectionalIncidenceGraph<int, Edge<int>> graph,
            Action<IEnumerable<Edge<int>>> addVerticesAndEdgeRange)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 3);

            addVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6, edge7]);

            Assert.That(graph.TryGetInEdges(0, out _), Is.False);

            Assert.That(graph.TryGetInEdges(5, out IEnumerable<Edge<int>> gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);

            Assert.That(graph.TryGetInEdges(4, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.That(graph.TryGetInEdges(2, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge4 }, gotEdges);
        }

        protected static void TryGetInEdges_Test(
            IMutableBidirectionalGraph<int, Edge<int>> graph)
        {
            TryGetInEdges_Test(
                graph,
                edges => graph.AddVerticesAndEdgeRange(edges));
        }

        protected static void TryGetInEdges_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IBidirectionalIncidenceGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(2, 2);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(5, 3);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6, edge7]);
            IBidirectionalIncidenceGraph<int, Edge<int>> graph = createGraph();

            Assert.That(graph.TryGetInEdges(0, out _), Is.False);

            Assert.That(graph.TryGetInEdges(5, out IEnumerable<Edge<int>> gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);

            Assert.That(graph.TryGetInEdges(4, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge5 }, gotEdges);

            Assert.That(graph.TryGetInEdges(2, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge4 }, gotEdges);
        }

        protected static void TryGetInEdges_ImmutableVertices_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);
            var edge4 = new Edge<int>(2, 4);
            var edge5 = new Edge<int>(3, 1);
            var edge6 = new Edge<int>(5, 3);

            graph.AddEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6]);

            Assert.That(graph.TryGetInEdges(6, out _), Is.False);

            Assert.That(graph.TryGetInEdges(5, out IEnumerable<Edge<int>> gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);

            Assert.That(graph.TryGetInEdges(4, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge4 }, gotEdges);

            Assert.That(graph.TryGetInEdges(2, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge1, edge3 }, gotEdges);
        }

        protected static void TryGetInEdges_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            var edge4 = new Edge<int>(1, 1);
            var edge5 = new Edge<int>(2, 4);
            var edge6 = new Edge<int>(3, 1);
            var edge7 = new Edge<int>(4, 5);

            wrappedGraph.AddVerticesAndEdgeRange([edge1, edge2, edge3, edge4, edge5, edge6, edge7]);
            IBidirectionalIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            Assert.That(graph.TryGetInEdges(0, out _), Is.False);

            Assert.That(graph.TryGetInEdges(5, out IEnumerable<SReversedEdge<int, Edge<int>>> gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);

            Assert.That(graph.TryGetInEdges(4, out gotEdges), Is.True);
            AssertSameReversedEdges([edge7], gotEdges);

            Assert.That(graph.TryGetInEdges(1, out gotEdges), Is.True);
            AssertSameReversedEdges([edge1, edge2, edge3, edge4], gotEdges);
        }

        protected static void TryGetInEdges_Throws_Test<TVertex, TEdge>(
            IBidirectionalIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetInEdges(null, out _));
        }

        #endregion
    }
}