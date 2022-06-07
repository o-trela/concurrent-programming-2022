using BallSimulator.Logic.API;
using BallSimulator.Presentation.Model.API;

namespace BallSimulator.Presentation.Model;

internal class ModelApi : ModelAbstractApi
{
    private readonly LogicAbstractApi _logic;
    private readonly ISet<IObserver<IBallModel>> _observers;
    private readonly IDictionary<IBallLogic, IBallModel> _ballToBallModel;

    private IDisposable? _unsubscriber;

    public ModelApi(LogicAbstractApi? logic = default)
    {
        _logic = logic ?? LogicAbstractApi.CreateLogicApi();
        _observers = new HashSet<IObserver<IBallModel>>();
        _ballToBallModel = new Dictionary<IBallLogic, IBallModel>();
    }

    public override void Start(int ballsCount)
    {
        Follow(_logic);
        _logic.CreateBalls(ballsCount);
    }

    #region Observer

    public void Follow(IObservable<IBallLogic> provider)
    {
        _unsubscriber = provider.Subscribe(this);
    }

    public override void OnCompleted()
    {
        _unsubscriber?.Dispose();
        EndTransmission();
    }

    public override void OnNext(IBallLogic ball)
    {
        _ballToBallModel.TryGetValue(ball, out var ballModel);
        if (ballModel is null)
        {
            ballModel = new BallModel(ball);
            _ballToBallModel.Add(ball, ballModel);
        }
        TrackBall(ballModel);
    }

    #endregion

    #region Provider

    public override IDisposable Subscribe(IObserver<IBallModel> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    private void TrackBall(IBallModel ball)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(ball);
        }
    }

    private void EndTransmission()
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

    public override void Dispose()
    {
        _logic.Dispose();
        _unsubscriber?.Dispose();
    }
}
