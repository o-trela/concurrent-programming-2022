using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator.Data;

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
        private static readonly Vector2 Position = new(TestXPos, TestYPos);
        private static readonly Vector2 Speed = new(TestXSpeed, TestYSpeed);

        private readonly Ball _testBall;

        public BallTest()
        {
            _testBall = new Ball(TestDiameter, Position, Speed);
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
            float delta = 100f;
            var ball = new Ball(TestDiameter, Position, Vector2.Zero)
            {
                Speed = new Vector2(0f, -2.5f)
            };
            Assert.AreEqual(ball.Speed.X, 0f);

            ball.Move(delta);
            Assert.AreEqual(ball.Position.X, TestXPos);

            ball.Move(delta);
            ball.Move(delta);
            ball.Move(delta);

            ball.Speed = new Vector2(3f, 5f);
            Assert.AreEqual(ball.Speed, new Vector2(3f, 5f));

            ball.Move(delta);
            Assert.AreEqual(ball.Position.Y, 10f);

            ball.Move(delta);
            ball.Move(delta);
            ball.Move(delta);

            Assert.AreEqual(ball.Position, new Vector2(17f, 25f));
        }

        [TestMethod]
        public void EqualTest()
        {
            Ball newBall = _testBall;
            Assert.AreEqual(_testBall, newBall);
        }
    }
}