using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="Arc{TToken}"/>.
    /// </summary>
    internal sealed class ArcTests
    {
        #region Test classes

        private class TestPlace : IPlace<int>
        {
            public string Name => "PlaceName";
            public IList<int> Marking { get; } = new List<int>();

            public string ToStringWithMarking()
            {
                return string.Empty;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private class TestTransition : ITransition<int>
        {
            public string Name => "TransitionName";
            public IConditionExpression<int> Condition { get; set; } = new AlwaysTrueConditionExpression<int>();

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var place = new TestPlace();
            var transition = new TestTransition();

            var arc = new Arc<int>(place, transition);
            Assert.That(arc.IsInputArc, Is.True);
            Assert.That(place, Is.SameAs(arc.Source));
            Assert.That(place, Is.SameAs(arc.Place));
            Assert.That(transition, Is.SameAs(arc.Target));
            Assert.That(transition, Is.SameAs(arc.Transition));
            Assert.That(arc.Annotation, Is.Not.Null);

            arc = new Arc<int>(transition, place);
            Assert.That(arc.IsInputArc, Is.False);
            Assert.That(place, Is.SameAs(arc.Source));
            Assert.That(place, Is.SameAs(arc.Place));
            Assert.That(transition, Is.SameAs(arc.Target));
            Assert.That(transition, Is.SameAs(arc.Transition));
            Assert.That(arc.Annotation, Is.Not.Null);
        }

        [Test]
        public void Constructor_Throws()
        {
            var place = new TestPlace();
            var transition = new TestTransition();

            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new Arc<int>(null, transition));
            Assert.Throws<ArgumentNullException>(() => new Arc<int>(place, null));
            Assert.Throws<ArgumentNullException>(() => new Arc<int>((IPlace<int>)null, null));

            Assert.Throws<ArgumentNullException>(() => new Arc<int>(null, place));
            Assert.Throws<ArgumentNullException>(() => new Arc<int>(transition, null));
            Assert.Throws<ArgumentNullException>(() => new Arc<int>((ITransition<int>)null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void ObjectToString()
        {
            var place = new TestPlace();
            var transition = new TestTransition();

            var arc = new Arc<int>(place, transition);
            Assert.That("PlaceName -> TransitionName", Is.EqualTo(arc.ToString()));

            arc = new Arc<int>(transition, place);
            Assert.That("TransitionName -> PlaceName", Is.EqualTo(arc.ToString()));
        }
    }
}