using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExampleProgram;

namespace ExampleProgramTest;

[TestClass]
public class EuclidesTest
{
    [TestMethod]
    public void TestGCD()
    {
        int a = 22;
        int b = 33;
        int expectedGCD = 11;

        var euclidean = new Euclidean(); 

        int acutalGCD = euclidean.GCD(a, b);

        Assert.AreEqual(acutalGCD, expectedGCD);
    }
}
