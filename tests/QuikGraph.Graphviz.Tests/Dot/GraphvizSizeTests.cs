using System;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;
using static QuikGraph.Tests.SerializationTestHelpers;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizSizeF"/> and <see cref="GraphvizSize"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizSizeTests
    {
        [Test]
        public void Constructor_Size()
        {
            var size = default(GraphvizSize);
            CheckSize(size, 0, 0, true);

            size = new GraphvizSize(0, 10);
            CheckSize(size, 0, 10, true);

            size = new GraphvizSize(10, 0);
            CheckSize(size, 10, 0, true);

            size = new GraphvizSize(10, 15);
            CheckSize(size, 10, 15, false);

            size = new GraphvizSize(30, 25);
            CheckSize(size, 30, 25, false);

            #region Local function

            void CheckSize(GraphvizSize s, int w, int h, bool empty)
            {
                Assert.That(w, Is.EqualTo(s.Width));
                Assert.That(h, Is.EqualTo(s.Height));
                Assert.That(empty, Is.EqualTo(s.IsEmpty));
            }

            #endregion
        }

        [Test]
        public void Constructor_SizeThrow()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => new GraphvizSize(-1, 10));
            Assert.Throws<ArgumentException>(() => new GraphvizSize(10, -2));
            Assert.Throws<ArgumentException>(() => new GraphvizSize(-5, -2));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void Constructor_SizeF()
        {
            var size = default(GraphvizSizeF);
            CheckSize(size, 0.0f, 0.0f, true);

            size = new GraphvizSizeF(0, 10);
            CheckSize(size, 0.0f, 10.0f, true);

            size = new GraphvizSizeF(10, 0);
            CheckSize(size, 10.0f, 0.0f, true);

            size = new GraphvizSizeF(10, 15);
            CheckSize(size, 10.0f, 15.0f, false);

            size = new GraphvizSizeF(30.3f, 25.5f);
            CheckSize(size, 30.3f, 25.5f, false);

            #region Local function

            void CheckSize(GraphvizSizeF s, float w, float h, bool empty)
            {
                Assert.That(w, Is.EqualTo(s.Width));
                Assert.That(h, Is.EqualTo(s.Height));
                Assert.That(empty, Is.EqualTo(s.IsEmpty));
            }

            #endregion
        }

        [Test]
        public void Constructor_SizeFThrow()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => new GraphvizSizeF(-1.0f, 10.0f));
            Assert.Throws<ArgumentException>(() => new GraphvizSizeF(10.0f, -2.0f));
            Assert.Throws<ArgumentException>(() => new GraphvizSizeF(-5.0f, -2.0f));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void ToString_Size()
        {
            var size1 = default(GraphvizSize);
            var size2 = new GraphvizSize(12, 25);

            Assert.That("0x0", Is.EqualTo(size1.ToString()));
            Assert.That("12x25", Is.EqualTo(size2.ToString()));
        }

        [Test]
        public void ToString_SizeF()
        {
            var size1 = default(GraphvizSizeF);
            var size2 = new GraphvizSizeF(12.2f, 25.6f);

            Assert.That("0x0", Is.EqualTo(size1.ToString()));
            Assert.That("12.2x25.6", Is.EqualTo(size2.ToString()));
        }

        [Test]
        [Obsolete("Obsolete")]
        public void Serialization_Size()
        {
            var size = new GraphvizSize(150, 200);
            GraphvizSize deserializedSize = SerializeAndDeserialize(size);
            Assert.That(size.IsEmpty, Is.EqualTo(deserializedSize.IsEmpty));
            Assert.That(size.Width, Is.EqualTo(deserializedSize.Width));
            Assert.That(size.Height, Is.EqualTo(deserializedSize.Height));
        }

        [Test]
        [Obsolete("Obsolete")]
        public void Serialization_SizeF()
        {
            var size = new GraphvizSizeF(150.5f, 200.6f);
            GraphvizSizeF deserializedSize = SerializeAndDeserialize(size);
            Assert.That(size.IsEmpty, Is.EqualTo(deserializedSize.IsEmpty));
            Assert.That(size.Width, Is.EqualTo(deserializedSize.Width));
            Assert.That(size.Height, Is.EqualTo(deserializedSize.Height));
        }
    }
}