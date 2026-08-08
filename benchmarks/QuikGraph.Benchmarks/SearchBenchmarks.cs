using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.Search;

namespace QuikGraph.Benchmarks
{
    [MemoryDiagnoser]
    public class SearchBenchmarks
    {
        private AdjacencyGraph<int, Edge<int>> _graph;
        private int _root;

        [Params(100, 1000)]
        public int VertexCount;

        [Params(2, 5, 10)]
        public int EdgeRatio;

        [GlobalSetup]
        public void Setup()
        {
            _graph = new AdjacencyGraph<int, Edge<int>>();
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
            _root = 0;
        }

        [Benchmark]
        public void BreadthFirstSearch()
        {
            var bfs = new BreadthFirstSearchAlgorithm<int, Edge<int>>(_graph);
            bfs.Compute(_root);
        }

        [Benchmark]
        public void DepthFirstSearch()
        {
            var dfs = new DepthFirstSearchAlgorithm<int, Edge<int>>(_graph);
            dfs.Compute(_root);
        }
    }
}
