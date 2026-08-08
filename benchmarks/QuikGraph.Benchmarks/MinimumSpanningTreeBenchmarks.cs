using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.MinimumSpanningTree;

namespace QuikGraph.Benchmarks
{
    [MemoryDiagnoser]
    public class MinimumSpanningTreeBenchmarks
    {
        private UndirectedGraph<int, Edge<int>> _graph;
        private Dictionary<Edge<int>, double> _distances;

        [Params(100, 1000)]
        public int VertexCount;

        [Params(2, 5, 10)]
        public int EdgeRatio;

        [GlobalSetup]
        public void Setup()
        {
            _graph = new UndirectedGraph<int, Edge<int>>();
            var rng = new Random(42);
            int edgeCount = VertexCount * EdgeRatio;
            RandomGraphFactory.Create(
                _graph,
                () => _graph.VertexCount,
                (s, t) => new Edge<int>(s, t),
                rng,
                VertexCount,
                edgeCount,
                false);

            _distances = new Dictionary<Edge<int>, double>(_graph.EdgeCount);
            foreach (var edge in _graph.Edges)
            {
                _distances[edge] = rng.NextDouble();
            }
        }

        [Benchmark]
        public void Kruskal()
        {
            var kruskal = new KruskalMinimumSpanningTreeAlgorithm<int, Edge<int>>(_graph, e => _distances[e]);
            kruskal.Compute();
        }

        [Benchmark]
        public void Prim()
        {
            var prim = new PrimMinimumSpanningTreeAlgorithm<int, Edge<int>>(_graph, e => _distances[e]);
            prim.Compute();
        }
    }
}
