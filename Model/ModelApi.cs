using BallSimulator.Logic;
using System.Collections.Generic;

namespace BallSimulator.Presentation.Model
{
    public abstract class ModelApi
    {
        protected Observer _observer;

        public delegate void Observer(IEnumerable<BallModel> ballModels);
        public abstract void NotifyUpdate();
        public abstract void SetObserver(Observer observer);

        public abstract void SpawnBalls(int count);
        public abstract void Start();
        public abstract void Stop();
        public abstract IEnumerable<BallModel> MapBallToBallModel();

        public static ModelApi CreateModelApi(LogicAbstractApi logic = default)
        {
            return new Model(logic ?? LogicAbstractApi.CreateLogicApi());
        }
    }
}
