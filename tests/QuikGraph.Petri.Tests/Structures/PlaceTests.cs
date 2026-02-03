using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace QuikGraph.Petri.Tests
{
    /// <summary>
    /// Tests related to <see cref="Place{TToken}"/>.
    /// </summary>
    internal sealed class PlaceTests
    {
        [Test]
        public void Constructor()
        {
            var place = new Place<int>("MyPlace");
            Assert.That("MyPlace", Is.EqualTo(place.Name));
            CollectionAssert.IsEmpty(place.Marking);
        }

        [Test]
        public void Constructor_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new Place<int>(null));
        }

        [Test]
        public void ToStringWithMarking()
        {
            var place = new Place<int>("TestName");
            string expectedString = "P(TestName|0)";
            Assert.That(expectedString, Is.EqualTo(place.ToStringWithMarking()));

            place.Marking.Add(1);
            place.Marking.Add(3);
            place.Marking.Add(5);
            place.Marking.Add(2);
            expectedString =
                "P(TestName|4)" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                "\tInt32" + Environment.NewLine +
                "\tInt32";
            Assert.That(expectedString, Is.EqualTo(place.ToStringWithMarking()));
        }

        [Test]
        public void ObjectToString()
        {
            var place = new Place<int>("TestName");
            Assert.That("P(TestName|0)", Is.EqualTo(place.ToString()));

            place = new Place<int>("OtherTestName");
            Assert.That("P(OtherTestName|0)", Is.EqualTo(place.ToString()));

            place = new Place<int>("TestName_1");
            place.Marking.Add(1);
            place.Marking.Add(3);
            place.Marking.Add(5);
            Assert.That("P(TestName_1|3)", Is.EqualTo(place.ToString()));
        }
    }
}