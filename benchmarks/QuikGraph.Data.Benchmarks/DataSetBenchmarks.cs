using System.Data;
using BenchmarkDotNet.Attributes;

namespace QuikGraph.Data.Benchmarks
{
    [MemoryDiagnoser]
    public class DataSetBenchmarks
    {
        private DataSet _dataSet;

        [Params(10, 50)] public int TableCount;

        [GlobalSetup]
        public void Setup()
        {
            _dataSet = new DataSet();
            for (int i = 0; i < TableCount; i++)
            {
                var table = new DataTable($"Table{i}");
                table.Columns.Add("Id", typeof(int));
                _dataSet.Tables.Add(table);
            }

            for (int i = 0; i < TableCount - 1; i++)
            {
                var relation = new DataRelation(
                    $"Relation{i}",
                    _dataSet.Tables[i].Columns["Id"]!,
                    _dataSet.Tables[i + 1].Columns["Id"]!);
                _dataSet.Relations.Add(relation);
            }
        }

        [Benchmark]
        public DataSetGraph ToGraph()
        {
            return _dataSet.ToGraph();
        }
    }
}