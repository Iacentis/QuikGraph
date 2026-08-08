using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;
using QuikGraph.Algorithms;
using QuikGraph.Serialization.DirectedGraphML;
using QuikGraph.Tests;
using static QuikGraph.Tests.QuikGraphUnitTestsHelpers;

namespace QuikGraph.Serialization.Tests
{
    /// <summary>
    /// Tests for <see cref="DirectedGraphMLExtensions"/>.
    /// </summary>
    [TestFixture]
    internal sealed class DirectedGraphMLExtensionsTests
    {
        #region Test helpers

        private static void SerializeAndRead(
            DirectedGraph graph,
            Func<DirectedGraph, string> onSerialize,
            Action<string> checkSerializedContent)
        {
            string xml = onSerialize(graph);
            checkSerializedContent(xml);
        }

        private static void AssertGraphContentEquivalent<TEdge>(
            IEdgeListGraph<string, TEdge> graph,
            DirectedGraph directedGraph)
            where TEdge : IEdge<string>
        {
            // Vertices
            Assert.That(graph.VertexCount, Is.EqualTo(directedGraph.Nodes.Length));

            string[] expectedNodes = graph.Vertices.ToArray();
            for (int i = 0; i < graph.VertexCount; ++i)
            {
                Assert.That(expectedNodes[i], Is.EqualTo(directedGraph.Nodes[i].Id));
            }

            // Edges
            Assert.That(graph.EdgeCount, Is.EqualTo(directedGraph.Links.Length));

            TEdge[] expectedEdges = graph.Edges.ToArray();
            for (int i = 0; i < graph.EdgeCount; ++i)
            {
                Assert.That(expectedEdges[i].Source, Is.EqualTo(directedGraph.Links[i].Source));
                Assert.That(expectedEdges[i].Target, Is.EqualTo(directedGraph.Links[i].Target));
            }
        }

        #endregion


        private const string XmlHeaderRegex = @"<\?xml\ version=""1\.0""(\ encoding=""utf-(8|16)""|)\?>";


        private static readonly string WriteThrowsTestFilePath =
            Path.Combine(GetTemporaryTestDirectory(), "serialization_from_directegraph_to_xml_throws_test.xml");


