using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using BallSimulator.Presentation.Model;

namespace BallSimulator.Presentation.ViewModel
{
    public class SimulationViewModel : ViewModelBase
    {
        private readonly ObservableCollection<BallModel> _balls;
        private LogicModel _logic = Logic;

        public IEnumerable<BallModel> Balls => _balls;

        private int _ballsCount;

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
