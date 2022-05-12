using BallSimulator.Data;
using System.Diagnostics;

namespace BallSimulator.Logic;

internal class LogicApi : LogicAbstractApi
{
    private readonly IList<IBall> _balls;
    private readonly ISet<IObserver<IBall>> _observers;
    private readonly DataAbstractApi _data;
    private readonly Board _board;
    private readonly Random _rand = new();

    public LogicApi(DataAbstractApi? data = default)
    {
        _data = data ?? DataAbstractApi.CreateDataApi();
        _observers = new HashSet<IObserver<IBall>>();

        _board = new Board(_data.BoardHeight, _data.BoardWidth);
        _balls = new List<IBall>();
    }

    public override void CreateBalls(int count)
    {
        for (var i = 0; i < count; i++)
        {
            int diameter = GetRandomDiameter();
            Vector2 position = GetRandomPos(diameter);
            Vector2 speed = GetRandomSpeed();
            Ball newBall = new(diameter, position, speed, _board);
            _balls.Add(newBall);

            TrackBall(newBall);
        }
        Task.Run(LogCollisions);
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
        int diameter = _rand.Next(_data.MinDiameter, _data.MaxDiameter + 1);
        return diameter;
    }

    #region Provider

    public override IDisposable Subscribe(IObserver<IBall> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void TrackBall(IBall ball)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(ball);
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

    private void LogCollisions()
    {
        while (true)
        {
            var collisions = Collisions.Get(_balls);
            if (collisions.Count > 0)
            {
                foreach (var col in collisions)
                {
                    var (ball1, ball2) = col;
                    /*Vector2 temp = ball1.Speed;
                    ball1.AddSpeed(ball2.Speed);
                    ball2.AddSpeed(temp);*/
                    Trace.WriteLine($"{ball1} HIT {ball2}");
                }
                Trace.Write('\n');
            }
            Thread.Sleep(50);
        }
    }

    private static class Collisions
    {
        public static IList<(IBall, IBall)> Get(IList<IBall> balls)
        {
            var collisions = new List<(IBall, IBall)>(balls.Count);
            foreach (var ball1 in balls)
            {
                foreach (var ball2 in balls)
                {
                    if (ball1 == ball2) continue;
                    if (ball1.Touches(ball2)) collisions.Add((ball1, ball2));
                }
            }
            return collisions;
        }
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

        foreach (var ball in _balls)
        {
            ball.Dispose();
        }
    }
}
