using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Predicates;

namespace QuikGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="SinkVertexPredicate{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class SinkVertexPredicateTests
    {
        [Test]
        public void Construction()
        {
            Assert.DoesNotThrow(
                // ReSharper disable once ObjectCreationAsStatement
                () => new SinkVertexPredicate<int, Edge<int>>(
                    new AdjacencyGraph<int, Edge<int>>()));
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SinkVertexPredicate<int, Edge<int>>(null));
        }


        private static IEnumerable<TestCaseData> PredicateTestCases
        {

            get
            {
                yield return new TestCaseData(new AdjacencyGraph<int, Edge<int>>());
                yield return new TestCaseData(new BidirectionalGraph<int, Edge<int>>());
            }
        }

        [TestCaseSource(nameof(PredicateTestCases))]
        public void Predicate<TGraph>( TGraph graph)
            where TGraph
            : IIncidenceGraph<int, Edge<int>>
            , IMutableVertexSet<int>
            , IMutableEdgeListGraph<int, Edge<int>>
        {
            var predicate = new SinkVertexPredicate<int, Edge<int>>(graph);

            graph.AddVertex(1);
            graph.AddVertex(2);
            Assert.That(predicate.Test(1),Is.True);
            Assert.That(predicate.Test(2),Is.True);

            graph.AddVertex(3);
            graph.AddEdge(new Edge<int>(1, 3));
            Assert.That(predicate.Test(1),Is.False);
            Assert.That(predicate.Test(2),Is.True);
            Assert.That(predicate.Test(3),Is.True);

            graph.AddEdge(new Edge<int>(1, 2));
            Assert.That(predicate.Test(1),Is.False);
            Assert.That(predicate.Test(2),Is.True);
            Assert.That(predicate.Test(3),Is.True);

            var edge23 = new Edge<int>(2, 3);
            graph.AddEdge(edge23);
            Assert.That(predicate.Test(1),Is.False);
            Assert.That(predicate.Test(2),Is.False);
            Assert.That(predicate.Test(3),Is.True);

            graph.RemoveEdge(edge23);
            Assert.That(predicate.Test(1),Is.False);
            Assert.That(predicate.Test(2),Is.True);
            Assert.That(predicate.Test(3),Is.True);
        }

        [Test]
        public void Predicate_Throws()
        {
            var graph = new AdjacencyGraph<TestVertex, Edge<TestVertex>>();
            var predicate = new SinkVertexPredicate<TestVertex, Edge<TestVertex>>(graph);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => predicate.Test(new TestVertex("1")));
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => predicate.Test(null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}