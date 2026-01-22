using BenchmarkDotNet.Running;

namespace QuikGraph.Serialization.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<GraphMLBenchmarks>();
        }
    }
}
