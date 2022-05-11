namespace BallSimulator.Logic;

public class BallChangedEventArgs : EventArgs
{
    public IBall Ball { get; set; }

    public BallChangedEventArgs(IBall ball)
    {
        Ball = ball;
    }
}
