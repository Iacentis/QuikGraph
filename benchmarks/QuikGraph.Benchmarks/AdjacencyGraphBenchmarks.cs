using System.Linq;
using BenchmarkDotNet.Attributes;

namespace QuikGraph.Benchmarks
{
    [MemoryDiagnoser]
    public class AdjacencyGraphBenchmarks
    {
        private AdjacencyGraph<int, Edge<int>> _graph;

        [Params(100, 1000)]
        public int VertexCount;

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
        }

        [Benchmark]
        public bool ContainsVertex()
        {
            return _graph.ContainsVertex(VertexCount / 2);
        }

        [Benchmark]
        public int OutDegree()
        {
            return _graph.OutDegree(VertexCount / 2);
        }

        [Benchmark]
        public Edge<int>[] OutEdges()
        {
            return _graph.OutEdges(VertexCount / 2).ToArray();
        }
    }
}
