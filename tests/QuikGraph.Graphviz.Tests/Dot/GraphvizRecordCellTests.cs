using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Graphviz.Dot;

namespace QuikGraph.Graphviz.Tests
{
    /// <summary>
    /// Tests related to <see cref="GraphvizRecordCell"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphvizRecordCellTests
    {
        [Test]
        public void Constructor()
        {
            var cell = new GraphvizRecordCell();
            Assert.That(cell.HasPort, Is.False);
            Assert.That(cell.Port, Is.Null);
            Assert.That(cell.HasText, Is.False);
            Assert.That(cell.Text, Is.Null);
            CollectionAssert.IsEmpty(cell.Cells);
        }

        [Test]
        public void Port()
        {
            var cell = new GraphvizRecordCell();
            if (cell.Port != null)
                throw new InvalidOperationException($"Cell has not null {nameof(GraphvizRecordCell.Port)}.");
            Assert.That(cell.HasPort, Is.False);

            cell.Port = null;

            Assert.That(cell.HasPort, Is.False);
            Assert.That(cell.Port, Is.Null);

            cell.Port = string.Empty;

            Assert.That(cell.HasPort, Is.False);
            Assert.That(cell.Port, Is.Empty);

            const string port = "TestPort";
            cell.Port = port;

            Assert.That(cell.HasPort, Is.True);
            Assert.That(port, Is.EqualTo(cell.Port));
        }

        [Test]
        public void Text()
        {
            var cell = new GraphvizRecordCell();
            if (cell.Text != null)
                throw new InvalidOperationException($"Cell has not null {nameof(GraphvizRecordCell.Text)}.");
            Assert.That(cell.HasText, Is.False);

            cell.Port = null;

            Assert.That(cell.HasText, Is.False);
            Assert.That(cell.Text, Is.Null);

            cell.Text = string.Empty;

            Assert.That(cell.HasText, Is.False);
            Assert.That(cell.Text, Is.Empty);

            const string text = "TestText";
            cell.Text = text;

            Assert.That(cell.HasText, Is.True);
            Assert.That(text, Is.EqualTo(cell.Text));
        }

        [Test]
        public void Cells()
        {
            var cell = new GraphvizRecordCell();
            if (cell.Cells is null)
                throw new InvalidOperationException($"Cell has null {nameof(GraphvizRecordCell.Cells)}.");

            var recordCollection = new GraphvizRecordCellCollection();
            cell.Cells = recordCollection;
            Assert.That(recordCollection, Is.SameAs(cell.Cells));
        }

        [Test]
        public void Cells_Throws()
        {
            var cell = new GraphvizRecordCell();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => cell.Cells = null);
        }


