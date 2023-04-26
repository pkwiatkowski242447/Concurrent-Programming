using Logic;
namespace LogicTest
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void CreatingApiTest()
        {
            LogicAbstractAPI TestLogicApi = LogicAbstractAPI.CreateLogicApi();
            Assert.IsNotNull(TestLogicApi);
        }
        [TestMethod]
        public void CreateBallsTest()
        {
            LogicAbstractAPI TestLogicApi = LogicAbstractAPI.CreateLogicApi();
            TestLogicApi.CreateSpecifiedNumerOfBalls(10);
            List<List<double>> listOfBallsCoordinates = TestLogicApi.GetAllBallsCoordinates();
            Assert.AreEqual(10, TestLogicApi.GetAllBallsCoordinates().Count);
            bool correct = true;
            for (int i = 0; i < listOfBallsCoordinates.Count; i++)
            {
                if (listOfBallsCoordinates[i] == null)
                {
                    correct = false;
                }
            }
            Assert.AreEqual(true, correct);
        }
        [TestMethod]
        public void ClearPoolTableTest()
        {
            LogicAbstractAPI TestLogicApi = LogicAbstractAPI.CreateLogicApi();
            TestLogicApi.CreateSpecifiedNumerOfBalls(10);
            TestLogicApi.MoveBalls();
            List<List<double>> listOfBalls = TestLogicApi.GetAllBallsCoordinates();
            Assert.AreEqual(10, listOfBalls.Count);
            TestLogicApi.ClearPoolTable();
            listOfBalls = TestLogicApi.GetAllBallsCoordinates();
            Assert.AreEqual(0, listOfBalls.Count);
        }
        [TestMethod]
        public void MoveBallsTest()
        {
            LogicAbstractAPI TestLogicApi = LogicAbstractAPI.CreateLogicApi();
            TestLogicApi.CreateSpecifiedNumerOfBalls(1);
            List<List<double>> ball_before_move = TestLogicApi.GetAllBallsCoordinates();
            TestLogicApi.MoveBalls();
            List<List<double>> ball_after_move = TestLogicApi.GetAllBallsCoordinates();
            CollectionAssert.AreNotEqual(ball_after_move, ball_before_move);
        }
    }
}
