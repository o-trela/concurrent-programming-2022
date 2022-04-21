using BallSimulator.Presentation.Model;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using BallSimulator.Logic;

namespace BallSimulator.Presentation.ViewModel
{
    public class SimulationViewModel : ViewModelBase
    {
        private ObservableCollection<BallModel> _balls;
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
        public IList<BallModel> Balls => _balls;
        public ICommand StartSimulationCommand { get; }
        public ICommand StopSimulationCommand { get; }

        public SimulationViewModel(LogicModel logic = default, IValidator<int> ballsCountValidator = default)
        {
            _logic = logic ?? new LogicModel();
            _ballsCountValidator = ballsCountValidator ?? new BallsCountValidator();

            _balls = new ObservableCollection<BallModel>();
            StartSimulationCommand = new StartSimulationCommand(this);
            StopSimulationCommand = new StopSimulationCommand(this);
            _logic.SetObserver(UpdateBalls);
        }

        public void StartSimulation()
        {
            IsSimulationRunning = true;
            Trace.WriteLine("Simulation Started");
            _logic.SpawnBalls(10);
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
            //Trace.WriteLine(Balls[0].Position.ToString());
        }
    }
}
