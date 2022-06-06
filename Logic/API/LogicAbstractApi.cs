using BallSimulator.Data.API;

namespace BallSimulator.Logic.API;

public abstract class LogicAbstractApi : IObservable<IBallLogic>, IDisposable
{
    public static LogicAbstractApi CreateLogicApi(DataAbstractApi? data = default)
    {
        return new LogicApi(data ?? DataAbstractApi.CreateDataApi());
    }
    public abstract IEnumerable<IBall> CreateBalls(int count);

    public abstract IDisposable Subscribe(IObserver<IBallLogic> observer);

    public abstract void Dispose();
}
