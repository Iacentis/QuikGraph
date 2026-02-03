using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection.Emit;
using System.Linq;
using System.Xml.Schema;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using QuikGraph.Algorithms;
using QuikGraph.Tests;
using static QuikGraph.Serialization.Tests.SerializationTestCaseSources;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;
using static QuikGraph.Serialization.Tests.TestHelpers;

namespace QuikGraph.Serialization.Tests
{
    /// <summary>
    /// Tests for <see cref="GraphMLSerializer{TVertex,TEdge,TGraph}"/>.
    /// </summary>
    [TestFixture]
    internal sealed class GraphMLSerializerTests
    {
        #region Test helpers

        [Pure]
        private static TGraph VerifySerialization<TGraph>(
            TGraph graph,
            Func<TGraph, string> serializeGraph,
            Func<string, TGraph> deserializeGraph)
        {
            // Serialize the graph to GraphML
            string graphml = serializeGraph(graph);

            // Deserialize a graph from previous serialization
            TGraph serializedGraph = deserializeGraph(graphml);

            // Serialize the deserialized graph
            string newGraphml = serializeGraph(serializedGraph);

            // => Serialization should produce the same result
            Assert.That(graphml, Is.EqualTo(newGraphml));

            Assert.That(graph, Is.Not.SameAs(serializeGraph));

            return serializedGraph;
        }

        #endregion

        #region Serialization

        private static readonly string WriteThrowsTestFilePath =
            Path.Combine(GetTemporaryTestDirectory(), "serialization_to_graphml_throws_test.graphml");

        [Test]
        public void SerializeToGraphML_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // Filepath
            Assert.Throws<ArgumentNullException>(() =>
                ((AdjacencyGraph<TestVertex, TestEdge>)null)
                .SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    WriteThrowsTestFilePath));

            var graph = new AdjacencyGraph<TestVertex, TestEdge>();
            Assert.Throws<ArgumentException>(() =>
                graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>((string)null));
            Assert.Throws<ArgumentException>(() =>
                graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(""));

