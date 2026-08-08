using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="SEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class SEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new SEdge<int>(1, 2), 1, 2);
            CheckEdge(new SEdge<int>(2, 1), 2, 1);
            CheckEdge(new SEdge<int>(1, 1), 1, 1);
            CheckEdge(default(SEdge<int>), 0, 0);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new SEdge<TestVertex>(v1, v2), v1, v2);
            CheckEdge(new SEdge<TestVertex>(v2, v1), v2, v1);
            CheckEdge(new SEdge<TestVertex>(v1, v1), v1, v1);

            // Struct break the contract with their implicit default constructor
            // Non struct edge should be preferred
            var defaultEdge = default(SEdge<TestVertex>);
            Assert.That(defaultEdge.Source,Is.Null);
            // ReSharper disable once HeuristicUnreachableCode
            // Justification: Since struct has implicit default constructor it allows initialization of invalid edge
            Assert.That(defaultEdge.Target,Is.Null);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SEdge<TestVertex>(null, new TestVertex("v1")));
            Assert.Throws<ArgumentNullException>(() => new SEdge<TestVertex>(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => new SEdge<TestVertex>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = default(SEdge<int>);
            var edge2 = new SEdge<int>(0, 0);
            var edge3 = new SEdge<int>(1, 2);
            var edge4 = new SEdge<int>(1, 2);
            var edge5 = new SEdge<int>(2, 1);

            Assert.That(edge1,Is.EqualTo(edge1));

            Assert.That(edge1,Is.EqualTo(edge2));  // Is equatable
            Assert.That(edge2,Is.EqualTo(edge1));  // Is equatable
            Assert.That(edge1.Equals(edge2),Is.True);  // Is equatable
            Assert.That(edge2.Equals(edge1),Is.True);  // Is equatable

            Assert.That(edge1,Is.Not.EqualTo(edge3));
            Assert.That(edge3,Is.Not.EqualTo(edge1));
            Assert.That(edge1.Equals(edge3),Is.False);
            Assert.That(edge3.Equals(edge1),Is.False);

            Assert.That(edge3,Is.EqualTo(edge4));
            Assert.That(edge4,Is.EqualTo(edge3));
            Assert.That(edge3.Equals(edge4),Is.True);
            Assert.That(edge4.Equals(edge3),Is.True);

            Assert.That(edge3,Is.Not.EqualTo(edge5));
            Assert.That(edge5,Is.Not.EqualTo(edge3));
            Assert.That(edge3.Equals(edge5),Is.False);
            Assert.That(edge5.Equals(edge3),Is.False);

            Assert.That(edge1,Is.Not.Null);
            Assert.That(edge1.Equals(null),Is.False);
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SEdge<TestVertex>);
            var edge2 = new SEdge<TestVertex>();

            Assert.That(edge1,Is.EqualTo(edge2));
            Assert.That(edge2,Is.EqualTo(edge1));
            Assert.That(edge1.Equals(edge2),Is.True);
            Assert.That(edge2.Equals(edge1),Is.True);
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new SEdge<int>(1, 2);
            var edge2 = new SEdge<int>(2, 1);

            Assert.That("1 -> 2",Is.EqualTo(edge1.ToString()));
            Assert.That("2 -> 1",Is.EqualTo(edge2.ToString()));
        }
    }
}