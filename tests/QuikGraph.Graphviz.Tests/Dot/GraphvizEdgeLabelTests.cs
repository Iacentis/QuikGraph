using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphvizEdgeLabel"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizEdgeLabelTests
    {
        [Test]
        public void Constructor()
        {
            var edgeLabel = new GraphvizEdgeLabel();
            Assert.That(-25.0,Is.EqualTo(edgeLabel.Angle));
            Assert.That(1.0,Is.EqualTo(edgeLabel.Distance));
            Assert.That(edgeLabel.Float,Is.True);
            Assert.That(edgeLabel.Font,Is.Null);
            Assert.That(GraphvizColor.Black,Is.EqualTo(edgeLabel.FontColor));
            Assert.That(edgeLabel.IsHtmlLabel,Is.False);
            Assert.That(edgeLabel.Value,Is.Null);
        }


        private static IEnumerable<TestCaseData> AddParametersTestCases
        {
            get
            {
                var label = new GraphvizEdgeLabel();
                yield return new TestCaseData(
                    label,
                    new Dictionary<string, object>());

                label = new GraphvizEdgeLabel
                {
                    FontColor = GraphvizColor.Azure
                };
                yield return new TestCaseData(
                    label,
                    new Dictionary<string, object>());

                label = new GraphvizEdgeLabel
                {
                    Value = "Test Label",
                    FontColor = GraphvizColor.Azure
                };
                yield return new TestCaseData(
                    label,
                    new Dictionary<string, object>
                    {
                        ["label"] = "Test Label",
                        ["labelfontcolor"] = GraphvizColor.Azure
                    });

                label = new GraphvizEdgeLabel
                {
                    Value = "The Demo Label",
                    Angle = 35.5,
                    Distance = 2.0,
                    Float = false,
                    Font = new GraphvizFont("Test font", 16.0f),
                    FontColor = GraphvizColor.BlueViolet
                };
                yield return new TestCaseData(
                    label,
                    new Dictionary<string, object>
                    {
                        ["label"] = "The Demo Label",
                        ["labelangle"] = 35.5,
                        ["labeldistance"] = 2.0,
                        ["labelfloat"] = false,
                        ["labelfontname"] = "Test font",
                        ["labelfontsize"] = 16.0f,
                        ["labelfontcolor"] = GraphvizColor.BlueViolet
                    });

                label = new GraphvizEdgeLabel
                {
                    Value = "\"The Label\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                };
                yield return new TestCaseData(
                    label,
                    new Dictionary<string, object>
                    {
                        ["label"] = @"\""The Label\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                    });
            }
        }

        [TestCaseSource(nameof(AddParametersTestCases))]
        public void AddParameters(
             GraphvizEdgeLabel label,
             Dictionary<string, object> expectedParameters)
        {
            var parameters = new Dictionary<string, object>();
            label.AddParameters(parameters);
            CollectionAssert.AreEquivalent(expectedParameters, parameters);
        }

        [Test]
        public void AddParameters_Throws()
        {
            var extremity = new GraphvizEdgeLabel();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => extremity.AddParameters(null));
        }
    }
}