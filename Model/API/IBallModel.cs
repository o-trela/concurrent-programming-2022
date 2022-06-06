using BallSimulator.Logic;
using System.ComponentModel;

namespace BallSimulator.Presentation.Model.API;

public interface IBallModel : IObserver<IBallLogic>, INotifyPropertyChanged
{
    public int Diameter { get; }
    public int Radius { get; }
    public float SpeedX { get; }
    public float SpeedY { get; }
    public float PositionX { get; }
    public float PositionY { get; }
}
