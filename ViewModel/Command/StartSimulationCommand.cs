using System.ComponentModel;

namespace BallSimulator.Presentation.ViewModel;

public class StartSimulationCommand : CommandBase
{
    private readonly SimulationViewModel _simulationViewModel;

    public StartSimulationCommand(SimulationViewModel simulationViewModel)
        : base()
    {
        _simulationViewModel = simulationViewModel;
        _simulationViewModel.PropertyChanged += OnSimulationViewModelPropertyChanged;
    }

    public override bool CanExecute(object? parameter)
    {
        return base.CanExecute(parameter)
            && !_simulationViewModel.IsSimulationRunning;
    }

    public override void Execute(object? parameter)
    {
        _simulationViewModel.StartSimulation();
    }

    private void OnSimulationViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SimulationViewModel.IsSimulationRunning))
        {
            OnCanExecuteChanged();
        }
    }
}
