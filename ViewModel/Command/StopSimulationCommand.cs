using System.ComponentModel;

namespace BallSimulator.Presentation.ViewModel;

public class StopSimulationCommand : CommandBase
{
    private readonly SimulationViewModel _simulationViewModel;

    public StopSimulationCommand(SimulationViewModel simulationViewModel)
        : base()
    {
        _simulationViewModel = simulationViewModel;
        _simulationViewModel.PropertyChanged += OnSimulationViewModelPropertyChanged;
    }

    public override bool CanExecute(object? parameter)
    {
        return base.CanExecute(parameter)
            && _simulationViewModel.IsSimulationRunning;
    }

    public override void Execute(object? parameter)
    {
        _simulationViewModel.StopSimulation();
    }

    private void OnSimulationViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_simulationViewModel.IsSimulationRunning))
        {
            OnCanExecuteChanged();
        }
    }
}
