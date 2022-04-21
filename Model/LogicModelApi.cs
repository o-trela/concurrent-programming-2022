using System;
using System.Collections.Generic;
using System.Text;

using BallSimulator.Logic;
using BallSimulator.Presentation.Model;

namespace BallSimulator.Presentation.Model
{
    public abstract class LogicModelApi
    {
        protected Observer _observer;


        public delegate void Observer(IEnumerable<BallModel> ballModels);
        public abstract void NotifyUpdate();
        public abstract void SetObserver(Observer modelObserver);

        public abstract void SpawnBalls(int count);
        public abstract void Start();
        public abstract void Stop();
        public abstract IEnumerable<BallModel> MapBallToBallModel();

        public static LogicModel CreateLogicModelApi(LogicAbstractApi logic = default)
        {
            return new LogicModel(logic ?? LogicAbstractApi.CreateLogicApi());
        }
    }
}
