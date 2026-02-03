using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizArrow"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizArrowTests
    {
        [Test]
        public void Constructor()
        {
            var arrow = new GraphvizArrow(GraphvizArrowShape.Box);
            CheckArrow(arrow, GraphvizArrowShape.Box);

            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond);
            CheckArrow(arrow, GraphvizArrowShape.Diamond);

            arrow = new GraphvizArrow(
                GraphvizArrowShape.Dot,
                GraphvizArrowClipping.Left,
                GraphvizArrowFilling.Open);
            CheckArrow(
                arrow,
                GraphvizArrowShape.Dot,
                GraphvizArrowClipping.Left,
                GraphvizArrowFilling.Open);

            arrow = new GraphvizArrow(
                GraphvizArrowShape.Normal,
                GraphvizArrowClipping.Right,
                GraphvizArrowFilling.Close);
            CheckArrow(
                arrow,
                GraphvizArrowShape.Normal,
                GraphvizArrowClipping.Right);

            #region Local function

            void CheckArrow(
                GraphvizArrow a,
                GraphvizArrowShape shape,
                GraphvizArrowClipping clipping = GraphvizArrowClipping.None,
                GraphvizArrowFilling filling = GraphvizArrowFilling.Close)
            {
                Assert.That(shape,Is.EqualTo(a.Shape));
                Assert.That(clipping,Is.EqualTo(a.Clipping));
                Assert.That(filling,Is.EqualTo(a.Filling));
            }

            #endregion
        }


        private static IEnumerable<TestCaseData> ToDotTestCases
        {
            get
            {
                Func<GraphvizArrow, string> toDot = arrow => arrow.ToDot();
                yield return new TestCaseData(toDot);

                Func<GraphvizArrow, string> toString = arrow => arrow.ToString();
                yield return new TestCaseData(toString);
            }
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot( Func<GraphvizArrow, string> convert)
        {
            // Box variants
            var arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("box",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("lbox",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("rbox",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("obox",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("olbox",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Box, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("orbox",Is.EqualTo(convert(arrow)));

            // Crow variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("crow",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("lcrow",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("rcrow",Is.EqualTo(convert(arrow)));

            // Diamond variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("diamond",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("ldiamond",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("rdiamond",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("odiamond",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("oldiamond",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Diamond, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("ordiamond",Is.EqualTo(convert(arrow)));

            // Dot variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("dot",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("odot",Is.EqualTo(convert(arrow)));

            // Inv variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("inv",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("linv",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("rinv",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("oinv",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("olinv",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Inv, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("orinv",Is.EqualTo(convert(arrow)));

            // None
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("none",Is.EqualTo(convert(arrow)));

            // Normal variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("normal",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("lnormal",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("rnormal",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("onormal",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("olnormal",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Normal, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("ornormal",Is.EqualTo(convert(arrow)));

            // Tee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("tee",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("ltee",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("rtee",Is.EqualTo(convert(arrow)));

            // Vee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("vee",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("lvee",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("rvee",Is.EqualTo(convert(arrow)));

            // Curve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("curve",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("lcurve",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("rcurve",Is.EqualTo(convert(arrow)));

            // ICurve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.None, GraphvizArrowFilling.Close);
            Assert.That("icurve",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("licurve",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("ricurve",Is.EqualTo(convert(arrow)));
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot_SkippedModifiers( Func<GraphvizArrow, string> convert)
        {
            // Skipped Crow variants
            var arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("crow",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("lcrow",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Crow, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("rcrow",Is.EqualTo(convert(arrow)));

            // Skipped Dot variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("dot",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("dot",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("odot",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Dot, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("odot",Is.EqualTo(convert(arrow)));

            // Skipped None variants
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Left, GraphvizArrowFilling.Close);
            Assert.That("none",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Right, GraphvizArrowFilling.Close);
            Assert.That("none",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("none",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("none",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.None, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("none",Is.EqualTo(convert(arrow)));

            // Skipped Tee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("tee",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("ltee",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Tee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("rtee",Is.EqualTo(convert(arrow)));

            // Skipped Vee variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("vee",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("lvee",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Vee, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("rvee",Is.EqualTo(convert(arrow)));

            // Skipped Curve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("curve",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("lcurve",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.Curve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("rcurve",Is.EqualTo(convert(arrow)));

            // Skipped ICurve variants
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.None, GraphvizArrowFilling.Open);
            Assert.That("icurve",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Left, GraphvizArrowFilling.Open);
            Assert.That("licurve",Is.EqualTo(convert(arrow)));
            arrow = new GraphvizArrow(GraphvizArrowShape.ICurve, GraphvizArrowClipping.Right, GraphvizArrowFilling.Open);
            Assert.That("ricurve",Is.EqualTo(convert(arrow)));
        }
    }
}