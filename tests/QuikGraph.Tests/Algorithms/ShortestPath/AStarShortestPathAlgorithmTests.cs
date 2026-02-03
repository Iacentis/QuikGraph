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
    /// Tests for <see cref="AStarShortestPathAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class AStartShortestPathAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunAStarAndCheck<TVertex, TEdge>(
            IVertexAndEdgeListGraph<TVertex, TEdge> graph,
            TVertex root)
            where TEdge : IEdge<TVertex>
        {
            var distances = new Dictionary<TEdge, double>();
            foreach (TEdge edge in graph.Edges)
                distances[edge] = graph.OutDegree(edge.Source) + 1;

            var algorithm = new AStarShortestPathAlgorithm<TVertex, TEdge>(
                graph,
                e => distances[e],
                _ => 0.0);

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

            var predecessors = new VertexPredecessorRecorderObserver<TVertex, TEdge>();
            using (predecessors.Attach(algorithm))
                algorithm.Compute(root);

            CollectionAssert.IsNotEmpty(algorithm.GetDistances());
            Assert.That(graph.VertexCount, Is.EqualTo(algorithm.GetDistances().Count()));

            Verify(algorithm, predecessors);
        }

        private static void Verify<TVertex, TEdge>(
            AStarShortestPathAlgorithm<TVertex, TEdge> algorithm,
            VertexPredecessorRecorderObserver<TVertex, TEdge> predecessors)
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
                    algorithm.TryGetDistance(predecessor.Source, out double predecessorDistance),
                    Is.EqualTo(algorithm.TryGetDistance(vertex, out double currentDistance)));
                Assert.That(currentDistance, Is.GreaterThanOrEqualTo(predecessorDistance));
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            Func<int, double> Heuristic = _ => 1.0;
            Func<Edge<int>, double> Weights = _ => 1.0;

            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, Heuristic);
            AssertAlgorithmProperties(algorithm, graph, Heuristic, Weights);

            algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, Heuristic,
                DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, Heuristic, Weights, DistanceRelaxers.CriticalDistance);

            algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(null, graph, Weights, Heuristic,
                DistanceRelaxers.CriticalDistance);
            AssertAlgorithmProperties(algorithm, graph, Heuristic, Weights, DistanceRelaxers.CriticalDistance);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                AStarShortestPathAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g,
                Func<TVertex, double> heuristic = null,
                Func<TEdge, double> eWeights = null,
                IDistanceRelaxer relaxer = null)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.That(algo.VerticesColors, Is.Null);
                if (heuristic is null)
                    Assert.That(algo.CostHeuristic, Is.Not.Null);
                else
                    Assert.That(heuristic, Is.SameAs(algo.CostHeuristic));
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
            var graph = new AdjacencyGraph<int, Edge<int>>();

            Func<int, double> Heuristic = _ => 1.0;
            Func<Edge<int>, double> Weights = _ => 1.0;

            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, Weights, Heuristic));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(graph, null, Heuristic));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(graph, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, Heuristic));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, Weights, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, Heuristic));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, null));

            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, Weights, Heuristic,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(graph, null, Heuristic,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, null,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, Heuristic, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(graph, Weights, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(graph, null, Heuristic, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(graph, null, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, Weights, Heuristic, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, Weights, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, Heuristic,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, Weights, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, Heuristic, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, null, DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, null, null));

            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, Weights, Heuristic,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, graph, null, Heuristic,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, graph, Weights, null,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, graph, Weights, Heuristic, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, graph, Weights, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, graph, null, Heuristic, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, graph, null, null,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, Weights, Heuristic, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, Weights, null,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, null, Heuristic,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, Weights, null, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, null, Heuristic, null));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, null, null,
                    DistanceRelaxers.CriticalDistance));
            Assert.Throws<ArgumentNullException>(() =>
                new AStarShortestPathAlgorithm<int, Edge<int>>(null, null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new AStarShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0, _ => 0.0);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_NoThrows_Test(
                graph,
                () => new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(() =>
                new AStarShortestPathAlgorithm<TestVertex, Edge<TestVertex>>(graph, _ => 1.0, _ => 0.0));
        }

        #endregion

        [Test]
        public void GetVertexColor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdge(new Edge<int>(1, 2));

            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(graph, _ => 1.0, _ => 0.0);
            algorithm.Compute(1);

            Assert.That(GraphColor.Black, Is.EqualTo(algorithm.GetVertexColor(1)));
            Assert.That(GraphColor.Black, Is.EqualTo(algorithm.GetVertexColor(2)));
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void AStar()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_SlowTests())
            {
                foreach (string root in graph.Vertices)
                    RunAStarAndCheck(graph, root);
            }
        }

        [Test]
        public void AStar_Throws()
        {
            var edge12 = new Edge<int>(1, 2);
            var edge23 = new Edge<int>(2, 3);
            var edge34 = new Edge<int>(3, 4);

            var negativeWeightGraph = new AdjacencyGraph<int, Edge<int>>();
            negativeWeightGraph.AddVerticesAndEdgeRange([
                edge12, edge23, edge34
            ]);

            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(
                negativeWeightGraph,
                e =>
                {
                    if (e == edge12)
                        return 12.0;
                    if (e == edge23)
                        return -23.0;
                    if (e == edge34)
                        return 34.0;
                    return 1.0;
                },
                _ => 0.0);
            Assert.Throws<NegativeWeightException>(() => algorithm.Compute(1));
        }

        [Test]
        public void AStar_HeuristicCalls()
        {
            var edge01 = new Edge<int>(0, 1);
            var edge02 = new Edge<int>(0, 2);
            var edge03 = new Edge<int>(0, 3);
            var edge14 = new Edge<int>(1, 4);
            var edge23 = new Edge<int>(2, 3);
            var edge34 = new Edge<int>(3, 4);

            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange([
                edge01,
                edge02,
                edge03,
                edge23,
                edge14,
                edge34
            ]);

            const int root = 0;

            var colorUpdates = new HashSet<GraphColor> { GraphColor.White, GraphColor.Gray, GraphColor.Black };

            int heuristicCalls = 0;
            AStarShortestPathAlgorithm<int, Edge<int>> algorithm = null;

            algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(
                graph,
                e =>
                {
                    if (e == edge01)
                        return 8.0;
                    if (e == edge02)
                        return 6.0;
                    if (e == edge03)
                        return 20.0;
                    if (e == edge34)
                        return 5.0;
                    return 1.0;
                },
                Heuristic);

            algorithm.Compute(root);

            CollectionAssert.IsEmpty(colorUpdates);
            return;

            double Heuristic(int v)
            {
                // ReSharper disable once PossibleNullReferenceException
                // ReSharper disable once AccessToModifiedClosure
                colorUpdates.Remove(algorithm!.GetVertexColor(v));
                ++heuristicCalls;
                return 10.0 / heuristicCalls;
            }
        }

        [Test]
        public void AStar_HeuristicCallCount()
        {
            var lineGraph = new AdjacencyGraph<int, Edge<int>>();
            lineGraph.AddVerticesAndEdgeRange([
                new Edge<int>(2, 3),
                new Edge<int>(3, 4),
                new Edge<int>(2, 1),
                new Edge<int>(1, 0)
            ]);

            const int root = 2;

            var heuristicCalls = new List<int>();
            var algorithm = new AStarShortestPathAlgorithm<int, Edge<int>>(
                lineGraph,
                _ => 1.0,
                v =>
                {
                    // Goal is 2, h(v) = v
                    heuristicCalls.Add(v);
                    return v;
                });

            algorithm.Compute(root);

            // Heuristic function must be called at least 4 times
            Assert.That(heuristicCalls.Count, Is.AtLeast(4));

            // 0 must be expanded before 4
            Assert.That(heuristicCalls, Does.Contain(0));
            Assert.That(heuristicCalls, Does.Contain(4));
            Assert.That(heuristicCalls.IndexOf(0), Is.LessThan(heuristicCalls.IndexOf(4)));
        }

        [Pure]
        public static AStarShortestPathAlgorithm<T, Edge<T>> CreateAlgorithmAndMaybeDoComputation<T>(
            ContractScenario<T> scenario)
        {
            var graph = new AdjacencyGraph<T, Edge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new Edge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            double Heuristic(T v) => 1.0;
            double Weights(Edge<T> e) => 1.0;
            var algorithm = new AStarShortestPathAlgorithm<T, Edge<T>>(graph, Weights, Heuristic);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root);
            return algorithm;
        }
    }
}