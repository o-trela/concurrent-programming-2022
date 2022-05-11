using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BallSimulator.Logic;

public class Ball : IBall, IEquatable<Ball>
{
    //public event PropertyChangedEventHandler? PropertyChanged;

    public int Diameter { get; init; }
    public int Radius { get; init; }
    public Vector2 Speed { get; private set; }
    public Vector2 Position
    {
        get => _position;
        private set
        {
            if (_position == value) return;
            _position = value;
            OnPropertyChanged();
        }
    }

    private readonly ISet<IObserver<IBall>> _observers;
    private readonly Board _board;
    private readonly Timer _moveTimer;

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

        _observers = new HashSet<IObserver<IBall>>();
        _moveTimer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        TrackBall(this);
    }

    public void Move(object? state)
    {
        if (state is not null) throw new ArgumentOutOfRangeException(nameof(state));

        if (Speed.IsZero()) return;

        Position += Speed * 0.1f;

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

    public Vector2 AddSpeed(Vector2 speed)
    {
        return Speed += speed;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _moveTimer.Dispose();
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

    /*public void EndTransmission()
    {
        foreach (var observer in _observers)
        {
            observer.OnCompleted();
        }
        _observers.Clear();
    }*/

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
}
