using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator.Presentation.Model;

namespace ModelTests
{
    [TestClass]
    public class ModelApiTest
    {
        [TestMethod]
        public void CreateModelApiTest()
        {
            ModelApi modelApi = ModelApi.CreateModelApi();
            Assert.IsNotNull(modelApi);
        }
    }
}