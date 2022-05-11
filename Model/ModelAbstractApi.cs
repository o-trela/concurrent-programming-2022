using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model;

public abstract class ModelAbstractApi : IObserver<IBall>, IObservable<IBallModel>
{
    public static ModelAbstractApi CreateModelApi(LogicAbstractApi? logic = default)
    {
        return new ModelApi(logic ?? LogicAbstractApi.CreateLogicApi());
    }

    public abstract void SpawnBalls(int count);
    public abstract void Stop();

    public abstract void OnCompleted();
    public abstract void OnError(Exception error);
    public abstract void OnNext(IBall value);
    public abstract IDisposable Subscribe(IObserver<IBallModel> observer);
}
