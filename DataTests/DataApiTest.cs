using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator.Data;

namespace DataTests
{
    [TestClass]
    public class DataApiTest
    {
        [TestMethod]
        public void DataApiCreateTest()
        {
            DataAbstractApi dataApi = DataAbstractApi.CreateDataApi();
            Assert.IsNotNull(dataApi);
        }

        [TestMethod]
        public void PropertiesTest()
        {
            DataAbstractApi dataApi = DataAbstractApi.CreateDataApi();
            Assert.AreNotEqual(dataApi.BoardHeight, default);
            Assert.AreNotEqual(dataApi.BoardWidth, default);
            Assert.AreNotEqual(dataApi.BallRadius, default);
        }
    }
}