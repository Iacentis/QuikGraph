using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Algorithms;
using static QuikGraph.Tests.GraphTestHelpers;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;
using static QuikGraph.Serialization.Tests.SerializationTestCaseSources;
using static QuikGraph.Serialization.Tests.TestHelpers;

namespace QuikGraph.Serialization.Tests
{
    /// <summary>
    /// Tests relative to XML serialization.
    /// </summary>
    [TestFixture]
    internal sealed class XmlSerializationTests
    {
        #region Constants

        private static readonly string TestNamespace = string.Empty;


        private const string IdTag = "id";


        private const string GraphTag = "graph";


        private const string NodeTag = "node";


        private const string EdgeTag = "edge";


        private const string SourceTag = "source";


        private const string TargetTag = "target";


        #region Custom data constants

        private const string WeightTag = "weight";


        private const string TypeTag = "type";


        private const string StringDefaultTag = "strDefault";


        private const string StringTag = "str";


        private const string BoolTag = "bool";


        private const string IntTag = "i32";


        private const string LongTag = "i64";


        private const string FloatTag = "f32";


        private const string DoubleTag = "f64";


        private const string AdditionalDataTag = "additional_data";


        private const string IsTaggedTag = "has_tag";


        private const string DoubleValueTag = "double_tag";


        private const string StringValueTag = "string_tag";

        #endregion


        private const string FamilyGraphTag = "family";


        private const string PersonNodeTag = "person";


        private const string RelationEdgeTag = "relationship";


        private const string XmlHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";


        private const string Indent = "    ";


        private const string NullString = "<null>";

        #endregion

        #region Serialization

        #region Test helpers

        private static void SerializeAndRead(
            Action<XmlWriter> onSerialize,
            Action<string> checkSerializedContent)
        {
            var settings = new XmlWriterSettings { Indent = true, IndentChars = Indent };
            using var memory = new MemoryStream();
            using var writer = new StreamWriter(memory);
            using (var xmlWriter = XmlWriter.Create(writer, settings))
            {
                onSerialize(xmlWriter);
            }

            memory.Position = 0;

            using (var reader = new StreamReader(memory))
            {
                checkSerializedContent(reader.ReadToEnd());
            }
        }

        #endregion

        [Test]
        public void SerializeToXml_Empty()
        {
            var emptyGraph = new BidirectionalGraph<Person, Edge<Person>>();

            SerializeAndRead(
                writer => emptyGraph.SerializeToXml(
                    writer,
                    person => person.Id,
                    emptyGraph.GetEdgeIdentity(),
                    FamilyGraphTag,
                    PersonNodeTag,
                    RelationEdgeTag,
                    TestNamespace),
                content =>
                {
                    StringAssert.AreEqualIgnoringCase(
                        $"{XmlHeader}{Environment.NewLine}<{FamilyGraphTag} />",
                        content);
                });
        }


        private static IEnumerable<TestCaseData> XmlSerializationGraphTestCases
        {
            get
            {
                yield return new TestCaseData(new AdjacencyGraph<Person, TaggedEdge<Person, string>>());
                yield return new TestCaseData(new BidirectionalGraph<Person, TaggedEdge<Person, string>>());
                yield return new TestCaseData(new UndirectedGraph<Person, TaggedEdge<Person, string>>());
            }
        }

        [TestCaseSource(nameof(XmlSerializationGraphTestCases))]
        public void SerializeToXml<TGraph>(TGraph graph)
            where TGraph : IMutableVertexAndEdgeSet<Person, TaggedEdge<Person, string>>
        {
            var jacob = new Person("Jacob", "Hochstetler")
            {
                BirthDate = new DateTime(1712, 01, 01),
                BirthPlace = "Alsace, France",
                DeathDate = new DateTime(1776, 01, 01),
                DeathPlace = "Pennsylvania, USA",
                Gender = Gender.Male
            };

            var john = new Person("John", "Hochstetler")
            {
                BirthDate = new DateTime(1735, 01, 01),
                BirthPlace = "Alsace, France",
                DeathDate = new DateTime(1805, 04, 15),
                DeathPlace = "Summit Mills, PA",
                Gender = Gender.Male
            };

            var jonathon = new Person("Jonathon", "Hochstetler")
            {
                BirthPlace = "Pennsylvania", DeathDate = new DateTime(1823, 05, 08), Gender = Gender.Male
            };

            var emanuel = new Person("Emanuel", "Hochstedler")
            {
                BirthDate = new DateTime(1855, 01, 01), DeathDate = new DateTime(1900, 01, 01), Gender = Gender.Male
            };

            graph.AddVerticesAndEdgeRange(
            [
                new(jacob, john, jacob.ChildRelationshipText),
                new(john, jonathon, john.ChildRelationshipText),
                new(jonathon, emanuel, jonathon.ChildRelationshipText)
            ]);

            SerializeAndRead(
                writer => graph.SerializeToXml(
                    writer,
                    person => person.Id,
                    graph.GetEdgeIdentity(),
                    FamilyGraphTag,
                    PersonNodeTag,
                    RelationEdgeTag,
                    TestNamespace),
                content => CheckXmlGraphSerialization(graph, content));

            #region Local function

            static void CheckXmlGraphSerialization(
                IEdgeListGraph<Person, TaggedEdge<Person, string>> graph,
                string xmlGraph)
            {
                var expectedSerializedGraph = new StringBuilder($"{XmlHeader}{Environment.NewLine}");
                expectedSerializedGraph.AppendLine($"<{FamilyGraphTag}>");

                foreach (Person person in graph.Vertices)
                {
                    expectedSerializedGraph.AppendLine($"{Indent}<{PersonNodeTag} id=\"{person.Id}\" />");
                }

                TaggedEdge<Person, string>[] relations = graph.Edges.ToArray();
                for (int i = 0; i < relations.Length; ++i)
                {
                    expectedSerializedGraph.AppendLine(
                        $"{Indent}<{RelationEdgeTag} id=\"{i}\" source=\"{relations[i].Source.Id}\" target=\"{relations[i].Target.Id}\" />");
                }

                expectedSerializedGraph.Append($"</{FamilyGraphTag}>");

                StringAssert.AreEqualIgnoringCase(
                    expectedSerializedGraph.ToString(),
                    xmlGraph);
            }

            #endregion
        }

        [Test]
        public void SerializationToXml_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new AdjacencyGraph<int, Edge<int>>();
            Assert.Throws<ArgumentNullException>(() => graph.SerializeToXml(
                null,
                vertex => vertex.ToString(),
                graph.GetEdgeIdentity(),
                FamilyGraphTag,
                PersonNodeTag,
                RelationEdgeTag,
                TestNamespace));

