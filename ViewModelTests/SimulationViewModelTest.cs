using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator.Presentation.ViewModel;
using System.ComponentModel;
using System.Collections.Generic;

namespace ViewModelTests
{
    [TestClass]
    public class SimulationViewModelTest
    {
        private readonly SimulationViewModel simulationViewModel = new SimulationViewModel();

        [TestMethod]
        public void BallsCountPropertyChanged()
        {
            bool ballsCountChangedRaised = false;

            simulationViewModel.PropertyChanged += (object sender, PropertyChangedEventArgs e) => ballsCountChangedRaised = true;

            Assert.IsFalse(ballsCountChangedRaised);

            simulationViewModel.BallsCount = 15;
            Assert.IsTrue(ballsCountChangedRaised);
        }

        [TestMethod]
        public void StartStopSimulationTest()
        {
            bool isSimulationRunningChangedRaised = false;

            simulationViewModel.PropertyChanged += (object sender, PropertyChangedEventArgs e) => isSimulationRunningChangedRaised = true;

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

            simulationViewModel.PropertyChanged += (object sender, PropertyChangedEventArgs e) => ballsChangedRaised = true;

            Assert.IsFalse(ballsChangedRaised);
            var collectionBefore = simulationViewModel.Balls;

            simulationViewModel.UpdateBalls();

            Assert.IsTrue(ballsChangedRaised);
            var collectionAfter = simulationViewModel.Balls;

            Assert.AreNotSame(collectionBefore, collectionAfter);
        }
    }
}