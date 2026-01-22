using BenchmarkDotNet.Running;

namespace QuikGraph.Petri.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<PetriBenchmarks>();
        }
    }
}
