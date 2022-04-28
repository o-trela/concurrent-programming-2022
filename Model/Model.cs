using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model
{
    internal class Model : ModelApi
    {
        // provider
        private readonly ISet<IObserver<IEnumerable<BallModel>>> _observers;
        // observer
        private IDisposable? _unsubscriber;

        private readonly LogicAbstractApi _logic;

        public Model(LogicAbstractApi? logic = default)
        {
            _logic = logic ?? LogicAbstractApi.CreateLogicApi();
            _observers = new HashSet<IObserver<IEnumerable<BallModel>>>();
            Subscribe(_logic);
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

        public static IEnumerable<BallModel> MapBallToBallModel(IEnumerable<Ball> balls)
        {
            return balls.Select(ball => new BallModel(ball));
        }

        #region Observer

        public void Subscribe(IObservable<IEnumerable<Ball>> provider)
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

        public override void OnNext(IEnumerable<Ball> balls)
        {
            TrackBalls(MapBallToBallModel(balls));
        }

        public void Unsubscribe()
        {
            _unsubscriber?.Dispose();
        }

        #endregion

        #region Provider

        public override IDisposable Subscribe(IObserver<IEnumerable<BallModel>> observer)
        {
            if (!_observers.Contains(observer)) _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly ISet<IObserver<IEnumerable<BallModel>>> _observers;
            private readonly IObserver<IEnumerable<BallModel>> _observer;

            public Unsubscriber(ISet<IObserver<IEnumerable<BallModel>>> observers, IObserver<IEnumerable<BallModel>> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer is not null) _observers.Remove(_observer);
            }
        }

        public void TrackBalls(IEnumerable<BallModel> balls)
        {
            foreach (var observer in _observers)
            {
                if (balls is null) observer.OnError(new NullReferenceException("Ball Object Is Null"));
                else observer.OnNext(balls);
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
