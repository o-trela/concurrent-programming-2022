using BallSimulator.Presentation.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Linq;

namespace ViewModelTests
{
    [TestClass]
    public class SimulationViewModelTest
    {
        private readonly SimulationViewModel simulationViewModel = new();

        [TestMethod]
        public void BallsCountPropertyChanged()
        {
            bool ballsCountChangedRaised = false;

            simulationViewModel.PropertyChanged += (object? sender, PropertyChangedEventArgs e) => ballsCountChangedRaised = true;

            Assert.IsFalse(ballsCountChangedRaised);

            simulationViewModel.BallsCount = 15;
            Assert.IsTrue(ballsCountChangedRaised);
        }

        [TestMethod]
        public void StartStopSimulationTest()
        {
            bool isSimulationRunningChangedRaised = false;

            simulationViewModel.PropertyChanged += (object? sender, PropertyChangedEventArgs e) => isSimulationRunningChangedRaised = true;

            Assert.IsFalse(simulationViewModel.IsSimulationRunning);
            Assert.IsFalse(isSimulationRunningChangedRaised);

            simulationViewModel.StartSimulation();
            Assert.IsTrue(simulationViewModel.IsSimulationRunning);
            Assert.IsTrue(isSimulationRunningChangedRaised);

            simulationViewModel.StopSimulation();
            Assert.IsFalse(simulationViewModel.IsSimulationRunning);
        }

        [TestMethod]
        public void UpdateBallsTest()
        {
            bool ballsChangedRaised = false;

            simulationViewModel.PropertyChanged += (object? sender, PropertyChangedEventArgs e) => ballsChangedRaised = true;

            Assert.IsFalse(ballsChangedRaised);
            var collectionBefore = simulationViewModel.Balls;

            simulationViewModel.StartSimulation();
            simulationViewModel.OnNext(collectionBefore.First());

            /*Assert.IsTrue(ballsChangedRaised);*/
            var collectionAfter = simulationViewModel.Balls;

            Assert.AreSame(collectionBefore, collectionAfter);
            simulationViewModel.StopSimulation();
        }
    }
}