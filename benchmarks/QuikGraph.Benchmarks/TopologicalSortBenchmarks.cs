using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using QuikGraph.Algorithms.TopologicalSort;

namespace QuikGraph.Benchmarks
{
    [MemoryDiagnoser]
    public class TopologicalSortBenchmarks
    {
        private AdjacencyGraph<int, Edge<int>> _dag;

        [Params(100, 1000)]
        public int VertexCount;

        [Params(2, 5, 10)]
        public int EdgeRatio;

        [GlobalSetup]
        public void Setup()
        {
            _dag = new AdjacencyGraph<int, Edge<int>>();
            for (int i = 0; i < VertexCount; i++)
            {
                _dag.AddVertex(i);
            }

            var rng = new Random(42);
            int edgeCount = VertexCount * EdgeRatio;
            int addedEdges = 0;
            while (addedEdges < edgeCount)
            {
                int u = rng.Next(VertexCount);
                int v = rng.Next(VertexCount);
                if (u < v)
                {
                    if (_dag.AddEdge(new Edge<int>(u, v)))
                    {
                        addedEdges++;
                    }
                }
            }
        }

        [Benchmark]
        public void TopologicalSort()
        {
            var algorithm = new TopologicalSortAlgorithm<int, Edge<int>>(_dag);
            algorithm.Compute();
        }

        [Benchmark]
        public void SourceFirstTopologicalSort()
        {
            var algorithm = new SourceFirstTopologicalSortAlgorithm<int, Edge<int>>(_dag);
            algorithm.Compute();
        }
    }
}
