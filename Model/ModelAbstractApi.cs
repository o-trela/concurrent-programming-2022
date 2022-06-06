using BallSimulator.Logic;

namespace BallSimulator.Presentation.Model;

public abstract class ModelAbstractApi : IObserver<IBallLogic>, IObservable<IBallModel>
{
    public static ModelAbstractApi CreateModelApi(LogicAbstractApi? logic = default)
    {
        return new ModelApi(logic ?? LogicAbstractApi.CreateLogicApi());
    }

    public abstract void Start(int count);
    public abstract void Stop();

    public abstract IDisposable Subscribe(IObserver<IBallModel> observer);

    public abstract void OnCompleted();
    public virtual void OnError(Exception error) => throw error;
    public abstract void OnNext(IBallLogic value);
}