        private static IEnumerable<TestCaseData> ToDotTestCases
        {
            get
            {
                var emptyCell = new GraphvizRecordCell();
                yield return new TestCaseData(emptyCell, string.Empty);

                var cellWithText = new GraphvizRecordCell { Text = "SomeText" };
                yield return new TestCaseData(cellWithText, "SomeText");

                var cellWithPort = new GraphvizRecordCell { Port = "SomePort" };
                yield return new TestCaseData(cellWithPort, "<SomePort> ");

                var cellWithBoth = new GraphvizRecordCell { Port = "Port", Text = "Text" };
                yield return new TestCaseData(cellWithBoth, "<Port> Text");

                var cellWithSpaceToEscape = new GraphvizRecordCell { Port = "Some Port", Text = "Some Text" };
                yield return new TestCaseData(cellWithSpaceToEscape, @"<Some_Port> Some\ Text");

                var cellWithBackslashToEscape = new GraphvizRecordCell { Port = @"Some\Port", Text = @"Some\Text" };
                yield return new TestCaseData(cellWithBackslashToEscape, @"<Some_Port> Some\\Text");

                var cellWithDoubleQuoteToEscape = new GraphvizRecordCell { Port = "Some\"Port", Text = "Some\"Text" };
                yield return new TestCaseData(cellWithDoubleQuoteToEscape, @"<Some_Port> Some\""Text");

                var cellWithLessToEscape = new GraphvizRecordCell { Port = "Some<Port", Text = "Some<Text" };
                yield return new TestCaseData(cellWithLessToEscape, @"<Some_Port> Some\<Text");

                var cellWithGreaterToEscape = new GraphvizRecordCell { Port = "Some>Port", Text = "Some>Text" };
                yield return new TestCaseData(cellWithGreaterToEscape, @"<Some_Port> Some\>Text");

                var cellWithPipeToEscape = new GraphvizRecordCell { Port = "Some|Port", Text = "Some|Text" };
                yield return new TestCaseData(cellWithPipeToEscape, @"<Some_Port> Some\|Text");

                var cellWithLineBreakToEscape = new GraphvizRecordCell { Port = "Some\nPort", Text = "Some\nText" };
                yield return new TestCaseData(cellWithLineBreakToEscape, @"<Some_Port> Some\nText");

                var cellWithChildCell = new GraphvizRecordCell
                {
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell { Port = "SubPort", Text = "SubText" }
                    })
                };
                yield return new TestCaseData(cellWithChildCell, @"{ <SubPort> SubText }");

                var cellMultiPatternWithChildCell = new GraphvizRecordCell
                {
                    Port = "SomePort",
                    Text = "SomeText",
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell { Port = "SubPort", Text = "SubText" }
                    })
                };
                yield return new TestCaseData(cellMultiPatternWithChildCell,
                    @"<SomePort> SomeText | { <SubPort> SubText }");

                var cellWithChildrenCells = new GraphvizRecordCell
                {
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell { Port = "SubPort1" },
                        new GraphvizRecordCell { Port = "SubPort2", Text = "SubText2" },
                        new GraphvizRecordCell { Text = "SubText3" }
                    })
                };
                yield return new TestCaseData(
                    cellWithChildrenCells,
                    @"{ <SubPort1>  | <SubPort2> SubText2 | SubText3 }");

                var cellMultiPatternWithChildrenCells = new GraphvizRecordCell
                {
                    Port = "SomePort",
                    Text = "SomeText",
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell { Port = "SubPort1" },
                        new GraphvizRecordCell { Port = "SubPort2", Text = "SubText2" },
                        new GraphvizRecordCell { Text = "SubText3" }
                    })
                };
                yield return new TestCaseData(
                    cellMultiPatternWithChildrenCells,
                    @"<SomePort> SomeText | { <SubPort1>  | <SubPort2> SubText2 | SubText3 }");

                var cellWithChildrenCellsAndNestedCells = new GraphvizRecordCell
                {
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell { Port = "SubPort1" },
                        new GraphvizRecordCell
                        {
                            Cells = new GraphvizRecordCellCollection(new[]
                            {
                                new GraphvizRecordCell { Text = "NestedText1" },
                                new GraphvizRecordCell { Port = "NestedPort2", Text = "NestedText2" },
                                new GraphvizRecordCell { Text = "NestedText3" }
                            })
                        },
                        new GraphvizRecordCell { Text = "SubText3" }
                    })
                };
                yield return new TestCaseData(
                    cellWithChildrenCellsAndNestedCells,
                    @"{ <SubPort1>  | { NestedText1 | <NestedPort2> NestedText2 | NestedText3 } | SubText3 }");

                var cellWithChildrenCellsAndNestedCells2 = new GraphvizRecordCell
                {
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell { Port = "SubPort1" },
                        new GraphvizRecordCell
                        {
                            Port = "SubPort2",
                            Text = "SubText2",
                            Cells = new GraphvizRecordCellCollection(new[]
                            {
                                new GraphvizRecordCell { Text = "NestedText1" },
                                new GraphvizRecordCell { Port = "NestedPort2", Text = "NestedText2" },
                                new GraphvizRecordCell { Text = "NestedText3" }
                            })
                        },
                        new GraphvizRecordCell { Text = "SubText3" }
                    })
                };
                yield return new TestCaseData(
                    cellWithChildrenCellsAndNestedCells2,
                    @"{ <SubPort1>  | <SubPort2> SubText2 | { NestedText1 | <NestedPort2> NestedText2 | NestedText3 } | SubText3 }");

                var cellMultiPatternWithChildrenCellsAndNestedCells = new GraphvizRecordCell
                {
                    Port = "SomePort",
                    Text = "SomeText",
                    Cells = new GraphvizRecordCellCollection(new[]
                    {
                        new GraphvizRecordCell { Port = "SubPort1" },
                        new GraphvizRecordCell
                        {
                            Cells = new GraphvizRecordCellCollection(new[]
                            {
                                new GraphvizRecordCell { Text = "NestedText1" },
                                new GraphvizRecordCell { Port = "NestedPort2", Text = "NestedText2" },
                                new GraphvizRecordCell { Text = "NestedText3" }
                            })
                        },
                        new GraphvizRecordCell { Text = "SubText3" }
                    })
                };
                yield return new TestCaseData(
                    cellMultiPatternWithChildrenCellsAndNestedCells,
                    @"<SomePort> SomeText | { <SubPort1>  | { NestedText1 | <NestedPort2> NestedText2 | NestedText3 } | SubText3 }");
            }
        }

        [TestCaseSource(nameof(ToDotTestCases))]
        public void ToDot(GraphvizRecordCell recordCell, string expectedDot)
        {
            Assert.That(expectedDot, Is.EqualTo(recordCell.ToDot()));
            Assert.That(expectedDot, Is.EqualTo(recordCell.ToString()));
        }
    }
}