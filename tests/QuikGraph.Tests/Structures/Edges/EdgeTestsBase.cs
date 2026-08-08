using NUnit.Framework;

namespace QuikGraph.Tests.Structures
{
    /// <summary>
    /// Base class for tests relative to edges structures.
    ///</summary>
    internal abstract class EdgeTestsBase
    {
        private static void AssertAreEqual<T>( T left,  T right)
        {
            bool isValueType = typeof(T).IsValueType;
            if (isValueType)
                Assert.That(left,Is.EqualTo(right));
            else
                Assert.That(left,Is.SameAs(right));
        }

        protected static void CheckStructEdge<T>( IEdge<T> edge, T source, T target)
        {
            AssertAreEqual(source, edge.Source);
            AssertAreEqual(target, edge.Target);
        }

        protected static void CheckEdge<T>( IEdge<T> edge,  T source,  T target)
        {
            Assert.That(edge.Source,Is.Not.Null);
            AssertAreEqual(source, edge.Source);

            Assert.That(edge.Target,Is.Not.Null);
            AssertAreEqual(target, edge.Target);
        }

        protected static void CheckTermEdge<T>( ITermEdge<T> edge,  T source,  T target, int sourceTerm, int targetTerm)
        {
            CheckEdge(edge, source, target);
            Assert.That(sourceTerm,Is.EqualTo(edge.SourceTerminal));
            Assert.That(targetTerm,Is.EqualTo(edge.TargetTerminal));
        }

        protected static void CheckTaggedEdge<TVertex, TEdge, TTag>( TEdge edge,  TVertex source,  TVertex target,  TTag tag)
            where TEdge : IEdge<TVertex>, ITagged<TTag>
        {
            CheckEdge(edge, source, target);
            AssertAreEqual(tag, edge.Tag);
        }

        protected static void CheckStructTaggedEdge<TVertex, TEdge, TTag>( TEdge edge, TVertex source, TVertex target,  TTag tag)
            where TEdge : IEdge<TVertex>, ITagged<TTag>
        {
            CheckStructEdge(edge, source, target);
            AssertAreEqual(tag, edge.Tag);
        }
    }
}