using BallSimulator.Presentation.Model;

namespace BallSimulator.Presentation.ViewModel;

public class MainViewModel : ViewModelBase
{
    public ViewModelBase CurrentViewModel { get; }

    public MainViewModel()
        : base()
    {
        CurrentViewModel = new SimulationViewModel(
            ballsCountValidator: new BallsCountValidator(1, 30)
            );
    }
}
