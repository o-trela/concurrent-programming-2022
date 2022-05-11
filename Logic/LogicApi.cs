using BallSimulator.Data;

namespace BallSimulator.Logic;

internal class LogicApi : LogicAbstractApi
{
    private const float MaxSpeed = 30;

    public IList<IBall> DisposableBalls { get; private set; }

    private readonly ISet<IObserver<IBall>> _observers;
    private readonly DataAbstractApi _data;
    private readonly Board _board;
    private readonly int _ballDiameter;
    private readonly int _ballRadius;
    private readonly Random _rand = new();

    public LogicApi(DataAbstractApi? data = default)
    {
        _data = data ?? DataAbstractApi.CreateDataApi();
        _observers = new HashSet<IObserver<IBall>>();

        _board = new Board(_data.BoardHeight, _data.BoardWidth);
        _ballDiameter = _data.BallDiameter;
        _ballRadius = _ballDiameter / 2;

        DisposableBalls = new List<IBall>();
    }

    public override void CreateBalls(int count)
    {
        DisposableBalls = new List<IBall>(count);

        for (var i = 0; i < count; i++)
        {
            Vector2 position = GetRandomPos();
            Vector2 speed = GetRandomSpeed();
            Ball newBall = new Ball(_ballDiameter, position, speed, _board);
            DisposableBalls.Add(newBall);

            TrackBall(newBall);
        }
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
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void TrackBall(IBall ball)
    {
        //if (ball is null) observer.OnError(new NullReferenceException("Ball Object Is Null"));
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

        foreach (var ball in DisposableBalls) ball.Dispose();
    }
}
