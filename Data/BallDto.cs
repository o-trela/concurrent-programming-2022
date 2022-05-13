namespace BallSimulator.Data;

public class BallDto : IBallDto
{
    public int Diameter { get; init; }
    public float PositionX { get; private set; }
    public float PositionY { get; private set; }
    public float SpeedX { get; private set; }
    public float SpeedY { get; private set; }

    private readonly ISet<IObserver<IBallDto>> _observers;

    public BallDto(int diameter, float positionX, float positionY, float speedX, float speedY)
    {
        Diameter = diameter;
        PositionX = positionX;
        PositionY = positionY;
        SpeedX = speedX;
        SpeedY = speedY;

        _observers = new HashSet<IObserver<IBallDto>>();
    }

    public async Task Move(float moveX, float moveY)
    {
        PositionX += moveX;
        PositionY += moveY;
        TrackBall(this);

        await Write();
    }

    public async Task SetSpeed(float speedX, float speedY)
    {
        SpeedX = speedX;
        SpeedY = speedY;
        TrackBall(this);

        await Write();
    }

    private async Task Write()
    {
        await Task.Delay(1);
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
