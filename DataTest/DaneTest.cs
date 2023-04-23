using Dane;

namespace DataTest
{
    [TestClass]
    public class DaneTest
    {
        [TestMethod]
        public void creatingApiTest()
        {
            DataAbstractAPI test_api = DataAbstractAPI.CreateApi(1000, 1000);
            Assert.IsNotNull(test_api);
        }
        [TestMethod]
        public void creatingBallTest()
        {
            DataAbstractAPI test_api = DataAbstractAPI.CreateApi(1000, 1000);
            Ball test_ball = test_api.CreateBall();
            Assert.IsNotNull(test_ball);
            Assert.AreEqual(10, test_api.getMassOfTheBall());
            Assert.AreEqual(10, test_api.getRadiusOfTheBall());
            Assert.AreNotEqual(0, test_ball.velocityVector.xCoordinate);
            Assert.AreNotEqual(0, test_ball.velocityVector.yCoordinate);
        }
    }
}