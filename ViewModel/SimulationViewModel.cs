using BallSimulator.Presentation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BallSimulator.Presentation.ViewModel
{
    public class SimulationViewModel : ViewModelBase
    {
        private ObservableCollection<BallModel> _balls;
        private readonly LogicModel _logic;
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
            set
            {
                _isSimulationRunning = value;
                OnPropertyChanged(nameof(IsSimulationRunning));
            }
        }
        public IEnumerable<BallModel> Balls => _balls; //change to IEnumerable
        public ICommand StartSimulationCommand { get; }
        public ICommand StopSimulationCommand { get; }

        public SimulationViewModel(LogicModel logic = default, IValidator<int> ballsCountValidator = default)
        {
            _logic = logic ?? new LogicModel();
            _ballsCountValidator = ballsCountValidator ?? new BallsCountValidator();

            StartSimulationCommand = new StartSimulationCommand(this);
            StopSimulationCommand = new StopSimulationCommand(this);
            _logic.SetObserver(UpdateBalls);
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

        public void UpdateBalls(IEnumerable<BallModel> ballModels)
        {
            _balls = new ObservableCollection<BallModel>(ballModels);
            OnPropertyChanged(nameof(Balls));
        }
    }
}
