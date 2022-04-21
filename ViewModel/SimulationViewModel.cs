using BallSimulator.Presentation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BallSimulator.Presentation.ViewModel
{
    public class SimulationViewModel : ViewModelBase
    { 
        public ObservableCollection<BallModel> _balls;
        private readonly LogicModel _logic;
        private int _ballsCount;

        public IEnumerable<BallModel> Balls => _balls;

        public SimulationViewModel(LogicModel logic = default)
        {
            _logic = logic ?? new LogicModel();
            _balls = new ObservableCollection<BallModel>();
            _ballsCount = 10;
        }

        public int BallsCount
        {
            get => _ballsCount;
            set
            {
                _ballsCount = value;
                OnPropertyChanged(nameof(BallsCount));
            }

        }

        private void StartHandler()
        {
            _logic.SpawnBalls(BallsCount);
            _logic.Start();
            foreach (BallModel ball in _balls)
            {
                _balls.Add(ball);
            }
            OnPropertyChanged(nameof(_balls));
        }

        public CommandBase StartSimulation { get; }
    }
}
