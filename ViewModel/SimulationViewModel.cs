using BallSimulator.Presentation.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace BallSimulator.Presentation.ViewModel
{
    public class SimulationViewModel : ViewModelBase, IObserver<IEnumerable<BallModel>>
    {
        // observer
        private IDisposable? unsubscriber;

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
                if (_ballsCountValidator.IsValid(value)) SetField(ref _ballsCount, value);
                else _ballsCount = 1;
            }
        }
        public bool IsSimulationRunning
        {
            get => _isSimulationRunning;
            private set => SetField(ref _isSimulationRunning, value);
        }
        public IEnumerable<BallModel> Balls => _balls;
        public ICommand StartSimulationCommand { get; init; }
        public ICommand StopSimulationCommand { get; init; }

        public SimulationViewModel(ModelApi? model = default, IValidator<int>? ballsCountValidator = default)
            : base()
        {
            _logic = model ?? ModelApi.CreateModelApi();
            _ballsCountValidator = ballsCountValidator ?? new BallsCountValidator();
            _balls = new ObservableCollection<BallModel>();

            StartSimulationCommand = new StartSimulationCommand(this);
            StopSimulationCommand = new StopSimulationCommand(this);
            Subscribe(_logic);
        }

        public void StartSimulation()
        {
            IsSimulationRunning = true;
            _logic.SpawnBalls(BallsCount);
            _logic.Start();
        }

        public void StopSimulation()
        {
            IsSimulationRunning = false;
            _logic.Stop();
        }

        #region Observer

        public void Subscribe(IObservable<IEnumerable<BallModel>> provider)
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

        public void OnNext(IEnumerable<BallModel> balls)
        {
            if (balls is null) balls = new List<BallModel>();
            _balls = new ObservableCollection<BallModel>(balls);
            OnPropertyChanged(nameof(Balls));
        }

        public void Unsubscribe()
        {
            unsubscriber?.Dispose();
        }

        #endregion
    }
}
