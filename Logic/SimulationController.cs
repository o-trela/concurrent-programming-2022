using BallSimulator.Data;

namespace BallSimulator.Logic
{
    internal class SimulationController : LogicAbstractApi
    {
        public override IEnumerable<Ball> Balls => _simulationManager.Balls;

        private readonly ISet<IObserver<IEnumerable<Ball>>> _observers;
        private readonly DataAbstractApi _data;
        private readonly SimulationManager _simulationManager;

        private bool _running = false;

        public SimulationController(DataAbstractApi? data = default)
        {
            _data = data ?? DataAbstractApi.CreateDataApi();
            _simulationManager = new SimulationManager(new Board(_data.BoardHeight, _data.BoardWidth), _data.BallDiameter);
            _observers = new HashSet<IObserver<IEnumerable<Ball>>>();
        }

        public override void CreateBalls(int count)
        {
            _simulationManager.RandomBallCreation(count);
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
                TrackBalls(Balls);
                Thread.Sleep(10);
            }
        }

        #region Provider

        public override IDisposable Subscribe(IObserver<IEnumerable<Ball>> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly ISet<IObserver<IEnumerable<Ball>>> _observers;
            private readonly IObserver<IEnumerable<Ball>> _observer;

            public Unsubscriber(ISet<IObserver<IEnumerable<Ball>>> observers, IObserver<IEnumerable<Ball>> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer is not null) _observers.Remove(_observer);
            }
        }

        public void TrackBalls(IEnumerable<Ball> balls)
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
                if (_observers.Contains(observer)) observer.OnCompleted();
            }

            _observers.Clear();
        }

        #endregion
    }
}
