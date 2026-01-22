using BenchmarkDotNet.Attributes;
using QuikGraph;
using QuikGraph.Graphviz;

namespace QuikGraph.Graphviz.Benchmarks
{
    [MemoryDiagnoser]
    public class GraphvizBenchmarks
    {
        private AdjacencyGraph<int, Edge<int>> _graph;

        [Params(10, 100)]
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
        public string GenerateDot()
        {
            var algorithm = new GraphvizAlgorithm<int, Edge<int>>(_graph);
            return algorithm.Generate();
        }
    }
}
