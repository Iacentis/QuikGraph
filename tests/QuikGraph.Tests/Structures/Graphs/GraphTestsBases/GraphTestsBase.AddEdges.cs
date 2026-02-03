using System;
using NUnit.Framework;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    internal partial class GraphTestsBase
    {
        #region Add Edges

        protected static void AddEdge_ParallelEdges_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.That(graph.AddEdge(edge1), Is.True);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.That(graph.AddEdge(edge2), Is.True);
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2]);

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.That(graph.AddEdge(edge3), Is.True);
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3]);

            // Edge 1 bis
            Assert.That(graph.AddEdge(edge1), Is.True);
            Assert.That(4, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3, edge1]);

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.That(graph.AddEdge(edge4), Is.True);
            Assert.That(5, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3, edge1, edge4]);
        }

        protected static void AddEdge_ParallelEdges_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.That(graph1.AddEdge(edge1), Is.True);
            AssertHasEdges(graph1, [edge1]);

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.That(graph1.AddEdge(edge2), Is.True);
            AssertHasEdges(graph1, [edge1, edge2]);

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.That(graph1.AddEdge(edge3), Is.True);
            AssertHasEdges(graph1, [edge1, edge2, edge3]);

            // Edge 1 bis
            Assert.That(graph1.AddEdge(edge1), Is.True);
            AssertHasEdges(graph1, [edge1, edge2, edge3, edge1]);

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.That(graph1.AddEdge(edge4), Is.True);
            AssertHasEdges(graph1, [edge1, edge2, edge3, edge1, edge4]);


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            Assert.That(graph2.AddEdge(edge1), Is.True);
            AssertHasEdges(parent2, [edge1]);
            AssertHasEdges(graph2, [edge1]);

            // Edge 2
            Assert.That(parent2.AddEdge(edge2), Is.True);
            AssertHasEdges(parent2, [edge1, edge2]);
            AssertHasEdges(graph2, [edge1]);

            Assert.That(graph2.AddEdge(edge2), Is.True);
            AssertHasEdges(parent2, [edge1, edge2]);
            AssertHasEdges(graph2, [edge1, edge2]);

            // Edge 3
            Assert.That(graph2.AddEdge(edge3), Is.True);
            AssertHasEdges(parent2, [edge1, edge2, edge3]);
            AssertHasEdges(graph2, [edge1, edge2, edge3]);

            // Edge 1 bis
            Assert.That(graph2.AddEdge(edge1), Is.True);
            AssertHasEdges(parent2, [edge1, edge2, edge3]);
            AssertHasEdges(graph2, [edge1, edge2, edge3, edge1]);

            // Edge 4 self edge
            Assert.That(graph2.AddEdge(edge4), Is.True);
            AssertHasEdges(parent2, [edge1, edge2, edge3, edge4]);
            AssertHasEdges(graph2, [edge1, edge2, edge3, edge1, edge4]);
        }

        protected static void AddEdge_ParallelEdges_EquatableEdge_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.That(graph.AddEdge(edge1), Is.True);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.That(graph.AddEdge(edge2), Is.True);
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2]);

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.That(graph.AddEdge(edge3), Is.True);
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3]);

            // Edge 1 bis
            Assert.That(graph.AddEdge(edge1), Is.True);
            Assert.That(4, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3, edge1]);

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.That(graph.AddEdge(edge4), Is.True);
            Assert.That(5, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3, edge1, edge4]);
        }

        protected static void AddEdge_ParallelEdges_EquatableEdge_Clusters_Test(
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph1,
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> parent2,
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.That(graph1.AddEdge(edge1), Is.True);
            AssertHasEdges(graph1, [edge1]);

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.That(graph1.AddEdge(edge2), Is.True);
            AssertHasEdges(graph1, [edge1, edge2]);

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.That(graph1.AddEdge(edge3), Is.True);
            AssertHasEdges(graph1, [edge1, edge2, edge3]);

            // Edge 1 bis
            Assert.That(graph1.AddEdge(edge1), Is.True);
            AssertHasEdges(graph1, [edge1, edge2, edge3, edge1]);

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.That(graph1.AddEdge(edge4), Is.True);
            AssertHasEdges(graph1, [edge1, edge2, edge3, edge1, edge4]);


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            Assert.That(graph2.AddEdge(edge1), Is.True);
            AssertHasEdges(parent2, [edge1]);
            AssertHasEdges(graph2, [edge1]);

            // Edge 2
            Assert.That(parent2.AddEdge(edge2), Is.True);
            AssertHasEdges(parent2, [edge1, edge2]);
            AssertHasEdges(graph2, [edge1]);

            Assert.That(graph2.AddEdge(edge2), Is.True);
            AssertHasEdges(parent2, [edge1, edge2]);
            AssertHasEdges(graph2, [edge1, edge2]);

            // Edge 3
            Assert.That(graph2.AddEdge(edge3), Is.True);
            AssertHasEdges(parent2, [edge1, edge2, edge3]);
            AssertHasEdges(graph2, [edge1, edge2, edge3]);

            // Edge 1 bis
            Assert.That(graph2.AddEdge(edge1), Is.True);
            AssertHasEdges(parent2, [edge1, edge2, edge3]);
            AssertHasEdges(graph2, [edge1, edge2, edge3, edge1]);

            // Edge 4 self edge
            Assert.That(graph2.AddEdge(edge4), Is.True);
            AssertHasEdges(parent2, [edge1, edge2, edge3, edge4]);
            AssertHasEdges(graph2, [edge1, edge2, edge3, edge1, edge4]);
        }

        protected static void AddEdge_NoParallelEdges_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.That(graph.AddEdge(edge1), Is.True);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.That(graph.AddEdge(edge2), Is.False);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.That(graph.AddEdge(edge3), Is.True);
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge3]);

            // Edge 1 bis
            Assert.That(graph.AddEdge(edge1), Is.False);
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge3]);

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.That(graph.AddEdge(edge4), Is.True);
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge3, edge4]);
        }

        protected static void AddEdge_NoParallelEdges_UndirectedGraph_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.That(graph.AddEdge(edge1), Is.True);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.That(graph.AddEdge(edge2), Is.False);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.That(graph.AddEdge(edge3), Is.False); // Parallel to edge 1
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 1 bis
            Assert.That(graph.AddEdge(edge1), Is.False);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.That(graph.AddEdge(edge4), Is.True);
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge4]);
        }

        protected static void AddEdge_NoParallelEdges_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.That(graph1.AddEdge(edge1), Is.True);
            AssertHasEdges(graph1, [edge1]);

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.That(graph1.AddEdge(edge2), Is.False);
            AssertHasEdges(graph1, [edge1]);

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.That(graph1.AddEdge(edge3), Is.True);
            AssertHasEdges(graph1, [edge1, edge3]);

            // Edge 1 bis
            Assert.That(graph1.AddEdge(edge1), Is.False);
            AssertHasEdges(graph1, [edge1, edge3]);

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.That(graph1.AddEdge(edge4), Is.True);
            AssertHasEdges(graph1, [edge1, edge3, edge4]);


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            Assert.That(graph2.AddEdge(edge1), Is.True);
            AssertHasEdges(parent2, [edge1]);
            AssertHasEdges(graph2, [edge1]);

            // Edge 2
            Assert.That(graph2.AddEdge(edge2), Is.False);
            AssertHasEdges(parent2, [edge1]);
            AssertHasEdges(graph2, [edge1]);

            // Edge 3
            Assert.That(parent2.AddEdge(edge3), Is.True);
            AssertHasEdges(parent2, [edge1, edge3]);
            AssertHasEdges(graph2, [edge1]);

            Assert.That(graph2.AddEdge(edge3), Is.True);
            AssertHasEdges(parent2, [edge1, edge3]);
            AssertHasEdges(graph2, [edge1, edge3]);

            // Edge 1 bis
            Assert.That(graph2.AddEdge(edge1), Is.False);
            AssertHasEdges(parent2, [edge1, edge3]);
            AssertHasEdges(graph2, [edge1, edge3]);

            // Edge 4 self edge
            Assert.That(graph2.AddEdge(edge4), Is.True);
            AssertHasEdges(parent2, [edge1, edge3, edge4]);
            AssertHasEdges(graph2, [edge1, edge3, edge4]);
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.That(graph.AddEdge(edge1), Is.True);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.That(graph.AddEdge(edge2), Is.False);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.That(graph.AddEdge(edge3), Is.True);
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge3]);

            // Edge 1 bis
            Assert.That(graph.AddEdge(edge1), Is.False);
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge3]);

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.That(graph.AddEdge(edge4), Is.True);
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge3, edge4]);
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_UndirectedGraph_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, EquatableEdge<int>>
        {
            int edgeAdded = 0;

            graph.AddVertex(1);
            graph.AddVertex(2);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.That(graph.AddEdge(edge1), Is.True);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.That(graph.AddEdge(edge2), Is.False);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.That(graph.AddEdge(edge3), Is.False); // Parallel to edge 1
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 1 bis
            Assert.That(graph.AddEdge(edge1), Is.False);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.That(graph.AddEdge(edge4), Is.True);
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge4]);
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_Clusters_Test(
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph1,
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> parent2,
            ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);

            AssertNoEdge(graph1);

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.That(graph1.AddEdge(edge1), Is.True);
            AssertHasEdges(graph1, [edge1]);

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.That(graph1.AddEdge(edge2), Is.False);
            AssertHasEdges(graph1, [edge1]);

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.That(graph1.AddEdge(edge3), Is.True);
            AssertHasEdges(graph1, [edge1, edge3]);

            // Edge 1 bis
            Assert.That(graph1.AddEdge(edge1), Is.False);
            AssertHasEdges(graph1, [edge1, edge3]);

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.That(graph1.AddEdge(edge4), Is.True);
            AssertHasEdges(graph1, [edge1, edge3, edge4]);


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1
            Assert.That(graph2.AddEdge(edge1), Is.True);
            AssertHasEdges(parent2, [edge1]);
            AssertHasEdges(graph2, [edge1]);

            // Edge 2
            Assert.That(graph2.AddEdge(edge2), Is.False);
            AssertHasEdges(parent2, [edge1]);
            AssertHasEdges(graph2, [edge1]);

            // Edge 3
            Assert.That(parent2.AddEdge(edge3), Is.True);
            AssertHasEdges(parent2, [edge1, edge3]);
            AssertHasEdges(graph2, [edge1]);

            Assert.That(graph2.AddEdge(edge3), Is.True);
            AssertHasEdges(parent2, [edge1, edge3]);
            AssertHasEdges(graph2, [edge1, edge3]);

            // Edge 1 bis
            Assert.That(graph2.AddEdge(edge1), Is.False);
            AssertHasEdges(parent2, [edge1, edge3]);
            AssertHasEdges(graph2, [edge1, edge3]);

            // Edge 4 self edge
            Assert.That(graph2.AddEdge(edge4), Is.True);
            AssertHasEdges(parent2, [edge1, edge3, edge4]);
            AssertHasEdges(graph2, [edge1, edge3, edge4]);
        }

        protected static void AddEdge_ForbiddenParallelEdges_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.That(graph.AddEdge(edge1), Is.True);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 2
            var edge2 = new Edge<int>(2, 1);
            Assert.That(graph.AddEdge(edge2), Is.True);
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2]);

            // Edge 3 self edge
            var edge3 = new Edge<int>(2, 2);
            Assert.That(graph.AddEdge(edge3), Is.True);
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3]);
        }

        protected static void AddEdge_EquatableEdge_ForbiddenParallelEdges_Test(
            BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.That(graph.AddEdge(edge1), Is.True);
            Assert.That(1, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1]);

            // Edge 2
            var edge2 = new EquatableEdge<int>(2, 1);
            Assert.That(graph.AddEdge(edge2), Is.True);
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2]);

            // Edge 3 self edge
            var edge3 = new EquatableEdge<int>(2, 2);
            Assert.That(graph.AddEdge(edge3), Is.True);
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3]);
        }

        protected static void AddEdge_Throws_EdgesOnly_Test(
            IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdge(null));
            AssertNoEdge(graph);
        }

        protected static void AddEdge_Throws_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            AddEdge_Throws_EdgesOnly_Test(graph);

            // Both vertices not in graph
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(0, 1)));
            AssertNoEdge(graph);

            // Source not in graph
            graph.AddVertex(1);
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(0, 1)));
            AssertNoEdge(graph);

            // Target not in graph
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(1, 0)));
            AssertNoEdge(graph);
        }

        protected static void AddEdge_Throws_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdge(null));
            AssertNoEdge(graph);

            // Both vertices not in graph
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(0, 1)));
            AssertNoEdge(graph);

            // Source not in graph
            graph.AddVertex(1);
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(0, 1)));
            AssertNoEdge(graph);

            // Target not in graph
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdge(new Edge<int>(1, 0)));
            AssertNoEdge(graph);
        }

        protected static void AddEdgeRange_EdgesOnly_Test(
            IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            Assert.That(3, Is.EqualTo(graph.AddEdgeRange([edge1, edge2, edge3])));
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3]);

            // Edge 1, 4
            var edge4 = new Edge<int>(2, 2);
            Assert.That(1, Is.EqualTo(graph.AddEdgeRange([edge1, edge4]))); // Showcase the add of only one edge
            Assert.That(4, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3, edge4]);
        }

        protected static void AddEdgeRange_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            AddEdgeRange_EdgesOnly_Test(graph);
        }

        protected static void AddEdgeRange_ForbiddenParallelEdges_Test()
        {
            int edgeAdded = 0;
            var graph = new BidirectionalMatrixGraph<Edge<int>>(3);

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(0, 1);
            var edge2 = new Edge<int>(0, 2);
            var edge3 = new Edge<int>(1, 2);
            Assert.That(3, Is.EqualTo(graph.AddEdgeRange([edge1, edge2, edge3])));
            Assert.That(3, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3]);

            // Edge 4
            var edge4 = new Edge<int>(2, 2);
            Assert.That(1, Is.EqualTo(graph.AddEdgeRange([edge4])));
            Assert.That(4, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge2, edge3, edge4]);
        }

        protected static void AddEdgeRange_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph1,
            ClusteredAdjacencyGraph<int, Edge<int>> parent2,
            ClusteredAdjacencyGraph<int, Edge<int>> graph2)
        {
            // Graph without parent
            graph1.AddVertex(1);
            graph1.AddVertex(2);
            graph1.AddVertex(3);

            AssertNoEdge(graph1);

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            Assert.That(3, Is.EqualTo(graph1.AddEdgeRange([edge1, edge2, edge3])));
            AssertHasEdges(graph1, [edge1, edge2, edge3]);

            // Edge 1, 4
            var edge4 = new Edge<int>(2, 2);
            Assert.That(1, Is.EqualTo(graph1.AddEdgeRange([edge1, edge4]))); // Showcase the add of only one edge
            AssertHasEdges(graph1, [edge1, edge2, edge3, edge4]);


            // Graph with parent
            graph2.AddVertex(1);
            graph2.AddVertex(2);
            graph2.AddVertex(3);

            AssertNoEdge(parent2);
            AssertNoEdge(graph2);

            // Edge 1, 2, 3
            Assert.That(3, Is.EqualTo(graph2.AddEdgeRange([edge1, edge2, edge3])));
            AssertHasEdges(parent2, [edge1, edge2, edge3]);
            AssertHasEdges(graph2, [edge1, edge2, edge3]);

            // Edge 1, 4
            Assert.That(1, Is.EqualTo(parent2.AddEdgeRange([edge1, edge4]))); // Showcase the add of only one edge
            AssertHasEdges(parent2, [edge1, edge2, edge3, edge4]);
            AssertHasEdges(graph2, [edge1, edge2, edge3]);

            Assert.That(1, Is.EqualTo(graph2.AddEdgeRange([edge1, edge4]))); // Showcase the add of only one edge
            AssertHasEdges(parent2, [edge1, edge2, edge3, edge4]);
            AssertHasEdges(graph2, [edge1, edge2, edge3, edge4]);
        }

        protected static void AddEdgeRange_Throws_EdgesOnly_Test(
            IMutableEdgeListGraph<int, Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(null));
            AssertNoEdge(graph);
            Assert.That(0, Is.EqualTo(edgeAdded));

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange([edge1, null, edge3]));
            Assert.That(0, Is.EqualTo(edgeAdded));
            AssertNoEdge(graph);
        }

        protected static void AddEdgeRange_Throws_Test<TGraph>(TGraph graph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            AddEdgeRange_Throws_EdgesOnly_Test(graph);
        }

        protected static void AddEdgeRange_Throws_Clusters_Test(
            ClusteredAdjacencyGraph<int, Edge<int>> graph)
        {
            graph.AddVertex(1);
            graph.AddVertex(2);
            graph.AddVertex(3);

            AssertNoEdge(graph);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(null));
            AssertNoEdge(graph);

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(1, 2);
            var edge3 = new Edge<int>(2, 3);
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange([edge1, null, edge3]));
            AssertNoEdge(graph);
        }

        protected static void AddEdgeRange_ForbiddenParallelEdges_Throws_Test(
            BidirectionalMatrixGraph<Edge<int>> graph)
        {
            int edgeAdded = 0;

            AssertNoEdge(graph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            graph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++edgeAdded;
            };

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange(null));
            AssertNoEdge(graph);
            Assert.That(0, Is.EqualTo(edgeAdded));

            // Edge 1, 2, 3
            var edge1 = new Edge<int>(0, 1);
            var edge3 = new Edge<int>(1, 2);
            Assert.Throws<ArgumentNullException>(() => graph.AddEdgeRange([edge1, null, edge3]));
            Assert.That(0, Is.EqualTo(edgeAdded));
            AssertNoEdge(graph);

            // Edge 1, 3, 4
            var edge4 = new Edge<int>(0, 1);
            Assert.Throws<ParallelEdgeNotAllowedException>(() => graph.AddEdgeRange([edge1, edge3, edge4]));
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge3]);

            // Out of range => vertex not found
            Assert.Throws<VertexNotFoundException>(() => graph.AddEdgeRange([new Edge<int>(4, 5)]));
            Assert.That(2, Is.EqualTo(edgeAdded));
            AssertHasEdges(graph, [edge1, edge3]);
        }


        protected static void AddEdge_ParallelEdges_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> directedGraph,
            EdgeListGraph<int, Edge<int>> undirectedGraph,
            Func<
                EdgeListGraph<int, Edge<int>>,
                Edge<int>,
                bool> addEdge)
        {
            if (!directedGraph.IsDirected && directedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be directed and allow parallel edges.");
            if (undirectedGraph.IsDirected && undirectedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be undirected and allow parallel edges.");

            int directedEdgeAdded = 0;
            int undirectedEdgeAdded = 0;

            AssertNoEdge(directedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            directedGraph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            undirectedGraph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.That(addEdge(directedGraph, edge1), Is.True);
            Assert.That(1, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1]);

            Assert.That(addEdge(undirectedGraph, edge1), Is.True);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.That(addEdge(directedGraph, edge2), Is.True);
            Assert.That(2, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge2]);

            Assert.That(addEdge(undirectedGraph, edge2), Is.True);
            Assert.That(2, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1, edge2]);

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.That(addEdge(directedGraph, edge3), Is.True);
            Assert.That(3, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge2, edge3]);

            Assert.That(addEdge(undirectedGraph, edge3), Is.True);
            Assert.That(3, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1, edge2, edge3]);

            // Edge 1 bis
            Assert.That(addEdge(directedGraph, edge1), Is.False);
            Assert.That(3, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge2, edge3]);

            Assert.That(addEdge(undirectedGraph, edge1), Is.False);
            Assert.That(3, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1, edge2, edge3]);

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.That(addEdge(directedGraph, edge4), Is.True);
            Assert.That(4, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge2, edge3, edge4]);

            Assert.That(addEdge(undirectedGraph, edge4), Is.True);
            Assert.That(4, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1, edge2, edge3, edge4]);
        }

        protected static void AddEdge_ParallelEdges_EquatableEdge_EdgesOnly_Test(
            EdgeListGraph<int, EquatableEdge<int>> directedGraph,
            EdgeListGraph<int, EquatableEdge<int>> undirectedGraph,
            Func<
                EdgeListGraph<int, EquatableEdge<int>>,
                EquatableEdge<int>,
                bool> addEdge)
        {
            if (!directedGraph.IsDirected && directedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be directed and allow parallel edges.");
            if (undirectedGraph.IsDirected && undirectedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be undirected and allow parallel edges.");

            int directedEdgeAdded = 0;
            int undirectedEdgeAdded = 0;

            AssertNoEdge(directedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            directedGraph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            undirectedGraph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.That(addEdge(directedGraph, edge1), Is.True);
            Assert.That(1, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1]);

            Assert.That(addEdge(undirectedGraph, edge1), Is.True);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.That(addEdge(directedGraph, edge2), Is.False);
            Assert.That(1, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1]);

            Assert.That(addEdge(undirectedGraph, edge2), Is.False);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.That(addEdge(directedGraph, edge3), Is.True);
            Assert.That(2, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge3]);

            Assert.That(addEdge(undirectedGraph, edge3), Is.True);
            Assert.That(2, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1, edge3]);

            // Edge 1 bis
            Assert.That(addEdge(directedGraph, edge1), Is.False);
            Assert.That(2, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge3]);

            Assert.That(addEdge(undirectedGraph, edge1), Is.False);
            Assert.That(2, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1, edge3]);

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.That(addEdge(directedGraph, edge4), Is.True);
            Assert.That(3, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge3, edge4]);

            Assert.That(addEdge(undirectedGraph, edge4), Is.True);
            Assert.That(3, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1, edge3, edge4]);
        }

        protected static void AddEdge_NoParallelEdges_EdgesOnly_Test(
            EdgeListGraph<int, Edge<int>> directedGraph,
            EdgeListGraph<int, Edge<int>> undirectedGraph,
            Func<
                EdgeListGraph<int, Edge<int>>,
                Edge<int>,
                bool> addEdge)
        {
            if (!directedGraph.IsDirected && !directedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be directed and not allow parallel edges.");
            if (undirectedGraph.IsDirected && !undirectedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be undirected and not allow parallel edges.");

            int directedEdgeAdded = 0;
            int undirectedEdgeAdded = 0;

            AssertNoEdge(directedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            directedGraph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            undirectedGraph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new Edge<int>(1, 2);
            Assert.That(addEdge(directedGraph, edge1), Is.True);
            Assert.That(1, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1]);

            Assert.That(addEdge(undirectedGraph, edge1), Is.True);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 2
            var edge2 = new Edge<int>(1, 2);
            Assert.That(addEdge(directedGraph, edge2), Is.False);
            Assert.That(1, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1]);

            Assert.That(addEdge(undirectedGraph, edge2), Is.False);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 3
            var edge3 = new Edge<int>(2, 1);
            Assert.That(addEdge(directedGraph, edge3), Is.True);
            Assert.That(2, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge3]);

            Assert.That(addEdge(undirectedGraph, edge3), Is.False);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 1 bis
            Assert.That(addEdge(directedGraph, edge1), Is.False);
            Assert.That(2, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge3]);

            Assert.That(addEdge(undirectedGraph, edge1), Is.False);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 4 self edge
            var edge4 = new Edge<int>(2, 2);
            Assert.That(addEdge(directedGraph, edge4), Is.True);
            Assert.That(3, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge3, edge4]);

            Assert.That(addEdge(undirectedGraph, edge4), Is.True);
            Assert.That(2, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1, edge4]);
        }

        protected static void AddEdge_NoParallelEdges_EquatableEdge_EdgesOnly_Test(
            EdgeListGraph<int, EquatableEdge<int>> directedGraph,
            EdgeListGraph<int, EquatableEdge<int>> undirectedGraph,
            Func<
                EdgeListGraph<int, EquatableEdge<int>>,
                EquatableEdge<int>,
                bool> addEdge)
        {
            if (!directedGraph.IsDirected && !directedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be directed and not allow parallel edges.");
            if (undirectedGraph.IsDirected && !undirectedGraph.AllowParallelEdges)
                throw new InvalidOperationException("Graph must be undirected and not allow parallel edges.");

            int directedEdgeAdded = 0;
            int undirectedEdgeAdded = 0;

            AssertNoEdge(directedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            directedGraph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++directedEdgeAdded;
            };

            AssertNoEdge(undirectedGraph);
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            undirectedGraph.EdgeAdded += e =>
            {
                Assert.That(e, Is.Not.Null);
                ++undirectedEdgeAdded;
            };

            // Edge 1
            var edge1 = new EquatableEdge<int>(1, 2);
            Assert.That(addEdge(directedGraph, edge1), Is.True);
            Assert.That(1, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1]);

            Assert.That(addEdge(undirectedGraph, edge1), Is.True);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 2
            var edge2 = new EquatableEdge<int>(1, 2);
            Assert.That(addEdge(directedGraph, edge2), Is.False);
            Assert.That(1, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1]);

            Assert.That(addEdge(undirectedGraph, edge2), Is.False);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 3
            var edge3 = new EquatableEdge<int>(2, 1);
            Assert.That(addEdge(directedGraph, edge3), Is.True);
            Assert.That(2, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge3]);

            Assert.That(addEdge(undirectedGraph, edge3), Is.False);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 1 bis
            Assert.That(addEdge(directedGraph, edge1), Is.False);
            Assert.That(2, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge3]);

            Assert.That(addEdge(undirectedGraph, edge1), Is.False);
            Assert.That(1, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1]);

            // Edge 4 self edge
            var edge4 = new EquatableEdge<int>(2, 2);
            Assert.That(addEdge(directedGraph, edge4), Is.True);
            Assert.That(3, Is.EqualTo(directedEdgeAdded));
            AssertHasEdges(directedGraph, [edge1, edge3, edge4]);

            Assert.That(addEdge(undirectedGraph, edge4), Is.True);
            Assert.That(2, Is.EqualTo(undirectedEdgeAdded));
            AssertHasEdges(undirectedGraph, [edge1, edge4]);
        }


        protected static void AddEdge_ImmutableGraph_NoUpdate<TGraph>(
            TGraph wrappedGraph,
            Func<IEdgeSet<int, Edge<int>>> createGraph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            IEdgeSet<int, Edge<int>> graph = createGraph();

            var edge = new Edge<int>(1, 2);
            wrappedGraph.AddVertex(1);
            wrappedGraph.AddVertex(2);
            wrappedGraph.AddEdge(edge);

            AssertNoEdge(graph); // Graph is not updated
        }

        protected static void AddEdge_ImmutableGraph_WithUpdate<TGraph>(
            TGraph wrappedGraph,
            Func<IEdgeSet<int, Edge<int>>> createGraph)
            where TGraph : IMutableVertexSet<int>, IMutableEdgeListGraph<int, Edge<int>>
        {
            IEdgeSet<int, Edge<int>> graph = createGraph();

            var edge = new Edge<int>(1, 2);
            wrappedGraph.AddVertex(1);
            wrappedGraph.AddVertex(2);
            wrappedGraph.AddEdge(edge);

            AssertHasEdges(graph, [edge]); // Graph is updated
        }

        #endregion
    }
}