        private static IEnumerable<TestCaseData> WriteXmlTestCases
        {
            get
            {
                Func<DirectedGraph, string> serialize1 = graph =>
                {
                    string filePath =
                        Path.Combine(GetTemporaryTestDirectory(), "serialization_from_directegraph_to_xml_test.xml");

                    graph.WriteXml(filePath);
                    Assert.That(File.Exists(filePath), Is.True);
                    return File.ReadAllText(filePath);
                };
                yield return new TestCaseData(serialize1);

                Func<DirectedGraph, string> serialize2 = graph =>
                {
                    using (var writer = new StringWriter())
                    {
                        var settings = new XmlWriterSettings { Indent = true };
                        using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                        {
                            graph.WriteXml(xmlWriter);
                        }

                        return writer.ToString();
                    }
                };
                yield return new TestCaseData(serialize2);

                Func<DirectedGraph, string> serialize3 = graph =>
                {
                    using (var writer = new MemoryStream())
                    {
                        graph.WriteXml(writer);

                        writer.Position = 0;

                        using (var reader = new StreamReader(writer))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                };
                yield return new TestCaseData(serialize3);

                Func<DirectedGraph, string> serialize4 = graph =>
                {
                    using (var memory = new MemoryStream())
                    using (var writer = new StreamWriter(memory))
                    {
                        graph.WriteXml(writer);

                        memory.Position = 0;

                        using (var reader = new StreamReader(memory))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                };
                yield return new TestCaseData(serialize4);
            }
        }

        [TestCaseSource(nameof(WriteXmlTestCases))]
        public void WriteXml(
            Func<DirectedGraph, string> onSerialize)
        {
            var graph = new DirectedGraph();
            var persons = new List<DirectedGraphNode>();
            var jacob = new DirectedGraphNode { Id = "Jacob" };
            persons.Add(jacob);

            var john = new DirectedGraphNode { Id = "John" };
            persons.Add(john);

            var jonathon = new DirectedGraphNode { Id = "Jonathon" };
            persons.Add(jonathon);

            var emanuel = new DirectedGraphNode { Id = "Emanuel" };
            persons.Add(emanuel);

            var relations = new List<DirectedGraphLink>
            {
                new DirectedGraphLink { Source = jacob.Id, Target = john.Id },
                new DirectedGraphLink { Source = john.Id, Target = jonathon.Id },
                new DirectedGraphLink { Source = jonathon.Id, Target = emanuel.Id }
            };

            graph.Nodes = persons.ToArray();
            graph.Links = relations.ToArray();

            SerializeAndRead(
                graph,
                onSerialize,
                content =>
                {
                    var graphContent = new StringBuilder();

                    graphContent.Append(@"<Nodes>\s*");
                    foreach (DirectedGraphNode node in graph.Nodes)
                    {
                        graphContent.Append(
                            $@"<Node\s*Id=""{node.Id}""\s*\/>\s*");
                    }

                    graphContent.Append(@"<\/Nodes>\s*");

                    graphContent.Append(@"<Links>\s*");
                    foreach (DirectedGraphLink link in graph.Links)
                    {
                        graphContent.Append(
                            $@"<Link\s*Source=""{link.Source}""\s*Target=""{link.Target}""\s*\/>\s*");
                    }

                    graphContent.Append(@"<\/Links>");

                    var regex = new Regex(
                        $@"{XmlHeaderRegex}\s*<DirectedGraph\s*.*?\s*>\s*{graphContent}\s*<\/DirectedGraph>");
                    Assert.That(regex.Match(content).Success, Is.True);
                });
        }

        [Test]
        public void WriteXml_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            var directedGraph = new DirectedGraph();

            // Filepath
            Assert.Throws<ArgumentNullException>(() => ((DirectedGraph)null).WriteXml(WriteThrowsTestFilePath));
            Assert.Throws<ArgumentException>(() => directedGraph.WriteXml((string)null));
            Assert.Throws<ArgumentException>(() => directedGraph.WriteXml(""));
            Assert.Throws<ArgumentException>(() => ((DirectedGraph)null).WriteXml((string)null));
            Assert.Throws<ArgumentException>(() => ((DirectedGraph)null).WriteXml(""));

            // XML writer
            using (XmlWriter writer = XmlWriter.Create(WriteThrowsTestFilePath))
            {
                Assert.Throws<ArgumentNullException>(() => ((DirectedGraph)null).WriteXml(writer));
                Assert.Throws<ArgumentNullException>(() => directedGraph.WriteXml((XmlWriter)null));
                Assert.Throws<ArgumentNullException>(() => ((DirectedGraph)null).WriteXml((XmlWriter)null));
            }

            // Stream
            using (var writer = new MemoryStream())
            {
                Assert.Throws<ArgumentNullException>(() => ((DirectedGraph)null).WriteXml(writer));
                Assert.Throws<ArgumentNullException>(() => directedGraph.WriteXml((Stream)null));
                Assert.Throws<ArgumentNullException>(() => ((DirectedGraph)null).WriteXml((Stream)null));
            }

            // TextWriter
            using (var writer = new StreamWriter(WriteThrowsTestFilePath))
            {
                Assert.Throws<ArgumentNullException>(() => ((DirectedGraph)null).WriteXml(writer));
                Assert.Throws<ArgumentNullException>(() => directedGraph.WriteXml((TextWriter)null));
                Assert.Throws<ArgumentNullException>(() => ((DirectedGraph)null).WriteXml((TextWriter)null));
            }
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void ToDirectedGraphML()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
            {
                DirectedGraph directedGraph = graph.ToDirectedGraphML();
                Assert.That(graph, Is.Not.Null);

                AssertGraphContentEquivalent(graph, directedGraph);
            }
        }

        [Test]
        public void ToDirectedGraphML_WithIdentity()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
            {
                int i = 0;
                DirectedGraph directedGraph = graph.ToDirectedGraphML(
                    vertex => vertex,
                    _ => (++i).ToString());
                Assert.That(graph, Is.Not.Null);

                AssertGraphContentEquivalent(graph, directedGraph);
            }
        }

