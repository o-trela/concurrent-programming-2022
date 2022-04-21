using BallSimulator.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BallSimulator.Tests
{
    [TestClass]
    public class SimulationManagerTest
    {
        private const int TestRadius = 2;
        private const int TestHeight = 100;
        private const int TestWidth = 100;
        private readonly SimulationManager _ballManager = new SimulationManager(new Board(TestHeight, TestWidth), TestRadius);

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

            Ball[] balls = _ballManager.RandomBallCreation(ballNumber);
            int counter = 0;

            foreach (Ball b in balls)
            {
                Assert.IsNotNull(b);
                Assert.AreEqual(TestRadius, b.Radius);
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
