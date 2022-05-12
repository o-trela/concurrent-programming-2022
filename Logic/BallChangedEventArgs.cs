namespace BallSimulator.Logic;

public class BallChangedEventArgs : EventArgs
{
    public IBall Ball { get; init; }

    public BallChangedEventArgs(IBall ball)
    {
        Ball = ball;
    }
}
