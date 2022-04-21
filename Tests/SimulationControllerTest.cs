using BallSimulator.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            Assert.AreEqual(controller.Balls.Length, 2);
        }

        [TestMethod]
        public void SimulationTest()
        {
            controller.CreateBalls(2);
            float xPos = controller.Balls[0].Position.X;
            controller.StartSimulation();
            Thread.Sleep(100);
            controller.StopSimulation();
            Assert.AreNotEqual(controller.Balls[0].Position.X, xPos);
        }
    }
}
