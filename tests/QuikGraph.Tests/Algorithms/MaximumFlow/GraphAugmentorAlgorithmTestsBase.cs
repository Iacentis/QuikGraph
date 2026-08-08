using System;
using NUnit.Framework;
using QuikGraph.Algorithms.MaximumFlow;

namespace QuikGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// Base class for graph augmentor algorithms.
    /// </summary>
    internal abstract class GraphAugmentorAlgorithmTestsBase
    {
        protected static void CreateAndSetSuperSource_Test<TGraph>(
             GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            bool added = false;
            const int superSource = 1;
            algorithm.SuperSourceAdded += vertex =>
            {
                added = true;
                Assert.That(superSource,Is.EqualTo(vertex));
            };

            algorithm.Compute();
            Assert.That(added,Is.True);
            Assert.That(superSource,Is.EqualTo(algorithm.SuperSource));
        }

        protected static void CreateAndSetSuperSink_Test<TGraph>(
             GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            bool added = false;
            const int superSink = 2;
            algorithm.SuperSinkAdded += vertex =>
            {
                added = true;
                Assert.That(superSink,Is.EqualTo(vertex));
            };

            algorithm.Compute();
            Assert.That(added,Is.True);
            Assert.That(superSink,Is.EqualTo(algorithm.SuperSink));
        }

        protected static void RunAugmentation_Test<TGraph>(

            Func<
                IMutableVertexAndEdgeSet<int, Edge<int>>,
                GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph>
            > createAlgorithm,
             Action<IMutableVertexAndEdgeSet<int, Edge<int>>> setupGraph = null)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            int vertexCount = graph.VertexCount;
            // Single run
            GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm = createAlgorithm(graph);
            Assert.That(algorithm.Augmented,Is.False);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            algorithm.Compute();

            Assert.That(algorithm.Augmented,Is.True);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount + 2,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            // Multiple runs
            graph = new AdjacencyGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            algorithm = createAlgorithm(graph);
            Assert.That(algorithm.Augmented,Is.False);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            algorithm.Compute();

            Assert.That(algorithm.Augmented,Is.True);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount + 2,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            algorithm.Rollback();

            Assert.That(algorithm.Augmented,Is.False);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            algorithm.Compute();

            Assert.That(algorithm.Augmented,Is.True);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount + 2,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            // Disposed algorithm
            graph = new AdjacencyGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            using (algorithm = createAlgorithm(graph))
            {
                Assert.That(algorithm.Augmented,Is.False);
                Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
                Assert.That(vertexCount,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

                algorithm.Compute();

                Assert.That(algorithm.Augmented,Is.True);
                Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
                Assert.That(vertexCount + 2,Is.EqualTo(algorithm.VisitedGraph.VertexCount));
            }
            Assert.That(vertexCount,Is.EqualTo(graph.VertexCount));
        }

        protected static void RunAugmentation_Test<TGraph>(

            Func<
                IMutableBidirectionalGraph<int, Edge<int>>,
                GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph>
            > createAlgorithm,
             Action<IMutableBidirectionalGraph<int, Edge<int>>> setupGraph = null)
            where TGraph : IMutableBidirectionalGraph<int, Edge<int>>
        {
            var graph = new BidirectionalGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            int vertexCount = graph.VertexCount;
            // Single run
            GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm = createAlgorithm(graph);
            Assert.That(algorithm.Augmented,Is.False);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            algorithm.Compute();

            Assert.That(algorithm.Augmented,Is.True);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount + 2,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            // Multiple runs
            graph = new BidirectionalGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            algorithm = createAlgorithm(graph);
            Assert.That(algorithm.Augmented,Is.False);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            algorithm.Compute();

            Assert.That(algorithm.Augmented,Is.True);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount + 2,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            algorithm.Rollback();

            Assert.That(algorithm.Augmented,Is.False);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            algorithm.Compute();

            Assert.That(algorithm.Augmented,Is.True);
            Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
            Assert.That(vertexCount + 2,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

            // Disposed algorithm
            graph = new BidirectionalGraph<int, Edge<int>>();
            setupGraph?.Invoke(graph);
            using (algorithm = createAlgorithm(graph))
            {
                Assert.That(algorithm.Augmented,Is.False);
                Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
                Assert.That(vertexCount,Is.EqualTo(algorithm.VisitedGraph.VertexCount));

                algorithm.Compute();

                Assert.That(algorithm.Augmented,Is.True);
                Assert.That(algorithm.AugmentedEdges,Is.Not.Null);
                Assert.That(vertexCount + 2,Is.EqualTo(algorithm.VisitedGraph.VertexCount));
            }
            Assert.That(vertexCount,Is.EqualTo(graph.VertexCount));
        }

        protected static void RunAugmentation_Throws_Test<TGraph>(
             GraphAugmentorAlgorithmBase<int, Edge<int>, TGraph> algorithm)
            where TGraph : IMutableVertexAndEdgeSet<int, Edge<int>>
        {
            // Multiple runs without clean
            Assert.DoesNotThrow(algorithm.Compute);
            Assert.Throws<InvalidOperationException>(algorithm.Compute);
        }
    }
}