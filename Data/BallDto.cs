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

    public IDisposable Subscribe(IObserver<IBallDto> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    private void TrackBall(IBallDto ball)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(ball);
        }
    }

    private class Unsubscriber : IDisposable
    {
        private readonly ISet<IObserver<IBallDto>> _observers;
        private readonly IObserver<IBallDto> _observer;

        public Unsubscriber(ISet<IObserver<IBallDto>> observers, IObserver<IBallDto> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            _observers.Remove(_observer);
        }
    }
}