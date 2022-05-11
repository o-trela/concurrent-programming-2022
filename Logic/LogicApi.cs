using BallSimulator.Data;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;

namespace BallSimulator.Logic
{
    internal class LogicApi : LogicAbstractApi
    {
        public IList<IBall> Balls { get; private set; }

        private readonly ISet<IObserver<IBall>> _observers;
        private readonly DataAbstractApi _data;

        private const float MaxSpeed = 30;

        private readonly Board _board;
        private readonly int _ballDiameter;
        private readonly int _ballRadius;
        private readonly Random _rand;

        public event EventHandler<BallChangedEventArgs> BallChanged;
        private IObservable<EventPattern<BallChangedEventArgs>> eventObservable = null;

        private bool _running = false;

        public LogicApi(DataAbstractApi? data = default)
        {
            _data = data ?? DataAbstractApi.CreateDataApi();
            _observers = new HashSet<IObserver<IBall>>();
            _board = new Board(_data.BoardHeight, _data.BoardWidth);
            _ballDiameter = _data.BallDiameter;
            _rand = new Random();
            _ballRadius = _ballDiameter / 2;
            Balls = new List<IBall>();
            eventObservable = Observable.FromEventPattern<BallChangedEventArgs>(this, "BallChanged");
        }

/*        public override void StartSimulation()
        {
            if (!_running)
            {
                _running = true;
                Task.Run(Simulation);
            }
        }

        public override void StopSimulation()
        {
            if (_running) _running = false;
            Dispose();
        }

        public override void Simulation()
        {
            while (_running)
            {
*//*                _simulationManager.PushBalls();
                TrackBalls(Balls);
                Thread.Sleep(10);*//*
            }
        }*/

        public override List<IBall> CreateBalls(int count)
        {
            Balls = new List<IBall>(count);

            for (var i = 0; i < count; i++)
            {
                Vector2 position = GetRandomPos();
                Vector2 speed = GetRandomSpeed();
                Ball newBall = new Ball(_ballDiameter, position, speed, _board);
                Balls.Add(newBall);
                //BallChanged?.Invoke(this, new BallChangedEventArgs() { Ball = newBall });
            }

            return (List<IBall>)Balls;
        }

        private Vector2 GetRandomPos()
        {
            int x = _rand.Next(_ballRadius, _board.Width - _ballRadius);
            int y = _rand.Next(_ballRadius, _board.Height - _ballRadius);
            return new Vector2(x, y);
        }

        private Vector2 GetRandomSpeed()
        {
            const float half = MaxSpeed / 2f;
            double x = _rand.NextDouble() * MaxSpeed - half;
            double y = _rand.NextDouble() * MaxSpeed - half;
            return new Vector2((float)x, (float)y);
        }

        #region Provider

        public override IDisposable Subscribe(IObserver<IBall> observer)
        {
            return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.Ball), ex => observer.OnError(ex), () => observer.OnCompleted());
        }

/*        public override IDisposable Subscribe(IObserver<IBall> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly ISet<IObserver<IBall>> _observers;
            private readonly IObserver<IBall> _observer;

            public Unsubscriber(ISet<IObserver<IBall>> observers, IObserver<IBall> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer is not null) _observers.Remove(_observer);
            }
        }

        public void TrackBall(IBall ball)
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
                if (_observers.Contains(observer)) observer.OnCompleted();
            }

            _observers.Clear();
        }*/

        public override void Dispose()
        {
            foreach (Ball ball in Balls)
                ball.Dispose();
        }

        #endregion
    }
}
