using System;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizFont"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizFontTests
    {
        [Test]
        public void Constructor()
        {
            var font = new GraphvizFont("TestFont", 12.5f);
            Assert.That("TestFont",Is.EqualTo(font.Name));
            Assert.That(12.5f,Is.EqualTo(font.SizeInPoints));

            font = new GraphvizFont("OtherFont", 22.0f);
            Assert.That("OtherFont",Is.EqualTo(font.Name));
            Assert.That(22.0f,Is.EqualTo(font.SizeInPoints));
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => new GraphvizFont(null, 12.5f));
            Assert.Throws<ArgumentException>(() => new GraphvizFont("", 12.5f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new GraphvizFont("Font", 0.0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new GraphvizFont("Font", -1.0f));
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}