using System;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;
using static QuikGraph.Tests.SerializationTestHelpers;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizColor"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizColorTests
    {
        #region Test classes

        private class TestClass
        {
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var color = default(GraphvizColor);
            CheckColor(color, 0, 0, 0, 0);

            color = new GraphvizColor(250, 255, 125, 45);
            CheckColor(color, 250, 255, 125, 45);

            color = new GraphvizColor(125, 12, 25, 42);
            CheckColor(color, 125, 12, 25, 42);

            #region Local function

            void CheckColor(GraphvizColor c, byte a, byte r, byte g, byte b)
            {
                Assert.That(a,Is.EqualTo(c.A));
                Assert.That(r,Is.EqualTo(c.R));
                Assert.That(g,Is.EqualTo(c.G));
                Assert.That(b,Is.EqualTo(c.B));
            }

            #endregion
        }

        [Test]
        public void Equals()
        {
            var color1 = new GraphvizColor(255, 150, 160, 25);
            var color2 = new GraphvizColor(255, 150, 160, 25);
            var color3 = new GraphvizColor(125, 150, 160, 25);
            var color4 = new GraphvizColor(255, 155, 160, 25);
            var color5 = new GraphvizColor(255, 150, 120, 25);
            var color6 = new GraphvizColor(255, 150, 160, 30);

            Assert.That(color1,Is.EqualTo(color1));

            Assert.That(color1 == color2,Is.True);
            Assert.That(color1.Equals(color2),Is.True);
            Assert.That(color1.Equals((object)color2),Is.True);
            Assert.That(color1 != color2,Is.False);
            Assert.That(color1,Is.EqualTo(color2));

            Assert.That(color1 == color3,Is.False);
            Assert.That(color1.Equals(color3),Is.False);
            Assert.That(color1.Equals((object)color3),Is.False);
            Assert.That(color1 != color3,Is.True);
            Assert.That(color1,Is.Not.EqualTo(color3));

            Assert.That(color1 == color4,Is.False);
            Assert.That(color1.Equals(color4),Is.False);
            Assert.That(color1.Equals((object)color4),Is.False);
            Assert.That(color1 != color4,Is.True);
            Assert.That(color1,Is.Not.EqualTo(color4));

            Assert.That(color1 == color5,Is.False);
            Assert.That(color1.Equals(color5),Is.False);
            Assert.That(color1.Equals((object)color5),Is.False);
            Assert.That(color1 != color5,Is.True);
            Assert.That(color1,Is.Not.EqualTo(color5));

            Assert.That(color1 == color6,Is.False);
            Assert.That(color1.Equals(color6),Is.False);
            Assert.That(color1.Equals((object)color6),Is.False);
            Assert.That(color1 != color6,Is.True);
            Assert.That(color1,Is.Not.EqualTo(color6));

            Assert.That(color1,Is.Not.Null);
            Assert.That(color1.Equals(null),Is.False);
            Assert.That(new TestClass(),Is.Not.EqualTo(color1));
            Assert.That(color1,Is.Not.EqualTo(new TestClass()));
        }

        [Test]
        public void HashCode()
        {
            var color1 = new GraphvizColor();
            var color2 = new GraphvizColor();
            var color3 = new GraphvizColor(125, 150, 160, 25);
            var color4 = new GraphvizColor(125, 150, 160, 25);

            Assert.That(color1.GetHashCode(),Is.EqualTo(color2.GetHashCode()));
            Assert.That(color1.GetHashCode(),Is.Not.EqualTo(color3.GetHashCode()));
            Assert.That(color3.GetHashCode(),Is.EqualTo(color4.GetHashCode()));
        }

        [Test]
        [Obsolete("Obsolete")]
        public void Serialization()
        {
            var color = new GraphvizColor(255, 25, 60, 234);
            GraphvizColor deserializedColor = SerializeAndDeserialize(color);
            Assert.That(color.A,Is.EqualTo(deserializedColor.A));
            Assert.That(color.R,Is.EqualTo(deserializedColor.R));
            Assert.That(color.G,Is.EqualTo(deserializedColor.G));
            Assert.That(color.B,Is.EqualTo(deserializedColor.B));
        }
    }
}