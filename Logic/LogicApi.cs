using BallSimulator.Data;
using BallSimulator.Data.API;
using BallSimulator.Data.Logging;
using BallSimulator.Logic.API;
using System.Diagnostics;

namespace BallSimulator.Logic;

internal class LogicApi : LogicAbstractApi
{
    private readonly Random _rand = new();
    private readonly DataAbstractApi _data;
    private readonly ILogger _logger;
    private readonly IList<IBall> _balls;
    private readonly ISet<IObserver<IBallLogic>> _observers;
    private readonly Board _board;

    public LogicApi(DataAbstractApi? data = default, ILogger? logger = default)
    {
        _data = data ?? DataAbstractApi.CreateDataApi();
        _observers = new HashSet<IObserver<IBallLogic>>();
        _logger = logger ?? new Logger();
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
            var newBall = new Ball(diameter, position, speed);
            _balls.Add(newBall);

            TrackBall(new BallLogic(newBall));
        }
        
        ThreadManager.SetValidator(HandleCollisions);
        ThreadManager.Start();

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
        double x = (_rand.NextDouble() * 2.0 - 1.0) * _data.MaxSpeed;
        double y = (_rand.NextDouble() * 2.0 - 1.0) * _data.MaxSpeed;
        return new Vector2((float)x, (float)y);
    }

    private int GetRandomDiameter()
    {
        return _rand.Next(_data.MinDiameter, _data.MaxDiameter + 1);
    }

    private void HandleCollisions()
    {
        foreach (var (ball1, ball2) in Collisions.GetBallsCollisions(_balls))
        {
            (ball1.Speed, ball2.Speed) = Collisions.CalculateSpeeds(ball1, ball2);
            _logger.LogInfo($"Balls collision detected: 1# {ball1}; 2# {ball2}");
        }
        foreach (var (ball, boundry, collisionsAxis) in Collisions.GetBoardCollisions(_balls, _board))
        {
            ball.Speed = Collisions.CalculateSpeed(ball, boundry, collisionsAxis);
            _logger.LogInfo($"Boundry collision detected: {ball}");
        }
    }

    #region Provider

    public override IDisposable Subscribe(IObserver<IBallLogic> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    private void TrackBall(IBallLogic ball)
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

    private class Unsubscriber : IDisposable
    {
        private readonly ISet<IObserver<IBallLogic>> _observers;
        private readonly IObserver<IBallLogic> _observer;

        public Unsubscriber(ISet<IObserver<IBallLogic>> observers, IObserver<IBallLogic> observer)
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
        ThreadManager.Stop();

        Trace.WriteLine($"Average Delta = {ThreadManager.AverageDeltaTime}");
        _logger.LogInfo($"Average Delta = {ThreadManager.AverageDeltaTime}");
        Trace.WriteLine($"Average Fps = {ThreadManager.AverageFps}");
        _logger.LogInfo($"Average Fps = {ThreadManager.AverageFps}");
        Trace.WriteLine($"Total Frame Count = {ThreadManager.FrameCount}");
        _logger.LogInfo($"Total Frame Count = {ThreadManager.FrameCount}");

        foreach (var ball in _balls)
        {
            ball.Dispose();
        }
        _balls.Clear();
        _logger.Dispose();
    }
}
