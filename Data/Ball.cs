using BallSimulator.Data.API;

namespace BallSimulator.Data;

public class Ball : IBall, IEquatable<Ball>
{
    private readonly object positionLock = new();
    private readonly object speedLock = new();

    public int Diameter { get; init; }
    public int Radius { get; init; }
    public Vector2 Speed
    {
        get
        {
            lock (speedLock)
            {
                return _speed;
            }
        }
        set
        {
            lock (speedLock)
            {
                _speed = value;
            }
        }
    }
    public Vector2 Position
    {
        get
        {
            lock (positionLock)
            {
                return _position;
            }
        }
        private set
        {
            lock (positionLock)
            {
                if (_position == value) return;
                _position = value;
                TrackBall(this);
            }
        }
    }

    private readonly ISet<IObserver<IBall>> _observers;
    private readonly IDisposable? _disposer;

    private Vector2 _speed;
    private Vector2 _position;

    public Ball(int diameter, Vector2 position, Vector2 speed)
    {
        Diameter = diameter;
        Position = position;
        Speed = speed;
        Radius = diameter / 2;

        _observers = new HashSet<IObserver<IBall>>();
        _disposer = ThreadManager.Add<float>(Move);
    }

    public void Move(float delta)
    {
        if (Speed.IsZero()) return;

        float strength = delta.Clamp(0f, 100f) * 0.01f;
        Position += Speed * strength;
    }

    public bool Touches(IBall ball)
    {
        int minDistance = this.Radius + ball.Radius;
        float minDistanceSquared = minDistance * minDistance;
        float actualDistanceSquared = Vector2.DistanceSquared(this.Position, ball.Position);

        return minDistanceSquared >= actualDistanceSquared;
    }

    #region Provider

    public IDisposable Subscribe(IObserver<IBall> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void TrackBall(IBall ball)
    {
        if (_observers is null) return;
        foreach (var observer in _observers)
        {
            observer.OnNext(ball);
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

    public override bool Equals(object? obj)
    {
        return obj is Ball ball
            && Equals(ball);
    }

    public bool Equals(Ball? other)
    {
        return other is not null
            && Diameter == other.Diameter
            && Position == other.Position
            && Speed == other.Speed;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Diameter, Position, Speed);
    }

    public override string? ToString()
    {
        return $"Ball d={Diameter}, P=[{Position.X:n1}, {Position.Y:n1}], S=[{Speed.X:n1}, {Speed.Y:n1}]";
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _disposer?.Dispose();
    }
}
