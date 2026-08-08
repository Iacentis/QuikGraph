using NUnit.Framework;
using Microsoft.Msagl.Drawing;

namespace QuikGraph.MSAGL.Tests
{
    internal static class MsaglGraphTestHelpers
    {
        public static void AssertAreEquivalent<TVertex, TEdge>(
            IEdgeListGraph<TVertex, TEdge> graph,
            Graph msaglGraph)
            where TEdge : IEdge<TVertex>
        {
            Assert.That(graph.IsDirected, Is.EqualTo(msaglGraph.Directed));
            Assert.That(graph.VertexCount, Is.EqualTo(msaglGraph.NodeCount));
            Assert.That(graph.EdgeCount, Is.EqualTo(msaglGraph.EdgeCount));
        }
    }
}