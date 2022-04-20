using BallSimulator.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BallSimulator.Tests
{
    [TestClass]
    public class SimulationManagerTest
    {
        readonly int _testRadius;
        readonly int _testHeight;
        readonly int _testWidth;
        readonly SimulationManager _ballManager;
        private Board _board;

        public SimulationManagerTest()
        {
            _testRadius = 2;
            _testHeight = 100;
            _testWidth = 100;
            _board = new Board(_testHeight, _testWidth);

            _ballManager = new SimulationManager(_board, _testRadius);
        }

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
                Assert.AreEqual(_testRadius, b.Radius);
                Assert.IsTrue(IsBallBetween((int)b.Position.X, 0, _testWidth));
                Assert.IsTrue(IsBallBetween((int)b.Position.Y, 0, _testHeight));
                counter++;
            }
            Assert.AreEqual(ballNumber, counter);
        }

        private bool IsBallBetween(int value, int bottom, int top)
        {
            if (value < top - _testRadius && value > bottom + _testRadius)
            {
                return true;
            }
            return false;
        }
    }
}
