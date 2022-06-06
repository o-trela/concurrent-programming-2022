using BallSimulator.Data;
using BallSimulator.Data.API;
using BallSimulator.Logic.API;
using System.Diagnostics;

namespace BallSimulator.Logic;

internal class LogicApi : LogicAbstractApi
{
    private readonly IList<IBall> _balls;
    private readonly ISet<IObserver<IBallLogic>> _observers;
    private readonly IDictionary<IBall, IBallLogic> _ballToBallLogic;
    private readonly DataAbstractApi _data;
    private readonly Board _board;
    private readonly Random _rand = new();

    public LogicApi(DataAbstractApi? data = default)
    {
        _data = data ?? DataAbstractApi.CreateDataApi();
        _observers = new HashSet<IObserver<IBallLogic>>();
        _ballToBallLogic = new Dictionary<IBall, IBallLogic>();

        _board = new Board(_data.BoardHeight, _data.BoardWidth);
        _balls = new List<IBall>();
    }

    public override IEnumerable<IBall> CreateBalls(int count)
    {
        for (var i = 0; i < count; i++)
        {
            int diameter = GetRandomDiameter();
            Data.Vector2 position = GetRandomPos(diameter);
            Data.Vector2 speed = GetRandomSpeed();
            var newBall = new Ball(diameter, position, speed, _board);
            _balls.Add(newBall);

            TrackBall(new BallLogic(newBall));
        }
        ThreadManager.SetValidator(HandleCollisions);
        ThreadManager.Start();

        return _balls;
    }

    private Data.Vector2 GetRandomPos(int diameter)
    {
        int radius = diameter / 2;
        int x = _rand.Next(radius, _board.Width - radius);
        int y = _rand.Next(radius, _board.Height - radius);
        return new Data.Vector2(x, y);
    }

    private Data.Vector2 GetRandomSpeed()
    {
        double x = (_rand.NextDouble() * 2.0 - 1.0) * _data.MaxSpeed;
        double y = (_rand.NextDouble() * 2.0 - 1.0) * _data.MaxSpeed;
        return new Data.Vector2((float)x, (float)y);
    }

    private int GetRandomDiameter()
    {
        return _rand.Next(_data.MinDiameter, _data.MaxDiameter + 1);
    }

    private void HandleCollisions()
    {
        foreach (var col in Collisions.Get(_balls))
        {
            var (ball1, ball2) = col;
            (ball1.Speed, ball2.Speed) = Collisions.CalculateSpeeds(ball1, ball2);
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
        Trace.WriteLine($"Average Fps = {ThreadManager.AverageFps}");
        Trace.WriteLine($"Total Frame Count = {ThreadManager.FrameCount}");

        foreach (var ball in _balls)
        {
            ball.Dispose();
        }
        _balls.Clear();
    }
}
