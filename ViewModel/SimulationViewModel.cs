using BallSimulator.Presentation.Model;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BallSimulator.Logic;
using System;
using System.Threading;

using BallSimulator.Logic;

namespace BallSimulator.Presentation.ViewModel
{
    public class SimulationViewModel : ViewModelBase
    {
        private readonly List<BallViewModel> _ballsList;
        private ObservableCollection<BallViewModel> _balls;
        
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
        public IList<BallViewModel> Balls => _balls; //change to IEnumerable
        public ICommand StartSimulationCommand { get; }
        public ICommand StopSimulationCommand { get; }

        public SimulationViewModel(LogicModel logic = default, IValidator<int> ballsCountValidator = default)
        {
            _logic = logic ?? new LogicModel();
            _ballsCountValidator = ballsCountValidator ?? new BallsCountValidator();
            
            _ballsList = new List<BallViewModel>
            {
                new BallViewModel(
                    new BallModel(
                        new Ball(10, new Vector2(100, 200), new Vector2(0.5f, 0.5f))
                        )
                    ),
                new BallViewModel(
                     new BallModel(
                         new Ball(10, new Vector2(200, 200), new Vector2(0.5f, 0.5f))
                         )
                     ),
                new BallViewModel(
                     new BallModel(
                         new Ball(10, new Vector2(300, 100), new Vector2(0.5f, 0.5f))
                         )
                     )
            };

            _balls = new ObservableCollection<BallViewModel>(_ballsList);
            StartSimulationCommand = new StartSimulationCommand(this);
            StopSimulationCommand = new StopSimulationCommand(this);
            _logic.SetObserver(UpdateBalls);

            //StartPrevSimulation();
        }

        private void StartPrevSimulation()
        {
            Thread sim = new Thread(Simulation);
            sim.Start();

            void Simulation()
            {
                while (true)
                {
                    for (int i = 0; i < _balls.Count; i++)
                    {
                        var ball = _ballsList[i];
                        ball._ball._ball.Position += (Vector2.One * 2);
                    }
                    _balls = new ObservableCollection<BallViewModel>(_ballsList);
                    OnPropertyChanged(nameof(Balls));

                    Thread.Sleep(100);
                }
            }
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
            IEnumerable<BallViewModel> ballViewModels = MapBallModelToBallViewModel(ballModels);
            _balls = new ObservableCollection<BallViewModel>(ballViewModels);
            OnPropertyChanged(nameof(Balls));
            Trace.WriteLine(Balls[0].Position.ToString());
        }

        public IEnumerable<BallViewModel> MapBallModelToBallViewModel(IEnumerable<BallModel> ballModels)
        {
            List<BallViewModel> result = new List<BallViewModel>();
            foreach (BallModel ball in ballModels)
            {
                result.Add(new BallViewModel(ball));
            }
            return result;
        }
    }
}
