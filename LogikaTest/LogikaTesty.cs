using Logika;
namespace LogikaTest
{
    [TestClass]
    public class DaneTest
    {
        [TestMethod]
        public void creatingApiTest()
        {
            LogicAbstractAPI test_api = LogicAbstractAPI.CreateApi();
            Assert.IsNotNull(test_api);
        }
        [TestMethod]
        public void createBallsTest()
        {
            LogicAbstractAPI test_api = LogicAbstractAPI.CreateApi();
            test_api.CreateBalls(10);
            Assert.AreEqual(10, test_api.GetBalls().Count());
        }
        [TestMethod]
        public void clearBoardTest()
        {
            LogicAbstractAPI test_api = LogicAbstractAPI.CreateApi();
            test_api.CreateBalls(100);
            Assert.AreEqual(100, test_api.GetBalls().Count());
            test_api.ClearBoard();
            Assert.AreEqual(0, test_api.GetBalls().Count());
        }
        [TestMethod]
        public void moveBallsTest()
        {
            LogicAbstractAPI test_api = LogicAbstractAPI.CreateApi();
            test_api.CreateBalls(1);
            List<List<int>> ball_before_move = test_api.GetBalls();
            test_api.MoveBalls();
            List<List<int>> ball_after_move = test_api.GetBalls();
            Assert.AreNotEqual(ball_after_move, ball_before_move);
        }
    }
}
