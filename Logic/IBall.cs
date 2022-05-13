using BallSimulator.Data;

namespace BallSimulator.Logic;

public interface IBall : IObservable<IBall>, IObserver<IBallDto>, IDisposable
{
    int Diameter { get; }
    int Radius { get; }
    Vector2 Position { get; }
    Vector2 Speed { get; set; }

    void Move(float scaler);
    bool Touches(IBall ball);
}
