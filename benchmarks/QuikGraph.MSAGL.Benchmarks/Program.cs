using BenchmarkDotNet.Running;

namespace QuikGraph.MSAGL.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<MsaglBenchmarks>();
        }
    }
}
