using Microsoft.VisualStudio.TestTools.UnitTesting;

using Logic;

namespace Tests
{
    [TestClass]
    public class BallManagerTest
    {
        readonly int _testRadius;
        readonly int _testHeight;
        readonly int _testWidth;
        readonly BallManager _ballManager;

        public BallManagerTest()
        {
            _testRadius = 2;
            _testHeight = 100;
            _testWidth = 100;

            _ballManager = new BallManager(_testHeight, _testWidth, _testRadius);
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
                Assert.IsTrue(IsBallBetween(b.PosX, 0, _testWidth));
                Assert.IsTrue(IsBallBetween(b.PosY, 0, _testHeight));
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
