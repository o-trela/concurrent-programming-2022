namespace BallSimulator.Logic;

public interface IBall : IObservable<IBall>, IDisposable
{
    int Diameter { get; }
    int Radius { get; }
    Vector2 Speed { get; set; }
    Vector2 Position { get; }

    bool Yes { get; set;  }

    void Move(float scaler);
    Vector2 AddSpeed(Vector2 speed);
    bool Touches(IBall ball);
    void LockThread();
}
