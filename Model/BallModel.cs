using BallSimulator.Logic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BallSimulator.Presentation.Model;

public class BallModel : IBallModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public int Diameter => _ball.Diameter;
    public int Radius => _ball.Radius;
    public Vector2 Position => CalculateOffsetPosition(_ball.Position);
    public Vector2 Speed => _ball.Speed;

    private IBall _ball;
    private IDisposable? _unsubscriber;

    public BallModel(IBall ball)
    {
        _ball = ball;
        Subscribe(_ball);
    }

    private Vector2 CalculateOffsetPosition(Vector2 position)
    {
        return new Vector2(position.X - Radius, position.Y - Radius);
    }

    #region INotifyPropertyChanged

    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region Observer

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

    public void OnNext(IBall ball)
    {
        _ball = ball;
        OnPropertyChanged(nameof(Position));
    }

    public void Unsubscribe()
    {
        _unsubscriber?.Dispose();
    }

    #endregion
}
