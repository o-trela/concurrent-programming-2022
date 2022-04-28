using BallSimulator.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BallSimulator.Tests
{
    [TestClass]
    public class SimulationManagerTest
    {
        private const int TestDiameter = 2;
        private const int TestRadius = TestDiameter / 2;
        private const int TestHeight = 100;
        private const int TestWidth = 100;
        private readonly SimulationManager _ballManager = new(new Board(TestHeight, TestWidth), TestDiameter);

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(_ballManager);
        }

        [TestMethod]
        public void RandomBallsCreationTest()
        {
            Assert.IsNotNull(_ballManager);

            int ballNumber = 10;

            IEnumerable<Ball> balls = _ballManager.RandomBallCreation(ballNumber);
            int counter = 0;

            foreach (Ball b in balls)
            {
                Assert.IsNotNull(b);
                Assert.AreEqual(TestDiameter, b.Diameter);
                Assert.IsTrue(IsBallBetween(b.Position.X, 0, TestWidth));
                Assert.IsTrue(IsBallBetween(b.Position.Y, 0, TestHeight));
                counter++;
            }
            Assert.AreEqual(ballNumber, counter);
        }

        private static bool IsBallBetween(float value, float bottom, float top)
        {
            return value <= top - TestRadius && value >= bottom + TestRadius;
        }
    }
}
