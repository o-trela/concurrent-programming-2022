using BallSimulator.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BallSimulator.Logic
{
    internal class SimulationController : LogicAbstractApi
    {
        private readonly List<IObserver<IEnumerable<Ball>>> observers;

        public override IEnumerable<Ball> Balls => _simulationManager.Balls;

        private readonly DataAbstractApi _data;
        private readonly SimulationManager _simulationManager;

        private bool _running = false;

        public SimulationController(DataAbstractApi data = default)
        {
            _data = data ?? DataAbstractApi.CreateDataApi();
            _simulationManager = new SimulationManager(new Board(_data.BoardHeight, _data.BoardWidth), _data.BallDiameter);
            observers = new List<IObserver<IEnumerable<Ball>>>();
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

        public override IDisposable Subscribe(IObserver<IEnumerable<Ball>> observer)
        {
            if (! observers.Contains(observer)) observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<IEnumerable<Ball>>> _observers;
            private readonly IObserver<IEnumerable<Ball>> _observer;

            public Unsubscriber(List<IObserver<IEnumerable<Ball>>> observers, IObserver<IEnumerable<Ball>> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer is object) _observers.Remove(_observer);
            }
        }

        public void TrackBalls(IEnumerable<Ball> balls)
        {
            foreach (var observer in observers)
            {
                if (balls is null) observer.OnError(new NullReferenceException("Ball Object Is Null"));
                else observer.OnNext(balls);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in observers)
            {
                if (observers.Contains(observer)) observer.OnCompleted();
            }

            observers.Clear();
        }

        //   /\
        //  /  \   Provider  
        //   ||
        //   ||

        #endregion
    }
}
