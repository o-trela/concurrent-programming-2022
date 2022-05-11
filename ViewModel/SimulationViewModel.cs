using BallSimulator.Presentation.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BallSimulator.Presentation.ViewModel
{
    public class SimulationViewModel : ViewModelBase, IObserver<IBallModel>
    {
        // observer
        private IDisposable? unsubscriber;

        private readonly ModelAbstractApi _model;
        private readonly IValidator<int> _ballsCountValidator;
        private int _ballsCount = 10;
        private bool _isSimulationRunning = false;

        public int BallsCount
        {
            get => _ballsCount;
            set
            {
                if (_ballsCountValidator.IsValid(value)) SetField(ref _ballsCount, value);
                else _ballsCount = 1;
            }
        }
        public bool IsSimulationRunning
        {
            get => _isSimulationRunning;
            private set => SetField(ref _isSimulationRunning, value);
        }
        public ObservableCollection<IBallModel> Balls { get; } = new ObservableCollection<IBallModel>();
        public ICommand StartSimulationCommand { get; init; }
        public ICommand StopSimulationCommand { get; init; }

        public SimulationViewModel(ModelAbstractApi? model = default, IValidator<int>? ballsCountValidator = default)
            : base()
        {
            _model = model ?? ModelAbstractApi.CreateModelApi();
            _ballsCountValidator = ballsCountValidator ?? new BallsCountValidator();

            StartSimulationCommand = new StartSimulationCommand(this);
            StopSimulationCommand = new StopSimulationCommand(this);
            Subscribe(_model);
            //_model.Subscribe<IBallModel>(x => Balls.Add(x));
        }

        public void StartSimulation()
        {
            IsSimulationRunning = true;
            _model.SpawnBalls(BallsCount);
            //_model.Start();
        }

        public void StopSimulation()
        {
            IsSimulationRunning = false;
            _model.Stop();
        }

        #region Observer

        public void Subscribe(IObservable<IBallModel> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public void OnCompleted()
        {
            Unsubscribe();
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(IBallModel ball)
        {
            Balls.Add(ball);
            OnPropertyChanged(nameof(Balls));
            Trace.WriteLine(Balls.Count);

        }

        public void Unsubscribe()
        {
            unsubscriber?.Dispose();
        }

        #endregion
    }
}
