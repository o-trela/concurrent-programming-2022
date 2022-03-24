using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExampleProgram;
using System;

namespace ExampleProgramTest;

[TestClass]
public class EuclidesTest
{
    [TestMethod]
    public void EuclidesGCDCalculatedCorrectly()
    {
        int a = 22;
        int b = 33;

        int expected = 11;
        int acutal = Euclidean.GCD(a, b);

        Assert.AreEqual(expected, acutal);
    }

    [TestMethod]
    public void EuclidesGCDThrowsArgumentException()
    {
        int a = 0;
        int b = 0;

        Assert.ThrowsException<ArgumentException>(() => Euclidean.GCD(a, b));
    }
}
