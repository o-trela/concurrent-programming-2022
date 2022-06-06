using BallSimulator.Data;
using System.ComponentModel;

namespace BallSimulator.Logic;

public interface IBallLogic: IObservable<IBallLogic>, IObserver<IBall>, INotifyPropertyChanged
{
    public int Diameter { get; }
    public int Radius { get; }
    public Vector2 Speed { get; }
    public Vector2 Position { get; }
}
