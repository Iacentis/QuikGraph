using System.IO;
using System.Text;
using System.Xml;
using BenchmarkDotNet.Attributes;

namespace QuikGraph.Serialization.Benchmarks
{
    [MemoryDiagnoser]
    public class GraphMLBenchmarks
    {
        private AdjacencyGraph<int, Edge<int>> _graph;
        private string _graphXml;

        [Params(10, 100)] public int VertexCount;

        [GlobalSetup]
        public void Setup()
        {
            _graph = new AdjacencyGraph<int, Edge<int>>();
            for (int i = 0; i < VertexCount; i++)
            {
                _graph.AddVertex(i);
            }

            for (int i = 0; i < VertexCount - 1; i++)
            {
                _graph.AddEdge(new Edge<int>(i, i + 1));
            }

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                _graph.SerializeToGraphML<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(writer);
            }

            _graphXml = sb.ToString();
        }

        [Benchmark]
        public string Serialize()
        {
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                _graph.SerializeToGraphML<int, Edge<int>, AdjacencyGraph<int, Edge<int>>>(writer);
            }

            return sb.ToString();
        }

        [Benchmark]
        public AdjacencyGraph<int, Edge<int>> Deserialize()
        {
            var graph = new AdjacencyGraph<int, Edge<int>>();
            using (var reader = new StringReader(_graphXml))
            using (var xmlReader = XmlReader.Create(reader))
            {
                graph.DeserializeFromGraphML(xmlReader,
                    int.Parse,
                    (source, target, id) => new Edge<int>(source, target));
            }

            return graph;
        }
    }
}