using BallSimulator.Logic;
using System.Diagnostics;

namespace BallSimulator.Presentation.Model
{
    internal class ModelApi : ModelAbstractApi
    {
        private readonly LogicAbstractApi _logic;

        public ModelApi(LogicAbstractApi? logic = default)
        {
            _logic = logic ?? LogicAbstractApi.CreateLogicApi();
            _observers = new HashSet<IObserver<IBallModel>>();
            Subscribe(_logic);
        }

        public override void SpawnBalls(int count)
        {
            _logic.CreateBalls(count);
        }

        public override void Stop()
        {
            _logic.Dispose();
        }

        public static IBallModel MapBallToBallModel(IBall ball)
        {
            return new BallModel(ball);
        }

        #region Observer

        private IDisposable? _unsubscriber;

        public void Subscribe(IObservable<IBall> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }

        public override void OnCompleted()
        {
            Unsubscribe();
            EndTransmission();
        }

        public override void OnError(Exception error)
        {
            throw error;
        }

        public override void OnNext(IBall ball)
        {
            TrackBall(MapBallToBallModel(ball));
        }

        public void Unsubscribe()
        {
            _unsubscriber?.Dispose();
        }

        #endregion

        #region Provider
        
        private readonly ISet<IObserver<IBallModel>> _observers;

        public override IDisposable Subscribe(IObserver<IBallModel> observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly ISet<IObserver<IBallModel>> _observers;
            private readonly IObserver<IBallModel> _observer;

            public Unsubscriber(ISet<IObserver<IBallModel>> observers, IObserver<IBallModel> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer is not null) _observers.Remove(_observer);
            }
        }

        public void TrackBall(IBallModel ball)
        {
            foreach (var observer in _observers)
            {
                if (ball is null) observer.OnError(new NullReferenceException("Ball Object Is Null"));
                else observer.OnNext(ball);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }

            _observers.Clear();
        }

        #endregion
    }
}
