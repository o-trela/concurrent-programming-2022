using Microsoft.VisualStudio.TestTools.UnitTesting;

using Logic;

namespace Tests
{
    [TestClass]
    public class BallTest
    {
        readonly int _testXPos;
        readonly int _testYPos;
        readonly int _testRadius;

        readonly Ball _testBall;

        public BallTest()
        {
            _testXPos = 5;
            _testYPos = 5;  
            _testRadius = 2;

            _testBall = new Ball(_testRadius, _testXPos, _testYPos);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(_testBall);

            Assert.AreEqual(_testBall.PosX, _testXPos);
            Assert.AreEqual(_testBall.PosY, _testYPos);
            Assert.AreEqual(_testBall.Radius, _testRadius);
        }

        [TestMethod]
        public void SetAttributesTest()
        {
            int diff = 1;
            int newRadius = _testRadius + diff;
            int newXPos = _testXPos + diff;
            int newYPos = _testYPos + diff;

            _testBall.Radius = newRadius;
            Assert.AreEqual(newRadius, _testBall.Radius);
            
            _testBall.PosX = newXPos;
            Assert.AreEqual(newXPos, _testBall.PosX);

            _testBall.PosY = newYPos;
            Assert.AreEqual(newYPos, _testBall.PosY);
        }
    }
}