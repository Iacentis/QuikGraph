using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using static QuikGraph.Tests.AssertHelpers;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Base class for tests over delegated graphs.
    /// </summary>
    internal class DelegateGraphTestsBase : GraphTestsBase
    {
        [Pure]
        protected static TryFunc<TVertex, IEnumerable<TEdge>> GetEmptyGetter<TVertex, TEdge>()
            where TEdge : IEdge<TVertex>
        {
            return (_, out edges) =>
            {
                edges = null;
                return false;
            };
        }

        protected class GraphData<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            public GraphData()
            {
                TryGetEdges = (_, out edges) =>
                {
                    ++_nbCalls;

                    if (ShouldReturnValue)
                        edges = ShouldReturnEdges ?? [];
                    else
                        edges = null;

                    return ShouldReturnValue;
                };
            }

            private int _nbCalls;


            public TryFunc<TVertex, IEnumerable<TEdge>> TryGetEdges { get; }


            public IEnumerable<TEdge> ShouldReturnEdges { get; set; }

            public bool ShouldReturnValue { get; set; }

            public void CheckCalls(int expectedCalls)
            {
                Assert.That(expectedCalls, Is.EqualTo(_nbCalls));
                _nbCalls = 0;
            }
        }

        #region Test helpers

        #region Contains Vertex

        protected static void ContainsVertex_Test(
            GraphData<int, Edge<int>> data,
            IImplicitVertexSet<int> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.That(graph.ContainsVertex(1), Is.False);
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.That(graph.ContainsVertex(1), Is.True);
            data.CheckCalls(1);
        }

        #endregion

        #region Contains Edge

        protected static void ContainsEdge_Test(
            GraphData<int, Edge<int>> data,
            IEdgeSet<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            var edge12 = new Edge<int>(1, 2);
            var edge21 = new Edge<int>(2, 1);
            Assert.That(graph.ContainsEdge(edge12), Is.False);
            data.CheckCalls(1);
            Assert.That(graph.ContainsEdge(edge21), Is.False);
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.That(graph.ContainsEdge(edge12), Is.False);
            data.CheckCalls(1);
            Assert.That(graph.ContainsEdge(edge21), Is.False);
            data.CheckCalls(1);

            var edge13 = new Edge<int>(1, 3);
            data.ShouldReturnEdges = [edge12, edge13, edge21];
            Assert.That(graph.ContainsEdge(edge12), Is.True);
            data.CheckCalls(1);
            Assert.That(graph.ContainsEdge(edge21), Is.True);
            data.CheckCalls(1);

            var edge15 = new Edge<int>(1, 5);
            var edge51 = new Edge<int>(5, 1);
            var edge56 = new Edge<int>(5, 6);
            Assert.That(graph.ContainsEdge(edge15), Is.False);
            Assert.That(graph.ContainsEdge(edge51), Is.False);
            Assert.That(graph.ContainsEdge(edge56), Is.False);
        }

        private static void ContainsEdge_SourceTarget_GenericTest(
            GraphData<int, Edge<int>> data,
            Func<int, int, bool> hasEdge,
            bool isDirected = true)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.That(hasEdge(1, 2), Is.False);
            data.CheckCalls(1);
            Assert.That(hasEdge(2, 1), Is.False);
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.That(hasEdge(1, 2), Is.False);
            data.CheckCalls(1);
            Assert.That(hasEdge(2, 1), Is.False);
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(1, 3), new Edge<int>(1, 2)];
            Assert.That(hasEdge(1, 2), Is.True);
            data.CheckCalls(1);
            if (isDirected)
                Assert.That(hasEdge(2, 1), Is.False);
            else
                Assert.That(hasEdge(2, 1), Is.True);
            data.CheckCalls(1);

            Assert.That(hasEdge(1, 5), Is.False);
            Assert.That(hasEdge(5, 1), Is.False);
            Assert.That(hasEdge(5, 6), Is.False);
        }

        protected static void ContainsEdge_SourceTarget_Test(
            GraphData<int, Edge<int>> data,
            IIncidenceGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                graph.ContainsEdge);
        }

        protected static void ContainsEdge_SourceTarget_UndirectedGraph_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                graph.ContainsEdge,
                false);
        }

        #endregion

        #region Out Edges

        protected static void OutEdge_Test(
            GraphData<int, Edge<int>> data,
            IImplicitGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);

            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = [edge11, edge12, edge13];
            Assert.That(edge11, Is.SameAs(graph.OutEdge(1, 0)));
            data.CheckCalls(1);

            Assert.That(edge13, Is.SameAs(graph.OutEdge(1, 2)));
            data.CheckCalls(1);
        }

        protected static void OutEdge_Throws_Test(
            GraphData<int, Edge<int>> data,
            IImplicitGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.OutEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            AssertIndexOutOfRange(() => graph.OutEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(1, 2)];
            AssertIndexOutOfRange(() => graph.OutEdge(1, 1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void OutEdges_Test(
            GraphData<int, Edge<int>> data,
            IImplicitGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            AssertNoOutEdge(graph, 1);
            data.CheckCalls(3);

            Edge<int>[] edges =
            [
                new Edge<int>(1, 2),
                new Edge<int>(1, 3)
            ];
            data.ShouldReturnEdges = edges;
            AssertHasOutEdges(graph, 1, edges);
            data.CheckCalls(3);
        }

        protected static void OutEdges_Throws_Test(
            GraphData<int, Edge<int>> data,
            IImplicitGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.IsOutEdgesEmpty(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.OutDegree(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.OutEdges(1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Adjacent Edges

        protected static void AdjacentEdge_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);

            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = [edge11, edge12, edge13];
            Assert.That(edge11, Is.SameAs(graph.AdjacentEdge(1, 0)));
            data.CheckCalls(1);

            Assert.That(edge13, Is.SameAs(graph.AdjacentEdge(1, 2)));
            data.CheckCalls(1);
        }

        protected static void AdjacentEdge_Throws_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.AdjacentEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            AssertIndexOutOfRange(() => graph.AdjacentEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(1, 2)];
            AssertIndexOutOfRange(() => graph.AdjacentEdge(1, 1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void AdjacentEdges_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            AssertNoAdjacentEdge(graph, 1);
            data.CheckCalls(3);

            Edge<int>[] edges =
            [
                new Edge<int>(1, 2),
                new Edge<int>(1, 3)
            ];
            data.ShouldReturnEdges = edges;
            AssertHasAdjacentEdges(graph, 1, edges);
            data.CheckCalls(3);
        }

        protected static void AdjacentEdges_Throws_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.IsAdjacentEdgesEmpty(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.AdjacentDegree(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.AdjacentEdges(1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Out Edges

        protected static void InEdge_Test(
            GraphData<int, Edge<int>> data,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            var edge11 = new Edge<int>(1, 1);
            var edge21 = new Edge<int>(2, 1);
            var edge31 = new Edge<int>(3, 1);

            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = [edge11, edge21, edge31];
            Assert.That(edge11, Is.SameAs(graph.InEdge(1, 0)));
            data.CheckCalls(1);

            Assert.That(edge31, Is.SameAs(graph.InEdge(1, 2)));
            data.CheckCalls(1);
        }

        protected static void InEdge_Throws_Test(
            GraphData<int, Edge<int>> data,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.InEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            AssertIndexOutOfRange(() => graph.InEdge(1, 0));
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(1, 2)];
            AssertIndexOutOfRange(() => graph.InEdge(1, 1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        protected static void InEdges_Test(
            GraphData<int, Edge<int>> data,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = true;
            AssertNoInEdge(graph, 1);
            data.CheckCalls(3);

            Edge<int>[] edges =
            [
                new Edge<int>(1, 2),
                new Edge<int>(1, 3)
            ];
            data.ShouldReturnEdges = edges;
            AssertHasInEdges(graph, 1, edges);
            data.CheckCalls(3);
        }

        protected static void InEdges_Throws_Test(
            GraphData<int, Edge<int>> data,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.IsInEdgesEmpty(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.InDegree(1));
            data.CheckCalls(1);

            Assert.Throws<VertexNotFoundException>(() => graph.InEdges(1));
            data.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Degree

        protected static void Degree_Test(
            GraphData<int, Edge<int>> data1,
            GraphData<int, Edge<int>> data2,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            data1.CheckCalls(0);
            data2.CheckCalls(0);

            data1.ShouldReturnValue = false;
            data2.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(1));
            data1.CheckCalls(0);
            data2.CheckCalls(1);

            data1.ShouldReturnValue = true;
            data2.ShouldReturnValue = false;
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(1));
            data1.CheckCalls(0);
            data2.CheckCalls(1);

            data1.ShouldReturnValue = false;
            data2.ShouldReturnValue = true;
            Assert.Throws<VertexNotFoundException>(() => graph.Degree(1));
            data1.CheckCalls(1);
            data2.CheckCalls(1);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            data1.ShouldReturnValue = true;
            data2.ShouldReturnValue = true;
            Assert.That(0, Is.EqualTo(graph.Degree(1)));

            data1.ShouldReturnEdges = [new Edge<int>(1, 2)];
            data2.ShouldReturnEdges = null;
            Assert.That(1, Is.EqualTo(graph.Degree(1)));

            data1.ShouldReturnEdges = null;
            data2.ShouldReturnEdges = [new Edge<int>(3, 1)];
            Assert.That(1, Is.EqualTo(graph.Degree(1)));

            data1.ShouldReturnEdges = [new Edge<int>(1, 2), new Edge<int>(1, 3)];
            data2.ShouldReturnEdges = [new Edge<int>(4, 1)];
            Assert.That(3, Is.EqualTo(graph.Degree(1)));

            // Self edge
            data1.ShouldReturnEdges = [new Edge<int>(1, 2), new Edge<int>(1, 3), new Edge<int>(1, 1)];
            data2.ShouldReturnEdges = [new Edge<int>(4, 1), new Edge<int>(1, 1)];
            Assert.That(5, Is.EqualTo(graph.Degree(1)));
        }

        #endregion

        #region Try Get Edges

        protected static void TryGetEdge_Test(
            GraphData<int, Edge<int>> data,
            IIncidenceGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                (source, target) => graph.TryGetEdge(source, target, out _));
        }

        protected static void TryGetEdge_UndirectedGraph_Test(
            GraphData<int, Edge<int>> data,
            IImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            ContainsEdge_SourceTarget_GenericTest(
                data,
                (source, target) => graph.TryGetEdge(source, target, out _),
                false);
        }

        protected static void TryGetEdges_Test(
            GraphData<int, Edge<int>> data,
            IIncidenceGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.That(graph.TryGetEdges(0, 1, out _), Is.False);
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.That(graph.TryGetEdges(1, 2, out IEnumerable<Edge<int>> edges), Is.True);
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(1, 2), new Edge<int>(1, 2)];
            Assert.That(graph.TryGetEdges(1, 2, out edges), Is.True);
            CollectionAssert.AreEqual(data.ShouldReturnEdges,edges);
            data.CheckCalls(1);
        }

        protected static void TryGetEdges_Test(
            GraphData<int, Edge<int>> data,
            DelegateVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.That(graph.TryGetEdges(0, 1, out _), Is.False);
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            Assert.That(graph.TryGetEdges(1, 2, out IEnumerable<Edge<int>> edges), Is.True);
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(1, 2), new Edge<int>(1, 2)];
            Assert.That(graph.TryGetEdges(1, 2, out edges), Is.True);
            CollectionAssert.AreEqual(data.ShouldReturnEdges,edges);
            data.CheckCalls(1);

            var edge14 = new Edge<int>(1, 4);
            var edge12 = new Edge<int>(1, 2);
            var edge12Bis = new Edge<int>(1, 2);
            data.ShouldReturnValue = true;
            data.ShouldReturnEdges = [edge14, edge12];
            Assert.That(graph.TryGetEdges(1, 2, out edges), Is.True);
            CollectionAssert.AreEqual(new[] { edge12 }, edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = [edge14, edge12, edge12Bis];
            Assert.That(graph.TryGetEdges(1, 2, out edges), Is.True);
            CollectionAssert.AreEqual(new[] { edge12, edge12Bis }, edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = [edge14, edge12];
            Assert.That(graph.TryGetEdges(2, 1, out edges), Is.True);
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            var edge41 = new Edge<int>(4, 1);
            data.ShouldReturnEdges = [edge14, edge41];
            Assert.That(graph.TryGetEdges(1, 4, out edges), Is.True);
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            Assert.That(graph.TryGetEdges(4, 1, out _), Is.False);
            data.CheckCalls(0);

            var edge45 = new Edge<int>(4, 5);
            data.ShouldReturnEdges = [edge14, edge41, edge45];
            Assert.That(graph.TryGetEdges(4, 5, out _), Is.False);
            data.CheckCalls(0);
        }

        protected static void TryGetOutEdges_Test(
            GraphData<int, Edge<int>> data,
            IImplicitGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.That(graph.TryGetOutEdges(1, out _), Is.False);
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.That(graph.TryGetOutEdges(1, out IEnumerable<Edge<int>> edges), Is.True);
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(1, 4), new Edge<int>(1, 2)];
            Assert.That(graph.TryGetOutEdges(1, out edges), Is.True);
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);
        }

        protected static void TryGetOutEdges_Test(
            GraphData<int, Edge<int>> data,
            DelegateVertexAndEdgeListGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.That(graph.TryGetOutEdges(5, out _), Is.False);
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            Assert.That(graph.TryGetOutEdges(1, out IEnumerable<Edge<int>> edges), Is.True);
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(1, 4), new Edge<int>(1, 2)];
            Assert.That(graph.TryGetOutEdges(1, out edges), Is.True);
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = null;
            Assert.That(graph.TryGetOutEdges(1, out IEnumerable<Edge<int>> outEdges), Is.True);
            CollectionAssert.IsEmpty(outEdges);
            data.CheckCalls(1);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge15 = new Edge<int>(1, 5);
            var edge21 = new Edge<int>(2, 1);
            var edge23 = new Edge<int>(2, 3);
            data.ShouldReturnEdges = [edge12, edge13, edge15, edge21, edge23];
            Assert.That(graph.TryGetOutEdges(1, out outEdges), Is.True);
            CollectionAssert.AreEqual(
                new[] { edge12, edge13 },
                outEdges);
            data.CheckCalls(1);

            var edge52 = new Edge<int>(5, 2);
            data.ShouldReturnEdges = [edge15, edge52];
            Assert.That(graph.TryGetOutEdges(5, out _), Is.False);
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
        }

        protected static void TryGetAdjacentEdges_Test(
            GraphData<int, Edge<int>> data,
            DelegateImplicitUndirectedGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.That(graph.TryGetAdjacentEdges(1, out _), Is.False);
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.That(graph.TryGetAdjacentEdges(1, out IEnumerable<Edge<int>> edges), Is.True);
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(1, 4), new Edge<int>(1, 2)];
            Assert.That(graph.TryGetAdjacentEdges(1, out edges), Is.True);
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);
        }

        protected static void TryGetAdjacentEdges_Test(
            GraphData<int, Edge<int>> data,
            DelegateUndirectedGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.That(graph.TryGetAdjacentEdges(5, out _), Is.False);
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code

            data.ShouldReturnValue = true;
            Assert.That(graph.TryGetAdjacentEdges(1, out IEnumerable<Edge<int>> edges), Is.True);
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(1, 4), new Edge<int>(1, 2)];
            Assert.That(graph.TryGetAdjacentEdges(1, out edges), Is.True);
            CollectionAssert.AreEqual(data.ShouldReturnEdges, (edges));
            data.CheckCalls(1);

            data.ShouldReturnEdges = null;
            Assert.That(graph.TryGetAdjacentEdges(1, out IEnumerable<Edge<int>> adjacentEdges), Is.True);
            CollectionAssert.IsEmpty(adjacentEdges);
            data.CheckCalls(1);

            var edge12 = new Edge<int>(1, 2);
            var edge13 = new Edge<int>(1, 3);
            var edge15 = new Edge<int>(1, 5);
            var edge21 = new Edge<int>(2, 1);
            var edge23 = new Edge<int>(2, 3);
            data.ShouldReturnEdges = [edge12, edge13, edge15, edge21, edge23];
            Assert.That(graph.TryGetAdjacentEdges(1, out adjacentEdges), Is.True);
            CollectionAssert.AreEqual(
                new[] { edge12, edge13, edge21 },
                adjacentEdges);
            data.CheckCalls(1);

            var edge52 = new Edge<int>(5, 2);
            data.ShouldReturnEdges = [edge15, edge52];
            Assert.That(graph.TryGetAdjacentEdges(5, out _), Is.False);
            data.CheckCalls(0); // Vertex is not in graph so no need to call user code
        }

        protected static void TryGetAdjacentEdges_Throws_Test<TVertex, TEdge>(
            DelegateImplicitUndirectedGraph<TVertex, TEdge> graph)
            where TVertex : class
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.TryGetAdjacentEdges(null, out _));
        }

        protected static void TryGetInEdges_Test(
            GraphData<int, Edge<int>> data,
            IBidirectionalIncidenceGraph<int, Edge<int>> graph)
        {
            data.CheckCalls(0);

            data.ShouldReturnValue = false;
            Assert.That(graph.TryGetInEdges(1, out _), Is.False);
            data.CheckCalls(1);

            data.ShouldReturnValue = true;
            Assert.That(graph.TryGetInEdges(1, out IEnumerable<Edge<int>> edges), Is.True);
            CollectionAssert.IsEmpty(edges);
            data.CheckCalls(1);

            data.ShouldReturnEdges = [new Edge<int>(4, 1), new Edge<int>(2, 1)];
            Assert.That(graph.TryGetInEdges(1, out edges), Is.True);
            CollectionAssert.AreEqual(data.ShouldReturnEdges, edges);
            data.CheckCalls(1);
        }

        #endregion

        #endregion
    }
}