            using var memory = new MemoryStream();
            using var writer = new StreamWriter(memory);
            using var xmlWriter = XmlWriter.Create(writer);
            Assert.Throws<ArgumentNullException>(() => ((AdjacencyGraph<int, Edge<int>>)null).SerializeToXml(
                xmlWriter,
                vertex => vertex.ToString(),
                graph.GetEdgeIdentity(),
                FamilyGraphTag,
                PersonNodeTag,
                RelationEdgeTag,
                TestNamespace));

            Assert.Throws<ArgumentNullException>(() => graph.SerializeToXml(
                xmlWriter,
                null,
                graph.GetEdgeIdentity(),
                FamilyGraphTag,
                PersonNodeTag,
                RelationEdgeTag,
                TestNamespace));

            Assert.Throws<ArgumentNullException>(() =>
                graph.SerializeToXml<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(
                    xmlWriter,
                    vertex => vertex.ToString(),
                    null,
                    FamilyGraphTag,
                    PersonNodeTag,
                    RelationEdgeTag,
                    TestNamespace));

            Assert.Throws<ArgumentException>(() => graph.SerializeToXml(
                xmlWriter,
                vertex => vertex.ToString(),
                graph.GetEdgeIdentity(),
                null,
                PersonNodeTag,
                RelationEdgeTag,
                TestNamespace));

            Assert.Throws<ArgumentException>(() => graph.SerializeToXml(
                xmlWriter,
                vertex => vertex.ToString(),
                graph.GetEdgeIdentity(),
                "",
                PersonNodeTag,
                RelationEdgeTag,
                TestNamespace));

            Assert.Throws<ArgumentException>(() => graph.SerializeToXml(
                xmlWriter,
                vertex => vertex.ToString(),
                graph.GetEdgeIdentity(),
                FamilyGraphTag,
                null,
                RelationEdgeTag,
                TestNamespace));

            Assert.Throws<ArgumentException>(() => graph.SerializeToXml(
                xmlWriter,
                vertex => vertex.ToString(),
                graph.GetEdgeIdentity(),
                FamilyGraphTag,
                "",
                RelationEdgeTag,
                TestNamespace));

            Assert.Throws<ArgumentException>(() => graph.SerializeToXml(
                xmlWriter,
                vertex => vertex.ToString(),
                graph.GetEdgeIdentity(),
                FamilyGraphTag,
                PersonNodeTag,
                null,
                TestNamespace));

            Assert.Throws<ArgumentException>(() => graph.SerializeToXml(
                xmlWriter,
                vertex => vertex.ToString(),
                graph.GetEdgeIdentity(),
                FamilyGraphTag,
                PersonNodeTag,
                "",
                ""));

