using Model;

namespace ModelTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ModelAbstractAPI test_api = ModelAbstractAPI.CreateModelAPI(1000, 1000);
            test_api.MoveBalls(1);
            ModelBall test_1 = test_api.GetModelBall(0);
            Thread.Sleep(1000);
            ModelBall test_2 = test_api.GetModelBall(0);
            Assert.AreNotEqual(test_1.TopValue, test_2.TopValue);
            Assert.AreNotEqual(test_1.LeftValue, test_2.LeftValue);
        }
    }
}