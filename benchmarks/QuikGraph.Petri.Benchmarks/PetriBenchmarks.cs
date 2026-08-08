using BenchmarkDotNet.Attributes;

namespace QuikGraph.Petri.Benchmarks
{
    [MemoryDiagnoser]
    public class PetriBenchmarks
    {
        private PetriNet<int> _net;
        private PetriNetSimulator<int> _simulator;

        [Params(10, 50)] public int TransitionCount;

        [GlobalSetup]
        public void Setup()
        {
            _net = new PetriNet<int>();
            var places = new IPlace<int>[TransitionCount + 1];
            for (int i = 0; i <= TransitionCount; i++)
            {
                places[i] = _net.AddPlace($"P{i}");
            }

            places[0].Marking.Add(1);

            for (int i = 0; i < TransitionCount; i++)
            {
                var transition = _net.AddTransition($"T{i}");
                _net.AddArc(places[i], transition);
                _net.AddArc(transition, places[i + 1]);
            }

            _simulator = new PetriNetSimulator<int>(_net);
        }

        [Benchmark]
        public void SimulateStep()
        {
            _simulator.Initialize();
            _simulator.SimulateStep();
        }
    }
}