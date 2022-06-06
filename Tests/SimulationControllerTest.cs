using BallSimulator.Data.API;
using BallSimulator.Logic.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BallSimulator.Tests;

[TestClass]
public class LogicTest
{
    private readonly int _testWidth;
    private readonly int _testHeight;
    private readonly int _testMinDiameter;
    private readonly int _testMaxDiameter;
    private readonly DataAbstractApi _dataFixture;

    private LogicAbstractApi _controller;
    private IEnumerable<IBall>? _balls;

    public LogicTest()
    {
        _dataFixture = new DataFixture();
        _controller = LogicAbstractApi.CreateLogicApi(_dataFixture);

        _testWidth = _dataFixture.BoardWidth;
        _testHeight = _dataFixture.BoardHeight;
        _testMinDiameter = _dataFixture.MinDiameter;
        _testMaxDiameter = _dataFixture.MaxDiameter;
    }

    [TestMethod]
    public void ConstructorTest()
    {
        Assert.IsNotNull(_controller);
    }

    [TestMethod]
    public void SimulationTest()
    {
        _controller = LogicAbstractApi.CreateLogicApi(_dataFixture);
        Assert.IsNotNull(_controller);

        _balls = _controller.CreateBalls(2);
        Assert.AreEqual(_balls.Count(), 2);

        float xPos = _balls.First().Position.X;
        Thread.Sleep(100);
        _balls.First().Move(1f);
        Assert.AreNotEqual(_balls.First().Position.X, xPos);
    }

    [TestMethod]
    public void RandomBallsCreationTest()
    {
        _controller = LogicAbstractApi.CreateLogicApi(_dataFixture);
        Assert.IsNotNull(_controller);

        int ballNumber = 10;

        var balls = _controller.CreateBalls(ballNumber);
        int counter = 0;

        foreach (var b in balls)
        {
            Assert.IsNotNull(b);
            Assert.IsTrue(IsBetween(b.Diameter, _testMinDiameter, _testMaxDiameter));
            Assert.IsTrue(IsBetween(b.Radius, _testMinDiameter / 2, _testMaxDiameter / 2));
            Assert.IsTrue(IsBetween(b.Position.X, 0, _testWidth));
            Assert.IsTrue(IsBetween(b.Position.Y, 0, _testHeight));
            counter++;
        }
        Assert.AreEqual(ballNumber, counter);
    }

    private static bool IsBetween(float value, float bottom, float top)
    {
        return value <= top && value >= bottom;
    }

    private class DataFixture : DataAbstractApi
    {
        public override int BoardHeight => 100;
        public override int BoardWidth => 100;
        public override float MaxSpeed => 50;
        public override int MinDiameter => 20;
        public override int MaxDiameter => 50;
    }
}
