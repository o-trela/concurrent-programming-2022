using BallSimulator.Data;

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
                _ballDto?.SetSpeed(Speed.X, Speed.Y);
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
                _ballDto?.SetPosition(Position.X, Position.Y);
                TrackBall(this);
            }
        }
    }

    private readonly ISet<IObserver<IBall>> _observers;
    private readonly Board _board;
    private readonly IBallDto _ballDto;

    private IDisposable? _disposer;
    private IDisposable? _unsubscriber;
    private Vector2 _speed;
    private Vector2 _position;

    public Ball(int diameter, int posX, int posY, float speedX, float speedY, Board board, IBallDto? ballDto = default)
        : this(diameter, new Vector2(posX, posY), new Vector2(speedX, speedY), board, ballDto)
    { }

    public Ball(int diameter, Vector2 position, Vector2 speed, Board board, IBallDto? ballDto = default)
    {
        Diameter = diameter;
        Position = position;
        Speed = speed;
        Radius = diameter / 2;
        _board = board;
        _ballDto = ballDto ?? new BallDto(Diameter)
        {
            SpeedX = Speed.X,
            SpeedY = Speed.Y,
            PositionX = Position.X,
            PositionY = Position.Y,
        };

        _observers = new HashSet<IObserver<IBall>>();
        _disposer = ThreadManager.Add<float>(Move);
        Follow(_ballDto);
    }

    public void Move(float delta)
    {
        if (Speed.IsZero()) return;

        float strength = (delta * 0.01f).Clamp(0f, 1f);

        Position += Speed * strength;
        var (posX, posY) = Position;

        var (boundryXx, boundryXy) = _board.BoundryX;
        var (newSpeedX, newSpeedY) = Speed;
        if (!posX.Between(boundryXx, boundryXy, Radius))
        {
            if (posX <= boundryXx + Radius)
            {
                newSpeedX = MathF.Abs(newSpeedX);
            }
            else
            {
                newSpeedX = -MathF.Abs(newSpeedX);
            }
            Speed = new Vector2(newSpeedX, Speed.Y);
        }
        var (boundryYx, boundryYy) = _board.BoundryY;
        if (!posY.Between(boundryYx, boundryYy, Radius))
        {
            if (posY <= boundryYx + Radius)
            {
                newSpeedY = MathF.Abs(newSpeedY);
            }
            else
            {
                newSpeedY = -MathF.Abs(newSpeedY);
            }
            Speed = new Vector2(Speed.X, newSpeedY);
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


    public void Follow(IObservable<IBallDto> provider)
    {
        _unsubscriber = provider.Subscribe(this);
    }

    public void OnCompleted()
    {
        _unsubscriber?.Dispose();
    }

    public void OnError(Exception error) => throw error;

    public void OnNext(IBallDto ballDto)
    {
        Position = new Vector2(ballDto.PositionX, ballDto.PositionY);
        Speed = new Vector2(ballDto.SpeedX, ballDto.SpeedY);
    }


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

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _disposer?.Dispose();
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
