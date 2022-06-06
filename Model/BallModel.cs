﻿using BallSimulator.Logic;
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

    private readonly IBallLogic _ball;

    private IDisposable? _unsubscriber;

    public BallModel(IBallLogic ball)
    {
        _ball = ball;
        Follow(_ball);
    }

    private Vector2 CalculateOffsetPosition(Vector2 position)
    {
        return new Vector2(position.X - Radius, position.Y - Radius);
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
        OnPropertyChanged(nameof(Position));
    }

    #endregion

    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
