using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Contains Vertex

        protected static void ContainsVertex_Test(
             IMutableVertexSet<TestVertex> graph)
        {
            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var otherVertex1 = new TestVertex("1");

            Assert.That(graph.ContainsVertex(vertex1),Is.False);
            Assert.That(graph.ContainsVertex(vertex2),Is.False);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.False);

            graph.AddVertex(vertex1);
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.False);

            graph.AddVertex(vertex2);
            Assert.That(graph.ContainsVertex(vertex2),Is.True);

            graph.AddVertex(otherVertex1);
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.True);
        }

        protected static void ContainsVertex_ImmutableGraph_Test(
             IMutableVertexSet<TestVertex> wrappedGraph,
             Func<IImplicitVertexSet<TestVertex>> createGraph)
        {
            IImplicitVertexSet<TestVertex> graph = createGraph();

            var vertex1 = new TestVertex("1");
            var vertex2 = new TestVertex("2");
            var otherVertex1 = new TestVertex("1");

            Assert.That(graph.ContainsVertex(vertex1),Is.False);
            Assert.That(graph.ContainsVertex(vertex2),Is.False);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.False);

            wrappedGraph.AddVertex(vertex1);
            graph = createGraph();
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.False);

            wrappedGraph.AddVertex(vertex2);
            graph = createGraph();
            Assert.That(graph.ContainsVertex(vertex2),Is.True);

            wrappedGraph.AddVertex(otherVertex1);
            graph = createGraph();
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.True);
        }

        protected static void ContainsVertex_OnlyEdges_Test(
             EdgeListGraph<TestVertex, Edge<TestVertex>> graph)
        {
            var vertex1 = new TestVertex("1");
            var toVertex1 = new TestVertex("target 1");
            var vertex2 = new TestVertex("2");
            var toVertex2 = new TestVertex("target 2");
            var otherVertex1 = new TestVertex("1");
            var toOtherVertex1 = new TestVertex("target 1");

            Assert.That(graph.ContainsVertex(vertex1),Is.False);
            Assert.That(graph.ContainsVertex(vertex2),Is.False);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.False);

            graph.AddEdge(new Edge<TestVertex>(vertex1, toVertex1));
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.False);

            graph.AddEdge(new Edge<TestVertex>(vertex2, toVertex2));
            Assert.That(graph.ContainsVertex(vertex2),Is.True);

            graph.AddEdge(new Edge<TestVertex>(otherVertex1, toOtherVertex1));
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.True);
        }

        protected static void ContainsVertex_EquatableVertex_Test(
             IMutableVertexSet<EquatableTestVertex> graph)
        {
            var vertex1 = new EquatableTestVertex("1");
            var vertex2 = new EquatableTestVertex("2");
            var otherVertex1 = new EquatableTestVertex("1");

            Assert.That(graph.ContainsVertex(vertex1),Is.False);
            Assert.That(graph.ContainsVertex(vertex2),Is.False);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.False);

            graph.AddVertex(vertex1);
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.True);

            graph.AddVertex(vertex2);
            Assert.That(graph.ContainsVertex(vertex2),Is.True);

            graph.AddVertex(otherVertex1);
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.True);
        }

        protected static void ContainsVertex_EquatableVertex_ImmutableGraph_Test(
             IMutableVertexSet<EquatableTestVertex> wrappedGraph,
             Func<IImplicitVertexSet<EquatableTestVertex>> createGraph)
        {
            IImplicitVertexSet<EquatableTestVertex> graph = createGraph();

            var vertex1 = new EquatableTestVertex("1");
            var vertex2 = new EquatableTestVertex("2");
            var otherVertex1 = new EquatableTestVertex("1");

            Assert.That(graph.ContainsVertex(vertex1),Is.False);
            Assert.That(graph.ContainsVertex(vertex2),Is.False);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.False);

            wrappedGraph.AddVertex(vertex1);
            graph = createGraph();
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.True);

            wrappedGraph.AddVertex(vertex2);
            graph = createGraph();
            Assert.That(graph.ContainsVertex(vertex2),Is.True);

            wrappedGraph.AddVertex(otherVertex1);
            graph = createGraph();
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.True);
        }

        protected static void ContainsVertex_EquatableVertex_OnlyEdges_Test(
             EdgeListGraph<EquatableTestVertex, Edge<EquatableTestVertex>> graph)
        {
            var vertex1 = new EquatableTestVertex("1");
            var toVertex1 = new EquatableTestVertex("target 1");
            var vertex2 = new EquatableTestVertex("2");
            var toVertex2 = new EquatableTestVertex("target 2");
            var otherVertex1 = new EquatableTestVertex("1");
            var toOtherVertex1 = new EquatableTestVertex("target 1");

            Assert.That(graph.ContainsVertex(vertex1),Is.False);
            Assert.That(graph.ContainsVertex(vertex2),Is.False);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.False);

            graph.AddEdge(new Edge<EquatableTestVertex>(vertex1, toVertex1));
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.True);

            graph.AddEdge(new Edge<EquatableTestVertex>(vertex2, toVertex2));
            Assert.That(graph.ContainsVertex(vertex2),Is.True);

            graph.AddEdge(new Edge<EquatableTestVertex>(otherVertex1, toOtherVertex1));
            Assert.That(graph.ContainsVertex(vertex1),Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1),Is.True);
        }

        protected static void ContainsVertex_Throws_Test<TVertex>(
             IImplicitVertexSet<TVertex> graph)
            where TVertex : class
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => graph.ContainsVertex(null));
        }

        #endregion
    }
}