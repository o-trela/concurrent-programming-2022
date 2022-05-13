namespace BallSimulator.Data;

public interface IBallDto : IObservable<IBallDto>
{
    int Diameter { get; init; }
    float SpeedX { get; }
    float SpeedY { get; }
    float PositionX { get; }
    float PositionY { get; }

    Task SetSpeed(float speedX, float speedY);
    Task Move(float moveX, float moveY);
}
