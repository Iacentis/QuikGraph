using NUnit.Framework;
using QuikGraph.Algorithms;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Test helpers for algorithms.
    /// </summary>
    internal static class AlgorithmTestHelpers
    {
        #region Test helpers

        public static void AssertAlgorithmState<TGraph>(
             AlgorithmBase<TGraph> algorithm,
             TGraph treatedGraph,
            ComputationState state = ComputationState.NotRunning)
        {
            Assert.That(treatedGraph,Is.Not.Null);
            Assert.That(treatedGraph,Is.SameAs(algorithm.VisitedGraph));
            Assert.That(algorithm.Services,Is.Not.Null);
            Assert.That(algorithm.SyncRoot,Is.Not.Null);
            Assert.That(state,Is.EqualTo(algorithm.State));
        }

        #endregion
    }
}