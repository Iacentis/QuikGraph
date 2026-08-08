using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.MaximumFlow;

namespace QuikGraph.Benchmarks
{
    [MemoryDiagnoser]
    public class MaximumFlowBenchmarks
    {
        private AdjacencyGraph<int, Edge<int>> _graph;
        private Dictionary<Edge<int>, double> _capacities;
        private ReversedEdgeAugmentorAlgorithm<int, Edge<int>> _augmentor;
        private int _source;
        private int _sink;

        [Params(50, 100)]
        public int VertexCount;

        [Params(2, 5)]
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

            _capacities = new Dictionary<Edge<int>, double>(_graph.EdgeCount);
            foreach (var edge in _graph.Edges)
            {
                _capacities[edge] = rng.Next(1, 100);
            }

            _source = 0;
            _sink = VertexCount - 1;

            _augmentor = new ReversedEdgeAugmentorAlgorithm<int, Edge<int>>(
                _graph,
                (s, t) => new Edge<int>(s, t));
            _augmentor.AddReversedEdges();

            // We need to add capacities for the reversed edges too (initially 0)
            foreach (var edge in _augmentor.ReversedEdges.Values)
            {
                if (!_capacities.ContainsKey(edge))
                {
                    _capacities[edge] = 0;
                }
            }
        }

        [Benchmark]
        public void EdmondsKarp()
        {
            var algorithm = new EdmondsKarpMaximumFlowAlgorithm<int, Edge<int>>(
                _graph,
                e => _capacities[e],
                (s, t) => new Edge<int>(s, t),
                _augmentor);
            algorithm.Compute(_source, _sink);
        }
    }
}
