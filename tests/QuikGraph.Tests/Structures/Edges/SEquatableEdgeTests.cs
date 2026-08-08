using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="SEquatableEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class SEquatableEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new SEquatableEdge<int>(1, 2), 1, 2);
            CheckEdge(new SEquatableEdge<int>(2, 1), 2, 1);
            CheckEdge(new SEquatableEdge<int>(1, 1), 1, 1);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new SEquatableEdge<TestVertex>(v1, v2), v1, v2);
            CheckEdge(new SEquatableEdge<TestVertex>(v2, v1), v2, v1);
            CheckEdge(new SEquatableEdge<TestVertex>(v1, v1), v1, v1);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SEquatableEdge<TestVertex>(null, new TestVertex("v1")));
            Assert.Throws<ArgumentNullException>(() => new SEquatableEdge<TestVertex>(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => new SEquatableEdge<TestVertex>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = new SEquatableEdge<int>(1, 2);
            var edge2 = new SEquatableEdge<int>(1, 2);
            var edge3 = new SEquatableEdge<int>(2, 1);

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

            Assert.That(edge1,Is.Not.Null);
            Assert.That(edge1.Equals(null),Is.False);
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SEquatableEdge<TestVertex>);
            var edge2 = new SEquatableEdge<TestVertex>();

            Assert.That(edge1,Is.EqualTo(edge2));
            Assert.That(edge2,Is.EqualTo(edge1));
            Assert.That(edge1.Equals(edge2),Is.True);
            Assert.That(edge2.Equals(edge1),Is.True);
        }

        [Test]
        public void Hashcode()
        {
            var edge1 = new SEquatableEdge<int>(1, 2);
            var edge2 = new SEquatableEdge<int>(1, 2);
            var edge3 = new SEquatableEdge<int>(2, 1);

            Assert.That(edge1.GetHashCode(),Is.EqualTo(edge2.GetHashCode()));
            Assert.That(edge1.GetHashCode(),Is.Not.EqualTo(edge3.GetHashCode()));
        }

        [Test]
        public void HashcodeDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SEquatableEdge<TestVertex>);
            var edge2 = new SEquatableEdge<TestVertex>();

            Assert.That(edge1.GetHashCode(),Is.EqualTo(edge2.GetHashCode()));
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new SEquatableEdge<int>(1, 2);
            var edge2 = new SEquatableEdge<int>(2, 1);

            Assert.That("1 -> 2",Is.EqualTo(edge1.ToString()));
            Assert.That("2 -> 1",Is.EqualTo(edge2.ToString()));
        }
    }
}