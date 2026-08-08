using System;
using NUnit.Framework;

namespace QuikGraph.Tests.Events
{
    /// <summary>
    /// Tests related to <see cref="UndirectedEdgeEventArgs{TVertex,TEdge}"/>.
    /// </summary>
    internal sealed class UndirectedEdgeEventArgsTests
    {
        [Test]
        public void Constructor()
        {
            var edge = new Edge<int>(1, 2);

            var args = new UndirectedEdgeEventArgs<int, Edge<int>>(edge, false);
            Assert.That(args.Reversed,Is.False);
            Assert.That(edge,Is.SameAs(args.Edge));
            Assert.That(1,Is.EqualTo(args.Source));
            Assert.That(2,Is.EqualTo(args.Target));

            args = new UndirectedEdgeEventArgs<int, Edge<int>>(edge, true);
            Assert.That(args.Reversed,Is.True);
            Assert.That(edge,Is.SameAs(args.Edge));
            Assert.That(2,Is.EqualTo(args.Source));
            Assert.That(1,Is.EqualTo(args.Target));
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedEdgeEventArgs<int, Edge<int>>(null, false));
            Assert.Throws<ArgumentNullException>(
                () => new UndirectedEdgeEventArgs<int, Edge<int>>(null, true));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }
    }
}