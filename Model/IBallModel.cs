using BallSimulator.Logic;
using System.ComponentModel;

namespace BallSimulator.Presentation.Model;

public interface IBallModel : IObserver<IBallLogic>, INotifyPropertyChanged
{
    public int Diameter { get; }
    public int Radius { get; }
    public Vector2 Speed { get; }
    public Vector2 Position { get; }
}
