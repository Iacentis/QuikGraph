using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.ShortestPath;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Tests for <see cref="UndirectedDijkstraShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class UndirectedDijkstraShortestPathAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunUndirectedDijkstraAndCheck<TVertex, TEdge>(IUndirectedGraph<TVertex, TEdge> graph,
            TVertex root)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.AdjacentDegree(edge.Source) + 1;

            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<TVertex, TEdge>(graph, e => distances[e]);
            var predecessors = new UndirectedVertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algorithm))
                algorithm.Compute(root);

            algorithm.InitializeVertex += vertex =>
            {
                Assert.That(GraphColor.White, Is.EqualTo(algorithm.VerticesColors[vertex]));
            };

            algorithm.DiscoverVertex += vertex =>
            {
                Assert.That(GraphColor.Gray, Is.EqualTo(algorithm.VerticesColors[vertex]));
            };

            algorithm.FinishVertex += vertex =>
            {
                Assert.That(GraphColor.Black, Is.EqualTo(algorithm.VerticesColors[vertex]));
            };

            CollectionAssert.IsNotEmpty(algorithm.GetDistances());
            Assert.That(graph.VertexCount, Is.EqualTo(algorithm.GetDistances().Count()));

            Verify(algorithm, predecessors);
        }

        private static void Verify<TVertex, TEdge>(
            UndirectedDijkstraShortestPathAlgorithm<TVertex, TEdge> algorithm,
            UndirectedVertexPredecessorRecorderObserver<TVertex, TEdge> predecessors)
            where TEdge : IEdge<TVertex>
        {
            // Verify the result
            foreach (TVertex vertex in algorithm.VisitedGraph.Vertices)
            {
                if (!predecessors.VerticesPredecessors.TryGetValue(vertex, out TEdge predecessor))
                    continue;
                if (predecessor.Source.Equals(vertex))
                    continue;
                Assert.That(
                    algorithm.TryGetDistance(vertex, out double currentDistance),
                    Is.EqualTo(algorithm.TryGetDistance(predecessor.Source, out double predecessorDistance)));
                Assert.That(currentDistance, Is.GreaterThanOrEqualTo(predecessorDistance));
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            Func<Edge<int>, double> Weights = _ => 1.0;

            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, Weights);
            AssertAlgorithmProperties(algorithm, graph, Weights);

            algorithm = new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, Weights,
                DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, Weights, DistanceRelaxers.CriticalDistance);

            algorithm = new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, graph, Weights,
                DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, Weights, DistanceRelaxers.CriticalDistance);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                UndirectedDijkstraShortestPathAlgorithm<TVertex, TEdge> algo,
                IUndirectedGraph<TVertex, TEdge> g,
                Func<TEdge, double> eWeights = null,
                IDistanceRelaxer relaxer = null)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.That(algo.VerticesColors, Is.Null);
                if (eWeights is null)
                    Assert.That(algo.Weights, Is.Not.Null);
                else
                    Assert.That(eWeights, Is.SameAs(algo.Weights));
                CollectionAssert.IsEmpty(algo.GetDistances());
                if (relaxer is null)
                    Assert.That(algo.DistanceRelaxer, Is.Not.Null);
                else
                    Assert.That(relaxer, Is.SameAs(algo.DistanceRelaxer));
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new UndirectedGraph<int, Edge<int>>();

            Func<Edge<int>, double> Weights = _ => 1.0;

            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, Weights));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, null));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, null));

            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, Weights,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, null,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, Weights, null));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, null,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, Weights, null));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, null, null));

            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, null, Weights,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, graph, null,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, graph, Weights, null));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, null, null,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, null, Weights, null));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, graph, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            ComputeWithoutRoot_NoThrows_Test(
                graph,
                () => new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new UndirectedGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(() =>
                new UndirectedDijkstraShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new UndirectedGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));

            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0);
            algorithm.Compute(1);

            Assert.That(GraphColor.Black, Is.EqualTo(algorithm.GetVertexColor(1)));
            Assert.That(GraphColor.Black, Is.EqualTo(algorithm.GetVertexColor(2)));
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void UndirectedDijkstra()
        {
            foreach (UndirectedGraph<string, Edge<string>> graph in TestGraphFactory.GetUndirectedGraphs_SlowTests(20))
            {
                int cut = 0;
                foreach (string root in graph.Vertices)
                {
                    if (cut++ > 10)
                        break;
                    RunUndirectedDijkstraAndCheck(graph, root);
                }
            }
        }

        [Test]
        public void UndirectedDijkstraSimpleGraph()
        {
            var undirectedGraph = new UndirectedGraph<object, Edge<object>>(true);
            object v1 = "vertex1";
            object v2 = "vertex2";
            object v3 = "vertex3";
            var e1 = new Edge<object>(v1, v2);
            var e2 = new Edge<object>(v2, v3);
            var e3 = new Edge<object>(v3, v1);
            undirectedGraph.AddVertex(v1);
            undirectedGraph.AddVertex(v2);
            undirectedGraph.AddVertex(v3);
            undirectedGraph.AddEdge(e1);
            undirectedGraph.AddEdge(e2);
            undirectedGraph.AddEdge(e3);

            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<object, Edge<object>>(
                undirectedGraph,
                _ => 1.0);
            var observer = new UndirectedVertexPredecessorRecorderObserver<object, Edge<object>>();
            using (observer.Attach(algorithm))
                algorithm.Compute(v1);

            Assert.That(observer.TryGetPath(v3, out _), Is.True);
        }

        [Pure]
        public static UndirectedDijkstraShortestPathAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
        {
            var graph = new UndirectedGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            double Weights(Edge<T> e) => 1.0;
            var algorithm = new UndirectedDijkstraShortestPathAlgorithm<T, Edge<T>>(graph, Weights);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root);
            return algorithm;
        }
    }
}