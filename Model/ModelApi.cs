using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model;

internal class ModelApi : ModelAbstractApi
{
    private readonly LogicAbstractApi _logic;
    private readonly ISet<IObserver<IBallModel>> _observers;

    private IDisposable? _unsubscriber;

    public ModelApi(LogicAbstractApi? logic = default)
    {
        _logic = logic ?? LogicAbstractApi.CreateLogicApi();
        _observers = new HashSet<IObserver<IBallModel>>();
        Follow(_logic);
    }

    public override void Start(int ballsCount)
    {
        _logic.CreateBalls(ballsCount);
    }

    public override void Stop()
    {
        _logic.Dispose();
    }

    private static IBallModel MapBallToBallModel(IBall ball)
    {
        return new BallModel(ball);
    }

    #region Observer

    public void Follow(IObservable<IBall> provider)
    {
        _unsubscriber = provider.Subscribe(this);
    }

    public override void OnCompleted()
    {
        _unsubscriber?.Dispose();
        EndTransmission();
    }

    public override void OnNext(IBall ball)
    {
        TrackBall(MapBallToBallModel(ball));
    }

    #endregion

    #region Provider

    public override IDisposable Subscribe(IObserver<IBallModel> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void TrackBall(IBallModel ball)
    {
        //if (ball is null) observer.OnError(new NullReferenceException("Ball Object Is Null"));
        foreach (var observer in _observers)
        {
            observer.OnNext(ball);
        }
    }

    public void EndTransmission()
    {
        foreach (var observer in _observers)
        {
            observer.OnCompleted();
        }
        _observers.Clear();
    }

    private class Unsubscriber : IDisposable
    {
        private readonly ISet<IObserver<IBallModel>> _observers;
        private readonly IObserver<IBallModel> _observer;

        public Unsubscriber(ISet<IObserver<IBallModel>> observers, IObserver<IBallModel> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            _observers.Remove(_observer);
        }
    }

    #endregion
}
