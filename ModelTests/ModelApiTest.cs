using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.API;

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