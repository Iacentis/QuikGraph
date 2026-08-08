using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Benchmarks
{
    [MemoryDiagnoser]
    public class ShortestPathBenchmarks
    {
        private AdjacencyGraph<int, Edge<int>> _graph;
        private Dictionary<Edge<int>, double> _distances;
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

            _distances = new Dictionary<Edge<int>, double>(_graph.EdgeCount);
            foreach (var edge in _graph.Edges)
            {
                _distances[edge] = rng.NextDouble();
            }

            _root = 0;
        }

        [Benchmark]
        public void Dijkstra()
        {
            var dijkstra = new DijkstraShortestPathAlgorithm<int, Edge<int>>(_graph, e => _distances[e]);
            dijkstra.Compute(_root);
        }

        [Benchmark]
        public void AStar()
        {
            // Using a dummy heuristic for A* (always 0) which makes it equivalent to Dijkstra in terms of nodes visited
            // but still benchmarks the A* implementation overhead.
            var astar = new AStarShortestPathAlgorithm<int, Edge<int>>(_graph, e => _distances[e], v => 0);
            astar.Compute(_root);
        }

        [Benchmark]
        public void BellmanFord()
        {
            var bellmanFord = new BellmanFordShortestPathAlgorithm<int, Edge<int>>(_graph, e => _distances[e]);
            bellmanFord.Compute(_root);
        }
    }
}
