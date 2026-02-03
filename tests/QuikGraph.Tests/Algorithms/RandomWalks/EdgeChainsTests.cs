using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using NUnit.Framework;
using QuikGraph.Algorithms.RandomWalks;

namespace QuikGraph.Tests.Algorithms.RandomWalks
{
    /// <summary>
    /// Tests for <see cref="RandomWalkAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class EdgeChainsTests
    {
        #region Test helpers

        [Pure]

        private static IVertexAndEdgeListGraph<int, EquatableEdge<int>> CreateGraph1()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
            graph.AddVerticesAndEdgeRange([
                new EquatableEdge<int>(1, 2),
                new EquatableEdge<int>(1, 3),
                new EquatableEdge<int>(2, 3),
                new EquatableEdge<int>(2, 5),
                new EquatableEdge<int>(4, 3),
                new EquatableEdge<int>(4, 5),
                new EquatableEdge<int>(4, 7),
                new EquatableEdge<int>(5, 6),
                new EquatableEdge<int>(6, 7),
                new EquatableEdge<int>(7, 4),
                new EquatableEdge<int>(8, 3)
            ]);

            return graph;
        }

        [Pure]

        private static IVertexAndEdgeListGraph<int, EquatableEdge<int>> CreateGraph2()
        {
            var graph = new AdjacencyGraph<int, EquatableEdge<int>>();
            graph.AddVerticesAndEdgeRange([
                new EquatableEdge<int>(1, 2),
                new EquatableEdge<int>(1, 3),
                new EquatableEdge<int>(2, 3),
                new EquatableEdge<int>(3, 4),
                new EquatableEdge<int>(4, 5),
                new EquatableEdge<int>(4, 7),
                new EquatableEdge<int>(5, 2),
                new EquatableEdge<int>(5, 6),
                new EquatableEdge<int>(6, 7),
                new EquatableEdge<int>(7, 4),
                new EquatableEdge<int>(8, 3)
            ]);

            return graph;
        }

        #endregion

