using System.ComponentModel;

namespace BallSimulator.Data
{
    public abstract class DataAbstractApi
    {
        public abstract int BoardHeight { get; }
        public abstract int BoardWidth { get; }
        public abstract int BallDiameter { get; }

        public interface IBall : INotifyPropertyChanged
        {
            double Top { get; }
            double Left { get; }
            double Diameter { get; }
        }

        public class BallChangedEventArgs : EventArgs
        {
            public IBall Ball { get; set; }
        }

        public static DataAbstractApi CreateDataApi()
        {
            return new DataApi();
        }
    }
}