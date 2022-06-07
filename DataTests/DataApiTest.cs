using Microsoft.VisualStudio.TestTools.UnitTesting;
using BallSimulator.Data.API;


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
            Assert.AreNotEqual(dataApi.MinDiameter, default);
            Assert.AreNotEqual(dataApi.MaxDiameter, default);
            Assert.AreNotEqual(dataApi.MaxSpeed, default);
        }
    }
}