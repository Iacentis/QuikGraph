using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Tests for <see cref="SEquatableTaggedEdge{TVertex,TTag}"/>.
    ///</summary>
    [TestFixture]
    internal sealed class SEquatableTaggedEdgeTests : EdgeTestsBase
    {
        [Test]
        public void Construction()
        {
            var tag = new TestObject(1);

            // Value type
            CheckTaggedEdge(new SEquatableTaggedEdge<int, TestObject>(1, 2, null), 1, 2, (TestObject)null);
            CheckTaggedEdge(new SEquatableTaggedEdge<int, TestObject>(2, 1, null), 2, 1, (TestObject)null);
            CheckTaggedEdge(new SEquatableTaggedEdge<int, TestObject>(1, 1, null), 1, 1, (TestObject)null);
            CheckTaggedEdge(default(SEquatableTaggedEdge<int, TestObject>), 0, 0, (TestObject)null);
            CheckTaggedEdge(new SEquatableTaggedEdge<int, TestObject>(1, 2, tag), 1, 2, tag);

            // Reference type
            var v1 = new TestVertex("v1");
            var v2 = new TestVertex("v2");
            CheckTaggedEdge(new SEquatableTaggedEdge<TestVertex, TestObject>(v1, v2, null), v1, v2, (TestObject)null);
            CheckTaggedEdge(new SEquatableTaggedEdge<TestVertex, TestObject>(v2, v1, null), v2, v1, (TestObject)null);
            CheckTaggedEdge(new SEquatableTaggedEdge<TestVertex, TestObject>(v1, v1, null), v1, v1, (TestObject)null);
            CheckStructTaggedEdge(default(SEquatableTaggedEdge<TestVertex, TestObject>), (TestVertex)null, null, (TestObject)null);
            CheckTaggedEdge(new SEquatableTaggedEdge<TestVertex, TestObject>(v1, v2, tag), v1, v2, tag);

            // Struct break the contract with their implicit default constructor
            // Non struct edge should be preferred
            var defaultEdge = default(SEquatableTaggedEdge<TestVertex, int>);
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
            Assert.Throws<ArgumentNullException>(() => new SEquatableTaggedEdge<TestVertex, TestObject>(null, new TestVertex("v1"), null));
            Assert.Throws<ArgumentNullException>(() => new SEquatableTaggedEdge<TestVertex, TestObject>(new TestVertex("v1"), null, null));
            Assert.Throws<ArgumentNullException>(() => new SEquatableTaggedEdge<TestVertex, TestObject>(null, null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Equals()
        {
            var tag1 = new TestObject(1);
            var tag2 = new TestObject(2);
            var edge1 = default(SEquatableTaggedEdge<int, TestObject>);
            var edge2 = new SEquatableTaggedEdge<int, TestObject>(0, 0, null);
            var edge3 = new SEquatableTaggedEdge<int, TestObject>(1, 2, null);
            var edge4 = new SEquatableTaggedEdge<int, TestObject>(1, 2, null);
            var edge5 = new SEquatableTaggedEdge<int, TestObject>(2, 1, null);
            var edge6 = new SEquatableTaggedEdge<int, TestObject>(1, 2, tag1);
            var edge7 = new SEquatableTaggedEdge<int, TestObject>(1, 2, tag1);
            var edge8 = new SEquatableTaggedEdge<int, TestObject>(1, 2, tag2);

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

            Assert.That(edge3,Is.EqualTo(edge4));
            Assert.That(edge4,Is.EqualTo(edge3));
            Assert.That(edge3.Equals((object)edge4),Is.True);
            Assert.That(edge3.Equals(edge4),Is.True);
            Assert.That(edge4.Equals(edge3),Is.True);

            Assert.That(edge3,Is.Not.EqualTo(edge5));
            Assert.That(edge5,Is.Not.EqualTo(edge3));
            Assert.That(edge3.Equals((object)edge5),Is.False);
            Assert.That(edge3.Equals(edge5),Is.False);
            Assert.That(edge5.Equals(edge3),Is.False);

            Assert.That(edge3,Is.EqualTo(edge6));  // Tag is not taken into account for equality
            Assert.That(edge6,Is.EqualTo(edge3));  // Tag is not taken into account for equality
            Assert.That(edge3.Equals((object)edge6),Is.True); // Tag is not taken into account for equality
            Assert.That(edge3.Equals(edge6),Is.True);  // Tag is not taken into account for equality
            Assert.That(edge6.Equals(edge3),Is.True);  // Tag is not taken into account for equality

            Assert.That(edge6,Is.EqualTo(edge7));
            Assert.That(edge7,Is.EqualTo(edge6));
            Assert.That(edge6.Equals((object)edge7),Is.True);
            Assert.That(edge6.Equals(edge7),Is.True);
            Assert.That(edge7.Equals(edge6),Is.True);

            Assert.That(edge6,Is.EqualTo(edge8));  // Tag is not taken into account for equality
            Assert.That(edge8,Is.EqualTo(edge6));  // Tag is not taken into account for equality
            Assert.That(edge6.Equals((object)edge8),Is.True); // Tag is not taken into account for equality
            Assert.That(edge6.Equals(edge8),Is.True);  // Tag is not taken into account for equality
            Assert.That(edge8.Equals(edge6),Is.True);  // Tag is not taken into account for equality

            Assert.That(edge1,Is.Not.Null);
            Assert.That(edge1.Equals(null),Is.False);
        }

        [Test]
        public void EqualsDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SEquatableTaggedEdge<TestVertex, TestObject>);
            var edge2 = new SEquatableTaggedEdge<TestVertex, TestObject>();

            Assert.That(edge1,Is.EqualTo(edge2));
            Assert.That(edge2,Is.EqualTo(edge1));
            Assert.That(edge1.Equals(edge2),Is.True);
            Assert.That(edge2.Equals(edge1),Is.True);
        }

        [Test]
        public void Hashcode()
        {
            var tag = new TestObject(1);

            var edge1 = new SEquatableTaggedEdge<int, TestObject>(1, 2, null);
            var edge2 = new SEquatableTaggedEdge<int, TestObject>(1, 2, null);
            var edge3 = new SEquatableTaggedEdge<int, TestObject>(2, 1, null);
            var edge4 = new SEquatableTaggedEdge<int, TestObject>(1, 2, tag);

            Assert.That(edge1.GetHashCode(),Is.EqualTo(edge2.GetHashCode()));
            Assert.That(edge1.GetHashCode(),Is.Not.EqualTo(edge3.GetHashCode()));
            Assert.That(edge1.GetHashCode(),Is.EqualTo(edge4.GetHashCode()));  // Tag is not taken into account for hashcode
        }

        [Test]
        public void HashcodeDefaultEdge_ReferenceTypeExtremities()
        {
            var edge1 = default(SEquatableTaggedEdge<TestVertex, TestObject>);
            var edge2 = new SEquatableTaggedEdge<TestVertex, TestObject>();

            Assert.That(edge1.GetHashCode(),Is.EqualTo(edge2.GetHashCode()));
        }

        [Test]
        public void TagChanged()
        {
            var edge = new SEquatableTaggedEdge<int, TestObject>(1, 2, null);

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
            var edge1 = new SEquatableTaggedEdge<int, TestObject>(1, 2, null);
            var edge2 = new SEquatableTaggedEdge<int, TestObject>(1, 2, new TestObject(42));
            var edge3 = new SEquatableTaggedEdge<int, TestObject>(2, 1, null);

            Assert.That("1 -> 2 (no tag)",Is.EqualTo(edge1.ToString()));
            Assert.That("1 -> 2 (42)",Is.EqualTo(edge2.ToString()));
            Assert.That("2 -> 1 (no tag)",Is.EqualTo(edge3.ToString()));
        }
    }
}