            Assert.Throws<ArgumentNullException>(() =>
                ((AdjacencyGraph<TestVertex, TestEdge>)null)
                .SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    WriteThrowsTestFilePath,
                    vertex => vertex.ID,
                    edge => edge.ID));

            Assert.Throws<ArgumentException>(() =>
                graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    (string)null,
                    vertex => vertex.ID,
                    edge => edge.ID));
            Assert.Throws<ArgumentException>(() =>
                graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    "",
                    vertex => vertex.ID,
                    edge => edge.ID));

            Assert.Throws<ArgumentNullException>(() =>
                graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    WriteThrowsTestFilePath,
                    null,
                    edge => edge.ID));

            Assert.Throws<ArgumentNullException>(() =>
                ((AdjacencyGraph<TestVertex, TestEdge>)null)
                .SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    WriteThrowsTestFilePath,
                    vertex => vertex.ID,
                    null));

            // XML writer
            Assert.Throws<ArgumentNullException>(() =>
                graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>((XmlWriter)null));
            Assert.Throws<ArgumentNullException>(() =>
                graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                    (XmlWriter)null,
                    vertex => vertex.ID,
                    edge => edge.ID));

            using (XmlWriter writer = XmlWriter.Create(WriteThrowsTestFilePath))
            {
                Assert.Throws<ArgumentNullException>(() =>
                    ((AdjacencyGraph<TestVertex, TestEdge>)null)
                    .SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(writer));

                Assert.Throws<ArgumentNullException>(() =>
                    ((AdjacencyGraph<TestVertex, TestEdge>)null)
                    .SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                        writer,
                        vertex => vertex.ID,
                        edge => edge.ID));

                Assert.Throws<ArgumentNullException>(() =>
                    graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                        writer,
                        null,
                        edge => edge.ID));

                Assert.Throws<ArgumentNullException>(() =>
                    graph.SerializeToGraphML<TestVertex, TestEdge, AdjacencyGraph<TestVertex, TestEdge>>(
                        writer,
                        vertex => vertex.ID,
                        null));
            }
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void SerializeToGraphML_Throws_InvalidData()
        {
            AssertSerializationFail<TestGraphArrayDefaultValue, NotSupportedException>(
                new TestGraphArrayDefaultValue());
            AssertSerializationFail<TestGraphNoGetter, TypeInitializationException>(new TestGraphNoGetter());
            AssertSerializationFail<TestGraphNullDefaultValue, TypeInitializationException>(
                new TestGraphNullDefaultValue());
            AssertSerializationFail<TestGraphWrongDefaultValue, TypeInitializationException>(
                new TestGraphWrongDefaultValue());
            AssertSerializationFail<TestGraphNotSupportedType, TypeInitializationException>(
                new TestGraphNotSupportedType());

            AssertSerializationFail<TestGraphObjectDefaultValue, NotSupportedException>(
                new TestGraphObjectDefaultValue());
            AssertSerializationFail<TestGraphNotSupportedType2, NotSupportedException>(
                new TestGraphNotSupportedType2());
            AssertSerializationFail<TestGraphNotSupportedType3, NotSupportedException>(
                new TestGraphNotSupportedType3());
            AssertSerializationFail<TestGraphWrongDefaultValue2, NotSupportedException>(
                new TestGraphWrongDefaultValue2());

            #region Local function

            void AssertSerializationFail<TGraph, TException>(TGraph g)
                where TGraph : IMutableVertexAndEdgeListGraph<TestVertex, TestEdge>
                where TException : Exception
            {
                Assert.Throws<TException>(() => g.SerializeToGraphML<TestVertex, TestEdge, TGraph>(
                    WriteThrowsTestFilePath,
                    vertex => vertex.ID,
                    edge => edge.ID));
            }

            #endregion
        }

        #endregion

        #region Deserialization

        private const string TestGraphFileName = "DCT8.graphml";


        private const string MissingAttributeTestGraphFileName = "DCT8_with_missing_attribute.graphml";


        private const string MissingSourceTestGraphFileName = "DCT8_with_missing_source_id.graphml";


        private const string MissingTargetTestGraphFileName = "DCT8_with_missing_target_id.graphml";


        private const string InvalidTagTestGraphFileName = "DCT8_invalid_tag.graphml";


        private const string MissingGraphMLTestGraphFileName = "DCT8_missing_graphml_tag.graphml";


        private const string MissingGraphTestGraphFileName = "DCT8_missing_graph_tag.graphml";

        [Test]
        public void DeserializeFromGraphML()
        {
            foreach (string graphMLFilePath in TestGraphFactory.GetGraphMLFilePaths())
            {
                var graph = new AdjacencyGraph<string, Edge<string>>();
                using (var reader = new StreamReader(graphMLFilePath))
                {
                    graph.DeserializeFromGraphML(
                        reader,
                        id => id,
                        (source, target, _) => new Edge<string>(source, target));
                }

                var vertices = new Dictionary<string, string>();
                foreach (string vertex in graph.Vertices)
                    vertices.Add(vertex, vertex);

                // Check all nodes are loaded

                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Ignore,
                    XmlResolver = new GraphMLXmlResolver(),
                    ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings
                };

                using (XmlReader xmlReader = XmlReader.Create(graphMLFilePath, settings))
                {
                    using (var reader = new XmlTextReader(graphMLFilePath))
                    {
                        reader.XmlResolver = null;

                        var document = new XPathDocument(reader);

                        foreach (XPathNavigator node in document.CreateNavigator().Select("/graphml/graph/node"))
                        {
                            string id = node.GetAttribute("id", "");
                            Assert.That(vertices.ContainsKey(id), Is.True);
                        }

                        // Check all edges are loaded
                        foreach (XPathNavigator node in document.CreateNavigator().Select("/graphml/graph/edge"))
                        {
                            string source = node.GetAttribute("source", "");
                            string target = node.GetAttribute("target", "");
                            Assert.That(graph.ContainsEdge(vertices[source], vertices[target]), Is.True);
                        }
                    }
                }
            }
        }

        [Test]
        public void DeserializeFromGraphML_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // Filepath
            Assert.Throws<ArgumentNullException>(() =>
                ((AdjacencyGraph<string, Edge<string>>)null).DeserializeFromGraphML(
                    GetGraphFilePath(TestGraphFileName),
                    id => id,
                    (source, target, _) => new Edge<string>(source, target)));

            var graph = new AdjacencyGraph<string, Edge<string>>();
            Assert.Throws<ArgumentException>(() => graph.DeserializeFromGraphML(
                (string)null,
                id => id,
                (source, target, _) => new Edge<string>(source, target)));

            Assert.Throws<ArgumentException>(() => graph.DeserializeFromGraphML(
                "",
                id => id,
                (source, target, _) => new Edge<string>(source, target)));

            Assert.Throws<ArgumentNullException>(() =>
                graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    GetGraphFilePath(TestGraphFileName),
                    null,
                    (source, target, _) => new Edge<string>(source, target)));

            Assert.Throws<ArgumentNullException>(() =>
                graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                    GetGraphFilePath(TestGraphFileName),
                    id => id,
                    null));

            // Text reader
            Assert.Throws<ArgumentNullException>(() => graph.DeserializeFromGraphML(
                (TextReader)null,
                id => id,
                (source, target, _) => new Edge<string>(source, target)));

            using (var reader = new StreamReader(GetGraphFilePath(TestGraphFileName)))
            {
                Assert.Throws<ArgumentNullException>(() =>
                    ((AdjacencyGraph<string, Edge<string>>)null).DeserializeFromGraphML(
                        reader,
                        id => id,
                        (source, target, _) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(() =>
                    graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        null,
                        (source, target, _) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(() =>
                    graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        id => id,
                        null));
            }

            // XML reader
            Assert.Throws<ArgumentNullException>(() => graph.DeserializeFromGraphML(
                (XmlReader)null,
                id => id,
                (source, target, _) => new Edge<string>(source, target)));

            using (var reader = XmlReader.Create(GetGraphFilePath(TestGraphFileName)))
            {
                Assert.Throws<ArgumentNullException>(() =>
                    ((AdjacencyGraph<string, Edge<string>>)null).DeserializeFromGraphML(
                        reader,
                        id => id,
                        (source, target, _) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(() =>
                    graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        null,
                        (source, target, _) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(() =>
                    graph.DeserializeFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        id => id,
                        null));
            }
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void DeserializeFromGraphML_Throws_InvalidData()
        {
            AssertDeserializationFail<TestGraphArrayDefaultValue, TypeInitializationException>(
                new TestGraphArrayDefaultValue());
            AssertDeserializationFail<TestGraphNoSetter, TypeInitializationException>(new TestGraphNoSetter());
            AssertDeserializationFail<TestGraphNullDefaultValue, TypeInitializationException>(
                new TestGraphNullDefaultValue());
            AssertDeserializationFail<TestGraphWrongDefaultValue, TypeInitializationException>(
                new TestGraphWrongDefaultValue());
            AssertDeserializationFail<TestGraphNotSupportedType, TypeInitializationException>(
                new TestGraphNotSupportedType());

            var graph = new TestGraph();
            AssertDeserializationFail<TestGraph, ArgumentException>(graph, MissingAttributeTestGraphFileName);
            AssertDeserializationFail<TestGraph, ArgumentException>(graph, MissingSourceTestGraphFileName);
            AssertDeserializationFail<TestGraph, ArgumentException>(graph, MissingTargetTestGraphFileName);
            AssertDeserializationFail<TestGraph, InvalidOperationException>(graph, InvalidTagTestGraphFileName);
            AssertDeserializationFail<TestGraph, InvalidOperationException>(graph, MissingGraphTestGraphFileName);
            AssertDeserializationFail<TestGraph, InvalidOperationException>(graph, MissingGraphMLTestGraphFileName);

            #region Local function

            void AssertDeserializationFail<TGraph, TException>(TGraph g, string fileName = TestGraphFileName)
                where TGraph : IMutableVertexAndEdgeListGraph<TestVertex, TestEdge>
                where TException : Exception
            {
                Assert.Throws<TException>(() => g.DeserializeFromGraphML(
                    GetGraphFilePath(fileName),
                    id => new TestVertex(id),
                    (source, target, id) => new TestEdge(source, target, id)));
            }

            #endregion
        }

        #region Test class

        private class TestClass
        {
            public char Char { get; }
        }

        #endregion

        [Test]
        public void EmitValue_Throws()
        {
            var method = new DynamicMethod("TestMethod", typeof(void), Type.EmptyTypes);
            ILGenerator generator = method.GetILGenerator();
            Assert.Throws<NotSupportedException>(() => ILHelpers.EmitValue(
                generator,
                typeof(TestClass).GetProperty(nameof(TestClass.Char)) ??
                throw new AssertionException("Property must exist."),
                'a'));
        }


        #region Test data

        private static readonly bool[] Bools = { true, false, true, true };


        private static readonly int[] Ints = { 2, 3, 45, 3, 44, -2, 3, 5, 99999999 };


        private static readonly long[] Longs =
        {
            3, 4, 43, 999999999999999999L, 445, 55, 3, 98, 49789238740598170, 987459, 97239, 234245, 0, -2232
        };


        private static readonly float[] Floats =
        {
            3.14159265F, 1.1F, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345F
        };


        private static readonly double[] Doubles =
        {
            3.14159265, 1.1, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345
        };


        private static readonly string[] Strings =
        {
            "", "Quick", "", "brown", "fox", "jumps", "over", "the", "lazy", "dog", ".", ""
        };


        private static readonly IList<bool> BoolsList = new[] { true, false, true, true };


        private static readonly IList<int> IntsList = new[] { 2, 3, 45, 3, 44, -2, 3, 5, 99999999 };


        private static readonly IList<long> LongsList = new[]
        {
            3, 4, 43, 999999999999999999L, 445, 55, 3, 98, 49789238740598170, 987459, 97239, 234245, 0, -2232
        };


        private static readonly IList<float> FloatsList =
            new[] { 3.14159265F, 1.1F, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345F };


        private static readonly IList<double> DoublesList =
            new[] { 3.14159265, 1.1, 1, 23, -2, 987459, 97239, 234245, 0, -2232, 234.55345 };


        private static readonly IList<string> StringsList = new[]
        {
            "", "Quick", "", "brown", "fox", "jumps", "over", "the", "lazy", "dog", ".", ""
        };

        #endregion

        #region Test helpers

        [Pure]
        private static string SerializeGraph1(TestGraph graph)
        {
            string filePath = Path.Combine(
                GetTemporaryTestDirectory(),
                $"serialization_to_graphml_test_{Guid.NewGuid()}.graphml");

            graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(filePath);
            Assert.That(File.Exists(filePath), Is.True);
            return File.ReadAllText(filePath);
        }

        [Pure]
        private static string SerializeGraph2(TestGraph graph)
        {
            string filePath = Path.Combine(
                GetTemporaryTestDirectory(),
                $"serialization_to_graphml_test_{Guid.NewGuid()}.graphml");

            graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(
                filePath,
                vertex => vertex.ID,
                edge => edge.ID);
            Assert.That(File.Exists(filePath), Is.True);
            return File.ReadAllText(filePath);
        }

        [Pure]
        private static string SerializeGraph3(TestGraph graph)
        {
            using (var writer = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(xmlWriter);
                }

                return writer.ToString();
            }
        }

        [Pure]
        private static string SerializeGraph4(TestGraph graph)
        {
            using (var writer = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    graph.SerializeToGraphML<TestVertex, TestEdge, TestGraph>(
                        xmlWriter,
                        vertex => vertex.ID,
                        edge => edge.ID);
                }

                return writer.ToString();
            }
        }

        [Pure]
        private static TestGraph DeserializeGraph(string graphml)
        {
            using (var reader = new StringReader(graphml))
            {
                var serializedGraph = new TestGraph();
                serializedGraph.DeserializeAndValidateFromGraphML(
                    reader,
                    id => new TestVertex(id),
                    (source, target, id) => new TestEdge(source, target, id));
                return serializedGraph;
            }
        }

        [Pure]
        private static TestGraph VerifySerialization(
            TestGraph graph,
            Func<TestGraph, string> serializeGraph)
        {
            return VerifySerialization(graph, serializeGraph, DeserializeGraph);
        }

        #endregion


        private static IEnumerable<TestCaseData> GraphSerializationTestCases
        {
            get
            {
                // Second parameter is used to know if serialization
                // will keep objects Ids or use other ones

                Func<TestGraph, string> serialize1 = SerializeGraph1;
                yield return new TestCaseData(serialize1, false);

                Func<TestGraph, string> serialize2 = SerializeGraph2;
                yield return new TestCaseData(serialize2, true);

                Func<TestGraph, string> serialize3 = SerializeGraph3;
                yield return new TestCaseData(serialize3, false);

                Func<TestGraph, string> serialize4 = SerializeGraph4;
                yield return new TestCaseData(serialize4, true);
            }
        }

        [TestCaseSource(nameof(GraphSerializationTestCases))]
        public void GraphMLSerializationWithValidation_WriteVertex(
            Func<TestGraph, string> serializeGraph,
            bool _)
        {
            var graph = new TestGraph
            {
                Bool = true,
                Double = 1.0,
                Float = 2.0F,
                Int = 10,
                Long = 100,
                String = "foo",
                BoolArray = Bools,
                IntArray = Ints,
                LongArray = Longs,
                FloatArray = Floats,
                DoubleArray = Doubles,
                StringArray = Strings,
                NullArray = null,
                BoolIList = BoolsList,
                IntIList = IntsList,
                LongIList = LongsList,
                FloatIList = FloatsList,
                DoubleIList = DoublesList,
                StringIList = StringsList
            };

            var vertex1 = new TestVertex("v1")
            {
                StringDefault = "bar",
                String = "string",
                Long = 20,
                Float = 25.0F,
                Double = 30.0,
                Bool = true,
                IntArray = new[] { 1, 2, 3, 4 },
                IntIList = new[] { 4, 5, 6, 7 }
            };

            var vertex2 = new TestVertex("v2") { StringDefault = null, String = "", Int = 42, IntArray = null };

            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);

            TestGraph serializedGraph = VerifySerialization(graph, serializeGraph);

            Assert.That(graph.Bool, Is.EqualTo(serializedGraph.Bool));
            Assert.That(graph.Double, Is.EqualTo(serializedGraph.Double));
            Assert.That(graph.Float, Is.EqualTo(serializedGraph.Float));
            Assert.That(graph.Int, Is.EqualTo(serializedGraph.Int));
            Assert.That(graph.Long, Is.EqualTo(serializedGraph.Long));
            Assert.That(graph.String, Is.EqualTo(serializedGraph.String));
            CollectionAssert.AreEqual(graph.BoolArray, serializedGraph.BoolArray);
            CollectionAssert.AreEqual(graph.IntArray, serializedGraph.IntArray);
            CollectionAssert.AreEqual(graph.LongArray, serializedGraph.LongArray);
            CollectionAssert.AreEqual(graph.StringArray, serializedGraph.StringArray);
            CollectionAssert.AreEqual(graph.FloatArray, serializedGraph.FloatArray, new FloatComparer(0.001F));
            CollectionAssert.AreEqual(graph.DoubleArray, serializedGraph.DoubleArray, new DoubleComparer(0.0001));
            CollectionAssert.AreEqual(graph.BoolIList, serializedGraph.BoolIList);
            CollectionAssert.AreEqual(graph.IntIList, serializedGraph.IntIList);
            CollectionAssert.AreEqual(graph.LongIList, serializedGraph.LongIList);
            CollectionAssert.AreEqual(graph.StringIList, serializedGraph.StringIList);
            CollectionAssert.AreEqual(graph.FloatIList, serializedGraph.FloatIList, new FloatComparer(0.001F));
            CollectionAssert.AreEqual(graph.DoubleIList, serializedGraph.DoubleIList, new DoubleComparer(0.0001));

            TestVertex serializedVertex1 = serializedGraph.Vertices.First();
            Assert.That(vertex1.StringDefault, Is.EqualTo(serializedVertex1.StringDefault));
            Assert.That(vertex1.String, Is.EqualTo(serializedVertex1.String));
            Assert.That(vertex1.Int, Is.EqualTo(serializedVertex1.Int));
            Assert.That(vertex1.Long, Is.EqualTo(serializedVertex1.Long));
            Assert.That(vertex1.Float, Is.EqualTo(serializedVertex1.Float));
            Assert.That(vertex1.Double, Is.EqualTo(serializedVertex1.Double));
            Assert.That(vertex1.Bool, Is.EqualTo(serializedVertex1.Bool));
            CollectionAssert.AreEqual(vertex1.IntArray, serializedVertex1.IntArray);
            CollectionAssert.AreEqual(vertex1.IntIList, serializedVertex1.IntIList);

            TestVertex serializedVertex2 = serializedGraph.Vertices.Last();
            Assert.That(vertex2.StringDefault, Is.EqualTo(serializedVertex2.StringDefault));
            Assert.That(vertex2.String, Is.EqualTo(serializedVertex2.String));
            Assert.That(vertex2.Int, Is.EqualTo(serializedVertex2.Int));
            Assert.That(vertex2.Long, Is.EqualTo(serializedVertex2.Long));
            Assert.That(vertex2.Float, Is.EqualTo(serializedVertex2.Float));
            Assert.That(vertex2.Double, Is.EqualTo(serializedVertex2.Double));
            Assert.That(vertex2.Bool, Is.EqualTo(serializedVertex2.Bool));
            CollectionAssert.AreEqual(vertex2.IntArray!, serializedVertex2.IntArray);
            CollectionAssert.AreEqual(vertex2.IntIList, serializedVertex2.IntIList);
        }

        [TestCaseSource(nameof(GraphSerializationTestCases))]
        public void GraphMLSerializationWithValidation_WriteEdge(
            Func<TestGraph, string> serializeGraph,
            bool keepIds)
        {
            var graph = new TestGraph
            {
                Bool = true,
                Double = 1.0,
                Float = 2.0F,
                Int = 10,
                Long = 100,
                String = "foo"
            };

            var vertex1 = new TestVertex("v1")
            {
                StringDefault = "foo",
                String = "string",
                Int = 10,
                Long = 20,
                Float = 25.0F,
                Double = 30.0,
                Bool = true
            };

            var vertex2 = new TestVertex("v2")
            {
                StringDefault = "bar",
                String = "string2",
                Int = 110,
                Long = 120,
                Float = 125.0F,
                Double = 130.0,
                Bool = true
            };

            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);

            var edge1 = new TestEdge(
                vertex1, vertex2,
                "e1",
                "edge",
                90,
                100,
                25.0F,
                110.0,
                true);

            var edge2 = new TestEdge(vertex1, vertex2, "e2") { Float = 13.4f, Long = 42L };

            graph.AddEdge(edge1);
            graph.AddEdge(edge2);

            TestGraph serializedGraph = VerifySerialization(graph, serializeGraph);

            TestEdge serializedEdge1 = serializedGraph.Edges.First();
            if (keepIds)
            {
                Assert.That(edge1.ID, Is.EqualTo(serializedEdge1.ID));
            }

            Assert.That(edge1.String, Is.EqualTo(serializedEdge1.String));
            Assert.That(edge1.Int, Is.EqualTo(serializedEdge1.Int));
            Assert.That(edge1.Long, Is.EqualTo(serializedEdge1.Long));
            Assert.That(edge1.Float, Is.EqualTo(serializedEdge1.Float));
            Assert.That(edge1.Double, Is.EqualTo(serializedEdge1.Double));
            Assert.That(edge1.Bool, Is.EqualTo(serializedEdge1.Bool));

            TestEdge serializedEdge2 = serializedGraph.Edges.Last();
            if (keepIds)
            {
                Assert.That(edge2.ID, Is.EqualTo(serializedEdge2.ID));
            }

            Assert.That(edge2.String, Is.EqualTo(serializedEdge2.String));
            Assert.That(edge2.Int, Is.EqualTo(serializedEdge2.Int));
            Assert.That(edge2.Long, Is.EqualTo(serializedEdge2.Long));
            Assert.That(edge2.Float, Is.EqualTo(serializedEdge2.Float));
            Assert.That(edge2.Double, Is.EqualTo(serializedEdge2.Double));
            Assert.That(edge2.Bool, Is.EqualTo(serializedEdge2.Bool));
        }

        [Test]
        public void DeserializeAndValidateFromGraphML_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var graph = new AdjacencyGraph<string, Edge<string>>();

            // Text reader
            Assert.Throws<ArgumentNullException>(() => graph.DeserializeAndValidateFromGraphML(
                null,
                id => id,
                (source, target, _) => new Edge<string>(source, target)));

            using (var reader = new StreamReader(GetGraphFilePath(TestGraphFileName)))
            {
                Assert.Throws<ArgumentNullException>(() =>
                    ((AdjacencyGraph<string, Edge<string>>)null).DeserializeAndValidateFromGraphML(
                        reader,
                        id => id,
                        (source, target, _) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(() =>
                    graph.DeserializeAndValidateFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        null,
                        (source, target, _) => new Edge<string>(source, target)));

                Assert.Throws<ArgumentNullException>(() =>
                    graph.DeserializeAndValidateFromGraphML<string, Edge<string>, AdjacencyGraph<string, Edge<string>>>(
                        reader,
                        id => id,
                        null));
            }
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Serialization/Deserialization

        #region Test Helpers

        [Pure]
        private static int DeserializeVertex_Simple(string id)
        {
            return int.Parse(id);
        }

        [Pure]
        private static EquatableEdge<int> DeserializeEdge_Simple(int source, int target, string id)
        {
            return new EquatableEdge<int>(source, target);
        }

        [Pure]
        private static SEquatableEdge<int> DeserializeSEdge_Simple(int source, int target, string id)
        {
            return new SEquatableEdge<int>(source, target);
        }

        [Pure]
        private static EquatableTestVertex DeserializeVertex_Complex(string id)
        {
            return new EquatableTestVertex(id);
        }

        [Pure]
        private static EquatableTestEdge DeserializeEdge_Complex(EquatableTestVertex source, EquatableTestVertex target,
            string id)
        {
            return new EquatableTestEdge(source, target, id);
        }

        [Pure]
        private static SEquatableEdge<EquatableTestVertex> DeserializeSEdge_Complex(EquatableTestVertex source,
            EquatableTestVertex target, string id)
        {
            return new SEquatableEdge<EquatableTestVertex>(source, target);
        }

        [Pure]
        private static TOutGraph SerializeDeserialize<TVertex, TInEdge, TOutEdge, TInGraph, TOutGraph>(
            TInGraph graph,
            VertexIdentity<TVertex> vertexIdentity,
            Func<XmlReader, TOutGraph> deserialize)
            where TInEdge : IEdge<TVertex>, IEquatable<TInEdge>
            where TOutEdge : IEdge<TVertex>, IEquatable<TOutEdge>
            where TInGraph : IEdgeListGraph<TVertex, TInEdge>
            where TOutGraph : IEdgeListGraph<TVertex, TOutEdge>
        {
            Assert.That(graph, Is.Not.Null);

            using (var stream = new MemoryStream())
            {
                // Serialize
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    graph.SerializeToGraphML(
                        writer,
                        vertexIdentity,
                        graph.GetEdgeIdentity());
                }

                stream.Position = 0;

                // Deserialize
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    TOutGraph deserializedGraph = deserialize(reader);
                    Assert.That(deserializedGraph, Is.Not.Null);
                    Assert.That(graph, Is.Not.SameAs(deserializedGraph));
                    return deserializedGraph;
                }
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
                deserialize);
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Simple<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, EquatableEdge<int>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, EquatableEdge<int>>, new()
        {
            return SerializeDeserialize_Simple<EquatableEdge<int>, EquatableEdge<int>, TInGraph, TOutGraph>(graph,
                reader =>
                {
                    var deserializedGraph = new TOutGraph();
                    deserializedGraph.DeserializeFromGraphML(
                        reader,
                        DeserializeVertex_Simple,
                        DeserializeEdge_Simple);
                    return deserializedGraph;
                });
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Complex<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<EquatableTestVertex, EquatableTestEdge>
            where TOutGraph : class, IMutableVertexAndEdgeSet<EquatableTestVertex, EquatableTestEdge>, new()
        {
            return SerializeDeserialize_Complex<EquatableTestEdge, EquatableTestEdge, TInGraph, TOutGraph>(graph,
                reader =>
                {
                    var deserializedGraph = new TOutGraph();
                    deserializedGraph.DeserializeFromGraphML(
                        reader,
                        DeserializeVertex_Complex,
                        DeserializeEdge_Complex);
                    return deserializedGraph;
                });
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_SEdge_Simple<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, SEquatableEdge<int>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, SEquatableEdge<int>>, new()
        {
            return SerializeDeserialize_Simple<SEquatableEdge<int>, SEquatableEdge<int>, TInGraph, TOutGraph>(graph,
                reader =>
                {
                    var deserializedGraph = new TOutGraph();
                    deserializedGraph.DeserializeFromGraphML(
                        reader,
                        DeserializeVertex_Simple,
                        DeserializeSEdge_Simple);
                    return deserializedGraph;
                });
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_SEdge_Complex<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<EquatableTestVertex, SEquatableEdge<EquatableTestVertex>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<EquatableTestVertex, SEquatableEdge<EquatableTestVertex>>,
            new()
        {
            return SerializeDeserialize_Complex<SEquatableEdge<EquatableTestVertex>, SEquatableEdge<EquatableTestVertex>
                , TInGraph, TOutGraph>(graph, reader =>
            {
                var deserializedGraph = new TOutGraph();
                deserializedGraph.DeserializeFromGraphML(
                    reader,
                    DeserializeVertex_Complex,
                    DeserializeSEdge_Complex);
                return deserializedGraph;
            });
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Reversed_Simple<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<int, SReversedEdge<int, EquatableEdge<int>>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<int, EquatableEdge<int>>, new()
        {
            return SerializeDeserialize_Simple<SReversedEdge<int, EquatableEdge<int>>, EquatableEdge<int>, TInGraph,
                TOutGraph>(graph, reader =>
            {
                var deserializedGraph = new TOutGraph();
                deserializedGraph.DeserializeFromGraphML(
                    reader,
                    DeserializeVertex_Simple,
                    DeserializeEdge_Simple);
                return deserializedGraph;
            });
        }

        [Pure]
        private static TOutGraph SerializeDeserialize_Reversed_Complex<TInGraph, TOutGraph>(TInGraph graph)
            where TInGraph : IEdgeListGraph<EquatableTestVertex, SReversedEdge<EquatableTestVertex, EquatableTestEdge>>
            where TOutGraph : class, IMutableVertexAndEdgeSet<EquatableTestVertex, EquatableTestEdge>, new()
        {
            return SerializeDeserialize_Complex<SReversedEdge<EquatableTestVertex, EquatableTestEdge>, EquatableTestEdge
                , TInGraph, TOutGraph>(graph, reader =>
            {
                var deserializedGraph = new TOutGraph();
                deserializedGraph.DeserializeFromGraphML(
                    reader,
                    DeserializeVertex_Complex,
                    DeserializeEdge_Complex);
                return deserializedGraph;
            });
        }

        #endregion

        // TODO Support heterogeneous types for vertices and edges

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationAdjacencyGraphTestCases))]
        public void GraphMLSerialization_AdjacencyGraph_Simple(AdjacencyGraph<int, EquatableEdge<int>> graph)
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

        [TestCaseSource(typeof(SerializationTestCaseSources),
            nameof(SerializationAdjacencyGraphComplexNotHeterogeneousTestCases))]
        public void GraphMLSerialization_AdjacencyGraph_Complex(
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
        public void GraphMLSerialization_AdapterGraph_Simple(AdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            var bidirectionalAdapterGraph = new BidirectionalAdapterGraph<int, EquatableEdge<int>>(graph);
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_Simple<BidirectionalAdapterGraph<int, EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(bidirectionalAdapterGraph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources),
            nameof(SerializationAdjacencyGraphComplexNotHeterogeneousTestCases))]
        public void GraphMLSerialization_AdapterGraph_Complex(
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> graph)
        {
            var bidirectionalAdapterGraph =
                new BidirectionalAdapterGraph<EquatableTestVertex, EquatableTestEdge>(graph);
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph =
                SerializeDeserialize_Complex<BidirectionalAdapterGraph<EquatableTestVertex, EquatableTestEdge>,
                    AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>>(bidirectionalAdapterGraph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationClusteredAdjacencyGraphTestCases))]
        public void GraphMLSerialization_ClusteredGraph_Simple(ClusteredAdjacencyGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_Simple<ClusteredAdjacencyGraph<int, EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources),
            nameof(SerializationClusteredAdjacencyGraphComplexNotHeterogeneousTestCases))]
        public void GraphMLSerialization_ClusteredGraph_Complex(
            ClusteredAdjacencyGraph<EquatableTestVertex, EquatableTestEdge> graph)
        {
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph =
                SerializeDeserialize_Complex<ClusteredAdjacencyGraph<EquatableTestVertex, EquatableTestEdge>,
                    AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationCompressedGraphTestCases))]
        public void GraphMLSerialization_CompressedGraph_Simple(CompressedSparseRowGraph<int> graph)
        {
            AdjacencyGraph<int, SEquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_SEdge_Simple<CompressedSparseRowGraph<int>,
                    AdjacencyGraph<int, SEquatableEdge<int>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources),
            nameof(SerializationCompressedGraphComplexNotHeterogeneousTestCases))]
        public void GraphMLSerialization_CompressedGraph_Complex(CompressedSparseRowGraph<EquatableTestVertex> graph)
        {
            AdjacencyGraph<EquatableTestVertex, SEquatableEdge<EquatableTestVertex>> deserializedGraph =
                SerializeDeserialize_SEdge_Complex<CompressedSparseRowGraph<EquatableTestVertex>,
                    AdjacencyGraph<EquatableTestVertex, SEquatableEdge<EquatableTestVertex>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationBidirectionalGraphTestCases))]
        public void GraphMLSerialization_BidirectionalGraph_Simple(BidirectionalGraph<int, EquatableEdge<int>> graph)
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

        [TestCaseSource(typeof(SerializationTestCaseSources),
            nameof(SerializationBidirectionalGraphComplexNotHeterogeneousTestCases))]
        public void GraphMLSerialization_BidirectionalGraph_Complex(
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
                        // TODO SReversedEdge<T> original edge is not parsed by the serializer so data are not serialized
                        (edge1, edge2) =>
                            Equals(edge1.Source, edge2.Target) &&
                            Equals(edge1.Target, edge2.Source) /* && edge1.DataContentEquals(edge2)*/,
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
        public void GraphMLSerialization_BidirectionalMatrixGraph(BidirectionalMatrixGraph<EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_Simple<BidirectionalMatrixGraph<EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources), nameof(SerializationUndirectedGraphTestCases))]
        public void GraphMLSerialization_UndirectedGraph_Simple(UndirectedGraph<int, EquatableEdge<int>> graph)
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

        [TestCaseSource(typeof(SerializationTestCaseSources),
            nameof(SerializationUndirectedGraphComplexNotHeterogeneousTestCases))]
        public void GraphMLSerialization_UndirectedGraph_Complex(
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
        public void GraphMLSerialization_EdgeListGraph_Simple(EdgeListGraph<int, EquatableEdge<int>> graph)
        {
            AdjacencyGraph<int, EquatableEdge<int>> deserializedGraph =
                SerializeDeserialize_Simple<EdgeListGraph<int, EquatableEdge<int>>,
                    AdjacencyGraph<int, EquatableEdge<int>>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        [TestCaseSource(typeof(SerializationTestCaseSources),
            nameof(SerializationEdgeListGraphComplexNotHeterogeneousTestCases))]
        public void GraphMLSerialization_EdgeListGraph_Complex(
            EdgeListGraph<EquatableTestVertex, EquatableTestEdge> graph)
        {
            AdjacencyGraph<EquatableTestVertex, EquatableTestEdge> deserializedGraph =
                SerializeDeserialize_Complex<EdgeListGraph<EquatableTestVertex, EquatableTestEdge>,
                    AdjacencyGraph<EquatableTestVertex, EquatableTestEdge>>(graph);
            Assert.That(EquateGraphs.Equate(graph, deserializedGraph), Is.True);
        }

        #endregion
    }
}