            Assert.Throws<ArgumentNullException>(() => graph.SerializeToXml(
                xmlWriter,
                vertex => vertex.ToString(),
                graph.GetEdgeIdentity(),
                FamilyGraphTag,
                PersonNodeTag,
                RelationEdgeTag,
                null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Deserialization

        private const string EmptyGraphFileName = "emptyGraph.xml";


        private const string TestGraphFileName = "testGraph.xml";

        #region Test helpers

        private static void AssetTestGraphContent<TEdge, TGraph>(
            TGraph graph,
            Func<string, string, double, TEdge> edgeFactory)
            where TEdge : IEdge<string>
            where TGraph : IVertexSet<string>, IEdgeSet<string, TEdge>
        {
            AssertHasVertices(
                graph,
                ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"]);
            AssertHasEdges(
                graph,
                [
                    edgeFactory("1", "2", 6.0),
                    edgeFactory("1", "4", 11.0),
                    edgeFactory("1", "5", 5.0),
                    edgeFactory("2", "5", 8.0),
                    edgeFactory("2", "3", 15.0),
                    edgeFactory("2", "4", 18.0),
                    edgeFactory("2", "7", 11.0),
                    edgeFactory("3", "4", 8.0),
                    edgeFactory("3", "8", 18.0),
                    edgeFactory("3", "9", 6.0),
                    edgeFactory("4", "6", 10.0),
                    edgeFactory("4", "7", 7.0),
                    edgeFactory("4", "11", 17.0),
                    edgeFactory("5", "6", 15.0),
                    edgeFactory("5", "7", 9.0),
                    edgeFactory("6", "11", 3.0),
                    edgeFactory("7", "8", 9.0),
                    edgeFactory("7", "9", 4.0),
                    edgeFactory("7", "11", 12.0),
                    edgeFactory("7", "10", 13.0),
                    edgeFactory("8", "9", 14.0),
                    edgeFactory("8", "12", 5.0),
                    edgeFactory("9", "10", 19.0),
                    edgeFactory("10", "12", 2.0),
                    edgeFactory("11", "12", 7.0)
                ]);
        }

        #endregion

        [Test]
        public void DeserializeFromXml_Document_Empty()
        {
            var document = new XPathDocument(GetGraphFilePath(EmptyGraphFileName));

            // Directed graph
            AdjacencyGraph<string, Edge<string>> adjacencyGraph = document.DeserializeFromXml(
                GraphTag,
                NodeTag,
                EdgeTag,
                _ => new AdjacencyGraph<string, Edge<string>>(),
                nav => nav.GetAttribute(IdTag, TestNamespace),
                nav => new Edge<string>(
                    nav.GetAttribute(SourceTag, TestNamespace),
                    nav.GetAttribute(TargetTag, TestNamespace)));
            AssertEmptyGraph(adjacencyGraph);

            // Directed bidirectional graph
            BidirectionalGraph<string, Edge<string>> bidirectionalGraph = document.DeserializeFromXml(
                GraphTag,
                NodeTag,
                EdgeTag,
                _ => new BidirectionalGraph<string, Edge<string>>(),
                nav => nav.GetAttribute(IdTag, TestNamespace),
                nav => new Edge<string>(
                    nav.GetAttribute(SourceTag, TestNamespace),
                    nav.GetAttribute(TargetTag, TestNamespace)));
            AssertEmptyGraph(bidirectionalGraph);

            // Undirected graph
            UndirectedGraph<string, TaggedEdge<string, double>> undirectedGraph = document.DeserializeFromXml(
                GraphTag,
                NodeTag,
                EdgeTag,
                _ => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                nav => nav.GetAttribute(IdTag, TestNamespace),
                nav => new TaggedEdge<string, double>(
                    nav.GetAttribute(SourceTag, TestNamespace),
                    nav.GetAttribute(TargetTag, TestNamespace),
                    int.Parse(nav.GetAttribute(WeightTag, TestNamespace))));
            AssertEmptyGraph(undirectedGraph);
        }

        [Test]
        public void DeserializeFromXml_Document()
        {
            var document = new XPathDocument(GetGraphFilePath(TestGraphFileName));

            // Directed graph
            AdjacencyGraph<string, EquatableEdge<string>> adjacencyGraph = document.DeserializeFromXml(
                GraphTag,
                NodeTag,
                EdgeTag,
                _ => new AdjacencyGraph<string, EquatableEdge<string>>(),
                nav => nav.GetAttribute(IdTag, TestNamespace),
                nav => new EquatableEdge<string>(
                    nav.GetAttribute(SourceTag, TestNamespace),
                    nav.GetAttribute(TargetTag, TestNamespace)));
            AssetTestGraphContent(
                adjacencyGraph,
                (v1, v2, _) => new EquatableEdge<string>(v1, v2));

            // Directed bidirectional graph
            BidirectionalGraph<string, EquatableEdge<string>> bidirectionalGraph = document.DeserializeFromXml(
                GraphTag,
                NodeTag,
                EdgeTag,
                _ => new BidirectionalGraph<string, EquatableEdge<string>>(),
                nav => nav.GetAttribute(IdTag, TestNamespace),
                nav => new EquatableEdge<string>(
                    nav.GetAttribute(SourceTag, TestNamespace),
                    nav.GetAttribute(TargetTag, TestNamespace)));
            AssetTestGraphContent(
                bidirectionalGraph,
                (v1, v2, _) => new EquatableEdge<string>(v1, v2));

            // Undirected graph
            UndirectedGraph<string, EquatableTaggedEdge<string, double>> undirectedGraph = document.DeserializeFromXml(
                GraphTag,
                NodeTag,
                EdgeTag,
                _ => new UndirectedGraph<string, EquatableTaggedEdge<string, double>>(),
                nav => nav.GetAttribute(IdTag, TestNamespace),
                nav => new EquatableTaggedEdge<string, double>(
                    nav.GetAttribute(SourceTag, TestNamespace),
                    nav.GetAttribute(TargetTag, TestNamespace),
                    int.Parse(nav.GetAttribute(WeightTag, TestNamespace))));
            AssetTestGraphContent(
                undirectedGraph,
                (v1, v2, weight) => new EquatableTaggedEdge<string, double>(v1, v2, weight));
        }

        [Test]
        public void DeserializeFromXml_Document_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => ((XPathDocument)null).DeserializeFromXml(
                GraphTag,
                NodeTag,
                EdgeTag,
                _ => new AdjacencyGraph<string, EquatableEdge<string>>(),
                nav => nav.GetAttribute(IdTag, ""),
                nav => new EquatableEdge<string>(
                    nav.GetAttribute(SourceTag, ""),
                    nav.GetAttribute(TargetTag, ""))));

            var document = new XPathDocument(GetGraphFilePath(TestGraphFileName));

            Assert.Throws<ArgumentException>(() => document.DeserializeFromXml(
                null,
                NodeTag,
                EdgeTag,
                _ => new AdjacencyGraph<string, Edge<string>>(),
                nav => nav.GetAttribute(IdTag, ""),
                nav => new Edge<string>(
                    nav.GetAttribute(SourceTag, ""),
                    nav.GetAttribute(TargetTag, ""))));

            Assert.Throws<ArgumentException>(() => document.DeserializeFromXml(
                "",
                NodeTag,
                EdgeTag,
                _ => new AdjacencyGraph<string, Edge<string>>(),
                nav => nav.GetAttribute(IdTag, ""),
                nav => new Edge<string>(
                    nav.GetAttribute(SourceTag, ""),
                    nav.GetAttribute(TargetTag, ""))));

            Assert.Throws<ArgumentException>(() => document.DeserializeFromXml(
                GraphTag,
                null,
                EdgeTag,
                _ => new AdjacencyGraph<string, Edge<string>>(),
                nav => nav.GetAttribute(IdTag, ""),
                nav => new Edge<string>(
                    nav.GetAttribute(SourceTag, ""),
                    nav.GetAttribute(TargetTag, ""))));

            Assert.Throws<ArgumentException>(() => document.DeserializeFromXml(
                GraphTag,
                "",
                EdgeTag,
                _ => new AdjacencyGraph<string, Edge<string>>(),
                nav => nav.GetAttribute(IdTag, ""),
                nav => new Edge<string>(
                    nav.GetAttribute(SourceTag, ""),
                    nav.GetAttribute(TargetTag, ""))));

            Assert.Throws<ArgumentException>(() => document.DeserializeFromXml(
                GraphTag,
                NodeTag,
                null,
                _ => new AdjacencyGraph<string, Edge<string>>(),
                nav => nav.GetAttribute(IdTag, ""),
                nav => new Edge<string>(
                    nav.GetAttribute(SourceTag, ""),
                    nav.GetAttribute(TargetTag, ""))));

            Assert.Throws<ArgumentException>(() => document.DeserializeFromXml(
                GraphTag,
                NodeTag,
                "",
                _ => new AdjacencyGraph<string, Edge<string>>(),
                nav => nav.GetAttribute(IdTag, ""),
                nav => new Edge<string>(
                    nav.GetAttribute(SourceTag, ""),
                    nav.GetAttribute(TargetTag, ""))));

            Assert.Throws<ArgumentNullException>(() =>
                document.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    null,
                    nav => nav.GetAttribute(IdTag, ""),
                    nav => new Edge<string>(
                        nav.GetAttribute(SourceTag, ""),
                        nav.GetAttribute(TargetTag, ""))));

            Assert.Throws<ArgumentNullException>(() =>
                document.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    null,
                    nav => new Edge<string>(
                        nav.GetAttribute(SourceTag, ""),
                        nav.GetAttribute(TargetTag, ""))));

            Assert.Throws<ArgumentNullException>(() =>
                document.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    nav => nav.GetAttribute(IdTag, ""),
                    null));

