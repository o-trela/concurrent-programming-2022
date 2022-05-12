using BallSimulator.Data;

namespace BallSimulator.Logic;

public abstract class LogicAbstractApi : IObservable<IBall>, IDisposable
{
    public static LogicAbstractApi CreateLogicApi(DataAbstractApi? data = default)
    {
        return new LogicApi(data ?? DataAbstractApi.CreateDataApi());
    }
    public abstract void CreateBalls(int count);

    public abstract IDisposable Subscribe(IObserver<IBall> observer);

    public abstract void Dispose();
}
