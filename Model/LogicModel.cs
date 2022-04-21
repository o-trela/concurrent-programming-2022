using BallSimulator.Logic;
using System.Collections.Generic;

namespace BallSimulator.Presentation.Model
{
    public class LogicModel : LogicModelApi
    {
        private readonly LogicApi _logic;
        public IEnumerable<BallModel> _ballModels => MapBallToBallModel();

        public LogicModel(LogicApi logic = default)
        {
            _logic = logic ?? LogicApi.CreateLogicApi();
            _logic.SetObserver(NotifyUpdate);
        }

        public override void SpawnBalls(int count)
        {
            _logic.CreateBalls(count);
        }

        public override void Start()
        {
            _logic.StartSimulation();
        }

        public override void Stop()
        {
            _logic.StopSimulation();
        }

        public override void NotifyUpdate()
        {
            _observer.Invoke(_ballModels);
        }

        public override IEnumerable<BallModel> MapBallToBallModel()
        {
            List<BallModel> result = new List<BallModel>();
            foreach (Ball ball in _logic.GetBalls())
            {
                result.Add(new BallModel(ball));
            }
            return result;
        }
        public override void SetObserver(Observer observer)
        {
            _observer = observer;
        }
    }
}
