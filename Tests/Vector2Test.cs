using BallSimulator.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BallSimulator.Tests
{
    [TestClass]
    public class Vector2Test
    {
        private const float Delta = 1E-6f;
        private const float A = 2f;
        private const float B = 0.4f;
        private Vector2 Vec1 = new Vector2(A, B);
        private Vector2 Vec2 = new Vector2(B, A);

        [TestMethod]
        public void ConstructorTest()
        {
            Assert.AreEqual(A, Vec1.X);
            Assert.AreEqual(B, Vec1.Y);

            Assert.AreEqual(B, Vec2.X);
            Assert.AreEqual(A, Vec2.Y);
        }

        [TestMethod]
        public void EqualsTest()
        {
            Vector2 vec1 = new Vector2(A, B);
            Vector2 vec2 = new Vector2(A, B);

            Assert.AreEqual(vec1, vec2);
        }

        [TestMethod]
        public void AdditionTest()
        {
            Assert.AreEqual(new Vector2(2.4f, 2.4f), Vec1 + Vec2);
        }

        [TestMethod]
        public void SubtractionTest()
        {
            Assert.AreEqual(new Vector2(1.6f, -1.6f), Vec1 - Vec2);
            Assert.AreEqual(new Vector2(-1.6f, 1.6f), Vec2 - Vec1);
        }

        [TestMethod]
        public void MultiplicationTest()
        {
            Assert.AreEqual(new Vector2(0.8f, 0.8f), Vec1 * Vec2);
        }

        [TestMethod]
        public void DivisionTest()
        {
            Assert.AreEqual(new Vector2(5f, 0.2f), Vec1 / Vec2);
        }

        [TestMethod]
        public void DeconstructTest()
        {
            var (a, b) = Vec1;
            Assert.AreEqual(a, A);
            Assert.AreEqual(b, B);
        }

        [TestMethod]
        public void DistanceSquaredTest()
        {
            Assert.AreEqual(5.12f, Vector2.DistanceSquared(Vec1, Vec2), Delta);
        }

        [TestMethod]
        public void DistanceTest()
        {
            Assert.AreEqual(2.2627417f, Vector2.Distance(Vec1, Vec2), Delta);
        }
    }
}