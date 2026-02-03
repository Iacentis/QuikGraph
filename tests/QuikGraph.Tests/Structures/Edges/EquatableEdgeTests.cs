using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="EquatableEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class EquatableEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new EquatableEdge<int>(1, 2), 1, 2);
            CheckEdge(new EquatableEdge<int>(2, 1), 2, 1);
            CheckEdge(new EquatableEdge<int>(1, 1), 1, 1);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new EquatableEdge<TestVertex>(v1, v2), v1, v2);
            CheckEdge(new EquatableEdge<TestVertex>(v2, v1), v2, v1);
            CheckEdge(new EquatableEdge<TestVertex>(v1, v1), v1, v1);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new EquatableEdge<TestVertex>(null, new TestVertex("v1")));
            Assert.Throws<ArgumentNullException>(() => new EquatableEdge<TestVertex>(new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => new EquatableEdge<TestVertex>(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 2);
            var edge3 = new EquatableEdge<int>(2, 1);

            Assert.That(edge1, Is.EqualTo(edge1));

            Assert.That(edge1, Is.EqualTo(edge2));
            Assert.That(edge2, Is.EqualTo(edge1));
            Assert.That(edge1.Equals((object)edge2), Is.True);
            Assert.That(edge1.Equals(edge2), Is.True);
            Assert.That(edge2.Equals(edge1), Is.True);

            Assert.That(edge1, Is.Not.EqualTo(edge3));
            Assert.That(edge3, Is.Not.EqualTo(edge1));
            Assert.That(edge1.Equals((object)edge3), Is.False);
            Assert.That(edge1.Equals(edge3), Is.False);
            Assert.That(edge3.Equals(edge1), Is.False);

            Assert.That(edge1, Is.Not.EqualTo(null));
            Assert.That(edge1.Equals(null), Is.False);
        }

        [Test]
        public void Hashcode()
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(1, 2);
            var edge3 = new EquatableEdge<int>(2, 1);

            Assert.That(edge1.GetHashCode(), Is.EqualTo(edge2.GetHashCode()));
            Assert.That(edge1.GetHashCode(), Is.Not.EqualTo(edge3.GetHashCode()));
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new EquatableEdge<int>(1, 2);
            var edge2 = new EquatableEdge<int>(2, 1);

            Assert.That("1 -> 2", Is.EqualTo(edge1.ToString()));
            Assert.That("2 -> 1", Is.EqualTo(edge2.ToString()));
        }
    }
}