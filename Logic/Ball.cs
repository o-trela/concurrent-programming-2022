using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BallSimulator.Logic
{
    public class Ball : IBall, IEquatable<Ball>, IDisposable
    {
        private readonly ISet<IObserver<IBall>> _observers;

        public int Diameter { get; init; }
        public int Radius { get; init; }
        public Vector2 Speed { get; private set; }
        public Vector2 Position { get { return _position; }
            private set
            {
                if (_position == value)
                    return;
                _position = value;
                OnPropertyChanged();
            }
        }

        private Board _board;
        private Timer MoveTimer;
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
            MoveTimer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            TrackBall(this);
        }

        public void Move(object state)
        {
            if (state != null)
                throw new ArgumentOutOfRangeException(nameof(state));

            if (Speed.IsZero()) return;

            Position += Speed * 0.1f;

            var (posX, posY) = Position;
            if (!posX.Between(_board.XBoundry.X, _board.XBoundry.Y, Radius))
            {
                Speed = new Vector2(-Speed.X, Speed.Y);
            }
            if (!posY.Between(_board.YBoundry.X, _board.YBoundry.Y, Radius))
            {
                Speed = new Vector2(Speed.X, -Speed.Y);
            }
        }

        public Vector2 AddSpeed(Vector2 speed)
        {
            Speed += speed;
            return Speed;
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

        public void Dispose()
        {
            MoveTimer.Dispose();
        }

        public IDisposable Subscribe(IObserver<IBall> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
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
                if (_observer is not null) _observers.Remove(_observer);
            }
        }

        public void TrackBall(IBall ball)
        {
            if (_observers == null) return;
            foreach (var observer in _observers)
            {
                if (ball is null) observer.OnError(new NullReferenceException("Ball Object Is Null"));
                else observer.OnNext(ball);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in _observers)
            {
                if (_observers.Contains(observer)) observer.OnCompleted();
            }

            _observers.Clear();
        }
    }
}