using BallSimulator.Presentation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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