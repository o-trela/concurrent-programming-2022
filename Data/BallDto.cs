namespace BallSimulator.Data;

public class BallDto : IBallDto
{
    public int Diameter { get; init; }
    public float SpeedX { get; set; }
    public float SpeedY { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }

    private readonly ISet<IObserver<IBallDto>> _observers;

    public BallDto(int diameter)
    {
        Diameter = diameter;

        _observers = new HashSet<IObserver<IBallDto>>();
    }

    public async Task SetPosition(float positionX, float positionY)
    {
        (PositionX, PositionY) = (positionX, positionY);
        await Write();
    }

    public async Task SetSpeed(float speedX, float speedY)
    {
        (SpeedX, SpeedY) = (speedX, speedY);
        await Write();
    }

    private async Task Write()
    {
        await Task.Delay(100);
    }
}