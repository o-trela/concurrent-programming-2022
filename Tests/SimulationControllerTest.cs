using BallSimulator.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;

namespace BallSimulator.Tests
{
    [TestClass]
    public class SimulationControllerTests
    {
        private readonly LogicAbstractApi controller;

        public SimulationControllerTests()
        {
            controller = LogicAbstractApi.CreateLogicApi();
        }

        [TestMethod]
        public void BallCreationTest()
        {
            controller.CreateBalls(2);
            Assert.AreEqual(controller.Balls.Count(), 2);
        }

        [TestMethod]
        public void SimulationTest()
        {
            controller.CreateBalls(2);
            float xPos = controller.Balls.First().Position.X;
            controller.StartSimulation();
            Thread.Sleep(100);
            controller.StopSimulation();
            Assert.AreNotEqual(controller.Balls.First().Position.X, xPos);
        }
    }
}