            // No graph node found
            Assert.Throws<InvalidOperationException>(() => document.DeserializeFromXml(
                "g", // No node named "g" for the graph
                NodeTag,
                EdgeTag,
                _ => new AdjacencyGraph<string, EquatableEdge<string>>(),
                nav => nav.GetAttribute(IdTag, ""),
                nav => new EquatableEdge<string>(
                    nav.GetAttribute(SourceTag, ""),
                    nav.GetAttribute(TargetTag, ""))));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void DeserializeFromXml_Reader_Empty()
        {
            using (var reader = XmlReader.Create(GetGraphFilePath(EmptyGraphFileName)))
            {
                AdjacencyGraph<string, Edge<string>> adjacencyGraph = reader.DeserializeFromXml(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    TestNamespace,
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute(IdTag),
                    r => new Edge<string>(
                        r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute")));
                AssertEmptyGraph(adjacencyGraph);
            }

            using (var reader = XmlReader.Create(GetGraphFilePath(EmptyGraphFileName)))
            {
                BidirectionalGraph<string, Edge<string>> bidirectionalGraph = reader.DeserializeFromXml(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    TestNamespace,
                    _ => new BidirectionalGraph<string, Edge<string>>(),
                    r => r.GetAttribute(IdTag),
                    r => new Edge<string>(
                        r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute")));
                AssertEmptyGraph(bidirectionalGraph);
            }

            using (var reader = XmlReader.Create(GetGraphFilePath(EmptyGraphFileName)))
            {
                UndirectedGraph<string, TaggedEdge<string, double>> undirectedGraph = reader.DeserializeFromXml(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    TestNamespace,
                    _ => new UndirectedGraph<string, TaggedEdge<string, double>>(),
                    r => r.GetAttribute(IdTag),
                    r => new TaggedEdge<string, double>(
                        r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"),
                        int.Parse(r.GetAttribute(WeightTag) ??
                                  throw new AssertionException("Must have weight attribute"))));
                AssertEmptyGraph(undirectedGraph);
            }
        }

        [Test]
        public void DeserializeFromXml_Reader()
        {
            using (var reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                AdjacencyGraph<string, EquatableEdge<string>> adjacencyGraph = reader.DeserializeFromXml(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    TestNamespace,
                    _ => new AdjacencyGraph<string, EquatableEdge<string>>(),
                    r => r.GetAttribute(IdTag),
                    r => new EquatableEdge<string>(
                        r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute")));
                AssetTestGraphContent(
                    adjacencyGraph,
                    (v1, v2, _) => new EquatableEdge<string>(v1, v2));
            }

            using (var reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                BidirectionalGraph<string, EquatableEdge<string>> bidirectionalGraph = reader.DeserializeFromXml(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    TestNamespace,
                    _ => new BidirectionalGraph<string, EquatableEdge<string>>(),
                    r => r.GetAttribute(IdTag),
                    r => new EquatableEdge<string>(
                        r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute")));
                AssetTestGraphContent(
                    bidirectionalGraph,
                    (v1, v2, _) => new EquatableEdge<string>(v1, v2));
            }

            using (var reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                UndirectedGraph<string, EquatableTaggedEdge<string, double>> undirectedGraph =
                    reader.DeserializeFromXml(
                        GraphTag,
                        NodeTag,
                        EdgeTag,
                        TestNamespace,
                        _ => new UndirectedGraph<string, EquatableTaggedEdge<string, double>>(),
                        r => r.GetAttribute(IdTag),
                        r => new EquatableTaggedEdge<string, double>(
                            r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                            r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"),
                            int.Parse(r.GetAttribute(WeightTag) ??
                                      throw new AssertionException("Must have weight attribute"))));
                AssetTestGraphContent(
                    undirectedGraph,
                    (v1, v2, weight) => new EquatableTaggedEdge<string, double>(v1, v2, weight));
            }
        }

        [Test]
        public void DeserializeFromXml_Reader_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => ((XmlReader)null).DeserializeFromXml(
                GraphTag,
                NodeTag,
                EdgeTag,
                "",
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            using var reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName));
            Assert.Throws<ArgumentNullException>(() =>
                reader.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    "",
                    null,
                    r => r.GetAttribute(IdTag),
                    r => new Edge<string>(
                        r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            Assert.Throws<ArgumentNullException>(() =>
                reader.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    null,
                    r => new Edge<string>(
                        r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                        r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            Assert.Throws<ArgumentNullException>(() =>
                reader.DeserializeFromXml<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    "",
                    _ => new AdjacencyGraph<string, Edge<string>>(),
                    r => r.GetAttribute(IdTag),
                    null));


            Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml(
                null,
                r => r.Name == "vertex",
                r => r.Name == EdgeTag,
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml(
                r => r.Name == GraphTag,
                null,
                r => r.Name == EdgeTag,
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml(
                r => r.Name == GraphTag,
                r => r.Name == "vertex",
                null,
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));


            Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                null,
                NodeTag,
                EdgeTag,
                "",
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                "",
                NodeTag,
                EdgeTag,
                "",
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                GraphTag,
                null,
                EdgeTag,
                "",
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                GraphTag,
                "",
                EdgeTag,
                "",
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                GraphTag,
                NodeTag,
                null,
                "",
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            Assert.Throws<ArgumentException>(() => reader.DeserializeFromXml(
                GraphTag,
                NodeTag,
                "",
                "",
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));

            Assert.Throws<ArgumentNullException>(() => reader.DeserializeFromXml(
                GraphTag,
                NodeTag,
                EdgeTag,
                null,
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));


            // No graph node found
            Assert.Throws<InvalidOperationException>(() => reader.DeserializeFromXml(
                "g", // No node named "g" for the graph
                NodeTag,
                EdgeTag,
                "",
                _ => new AdjacencyGraph<string, Edge<string>>(),
                r => r.GetAttribute(IdTag),
                r => new Edge<string>(
                    r.GetAttribute(SourceTag) ?? throw new AssertionException("Must have source attribute"),
                    r.GetAttribute(TargetTag) ?? throw new AssertionException("Must have target attribute"))));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        #endregion

        #region Serialization/Deserialization

        #region Test Helpers

        [Pure]
        private static string GetSerializableString(string str)
        {
            return str ?? NullString;
        }

        private static void SerializeEdge_Simple<TInEdge>(XmlWriter writer, TInEdge edge)
            where TInEdge : IEdge<int>, IEquatable<TInEdge>
        {
            // Serialize only relevant stuff for the test
            switch (edge)
            {
                case EquatableTaggedEdge<int, double> edgeDoubleTag:
                    writer.WriteAttributeString(IsTaggedTag, TestNamespace, bool.TrueString);
                    writer.WriteAttributeString(DoubleValueTag, TestNamespace,
                        edgeDoubleTag.Tag.ToString(CultureInfo.InvariantCulture));
                    break;

                case EquatableTaggedEdge<int, string> edgeStringTag:
                    writer.WriteAttributeString(IsTaggedTag, TestNamespace, bool.TrueString);
                    writer.WriteAttributeString(StringValueTag, TestNamespace,
                        GetSerializableString(edgeStringTag.Tag));
                    break;

                default:
                    writer.WriteAttributeString(IsTaggedTag, TestNamespace, bool.FalseString);
                    break;
            }
        }

        [Pure]
        private static int DeserializeVertex_Simple(XmlReader reader)
        {
            return int.Parse(reader.GetAttribute(IdTag, TestNamespace) ??
                             throw new AssertionException("Unable to deserialize vertex."));
        }

        [Pure]
        private static EquatableEdge<int> DeserializeEdge_Simple(XmlReader reader)
        {
            int source = int.Parse(reader.GetAttribute(SourceTag, TestNamespace) ??
                                   throw new AssertionException("Unable to deserialize edge source."));
            int target = int.Parse(reader.GetAttribute(TargetTag, TestNamespace) ??
                                   throw new AssertionException("Unable to deserialize edge target."));

            bool hasTag = bool.Parse(reader.GetAttribute(IsTaggedTag, TestNamespace) ??
                                     throw new AssertionException("Unable to deserialize edge tag info."));
            if (hasTag)
            {
                string attribute = reader.GetAttribute(DoubleValueTag, TestNamespace);
                if (attribute is null)
                {
                    attribute = reader.GetAttribute(StringValueTag, TestNamespace) ??
                                throw new AssertionException("Unable to deserialize vertex tag.");
                    return new EquatableTaggedEdge<int, string>(source, target, GetString(attribute));
                }

                return new EquatableTaggedEdge<int, double>(
                    source,
                    target,
                    double.Parse(attribute, CultureInfo.InvariantCulture));
            }

            return new EquatableEdge<int>(source, target);


            #region Local function

            string GetString(string readStr)
            {
                return NullString.Equals(readStr, StringComparison.Ordinal) ? null : readStr;
            }

            #endregion
        }

        [Pure]
        private static SEquatableEdge<int> DeserializeSEdge_Simple(XmlReader reader)
        {
            return new SEquatableEdge<int>(
                int.Parse(reader.GetAttribute(SourceTag, TestNamespace) ??
                          throw new AssertionException("Unable to deserialize edge source.")),
                int.Parse(reader.GetAttribute(TargetTag, TestNamespace) ??
                          throw new AssertionException("Unable to deserialize edge target.")));
        }

        private static void SerializeVertex_Complex(XmlWriter writer, EquatableTestVertex vertex)
        {
            // Serialize only relevant stuff for the test
            Type vertexType = vertex.GetType();
            writer.WriteAttributeString(TypeTag, TestNamespace, TypeToSerializableType(vertexType));

            writer.WriteAttributeString(StringDefaultTag, TestNamespace, GetSerializableString(vertex.StringDefault));
            writer.WriteAttributeString(StringTag, TestNamespace, GetSerializableString(vertex.String));
            writer.WriteAttributeString(BoolTag, TestNamespace, vertex.Bool.ToString());
            writer.WriteAttributeString(IntTag, TestNamespace, vertex.Int.ToString());
            writer.WriteAttributeString(LongTag, TestNamespace, vertex.Long.ToString());
            writer.WriteAttributeString(FloatTag, TestNamespace, vertex.Float.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString(DoubleTag, TestNamespace, vertex.Double.ToString(CultureInfo.InvariantCulture));

            if (vertex is EquatableAdditionalDataTestVertex vertexWithData)
            {
                writer.WriteAttributeString(AdditionalDataTag, TestNamespace,
                    vertexWithData.Data.ToString(CultureInfo.InvariantCulture));
            }
        }

        private static void SerializeEdge_Complex<TInEdge>(XmlWriter writer, TInEdge edge)
            where TInEdge : IEdge<EquatableTestVertex>, IEquatable<TInEdge>
        {
            // Serialize only relevant stuff for the test
            IEdge<EquatableTestVertex> edgeToCheck = edge;
            if (edge is SReversedEdge<EquatableTestVertex, EquatableTestEdge> reversedEdge)
            {
                edgeToCheck = reversedEdge.OriginalEdge;
            }

            Type edgeType = edgeToCheck.GetType();
            writer.WriteAttributeString(TypeTag, TestNamespace, TypeToSerializableType(edgeType));

            if (edgeToCheck is EquatableTestEdge edgeToSerialize)
            {
                writer.WriteAttributeString(StringTag, TestNamespace, GetSerializableString(edgeToSerialize.String));
                writer.WriteAttributeString(BoolTag, TestNamespace, edgeToSerialize.Bool.ToString());
                writer.WriteAttributeString(IntTag, TestNamespace, edgeToSerialize.Int.ToString());
                writer.WriteAttributeString(LongTag, TestNamespace, edgeToSerialize.Long.ToString());
                writer.WriteAttributeString(FloatTag, TestNamespace,
                    edgeToSerialize.Float.ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString(DoubleTag, TestNamespace,
                    edgeToSerialize.Double.ToString(CultureInfo.InvariantCulture));
            }

            if (edgeToCheck is EquatableAdditionalDataTestEdge edgeToSerializeWithData)
            {
                writer.WriteAttributeString(AdditionalDataTag, TestNamespace,
                    edgeToSerializeWithData.Data.ToString(CultureInfo.InvariantCulture));
            }
        }

        [Pure]
        private static EquatableTestVertex DeserializeVertex_Complex(XmlReader reader)
        {
            string id = reader.GetAttribute(IdTag, TestNamespace) ??
                        throw new AssertionException("Unable to deserialize vertex id.");
            var type = Type.GetType(reader.GetAttribute(TypeTag, TestNamespace) ??
                                    throw new AssertionException("Unable to deserialize vertex type."));

            EquatableTestVertex vertex = type == typeof(EquatableAdditionalDataTestVertex)
                ? new EquatableAdditionalDataTestVertex(
                    id,
                    double.Parse(reader.GetAttribute(AdditionalDataTag, TestNamespace) ??
                                 throw new AssertionException("Unable to deserialize vertex data.")))
                : new EquatableTestVertex(id);

            return AssignData(vertex);

            #region Local function

            EquatableTestVertex AssignData(EquatableTestVertex v)
            {
                v.StringDefault = GetString(StringDefaultTag);
                v.String = GetString(StringTag);
                v.Bool = bool.Parse(reader.GetAttribute(BoolTag, TestNamespace) ??
                                    throw new AssertionException($"Unable to deserialize vertex \"{BoolTag}\" tag."));
                v.Int = int.Parse(reader.GetAttribute(IntTag, TestNamespace) ??
                                  throw new AssertionException($"Unable to deserialize vertex \"{IntTag}\" tag."));
                v.Long = long.Parse(reader.GetAttribute(LongTag, TestNamespace) ??
                                    throw new AssertionException($"Unable to deserialize vertex \"{LongTag}\" tag."));
                v.Float = float.Parse(
                    reader.GetAttribute(FloatTag, TestNamespace) ??
                    throw new AssertionException($"Unable to deserialize vertex \"{FloatTag}\" tag."),
                    CultureInfo.InvariantCulture);
                v.Double = double.Parse(
                    reader.GetAttribute(DoubleTag, TestNamespace) ??
                    throw new AssertionException($"Unable to deserialize vertex \"{DoubleTag}\" tag."),
                    CultureInfo.InvariantCulture);

                return v;

                #region Local function

                string GetString(string tag)
                {
                    string readStr = reader.GetAttribute(tag, TestNamespace)
                                     ?? throw new AssertionException($"Unable to deserialize vertex \"{tag}\" tag.");
                    return NullString.Equals(readStr, StringComparison.Ordinal) ? null : readStr;
                }

                #endregion
            }

            #endregion
        }

        [Pure]
        private static EquatableTestVertex DeserializeVertex_Complex(XmlReader reader,
            IDictionary<string, EquatableTestVertex> verticesCache)
        {
            EquatableTestVertex vertex = DeserializeVertex_Complex(reader);
            verticesCache[VertexIdentity_Complex(vertex)] = vertex;
            return vertex;
        }

        [Pure]
        private static EquatableTestEdge DeserializeEdge_Complex(XmlReader reader,
            IDictionary<string, EquatableTestVertex> verticesCache)
        {
            string id = reader.GetAttribute(IdTag, TestNamespace) ??
                        throw new AssertionException("Unable to deserialize edge id.");

            string sourceId = reader.GetAttribute(SourceTag, TestNamespace) ??
                              throw new AssertionException("Unable to deserialize edge source id.");
            string targetId = reader.GetAttribute(TargetTag, TestNamespace) ??
                              throw new AssertionException("Unable to deserialize edge target id.");
            EquatableTestVertex source = verticesCache[sourceId];
            EquatableTestVertex target = verticesCache[targetId];

            var type = Type.GetType(reader.GetAttribute(TypeTag, TestNamespace) ??
                                    throw new AssertionException("Unable to deserialize edge type."));

            EquatableTestEdge edge = type == typeof(EquatableAdditionalDataTestEdge)
                ? new EquatableAdditionalDataTestEdge(
                    source,
                    target,
                    id,
                    double.Parse(
                        reader.GetAttribute(AdditionalDataTag, TestNamespace) ??
                        throw new AssertionException("Unable to deserialize edge data."), CultureInfo.InvariantCulture))
                : new EquatableTestEdge(source, target, id);

            return AssignData(edge);

            #region Local function

            EquatableTestEdge AssignData(EquatableTestEdge e)
            {
                e.String = GetString(StringTag);
                e.Bool = bool.Parse(reader.GetAttribute(BoolTag, TestNamespace) ??
                                    throw new AssertionException($"Unable to deserialize edge \"{BoolTag}\" tag."));
                e.Int = int.Parse(reader.GetAttribute(IntTag, TestNamespace) ??
                                  throw new AssertionException($"Unable to deserialize edge \"{IntTag}\" tag."));
                e.Long = long.Parse(reader.GetAttribute(LongTag, TestNamespace) ??
                                    throw new AssertionException($"Unable to deserialize edge \"{LongTag}\" tag."));
                e.Float = float.Parse(
                    reader.GetAttribute(FloatTag, TestNamespace) ??
                    throw new AssertionException($"Unable to deserialize edge \"{FloatTag}\" tag."),
                    CultureInfo.InvariantCulture);
                e.Double = double.Parse(
                    reader.GetAttribute(DoubleTag, TestNamespace) ??
                    throw new AssertionException($"Unable to deserialize edge \"{DoubleTag}\" tag."),
                    CultureInfo.InvariantCulture);

                return e;

                #region Local function

                string GetString(string tag)
                {
                    string readStr = reader.GetAttribute(tag, TestNamespace)
                                     ?? throw new AssertionException($"Unable to deserialize edge \"{tag}\" tag.");
                    return NullString.Equals(readStr, StringComparison.Ordinal) ? null : readStr;
                }

                #endregion
            }

            #endregion
        }

        [Pure]
        private static SEquatableEdge<EquatableTestVertex> DeserializeSEdge_Complex(XmlReader reader,
            IDictionary<string, EquatableTestVertex> verticesCache)
        {
            string sourceId = reader.GetAttribute(SourceTag, TestNamespace) ??
                              throw new AssertionException("Unable to deserialize edge source id.");
            string targetId = reader.GetAttribute(TargetTag, TestNamespace) ??
                              throw new AssertionException("Unable to deserialize edge target id.");
            EquatableTestVertex source = verticesCache[sourceId];
            EquatableTestVertex target = verticesCache[targetId];

            return new SEquatableEdge<EquatableTestVertex>(source, target);
        }

        [Pure]
        private static TOutGraph SerializeDeserialize<TVertex, TInEdge, TOutEdge, TInGraph, TOutGraph>(
            TInGraph graph,
            VertexIdentity<TVertex> vertexIdentity,
            Action<XmlWriter, TVertex> vertexAttributes,
            Action<XmlWriter, TInEdge> edgeAttributes,
            Func<XmlReader, TOutGraph> deserialize)
            where TInEdge : IEdge<TVertex>, IEquatable<TInEdge>
            where TOutEdge : IEdge<TVertex>, IEquatable<TOutEdge>
            where TInGraph : IEdgeListGraph<TVertex, TInEdge>
            where TOutGraph : IEdgeListGraph<TVertex, TOutEdge>
        {
            Assert.That(graph, Is.Not.Null);

            var settings = new XmlWriterSettings { Indent = true, IndentChars = Indent };
            using var memory = new MemoryStream();
            using var writer = new StreamWriter(memory);
            // Serialize
            using (var xmlWriter = XmlWriter.Create(writer, settings))
            {
                graph.SerializeToXml(
                    xmlWriter,
                    vertexIdentity,
                    graph.GetEdgeIdentity(),
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    TestNamespace,
                    null,
                    vertexAttributes,
                    edgeAttributes);
            }

            memory.Position = 0;

            // Deserialize
            using (var xmlReader = XmlReader.Create(memory))
            {
                TOutGraph deserializedGraph = deserialize(xmlReader);
                Assert.That(deserializedGraph, Is.Not.Null);
                Assert.That(graph, Is.Not.SameAs(deserializedGraph));
                return deserializedGraph;
            }
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Simple<TInEdge, TOutEdge, TInGraph, TOutGraph>(
            TInGraph graph,
            Func<XmlReader, TOutGraph> deserialize)
            where TInEdge : IEdge<int>, IEquatable<TInEdge>
            where TOutEdge : IEdge<int>, IEquatable<TOutEdge>
            where TInGraph : IEdgeListGraph<int, TInEdge>
            where TOutGraph : IEdgeListGraph<int, TOutEdge>
        {
            return SerializeDeserialize<int, TInEdge, TOutEdge, TInGraph, TOutGraph>(
                graph,
                VertexIdentity_Simple,
                null,
                SerializeEdge_Simple,
                deserialize);
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Complex<TInEdge, TOutEdge, TInGraph, TOutGraph>(
            TInGraph graph,
            Func<XmlReader, TOutGraph> deserialize)
            where TInEdge : IEdge<EquatableTestVertex>, IEquatable<TInEdge>
            where TOutEdge : IEdge<EquatableTestVertex>, IEquatable<TOutEdge>
            where TInGraph : IEdgeListGraph<EquatableTestVertex, TInEdge>
            where TOutGraph : IEdgeListGraph<EquatableTestVertex, TOutEdge>
        {
            return SerializeDeserialize<EquatableTestVertex, TInEdge, TOutEdge, TInGraph, TOutGraph>(
                graph,
                VertexIdentity_Complex,
                SerializeVertex_Complex,
                SerializeEdge_Complex,
                deserialize);
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Simple<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, EquatableEdge<int>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, EquatableEdge<int>>, new()
        {
            return SerializeDeserialize_Simple<EquatableEdge<int>, EquatableEdge<int>, TInGraph, TOutGraph>(graph,
                reader =>
                    reader.DeserializeFromXml(
                        GraphTag,
                        NodeTag,
                        EdgeTag,
                        TestNamespace,
                        _ => new TOutGraph(),
                        DeserializeVertex_Simple,
                        DeserializeEdge_Simple));
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Complex<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<EquatableTestVertex, EquatableTestEdge>
            where TOutGraph : class, IMutableVertexAndEdgeSet<EquatableTestVertex, EquatableTestEdge>, new()
        {
            var verticesCache = new Dictionary<string, EquatableTestVertex>();
            return SerializeDeserialize_Complex<EquatableTestEdge, EquatableTestEdge, TInGraph, TOutGraph>(graph,
                reader =>
                    reader.DeserializeFromXml(
                        GraphTag,
                        NodeTag,
                        EdgeTag,
                        TestNamespace,
                        _ => new TOutGraph(),
                        r => DeserializeVertex_Complex(r, verticesCache),
                        r => DeserializeEdge_Complex(r, verticesCache)));
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_SEdge_Simple<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, SEquatableEdge<int>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, SEquatableEdge<int>>, new()
        {
            return SerializeDeserialize_Simple<SEquatableEdge<int>, SEquatableEdge<int>, TInGraph, TOutGraph>(graph,
                reader =>
                    reader.DeserializeFromXml(
                        GraphTag,
                        NodeTag,
                        EdgeTag,
                        TestNamespace,
                        _ => new TOutGraph(),
                        DeserializeVertex_Simple,
                        DeserializeSEdge_Simple));
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_SEdge_Complex<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<EquatableTestVertex, SEquatableEdge<EquatableTestVertex>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<EquatableTestVertex, SEquatableEdge<EquatableTestVertex>>,
            new()
        {
            var verticesCache = new Dictionary<string, EquatableTestVertex>();
            return SerializeDeserialize_Complex<SEquatableEdge<EquatableTestVertex>, SEquatableEdge<EquatableTestVertex>
                , TInGraph, TOutGraph>(graph, reader =>
                reader.DeserializeFromXml(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    TestNamespace,
                    _ => new TOutGraph(),
                    r => DeserializeVertex_Complex(r, verticesCache),
                    r => DeserializeSEdge_Complex(r, verticesCache)));
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Reversed_Simple<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, SReversedEdge<int, EquatableEdge<int>>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, EquatableEdge<int>>, new()
        {
            return SerializeDeserialize_Simple<SReversedEdge<int, EquatableEdge<int>>, EquatableEdge<int>, TInGraph,
                TOutGraph>(graph, reader =>
                reader.DeserializeFromXml(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    TestNamespace,
                    _ => new TOutGraph(),
                    DeserializeVertex_Simple,
                    DeserializeEdge_Simple));
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Reversed_Complex<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<EquatableTestVertex, SReversedEdge<EquatableTestVertex, EquatableTestEdge>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<EquatableTestVertex, EquatableTestEdge>, new()
        {
            var verticesCache = new Dictionary<string, EquatableTestVertex>();
            return SerializeDeserialize_Complex<SReversedEdge<EquatableTestVertex, EquatableTestEdge>, EquatableTestEdge
                , TInGraph, TOutGraph>(graph, reader =>
                reader.DeserializeFromXml(
                    GraphTag,
                    NodeTag,
                    EdgeTag,
                    TestNamespace,
                    _ => new TOutGraph(),
                    r => DeserializeVertex_Complex(r, verticesCache),
                    r => DeserializeEdge_Complex(r, verticesCache)));
        }

        #endregion

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphTestCases))]
        public void XmlSerialization_AdjacencyGraph_Simple(AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph1 =
                SerializeDeserialize_Simple<AdjacencyGraph<int, EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph1), Is.True);

            var arrayGraph = new ArrayAdjacencyGraph<int, EquatableEdge<int>>(graph);
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize_Simple<ArrayAdjacencyGraph<int, EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(arrayGraph);
            Assert.That(EquateGraphs.Equate(arrayGraph, deserializedGraph2), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphComplexTestCases))]
        public void XmlSerialization_AdjacencyGraph_Complex(
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> graph)
        {
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph1 =
                SerializeDeserialize_Complex<AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>,
                    AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph1), Is.True);

            var arrayGraph = new ArrayAdjacencyGraph<EquatableTestVertex, EquatableTestEdge>(graph);
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph2 =
                SerializeDeserialize_Complex<ArrayAdjacencyGraph<EquatableTestVertex, EquatableTestEdge>,
                    AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>>(arrayGraph);
            Assert.That(EquateGraphs.Equate(arrayGraph, deserializedGraph2), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphTestCases))]
        public void XmlSerialization_AdapterGraph_Simple(AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            var bidirectionalAdapterGraph = new BidirectionalAdapterGraph<int, EquatableEdge<int>>(graph);
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_Simple<BidirectionalAdapterGraph<int, EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(bidirectionalAdapterGraph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphComplexTestCases))]
        public void XmlSerialization_AdapterGraph_Complex(AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> graph)
        {
            var bidirectionalAdapterGraph =
                new BidirectionalAdapterGraph<EquatableTestVertex, EquatableTestEdge>(graph);
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph =
                SerializeDeserialize_Complex<BidirectionalAdapterGraph<EquatableTestVertex, EquatableTestEdge>,
                    AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>>(bidirectionalAdapterGraph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationClusteredAdjacencyGraphTestCases))]
        public void XmlSerialization_ClusteredGraph_Simple(ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_Simple<ClusteredAdjacencyGraph<int, EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources),
            nameof(SerializationClusteredAdjacencyGraphComplexTestCases))]
        public void XmlSerialization_ClusteredGraph_Complex(
            ClusteredAdjacencyGraph<EquatableTestVertex, EquatableTestEdge> graph)
        {
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph =
                SerializeDeserialize_Complex<ClusteredAdjacencyGraph<EquatableTestVertex, EquatableTestEdge>,
                    AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationCompressedGraphTestCases))]
        public void XmlSerialization_CompressedGraph_Simple(CompressedSparseRowGraph<int> graph)
        {
            AdjacencyGraph<int, SEquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_SEdge_Simple<CompressedSparseRowGraph<int>,
                    AdjacencyGraph<int, SEquatableEdge<int>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationCompressedGraphComplexTestCases))]
        public void XmlSerialization_CompressedGraph_Complex(CompressedSparseRowGraph<EquatableTestVertex> graph)
        {
            AdjacencyGraph<EquatableTestVertex, SEquatableEdge<EquatableTestVertex>> deserializedGraph =
                SerializeDeserialize_SEdge_Complex<CompressedSparseRowGraph<EquatableTestVertex>,
                    AdjacencyGraph<EquatableTestVertex, SEquatableEdge<EquatableTestVertex>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationBidirectionalGraphTestCases))]
        public void XmlSerialization_BidirectionalGraph_Simple(BidirectionalGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_Simple<BidirectionalGraph<int, EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);

            var arrayGraph = new ArrayBidirectionalGraph<int, EquatableEdge<int>>(graph);
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize_Simple<ArrayBidirectionalGraph<int, EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(arrayGraph);
            Assert.That(EquateGraphs.Equate(arrayGraph, deserializedGraph2), Is.True);

            var reversedGraph = new ReversedBidirectionalGraph<int, EquatableEdge<int>>(graph);
            BidirectionalGraph<int, EquatableEdge<int>> deserializedGraph3 =
                SerializeDeserialize_Reversed_Simple<ReversedBidirectionalGraph<int, EquatableEdge<int>>,
                    BidirectionalGraph<int, EquatableEdge<int>>>(reversedGraph);
            Assert.That(
                EquateGraphs.Equate(
                    graph,
                    deserializedGraph3,
                    EqualityComparer<int>.Default,
                    LambdaEqualityComparer<EquatableEdge<int>>.Create(
                        (edge1, edge2) => Equals(edge1.Source, edge2.Target)
                                          && Equals(edge1.Target, edge2.Source)
                                          && edge1.GetType() == edge2.GetType(),
                        edge => edge.GetHashCode())),
                Is.True);

            var undirectedBidirectionalGraph = new UndirectedBidirectionalGraph<int, EquatableEdge<int>>(graph);
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph4 =
                SerializeDeserialize_Simple<UndirectedBidirectionalGraph<int, EquatableEdge<int>>,
                    UndirectedGraph<int, EquatableEdge<int>>>(undirectedBidirectionalGraph);
            Assert.That(EquateGraphs.Equate(undirectedBidirectionalGraph, deserializedGraph4), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationBidirectionalGraphComplexTestCases))]
        public void XmlSerialization_BidirectionalGraph_Complex(
            BidirectionalGraph<EquatableTestVertex, EquatableTestEdge> graph)
        {
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph =
                SerializeDeserialize_Complex<BidirectionalGraph<EquatableTestVertex, EquatableTestEdge>,
                    AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);

