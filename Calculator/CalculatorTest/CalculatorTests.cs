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
    }
}