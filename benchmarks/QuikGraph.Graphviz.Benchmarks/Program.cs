using BenchmarkDotNet.Running;

namespace QuikGraph.Graphviz.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<GraphvizBenchmarks>();
        }
    }
}
