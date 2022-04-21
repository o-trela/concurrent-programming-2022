using BallSimulator.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BallSimulator.Logic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _data;
        private readonly SimulationManager _simulationManager;

        private bool _run;

        public LogicApi(DataAbstractApi data)
        {
            _data = data;

            Board board = new Board(100, 100);
            _simulationManager = new SimulationManager(board, 10);

            _run = false;
        }

        public override void CreateBalls(int count)
        {
            _simulationManager.RandomBallCreation(count);
        }

        public override Ball[] GetBalls()
        {
            return _simulationManager.Balls;
        }

        public override void InvokeSimulation()
        {
            while (_run)
            {
                _simulationManager.PushBalls();
                NotifyUpdate();
                Thread.Sleep(10);
            }
        }

        public override void NotifyUpdate()
        {
            _observer.Invoke();
        }

        public override void StartSimulation()
        {
            if (!_run)
            {
                _run = true;
                Task.Run(InvokeSimulation);
            }
        }

        public override void StopSimulation()
        {
            if (_run) _run = false;
        }

        public override void SetObserver(Observer observer)
        {
            _observer = observer;
        }
    }
}
