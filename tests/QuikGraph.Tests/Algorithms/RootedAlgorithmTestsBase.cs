using System;
using NUnit.Framework;
using QuikGraph.Algorithms;
using static QuikGraph.Tests.AssertHelpers;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Base class for rooted algorithm tests.
    /// </summary>
    internal abstract class RootedAlgorithmTestsBase
    {
        #region Test helpers

        protected static void TryGetRootVertex_Test<TVertex, TGraph>(
             RootedAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            Assert.That(algorithm.TryGetRootVertex(out _),Is.False);

            var vertex = new TVertex();
            algorithm.SetRootVertex(vertex);
            Assert.That(algorithm.TryGetRootVertex(out TVertex root),Is.True);
            AssertEqual(vertex, root);
        }

        protected static void SetRootVertex_Test<TGraph>(
             RootedAlgorithmBase<int, TGraph> algorithm)
            where TGraph : IImplicitVertexSet<int>
        {
            int rootVertexChangeCount = 0;
            algorithm.RootVertexChanged += (_, _) => ++rootVertexChangeCount;

            const int vertex1 = 0;
            algorithm.SetRootVertex(vertex1);
            Assert.That(1,Is.EqualTo(rootVertexChangeCount));
            algorithm.TryGetRootVertex(out int root);
            Assert.That(vertex1,Is.EqualTo(root));

            // Not changed
            algorithm.SetRootVertex(vertex1);
            Assert.That(1,Is.EqualTo(rootVertexChangeCount));
            algorithm.TryGetRootVertex(out root);
            Assert.That(vertex1,Is.EqualTo(root));

            const int vertex2 = 1;
            algorithm.SetRootVertex(vertex2);
            Assert.That(2,Is.EqualTo(rootVertexChangeCount));
            algorithm.TryGetRootVertex(out root);
            Assert.That(vertex2,Is.EqualTo(root));

            algorithm.SetRootVertex(vertex1);
            Assert.That(3,Is.EqualTo(rootVertexChangeCount));
            algorithm.TryGetRootVertex(out root);
            Assert.That(vertex1,Is.EqualTo(root));
        }

        protected static void SetRootVertex_Throws_Test<TVertex, TGraph>(
             RootedAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : class
            where TGraph : IImplicitVertexSet<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm.SetRootVertex(null));
        }

        protected static void ClearRootVertex_Test<TVertex, TGraph>(
             RootedAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            int rootVertexChangeCount = 0;
            // ReSharper disable once AccessToModifiedClosure
            algorithm.RootVertexChanged += (_, _) => ++rootVertexChangeCount;

            algorithm.ClearRootVertex();
            Assert.That(0,Is.EqualTo(rootVertexChangeCount));

            var vertex = new TVertex();
            SetRootVertex(vertex);
            algorithm.ClearRootVertex();
            Assert.That(1,Is.EqualTo(rootVertexChangeCount));

            algorithm.ClearRootVertex();
            Assert.That(1,Is.EqualTo(rootVertexChangeCount));

            #region Local function

            void SetRootVertex(TVertex v)
            {
                algorithm.SetRootVertex(v);
                rootVertexChangeCount = 0;
            }

            #endregion
        }

        protected static void ComputeWithoutRoot_NoThrows_Test<TGraph>(
             IMutableVertexSet<int> graph,
             Func<RootedAlgorithmBase<int, TGraph>> createAlgorithm)
            where TGraph : IImplicitVertexSet<int>
        {
            RootedAlgorithmBase<int, TGraph> algorithm = createAlgorithm();
            Assert.DoesNotThrow(algorithm.Compute);

            graph.AddVertexRange([1, 2]);
            algorithm = createAlgorithm();
            Assert.DoesNotThrow(algorithm.Compute);
        }

        protected static void ComputeWithoutRoot_Throws_Test<TVertex, TGraph>(
             Func<RootedAlgorithmBase<TVertex, TGraph>> createAlgorithm)
            where TVertex : new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            RootedAlgorithmBase<TVertex, TGraph> algorithm = createAlgorithm();
            Assert.Throws<InvalidOperationException>(algorithm.Compute);

            // Source vertex set but not to a vertex in the graph
            algorithm = createAlgorithm();
            algorithm.SetRootVertex(new TVertex());
            Assert.Throws<VertexNotFoundException>(algorithm.Compute);
        }

        protected static void ComputeWithRoot_Test<TVertex, TGraph>(
             RootedAlgorithmBase<TVertex, TGraph> algorithm)
            where TVertex : new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            var vertex = new TVertex();
            Assert.DoesNotThrow(() => algorithm.Compute(vertex));
            Assert.That(algorithm.TryGetRootVertex(out TVertex root),Is.True);
            AssertEqual(vertex, root);
        }

        protected static void ComputeWithRoot_Throws_Test<TVertex, TGraph>(
             Func<RootedAlgorithmBase<TVertex, TGraph>> createAlgorithm)
            where TVertex : class, new()
            where TGraph : IImplicitVertexSet<TVertex>
        {
            RootedAlgorithmBase<TVertex, TGraph> algorithm = createAlgorithm();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm.Compute(null));
            Assert.That(algorithm.TryGetRootVertex(out _),Is.False);

            // Vertex not in the graph
            algorithm = createAlgorithm();
            Assert.Throws<ArgumentException>(() => algorithm.Compute(new TVertex()));
        }

        #endregion
    }
}