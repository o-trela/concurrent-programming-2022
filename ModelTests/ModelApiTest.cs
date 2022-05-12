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
            ModelAbstractApi modelApi = ModelAbstractApi.CreateModelApi();
            Assert.IsNotNull(modelApi);
        }
    }
}