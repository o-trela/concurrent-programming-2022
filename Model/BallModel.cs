using BallSimulator.Logic;
using BallSimulator.Presentation.Model.API;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BallSimulator.Presentation.Model;

public class BallModel : IBallModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public int Diameter => _ball.Diameter;
    public int Radius => _ball.Radius;
    public float PositionX => _ball.Position.X - Radius;
    public float PositionY => _ball.Position.Y - Radius;
    public float SpeedX => _ball.Speed.X;
    public float SpeedY => _ball.Speed.Y;

    private readonly IBallLogic _ball;

    private IDisposable? _unsubscriber;

    public BallModel(IBallLogic ball)
    {
        _ball = ball;
        Follow(_ball);
    }

    #region Observer

    public void Follow(IObservable<IBallLogic> provider)
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

    public void OnNext(IBallLogic ball)
    {
        OnPropertyChanged(nameof(PositionX));
        OnPropertyChanged(nameof(PositionY));
    }

    #endregion

    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
