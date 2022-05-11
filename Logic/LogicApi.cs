using BallSimulator.Data;

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
        float half = _data.MaxSpeed / 2f;
        double x = _rand.NextDouble() * _data.MaxSpeed - half;
        double y = _rand.NextDouble() * _data.MaxSpeed - half;
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
