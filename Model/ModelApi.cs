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
        Subscribe(_logic);
    }

    public override void SpawnBalls(int count)
    {
        _logic.CreateBalls(count);
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

    public void Subscribe(IObservable<IBall> provider)
    {
        _unsubscriber = provider.Subscribe(this);
    }

    public override void OnCompleted()
    {
        Unsubscribe();
        EndTransmission();
    }

    public override void OnError(Exception error)
    {
        throw error;
    }

    public override void OnNext(IBall ball)
    {
        TrackBall(MapBallToBallModel(ball));
    }

    public void Unsubscribe()
    {
        _unsubscriber?.Dispose();
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
