using System;

namespace BallSimulator.Data;

public interface IBallDto : IObservable<IBallDto>
{
    int Diameter { get; init; }
    float SpeedX { get; set; }
    float SpeedY { get; set; }
    float PositionX { get; set; }
    float PositionY { get; set; }

    Task SetSpeed(float speedX, float speedY);
    Task SetPosition(float positionX, float positionY);
}
