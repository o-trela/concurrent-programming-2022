using BallSimulator.Presentation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BallSimulator.Presentation.ViewModel
{
    public class SimulationViewModel : ViewModelBase
    {
        public IEnumerable<BallModel> Balls => _balls;
        
        private readonly ObservableCollection<BallModel> _balls;
        private readonly LogicModel _logic;
        private int _ballsCount;

        public SimulationViewModel(LogicModel logic)
        {
            _logic = logic;
            _balls = new ObservableCollection<BallModel>();
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

        public CommandBase StartSimulation { get; }
    }
}
