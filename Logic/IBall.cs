using System.ComponentModel;

namespace BallSimulator.Logic;

public interface IBall : IObservable<IBall>, IDisposable
{
    public int Diameter { get; }
    public int Radius { get; }
    public Vector2 Speed { get; }
    public Vector2 Position { get; }
}