        [Test]
        public void ToDirectedGraphML_WithColors()
        {
            var random = new Random(123456);
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
            {
                Dictionary<string, GraphColor> verticesColors = graph.Vertices.ToDictionary(
                    vertex => vertex,
                    _ => (GraphColor)random.Next(0, 3));

                DirectedGraph directedGraph = graph.ToDirectedGraphML(vertex =>
                {
                    Assert.That(vertex, Is.Not.Null);
                    return verticesColors[vertex];
                });
                Assert.That(graph, Is.Not.Null);

                AssertGraphContentEquivalent(graph, directedGraph);

                foreach (DirectedGraphNode node in directedGraph.Nodes)
                {
                    Assert.That(
                        ColorToStringColor(verticesColors[node.Id]),
                        Is.EqualTo(node.Background));
                }
            }

            #region Local function

            string ColorToStringColor(GraphColor color)
            {
                switch (color)
                {
                    case GraphColor.Black:
                        return "Black";
                    case GraphColor.Gray:
                        return "LightGray";
                    case GraphColor.White:
                        return "White";
                    default:
                        Assert.Fail("Unknown color.");
                        return string.Empty;
                }
            }

            #endregion
        }

        [Test]
        public void ToDirectedGraphML_WithFormat()
        {
            foreach (AdjacencyGraph<string, Edge<string>> graph in TestGraphFactory.GetAdjacencyGraphs_All())
            {
                int formattedNodes = 0;
                int formattedEdges = 0;
                // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
                DirectedGraph directedGraph = graph.ToDirectedGraphML(
                    graph.GetVertexIdentity(),
                    graph.GetEdgeIdentity(),
                    (vertex, node) =>
                    {
                        Assert.That(vertex, Is.Not.Null);
                        Assert.That(node, Is.Not.Null);
                        ++formattedNodes;
                    },
                    (edge, link) =>
                    {
                        Assert.That(edge, Is.Not.Null);
                        Assert.That(link, Is.Not.Null);
                        ++formattedEdges;
                    });
                // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Local
                Assert.That(graph, Is.Not.Null);

                Assert.That(graph.VertexCount, Is.EqualTo(formattedNodes));
                Assert.That(graph.EdgeCount, Is.EqualTo(formattedEdges));
                AssertGraphContentEquivalent(graph, directedGraph);
            }
        }

        [Test]
        public void ToDirectedGraphML_Throws()
        {
            var graph = new AdjacencyGraph<string, Edge<string>>();
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() =>
                ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML());


            Assert.Throws<ArgumentNullException>(() =>
                ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML(_ => GraphColor.Black));
            Assert.Throws<ArgumentNullException>(() => graph.ToDirectedGraphML(null));
            Assert.Throws<ArgumentNullException>(() =>
                ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML(null));


            Assert.Throws<ArgumentNullException>(() => ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML(
                vertex => vertex,
                edge => $"{edge.Source}_{edge.Target}"));
            Assert.Throws<ArgumentNullException>(() => ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML(
                null,
                edge => $"{edge.Source}_{edge.Target}"));
            Assert.Throws<ArgumentNullException>(() => ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML(
                vertex => vertex,
                null));
            Assert.Throws<ArgumentNullException>(() => graph.ToDirectedGraphML(
                null,
                null));
            Assert.Throws<ArgumentNullException>(() => ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML(
                null,
                null));


            Assert.Throws<ArgumentNullException>(() => ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML(
                vertex => vertex,
                edge => $"{edge.Source}_{edge.Target}",
                null, null));
            Assert.Throws<ArgumentNullException>(() => ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML(
                null,
                edge => $"{edge.Source}_{edge.Target}",
                null, null));
            Assert.Throws<ArgumentNullException>(() => ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML(
                vertex => vertex,
                null,
                null, null));
            Assert.Throws<ArgumentNullException>(() => graph.ToDirectedGraphML(
                null,
                null,
                null, null));
            Assert.Throws<ArgumentNullException>(() => ((AdjacencyGraph<string, Edge<string>>)null).ToDirectedGraphML(
                null,
                null,
                null, null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}