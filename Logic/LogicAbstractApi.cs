using BallSimulator.Data;
using System.ComponentModel;

namespace BallSimulator.Logic
{
    public interface IBall : IObservable<IBall>, INotifyPropertyChanged
    {
        public int Diameter { get; }
        public int Radius { get; }
        public Vector2 Speed { get; }
        public Vector2 Position { get;  }

        void Dispose();
    }

    public class BallChangedEventArgs : EventArgs
    {
        public IBall Ball { get; set; }
    }

    public abstract class LogicAbstractApi : IObservable<IBall>, IDisposable
    {
        public abstract void CreateBalls(int count);

        public abstract IDisposable Subscribe(IObserver<IBall> observer);

        public abstract void Dispose();

        public static LogicAbstractApi CreateLogicApi(DataAbstractApi? data = default)
        {
            return new LogicApi(data ?? DataAbstractApi.CreateDataApi());
        }

    }
}
