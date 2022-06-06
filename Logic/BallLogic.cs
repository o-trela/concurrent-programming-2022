using BallSimulator.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BallSimulator.Logic;

internal class BallLogic : IBallLogic
{
    public int Diameter => _ball.Diameter;
    public int Radius => _ball.Radius;
    public Vector2 Position => new(_ball.Position.X, _ball.Position.Y);
    public Vector2 Speed => new(_ball.Speed.X, _ball.Speed.Y);

    private readonly IBall _ball;
    private readonly ISet<IObserver<IBallLogic>> _observers;

    private IDisposable? _unsubscriber;

    public BallLogic(IBall ball)
    {
        _observers = new HashSet<IObserver<IBallLogic>>();

        _ball = ball;
        Follow(_ball);
    }

    #region Observer

    public void Follow(IObservable<IBall> provider)
    {
        _unsubscriber = provider.Subscribe(this);
    }

    public void OnError(Exception error)
    {
        throw error;
    }

    public void OnCompleted()
    {
        _unsubscriber?.Dispose();
    }

    public void OnNext(IBall ball)
    {
        OnPropertyChanged(nameof(Position));
        TrackBall(this);
    }

    #endregion

    #region Provider

    public IDisposable Subscribe(IObserver<IBallLogic> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void TrackBall(IBallLogic ball)
    {
        if (_observers is null) return;
        foreach (var observer in _observers)
        {
            observer.OnNext(ball);
        }
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

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}

