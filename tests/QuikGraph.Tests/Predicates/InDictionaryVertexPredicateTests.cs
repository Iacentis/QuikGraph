using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Predicates;

namespace QuikGraph.Tests.Predicates
{
    /// <summary>
    /// Tests for <see cref="InDictionaryVertexPredicate{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class InDictionaryVertexPredicateTests
    {
        [Test]
        public void Construction()
        {
            Assert.DoesNotThrow(
                // ReSharper disable once ObjectCreationAsStatement
                () => new InDictionaryVertexPredicate<int, Edge<int>>(
                    new Dictionary<int, Edge<int>>()));
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new InDictionaryVertexPredicate<int, double>(null));
        }

        [Test]
        public void Predicate()
        {
            var vertexMap = new Dictionary<int, double>();
            var predicate = new InDictionaryVertexPredicate<int, double>(vertexMap);

            Assert.That(predicate.Test(1),Is.False);
            Assert.That(predicate.Test(2),Is.False);

            vertexMap.Add(2, 12);
            Assert.That(predicate.Test(1),Is.False);
            Assert.That(predicate.Test(2),Is.True);

            vertexMap.Add(1, 42);
            Assert.That(predicate.Test(1),Is.True);
            Assert.That(predicate.Test(2),Is.True);

            vertexMap.Remove(2);
            Assert.That(predicate.Test(1),Is.True);
            Assert.That(predicate.Test(2),Is.False);

            vertexMap.Remove(3);
            Assert.That(predicate.Test(1),Is.True);
            Assert.That(predicate.Test(2),Is.False);

            vertexMap.Remove(1);
            Assert.That(predicate.Test(1),Is.False);
            Assert.That(predicate.Test(2),Is.False);
        }

        [Test]
        public void Predicate_Throws()
        {
            var predicate = new InDictionaryVertexPredicate<TestVertex, Edge<TestVertex>>(
                new Dictionary<TestVertex, Edge<TestVertex>>());

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => predicate.Test(null));
        }
    }
}