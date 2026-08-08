using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace QuikGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="Transition{TToken}"/>.
    /// </summary>
    internal sealed class TransitionTests
    {
        #region Test classes

        private class AlwaysFalseCondition : IConditionExpression<int>
        {
            public bool IsEnabled(IList<int> tokens)
            {
                return false;
            }
        }

        #endregion

        [Test]
        public void Constructor()
        {
            var transition = new Transition<int>("MyTransition");
            Assert.That("MyTransition", Is.EqualTo(transition.Name));
            Assert.That(transition.Condition, Is.InstanceOf<AlwaysTrueConditionExpression<int>>());
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new Transition<int>(null));
        }

        [Test]
        public void Condition()
        {
            var transition = new Transition<int>("MyTransition");
            Assert.That(transition.Condition, Is.Not.Null);

            var newCondition = new AlwaysFalseCondition();
            transition.Condition = newCondition;
            Assert.That(newCondition, Is.SameAs(transition.Condition));

            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => transition.Condition = null);
        }

        [Test]
        public void ObjectToString()
        {
            var transition = new Transition<int>("TestName");
            Assert.That("T(TestName)", Is.EqualTo(transition.ToString()));

            transition = new Transition<int>("OtherTestName");
            Assert.That("T(OtherTestName)", Is.EqualTo(transition.ToString()));
        }
    }
}