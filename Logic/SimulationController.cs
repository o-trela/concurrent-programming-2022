using BallSimulator.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BallSimulator.Logic
{
    internal class SimulationController : LogicAbstractApi, IObservable<Ball[]>
    {
        private List<IObserver<Ball[]>> observers;

        public override Ball[] Balls => _simulationManager.Balls;

        private readonly DataAbstractApi _data;
        private readonly SimulationManager _simulationManager;

        private bool _running = false;

        public SimulationController(DataAbstractApi data = default)
        {
            _data = data ?? DataAbstractApi.CreateDataApi();
            _simulationManager = new SimulationManager(new Board(_data.BoardHeight, _data.BoardWidth), _data.BallRadius);
            observers = new List<IObserver<Ball[]>>();
        }

        public override void CreateBalls(int count)
        {
            _simulationManager.RandomBallCreation(count);
        }

        public override void StartSimulation()
        {
            if (!_running)
            {
                _running = true;
                Task.Run(InvokeSimulation);
            }
        }

        public override void StopSimulation()
        {
            if (_running) _running = false;

        }

        public override void InvokeSimulation()
        {
            while (_running)
            {
                _simulationManager.PushBalls();
                TrackBalls(Balls);
                Thread.Sleep(10);
            }
        }

        #region Provider

        //   ||
        //   ||    Provider   
        //  \  /
        //   \/

        public override IDisposable Subscribe(IObserver<Ball[]> observer)
        {
            if (! observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<Ball[]>> _observers;
            private IObserver<Ball[]> _observer;

            public Unsubscriber(List<IObserver<Ball[]>> observers, IObserver<Ball[]> observer)
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

        public void TrackBalls(Ball[] balls)
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
