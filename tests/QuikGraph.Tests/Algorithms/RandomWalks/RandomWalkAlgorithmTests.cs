using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.RandomWalks;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.RandomWalks
{
    /// <summary>
    /// Tests for <see cref="RandomWalkAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class RandomWalkAlgorithmTests : RootedAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunRandomWalkAndCheck<TVertex, TEdge>(
            IVertexListGraph<TVertex, TEdge> graph)
            where TEdge : IEdge<TVertex>
        {
            if (graph.VertexCount == 0)
                return;

            foreach (TVertex root in graph.Vertices)
            {
                RandomWalkAlgorithm<TVertex, TEdge> walker1 = CreateAlgorithm();
                bool calledStart1 = false;
                bool calledEnd1 = false;
                var encounteredEdges1 = new List<TEdge>();
                walker1.StartVertex += vertex =>
                {
                    Assert.That(calledStart1, Is.False);
                    calledStart1 = true;
                    Assert.That(root, Is.EqualTo(vertex));
                };
                walker1.TreeEdge += edge =>
                {
                    Assert.That(edge, Is.Not.Null);
                    encounteredEdges1.Add(edge);
                };
                walker1.EndVertex += vertex =>
                {
                    Assert.That(calledEnd1, Is.False);
                    calledEnd1 = true;
                    Assert.That(vertex, Is.Not.Null);
                };

                RandomWalkAlgorithm<TVertex, TEdge> walker2 = CreateAlgorithm();
                bool calledStart2 = false;
                bool calledEnd2 = false;
                var encounteredEdges2 = new List<TEdge>();
                walker2.StartVertex += vertex =>
                {
                    Assert.That(calledStart2, Is.False);
                    calledStart2 = true;
                    Assert.That(root, Is.EqualTo(vertex));
                };
                walker2.TreeEdge += edge =>
                {
                    Assert.That(edge, Is.Not.Null);
                    encounteredEdges2.Add(edge);
                };
                walker2.EndVertex += vertex =>
                {
                    Assert.That(calledEnd2, Is.False);
                    calledEnd2 = true;
                    Assert.That(vertex, Is.Not.Null);
                };

                RandomWalkAlgorithm<TVertex, TEdge> walker3 = CreateAlgorithm();
                bool calledStart3 = false;
                bool calledEnd3 = false;
                var encounteredEdges3 = new List<TEdge>();
                walker3.StartVertex += vertex =>
                {
                    Assert.That(calledStart3, Is.False);
                    calledStart3 = true;
                    Assert.That(root, Is.EqualTo(vertex));
                };
                walker3.TreeEdge += edge =>
                {
                    Assert.That(edge, Is.Not.Null);
                    encounteredEdges3.Add(edge);
                };
                walker3.EndVertex += vertex =>
                {
                    Assert.That(calledEnd3, Is.False);
                    calledEnd3 = true;
                    Assert.That(vertex, Is.Not.Null);
                };

                var vis1 = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis1.Attach(walker1))
                    walker1.Generate(root);
                Assert.That(calledStart1, Is.True);
                Assert.That(calledEnd1, Is.True);

                walker2.SetRootVertex(root);
                var vis2 = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis2.Attach(walker2))
                    walker2.Compute();
                Assert.That(calledStart2, Is.True);
                Assert.That(calledEnd2, Is.True);

                var vis3 = new EdgeRecorderObserver<TVertex, TEdge>();
                using (vis3.Attach(walker3))
                    walker3.Generate(root, 100);
                Assert.That(calledStart3, Is.True);
                Assert.That(calledEnd3, Is.True);

                CollectionAssert.AreEqual(vis1.Edges, encounteredEdges1);
                CollectionAssert.AreEqual(vis1.Edges, encounteredEdges2);
                CollectionAssert.AreEqual(vis1.Edges, encounteredEdges3);
                CollectionAssert.AreEqual(vis1.Edges, vis2.Edges);
                CollectionAssert.AreEqual(vis1.Edges, vis3.Edges);
            }

            #region Local function

            RandomWalkAlgorithm<TVertex, TEdge> CreateAlgorithm()
            {
                var walker = new RandomWalkAlgorithm<TVertex, TEdge>(graph)
                {
                    EdgeChain = new NormalizedMarkovEdgeChain<TVertex, TEdge> { Rand = new Random(123456) }
                };

                return walker;
            }

            #endregion
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var chain = new WeightedMarkovEdgeChain<int, Edge<int>>(new Dictionary<Edge<int>, double>());
            EdgePredicate<int, Edge<int>> predicate = _ => true;
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);
            AssertAlgorithmProperties(algorithm, graph);

            algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph, chain);
            AssertAlgorithmProperties(algorithm, graph, chain);

            algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph) { EndPredicate = predicate };
            AssertAlgorithmProperties(algorithm, graph, p: predicate);

            algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph) { EdgeChain = chain };
            AssertAlgorithmProperties(algorithm, graph, chain);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                RandomWalkAlgorithm<TVertex, TEdge> algo,
                IVertexListGraph<TVertex, TEdge> g,
                IEdgeChain<TVertex, TEdge> c = null,
                EdgePredicate<TVertex, TEdge> p = null)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                if (c is null)
                    Assert.That(algo.EdgeChain, Is.Not.Null);
                else
                    Assert.That(c, Is.SameAs(algo.EdgeChain));
                Assert.That(p, Is.EqualTo(algo.EndPredicate));
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var chain = new WeightedMarkovEdgeChain<int, Edge<int>>(new Dictionary<Edge<int>, double>());

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new RandomWalkAlgorithm<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(() => new RandomWalkAlgorithm<int, Edge<int>>(graph, null));
            Assert.Throws<ArgumentNullException>(() => new RandomWalkAlgorithm<int, Edge<int>>(null, chain));
            Assert.Throws<ArgumentNullException>(() => new RandomWalkAlgorithm<int, Edge<int>>(null, null));

            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph, chain);
            Assert.Throws<ArgumentNullException>(() => algorithm.EdgeChain = null);
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Rooted algorithm

        [Test]
        public void TryGetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);
            TryGetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);
            SetRootVertex_Test(algorithm);
        }

        [Test]
        public void SetRootVertex_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var algorithm = new RandomWalkAlgorithm<TestVertex, Edge<TestVertex>>(graph);
            SetRootVertex_Throws_Test(algorithm);
        }

        [Test]
        public void ClearRootVertex()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);
            ClearRootVertex_Test(algorithm);
        }

        [Test]
        public void ComputeWithoutRoot_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            ComputeWithoutRoot_Throws_Test(() => new RandomWalkAlgorithm<int, Edge<int>>(graph));
        }

        [Test]
        public void ComputeWithRoot()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVertex(0);
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);
            ComputeWithRoot_Test(algorithm);
        }

        [Test]
        public void ComputeWithRoot_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            ComputeWithRoot_Throws_Test(() => new RandomWalkAlgorithm<TestVertex, Edge<TestVertex>>(graph));
        }

        #endregion

        [Test]
        public void RandomWalk()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
                RunRandomWalkAndCheck(graph);
        }

        [Test]
        public void RandomWalkWithPredicate()
        {
            var edge1 = new Edge<int>(1, 2);
            var edge2 = new Edge<int>(1, 3);
            var edge3 = new Edge<int>(2, 3);
            var edge4 = new Edge<int>(3, 4);
            var edge5 = new Edge<int>(4, 5);
            var edge6 = new Edge<int>(5, 4);
            var graph = new AdjacencyGraph<int, Edge<int>>();
            graph.AddVerticesAndEdgeRange([
                edge1, edge2, edge3, edge4, edge5, edge6
            ]);
            var chain = new NormalizedMarkovEdgeChain<int, Edge<int>> { Rand = new Random(123456) };

            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph, chain)
            {
                EndPredicate = edge => edge == edge4
            };

            var encounteredEdges = new List<Edge<int>>();
            algorithm.TreeEdge += edge => encounteredEdges.Add(edge);
            algorithm.EndVertex += vertex => Assert.That(3, Is.EqualTo(vertex));

            algorithm.Generate(1, int.MaxValue);

            CollectionAssert.IsNotEmpty(encounteredEdges);
            Assert.That(3, Is.EqualTo(encounteredEdges.Last().Target));
            Assert.That(
                edge2 == encounteredEdges.Last()
                ||
                edge3 == encounteredEdges.Last(),
                Is.True);
        }

        [Test]
        public void RandomWalk_Throws()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            var algorithm = new RandomWalkAlgorithm<int, Edge<int>>(graph);

            Assert.Throws<VertexNotFoundException>(() => algorithm.Generate(1));
            Assert.Throws<VertexNotFoundException>(() => algorithm.Generate(1, 12));
        }
    }
}