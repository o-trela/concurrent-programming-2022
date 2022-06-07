using BallSimulator.Presentation.Model;
using BallSimulator.Presentation.Model.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ModelTests
{
    [TestClass]
    public class IValidatorTest
    {
        [TestMethod]
        public void BallCountValidatorTest()
        {
            const int min = 1;
            const int max = 20;

            IValidator<int> validator = new BallsCountValidator(min, max);

            Assert.IsTrue(validator.IsValid(max - 1));
            Assert.IsTrue(validator.IsValid(min + 1));

            Assert.IsFalse(validator.IsValid(max + 1));
            Assert.IsFalse(validator.IsValid(min - 1));
        }
    }
}