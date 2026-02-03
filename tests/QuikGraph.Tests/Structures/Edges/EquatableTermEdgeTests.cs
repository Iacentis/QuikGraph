using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="EquatableTermEdge{TVertex}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class EquatableTermEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            // Value type
            CheckEdge(new EquatableTermEdge<int>(1, 2), 1, 2);
            CheckEdge(new EquatableTermEdge<int>(2, 1), 2, 1);
            CheckEdge(new EquatableTermEdge<int>(1, 1), 1, 1);

            CheckTermEdge(new EquatableTermEdge<int>(1, 2, 0, 1), 1, 2, 0, 1);
            CheckTermEdge(new EquatableTermEdge<int>(2, 1, 1, 0), 2, 1, 1, 0);
            CheckTermEdge(new EquatableTermEdge<int>(1, 1, 0, 0), 1, 1, 0, 0);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckEdge(new EquatableTermEdge<TestVertex>(v1, v2), v1, v2);
            CheckEdge(new EquatableTermEdge<TestVertex>(v2, v1), v2, v1);
            CheckEdge(new EquatableTermEdge<TestVertex>(v1, v1), v1, v1);

            CheckTermEdge(new EquatableTermEdge<TestVertex>(v1, v2, 0, 1), v1, v2, 0, 1);
            CheckTermEdge(new EquatableTermEdge<TestVertex>(v2, v1, 1, 0), v2, v1, 1, 0);
            CheckTermEdge(new EquatableTermEdge<TestVertex>(v1, v1, 0, 0), v1, v1, 0, 0);
        }

        [Test]
        public void Construction_Throws()
        {
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(null, v1));
            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(v1, null));
            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(null, null));

            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(null, v1, 0, 1));
            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(v1, null, 0, 1));
            Assert.Throws<ArgumentNullException>(() => new EquatableTermEdge<TestVertex>(null, null, 0, 1));
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<ArgumentException>(() => new EquatableTermEdge<TestVertex>(v1, v2, -1, 0));
            Assert.Throws<ArgumentException>(() => new EquatableTermEdge<TestVertex>(v1, v2, 0, -1));
            Assert.Throws<ArgumentException>(() => new EquatableTermEdge<TestVertex>(v1, v2, -1, -1));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var edge1 = new EquatableTermEdge<int>(1, 2);
            var edge2 = new EquatableTermEdge<int>(1, 2);
            var edge3 = new EquatableTermEdge<int>(1, 2, 0, 0);
            var edge4 = new EquatableTermEdge<int>(1, 2, 0, 0);
            var edge5 = new EquatableTermEdge<int>(1, 2, 0, 1);
            var edge6 = new EquatableTermEdge<int>(1, 2, 0, 1);

            Assert.That(edge1,Is.EqualTo(edge1));
            Assert.That(edge3,Is.EqualTo(edge3));
            Assert.That(edge5,Is.EqualTo(edge5));

            Assert.That(edge1,Is.EqualTo(edge2));
            Assert.That(edge2,Is.EqualTo(edge1));
            Assert.That(edge1.Equals((object)edge2),Is.True);
            Assert.That(edge1.Equals(edge2),Is.True);
            Assert.That(edge2.Equals(edge1),Is.True);

            Assert.That(edge1,Is.EqualTo(edge3));
            Assert.That(edge3,Is.EqualTo(edge1));
            Assert.That(edge1.Equals((object)edge3),Is.True);
            Assert.That(edge1.Equals(edge3),Is.True);
            Assert.That(edge3.Equals(edge1),Is.True);

            Assert.That(edge1,Is.Not.EqualTo(edge5));
            Assert.That(edge5,Is.Not.EqualTo(edge1));
            Assert.That(edge1.Equals((object)edge5),Is.False);
            Assert.That(edge1.Equals(edge5),Is.False);
            Assert.That(edge5.Equals(edge1),Is.False);

            Assert.That(edge3,Is.EqualTo(edge4));
            Assert.That(edge4,Is.EqualTo(edge3));
            Assert.That(edge3.Equals((object)edge4),Is.True);
            Assert.That(edge3.Equals(edge4),Is.True);
            Assert.That(edge4.Equals(edge3),Is.True);

            Assert.That(edge5,Is.EqualTo(edge6));
            Assert.That(edge6,Is.EqualTo(edge5));
            Assert.That(edge5.Equals((object)edge6),Is.True);
            Assert.That(edge5.Equals(edge6),Is.True);
            Assert.That(edge6.Equals(edge5),Is.True);

            Assert.That(edge1,Is.Not.Null);
            Assert.That(edge1.Equals(null),Is.False);
        }

        [Test]
        public void Hashcode()
        {
            var edge1 = new EquatableTermEdge<int>(1, 2);
            var edge2 = new EquatableTermEdge<int>(1, 2);
            var edge3 = new EquatableTermEdge<int>(2, 1);

            Assert.That(edge1.GetHashCode(),Is.EqualTo(edge2.GetHashCode()));
            Assert.That(edge1.GetHashCode(),Is.Not.EqualTo(edge3.GetHashCode()));
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new EquatableTermEdge<int>(1, 2);
            var edge2 = new EquatableTermEdge<int>(1, 2, 1, 5);
            var edge3 = new EquatableTermEdge<int>(2, 1);

            Assert.That("1 (0) -> 2 (0)",Is.EqualTo(edge1.ToString()));
            Assert.That("1 (1) -> 2 (5)",Is.EqualTo(edge2.ToString()));
            Assert.That("2 (0) -> 1 (0)",Is.EqualTo(edge3.ToString()));
        }
    }
}