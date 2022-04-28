using BallSimulator.Data;
using System;

namespace BallSimulator.Logic
{
    public abstract class LogicAbstractApi : IObservable<Ball[]>
    {
        public abstract Ball[] Balls { get; }

        public abstract void CreateBalls(int count);
        public abstract void InvokeSimulation();
        public abstract void StartSimulation();
        public abstract void StopSimulation();

        public abstract IDisposable Subscribe(IObserver<Ball[]> observer);

        public static LogicAbstractApi CreateLogicApi(DataAbstractApi data = default)
        {
            return new SimulationController(data ?? DataAbstractApi.CreateDataApi());
        }
    }
}
