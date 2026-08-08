using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="STaggedUndirectedEdge{TVertex,TTag}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class STaggedUndirectedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var tag = new TestObject(1);

            // Value type
            CheckTaggedEdge(new STaggedUndirectedEdge<int, TestObject>(1, 2, null), 1, 2, (TestObject)null);
            CheckTaggedEdge(new STaggedUndirectedEdge<int, TestObject>(1, 1, null), 1, 1, (TestObject)null);
            CheckTaggedEdge(default(STaggedUndirectedEdge<int, TestObject>), 0, 0, (TestObject)null);
            CheckTaggedEdge(new STaggedUndirectedEdge<int, TestObject>(1, 2, tag), 1, 2, tag);

            // Reference type
            var v1 = new ComparableTestVertex("v1");
            var v2 = new ComparableTestVertex("v2");
            CheckTaggedEdge(new STaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v2, null), v1, v2, (TestObject)null);
            CheckTaggedEdge(new STaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v1, null), v1, v1, (TestObject)null);
            CheckStructTaggedEdge(default(STaggedUndirectedEdge<ComparableTestVertex, TestObject>), (ComparableTestVertex)null, null, (TestObject)null);
            CheckTaggedEdge(new STaggedUndirectedEdge<ComparableTestVertex, TestObject>(v1, v2, tag), v1, v2, tag);

            // Struct break the contract with their implicit default constructor
            // Non struct edge should be preferred
            var defaultEdge = default(STaggedUndirectedEdge<ComparableTestVertex, int>);
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
            Assert.Throws<ArgumentNullException>(() => new STaggedUndirectedEdge<TestVertex, TestObject>(null, new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => new STaggedUndirectedEdge<TestVertex, TestObject>(new TestVertex("v1"), null, null));
            Assert.Throws<ArgumentNullException>(() => new STaggedUndirectedEdge<TestVertex, TestObject>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<ArgumentException>(() => new STaggedUndirectedEdge<int, TestObject>(2, 1, null));

            // Not comparable
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            Assert.Throws<ArgumentException>(() => new STaggedUndirectedEdge<TestVertex, TestObject>(v1, v2, null));

            var comparableV1 = new ComparableTestVertex("v1");
            var comparableV2 = new ComparableTestVertex("v2");
            Assert.Throws<ArgumentException>(() => new STaggedUndirectedEdge<ComparableTestVertex, TestObject>(comparableV2, comparableV1, null));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var tag1 = new TestObject(1);
            var tag2 = new TestObject(2);
            var edge1 = default(STaggedUndirectedEdge<int, TestObject>);
            var edge2 = new STaggedUndirectedEdge<int, TestObject>(0, 0, null);
            var edge3 = new STaggedUndirectedEdge<int, TestObject>(1, 2, null);
            var edge4 = new STaggedUndirectedEdge<int, TestObject>(1, 2, null);
            var edge6 = new STaggedUndirectedEdge<int, TestObject>(1, 2, tag1);
            var edge7 = new STaggedUndirectedEdge<int, TestObject>(1, 2, tag1);
            var edge8 = new STaggedUndirectedEdge<int, TestObject>(1, 2, tag2);

            Assert.That(edge1,Is.EqualTo(edge1));

            Assert.That(edge1,Is.EqualTo(edge2));
            Assert.That(edge2,Is.EqualTo(edge1));
            Assert.That(edge1.Equals(edge2),Is.True);
            Assert.That(edge2.Equals(edge1),Is.True);

            Assert.That(edge1,Is.Not.EqualTo(edge3));
            Assert.That(edge3,Is.Not.EqualTo(edge1));
            Assert.That(edge1.Equals(edge3),Is.False);
            Assert.That(edge3.Equals(edge1),Is.False);

            Assert.That(edge3,Is.EqualTo(edge4));
            Assert.That(edge4,Is.EqualTo(edge3));
            Assert.That(edge3.Equals(edge4),Is.True);
            Assert.That(edge4.Equals(edge3),Is.True);

            Assert.That(edge3,Is.Not.EqualTo(edge6));
            Assert.That(edge6,Is.Not.EqualTo(edge3));
            Assert.That(edge3.Equals(edge6),Is.False);
            Assert.That(edge6.Equals(edge3),Is.False);

            Assert.That(edge6,Is.EqualTo(edge7));
            Assert.That(edge7,Is.EqualTo(edge6));
            Assert.That(edge6.Equals(edge7),Is.True);
            Assert.That(edge7.Equals(edge6),Is.True);

            Assert.That(edge6,Is.Not.EqualTo(edge8));
            Assert.That(edge8,Is.Not.EqualTo(edge6));
            Assert.That(edge6.Equals(edge8),Is.False);
            Assert.That(edge8.Equals(edge6),Is.False);

            Assert.That(edge1,Is.Not.Null);
            Assert.That(edge1.Equals(null),Is.False);
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(STaggedUndirectedEdge<int, TestObject>);
            var edge2 = new STaggedUndirectedEdge<int, TestObject>();

            Assert.That(edge1,Is.EqualTo(edge2));
            Assert.That(edge2,Is.EqualTo(edge1));
            Assert.That(edge1.Equals(edge2),Is.True);
            Assert.That(edge2.Equals(edge1),Is.True);
        }

        [Test]
        public void TagChanged()
        {
            var edge = new STaggedUndirectedEdge<int, TestObject>(1, 2, null);

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
            var edge1 = new STaggedUndirectedEdge<int, TestObject>(1, 2, null);
            var edge2 = new STaggedUndirectedEdge<int, TestObject>(1, 2, new TestObject(42));

            Assert.That("1 <-> 2 (no tag)",Is.EqualTo(edge1.ToString()));
            Assert.That("1 <-> 2 (42)",Is.EqualTo(edge2.ToString()));
        }
    }
}