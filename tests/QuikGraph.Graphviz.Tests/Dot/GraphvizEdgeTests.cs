using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;
using static QuikGraph.Graphviz.Tests.CultureHelpers;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizEdge"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizEdgeTests
    {
        [Test]
        public void Constructor()
        {
            var edge = new GraphvizEdge();
            Assert.That(edge.Comment,Is.Null);
            Assert.That(edge.Label.IsHtmlLabel,Is.False);
            Assert.That(edge.Label,Is.Not.Null);
            Assert.That(edge.ToolTip,Is.Null);
            Assert.That(edge.Url,Is.Null);
            Assert.That(GraphvizEdgeDirection.Forward,Is.EqualTo(edge.Direction));
            Assert.That(edge.Font,Is.Null);
            Assert.That(GraphvizColor.Black,Is.EqualTo(edge.FontColor));
            Assert.That(1.0,Is.EqualTo(edge.PenWidth));
            Assert.That(edge.Head,Is.Not.Null);
            Assert.That(edge.HeadArrow,Is.Null);
            Assert.That(edge.HeadPort,Is.Null);
            Assert.That(edge.Tail,Is.Not.Null);
            Assert.That(edge.TailArrow,Is.Null);
            Assert.That(edge.TailPort,Is.Null);
            Assert.That(edge.IsConstrained,Is.True);
            Assert.That(edge.IsDecorated,Is.False);
            Assert.That(edge.Layer,Is.Null);
            Assert.That(GraphvizColor.Black,Is.EqualTo(edge.StrokeColor));
            Assert.That(GraphvizEdgeStyle.Unspecified,Is.EqualTo(edge.Style));
            Assert.That(1,Is.EqualTo(edge.Weight));
            Assert.That(1,Is.EqualTo(edge.Length));
            Assert.That(1,Is.EqualTo(edge.MinLength));
        }

        [Test]
        public void Label()
        {
            var edge = new GraphvizEdge();
            if (edge.Label is null)
                throw new InvalidOperationException($"Edge has null {nameof(GraphvizEdge.Label)}.");

            var label = new GraphvizEdgeLabel();
            edge.Label = label;
            Assert.That(label,Is.SameAs(edge.Label));
        }

        [Test]
        public void Label_Throws()
        {
            var edge = new GraphvizEdge();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => edge.Label = null);
        }

        [Test]
        public void Head()
        {
            var edge = new GraphvizEdge();
            if (edge.Head is null)
                throw new InvalidOperationException($"Edge has null {nameof(GraphvizEdge.Head)}.");

            var headExtremity = new GraphvizEdgeExtremity(true);
            edge.Head = headExtremity;
            Assert.That(headExtremity,Is.SameAs(edge.Head));
        }

        [Test]
        public void Head_Throws()
        {
            var edge = new GraphvizEdge();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => edge.Head = null);
            Assert.Throws<ArgumentException>(() => edge.Head = new GraphvizEdgeExtremity(false));
        }

        [Test]
        public void Tail()
        {
            var edge = new GraphvizEdge();
            if (edge.Tail is null)
                throw new InvalidOperationException($"Edge has null {nameof(GraphvizEdge.Tail)}.");

            var tailExtremity = new GraphvizEdgeExtremity(false);
            edge.Tail = tailExtremity;
            Assert.That(tailExtremity,Is.SameAs(edge.Tail));
        }

        [Test]
        public void Tail_Throws()
        {
            var edge = new GraphvizEdge();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => edge.Tail = null);
            Assert.Throws<ArgumentException>(() => edge.Tail = new GraphvizEdgeExtremity(true));
        }


        private static IEnumerable<TestCaseData> ToDotTestCases
        {
            get
            {
                var edge = new GraphvizEdge();
                yield return new TestCaseData(edge, string.Empty);

                edge = new GraphvizEdge
                {
                    Url = "https://test.url.fr"
                };
                yield return new TestCaseData(edge, @"URL=""https://test.url.fr""");

                edge = new GraphvizEdge
                {
                    ToolTip = "The edge tooltip",
                    Style = GraphvizEdgeStyle.Dotted,
                    StrokeColor = GraphvizColor.LightYellow
                };
                yield return new TestCaseData(edge, @"color=""#FFFFE0FF"", style=dotted, tooltip=""The edge tooltip""");

                edge = new GraphvizEdge
                {
                    Label = new GraphvizEdgeLabel
                    {
                        Value = "<b>Bold</b> text"
                    },
                    Head = new GraphvizEdgeExtremity(true)
                    {
                        Label = "<b>Bold</b> text"
                    },
                    Tail = new GraphvizEdgeExtremity(false)
                    {
                        Label = "<b>Bold</b> text"
                    }
                };
                yield return new TestCaseData(
                    edge,
                    @"headlabel=""<b>Bold</b> text"", label=""<b>Bold</b> text"", taillabel=""<b>Bold</b> text""");

                edge = new GraphvizEdge
                {
                    Label = new GraphvizEdgeLabel
                    {
                        IsHtmlLabel = true,
                        Value = "<b>Bold</b> text"
                    },
                    Head = new GraphvizEdgeExtremity(true)
                    {
                        IsHtmlLabel = true,
                        Label = "<b>Bold</b> text"
                    },
                    Tail = new GraphvizEdgeExtremity(false)
                    {
                        IsHtmlLabel = true,
                        Label = "<b>Bold</b> text"
                    }
                };
                yield return new TestCaseData(
                    edge,
                    @"headlabel=<<b>Bold</b> text>, label=<<b>Bold</b> text>, taillabel=<<b>Bold</b> text>");

                edge = new GraphvizEdge
                {
                    Comment = "Test comment",
                    Label = new GraphvizEdgeLabel
                    {
                        Value = "Test Label",
                        Angle = 15,
                        Distance = 5,
                        Float = false,
                        Font = new GraphvizFont("Label font", 10.0f),
                        FontColor = GraphvizColor.Aquamarine
                    },
                    ToolTip = "Test tooltip",
                    Url = "https://test.com",
                    Direction = GraphvizEdgeDirection.Both,
                    Font = new GraphvizFont("Test font", 12.0f),
                    FontColor = GraphvizColor.Chocolate,
                    PenWidth = 2.0,
                    Head = new GraphvizEdgeExtremity(true)
                    {
                        Label = "Head label"
                    },
                    HeadArrow = new GraphvizArrow(GraphvizArrowShape.Diamond),
                    HeadPort = "TestHeadPort",
                    Tail = new GraphvizEdgeExtremity(false)
                    {
                        Label = "Tail label"
                    },
                    TailArrow = new GraphvizArrow(GraphvizArrowShape.Curve),
                    TailPort = "TestTailPort",
                    IsConstrained = false,
                    IsDecorated = true,
                    Layer = new GraphvizLayer("Edge Layer"),
                    StrokeColor = GraphvizColor.DarkGreen,
                    Style = GraphvizEdgeStyle.Bold,
                    Weight = 2,
                    Length = 5,
                    MinLength = 2
                };
                yield return new TestCaseData(
                    edge,
                    @"dir=both, fontname=""Test font"", fontsize=12, fontcolor=""#D2691EFF"", penwidth=2, "
                    + @"headlabel=""Head label"", arrowhead=""diamond"", headport=""TestHeadPort"", "
                    + @"constraint=false, decorate=true, label=""Test Label"", labelangle=15, "
                    + @"labeldistance=5, labelfloat=false, labelfontname=""Label font"", labelfontsize=10, "
                    + @"labelfontcolor=""#7FFFD4FF"", layer=""Edge Layer"", minlen=2, len=5, color=""#006400FF"", "
                    + @"style=bold, taillabel=""Tail label"", arrowtail=""curve"", tailport=""TestTailPort"", "
                    + @"tooltip=""Test tooltip"", comment=""Test comment"", URL=""https://test.com"", "
                    + @"weight=2");

                // With escape
                edge = new GraphvizEdge
                {
                    Comment = "\"The Comment\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪",
                    Label = new GraphvizEdgeLabel
                    {
                        Value = "\"The Label\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                    },
                    ToolTip = "\"The Tooltip\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪",
                    Head = new GraphvizEdgeExtremity(true)
                    {
                        Label = "\"The Head Label\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                    },
                    HeadPort = "\"The Head Port\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪",
                    Tail = new GraphvizEdgeExtremity(false)
                    {
                        Label = "\"The Tail Label\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                    },
                    TailPort = "\"The Tail Port\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                };
                yield return new TestCaseData(
                    edge,
                    @"headlabel=""\""The Head Label\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"", "
                    + @"headport=""_The_Head_Port___&/__@~__With_æéèêë£¤¶ÀÁÂÃÄÅ_Escaped_Ση←_♠_[]()_Content_∴∞⇐ℜΩ÷嗷娪"", "
                    + @"label=""\""The Label\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"", "
                    + @"taillabel=""\""The Tail Label\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"", "
                    + @"tailport=""_The_Tail_Port___&/__@~__With_æéèêë£¤¶ÀÁÂÃÄÅ_Escaped_Ση←_♠_[]()_Content_∴∞⇐ℜΩ÷嗷娪"", "
                    + @"tooltip=""\""The Tooltip\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"", "
                    + @"comment=""\""The Comment\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪""");

                edge = new GraphvizEdge
                {
                    Label = new GraphvizEdgeLabel
                    {
                        IsHtmlLabel = true,
                        Value = "<i>\"The Label\"</i>\n &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                    },
                    Head = new GraphvizEdgeExtremity(true)
                    {
                        IsHtmlLabel = true,
                        Label = "<i>\"The Label\"</i>\n &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                    },
                    Tail = new GraphvizEdgeExtremity(false)
                    {
                        IsHtmlLabel = true,
                        Label = "<i>\"The Label\"</i>\n &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                    }
                };
                yield return new TestCaseData(
                    edge,
                    @"headlabel=<<i>""The Label""</i>" + '\n' + @" &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\[]() Content ∴∞⇐ℜΩ÷嗷娪>, "
                    + @"label=<<i>""The Label""</i>" + '\n' + @" &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\[]() Content ∴∞⇐ℜΩ÷嗷娪>, "
                    + @"taillabel=<<i>""The Label""</i>" + '\n' + @" &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\[]() Content ∴∞⇐ℜΩ÷嗷娪>");
            }
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot( GraphvizEdge edge,  string expectedDot)
        {
            Assert.That(expectedDot,Is.EqualTo(edge.ToDot()));
            Assert.That(expectedDot,Is.EqualTo(edge.ToString()));
        }


        private static IEnumerable<TestCaseData> ToDotCultureInvariantTestCases
        {
            get
            {
                Func<GraphvizEdge, string> toDot = edge => edge.ToDot();
                yield return new TestCaseData(toDot);

                Func<GraphvizEdge, string> toString = edge => edge.ToString();
                yield return new TestCaseData(toString);
            }
        }

        [TestCaseSource(nameof(ToDotCultureInvariantTestCases))]
        public void ToDot_InvariantCulture( Func<GraphvizEdge, string> convert)
        {
            var edge = new GraphvizEdge
            {
                Label = new GraphvizEdgeLabel
                {
                    Value = "Edge label",
                    Angle = 25.5,
                    Distance = 1.5,
                    Font = new GraphvizFont("Label font", 14.5f)
                },
                Font = new GraphvizFont("Test font", 12.5f),
                PenWidth = 2.5,
                Weight = 1.5
            };

            const string expectedDot =
                @"fontname=""Test font"", fontsize=12.5, penwidth=2.5, label=""Edge label"", labelangle=25.5, "
                + @"labeldistance=1.5, labelfontname=""Label font"", labelfontsize=14.5, weight=1.5";

            using (CultureScope(EnglishCulture))
            {
                Assert.That(expectedDot,Is.EqualTo(convert(edge)));
            }

            using (CultureScope(FrenchCulture))
            {
                Assert.That(expectedDot,Is.EqualTo(convert(edge)));
            }
        }
    }
}