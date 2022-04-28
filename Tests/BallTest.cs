using BallSimulator.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BallSimulator.Tests
{
    [TestClass]
    public class BallTest
    {
        private static readonly int TestXPos = 5;
        private static readonly int TestYPos = 5;
        private static readonly float TestXSpeed = 0.2f;
        private static readonly float TestYSpeed = -0.1f;
        private static readonly int TestDiameter = 2;

        readonly Ball _testBall;

        public BallTest()
        {
            Vector2 position = new Vector2(TestXPos, TestYPos);
            Vector2 speed = new Vector2(TestXSpeed, TestYSpeed);

            _testBall = new Ball(TestDiameter, position, speed);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(_testBall);

            Assert.AreEqual((int)_testBall.Position.X, TestXPos);
            Assert.AreEqual((int)_testBall.Position.Y, TestYPos);
            Assert.AreEqual(_testBall.Diameter, TestDiameter);
        }

        [TestMethod]
        public void MoveTest()
        {
            Vector2 boundX = new Vector2(0, 100);
            Vector2 boundY = new Vector2(0, 100);
            Ball ball = new Ball(TestDiameter, new Vector2(TestXPos, TestYPos), Vector2.Zero);

            ball.AddSpeed(new Vector2(-2.5f, 0));
            Assert.AreEqual(ball.Speed.X, -2.5f);

            ball.Move(boundX, boundY);
            Assert.AreEqual(ball.Position.X, TestXPos - 2.5f);

            ball.Move(boundX, boundY);
            ball.Move(boundX, boundY);
            ball.Move(boundX, boundY);

            ball.AddSpeed(new Vector2(-2.5f, -2.5f));
            Assert.AreEqual(ball.Speed, new Vector2(0, -2.5f));

            ball.Move(boundX, boundY);
            Assert.AreEqual(ball.Position.Y, TestYPos - 2.5f);

            ball.Move(boundX, boundY);
            ball.Move(boundX, boundY);
            ball.Move(boundX, boundY);

            Assert.ThrowsException<ArgumentException>(() => ball.Move(boundX, boundY, 2));
        }

        [TestMethod]
        public void EqualTest()
        {
            Ball newBall = _testBall;
            Assert.AreEqual(_testBall, newBall);
        }
    }
}