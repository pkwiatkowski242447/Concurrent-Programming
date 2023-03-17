using Calculator;

namespace CalculatorTest
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void TestAdd()
        {
            Kalkulator kalkulator = new Kalkulator();
            Assert.AreEqual(kalkulator.Add(0, 1), 1);
            Assert.AreEqual(kalkulator.Add(-5, 0), -5);
            Assert.AreEqual(kalkulator.Add(5, 0), 5);
            Assert.AreEqual(kalkulator.Add(-2, -3), -5);
            Assert.AreEqual(kalkulator.Add(-2, 3), 1);
            Assert.AreEqual(kalkulator.Add(-2, 2), 0);
        }

        [TestMethod]
        public void TestSub()
        {
            Kalkulator kalkulator = new Kalkulator();
            Assert.AreEqual(kalkulator.Sub(2, 1), 1);
            Assert.AreEqual(kalkulator.Sub(0, -1), 1);
            Assert.AreEqual(kalkulator.Sub(-1, -1), 0);
            Assert.AreEqual(kalkulator.Sub(-3, -1), -2);
            Assert.AreEqual(kalkulator.Sub(4, 0), 4);
        }

        [TestMethod]
        public void TestMul()
        {
            Kalkulator kalkulator = new Kalkulator();
            Assert.AreEqual(kalkulator.Mul(0, 10), 0);
            Assert.AreEqual(kalkulator.Mul(5, 0), 0);
            Assert.AreEqual(kalkulator.Mul(5, 3), 15);
            Assert.AreEqual(kalkulator.Mul(-2, 3), -6);
            Assert.AreEqual(kalkulator.Mul(5, -7), -35);
            Assert.AreEqual(kalkulator.Mul(-5, -1), 5);
        }

        [TestMethod]
        public void TestDiv()
        {
            Kalkulator kalkulator = new Kalkulator();
            Assert.AreEqual(kalkulator.Div(0, 3), 0);
            Assert.AreEqual(kalkulator.Div(5, 10), 0,5);
            Assert.AreEqual(kalkulator.Div(-20, 10), -2);
            Assert.AreEqual(kalkulator.Div(42, -6), -7);
            Assert.AreEqual(kalkulator.Div(-33, -6), 5,5);
            Assert.ThrowsException<NotImplementedException>(() => kalkulator.Div(0, 0));
        }
    }
}