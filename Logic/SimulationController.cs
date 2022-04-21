using BallSimulator.Data;
using System.Threading;
using System.Threading.Tasks;

namespace BallSimulator.Logic
{
    internal class SimulationController : LogicAbstractApi
    {
        public override Ball[] Balls => _simulationManager.Balls;

        private readonly DataAbstractApi _data;
        private readonly SimulationManager _simulationManager;

        private bool _running = false;

        public SimulationController(DataAbstractApi data = default)
        {
            _data = data ?? DataAbstractApi.CreateDataApi();
            _simulationManager = new SimulationManager(new Board(_data.BoardHeight, _data.BoardWidth), _data.BallRadius);
        }

        public override void CreateBalls(int count)
        {
            _simulationManager.RandomBallCreation(count);
        }

        public override void NotifyUpdate()
        {
            _observer.Invoke();
        }

        public override void StartSimulation()
        {
            if (!_running)
            {
                _running = true;
                Task.Run(InvokeSimulation);
            }
        }

        public override void StopSimulation()
        {
            if (_running) _running = false;
        }

        public override void InvokeSimulation()
        {
            while (_running)
            {
                _simulationManager.PushBalls();
                NotifyUpdate();
                Thread.Sleep(10);
            }
        }

        public override void SetObserver(Observer observer)
        {
            _observer = observer;
        }
    }
}
