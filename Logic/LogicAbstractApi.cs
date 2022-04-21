using BallSimulator.Data;

namespace BallSimulator.Logic
{
    public abstract class LogicAbstractApi
    {
        protected Observer _observer;
        public abstract Ball[] Balls { get; }

        public delegate void Observer();
        public abstract void NotifyUpdate();
        public abstract void SetObserver(Observer observer);

        public abstract void CreateBalls(int count);
        public abstract void InvokeSimulation();
        public abstract void StartSimulation();
        public abstract void StopSimulation();

        public static LogicAbstractApi CreateLogicApi(DataAbstractApi data = default)
        {
            return new LogicApi(data ?? DataAbstractApi.CreateDataApi());
        }
    }
}
