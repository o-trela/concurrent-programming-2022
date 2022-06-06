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

        private readonly Ball _testBall;
        private readonly Board _testBoard;

        public BallTest()
        {
            Vector2 position = new Vector2(TestXPos, TestYPos);
            Vector2 speed = new Vector2(TestXSpeed, TestYSpeed);

            _testBoard = new Board(100, 100);
            _testBall = new Ball(TestDiameter, position, speed, _testBoard);
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
            var ball = new Ball(TestDiameter, new Vector2(TestXPos, TestYPos), Vector2.Zero, _testBoard)
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