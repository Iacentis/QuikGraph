using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.TSP;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.TSP
{
    /// <summary>
    /// Tests for <see cref="TSP{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class TSPTests : RootedAlgorithmTestsBase
    {
        #region Test helpers & classes

        private class TestCase
        {

            public BidirectionalGraph<string, EquatableEdge<string>> Graph { get; } = new BidirectionalGraph<string, EquatableEdge<string>>();


            private readonly Dictionary<EquatableEdge<string>, double> _weightsDict = new Dictionary<EquatableEdge<string>, double>();


            public TestCase AddVertex( string vertex)
            {
                Graph.AddVertex(vertex);
                return this;
            }


            public TestCase AddUndirectedEdge( string source,  string target, double weight)
            {
                AddDirectedEdge(source, target, weight);
                AddDirectedEdge(target, source, weight);
                return this;
            }


            public TestCase AddDirectedEdge( string source,  string target, double weight)
            {
                var edge = new EquatableEdge<string>(source, target);
                Graph.AddEdge(edge);
                _weightsDict.Add(edge, weight);

                return this;
            }

            [Pure]

            public Func<EquatableEdge<string>, double> GetWeightsFunc()
            {
                return edge => _weightsDict[edge];
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            Func<EquatableEdge<int>, double> Weights = _ => 1.0;
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();

            var algorithm = new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph, Weights);
            AssertAlgorithmProperties(algorithm, graph, Weights);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge, TGraph>(
                TSP<TVertex, TEdge, TGraph> algo,
                TGraph g,
                Func<TEdge, double> eWeights)
                where TEdge : EquatableEdge<TVertex>
                where TGraph : BidirectionalGraph<TVertex, TEdge>
            {
                AssertAlgorithmState(algo, g);
                Assert.That(algo.VerticesColors,Is.Null);
                if (eWeights is null)
                    Assert.That(algo.Weights,Is.Not.Null);
                else
                    Assert.That(eWeights,Is.SameAs(algo.Weights));
                CollectionAssert.IsEmpty(algo.GetDistances());
                Assert.That(DistanceRelaxers.ShortestDistance,Is.SameAs(algo.DistanceRelaxer));
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();

            Func<EquatableEdge<int>, double> Weights = _ => 1.0;

            Assert.Throws<ArgumentNullException>(
                () => new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(null, Weights));
            Assert.Throws<ArgumentNullException>(
                () => new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph, null));
            Assert.Throws<ArgumentNullException>(
                () => new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            var algorithm = new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph, _ => 1.0);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            var algorithm = new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph, _ => 1.0);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, EquatableEdge<TestVertex>>();
            var algorithm = new TSP<TestVertex, EquatableEdge<TestVertex>, BidirectionalGraph<TestVertex, EquatableEdge<TestVertex>>>(graph, _ => 1.0);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            var algorithm = new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph, _ => 1.0);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            ComputeWithoutRoot_NoThrows_Test(
                graph,
                () => new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph, _ => 1.0));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            graph.AddVertex(0);
            var algorithm = new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph, _ => 1.0);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new BidirectionalGraph<TestVertex, EquatableEdge<TestVertex>>();
            ComputeWithRoot_Throws_Test(
                () => new TSP<TestVertex, EquatableEdge<TestVertex>, BidirectionalGraph<TestVertex, EquatableEdge<TestVertex>>>(graph, _ => 1.0));
        }

        #endregion

        #region Shortest path algorithm

        [Test]
        public void TryGetDistance_Throws()
        {
            // Algorithm don't use the Distances
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            graph.AddVertex(1);
            var algorithm = new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph, _ => 1.0);
            algorithm.Compute(1);
            Assert.That(algorithm.TryGetDistance(1, out double _),Is.False);

            var graph2 = new BidirectionalGraph<TestVertex, EquatableEdge<TestVertex>>();
            var algorithm2 = new TSP<TestVertex, EquatableEdge<TestVertex>, BidirectionalGraph<TestVertex, EquatableEdge<TestVertex>>>(graph2, _ => 1.0);
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm2.TryGetDistance(null, out _));

            var vertex = new TestVertex();
            Assert.Throws<InvalidOperationException>(() => algorithm2.TryGetDistance(vertex, out _));
        }

        #endregion

        [Test]
        public void GetVertexColor_Throws()
        {
            // Algorithm don't use the VerticesColors
            var graph = new BidirectionalGraph<int, EquatableEdge<int>>();
            graph.AddVerticesAndEdge(new EquatableEdge<int>(1, 2));
            var algorithm = new TSP<int, EquatableEdge<int>, BidirectionalGraph<int, EquatableEdge<int>>>(graph, _ => 1.0);
            algorithm.Compute(1);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => algorithm.GetVertexColor(1));
        }

        [Test]
        public void UndirectedFullGraph()
        {
            var testCase = new TestCase();
            testCase
                .AddVertex("n1")
                .AddVertex("n2")
                .AddVertex("n3")
                .AddVertex("n4")
                .AddVertex("n5");

            testCase
                .AddUndirectedEdge("n1", "n2", 16)
                .AddUndirectedEdge("n1", "n3", 9)
                .AddUndirectedEdge("n1", "n4", 15)
                .AddUndirectedEdge("n1", "n5", 3)
                .AddUndirectedEdge("n2", "n3", 14)
                .AddUndirectedEdge("n2", "n4", 4)
                .AddUndirectedEdge("n2", "n5", 5)
                .AddUndirectedEdge("n3", "n4", 4)
                .AddUndirectedEdge("n3", "n5", 2)
                .AddUndirectedEdge("n4", "n5", 1);

            var tsp = new TSP<string, EquatableEdge<string>, BidirectionalGraph<string, EquatableEdge<string>>>(
                testCase.Graph, testCase.GetWeightsFunc());
            tsp.Compute();

            Assert.That(25,Is.EqualTo(tsp.BestCost));
            Assert.That(tsp.ResultPath,Is.Not.Null);
            Assert.That(tsp.ResultPath.IsDirectedAcyclicGraph(),Is.False);
        }

        [Test]
        public void UndirectedSparseGraph()
        {
            var testCase = new TestCase();
            testCase
                .AddVertex("n1")
                .AddVertex("n2")
                .AddVertex("n3")
                .AddVertex("n4")
                .AddVertex("n5")
                .AddVertex("n6");

            testCase
                .AddUndirectedEdge("n1", "n2", 10)
                .AddUndirectedEdge("n2", "n3", 8)
                .AddUndirectedEdge("n3", "n4", 11)
                .AddUndirectedEdge("n4", "n5", 6)
                .AddUndirectedEdge("n5", "n6", 9)
                .AddUndirectedEdge("n1", "n6", 3)
                .AddUndirectedEdge("n2", "n6", 5)
                .AddUndirectedEdge("n3", "n6", 18)
                .AddUndirectedEdge("n3", "n5", 21);

            var tsp = new TSP<string, EquatableEdge<string>, BidirectionalGraph<string, EquatableEdge<string>>>(
                testCase.Graph, testCase.GetWeightsFunc());
            tsp.Compute();

            Assert.That(47,Is.EqualTo(tsp.BestCost));
            Assert.That(tsp.ResultPath,Is.Not.Null);
            Assert.That(tsp.ResultPath.IsDirectedAcyclicGraph(),Is.False);
        }

        [Test]
        public void DirectedSparseGraphWithoutPath()
        {
            var testCase = new TestCase();
            testCase
                .AddVertex("n1")
                .AddVertex("n2")
                .AddVertex("n3")
                .AddVertex("n4")
                .AddVertex("n5")
                .AddVertex("n6");

            testCase
                .AddDirectedEdge("n1", "n2", 10)
                .AddDirectedEdge("n2", "n3", 8)
                .AddDirectedEdge("n3", "n4", 11)
                .AddDirectedEdge("n4", "n5", 6)
                .AddDirectedEdge("n5", "n6", 9)
                .AddDirectedEdge("n1", "n6", 3)
                .AddDirectedEdge("n2", "n6", 5)
                .AddDirectedEdge("n3", "n6", 18)
                .AddDirectedEdge("n3", "n5", 21);

            var tsp = new TSP<string, EquatableEdge<string>, BidirectionalGraph<string, EquatableEdge<string>>>(
                testCase.Graph, testCase.GetWeightsFunc());
            tsp.Compute();

            Assert.That(double.PositiveInfinity,Is.EqualTo(tsp.BestCost));
            Assert.That(tsp.ResultPath,Is.Null);
        }

        [Test]
        public void DirectedSparseGraph()
        {
            var testCase = new TestCase();
            testCase
                .AddVertex("n1")
                .AddVertex("n2")
                .AddVertex("n3")
                .AddVertex("n4")
                .AddVertex("n5")
                .AddVertex("n6");

            testCase
                .AddDirectedEdge("n1", "n2", 10)
                .AddDirectedEdge("n2", "n3", 8)
                .AddDirectedEdge("n3", "n4", 11)
                .AddDirectedEdge("n4", "n5", 6)
                .AddDirectedEdge("n5", "n6", 9)
                .AddDirectedEdge("n1", "n6", 3)
                .AddDirectedEdge("n2", "n6", 5)
                .AddDirectedEdge("n3", "n6", 18)
                .AddDirectedEdge("n3", "n5", 21)
                .AddDirectedEdge("n6", "n1", 1);

            var tsp = new TSP<string, EquatableEdge<string>, BidirectionalGraph<string, EquatableEdge<string>>>(
                testCase.Graph, testCase.GetWeightsFunc());
            tsp.Compute();

            Assert.That(45,Is.EqualTo(tsp.BestCost));
            Assert.That(tsp.ResultPath,Is.Not.Null);
            Assert.That(tsp.ResultPath.IsDirectedAcyclicGraph(),Is.False);
        }

        [Pure]

        public static TSP<T, EquatableEdge<T>, BidirectionalGraph<T, EquatableEdge<T>>> CreateAlgorithmAndMaybeDoComputation<T>(
             ContractScenario<T> scenario)
        {
            var graph = new BidirectionalGraph<T, EquatableEdge<T>>();
            graph.AddVerticesAndEdgeRange(scenario.EdgesInGraph.Select(e => new EquatableEdge<T>(e.Source, e.Target)));
            graph.AddVertexRange(scenario.SingleVerticesInGraph);

            double Weights(Edge<T> e) => 1.0;
            var algorithm = new TSP<T, EquatableEdge<T>, BidirectionalGraph<T, EquatableEdge<T>>>(graph, Weights);

            if (scenario.DoComputation)
                algorithm.Compute(scenario.Root);
            return algorithm;
        }
    }
}