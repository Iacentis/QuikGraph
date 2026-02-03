using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Algorithms.Exploration;
using QuikGraph.Tests.Structures;
using static QuikGraph.Tests.AssertHelpers;
using static QuikGraph.Tests.GraphTestHelpers;

namespace QuikGraph.Tests.Algorithms.Exploration
{
    /// <summary>
    /// Tests for <see cref="TransitionFactoryImplicitGraph{TVertex,TEdge}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class TransitionFactoryImplicitGraphTests : GraphTestsBase
    {
        [Test]
        public void Construction()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            AssertGraphProperties(graph);

            #region Local function

            void AssertGraphProperties<TVertex, TEdge>(
                // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
                TransitionFactoryImplicitGraph<TVertex, TEdge> g)
                where TVertex : ICloneable
                where TEdge : IEdge<TVertex>
            {
                Assert.That(g.IsDirected, Is.True);
                Assert.That(g.AllowParallelEdges, Is.True);
                Assert.That(g.SuccessorVertexPredicate, Is.Not.Null);
                Assert.That(g.SuccessorEdgePredicate, Is.Not.Null);
            }

            #endregion
        }

        [Test]
        public void Constructor_Throws()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.SuccessorVertexPredicate = null);
            Assert.Throws<ArgumentNullException>(() => graph.SuccessorEdgePredicate = null);
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #region Factory manipulations

        [Test]
        public void AddTransitionFactory()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, []);
            graph.AddTransitionFactory(factory1);

            Assert.That(graph.ContainsTransitionFactory(factory1), Is.True);

            var vertex2 = new CloneableTestVertex("2");
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, []);
            graph.AddTransitionFactory(factory2);

            Assert.That(graph.ContainsTransitionFactory(factory2), Is.True);

            graph.AddTransitionFactory(factory1);

            Assert.That(graph.ContainsTransitionFactory(factory1), Is.True);
        }

        [Test]
        public void AddTransitionFactory_Throws()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddTransitionFactory(null));
        }

        [Test]
        public void AddTransitionFactories()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, []);
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, []);
            graph.AddTransitionFactories(new[] { factory1, factory2 });

            Assert.That(graph.ContainsTransitionFactory(factory1), Is.True);
            Assert.That(graph.ContainsTransitionFactory(factory2), Is.True);

            var vertex3 = new CloneableTestVertex("3");
            var factory3 = new TestTransitionFactory<CloneableTestVertex>(vertex3, []);
            graph.AddTransitionFactory(factory3);

            Assert.That(graph.ContainsTransitionFactory(factory1), Is.True);
            Assert.That(graph.ContainsTransitionFactory(factory2), Is.True);
            Assert.That(graph.ContainsTransitionFactory(factory3), Is.True);
        }

        [Test]
        public void AddTransitionFactories_Throws()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.AddTransitionFactories(null));
        }

        [Test]
        public void RemoveTransitionFactories()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            Assert.That(graph.RemoveTransitionFactory(null), Is.False);

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, []);
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, []);
            var factory3 = new TestTransitionFactory<CloneableTestVertex>(vertex3, []);
            graph.AddTransitionFactories(new[] { factory1, factory2 });

            Assert.That(graph.ContainsTransitionFactory(null), Is.False);
            Assert.That(graph.ContainsTransitionFactory(factory1), Is.True);
            Assert.That(graph.ContainsTransitionFactory(factory2), Is.True);

            Assert.That(graph.RemoveTransitionFactory(factory3), Is.False);
            Assert.That(graph.RemoveTransitionFactory(factory1), Is.True);
            Assert.That(graph.RemoveTransitionFactory(factory1), Is.False);
            Assert.That(graph.RemoveTransitionFactory(factory2), Is.True);

            var factory4 = new TestTransitionFactory<CloneableTestVertex>(
                vertex1,
                [
                    new Edge<CloneableTestVertex>(vertex1, vertex2),
                    new Edge<CloneableTestVertex>(vertex1, vertex3)
                ]);
            graph.AddTransitionFactory(factory4);
            Assert.That(graph.ContainsTransitionFactory(factory4), Is.True);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(vertex1); // Force exploration from vertex1

            Assert.That(graph.RemoveTransitionFactory(factory4), Is.True);
        }

        [Test]
        public void ContainsTransitionFactories()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, []);

            Assert.That(graph.ContainsTransitionFactory(null), Is.False);
            Assert.That(graph.ContainsTransitionFactory(factory1), Is.False);

            graph.AddTransitionFactory(factory1);

            Assert.That(graph.ContainsTransitionFactory(null), Is.False);
            Assert.That(graph.ContainsTransitionFactory(factory1), Is.True);

            var vertex2 = new CloneableTestVertex("2");
            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, []);
            graph.AddTransitionFactory(factory2);

            Assert.That(graph.ContainsTransitionFactory(null), Is.False);
            Assert.That(graph.ContainsTransitionFactory(factory1), Is.True);
            Assert.That(graph.ContainsTransitionFactory(factory2), Is.True);

            graph.RemoveTransitionFactory(factory1);

            Assert.That(graph.ContainsTransitionFactory(null), Is.False);
            Assert.That(graph.ContainsTransitionFactory(factory1), Is.False);
            Assert.That(graph.ContainsTransitionFactory(factory2), Is.True);
        }

        [Test]
        public void ClearTransitionFactories()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");

            var edge11 = new Edge<CloneableTestVertex>(vertex1, vertex1);
            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge23 = new Edge<CloneableTestVertex>(vertex2, vertex3);
            var edge33 = new Edge<CloneableTestVertex>(vertex3, vertex3);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>([
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, [edge11, edge12, edge13]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, [edge23])
                ]));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex3, [edge33]));

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed => trigger caching of edges
            graph.OutEdges(vertex1);
            graph.OutEdges(vertex2);
            graph.OutEdges(vertex3);
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            AssertHasVertices(graph, new[] { vertex1, vertex2, vertex3 });

            graph.ClearTransitionFactories();

            AssertNoVertices(graph, new[] { vertex1, vertex2, vertex3 });
        }

        #endregion

        #region Contains Vertex

        [Test]
        public void ContainsVertex()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var otherVertex1 = new CloneableTestVertex("1");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");

            var edge34 = new Edge<CloneableTestVertex>(vertex3, vertex4);

            Assert.That(graph.ContainsVertex(vertex1), Is.False);
            Assert.That(graph.ContainsVertex(vertex2), Is.False);
            Assert.That(graph.ContainsVertex(otherVertex1), Is.False);
            Assert.That(graph.ContainsVertex(vertex3), Is.False);
            Assert.That(graph.ContainsVertex(vertex4), Is.False);

            var factory1 = new TestTransitionFactory<CloneableTestVertex>(vertex1, []);
            graph.AddTransitionFactory(factory1);
            Assert.That(graph.ContainsVertex(vertex1), Is.False); // Not explored yet
            Assert.That(graph.ContainsVertex(vertex2), Is.False);
            Assert.That(graph.ContainsVertex(otherVertex1), Is.False);
            Assert.That(graph.ContainsVertex(vertex3), Is.False);
            Assert.That(graph.ContainsVertex(vertex4), Is.False);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(vertex1);

            Assert.That(graph.ContainsVertex(vertex1), Is.True);
            Assert.That(graph.ContainsVertex(vertex2), Is.False);
            Assert.That(graph.ContainsVertex(otherVertex1), Is.False);
            Assert.That(graph.ContainsVertex(vertex3), Is.False);
            Assert.That(graph.ContainsVertex(vertex4), Is.False);

            var factory2 = new TestTransitionFactory<CloneableTestVertex>(vertex2, []);
            graph.AddTransitionFactory(factory2);
            Assert.That(graph.ContainsVertex(vertex1), Is.True);
            Assert.That(graph.ContainsVertex(vertex2), Is.False); // Not explored yet
            Assert.That(graph.ContainsVertex(otherVertex1), Is.False);
            Assert.That(graph.ContainsVertex(vertex3), Is.False);
            Assert.That(graph.ContainsVertex(vertex4), Is.False);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(vertex2);

            Assert.That(graph.ContainsVertex(vertex1), Is.True);
            Assert.That(graph.ContainsVertex(vertex2), Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1), Is.False);
            Assert.That(graph.ContainsVertex(vertex3), Is.False);
            Assert.That(graph.ContainsVertex(vertex4), Is.False);

            var factoryOther1 = new TestTransitionFactory<CloneableTestVertex>(otherVertex1, []);
            graph.AddTransitionFactory(factoryOther1);
            Assert.That(graph.ContainsVertex(vertex1), Is.True);
            Assert.That(graph.ContainsVertex(vertex2), Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1), Is.False); // Not explored yet
            Assert.That(graph.ContainsVertex(vertex3), Is.False);
            Assert.That(graph.ContainsVertex(vertex4), Is.False);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(otherVertex1);

            Assert.That(graph.ContainsVertex(vertex1), Is.True);
            Assert.That(graph.ContainsVertex(vertex2), Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1), Is.True);
            Assert.That(graph.ContainsVertex(vertex3), Is.False);
            Assert.That(graph.ContainsVertex(vertex4), Is.False);

            var factory3 = new TestTransitionFactory<CloneableTestVertex>(vertex3, [edge34]);
            graph.AddTransitionFactory(factory3);
            Assert.That(graph.ContainsVertex(vertex1), Is.True);
            Assert.That(graph.ContainsVertex(vertex2), Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1), Is.True);
            Assert.That(graph.ContainsVertex(vertex3), Is.False); // Not explored yet
            Assert.That(graph.ContainsVertex(vertex4), Is.False);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(vertex3);

            Assert.That(graph.ContainsVertex(vertex1), Is.True);
            Assert.That(graph.ContainsVertex(vertex2), Is.True);
            Assert.That(graph.ContainsVertex(otherVertex1), Is.True);
            Assert.That(graph.ContainsVertex(vertex3), Is.True);
            Assert.That(graph.ContainsVertex(vertex4), Is.True); // Discovered when requesting vertex3
        }

        [Test]
        public void ContainsVertex_Throws()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            ContainsVertex_Throws_Test(graph);
        }

        #endregion

        #region Out Edges

        [Test]
        public void OutEdge()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");

            var edge11 = new Edge<CloneableTestVertex>(vertex1, vertex1);
            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge24 = new Edge<CloneableTestVertex>(vertex2, vertex4);
            var edge33 = new Edge<CloneableTestVertex>(vertex3, vertex3);
            var edge41 = new Edge<CloneableTestVertex>(vertex4, vertex1);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>([
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, [edge11, edge12]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, [edge24]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex3, [edge33])
                ]));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, [edge13]));
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex4, [edge41]));

            Assert.That(edge11, Is.SameAs(graph.OutEdge(vertex1, 0)));
            Assert.That(edge13, Is.SameAs(graph.OutEdge(vertex1, 2)));
            Assert.That(edge24, Is.SameAs(graph.OutEdge(vertex2, 0)));
            Assert.That(edge33, Is.SameAs(graph.OutEdge(vertex3, 0)));
            Assert.That(edge41, Is.SameAs(graph.OutEdge(vertex4, 0)));
            Assert.That(edge41, Is.SameAs(graph.OutEdge(vertex4, 0)));
        }

        [Test]
        public void OutEdge_WithFilter()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");
            var vertex5 = new CloneableTestVertex("5");
            var vertex6 = new CloneableTestVertex("6");
            var vertex7 = new CloneableTestVertex("7");

            var edge11 = new Edge<CloneableTestVertex>(vertex1, vertex1);
            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge54 = new Edge<CloneableTestVertex>(vertex5, vertex4);
            var edge61 = new Edge<CloneableTestVertex>(vertex6, vertex1);
            var edge67 = new Edge<CloneableTestVertex>(vertex6, vertex7);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>([
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, [edge11, edge12, edge13]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex5, [edge54]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex6, [edge61, edge67])
                ]));

            graph.SuccessorVertexPredicate = vertex => vertex != vertex4;
            graph.SuccessorEdgePredicate = edge => edge != edge61;

            Assert.That(edge11, Is.SameAs(graph.OutEdge(vertex1, 0)));
            Assert.That(edge13, Is.SameAs(graph.OutEdge(vertex1, 2)));
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            AssertIndexOutOfRange(() => graph.OutEdge(vertex5, 0)); // Filtered
            Assert.That(edge67, Is.SameAs(graph.OutEdge(vertex6, 0))); // Because of the filter
            AssertIndexOutOfRange(() => graph.OutEdge(vertex6, 1)); // Filtered
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            // Restore no filter
            graph.SuccessorVertexPredicate = _ => true;
            graph.SuccessorEdgePredicate = _ => true;

            Assert.That(edge54, Is.SameAs(graph.OutEdge(vertex5, 0)));
            Assert.That(edge61, Is.SameAs(graph.OutEdge(vertex6, 0)));
            Assert.That(edge67, Is.SameAs(graph.OutEdge(vertex6, 1)));
        }

        [Test]
        public void OutEdge_Throws()
        {
            var graph1 = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            OutEdge_NullThrows_Test(graph1);

            var graph2 = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<VertexNotFoundException>(() => graph2.OutEdge(vertex1, 0));

            var factory1 = new TestTransitionFactory<CloneableTestVertex>(
                vertex1,
                []);
            graph2.AddTransitionFactory(factory1);
            graph2.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex2, []));
            AssertIndexOutOfRange(() => graph2.OutEdge(vertex1, 0));

            graph2.RemoveTransitionFactory(factory1);
            graph2.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, [
                    new Edge<CloneableTestVertex>(vertex1, vertex2)
                ]));
            AssertIndexOutOfRange(() => graph2.OutEdge(vertex1, 5));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void OutEdges()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");

            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge14 = new Edge<CloneableTestVertex>(vertex1, vertex4);
            var edge24 = new Edge<CloneableTestVertex>(vertex2, vertex4);
            var edge31 = new Edge<CloneableTestVertex>(vertex3, vertex1);
            var edge33 = new Edge<CloneableTestVertex>(vertex3, vertex3);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, []));
            AssertNoOutEdge(graph, vertex1);

            graph.ClearTransitionFactories();
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>([
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, [edge12, edge13]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, [edge24]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex3, [edge31, edge33])
                ]));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, [edge14]));
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex4, []));

            AssertHasOutEdges(graph, vertex1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, vertex2, new[] { edge24 });
            AssertHasOutEdges(graph, vertex3, new[] { edge31, edge33 });
            AssertNoOutEdge(graph, vertex4);
        }

        [Test]
        public void OutEdges_WithFilter()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");

            var edge12 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge13 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge14 = new Edge<CloneableTestVertex>(vertex1, vertex4);
            var edge24 = new Edge<CloneableTestVertex>(vertex2, vertex4);
            var edge31 = new Edge<CloneableTestVertex>(vertex3, vertex1);
            var edge33 = new Edge<CloneableTestVertex>(vertex3, vertex3);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, []));
            AssertNoOutEdge(graph, vertex1);

            graph.ClearTransitionFactories();
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>([
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, [edge12, edge13]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, [edge24]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex3, [edge31, edge33])
                ]));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, [edge14]));
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex4, []));

            graph.SuccessorVertexPredicate = vertex => vertex != vertex2;
            graph.SuccessorEdgePredicate = edge => edge.Source != edge.Target;

            AssertHasOutEdges(graph, vertex1, new[] { edge13, edge14 }); // Filtered
            AssertHasOutEdges(graph, vertex2, new[] { edge24 });
            AssertHasOutEdges(graph, vertex3, new[] { edge31 }); // Filtered
            AssertNoOutEdge(graph, vertex4);

            // Restore no filter
            graph.SuccessorVertexPredicate = _ => true;
            graph.SuccessorEdgePredicate = _ => true;

            AssertHasOutEdges(graph, vertex1, new[] { edge12, edge13, edge14 });
            AssertHasOutEdges(graph, vertex2, new[] { edge24 });
            AssertHasOutEdges(graph, vertex3, new[] { edge31, edge33 });
            AssertNoOutEdge(graph, vertex4);
        }

        [Test]
        public void OutEdges_Throws()
        {
            var graph1 = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            OutEdges_NullThrows_Test(graph1);

            var graph2 =
                new TransitionFactoryImplicitGraph<EquatableCloneableTestVertex, Edge<EquatableCloneableTestVertex>>();
            OutEdges_Throws_Test(graph2);
        }

        #endregion

        #region Try Get Edges

        [Test]
        public void TryGetOutEdges()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex0 = new CloneableTestVertex("0");
            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");
            var vertex5 = new CloneableTestVertex("5");

            var edge1 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge2 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge3 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge4 = new Edge<CloneableTestVertex>(vertex2, vertex2);
            var edge5 = new Edge<CloneableTestVertex>(vertex2, vertex4);
            var edge6 = new Edge<CloneableTestVertex>(vertex3, vertex1);
            var edge7 = new Edge<CloneableTestVertex>(vertex4, vertex5);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, []));
            AssertNoOutEdge(graph, vertex1);

            graph.ClearTransitionFactories();
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>([
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, [edge1, edge2, edge3]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, [edge4]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex3, [edge6])
                ]));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex2, [edge5]));
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex4, [edge7]));

            Assert.That(graph.TryGetOutEdges(vertex0, out _), Is.False);

            Assert.That(graph.TryGetOutEdges(vertex5, out _), Is.False); // Vertex5 was not discovered

            Assert.That(graph.TryGetOutEdges(vertex3, out IEnumerable<Edge<CloneableTestVertex>> gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge6 }, gotEdges);

            Assert.That(graph.TryGetOutEdges(vertex1, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge1, edge2, edge3 }, gotEdges);

            // Trigger discover of vertex5
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            graph.OutEdges(vertex4);

            Assert.That(graph.TryGetOutEdges(vertex5, out gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges);
        }

        [Test]
        public void TryGetOutEdges_WithFilter()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();

            var vertex1 = new CloneableTestVertex("1");
            var vertex2 = new CloneableTestVertex("2");
            var vertex3 = new CloneableTestVertex("3");
            var vertex4 = new CloneableTestVertex("4");
            var vertex5 = new CloneableTestVertex("5");

            var edge1 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge2 = new Edge<CloneableTestVertex>(vertex1, vertex2);
            var edge3 = new Edge<CloneableTestVertex>(vertex1, vertex3);
            var edge4 = new Edge<CloneableTestVertex>(vertex2, vertex2);
            var edge5 = new Edge<CloneableTestVertex>(vertex2, vertex4);
            var edge6 = new Edge<CloneableTestVertex>(vertex3, vertex1);
            var edge7 = new Edge<CloneableTestVertex>(vertex4, vertex5);

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex1, []));
            AssertNoOutEdge(graph, vertex1);

            graph.ClearTransitionFactories();
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>([
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex1, [edge1, edge2, edge3]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex2, [edge4]),
                    new TestTransitionFactory<CloneableTestVertex>.VertexEdgesSet(vertex3, [edge6])
                ]));

            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex2, [edge5]));
            graph.AddTransitionFactory(
                new TestTransitionFactory<CloneableTestVertex>(vertex4, [edge7]));

            graph.SuccessorVertexPredicate = vertex => vertex != vertex4;
            graph.SuccessorEdgePredicate = edge => edge.Source != edge.Target;

            Assert.That(graph.TryGetOutEdges(vertex2, out IEnumerable<Edge<CloneableTestVertex>> gotEdges), Is.True);
            CollectionAssert.IsEmpty(gotEdges); // Both edges filtered by the 2 filters combined

            // Restore no filter
            graph.SuccessorVertexPredicate = _ => true;
            graph.SuccessorEdgePredicate = _ => true;

            Assert.That(graph.TryGetOutEdges(vertex2, out gotEdges), Is.True);
            CollectionAssert.AreEqual(new[] { edge4, edge5 }, gotEdges);
        }

        [Test]
        public void TryGetOutEdges_Throws()
        {
            var graph = new TransitionFactoryImplicitGraph<CloneableTestVertex, Edge<CloneableTestVertex>>();
            TryGetOutEdges_Throws_Test(graph);
        }

        #endregion
    }
}