using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Contains Edge

        protected static void ContainsEdge_Test(
            IEdgeSet<int, Edge<int>> graph,
            Action<Edge<int>> addVerticesAndEdge)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.That(graph.ContainsEdge(edge1), Is.False);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            addVerticesAndEdge(edge1);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            addVerticesAndEdge(edge2);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            addVerticesAndEdge(edge3);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            addVerticesAndEdge(edge4);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            addVerticesAndEdge(otherEdge1);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(0, 10)), Is.False);
            // Source not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(0, 1)), Is.False);
            // Target not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(1, 0)), Is.False);
        }

        protected static void ContainsEdge_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            ContainsEdge_Test(
                graph,
                edge => graph.AddVerticesAndEdge(edge));
        }

        protected static void ContainsEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IEdgeSet<int, Edge<int>>> createGraph)
        {
            IEdgeSet<int, Edge<int>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.That(graph.ContainsEdge(edge1), Is.False);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(0, 10)), Is.False);
            // Source not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(0, 1)), Is.False);
            // Target not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(1, 0)), Is.False);
        }

        protected static void ContainsEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IEdgeSet<int, SEquatableEdge<int>>> createGraph)
        {
            IEdgeSet<int, SEquatableEdge<int>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var equatableEdge1 = new SEquatableEdge<int>(edge1.Source, edge1.Target);
            var edge2 = new Edge<int>(1, 3);
            var equatableEdge2 = new SEquatableEdge<int>(edge2.Source, edge2.Target);
            var edge3 = new Edge<int>(2, 1);
            var equatableEdge3 = new SEquatableEdge<int>(edge3.Source, edge3.Target);
            var edge4 = new Edge<int>(2, 2);
            var equatableEdge4 = new SEquatableEdge<int>(edge4.Source, edge4.Target);
            var otherEdge1 = new Edge<int>(1, 2);
            var equatableOtherEdge1 = new SEquatableEdge<int>(otherEdge1.Source, otherEdge1.Target);

            Assert.That(graph.ContainsEdge(equatableEdge1), Is.False);
            Assert.That(graph.ContainsEdge(equatableEdge2), Is.False);
            Assert.That(graph.ContainsEdge(equatableEdge3), Is.False);
            Assert.That(graph.ContainsEdge(equatableEdge4), Is.False);
            Assert.That(graph.ContainsEdge(equatableOtherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(equatableEdge1), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge2), Is.False);
            Assert.That(graph.ContainsEdge(equatableEdge3), Is.False);
            Assert.That(graph.ContainsEdge(equatableEdge4), Is.False);
            Assert.That(graph.ContainsEdge(equatableOtherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(equatableEdge1), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge2), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge3), Is.False);
            Assert.That(graph.ContainsEdge(equatableEdge4), Is.False);
            Assert.That(graph.ContainsEdge(equatableOtherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(equatableEdge1), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge2), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge3), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge4), Is.False);
            Assert.That(graph.ContainsEdge(equatableOtherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(equatableEdge1), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge2), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge3), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge4), Is.True);
            Assert.That(graph.ContainsEdge(equatableOtherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(equatableEdge1), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge2), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge3), Is.True);
            Assert.That(graph.ContainsEdge(equatableEdge4), Is.True);
            Assert.That(graph.ContainsEdge(equatableOtherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(graph.ContainsEdge(new SEquatableEdge<int>(0, 10)), Is.False);
            // Source not in graph
            Assert.That(graph.ContainsEdge(new SEquatableEdge<int>(0, 1)), Is.False);
            // Target not in graph
            Assert.That(graph.ContainsEdge(new SEquatableEdge<int>(1, 0)), Is.False);
        }

        protected static void ContainsEdge_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.That(graph.ContainsEdge(edge1), Is.False);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            graph.AddVerticesAndEdge(edge1);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            graph.AddVerticesAndEdge(edge2);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            graph.AddVerticesAndEdge(edge3);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            graph.AddVerticesAndEdge(edge4);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            graph.AddVerticesAndEdge(otherEdge1);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(0, 10)), Is.False);
            // Source not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(0, 1)), Is.False);
            // Target not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(1, 0)), Is.False);
        }

        protected static void ContainsEdge_ForbiddenParallelEdges_ImmutableVertices_Test(
            IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 1);
            var edge4 = new Edge<int>(2, 2);
            var otherEdge1 = new Edge<int>(1, 2);

            Assert.That(graph.ContainsEdge(edge1), Is.False);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            graph.AddEdge(edge1);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            graph.AddEdge(edge2);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            graph.AddEdge(edge3);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            graph.AddEdge(edge4);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(10, 11)), Is.False);
            // Source not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(10, 1)), Is.False);
            // Target not in graph
            Assert.That(graph.ContainsEdge(new Edge<int>(1, 10)), Is.False);
        }

        protected static void ContainsEdge_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IEdgeSet<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            IEdgeSet<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var reversedEdge1 = new SReversedEdge<int, Edge<int>>(edge1);
            var edge2 = new Edge<int>(1, 3);
            var reversedEdge2 = new SReversedEdge<int, Edge<int>>(edge2);
            var edge3 = new Edge<int>(2, 1);
            var reversedEdge3 = new SReversedEdge<int, Edge<int>>(edge3);
            var edge4 = new Edge<int>(2, 2);
            var reversedEdge4 = new SReversedEdge<int, Edge<int>>(edge4);
            var otherEdge1 = new Edge<int>(1, 2);
            var reversedOtherEdge1 = new SReversedEdge<int, Edge<int>>(otherEdge1);

            Assert.That(graph.ContainsEdge(reversedEdge1), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.False);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(reversedEdge1), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.False);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(reversedEdge1), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.False);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(reversedEdge1), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.False);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(reversedEdge1), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.True);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(reversedEdge1), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.True);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(
                graph.ContainsEdge(
                    new SReversedEdge<int, Edge<int>>(
                        new Edge<int>(0, 10))), Is.False);
            // Source not in graph
            Assert.That(
                graph.ContainsEdge(
                    new SReversedEdge<int, Edge<int>>(
                        new Edge<int>(0, 1))), Is.False);
            // Target not in graph
            Assert.That(
                graph.ContainsEdge(
                    new SReversedEdge<int, Edge<int>>(
                        new Edge<int>(1, 0))), Is.False);
        }

        protected static void ContainsEdge_EquatableEdge_Test(
            IEdgeSet<int, EquatableEdge<int>> graph,
            Action<EquatableEdge<int>> addVerticesAndEdge)
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.That(graph.ContainsEdge(edge1), Is.False);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            addVerticesAndEdge(edge1);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            addVerticesAndEdge(edge2);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            addVerticesAndEdge(edge3);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            addVerticesAndEdge(edge4);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            addVerticesAndEdge(otherEdge1);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(0, 10)), Is.False);
            // Source not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(0, 1)), Is.False);
            // Target not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(1, 0)), Is.False);
        }

        protected static void ContainsEdge_EquatableEdge_Test(
            IMutableVertexAndEdgeSet<int, EquatableEdge<int>> graph)
        {
            ContainsEdge_EquatableEdge_Test(
                graph,
                edge => graph.AddVerticesAndEdge(edge));
        }

        protected static void ContainsEdge_EquatableEdge_ImmutableGraph_Test(
            IMutableVertexAndEdgeSet<int, EquatableEdge<int>> wrappedGraph,
            Func<IEdgeSet<int, EquatableEdge<int>>> createGraph)
        {
            IEdgeSet<int, EquatableEdge<int>> graph = createGraph();

            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.That(graph.ContainsEdge(edge1), Is.False);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(0, 10)), Is.False);
            // Source not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(0, 1)), Is.False);
            // Target not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(1, 0)), Is.False);
        }

        protected static void ContainsEdge_EquatableEdge_EdgesOnly_Test(
            EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.That(graph.ContainsEdge(edge1), Is.False);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            graph.AddVerticesAndEdge(edge1);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            graph.AddVerticesAndEdge(edge2);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            graph.AddVerticesAndEdge(edge3);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            graph.AddVerticesAndEdge(edge4);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            graph.AddVerticesAndEdge(otherEdge1);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(0, 10)), Is.False);
            // Source not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(0, 1)), Is.False);
            // Target not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(1, 0)), Is.False);
        }

        protected static void ContainsEdge_EquatableEdges_ForbiddenParallelEdges_ImmutableVertices_Test(
            IMutableEdgeListGraph<int, EquatableEdge<int>> graph)
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 3);
            var edge3 = new EquatableEdge<int>(2, 1);
            var edge4 = new EquatableEdge<int>(2, 2);
            var otherEdge1 = new EquatableEdge<int>(1, 2);

            Assert.That(graph.ContainsEdge(edge1), Is.False);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.False);

            graph.AddEdge(edge1);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.False);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            graph.AddEdge(edge2);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.False);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            graph.AddEdge(edge3);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.False);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            graph.AddEdge(edge4);
            Assert.That(graph.ContainsEdge(edge1), Is.True);
            Assert.That(graph.ContainsEdge(edge2), Is.True);
            Assert.That(graph.ContainsEdge(edge3), Is.True);
            Assert.That(graph.ContainsEdge(edge4), Is.True);
            Assert.That(graph.ContainsEdge(otherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(10, 11)), Is.False);
            // Source not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(10, 1)), Is.False);
            // Target not in graph
            Assert.That(graph.ContainsEdge(new EquatableEdge<int>(1, 10)), Is.False);
        }

        protected static void ContainsEdge_EquatableEdge_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, EquatableEdge<int>> wrappedGraph,
            Func<IEdgeSet<int, SReversedEdge<int, EquatableEdge<int>>>> createGraph)
        {
            IEdgeSet<int, SReversedEdge<int, EquatableEdge<int>>> graph = createGraph();

            var edge1 = new EquatableEdge<int>(1, 2);
            var reversedEdge1 = new SReversedEdge<int, EquatableEdge<int>>(edge1);
            var edge2 = new EquatableEdge<int>(1, 3);
            var reversedEdge2 = new SReversedEdge<int, EquatableEdge<int>>(edge2);
            var edge3 = new EquatableEdge<int>(2, 1);
            var reversedEdge3 = new SReversedEdge<int, EquatableEdge<int>>(edge3);
            var edge4 = new EquatableEdge<int>(2, 2);
            var reversedEdge4 = new SReversedEdge<int, EquatableEdge<int>>(edge4);
            var otherEdge1 = new EquatableEdge<int>(1, 2);
            var reversedOtherEdge1 = new SReversedEdge<int, EquatableEdge<int>>(otherEdge1);

            Assert.That(graph.ContainsEdge(reversedEdge1), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.False);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(reversedEdge1), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.False);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(reversedEdge1), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.False);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.False);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(reversedEdge1), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.False);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge4);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(reversedEdge1), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.True);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.True);

            wrappedGraph.AddVerticesAndEdge(otherEdge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(reversedEdge1), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge2), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge3), Is.True);
            Assert.That(graph.ContainsEdge(reversedEdge4), Is.True);
            Assert.That(graph.ContainsEdge(reversedOtherEdge1), Is.True);

            // Both vertices not in graph
            Assert.That(
                graph.ContainsEdge(
                    new SReversedEdge<int, EquatableEdge<int>>(
                        new EquatableEdge<int>(0, 10))), Is.False);
            // Source not in graph
            Assert.That(
                graph.ContainsEdge(
                    new SReversedEdge<int, EquatableEdge<int>>(
                        new EquatableEdge<int>(0, 1))), Is.False);
            // Target not in graph
            Assert.That(
                graph.ContainsEdge(
                    new SReversedEdge<int, EquatableEdge<int>>(
                        new EquatableEdge<int>(1, 0))), Is.False);
        }

        protected static void ContainsEdge_SourceTarget_Test(
            IIncidenceGraph<int, Edge<int>> graph,
            Action<Edge<int>> addVerticesAndEdge)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.That(graph.ContainsEdge(1, 2), Is.False);
            Assert.That(graph.ContainsEdge(2, 1), Is.False);

            addVerticesAndEdge(edge1);
            Assert.That(graph.ContainsEdge(1, 2), Is.True);
            Assert.That(graph.ContainsEdge(2, 1), Is.False);

            addVerticesAndEdge(edge2);
            Assert.That(graph.ContainsEdge(1, 3), Is.True);
            Assert.That(graph.ContainsEdge(3, 1), Is.False);

            addVerticesAndEdge(edge3);
            Assert.That(graph.ContainsEdge(2, 2), Is.True);

            // Vertices is not present in the graph
            Assert.That(graph.ContainsEdge(0, 4), Is.False);
            Assert.That(graph.ContainsEdge(1, 4), Is.False);
            Assert.That(graph.ContainsEdge(4, 1), Is.False);
        }

        protected static void ContainsEdge_SourceTarget_Test(
            IMutableVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_Test(
                graph,
                edge => graph.AddVerticesAndEdge(edge));
        }

        protected static void ContainsEdge_SourceTarget_ImmutableGraph_Test<TEdge>(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IIncidenceGraph<int, TEdge>> createGraph)
            where TEdge : IEdge<int>
        {
            IIncidenceGraph<int, TEdge> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.That(graph.ContainsEdge(1, 2), Is.False);
            Assert.That(graph.ContainsEdge(2, 1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(1, 2), Is.True);
            Assert.That(graph.ContainsEdge(2, 1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(1, 3), Is.True);
            Assert.That(graph.ContainsEdge(3, 1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(2, 2), Is.True);

            // Vertices is not present in the graph
            Assert.That(graph.ContainsEdge(0, 4), Is.False);
            Assert.That(graph.ContainsEdge(1, 4), Is.False);
            Assert.That(graph.ContainsEdge(4, 1), Is.False);
        }

        protected static void ContainsEdge_SourceTarget_ForbiddenParallelEdges_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.That(graph.ContainsEdge(1, 2), Is.False);
            Assert.That(graph.ContainsEdge(2, 1), Is.False);

            graph.AddEdge(edge1);
            Assert.That(graph.ContainsEdge(1, 2), Is.True);
            Assert.That(graph.ContainsEdge(2, 1), Is.False);

            graph.AddEdge(edge2);
            Assert.That(graph.ContainsEdge(1, 3), Is.True);
            Assert.That(graph.ContainsEdge(3, 1), Is.False);

            graph.AddEdge(edge3);
            Assert.That(graph.ContainsEdge(2, 2), Is.True);

            // Vertices is not present in the graph
            Assert.That(graph.ContainsEdge(4, 5), Is.False);
            Assert.That(graph.ContainsEdge(1, 4), Is.False);
            Assert.That(graph.ContainsEdge(4, 1), Is.False);
        }

        protected static void ContainsEdge_SourceTarget_ImmutableGraph_ReversedTest(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IIncidenceGraph<int, SReversedEdge<int, Edge<int>>>> createGraph)
        {
            IIncidenceGraph<int, SReversedEdge<int, Edge<int>>> graph = createGraph();

            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.That(graph.ContainsEdge(1, 2), Is.False);
            Assert.That(graph.ContainsEdge(2, 1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(1, 2), Is.False);
            Assert.That(graph.ContainsEdge(2, 1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(1, 3), Is.False);
            Assert.That(graph.ContainsEdge(3, 1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(2, 2), Is.True);

            // Vertices is not present in the graph
            Assert.That(graph.ContainsEdge(0, 4), Is.False);
            Assert.That(graph.ContainsEdge(1, 4), Is.False);
            Assert.That(graph.ContainsEdge(4, 1), Is.False);
        }

        protected static void ContainsEdge_SourceTarget_UndirectedGraph_Test(
            IMutableUndirectedGraph<int, Edge<int>> graph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            Assert.That(graph.ContainsEdge(1, 2), Is.False);
            Assert.That(graph.ContainsEdge(2, 1), Is.False);

            graph.AddVerticesAndEdge(edge1);
            Assert.That(graph.ContainsEdge(1, 2), Is.True);
            Assert.That(graph.ContainsEdge(2, 1), Is.True);

            graph.AddVerticesAndEdge(edge2);
            Assert.That(graph.ContainsEdge(1, 3), Is.True);
            Assert.That(graph.ContainsEdge(3, 1), Is.True);

            graph.AddVerticesAndEdge(edge3);
            Assert.That(graph.ContainsEdge(2, 2), Is.True);

            // Vertices is not present in the graph
            Assert.That(graph.ContainsEdge(0, 4), Is.False);
            Assert.That(graph.ContainsEdge(1, 4), Is.False);
            Assert.That(graph.ContainsEdge(4, 1), Is.False);
        }

        protected static void ContainsEdge_SourceTarget_ImmutableGraph_UndirectedGraph_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> wrappedGraph,
            Func<IImplicitUndirectedGraph<int, Edge<int>>> createGraph)
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 2);

            IImplicitUndirectedGraph<int, Edge<int>> graph = createGraph();
            Assert.That(graph.ContainsEdge(1, 2), Is.False);
            Assert.That(graph.ContainsEdge(2, 1), Is.False);

            wrappedGraph.AddVerticesAndEdge(edge1);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(1, 2), Is.True);
            Assert.That(graph.ContainsEdge(2, 1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge2);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(1, 3), Is.True);
            Assert.That(graph.ContainsEdge(3, 1), Is.True);

            wrappedGraph.AddVerticesAndEdge(edge3);
            graph = createGraph();
            Assert.That(graph.ContainsEdge(2, 2), Is.True);

            // Vertices is not present in the graph
            Assert.That(graph.ContainsEdge(0, 4), Is.False);
            Assert.That(graph.ContainsEdge(1, 4), Is.False);
            Assert.That(graph.ContainsEdge(4, 1), Is.False);
        }

        protected static void ContainsEdge_NullThrows_Test<TVertex, TEdge>(
            IEdgeSet<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null));
        }

        protected static void ContainsEdge_DefaultNullThrows_Test<TVertex>(
            IEdgeSet<TVertex, SEquatableEdge<TVertex>> graph)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(default));
        }

        protected static void ContainsEdge_NullThrows_ReversedTest<TVertex, TEdge>(
            IEdgeSet<TVertex, SReversedEdge<TVertex, TEdge>> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(default));
        }

        protected static void ContainsEdge_SourceTarget_Throws_Test<TVertex, TEdge>(
            IIncidenceGraph<TVertex, TEdge> graph)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(new TVertex(), null));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, new TVertex()));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void ContainsEdge_SourceTarget_Throws_UndirectedGraph_Test<TVertex, TEdge>(
            IImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(new TVertex(), null));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, new TVertex()));
            Assert.Throws<ArgumentNullException>(() => graph.ContainsEdge(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }


        protected static void ContainsEdge_UndirectedEdge_UndirectedGraph_Test(
            IMutableUndirectedGraph<int, EquatableEdge<int>> graph1,
            IMutableUndirectedGraph<int, EquatableUndirectedEdge<int>> graph2)
        {
            ///////////////////////////////////
            // ContainsEdge => Source/Target //
            ///////////////////////////////////
            // Equatable Edge
            var equatableEdge1 = new EquatableEdge<int>(1, 2);
            var equatableEdge2 = new EquatableEdge<int>(1, 3);

            Assert.That(graph1.ContainsEdge(1, 2), Is.False);
            Assert.That(graph1.ContainsEdge(2, 1), Is.False);

            graph1.AddVerticesAndEdge(equatableEdge1);
            Assert.That(graph1.ContainsEdge(1, 2), Is.True);
            Assert.That(graph1.ContainsEdge(2, 1), Is.True);

            graph1.AddVerticesAndEdge(equatableEdge2);
            Assert.That(graph1.ContainsEdge(1, 3), Is.True);
            Assert.That(graph1.ContainsEdge(3, 1), Is.True);

            // Vertices is not present in the graph
            Assert.That(graph1.ContainsEdge(0, 4), Is.False);
            Assert.That(graph1.ContainsEdge(1, 4), Is.False);
            Assert.That(graph1.ContainsEdge(4, 1), Is.False);


            // Undirected equatable edge
            var equatableUndirectedEdge1 = new EquatableUndirectedEdge<int>(1, 2);
            var equatableUndirectedEdge2 = new EquatableUndirectedEdge<int>(1, 3);

            Assert.That(graph2.ContainsEdge(1, 2), Is.False);
            Assert.That(graph2.ContainsEdge(2, 1), Is.False);

            graph2.AddVerticesAndEdge(equatableUndirectedEdge1);
            Assert.That(graph2.ContainsEdge(1, 2), Is.True);
            Assert.That(graph2.ContainsEdge(2, 1), Is.True);

            graph2.AddVerticesAndEdge(equatableUndirectedEdge2);
            Assert.That(graph2.ContainsEdge(1, 3), Is.True);
            Assert.That(graph2.ContainsEdge(3, 1), Is.True);

            // Vertices is not present in the graph
            Assert.That(graph2.ContainsEdge(0, 4), Is.False);
            Assert.That(graph2.ContainsEdge(1, 4), Is.False);
            Assert.That(graph2.ContainsEdge(4, 1), Is.False);
        }

        protected static void ContainsEdge_UndirectedEdge_ImmutableGraph_UndirectedGraph_Test(
            IMutableVertexAndEdgeSet<int, EquatableEdge<int>> wrappedGraph1,
            Func<IImplicitUndirectedGraph<int, EquatableEdge<int>>> createEquatableEdgeGraph,
            IMutableVertexAndEdgeSet<int, EquatableUndirectedEdge<int>> wrappedGraph2,
            Func<IImplicitUndirectedGraph<int, EquatableUndirectedEdge<int>>> createEquatableUndirectedEdgeGraph)
        {
            ///////////////////////////////////
            // ContainsEdge => Source/Target //
            ///////////////////////////////////
            // Equatable Edge
            var equatableEdge1 = new EquatableEdge<int>(1, 2);
            var equatableEdge2 = new EquatableEdge<int>(1, 3);

            IImplicitUndirectedGraph<int, EquatableEdge<int>> graph1 = createEquatableEdgeGraph();
            Assert.That(graph1.ContainsEdge(1, 2), Is.False);
            Assert.That(graph1.ContainsEdge(2, 1), Is.False);

            wrappedGraph1.AddVerticesAndEdge(equatableEdge1);
            graph1 = createEquatableEdgeGraph();
            Assert.That(graph1.ContainsEdge(1, 2), Is.True);
            Assert.That(graph1.ContainsEdge(2, 1), Is.True);

            wrappedGraph1.AddVerticesAndEdge(equatableEdge2);
            graph1 = createEquatableEdgeGraph();
            Assert.That(graph1.ContainsEdge(1, 3), Is.True);
            Assert.That(graph1.ContainsEdge(3, 1), Is.True);

            // Vertices is not present in the graph
            Assert.That(graph1.ContainsEdge(0, 4), Is.False);
            Assert.That(graph1.ContainsEdge(1, 4), Is.False);
            Assert.That(graph1.ContainsEdge(4, 1), Is.False);


            // Undirected equatable edge
            var equatableUndirectedEdge1 = new EquatableUndirectedEdge<int>(1, 2);
            var equatableUndirectedEdge2 = new EquatableUndirectedEdge<int>(1, 3);

            IImplicitUndirectedGraph<int, EquatableUndirectedEdge<int>> graph2 = createEquatableUndirectedEdgeGraph();
            Assert.That(graph2.ContainsEdge(1, 2), Is.False);
            Assert.That(graph2.ContainsEdge(2, 1), Is.False);

            wrappedGraph2.AddVerticesAndEdge(equatableUndirectedEdge1);
            graph2 = createEquatableUndirectedEdgeGraph();
            Assert.That(graph2.ContainsEdge(1, 2), Is.True);
            Assert.That(graph2.ContainsEdge(2, 1), Is.True);

            wrappedGraph2.AddVerticesAndEdge(equatableUndirectedEdge2);
            graph2 = createEquatableUndirectedEdgeGraph();
            Assert.That(graph2.ContainsEdge(1, 3), Is.True);
            Assert.That(graph2.ContainsEdge(3, 1), Is.True);

            // Vertices is not present in the graph
            Assert.That(graph2.ContainsEdge(0, 4), Is.False);
            Assert.That(graph2.ContainsEdge(1, 4), Is.False);
            Assert.That(graph2.ContainsEdge(4, 1), Is.False);
        }

        #endregion
    }
}