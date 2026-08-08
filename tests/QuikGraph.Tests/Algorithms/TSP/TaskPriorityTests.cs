using NUnit.Framework;
using QuikGraph.Algorithms.TSP;

namespace QuikGraph.Tests.Algorithms.TSP
{
    /// <summary>
    /// Tests for <see cref="TaskPriority"/>.
    /// </summary>
    [TestFixture]
    internal sealed class TaskPriorityTests
    {
        [Test]
        public void Constructor()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new TaskPriority(10.0, 5));
        }

        [Test]
        public void Equals()
        {
            var priority1 = new TaskPriority(1.0, 2);
            var priority2 = new TaskPriority(1.0, 2);
            var priority3 = new TaskPriority(2.0, 2);
            var priority4 = new TaskPriority(1.0, 1);
            var priority5 = new TaskPriority(2.0, 1);

            Assert.That(priority1,Is.EqualTo(priority1));
            Assert.That(priority1,Is.EqualTo(priority2));
            Assert.That(priority1 == priority2,Is.True);
            Assert.That(priority2 == priority1,Is.True);
            Assert.That(priority1 != priority2,Is.False);
            Assert.That(priority2 != priority1,Is.False);

            Assert.That(priority1,Is.Not.EqualTo(priority3));
            Assert.That(priority1 == priority3,Is.False);
            Assert.That(priority3 == priority1,Is.False);
            Assert.That(priority1 != priority3,Is.True);
            Assert.That(priority3 != priority1,Is.True);

            Assert.That(priority1,Is.Not.EqualTo(priority4));
            Assert.That(priority1 == priority4,Is.False);
            Assert.That(priority4 == priority1,Is.False);
            Assert.That(priority1 != priority4,Is.True);
            Assert.That(priority4 != priority1,Is.True);

            Assert.That(priority1,Is.Not.EqualTo(priority5));
            Assert.That(priority1 == priority5,Is.False);
            Assert.That(priority5 == priority1,Is.False);
            Assert.That(priority1 != priority5,Is.True);
            Assert.That(priority5 != priority1,Is.True);

            Assert.That(priority1,Is.Not.Null);
            Assert.That(priority1.Equals(null),Is.False);
            Assert.That(priority1 == null,Is.False);
            Assert.That(null == priority1,Is.False);
            Assert.That(priority1 != null,Is.True);
            Assert.That(null != priority1,Is.True);
        }

        [Test]
        public void Hashcode()
        {
            var priority1 = new TaskPriority(1.0, 2);
            var priority2 = new TaskPriority(1.0, 2);
            var priority3 = new TaskPriority(2.0, 2);

            Assert.That(priority1.GetHashCode(),Is.EqualTo(priority2.GetHashCode()));
            Assert.That(priority1.GetHashCode(),Is.Not.EqualTo(priority3.GetHashCode()));
        }

        [Test]
        public void Comparison()
        {
            var priority1 = new TaskPriority(1.0, 2);
            var priority2 = new TaskPriority(1.0, 2);
            var priority3 = new TaskPriority(2.0, 2);
            var priority4 = new TaskPriority(1.0, 1);

            Assert.That(priority1 < priority2,Is.False);
            Assert.That(priority1 <= priority2,Is.True);
            Assert.That(priority1 > priority2,Is.False);
            Assert.That(priority1 >= priority2,Is.True);

            Assert.That(priority1 < priority3,Is.True);
            Assert.That(priority1 <= priority3,Is.True);
            Assert.That(priority1 > priority3,Is.False);
            Assert.That(priority1 >= priority3,Is.False);

            Assert.That(priority1 < priority4,Is.True);
            Assert.That(priority1 <= priority4,Is.True);
            Assert.That(priority1 > priority4,Is.False);
            Assert.That(priority1 >= priority4,Is.False);

            Assert.That(priority1 < null,Is.False);
            Assert.That(priority1 <= null,Is.False);
            Assert.That(priority1 > null,Is.True);
            Assert.That(priority1 >= null,Is.True);
        }
    }
}