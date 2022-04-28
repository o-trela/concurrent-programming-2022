using BallSimulator.Logic;
using System;
using System.Collections.Generic;

namespace BallSimulator.Presentation.Model
{
    public abstract class ModelApi : IObserver<IEnumerable<Ball>>, IObservable<IEnumerable<BallModel>>
    {
        public abstract void SpawnBalls(int count);
        public abstract void Start();
        public abstract void Stop();

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(IEnumerable<Ball> value);
        public abstract IDisposable Subscribe(IObserver<IEnumerable<BallModel>> observer);

        public static ModelApi CreateModelApi(LogicAbstractApi logic = default)
        {
            return new Model(logic ?? LogicAbstractApi.CreateLogicApi());
        }
    }
}
