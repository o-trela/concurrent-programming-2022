namespace BallSimulator.Data;

public class BallChangedEventArgs : EventArgs
{
    public IBallDto Ball { get; set; }

    public BallChangedEventArgs(IBallDto ball)
    {
        Ball = ball;
    }
}
