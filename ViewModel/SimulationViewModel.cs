using BallSimulator.Presentation.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BallSimulator.Presentation.ViewModel;

public class SimulationViewModel : ViewModelBase, IObserver<IBallModel>
{
    private readonly ModelAbstractApi _model;
    private readonly IValidator<int> _ballsCountValidator;

    private int _ballsCount = 8;
    private bool _isSimulationRunning = false;
    private IDisposable? unsubscriber;

    public int BallsCount
    {
        get => _ballsCount;
        set => SetField(ref _ballsCount, value, _ballsCountValidator, 1);
    }
    public bool IsSimulationRunning
    {
        get => _isSimulationRunning;
        private set => SetField(ref _isSimulationRunning, value);
    }
    public ObservableCollection<IBallModel> Balls { get; } = new ();
    public ICommand StartSimulationCommand { get; init; }
    public ICommand StopSimulationCommand { get; init; }

    public SimulationViewModel(ModelAbstractApi? model = default, IValidator<int>? ballsCountValidator = default)
        : base()
    {
        _model = model ?? ModelAbstractApi.CreateModelApi();
        _ballsCountValidator = ballsCountValidator ?? new BallsCountValidator();

        StartSimulationCommand = new StartSimulationCommand(this);
        StopSimulationCommand = new StopSimulationCommand(this);
    }

    public void StartSimulation()
    {
        IsSimulationRunning = true;
        Follow(_model);
        _model.Start(BallsCount);
    }

    public void StopSimulation()
    {
        IsSimulationRunning = false;
        Balls.Clear();
        _model.Stop();
    }

    #region Observer

    public void Follow(IObservable<IBallModel> provider)
    {
        unsubscriber = provider.Subscribe(this);
    }

    public void OnCompleted()
    {
        unsubscriber?.Dispose();
    }

    public void OnError(Exception error)
    {
        throw error;
    }

    public void OnNext(IBallModel ball)
    {
        Balls.Add(ball);
    }

    #endregion
}
