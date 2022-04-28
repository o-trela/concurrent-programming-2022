using BallSimulator.Logic;
using System;
using System.Collections.Generic;

namespace BallSimulator.Presentation.Model
{
    internal class Model : ModelApi, IObserver<Ball[]>, IObservable<IEnumerable<BallModel>>
    {
        // provider
        private List<IObserver<IEnumerable<BallModel>>> observers;
        // observer
        private IDisposable unsubscriber;

        private readonly LogicAbstractApi _logic;

        public Model(LogicAbstractApi logic = default)
        {
            _logic = logic ?? LogicAbstractApi.CreateLogicApi();
            observers = new List<IObserver<IEnumerable<BallModel>>>();
            Subscribe(_logic);
        }

        public override void SpawnBalls(int count)
        {
            _logic.CreateBalls(count);
        }

        public override void Start()
        {
            _logic.StartSimulation();
        }

        public override void Stop()
        {
            _logic.StopSimulation();
        }

        public IEnumerable<BallModel> MapBallToBallModel(Ball[] balls)
        {
            List<BallModel> result = new List<BallModel>();
            foreach (Ball ball in balls)
            {
                result.Add(new BallModel(ball));
            }
            return result;
        }

        #region Observer

        //   ||
        //   ||    Observer  
        //  \  /
        //   \/

        public void Subscribe(IObservable<Ball[]> provider)
        {
            if (provider != null)
                this.unsubscriber = provider.Subscribe(this);
        }

        public override void OnCompleted()
        {
            this.Unsubscribe();
            EndTransmission();
        }

        public override void OnError(Exception error)
        {
            throw error;
        }

        public override void OnNext(Ball[] balls)
        {
            TrackBalls(MapBallToBallModel(balls));
        }

        public void Unsubscribe()
        {
            this.unsubscriber.Dispose();
        }


        //   /\
        //  /  \   Observer  
        //   ||
        //   ||

        #endregion

        #region Provider

        //   ||
        //   ||    Provider   
        //  \  /
        //   \/

        public override IDisposable Subscribe(IObserver<IEnumerable<BallModel>> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<IEnumerable<BallModel>>> _observers;
            private IObserver<IEnumerable<BallModel>> _observer;

            public Unsubscriber(List<IObserver<IEnumerable<BallModel>>> observers, IObserver<IEnumerable<BallModel>> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public void TrackBalls(IEnumerable<BallModel> balls)
        {
            foreach (var observer in observers)
                if (balls == null)
                    observer.OnError(new NullReferenceException("Ball Object Is Null"));
                else
                    observer.OnNext(balls);
        }

        public void EndTransmission()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }

        //   /\
        //  /  \   Provider  
        //   ||
        //   ||

        #endregion
    }
}
