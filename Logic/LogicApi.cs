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
        return _rand.Next(_data.MinDiameter, _data.MaxDiameter + 1);
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
                    //ball1.AddSpeed(ball1.Speed);
                    var (newSpeed1, newSpeed2) = Collisions.CalculateSpeeds(ball1, ball2);
                    ball1.Speed = newSpeed1;
                    ball2.Speed = newSpeed2;
                    //Trace.WriteLine($"{ball1} HIT {ball2}");
                }
                //Trace.Write('\n');
            }
            Thread.Sleep(1);
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
                    if (ball1.Touches(ball2) && !collisions.Contains((ball2, ball1))) 
                        collisions.Add((ball1, ball2));
                }
            }
            return collisions;
        }

        public static (Vector2, Vector2) CalculateSpeeds(IBall ball1, IBall ball2)
        {
            if (MathF.Sqrt(ball1.Speed.X * ball1.Speed.X + ball1.Speed.Y * ball1.Speed.Y) * MathF.Sqrt(ball2.Speed.X * ball2.Speed.X + ball2.Speed.Y * ball2.Speed.Y) < 0) 
                return (ball1.Speed, ball2.Speed);
            
            Vector2 normal = new Vector2(
                ball2.Position.X - ball1.Position.X,
                ball2.Position.Y - ball1.Position.Y);

            Vector2 unitNormal = normal/MathF.Sqrt(normal.X*normal.X + normal.Y*normal.Y);
        
            Vector2 unitTangent = new Vector2(-unitNormal.Y, unitNormal.X);

            Vector2 velocity1Normal = unitNormal * ball1.Speed;
            Vector2 velocity2Normal = unitNormal * ball2.Speed;

            Vector2 velocity1Tangent = unitTangent * ball1.Speed;
            Vector2 velocity2Tangent = unitTangent * ball2.Speed;

            Vector2 acVelocity1Tangent = velocity1Tangent;
            Vector2 acVelocity2Tangent = velocity2Tangent;

            Vector2 acVelocity1Normal = (velocity1Normal * (ball1.Radius - ball2.Radius) + velocity2Normal * 2 * ball2.Radius) / (ball1.Radius + ball2.Radius);
            Vector2 acVelocity2Normal = (velocity2Normal * (ball2.Radius - ball1.Radius) + velocity1Normal * 2 * ball1.Radius) / (ball1.Radius + ball2.Radius);

            Vector2 newVelocity1 = acVelocity1Normal * unitNormal + acVelocity1Tangent * unitTangent;
            Vector2 newVelocity2 = acVelocity2Normal * unitNormal + acVelocity2Tangent * unitTangent;

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

        foreach (var ball in _balls)
        {
            ball.Dispose();
        }
        _balls.Clear();
    }
}
