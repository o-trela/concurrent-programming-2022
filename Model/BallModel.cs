using BallSimulator.Logic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive;
using System.Runtime.CompilerServices;

namespace BallSimulator.Presentation.Model
{
    public class BallModel : IBallModel, IObserver<IBall>, INotifyPropertyChanged
    {
        // observer
        private IDisposable? _unsubscriber;

        private IBall _ball;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Diameter => _ball.Diameter;
        public int Radius => _ball.Radius;
        public Vector2 Position => CalculateOffsetPosition(_ball.Position);
        public Vector2 Speed => _ball.Speed;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BallModel(IBall ball)
        {
            _ball = ball;
            Subscribe(_ball);
        }

        private Vector2 CalculateOffsetPosition(Vector2 position)
        {
            return new Vector2(position.X - Radius, position.Y - Radius);
        }

        public void Subscribe(IObservable<IBall> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }

        public void OnCompleted()
        {
            Unsubscribe();
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(IBall value)
        {
            _ball = value;
            OnPropertyChanged(nameof(Position));
        }

        public void Unsubscribe()
        {
            _unsubscriber?.Dispose();
        }
    }
}
