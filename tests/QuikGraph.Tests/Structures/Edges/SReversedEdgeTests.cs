using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="SReversedEdge{TVertex,TEdge}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class SReversedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2)), 2, 1);
            CheckEdge(new SReversedEdge<int, Edge<int>>(new Edge<int>(2, 1)), 1, 2);
            CheckEdge(new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 1)), 1, 1);

            // Struct break the contract with their implicit default constructor
            var defaultEdge = default(SReversedEdge<int, Edge<int>>);
            // ReSharper disable HeuristicUnreachableCode
            // Justification: Since struct has implicit default constructor it allows initialization of invalid edge
            Assert.That(defaultEdge.OriginalEdge,Is.Null);
            // ReSharper disable  HeuristicUnreachableCode
            Assert.Throws<NullReferenceException>(() => { int _ = defaultEdge.Source; });
            Assert.Throws<NullReferenceException>(() => { int _ = defaultEdge.Target; });
            // ReSharper restore HeuristicUnreachableCode

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new SReversedEdge<TestVertex, Edge<TestVertex>>(new Edge<TestVertex>(v1, v2)), v2, v1);
            CheckEdge(new SReversedEdge<TestVertex, Edge<TestVertex>>(new Edge<TestVertex>(v2, v1)), v1, v2);
            CheckEdge(new SReversedEdge<TestVertex, Edge<TestVertex>>(new Edge<TestVertex>(v1, v1)), v1, v1);

            // Struct break the contract with their implicit default constructor
            var defaultEdge2 = default(SReversedEdge<TestVertex, Edge<TestVertex>>);
            // ReSharper disable HeuristicUnreachableCode
            // Justification: Since struct has implicit default constructor it allows initialization of invalid edge
            Assert.That(defaultEdge2.OriginalEdge,Is.Null);
            // ReSharper disable  HeuristicUnreachableCode
            Assert.Throws<NullReferenceException>(() => { TestVertex _ = defaultEdge2.Source; });
            Assert.Throws<NullReferenceException>(() => { TestVertex _ = defaultEdge2.Target; });
            // ReSharper restore HeuristicUnreachableCode
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SReversedEdge<TestVertex, Edge<TestVertex>>(null));
        }

        [Test]
        public void Equals()
        {
            var wrappedEdge = new Edge<int>(1, 2);
            var edge1 = new SReversedEdge<int, Edge<int>>(wrappedEdge);
            var edge2 = new SReversedEdge<int, Edge<int>>(wrappedEdge);
            var edge3 = new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2));
            var edge4 = new SReversedEdge<int, Edge<int>>(new Edge<int>(2, 1));

            Assert.That(edge1,Is.EqualTo(edge1));

            Assert.That(edge1,Is.EqualTo(edge2));
            Assert.That(edge2,Is.EqualTo(edge1));
            Assert.That(edge1.Equals((object)edge2),Is.True);
            Assert.That(edge1.Equals(edge2),Is.True);
            Assert.That(edge2.Equals(edge1),Is.True);

            Assert.That(edge1,Is.Not.EqualTo(edge3));
            Assert.That(edge3,Is.Not.EqualTo(edge1));
            Assert.That(edge1.Equals((object)edge3),Is.False);
            Assert.That(edge1.Equals(edge3),Is.False);
            Assert.That(edge3.Equals(edge1),Is.False);

            Assert.That(edge1,Is.Not.EqualTo(edge4));
            Assert.That(edge4,Is.Not.EqualTo(edge1));
            Assert.That(edge1.Equals((object)edge4),Is.False);
            Assert.That(edge1.Equals(edge4),Is.False);
            Assert.That(edge4.Equals(edge1),Is.False);

            Assert.That(edge1,Is.Not.Null);
            Assert.That(edge1.Equals(null),Is.False);
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SReversedEdge<int, Edge<int>>);
            var edge2 = new SReversedEdge<int, Edge<int>>();

            Assert.That(edge1,Is.EqualTo(edge2));
            Assert.That(edge2,Is.EqualTo(edge1));
            Assert.That(edge1.Equals(edge2),Is.True);
            Assert.That(edge2.Equals(edge1),Is.True);
        }

        [Test]
        public void Equals2()
        {
            var edge1 = new SReversedEdge<int, EquatableEdge<int>>(new EquatableEdge<int>(1, 2));
            var edge2 = new SReversedEdge<int, EquatableEdge<int>>(new EquatableEdge<int>(1, 2));
            var edge3 = new SReversedEdge<int, EquatableEdge<int>>(new EquatableEdge<int>(2, 1));

            Assert.That(edge1,Is.EqualTo(edge1));
            Assert.That(edge1,Is.EqualTo(edge2));
            Assert.That(edge1.Equals((object)edge2),Is.True);
            Assert.That(edge1,Is.Not.EqualTo(edge3));

            Assert.That(edge1,Is.Not.Null);
            Assert.That(edge1.Equals(null),Is.False);
        }

        [Test]
        public void Hashcode()
        {
            var wrappedEdge = new Edge<int>(1, 2);
            var edge1 = new SReversedEdge<int, Edge<int>>(wrappedEdge);
            var edge2 = new SReversedEdge<int, Edge<int>>(wrappedEdge);
            var edge3 = new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2));
            var edge4 = new SReversedEdge<int, Edge<int>>(new Edge<int>(2, 1));

            Assert.That(edge1.GetHashCode(),Is.EqualTo(edge2.GetHashCode()));
            Assert.That(edge1.GetHashCode(),Is.Not.EqualTo(edge3.GetHashCode()));
            Assert.That(edge1.GetHashCode(),Is.Not.EqualTo(edge4.GetHashCode()));
        }

        [Test]
        public void HashcodeDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SReversedEdge<int, Edge<int>>);
            var edge2 = new SReversedEdge<int, Edge<int>>();

            Assert.That(edge1.GetHashCode(),Is.EqualTo(edge2.GetHashCode()));
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new SReversedEdge<int, Edge<int>>(new Edge<int>(1, 2));
            var edge2 = new SReversedEdge<int, Edge<int>>(new Edge<int>(2, 1));
            var edge3 = new SReversedEdge<int, UndirectedEdge<int>>(new UndirectedEdge<int>(1, 2));

            Assert.That("R(1 -> 2)",Is.EqualTo(edge1.ToString()));
            Assert.That("R(2 -> 1)",Is.EqualTo(edge2.ToString()));
            Assert.That("R(1 <-> 2)",Is.EqualTo(edge3.ToString()));
        }
    }
}