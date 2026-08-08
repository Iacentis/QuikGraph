using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Algorithms.ConnectedComponents;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.ConnectedComponents
{
    /// <summary>
    /// Tests for <see cref="StronglyConnectedComponentsAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class StronglyConnectedComponentsAlgorithmTests
    {
        #region Test helpers

        public void RunStronglyConnectedComponentsAndCheck<TVertex, TEdge>( IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            var algorithm = new StronglyConnectedComponentsAlgorithm<TVertex, TEdge>(graph);
            algorithm.Compute();

            Assert.That(graph.VertexCount,Is.EqualTo(algorithm.Components.Count));
            Assert.That(graph.VertexCount,Is.EqualTo(algorithm.Roots.Count));
            Assert.That(graph.VertexCount,Is.EqualTo(algorithm.DiscoverTimes.Count));
            if (graph.VertexCount == 0)
            {
                Assert.That(0,Is.EqualTo(algorithm.Steps));
                Assert.That(algorithm.ComponentCount == 0,Is.True);
                return;
            }

            Assert.That(algorithm.ComponentCount, Is.Positive);
            Assert.That(algorithm.ComponentCount, Is.AtMost(graph.VertexCount));
            foreach (TVertex vertex in algorithm.VisitedGraph.Vertices)
            {
                Assert.That(algorithm.Components.ContainsKey(vertex),Is.True);
                Assert.That(algorithm.DiscoverTimes.ContainsKey(vertex),Is.True);
            }

            Assert.That(algorithm.Steps, Is.Positive);
            AssertStepsProperties();
            foreach (KeyValuePair<TVertex, int> pair in algorithm.Components)
            {
                Assert.That(pair.Value, Is.GreaterThanOrEqualTo(0));
                Assert.That(pair.Value < algorithm.ComponentCount, Is.True, $"{pair.Value} < {algorithm.ComponentCount}");
            }

            foreach (KeyValuePair<TVertex, int> time in algorithm.DiscoverTimes)
            {
                Assert.That(time.Key,Is.Not.Null);
            }

            foreach (TVertex vertex in graph.Vertices)
            {
                Assert.That(algorithm.Components[vertex], Is.GreaterThanOrEqualTo(0));
            }

            #region Local function

            void AssertStepsProperties()
            {
                Assert.That(algorithm.Steps, Is.GreaterThanOrEqualTo(0));
                Assert.That(algorithm.Steps,Is.EqualTo(algorithm.VerticesPerStep.Count));
                Assert.That(algorithm.Steps,Is.EqualTo(algorithm.ComponentsPerStep.Count));
            }

            #endregion
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();
            var algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph, components);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(null, graph, components);
            AssertAlgorithmProperties(algorithm, graph);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                StronglyConnectedComponentsAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.That(0,Is.EqualTo(algo.ComponentCount));
                CollectionAssert.IsEmpty(algo.Components);
                CollectionAssert.IsEmpty(algo.Graphs);
                CollectionAssert.IsEmpty(algo.Roots);

                Assert.That(0,Is.EqualTo(algo.Steps));
                Assert.That(algo.VerticesPerStep,Is.Null);
                Assert.That(algo.ComponentsPerStep,Is.Null);
                CollectionAssert.IsEmpty(algo.DiscoverTimes);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var components = new Dictionary<int, int>();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(null));

            Assert.Throws<ArgumentNullException>(
                () => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(null, components));
            Assert.Throws<ArgumentNullException>(
                () => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(null, null));

            Assert.Throws<ArgumentNullException>(
                () => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(null, graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(null, null, components));
            Assert.Throws<ArgumentNullException>(
                () => new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void OneComponent()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange([
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(3, 1)
            ]);

            var algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            Assert.That(1,Is.EqualTo(algorithm.ComponentCount));
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [1] = 0,
                    [2] = 0,
                    [3] = 0
                },
                algorithm.Components);
            Assert.That(1,Is.EqualTo(algorithm.Graphs.Length));
            CollectionAssert.AreEquivalent(
                new[] { 1, 2, 3 },
                algorithm.Graphs[0].Vertices);
        }

        [Test]
        public void ThreeComponents()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange([
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(2, 4),
                new Edge<int>(3, 1),
                new Edge<int>(4, 5)
            ]);

            var algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            Assert.That(3,Is.EqualTo(algorithm.ComponentCount));
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [1] = 2,
                    [2] = 2,
                    [3] = 2,
                    [4] = 1,
                    [5] = 0
                },
                algorithm.Components);
            Assert.That(3,Is.EqualTo(algorithm.Graphs.Length));
            CollectionAssert.AreEquivalent(
                new[] { 5 },
                algorithm.Graphs[0].Vertices);
            CollectionAssert.AreEquivalent(
                new[] { 4 },
                algorithm.Graphs[1].Vertices);
            CollectionAssert.AreEquivalent(
                new[] { 1, 2, 3 },
                algorithm.Graphs[2].Vertices);
        }

        [Test]
        public void MultipleComponents()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange([
                new Edge<int>(1, 2),
                new Edge<int>(2, 3),
                new Edge<int>(2, 4),
                new Edge<int>(2, 5),
                new Edge<int>(3, 1),
                new Edge<int>(3, 4),
                new Edge<int>(4, 6),
                new Edge<int>(5, 6),
                new Edge<int>(5, 7),
                new Edge<int>(6, 4),
                new Edge<int>(7, 5),
                new Edge<int>(7, 8),
                new Edge<int>(8, 6),
                new Edge<int>(8, 7)
            ]);
            graph.AddVertex(10);

            var algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(graph);
            algorithm.Compute();

            Assert.That(4,Is.EqualTo(algorithm.ComponentCount));
            CollectionAssert.AreEquivalent(
                new Dictionary<int, int>
                {
                    [1] = 2,
                    [2] = 2,
                    [3] = 2,
                    [4] = 0,
                    [5] = 1,
                    [6] = 0,
                    [7] = 1,
                    [8] = 1,
                    [10] = 3
                },
                algorithm.Components);
            Assert.That(4,Is.EqualTo(algorithm.Graphs.Length));
            CollectionAssert.AreEquivalent(
                new[] { 4, 6 },
                algorithm.Graphs[0].Vertices);
            CollectionAssert.AreEquivalent(
                new[] { 5, 7, 8 },
                algorithm.Graphs[1].Vertices);
            CollectionAssert.AreEquivalent(
                new[] { 1, 2, 3 },
                algorithm.Graphs[2].Vertices);
            CollectionAssert.AreEquivalent(
                new[] { 10 },
                algorithm.Graphs[3].Vertices);
        }

        [Test]
        [Category(TestCategories.LongRunning)]
        public void StronglyConnectedComponents()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                RunStronglyConnectedComponentsAndCheck(graph);
        }
    }
}