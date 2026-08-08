using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;
using QuikGraph.Collections;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.Search
{
    /// <summary>
    /// Tests for <see cref="BreadthFirstSearchAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class BreadthFirstAlgorithmSearchTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        public void RunBFSAndCheck<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            TVertex sourceVertex)
            where TEdge : IEdge<TVertex>
        {
            var parents = new Dictionary<TVertex, TVertex>();
            var distances = new Dictionary<TVertex, int>();
            TVertex currentVertex = default;
            int currentDistance = 0;
            var algorithm = new BreadthFirstSearchAlgorithm<TVertex, TEdge>(graph);

            algorithm.InitializeVertex += vertex =>
            {
                Assert.That(GraphColor.White, Is.EqualTo(algorithm.VerticesColors[vertex]));
            };

            algorithm.StartVertex += vertex =>
            {
                Assert.That(GraphColor.White, Is.EqualTo(algorithm.VerticesColors[vertex]));
            };

            algorithm.DiscoverVertex += vertex =>
            {
                Assert.That(GraphColor.Gray, Is.EqualTo(algorithm.VerticesColors[vertex]));
                if (vertex.Equals(sourceVertex))
                {
                    currentVertex = sourceVertex;
                }
                else
                {
                    Assert.That(currentVertex, Is.Not.Null);
                    Assert.That(parents[vertex], Is.EqualTo(currentVertex));
                    // ReSharper disable once AccessToModifiedClosure
                    Assert.That(distances[vertex], Is.EqualTo(currentDistance + 1));
                    Assert.That(distances[vertex], Is.EqualTo(distances[parents[vertex]] + 1));
                }
            };

            algorithm.ExamineEdge += edge =>
            {
                Assert.That(edge.Source, Is.EqualTo(currentVertex));
            };

            algorithm.ExamineVertex += vertex =>
            {
                TVertex u = vertex;
                currentVertex = u;
                // Ensure that the distances monotonically increase.
                // ReSharper disable AccessToModifiedClosure
                Assert.That(distances[u] == currentDistance || distances[u] == currentDistance + 1, Is.True);

                if (distances[u] == currentDistance + 1) // New level
                    ++currentDistance;
                // ReSharper restore AccessToModifiedClosure
            };

            algorithm.TreeEdge += edge =>
            {
                TVertex u = edge.Source;
                TVertex v = edge.Target;

                Assert.That(GraphColor.White, Is.EqualTo(algorithm.VerticesColors[v]));
                Assert.That(distances[u], Is.EqualTo(currentDistance));
                parents[v] = u;
                distances[v] = distances[u] + 1;
            };

            algorithm.NonTreeEdge += edge =>
            {
                TVertex u = edge.Source;
                TVertex v = edge.Target;

                Assert.That(algorithm.VerticesColors[v] == GraphColor.White, Is.False);

                if (algorithm.VisitedGraph.IsDirected)
                {
                    // Cross or back edge
                    Assert.That(distances[v] <= distances[u] + 1, Is.True);
                }
                else
                {
                    // Cross edge (or going backwards on a tree edge)
                    Assert.That(
                        distances[v] == distances[u]
                        || distances[v] == distances[u] + 1
                        || distances[v] == distances[u] - 1, Is.True);
                }
            };

            algorithm.GrayTarget += edge =>
            {
                Assert.That(GraphColor.Gray, Is.EqualTo(algorithm.VerticesColors[edge.Target]));
            };

            algorithm.BlackTarget += edge =>
            {
                Assert.That(GraphColor.Black, Is.EqualTo(algorithm.VerticesColors[edge.Target]));

                foreach (TEdge outEdge in algorithm.VisitedGraph.OutEdges(edge.Target))
                    Assert.That(algorithm.VerticesColors[outEdge.Target] == GraphColor.White, Is.False);
            };

            algorithm.FinishVertex += vertex =>
            {
                Assert.That(GraphColor.Black, Is.EqualTo(algorithm.VerticesColors[vertex]));
            };

            parents.Clear();
            distances.Clear();
            currentDistance = 0;

            foreach (TVertex vertex in graph.Vertices)
            {
                distances[vertex] = int.MaxValue;
                parents[vertex] = vertex;
            }

            distances[sourceVertex] = 0;

            var recorder = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (recorder.Attach(algorithm))
            {
                algorithm.Compute(sourceVertex);
            }

            // All white vertices should be unreachable from the source.
            foreach (TVertex vertex in graph.Vertices)
            {
                if (algorithm.VerticesColors[vertex] == GraphColor.White)
                {
                    // Check !IsReachable(sourceVertex, vertex, graph);
                    if (recorder.TryGetPath(vertex, out IEnumerable<TEdge> path))
                    {
                        foreach (TEdge edge in path)
                        {
                            Assert.That(sourceVertex, Is.Not.EqualTo(edge.Source));
                            Assert.That(sourceVertex, Is.Not.EqualTo(edge.Target));
                        }
                    }
                }
            }

            // The shortest path to a child should be one longer than
            // shortest path to the parent.
            foreach (TVertex vertex in graph.Vertices)
            {
                if (!parents[vertex].Equals(vertex)) // Not the root of the BFS tree
                    Assert.That(distances[vertex], Is.EqualTo(distances[parents[vertex]] + 1));
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            var verticesColors = new Dictionary<int, GraphColor>();
            var queue = new BinaryQueue<int, double>(_ => 1.0);
            algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph, queue, verticesColors);
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, queue, verticesColors);
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, queue, verticesColors,
                edges => edges.Where(e => e != null));
            AssertAlgorithmProperties(algorithm, graph, verticesColors);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                BreadthFirstSearchAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g,
                IDictionary<TVertex, GraphColor> vColors = null)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                if (vColors is null)
                    CollectionAssert.IsEmpty(algo.VerticesColors);
                else
                    Assert.That(vColors, Is.SameAs(algo.VerticesColors));
                Assert.That(algo.OutEdgesFilter, Is.Not.Null);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var verticesColors = new Dictionary<int, GraphColor>();
            var queue = new BinaryQueue<int, double>(_ => 1.0);
            IEnumerable<Edge<int>> Filter(IEnumerable<Edge<int>> edges) => edges.Where(e => e != null);

            Assert.Throws<ArgumentNullException>(() => new BreadthFirstSearchAlgorithm<int, Edge<int>>(null));

            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, queue, verticesColors));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph, null, verticesColors));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph, queue, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, verticesColors));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, queue, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, null));

            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, queue, verticesColors));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, null, verticesColors));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, queue, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, null, verticesColors));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, queue, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, null, null));

            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, queue, verticesColors, Filter));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, null, verticesColors, Filter));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, queue, null, Filter));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, queue, verticesColors, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, null, verticesColors, Filter));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, queue, null, Filter));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, queue, verticesColors, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, null, null, Filter));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, null, verticesColors, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, queue, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, graph, null, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, queue, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, null, verticesColors, null));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, null, null, Filter));
            Assert.Throws<ArgumentNullException>(() =>
                new BreadthFirstSearchAlgorithm<int, Edge<int>>(null, null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new BreadthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_NoThrows_Test(
                graph,
                () => new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(() => new BreadthFirstSearchAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));

            var algorithm = new BreadthFirstSearchAlgorithm<int, Edge<int>>(graph);
            // Algorithm not run
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => algorithm.GetVertexColor(1));

            algorithm.Compute();

            Assert.That(GraphColor.Black, Is.EqualTo(algorithm.GetVertexColor(1)));
            Assert.That(GraphColor.Black, Is.EqualTo(algorithm.GetVertexColor(2)));
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void BreadthFirstSearch()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests())
            {
                foreach (string vertex in graph.Vertices)
                    RunBFSAndCheck(graph, vertex);
            }
        }

        [Pure]
        public static BreadthFirstSearchAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
        {
            var graph = new AdjacencyGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            var algorithm = new BreadthFirstSearchAlgorithm<T, Edge<T>>(graph);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root);
            return algorithm;
        }
    }
}