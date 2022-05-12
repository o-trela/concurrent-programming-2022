namespace BallSimulator.Data;

public class BallDto : IBallDto
{
    public int Diameter { get; init; }
    public float SpeedX { get; set; }
    public float SpeedY { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }

    public BallDto(int diameter)
    {
        Diameter = diameter;
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

    public IDisposable Subscribe(IObserver<IBallDto> observer)
    {
        throw new NotImplementedException();
    }
}