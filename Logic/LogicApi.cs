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

    private bool _running = false;

    public LogicApi(DataAbstractApi? data = default)
    {
        _data = data ?? DataAbstractApi.CreateDataApi();
        _observers = new HashSet<IObserver<IBall>>();

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
        _running = true;
        Task.Run(LogCollisions);

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

    private void LogCollisions()
    {
        while (_running)
        {
            var collisions = Collisions.Get(_balls);
            if (collisions.Count > 0)
            {
                foreach (var col in collisions)
                {
                    var (ball1, ball2) = col;
                    /*ball1.Yes = true;
                    ball2.Yes = true;
                    ball1.LockThread();
                    ball2.LockThread();*/
                    var (newSpeed1, newSpeed2) = Collisions.CalculateSpeeds(ball1, ball2);
                    ball1.Speed = newSpeed1;
                    ball2.Speed = newSpeed2;
                    /*ball1.Yes = false;
                    ball2.Yes = false;*/
                    //Trace.WriteLine($"{ball1} HIT {ball2}");
                }
                //Trace.Write('\n');
            }
            Thread.Sleep(10);
        }
    }

    private static class Collisions
    {
        public static ISet<(IBall, IBall)> Get(IList<IBall> balls)
        {
            var collisions = new HashSet<(IBall, IBall)>(balls.Count);
            foreach (var ball1 in balls)
            {
                foreach (var ball2 in balls)
                {
                    if (ball1 == ball2) continue;
                    if (ball1.Touches(ball2) && !collisions.Contains((ball2, ball1))) collisions.Add((ball1, ball2));
                }
            }
            return collisions;
        }

        public static (Vector2, Vector2) CalculateSpeeds(IBall ball1, IBall ball2)
        {
            float ballsDistance = Vector2.Distance(ball1.Position, ball2.Position);

            Vector2 normal = new Vector2((ball2.Position.X - ball1.Position.X) / ballsDistance, (ball2.Position.Y - ball1.Position.Y) / ballsDistance);
            Vector2 tangent = new Vector2(-normal.Y, normal.X);

            if (Vector2.Scalar(ball1.Speed, normal) < 0) return (ball1.Speed, ball2.Speed);

            float dpTan1 = ball1.Speed.X * tangent.X + ball1.Speed.Y * tangent.Y;
            float dpTan2 = ball2.Speed.X * tangent.X + ball2.Speed.Y * tangent.Y;

            float dpNorm1 = ball1.Speed.X * normal.X + ball1.Speed.Y * normal.Y;
            float dpNorm2 = ball2.Speed.X * normal.X + ball2.Speed.Y * normal.Y;

            float momentum1 = (dpNorm1 * (ball1.Radius - ball2.Radius) + 2.0f * ball2.Radius * dpNorm2) / (ball1.Radius + ball2.Radius);
            float momentum2 = (dpNorm2 * (ball2.Radius - ball1.Radius) + 2.0f * ball1.Radius * dpNorm1) / (ball1.Radius + ball2.Radius);

            Vector2 newVelocity1 = new Vector2(tangent.X * dpTan1 + normal.X * momentum1, tangent.Y * dpTan1 + normal.Y * momentum1);
            Vector2 newVelocity2 = new Vector2(tangent.X * dpTan2 + normal.X * momentum2, tangent.Y * dpTan2 + normal.Y * momentum2);

            return (newVelocity1, newVelocity2);
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
        _running = false;

        foreach (var ball in _balls)
        {
            ball.Dispose();
        }
        _balls.Clear();
    }
}
