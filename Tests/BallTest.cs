using BallSimulator.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BallSimulator.Tests
{
    [TestClass]
    public class BallTest
    {
        readonly int _testXPos;
        readonly int _testYPos;
        readonly float _testXSpeed;
        readonly float _testYSpeed;
        readonly int _testRadius;

        readonly Ball _testBall;

        public BallTest()
        {
            _testXPos = 5;
            _testYPos = 5;
            _testXSpeed = 0.2f;
            _testYSpeed = -0.1f;
            _testRadius = 2;

            Vector2 position = new Vector2(_testXPos, _testYPos);
            Vector2 speed = new Vector2(_testXSpeed, _testYSpeed);

            _testBall = new Ball(_testRadius, position, speed);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(_testBall);

            Assert.AreEqual((int)_testBall.Position.X, _testXPos);
            Assert.AreEqual((int)_testBall.Position.Y, _testYPos);
            Assert.AreEqual(_testBall.Radius, _testRadius);
        }

        [TestMethod]
        public void SetAttributesTest()
        {
            int diff = 1;
            int newXPos = _testXPos + diff;
            int newYPos = _testYPos + diff;
            Vector2 newPos = new Vector2(newXPos, newYPos);

            _testBall.Position = newPos;
            Assert.AreEqual(newPos, _testBall.Position);
        }
    }
}