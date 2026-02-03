using System.Collections.Generic;
using System.Data;
using System.Linq;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace QuikGraph.Data.Tests
{
    internal static class GraphTestHelpers
    {
        public static void AssertHasRelations(
             IEdgeSet<DataTable, DataRelationEdge> graph,
             IEnumerable<DataRelationEdge> relations)
        {
            DataRelation[] relationArray = relations
                .Select(r => r.Relation)
                .ToArray();
            CollectionAssert.IsNotEmpty(relationArray);

            Assert.That(graph.IsEdgesEmpty,Is.False);
            Assert.That(relationArray.Length,Is.EqualTo(graph.EdgeCount));
            CollectionAssert.AreEquivalent(
                relationArray,
                graph.Edges.Select(r => r.Relation));
        }
    }
}