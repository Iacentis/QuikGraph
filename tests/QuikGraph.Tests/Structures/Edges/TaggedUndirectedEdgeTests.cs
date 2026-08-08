using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="TaggedUndirectedEdge{TVertex,TTag}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class TaggedUndirectedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var tag = new TestObject(1);

            // Value type
            CheckTaggedEdge(new TaggedUndirectedEdge<int, TestObject>(1, 2, null), 1, 2, (TestObject)null);
            CheckTaggedEdge(new TaggedUndirectedEdge<int, TestObject>(1, 1, null), 1, 1, (TestObject)null);
            CheckTaggedEdge(new TaggedUndirectedEdge<int, TestObject>(1, 2, tag), 1, 2, tag);

            // Reference type
            var v1 = new ComparableTestVertex("v1");
            var v2 = new ComparableTestVertex("v2");
            CheckTaggedEdge(new TaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v2, null), v1, v2, (TestObject)null);
            CheckTaggedEdge(new TaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v1, null), v1, v1, (TestObject)null);
            CheckTaggedEdge(new TaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v2, tag), v1, v2, tag);
        }

        [Test]
        public void Construction_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new TaggedUndirectedEdge<TestVertex, TestObject>(null, new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => new TaggedUndirectedEdge<TestVertex, TestObject>(new TestVertex("v1"), null, null));
            Assert.Throws<ArgumentNullException>(() => new TaggedUndirectedEdge<TestVertex, TestObject>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<ArgumentException>(() => new TaggedUndirectedEdge<int, TestObject>(2, 1, null));

            // Not comparable
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            Assert.Throws<ArgumentException>(() => new TaggedUndirectedEdge<TestVertex, TestObject>(v1, v2, null));

            var comparableV1 = new ComparableTestVertex("v1");
            var comparableV2 = new ComparableTestVertex("v2");
            Assert.Throws<ArgumentException>(() => new TaggedUndirectedEdge<ComparableTestVertex, TestObject>(comparableV2, comparableV1, null));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var tag1 = new TestObject(1);
            var tag2 = new TestObject(2);
            var edge1 = new TaggedUndirectedEdge<int, TestObject>(1, 2, tag1);
            var edge2 = new TaggedUndirectedEdge<int, TestObject>(1, 2, tag1);
            var edge3 = new TaggedUndirectedEdge<int, TestObject>(1, 2, tag2);
            var edge4 = new TaggedUndirectedEdge<int, TestObject>(1, 2, null);

            Assert.That(edge1,Is.EqualTo(edge1));

            Assert.That(edge1,Is.Not.EqualTo(edge2));
            Assert.That(edge2,Is.Not.EqualTo(edge1));
            Assert.That(edge1.Equals(edge2),Is.False);
            Assert.That(edge2.Equals(edge1),Is.False);

            Assert.That(edge1,Is.Not.EqualTo(edge3));
            Assert.That(edge3,Is.Not.EqualTo(edge1));
            Assert.That(edge1.Equals(edge3),Is.False);
            Assert.That(edge3.Equals(edge1),Is.False);

            Assert.That(edge1,Is.Not.EqualTo(edge4));
            Assert.That(edge4,Is.Not.EqualTo(edge1));
            Assert.That(edge1.Equals(edge4),Is.False);
            Assert.That(edge4.Equals(edge1),Is.False);

            Assert.That(edge1,Is.Not.Null);
            Assert.That(edge1.Equals(null),Is.False);
        }

        [Test]
        public void TagChanged()
        {
            var edge = new TaggedUndirectedEdge<int, TestObject>(1, 2, null);

            int changeCount = 0;
            edge.TagChanged += (_, _) => ++changeCount;

            edge.Tag = null;
            Assert.That(0,Is.EqualTo(changeCount));

            var tag1 = new TestObject(1);
            edge.Tag = tag1;
            Assert.That(1,Is.EqualTo(changeCount));

            edge.Tag = tag1;
            Assert.That(1,Is.EqualTo(changeCount));

            var tag2 = new TestObject(2);
            edge.Tag = tag2;
            Assert.That(2,Is.EqualTo(changeCount));

            edge.Tag = tag1;
            Assert.That(3,Is.EqualTo(changeCount));
        }

        [Test]
        public void ObjectToString()
        {
            var edge1 = new TaggedUndirectedEdge<int, TestObject>(1, 2, null);
            var edge2 = new TaggedUndirectedEdge<int, TestObject>(1, 2, new TestObject(12));

            Assert.That("1 <-> 2 (no tag)",Is.EqualTo(edge1.ToString()));
            Assert.That("1 <-> 2 (12)",Is.EqualTo(edge2.ToString()));
        }
    }
}