            var arrayGraph = new ArrayBidirectionalGraph<EquatableTestVertex, EquatableTestEdge>(graph);
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph2 =
                SerializeDeserialize_Complex<ArrayBidirectionalGraph<EquatableTestVertex, EquatableTestEdge>,
                    AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>>(arrayGraph);
            Assert.That(EquateGraphs.Equate(arrayGraph, deserializedGraph2), Is.True);

            var reversedGraph = new ReversedBidirectionalGraph<EquatableTestVertex, EquatableTestEdge>(graph);
            BidirectionalGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph3 =
                SerializeDeserialize_Reversed_Complex<ReversedBidirectionalGraph<EquatableTestVertex, EquatableTestEdge>
                    , BidirectionalGraph<EquatableTestVertex, EquatableTestEdge>>(reversedGraph);
            Assert.That(
                EquateGraphs.Equate(
                    graph,
                    deserializedGraph3,
                    EqualityComparer<EquatableTestVertex>.Default,
                    LambdaEqualityComparer<EquatableTestEdge>.Create(
                        (edge1, edge2) => Equals(edge1.Source, edge2.Target) && Equals(edge1.Target, edge2.Source) &&
                                          edge1.DataContentEquals(edge2),
                        edge => edge.GetHashCode())),
                Is.True);

