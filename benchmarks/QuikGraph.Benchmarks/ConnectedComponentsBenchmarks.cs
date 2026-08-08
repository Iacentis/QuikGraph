using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ConnectedComponents;

namespace QuikGraph.Benchmarks
{
    [MemoryDiagnoser]
    public class ConnectedComponentsBenchmarks
    {
        private AdjacencyGraph<int, Edge<int>> _directedGraph;
        private UndirectedGraph<int, Edge<int>> _undirectedGraph;

        [Params(100, 1000)]
        public int VertexCount;

        [Params(2, 5, 10)]
        public int EdgeRatio;

        [GlobalSetup]
        public void Setup()
        {
            var rng = new Random(42);
            int edgeCount = VertexCount * EdgeRatio;

            _directedGraph = new AdjacencyGraph<int, Edge<int>>();
            RandomGraphFactory.Create(
                _directedGraph,
                () => _directedGraph.VertexCount,
                (s, t) => new Edge<int>(s, t),
                rng,
                VertexCount,
                edgeCount,
                false);

            _undirectedGraph = new UndirectedGraph<int, Edge<int>>();
            RandomGraphFactory.Create(
                _undirectedGraph,
                () => _undirectedGraph.VertexCount,
                (s, t) => new Edge<int>(s, t),
                rng,
                VertexCount,
                edgeCount,
                false);
        }

        [Benchmark]
        public void ConnectedComponents()
        {
            var algorithm = new ConnectedComponentsAlgorithm<int, Edge<int>>(_undirectedGraph);
            algorithm.Compute();
        }

        [Benchmark]
        public void StronglyConnectedComponents()
        {
            var algorithm = new StronglyConnectedComponentsAlgorithm<int, Edge<int>>(_directedGraph);
            algorithm.Compute();
        }

        [Benchmark]
        public void WeaklyConnectedComponents()
        {
            var algorithm = new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(_directedGraph);
            algorithm.Compute();
        }
    }
}
