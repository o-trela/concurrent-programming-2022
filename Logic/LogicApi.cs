using BallSimulator.Data;
using System.Threading.Tasks;

namespace BallSimulator.Logic
{
    public class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _data;
        private readonly SimulationManager _simulationManager;

        private bool _run;

        public LogicApi(DataAbstractApi data)
        {
            _data = data;

            Board board = new Board(100, 100);
            _simulationManager = new SimulationManager(board, 2);

            _run = false;
        }

        public override void CreateBalls(int count)
        {
            _simulationManager.RandomBallCreation(count);
        }

        public override void InvokeSimulation()
        {
            while (_run)
            {
                _simulationManager.PushBalls();
            }
        }

        public override void StartSimulation()
        {
            if (!_run)
            {
                _run = true;
                Task.Run(() => InvokeSimulation());
            }
        }

        public override void StopSimulation()
        {
            if (_run) _run = false;
        }
    }
}
