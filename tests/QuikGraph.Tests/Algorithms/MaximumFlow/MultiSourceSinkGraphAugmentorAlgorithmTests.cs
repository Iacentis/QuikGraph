using System;
using System.Linq;

using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Algorithms.MaximumFlow;
using static QuikGraph.Tests.Algorithms.AlgorithmTestHelpers;

namespace QuikGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Tests for <see cref="MultiSourceSinkGraphAugmentorAlgorithm{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class MultiSourceSinkGraphAugmentorAlgorithmTests : GraphAugmentorAlgorithmTestsBase
    {
        #region Test helpers

        private static void RunAugmentationAndCheck(
             IMutableBidirectionalGraph<string, Edge<string>> graph)
        {
            int vertexCount = graph.VertexCount;
            int edgeCount = graph.EdgeCount;
            int vertexId = graph.VertexCount + 1;

            string[] noInEdgesVertices = graph.Vertices.Where(graph.IsInEdgesEmpty).ToArray();
            string[] noOutEdgesVertices = graph.Vertices.Where(graph.IsOutEdgesEmpty).ToArray();

            using (var augmentor = new MultiSourceSinkGraphAugmentorAlgorithm<string, Edge<string>>(
                graph,
                () => (vertexId++).ToString(),
                (s, t) => new Edge<string>(s, t)))
            {
                bool added = false;
                augmentor.EdgeAdded += _ => { added = true; };

                augmentor.Compute();
                Assert.That(added,Is.True);
                VerifyVertexCount(graph, augmentor, vertexCount);
                VerifySourceConnector(graph, augmentor, noInEdgesVertices);
                VerifySinkConnector(graph, augmentor, noOutEdgesVertices);
            }

            Assert.That(graph.VertexCount,Is.EqualTo(vertexCount));
            Assert.That(graph.EdgeCount,Is.EqualTo(edgeCount));
        }

        private static void VerifyVertexCount<TVertex, TEdge>(
             IVertexSet<TVertex> graph,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
             MultiSourceSinkGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
            int vertexCount)
            where TEdge : IEdge<TVertex>
        {
            Assert.That(vertexCount + 2 /* Source + Sink */,Is.EqualTo(graph.VertexCount));
            Assert.That(graph.ContainsVertex(augmentor.SuperSource),Is.True);
            Assert.That(graph.ContainsVertex(augmentor.SuperSink),Is.True);
        }

        private static void VerifySourceConnector<TVertex, TEdge>(
             IVertexListGraph<TVertex, TEdge> graph,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
             MultiSourceSinkGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
             TVertex[] noInEdgesVertices)
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in noInEdgesVertices)
            {
                Assert.That(graph.ContainsEdge(augmentor.SuperSource, vertex),Is.True);
            }

            foreach (TVertex vertex in graph.Vertices.Except(noInEdgesVertices))
            {
                if (vertex.Equals(augmentor.SuperSource))
                    continue;
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                Assert.That(graph.ContainsEdge(augmentor.SuperSource, vertex),Is.False);
            }
        }

        private static void VerifySinkConnector<TVertex, TEdge>(
             IVertexListGraph<TVertex, TEdge> graph,
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
             MultiSourceSinkGraphAugmentorAlgorithm<TVertex, TEdge> augmentor,
             TVertex[] noOutEdgesVertices)
            where TEdge : IEdge<TVertex>
        {
            foreach (TVertex vertex in noOutEdgesVertices)
            {
                Assert.That(graph.ContainsEdge(vertex, augmentor.SuperSink),Is.True);
            }

            foreach (TVertex vertex in graph.Vertices.Except(noOutEdgesVertices))
            {
                if (vertex.Equals(augmentor.SuperSource))
                    continue;
                if (vertex.Equals(augmentor.SuperSink))
                    continue;
                Assert.That(graph.ContainsEdge(vertex, augmentor.SuperSink),Is.False);
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            var algorithm = new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);
            AssertAlgorithmProperties(algorithm, graph, vertexFactory, edgeFactory);

            algorithm = new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, vertexFactory, edgeFactory);
            AssertAlgorithmProperties(algorithm, graph, vertexFactory, edgeFactory);

            #region Local function

            void AssertAlgorithmProperties<TVertex, TEdge>(
                MultiSourceSinkGraphAugmentorAlgorithm<TVertex, TEdge> algo,
                IMutableBidirectionalGraph<TVertex, TEdge> g,
                VertexFactory<int> vFactory,
                EdgeFactory<int, Edge<int>> eFactory)
                where TEdge : IEdge<TVertex>
            {
                AssertAlgorithmState(algo, g);
                Assert.That(algo.Augmented,Is.False);
                CollectionAssert.IsEmpty(algo.AugmentedEdges);
                Assert.That(vFactory,Is.SameAs(algo.VertexFactory));
                Assert.That(eFactory,Is.SameAs(algo.EdgeFactory));
                Assert.That(default(TVertex),Is.EqualTo(algo.SuperSource));
                Assert.That(default(TVertex),Is.EqualTo(algo.SuperSink));
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            VertexFactory<int> vertexFactory = () => 1;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null));

            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, null, vertexFactory, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, edgeFactory));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, null, vertexFactory, null));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, graph, null, null));
            Assert.Throws<ArgumentNullException>(
                () => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(null, null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        #region Graph augmentor

        [Test]
        public void CreateAndSetSuperSource()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var algorithm = new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);

            CreateAndSetSuperSource_Test(algorithm);
        }

        [Test]
        public void CreateAndSetSuperSink()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var algorithm = new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);

            CreateAndSetSuperSink_Test(algorithm);
        }

        [Test]
        public void RunAugmentation()
        {
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);

            RunAugmentation_Test(
                graph => new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory));
        }

        [Test]
        public void RunAugmentation_Throws()
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            int vertexID = 0;
            VertexFactory<int> vertexFactory = () => ++vertexID;
            EdgeFactory<int, Edge<int>> edgeFactory = (source, target) => new Edge<int>(source, target);
            var algorithm = new MultiSourceSinkGraphAugmentorAlgorithm<int, Edge<int>>(graph, vertexFactory, edgeFactory);

            RunAugmentation_Throws_Test(algorithm);
        }

        #endregion

        [Test]
        public void MultiSourceSinkGraphAugmentor()
        {
            foreach (BidirectionalGraph<string, Edge<string>> graph in TestGraphFactory.GetBidirectionalGraphs_All())
                RunAugmentationAndCheck(graph);
        }
    }
}