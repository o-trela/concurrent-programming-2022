using BallSimulator.Data;
using System.Diagnostics;
using System.Linq;

namespace BallSimulator.Logic;

internal class LogicApi : LogicAbstractApi
{
    private readonly IList<IBall> _balls;
    private readonly ISet<IObserver<IBall>> _observers;
    private readonly DataAbstractApi _data;
    private readonly Board _board;
    private readonly Random _rand = new();
    private readonly Timey _coliisionHandler;

    public LogicApi(DataAbstractApi? data = default)
    {
        _data = data ?? DataAbstractApi.CreateDataApi();
        _observers = new HashSet<IObserver<IBall>>();

        _coliisionHandler = new Timey((_) => HandleCollisions());
        _board = new Board(_data.BoardHeight, _data.BoardWidth);
        _balls = new List<IBall>();
    }

    public override IEnumerable<IBall> CreateBalls(int count)
    {
        for (var i = 0; i < count; i++)
        {
            int diameter = GetRandomDiameter();
            Vector2 position = GetRandomPos(diameter);
            Vector2 speed = GetRandomSpeed();
            Ball newBall = new(diameter, position, speed, _board);
            newBall.Start();
            _balls.Add(newBall);

            TrackBall(newBall);
        }
        _coliisionHandler.Start();

        return _balls;
    }

    private Vector2 GetRandomPos(int diameter)
    {
        int radius = diameter / 2;
        int x = _rand.Next(radius, _board.Width - radius);
        int y = _rand.Next(radius, _board.Height - radius);
        return new Vector2(x, y);
    }

    private Vector2 GetRandomSpeed()
    {
        double x = (_rand.NextDouble() - 0.5) * _data.MaxSpeed;
        double y = (_rand.NextDouble() - 0.5) * _data.MaxSpeed;
        return new Vector2((float)x, (float)y);
    }

    private int GetRandomDiameter()
    {
        return _rand.Next(_data.MinDiameter, _data.MaxDiameter + 1);
    }

    #region Provider

    public override IDisposable Subscribe(IObserver<IBall> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    private void TrackBall(IBall ball)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(ball);
        }
    }

    private void EndTransmission()
    {
        foreach (var observer in _observers)
        {
            observer.OnCompleted();
        }
        _observers.Clear();
    }

    private void HandleCollisions()
    {
        var collisions = Collisions.Get(_balls);
        if (collisions.Count > 0)
        {
            foreach (var col in collisions)
            {
                var (ball1, ball2) = col;
                var (newSpeed1, newSpeed2) = Collisions.CalculateSpeeds(ball1, ball2);
                ball1.Speed = newSpeed1;
                ball2.Speed = newSpeed2;
            }
        }
        Thread.Sleep(1);
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
            _observers.Remove(_observer);
        }
    }

    #endregion

    public override void Dispose()
    {
        EndTransmission();
        _coliisionHandler.Dispose();

        foreach (var ball in _balls)
        {
            ball.Dispose();
        }
        _balls.Clear();
    }
}
