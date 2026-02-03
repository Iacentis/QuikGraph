using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;
using static QuikGraph.Graphviz.Tests.CultureHelpers;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizGraph"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizGraphTests
    {
        [Test]
        public void Constructor()
        {
            var graph = new GraphvizGraph();
            Assert.That(graph.Comment, Is.Null);
            Assert.That(graph.Url, Is.Null);
            Assert.That(GraphvizColor.White, Is.EqualTo(graph.BackgroundColor));
            Assert.That(GraphvizClusterMode.Local, Is.EqualTo(graph.ClusterRank));
            Assert.That(graph.Font, Is.Null);
            Assert.That(GraphvizColor.Black, Is.EqualTo(graph.FontColor));
            Assert.That(1.0, Is.EqualTo(graph.PenWidth));
            Assert.That(graph.IsCentered, Is.False);
            Assert.That(graph.IsCompounded, Is.False);
            Assert.That(graph.IsConcentrated, Is.False);
            Assert.That(graph.IsLandscape, Is.False);
            Assert.That(graph.IsReMinCross, Is.False);
            Assert.That(graph.IsHtmlLabel, Is.False);
            Assert.That(graph.Label, Is.Null);
            Assert.That(GraphvizLabelJustification.C, Is.EqualTo(graph.LabelJustification));
            Assert.That(graph.Layers, Is.Not.Null);
            Assert.That(1.0, Is.EqualTo(graph.McLimit));
            Assert.That(0.25, Is.EqualTo(graph.NodeSeparation));
            Assert.That(GraphvizRankDirection.TB, Is.EqualTo(graph.RankDirection));
            Assert.That(0.5, Is.EqualTo(graph.RankSeparation));
            Assert.That(-1, Is.EqualTo(graph.NsLimit));
            Assert.That(-1, Is.EqualTo(graph.NsLimit1));
            Assert.That(GraphvizOutputMode.BreadthFirst, Is.EqualTo(graph.OutputOrder));
            Assert.That(GraphvizPageDirection.BL, Is.EqualTo(graph.PageDirection));
            Assert.That(graph.PageSize, Is.Not.Null);
            Assert.That(graph.PageSize.Width, Is.Zero);
            Assert.That(graph.PageSize.Height, Is.Zero);
            Assert.That(graph.Quantum, Is.Zero);
            Assert.That(GraphvizRatioMode.Auto, Is.EqualTo(graph.Ratio));
            Assert.That(0.96, Is.EqualTo(graph.Resolution));
            Assert.That(graph.Rotate, Is.Zero);
            Assert.That(8, Is.EqualTo(graph.SamplePoints));
            Assert.That(30, Is.EqualTo(graph.SearchSize));
            Assert.That(graph.Size, Is.Not.Null);
            Assert.That(graph.Size.Width, Is.Zero);
            Assert.That(graph.Size.Height, Is.Zero);
            Assert.That(GraphvizSplineType.Spline, Is.EqualTo(graph.Splines));
            Assert.That(graph.StyleSheet, Is.Null);
        }

        [Test]
        public void Name()
        {
            var graph = new GraphvizGraph();
            if (graph.Name is null)
                throw new InvalidOperationException($"Graph has null {nameof(GraphvizGraph.Name)}.");

            graph.Name = "GraphName";
            Assert.That("GraphName", Is.SameAs(graph.Name));
        }

        [Test]
        public void Name_Throws()
        {
            var graph = new GraphvizGraph();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => graph.Name = null);
        }


        private static IEnumerable<TestCaseData> ToDotTestCases
        {
            get
            {
                var graph = new GraphvizGraph();
                yield return new TestCaseData(graph, string.Empty);

                graph = new GraphvizGraph { Comment = "Test comment" };
                yield return new TestCaseData(graph, @"comment=""Test comment""");

                graph = new GraphvizGraph { Url = "https://test.fr", PageDirection = GraphvizPageDirection.LB };
                yield return new TestCaseData(graph, @"URL=""https://test.fr""; pagedir=LB;");

                graph = new GraphvizGraph
                {
                    IsNormalized = true,
                    NodeSeparation = 2,
                    RankSeparation = 2,
                    BackgroundColor = GraphvizColor.Coral
                };
                yield return new TestCaseData(graph, @"bgcolor=""#FF7F50FF""; nodesep=2; ranksep=2; normalize=true;");

                graph = new GraphvizGraph
                {
                    Name = "GraphName", // Not written there
                    Comment = "Test comment",
                    Url = "https://test.com",
                    BackgroundColor = GraphvizColor.Bisque,
                    ClusterRank = GraphvizClusterMode.Global,
                    Font = new GraphvizFont("Test font", 12),
                    FontColor = GraphvizColor.DarkOrange,
                    PenWidth = 2.0,
                    IsCentered = true,
                    IsCompounded = true,
                    IsConcentrated = true,
                    IsNormalized = true,
                    IsReMinCross = true,
                    Label = "Test label",
                    LabelJustification = GraphvizLabelJustification.L,
                    LabelLocation = GraphvizLabelLocation.T,
                    Layers = { new GraphvizLayer("Layer1"), new GraphvizLayer("Layer2") },
                    McLimit = 2.0,
                    NodeSeparation = 1.0,
                    RankDirection = GraphvizRankDirection.LR,
                    RankSeparation = 2.0,
                    NsLimit = 2,
                    NsLimit1 = 3,
                    OutputOrder = GraphvizOutputMode.NodesFirst,
                    PageDirection = GraphvizPageDirection.RT,
                    PageSize = new GraphvizSizeF(10.0f, 15.0f),
                    Quantum = 1.0,
                    Ratio = GraphvizRatioMode.Fill,
                    Resolution = 1,
                    SamplePoints = 9,
                    SearchSize = 25,
                    Size = new GraphvizSizeF(25.0f, 45.0f),
                    Splines = GraphvizSplineType.Curved,
                    StyleSheet = "stylesheet.xml"
                };
                graph.Layers.Separators = ":-:";
                yield return new TestCaseData(
                    graph,
                    @"URL=""https://test.com""; bgcolor=""#FFE4C4FF""; center=true; clusterrank=""global""; "
                    + @"comment=""Test comment""; compound=true; concentrate=true; fontname=""Test font""; "
                    + @"fontsize=12; fontcolor=""#FF8C00FF""; penwidth=2; label=""Test label""; labeljust=""l""; "
                    + @"labelloc=""t""; layers=""Layer1:-:Layer2""; layersep="":-:""; mclimit=2; "
                    + @"nodesep=1; rankdir=LR; ranksep=2; normalize=true; nslimit=2; nslimit1=3; "
                    + @"outputorder=""nodesfirst""; page=""10,15""; pagedir=RT; quantum=1; ratio=""fill""; "
                    + @"remincross=true; resolution=1; samplepoints=9; searchsize=25; size=""25,45""; "
                    + @"splines=curved; stylesheet=""stylesheet.xml"";");

                // Orientation
                graph = new GraphvizGraph { Rotate = 12 };
                yield return new TestCaseData(graph, "rotate=12");

                graph = new GraphvizGraph { IsLandscape = true };
                yield return new TestCaseData(graph, @"orientation=""[1L]*""");

                graph = new GraphvizGraph
                {
                    Rotate = 14, // Priority to rotation over landscape (rotate 90)
                    IsLandscape = true
                };
                yield return new TestCaseData(graph, @"rotate=14");

                graph = new GraphvizGraph
                {
                    Comment = "\"The Comment\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪",
                    Label = "\"The Label\"\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                };
                yield return new TestCaseData(
                    graph,
                    @"comment=""\""The Comment\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪""; "
                    + @"label=""\""The Label\""\n &/<>@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"";");

                graph = new GraphvizGraph
                {
                    IsHtmlLabel = true,
                    Label =
                        "<i>\"The Label\"</i>\n &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\\[]() Content ∴∞⇐ℜΩ÷嗷娪"
                };
                yield return new TestCaseData(
                    graph,
                    @"label=<<i>""The Label""</i>" + '\n' +
                    @" &amp;/&lt;&gt;@~| With æéèêë£¤¶ÀÁÂÃÄÅ Escaped Ση← ♠\[]() Content ∴∞⇐ℜΩ÷嗷娪>");
            }
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot(GraphvizGraph graph, string expectedDot)
        {
            Assert.That(expectedDot, Is.EqualTo(graph.ToDot()));
            Assert.That(expectedDot, Is.EqualTo(graph.ToString()));
        }


        private static IEnumerable<TestCaseData> ToDotCultureInvariantTestCases
        {
            get
            {
                Func<GraphvizGraph, string> toDot = graph => graph.ToDot();
                yield return new TestCaseData(toDot);

                Func<GraphvizGraph, string> toString = graph => graph.ToString();
                yield return new TestCaseData(toString);
            }
        }

        [TestCaseSource(nameof(ToDotCultureInvariantTestCases))]
        public void ToDot_InvariantCulture(Func<GraphvizGraph, string> convert)
        {
            var graph = new GraphvizGraph
            {
                Font = new GraphvizFont("Test font", 12.5f),
                PenWidth = 2.5,
                McLimit = 1.5,
                NodeSeparation = 0.5,
                RankSeparation = 0.75,
                PageSize = new GraphvizSizeF(500.5f, 425.6f),
                Quantum = 1.5,
                Resolution = 0.95,
                Size = new GraphvizSizeF(350.5f, 260.4f)
            };

            const string expectedDot =
                @"fontname=""Test font""; fontsize=12.5; penwidth=2.5; mclimit=1.5; nodesep=0.5; ranksep=0.75; "
                + @"page=""500.5,425.6""; quantum=1.5; resolution=0.95; size=""350.5,260.4"";";

            using (CultureScope(EnglishCulture))
            {
                Assert.That(expectedDot, Is.EqualTo(convert(graph)));
            }

            using (CultureScope(FrenchCulture))
            {
                Assert.That(expectedDot, Is.EqualTo(convert(graph)));
            }
        }
    }
}