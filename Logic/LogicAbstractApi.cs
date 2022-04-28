using BallSimulator.Data;

namespace BallSimulator.Logic
{
    public abstract class LogicAbstractApi : IObservable<IEnumerable<Ball>>
    {
        public abstract IEnumerable<Ball> Balls { get; }

        public abstract void CreateBalls(int count);
        public abstract void InvokeSimulation();
        public abstract void StartSimulation();
        public abstract void StopSimulation();

        public abstract IDisposable Subscribe(IObserver<IEnumerable<Ball>> observer);

        public static LogicAbstractApi CreateLogicApi(DataAbstractApi? data = default)
        {
            return new SimulationController(data ?? DataAbstractApi.CreateDataApi());
        }
    }
}
