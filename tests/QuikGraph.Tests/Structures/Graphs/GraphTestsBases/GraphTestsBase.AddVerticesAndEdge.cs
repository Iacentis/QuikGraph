using System;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Add Vertices & Edges

        protected static void AddVerticesAndEdge_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexAdded += v =>
            {
                Assert.That(v, Is.Not.Null);
                ++vertexAdded;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.That(graph.AddVerticesAndEdge(edge1), Is.True);
            Assert.That(2, Is.EqualTo(vertexAdded));
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasVertices(graph, [1, 2]);
            AssertHasEdges(graph, [edge1]);

            // Edge 2
            var edge2 = new Edge<int>(1, 3);
            Assert.That(graph.AddVerticesAndEdge(edge2), Is.True);
            Assert.That(3, Is.EqualTo(vertexAdded));
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasVertices(graph, [1, 2, 3]);
            AssertHasEdges(graph, [edge1, edge2]);

            // Edge 3
            var edge3 = new Edge<int>(2, 3);
            Assert.That(graph.AddVerticesAndEdge(edge3), Is.True);
            Assert.That(3, Is.EqualTo(vertexAdded));
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasVertices(graph, [1, 2, 3]);
            AssertHasEdges(graph, [edge1, edge2, edge3]);
        }

        protected static void AddVerticesAndEdge_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            AssertEmptyGraph(graph1);

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.That(graph1.AddVerticesAndEdge(edge1), Is.True);
            AssertHasVertices(graph1, [1, 2]);
            AssertHasEdges(graph1, [edge1]);

            // Edge 2
            var edge2 = new Edge<int>(1, 3);
            Assert.That(graph1.AddVerticesAndEdge(edge2), Is.True);
            AssertHasVertices(graph1, [1, 2, 3]);
            AssertHasEdges(graph1, [edge1, edge2]);

            // Edge 3
            var edge3 = new Edge<int>(2, 3);
            Assert.That(graph1.AddVerticesAndEdge(edge3), Is.True);
            AssertHasVertices(graph1, [1, 2, 3]);
            AssertHasEdges(graph1, [edge1, edge2, edge3]);


            // Graph with parent
            AssertEmptyGraph(parent2);
            AssertEmptyGraph(graph2);

            // Edge 1
            Assert.That(graph2.AddVerticesAndEdge(edge1), Is.True);
            AssertHasVertices(parent2, [1, 2]);
            AssertHasVertices(graph2, [1, 2]);
            AssertHasEdges(parent2, [edge1]);
            AssertHasEdges(graph2, [edge1]);

            // Edge 2
            Assert.That(parent2.AddVerticesAndEdge(edge2), Is.True);
            AssertHasVertices(parent2, [1, 2, 3]);
            AssertHasVertices(graph2, [1, 2]);
            AssertHasEdges(parent2, [edge1, edge2]);
            AssertHasEdges(graph2, [edge1]);

            Assert.That(graph2.AddVerticesAndEdge(edge2), Is.True);
            AssertHasVertices(parent2, [1, 2, 3]);
            AssertHasVertices(graph2, [1, 2, 3]);
            AssertHasEdges(parent2, [edge1, edge2]);
            AssertHasEdges(graph2, [edge1, edge2]);

            // Edge 3
            Assert.That(graph2.AddVerticesAndEdge(edge3), Is.True);
            AssertHasVertices(parent2, [1, 2, 3]);
            AssertHasVertices(graph2, [1, 2, 3]);
            AssertHasEdges(parent2, [edge1, edge2, edge3]);
            AssertHasEdges(graph2, [edge1, edge2, edge3]);
        }

        protected static void AddVerticesAndEdge_Throws_Test<TVertex, TEdge>(
            IMutableVertexAndEdgeSet<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdge(null));
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdge_Throws_EdgesOnly_Test<TVertex, TEdge>(
            EdgeListGraph<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdge(null));
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdge_Throws_Clusters_Test<TVertex, TEdge>(
            ClusteredAdjacencyGraph<TVertex, TEdge> graph)
            where TEdge : class, IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdge(null));
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdgeRange_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexAdded += v =>
            {
                Assert.That(v, Is.Not.Null);
                ++vertexAdded;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            Assert.That(2, Is.EqualTo(graph.AddVerticesAndEdgeRange([edge1, edge2])));
            Assert.That(3, Is.EqualTo(vertexAdded));
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasVertices(graph, [1, 2, 3]);
            AssertHasEdges(graph, [edge1, edge2]);

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            Assert.That(1,
                Is.EqualTo(graph.AddVerticesAndEdgeRange([edge1, edge3]))); // Showcase the add of only one edge
            Assert.That(3, Is.EqualTo(vertexAdded));
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasVertices(graph, [1, 2, 3]);
            AssertHasEdges(graph, [edge1, edge2, edge3]);
        }

        protected static void AddVerticesAndEdgeRange_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            Assert.That(2, Is.EqualTo(graph.AddVerticesAndEdgeRange([edge1, edge2])));
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasVertices(graph, [1, 2, 3]);
            AssertHasEdges(graph, [edge1, edge2]);

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            Assert.That(1,
                Is.EqualTo(graph.AddVerticesAndEdgeRange([edge1, edge3]))); // Showcase the add of only one edge
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasVertices(graph, [1, 2, 3]);
            AssertHasEdges(graph, [edge1, edge2, edge3]);
        }

        protected static void AddVerticesAndEdgeRange_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            AssertEmptyGraph(graph1);

            // Edge 1, 2
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            Assert.That(2, Is.EqualTo(graph1.AddVerticesAndEdgeRange([edge1, edge2])));
            AssertHasVertices(graph1, [1, 2, 3]);
            AssertHasEdges(graph1, [edge1, edge2]);

            // Edge 1, 3
            var edge3 = new Edge<int>(2, 3);
            Assert.That(1,
                Is.EqualTo(graph1.AddVerticesAndEdgeRange([edge1, edge3]))); // Showcase the add of only one edge
            AssertHasVertices(graph1, [1, 2, 3]);
            AssertHasEdges(graph1, [edge1, edge2, edge3]);


            // Graph with parent
            AssertEmptyGraph(parent2);
            AssertEmptyGraph(graph2);

            // Edge 1, 2
            Assert.That(2, Is.EqualTo(graph2.AddVerticesAndEdgeRange([edge1, edge2])));
            AssertHasVertices(parent2, [1, 2, 3]);
            AssertHasVertices(graph2, [1, 2, 3]);
            AssertHasEdges(parent2, [edge1, edge2]);
            AssertHasEdges(graph2, [edge1, edge2]);

            // Edge 1, 3
            Assert.That(1,
                Is.EqualTo(parent2.AddVerticesAndEdgeRange([edge1, edge3]))); // Showcase the add of only one edge
            AssertHasVertices(parent2, [1, 2, 3]);
            AssertHasVertices(graph2, [1, 2, 3]);
            AssertHasEdges(parent2, [edge1, edge2, edge3]);
            AssertHasEdges(graph2, [edge1, edge2]);

            Assert.That(1,
                Is.EqualTo(graph2.AddVerticesAndEdgeRange([edge1, edge3]))); // Showcase the add of only one edge
            AssertHasVertices(parent2, [1, 2, 3]);
            AssertHasVertices(graph2, [1, 2, 3]);
            AssertHasEdges(parent2, [edge1, edge2, edge3]);
            AssertHasEdges(graph2, [edge1, edge2, edge3]);
        }

        protected static void AddVerticesAndEdgeRange_Throws_Test(
            IMutableVertexAndEdgeSet<int, Edge<int>> graph)
        {
            int vertexAdded = 0;
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.VertexAdded += v =>
            {
                Assert.That(v, Is.Not.Null);
                ++vertexAdded;
            };
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(null));

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange([edge1, null, edge3]));
            Assert.That(0, Is.EqualTo(vertexAdded));
            Assert.That(0, Is.EqualTo(edgeAdded));
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdgeRange_Throws_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertEmptyGraph(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(null));

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange([edge1, null, edge3]));
            Assert.That(0, Is.EqualTo(edgeAdded));
            AssertEmptyGraph(graph);
        }

        protected static void AddVerticesAndEdgeRange_Throws_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            AssertEmptyGraph(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange(null));

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(1, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddVerticesAndEdgeRange([edge1, null, edge3]));
            AssertEmptyGraph(graph);
        }

        #endregion
    }
}