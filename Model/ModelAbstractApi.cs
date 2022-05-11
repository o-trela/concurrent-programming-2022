using BallSimulator.Logic;
using System.ComponentModel;

namespace BallSimulator.Presentation.Model
{
    public interface IBallModel : IObserver<IBall>, INotifyPropertyChanged
    {
        public int Diameter { get; }
        public int Radius { get; }
        public Vector2 Speed { get; }
        public Vector2 Position { get; }
    }

    public abstract class ModelAbstractApi : IObserver<IBall>, IObservable<IBallModel>
    {
        public abstract void SpawnBalls(int count);
        public abstract void Stop();

        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(IBall value);
        public abstract IDisposable Subscribe(IObserver<IBallModel> observer);

        public static ModelAbstractApi CreateModelApi(LogicAbstractApi? logic = default)
        {
            return new ModelApi(logic ?? LogicAbstractApi.CreateLogicApi());
        }
    }
}
