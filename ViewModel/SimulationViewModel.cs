using BallSimulator.Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BallSimulator.Presentation.ViewModel
{
    public class SimulationViewModel : ViewModelBase, IObserver<IEnumerable<BallModel>>
    {
        // observer
        private IDisposable unsubscriber;

        private ObservableCollection<BallModel> _balls;
        private readonly ModelApi _logic;
        private readonly IValidator<int> _ballsCountValidator;
        private int _ballsCount = 10;
        private bool _isSimulationRunning = false;

        public int BallsCount
        {
            get => _ballsCount;
            set
            {
                if (_ballsCountValidator.IsValid(value))
                {
                    _ballsCount = value;
                    OnPropertyChanged(nameof(BallsCount));
                }
                else _ballsCount = 1;
            }
        }
        public bool IsSimulationRunning
        {
            get => _isSimulationRunning;
            private set
            {
                _isSimulationRunning = value;
                OnPropertyChanged(nameof(IsSimulationRunning));
            }
        }
        public IEnumerable<BallModel> Balls => _balls;
        public ICommand StartSimulationCommand { get; }
        public ICommand StopSimulationCommand { get; }

        public SimulationViewModel(ModelApi model = default, IValidator<int> ballsCountValidator = default)
        {
            _logic = model ?? ModelApi.CreateModelApi();
            _ballsCountValidator = ballsCountValidator ?? new BallsCountValidator();

            StartSimulationCommand = new StartSimulationCommand(this);
            StopSimulationCommand = new StopSimulationCommand(this);
            Subscribe(_logic);
        }

        public void StartSimulation()
        {
            IsSimulationRunning = true;
            Trace.WriteLine("Simulation Started");
            _logic.SpawnBalls(BallsCount);
            _logic.Start();
        }

        public void StopSimulation()
        {
            IsSimulationRunning = false;
            Trace.WriteLine("Simulation Stopped");
            _logic.Stop();
        }

        #region Observer

        //   ||
        //   ||    Observer  
        //  \  /
        //   \/

        public void Subscribe(IObservable<IEnumerable<BallModel>> provider)
        {
            if (provider != null)
                this.unsubscriber = provider.Subscribe(this);
        }

        public void OnCompleted()
        {
            this.Unsubscribe();
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(IEnumerable<BallModel> balls)
        {
            if (balls is null) balls = new List<BallModel>();
            _balls = new ObservableCollection<BallModel>(balls);
            OnPropertyChanged(nameof(Balls));
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
    }
}
