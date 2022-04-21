using BallSimulator.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests
{
    [TestClass]
    public class Vector2Test
    {
        private const float A = 2f;
        private const float B = 0.4f;
        private Vector2 Vector1 = new Vector2(A, B);
        private Vector2 Vector2 = new Vector2(B, A);

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.Equals(Vector1.X, A);
            Assert.Equals(Vector1.Y, B);

            Assert.Equals(Vector2.X, B);
            Assert.Equals(Vector2.Y, A);
        }


    }
}