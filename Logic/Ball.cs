using BallSimulator.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BallSimulator.Logic;

public class Ball : IBall, IEquatable<Ball>
{
    private readonly object locker = new();

    public int Diameter { get; init; }
    public int Radius { get; init; }
    public Vector2 Speed
    {
        get
        {
            lock (locker)
            {
                return _speed;
            }
        }
        set
        {
            lock (locker)
            {
                _speed = value;
                ballDto?.SetSpeed(_speed.X, _speed.Y);
            }
        }
    }
    public Vector2 Position
    {
        get
        {
            lock (locker)
            {
                return _position;
            }
        }
        private set
        {
            lock (locker)
            {
                if (_position == value) return;
                _position = value;
                TrackBall(this);
                ballDto?.SetPosition(_position.X, _position.Y);
            }
        }
    }

    private readonly ISet<IObserver<IBall>> _observers;
    private readonly Board _board;
    private readonly Timey _ballMover;
    private readonly IBallDto ballDto; 

    private Vector2 _speed;
    private Vector2 _position;

    public Ball(int diameter, int posX, int posY, float speedX, float speedY, Board board)
        : this(diameter, new Vector2(posX, posY), new Vector2(speedX, speedY), board)
    { }

    public Ball(int diameter, Vector2 position, Vector2 speed, Board board)
    {
        Diameter = diameter;
        Position = position;
        Speed = speed;
        Radius = diameter / 2;
        _board = board;
        ballDto = new BallDto(Diameter)
        {
            SpeedX = Speed.X,
            SpeedY = Speed.Y,
            PositionX = Position.X,
            PositionY = Position.Y,
        };
        _observers = new HashSet<IObserver<IBall>>();
        _ballMover = new Timey(this.Move);
    }

    public void Start()
    {
        _ballMover.Start();
    }

    public void Stop()
    {
        _ballMover.Stop();
    }

    public void Move(float scaler)
    {
        if (Speed.IsZero()) return;

        lock (locker)
        {
            Position += Speed * scaler;
            var (posX, posY) = Position;

            var (boundryXx, boundryXy) = _board.BoundryX;
            if (!posX.Between(boundryXx, boundryXy, Radius))
            {
                Speed = new Vector2(-Speed.X, Speed.Y);
            }
            var (boundryYx, boundryYy) = _board.BoundryY;
            if (!posY.Between(boundryYx, boundryYy, Radius))
            {
                Speed = new Vector2(Speed.X, -Speed.Y);
            }
        }
    }

    public Vector2 AddSpeed(Vector2 speed)
    {
        return Speed += speed;
    }

    public bool Touches(IBall ball)
    {
        int minDistance = this.Radius + ball.Radius;
        float minDistanceSquared = minDistance * minDistance;
        float actualDistanceSquared = Vector2.DistanceSquared(this.Position, ball.Position);

        return minDistanceSquared >= actualDistanceSquared;
    }

    public IDisposable Subscribe(IObserver<IBall> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void TrackBall(IBall ball)
    {
        if (_observers == null) return;
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

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Stop();
        _ballMover.Dispose();
    }

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
        return $"Ball d={Diameter}, P=[{Position.X:n0}, {Position.Y:n0}], S=[{Speed.X:n0}, {Speed.Y:n0}]";
    }
}