        [Test]
        public void RoundRobinEdgeChain()
        {
            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph1 = CreateGraph1();

            var chain = new RoundRobinEdgeChain<int, EquatableEdge<int>>();
            Assert.That(chain.TryGetSuccessor(graph1, 1, out EquatableEdge<int> edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 2, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 3, out edge),Is.False);

            chain = new RoundRobinEdgeChain<int, EquatableEdge<int>>();
            EquatableEdge<int>[] edges = graph1.Edges.ToArray();
            Assert.That(chain.TryGetSuccessor(edges, 1, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 2, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 2, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 2, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(5,Is.EqualTo(edge.Target));
            // etc.
            Assert.That(chain.TryGetSuccessor([], 1, out _),Is.False);


            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph2 = CreateGraph2();

            chain = new RoundRobinEdgeChain<int, EquatableEdge<int>>();
            Assert.That(chain.TryGetSuccessor(graph2, 1, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 2, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 3, out edge),Is.True);
            Assert.That(3,Is.EqualTo(edge.Source));
            Assert.That(4,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 4, out edge),Is.True);
            Assert.That(4,Is.EqualTo(edge.Source));
            Assert.That(5,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 5, out edge),Is.True);
            Assert.That(5,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 2, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 3, out edge),Is.True);
            Assert.That(3,Is.EqualTo(edge.Source));
            Assert.That(4,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 4, out edge),Is.True);
            Assert.That(4,Is.EqualTo(edge.Source));
            Assert.That(7,Is.EqualTo(edge.Target));
            // Etc.

            chain = new RoundRobinEdgeChain<int, EquatableEdge<int>>();
            edges = graph2.Edges.ToArray();
            Assert.That(chain.TryGetSuccessor(edges, 1, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 2, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 2, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 2, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(3,Is.EqualTo(edge.Source));
            Assert.That(4,Is.EqualTo(edge.Target));
            // Etc.
        }

        [Test]
        public void NormalizedMarkovEdgeChain()
        {
            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph1 = CreateGraph1();

            var chain = new NormalizedMarkovEdgeChain<int, EquatableEdge<int>>
            {
                Rand = new Random(123456)
            };
            Assert.That(chain.TryGetSuccessor(graph1, 1, out EquatableEdge<int> edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 2, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 3, out edge),Is.False);

            chain = new NormalizedMarkovEdgeChain<int, EquatableEdge<int>>
            {
                Rand = new Random(123456)
            };
            EquatableEdge<int>[] edges = graph1.Edges.ToArray();
            Assert.That(chain.TryGetSuccessor(edges, 1, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            // etc.
            Assert.That(chain.TryGetSuccessor([], 1, out _),Is.False);


            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph2 = CreateGraph2();

            chain = new NormalizedMarkovEdgeChain<int, EquatableEdge<int>>
            {
                Rand = new Random(123456)
            };
            Assert.That(chain.TryGetSuccessor(graph2, 1, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 2, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 3, out edge),Is.True);
            Assert.That(3,Is.EqualTo(edge.Source));
            Assert.That(4,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 4, out edge),Is.True);
            Assert.That(4,Is.EqualTo(edge.Source));
            Assert.That(5,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 5, out edge),Is.True);
            Assert.That(5,Is.EqualTo(edge.Source));
            Assert.That(6,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 6, out edge),Is.True);
            Assert.That(6,Is.EqualTo(edge.Source));
            Assert.That(7,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 7, out edge),Is.True);
            Assert.That(7,Is.EqualTo(edge.Source));
            Assert.That(4,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 4, out edge),Is.True);
            // Etc.

            chain = new NormalizedMarkovEdgeChain<int, EquatableEdge<int>>
            {
                Rand = new Random(123456)
            };
            edges = graph2.Edges.ToArray();
            Assert.That(chain.TryGetSuccessor(edges, 1, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            // Etc.
        }

        [Test]
        public void Constructor_WeightedMarkovEdgeChains()
        {
            var weights = new Dictionary<Edge<int>, double>();
            var chain1 = new WeightedMarkovEdgeChain<int, Edge<int>>(weights);
            Assert.That(weights,Is.SameAs(chain1.Weights));

            var chain2 = new VanishingWeightedMarkovEdgeChain<int, Edge<int>>(weights);
            Assert.That(weights,Is.SameAs(chain2.Weights));

            chain2 = new VanishingWeightedMarkovEdgeChain<int, Edge<int>>(weights, 2.0);
            Assert.That(weights,Is.SameAs(chain2.Weights));
        }

        [Test]
        public void Constructor_WeightedMarkovEdgeChains_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new WeightedMarkovEdgeChain<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(
                () => new VanishingWeightedMarkovEdgeChain<int, Edge<int>>(null));
            Assert.Throws<ArgumentNullException>(
                () => new VanishingWeightedMarkovEdgeChain<int, Edge<int>>(null, 2.0));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void WeightedMarkovEdgeChain()
        {
            var weights = new Dictionary<EquatableEdge<int>, double>
            {
                [new EquatableEdge<int>(1, 2)] = 1.0,
                [new EquatableEdge<int>(1, 3)] = 2.0,
                [new EquatableEdge<int>(2, 3)] = -1.0,
                [new EquatableEdge<int>(2, 5)] = 10.0,
                [new EquatableEdge<int>(3, 4)] = 5.0,
                [new EquatableEdge<int>(4, 3)] = 25.0,
                [new EquatableEdge<int>(4, 5)] = 2.0,
                [new EquatableEdge<int>(4, 7)] = 1.0,
                [new EquatableEdge<int>(5, 2)] = 3.0,
                [new EquatableEdge<int>(5, 6)] = 1.0,
                [new EquatableEdge<int>(6, 7)] = 1.5,
                [new EquatableEdge<int>(7, 4)] = 0.0,
                [new EquatableEdge<int>(8, 3)] = 1.0
            };
            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph1 = CreateGraph1();

            var chain = new WeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            Assert.That(chain.TryGetSuccessor(graph1, 1, out EquatableEdge<int> edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 2, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(5,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 5, out edge),Is.True);
            Assert.That(5,Is.EqualTo(edge.Source));
            Assert.That(6,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 6, out edge),Is.True);
            Assert.That(6,Is.EqualTo(edge.Source));
            Assert.That(7,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 7, out edge),Is.True);
            Assert.That(7,Is.EqualTo(edge.Source));
            Assert.That(4,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 4, out edge),Is.True);
            Assert.That(4,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 3, out edge),Is.False);

            chain = new WeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            EquatableEdge<int>[] edges = graph1.Edges.ToArray();
            Assert.That(chain.TryGetSuccessor(edges, 1, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(5,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 5, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(5,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 5, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            // etc.
            Assert.That(chain.TryGetSuccessor([], 1, out _),Is.False);


            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph2 = CreateGraph2();

            chain = new WeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            Assert.That(chain.TryGetSuccessor(graph2, 1, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 2, out edge),Is.False);

            chain = new WeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            edges = graph2.Edges.ToArray();
            Assert.That(chain.TryGetSuccessor(edges, 1, out edge),Is.True);
            Assert.That(3,Is.EqualTo(edge.Source));
            Assert.That(4,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 4, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 1, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            // Etc.
        }

        [Test]
        public void VanishingWeightedMarkovEdgeChain()
        {
            var weights = new Dictionary<EquatableEdge<int>, double>
            {
                [new EquatableEdge<int>(1, 2)] = 1.0,
                [new EquatableEdge<int>(1, 3)] = 2.0,
                [new EquatableEdge<int>(2, 3)] = -1.0,
                [new EquatableEdge<int>(2, 5)] = 10.0,
                [new EquatableEdge<int>(3, 4)] = 5.0,
                [new EquatableEdge<int>(4, 3)] = 25.0,
                [new EquatableEdge<int>(4, 5)] = 2.0,
                [new EquatableEdge<int>(4, 7)] = 1.0,
                [new EquatableEdge<int>(5, 2)] = 3.0,
                [new EquatableEdge<int>(5, 6)] = 1.0,
                [new EquatableEdge<int>(6, 7)] = 1.5,
                [new EquatableEdge<int>(7, 4)] = 0.25,
                [new EquatableEdge<int>(8, 3)] = 1.0
            };
            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph1 = CreateGraph1();

            var chain = new VanishingWeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            Assert.That(chain.TryGetSuccessor(graph1, 1, out EquatableEdge<int> edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 2, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(5,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 5, out edge),Is.True);
            Assert.That(5,Is.EqualTo(edge.Source));
            Assert.That(6,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 6, out edge),Is.True);
            Assert.That(6,Is.EqualTo(edge.Source));
            Assert.That(7,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 7, out edge),Is.True);
            Assert.That(7,Is.EqualTo(edge.Source));
            Assert.That(4,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 4, out edge),Is.True);
            Assert.That(4,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph1, 3, out edge),Is.False);

            weights = new Dictionary<EquatableEdge<int>, double>
            {
                [new EquatableEdge<int>(1, 2)] = 1.0,
                [new EquatableEdge<int>(1, 3)] = 2.0,
                [new EquatableEdge<int>(2, 3)] = -1.0,
                [new EquatableEdge<int>(2, 5)] = 10.0,
                [new EquatableEdge<int>(3, 4)] = 5.0,
                [new EquatableEdge<int>(4, 3)] = 25.0,
                [new EquatableEdge<int>(4, 5)] = 2.0,
                [new EquatableEdge<int>(4, 7)] = 1.0,
                [new EquatableEdge<int>(5, 2)] = 3.0,
                [new EquatableEdge<int>(5, 6)] = 1.0,
                [new EquatableEdge<int>(6, 7)] = 1.5,
                [new EquatableEdge<int>(7, 4)] = 0.25,
                [new EquatableEdge<int>(8, 3)] = 1.0
            };
            chain = new VanishingWeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            EquatableEdge<int>[] edges = graph1.Edges.ToArray();
            Assert.That(chain.TryGetSuccessor(edges, 1, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(5,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 5, out edge),Is.True);
            Assert.That(2,Is.EqualTo(edge.Source));
            Assert.That(5,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 5, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            // etc.
            Assert.That(chain.TryGetSuccessor([], 1, out _),Is.False);


            IVertexAndEdgeListGraph<int, EquatableEdge<int>> graph2 = CreateGraph2();

            weights = new Dictionary<EquatableEdge<int>, double>
            {
                [new EquatableEdge<int>(1, 2)] = 1.0,
                [new EquatableEdge<int>(1, 3)] = 2.0,
                [new EquatableEdge<int>(2, 3)] = -1.0,
                [new EquatableEdge<int>(2, 5)] = 10.0,
                [new EquatableEdge<int>(3, 4)] = 5.0,
                [new EquatableEdge<int>(4, 3)] = 25.0,
                [new EquatableEdge<int>(4, 5)] = 2.0,
                [new EquatableEdge<int>(4, 7)] = 1.0,
                [new EquatableEdge<int>(5, 2)] = 3.0,
                [new EquatableEdge<int>(5, 6)] = 1.0,
                [new EquatableEdge<int>(6, 7)] = 1.5,
                [new EquatableEdge<int>(7, 4)] = 0.25,
                [new EquatableEdge<int>(8, 3)] = 1.0
            };
            chain = new VanishingWeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            Assert.That(chain.TryGetSuccessor(graph2, 1, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(graph2, 2, out edge),Is.False);

            weights = new Dictionary<EquatableEdge<int>, double>
            {
                [new EquatableEdge<int>(1, 2)] = 1.0,
                [new EquatableEdge<int>(1, 3)] = 2.0,
                [new EquatableEdge<int>(2, 3)] = -1.0,
                [new EquatableEdge<int>(2, 5)] = 10.0,
                [new EquatableEdge<int>(3, 4)] = 5.0,
                [new EquatableEdge<int>(4, 3)] = 25.0,
                [new EquatableEdge<int>(4, 5)] = 2.0,
                [new EquatableEdge<int>(4, 7)] = 1.0,
                [new EquatableEdge<int>(5, 2)] = 3.0,
                [new EquatableEdge<int>(5, 6)] = 1.0,
                [new EquatableEdge<int>(6, 7)] = 1.5,
                [new EquatableEdge<int>(7, 4)] = 0.25,
                [new EquatableEdge<int>(8, 3)] = 1.0
            };
            chain = new VanishingWeightedMarkovEdgeChain<int, EquatableEdge<int>>(weights)
            {
                Rand = new Random(123456)
            };
            edges = graph2.Edges.ToArray();
            Assert.That(chain.TryGetSuccessor(edges, 1, out edge),Is.True);
            Assert.That(3,Is.EqualTo(edge.Source));
            Assert.That(4,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 4, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(3,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 3, out edge),Is.True);
            Assert.That(1,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            Assert.That(chain.TryGetSuccessor(edges, 2, out edge),Is.True);
            Assert.That(5,Is.EqualTo(edge.Source));
            Assert.That(2,Is.EqualTo(edge.Target));
            // Etc.
        }
    }
}