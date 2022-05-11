using BallSimulator.Data;
using System.ComponentModel;

namespace BallSimulator.Logic
{
    public interface IBall : INotifyPropertyChanged, IObservable<IBall>
    {
        public int Diameter { get; }
        public int Radius { get; }
        public Vector2 Speed { get; }
        public Vector2 Position { get;  }
    }

    public class BallChangedEventArgs : EventArgs
    {
        public IBall Ball { get; set; }
    }

    public abstract class LogicAbstractApi : IObservable<IBall>, IDisposable
    {
        public abstract List<IBall> CreateBalls(int count);
/*        public abstract void Simulation();
        public abstract void StartSimulation();
        public abstract void StopSimulation();*/

        public abstract IDisposable Subscribe(IObserver<IBall> observer);

        public abstract void Dispose();

        public static LogicAbstractApi CreateLogicApi(DataAbstractApi? data = default)
        {
            return new LogicApi(data ?? DataAbstractApi.CreateDataApi());
        }

    }
}