            var undirectedBidirectionalGraph =
                new UndirectedBidirectionalGraph<EquatableTestVertex, EquatableTestEdge>(graph);
            UndirectedGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph4 =
                SerializeDeserialize_Complex<UndirectedBidirectionalGraph<EquatableTestVertex, EquatableTestEdge>,
                    UndirectedGraph<EquatableTestVertex, EquatableTestEdge>>(undirectedBidirectionalGraph);
            Assert.That(EquateGraphs.Equate(undirectedBidirectionalGraph, deserializedGraph4), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationBidirectionalMatrixGraphTestCases))]
        public void XmlSerialization_BidirectionalMatrixGraph(BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_Simple<BidirectionalMatrixGraph<EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.That(
                EquateGraphs.Equate(
                    graph,
                    deserializedGraph,
                    EqualityComparer<int>.Default,
                    LambdaEqualityComparer<EquatableEdge<int>>.Create(
                        (edge1, edge2) => EqualityComparer<EquatableEdge<int>>.Default.Equals(edge1, edge2) &&
                                          edge1.GetType() == edge2.GetType(),
                        edge => edge.GetHashCode())),
                Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationUndirectedGraphTestCases))]
        public void XmlSerialization_UndirectedGraph_Simple(UndirectedGraph<int, EquatableEdge<int>> graph)
        {
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph1 =
                SerializeDeserialize_Simple<UndirectedGraph<int, EquatableEdge<int>>,
                    UndirectedGraph<int, EquatableEdge<int>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph1), Is.True);

            var arrayGraph = new ArrayUndirectedGraph<int, EquatableEdge<int>>(graph);
            UndirectedGraph<int, EquatableEdge<int>> deserializedGraph2 =
                SerializeDeserialize_Simple<ArrayUndirectedGraph<int, EquatableEdge<int>>,
                    UndirectedGraph<int, EquatableEdge<int>>>(arrayGraph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph2), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationUndirectedGraphComplexTestCases))]
        public void XmlSerialization_UndirectedGraph_Complex(
            UndirectedGraph<EquatableTestVertex, EquatableTestEdge> graph)
        {
            UndirectedGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph1 =
                SerializeDeserialize_Complex<UndirectedGraph<EquatableTestVertex, EquatableTestEdge>,
                    UndirectedGraph<EquatableTestVertex, EquatableTestEdge>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph1), Is.True);

            var arrayGraph = new ArrayUndirectedGraph<EquatableTestVertex, EquatableTestEdge>(graph);
            UndirectedGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph2 =
                SerializeDeserialize_Complex<ArrayUndirectedGraph<EquatableTestVertex, EquatableTestEdge>,
                    UndirectedGraph<EquatableTestVertex, EquatableTestEdge>>(arrayGraph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph2), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationEdgeListGraphTestCases))]
        public void XmlSerialization_EdgeListGraph_Simple(EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_Simple<EdgeListGraph<int, EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationEdgeListGraphComplexTestCases))]
        public void XmlSerialization_EdgeListGraph_Complex(EdgeListGraph<EquatableTestVertex, EquatableTestEdge> graph)
        {
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph =
                SerializeDeserialize_Complex<EdgeListGraph<EquatableTestVertex, EquatableTestEdge>,
                    AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        #endregion
    }
}