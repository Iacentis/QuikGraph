using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizPoint"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizPointTests
    {
        [Test]
        public void Constructor()
        {
            var point = new GraphvizPoint(0, 0);
            Assert.That(0,Is.EqualTo(point.X));
            Assert.That(0,Is.EqualTo(point.Y));

            point = new GraphvizPoint(1, 5);
            Assert.That(1,Is.EqualTo(point.X));
            Assert.That(5,Is.EqualTo(point.Y));

            point = new GraphvizPoint(-1, 3);
            Assert.That(-1,Is.EqualTo(point.X));
            Assert.That(3,Is.EqualTo(point.Y));
        }
    }
}