using BallSimulator.Presentation.Model;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BallSimulator.Presentation.ViewModel
{
    public class SimulationViewModel : ViewModelBase
    {
        private readonly ObservableCollection<BallViewModel> _balls;
        private readonly LogicModel _logic;
        private readonly IValidator<int> _ballsCountValidator;
        private int _ballsCount = 1;
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
        public IEnumerable<BallViewModel> Balls => _balls;
        public ICommand StartSimulationCommand { get; }
        public ICommand StopSimulationCommand { get; }

        public SimulationViewModel(LogicModel logic = default, IValidator<int> ballsCountValidator = default)
        {
            _logic = logic ?? new LogicModel();
            _ballsCountValidator = ballsCountValidator ?? new BallsCountValidator();

            _balls = new ObservableCollection<BallViewModel>();
            StartSimulationCommand = new StartSimulationCommand(this);
            StopSimulationCommand = new StopSimulationCommand(this);
        }

        public void StartSimulation()
        {
            IsSimulationRunning = true;
            Trace.WriteLine("Simulation Started");
        }

        public void StopSimulation()
        {
            IsSimulationRunning = false;
            Trace.WriteLine("Simulation Stopped");
        }
    }
}
