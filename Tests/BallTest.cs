using BallSimulator.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BallSimulator.Tests
{
    [TestClass]
    public class BallTest
    {
        readonly int _testXPos;
        readonly int _testYPos;
        readonly float _testXSpeed;
        readonly float _testYSpeed;
        readonly int _testDiameter;

        readonly Ball _testBall;

        public BallTest()
        {
            _testXPos = 5;
            _testYPos = 5;
            _testXSpeed = 0.2f;
            _testYSpeed = -0.1f;
            _testDiameter = 2;

            Vector2 position = new Vector2(_testXPos, _testYPos);
            Vector2 speed = new Vector2(_testXSpeed, _testYSpeed);

            _testBall = new Ball(_testDiameter, position, speed);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(_testBall);

            Assert.AreEqual((int)_testBall.Position.X, _testXPos);
            Assert.AreEqual((int)_testBall.Position.Y, _testYPos);
            Assert.AreEqual(_testBall.Diameter, _testDiameter);
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

        [TestMethod]
        public void MoveTest()
        {
            Vector2 boundX = new Vector2(0, 100);
            Vector2 boundY = new Vector2(0, 100);
            Vector2 speed = new Vector2(-2.5f, 0);

            _testBall.Speed = speed;

            _testBall.Move(boundX, boundY, 1);
            Assert.AreEqual(_testBall.Position.X, _testXPos - 2.5);

            _testBall.Move(boundX, boundY, 1);
            _testBall.Move(boundX, boundY, 1);
            _testBall.Move(boundX, boundY, 1);

            Assert.AreEqual(_testBall.Speed.X, -speed.X);

            speed = new Vector2(0, -2.5f);
            _testBall.Speed = speed;

            _testBall.Move(boundX, boundY, 1);
            Assert.AreEqual(_testBall.Position.Y, _testYPos - 2.5);

            _testBall.Move(boundX, boundY, 1);
            _testBall.Move(boundX, boundY, 1);
            _testBall.Move(boundX, boundY, 1);

            Assert.AreEqual(_testBall.Speed, -speed);

            Assert.ThrowsException<ArgumentException>(() => _testBall.Move(boundX, boundY, 2));
        }

        [TestMethod]
        public void EqualTest()
        {
            Ball newBall = _testBall;
            Assert.AreEqual(_testBall, newBall);
        }
    }
}