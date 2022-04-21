using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model
{
    public class LogicModel
    {
        private readonly LogicAbstractApi _logic;

        public LogicModel(LogicAbstractApi logic = default)
        {
            _logic = logic ?? LogicAbstractApi.CreateLogicApi();
        }

        public void SpawnBalls(int count)
        {
            _logic.CreateBalls(count);
        }

        public void Start()
        {
            _logic.StartSimulation();
        }

        public void Stop()
        {
            _logic.StopSimulation();
        }
    }
}
