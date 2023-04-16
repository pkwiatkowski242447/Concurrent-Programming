using Logika;

namespace LogikaTest
{
    [TestClass]
    public class LogicAbstractAPITest
    {

        [TestMethod]
        public void creating_api_test()
        {
            LogicAbstractAPI test_api = LogicAbstractAPI.CreateApi(1000, 1000);
            Assert.IsNotNull(test_api);
        }
        [TestMethod]
        public void creating_balls_test()
        {
            LogicAbstractAPI test_api = LogicAbstractAPI.CreateApi(1000, 1000);
            test_api.CreateBalls(5);
            bool result = true;
            List<Ball> balls_list_test = test_api.GetBalls();
            Assert.AreEqual(5, test_api.GetBalls().Count);
            for (int i = 0; i < test_api.GetBalls().Count; i++)
            {
                if (balls_list_test[i] == null)
                {
                    result = false;
                }

            }
            Assert.AreEqual(result, true);
        }
        [TestMethod]
        public void move_balls_test()
        {
            LogicAbstractAPI test_api = LogicAbstractAPI.CreateApi(1000, 1000);
            test_api.CreateBalls(1);
            List<Ball> before_move_list = test_api.GetBalls();
            List<Position> position_before_move = new List<Position>();
            foreach (Ball ball in before_move_list)
            {
                position_before_move.Add(new Position(ball.GetCenterOfTheBall().xCoordinate, ball.GetCenterOfTheBall().yCoordinate));
            }
            test_api.MoveBalls();
            Thread.Sleep(25);
            before_move_list = test_api.GetBalls();
            List<Position> position_after_move = new List<Position>();
            foreach (Ball ball in before_move_list)
            {
                position_after_move.Add(new Position(ball.GetCenterOfTheBall().xCoordinate, ball.GetCenterOfTheBall().yCoordinate));
            }
            bool position_change = false;
            for (int i = 0; i < position_before_move.Count; i++)
            {
                if (position_before_move[i].xCoordinate != position_after_move[i].xCoordinate || position_before_move[i].yCoordinate != position_after_move[i].yCoordinate)
                {
                    position_change = true;
                    break;
                }
            }
            Assert.AreEqual(true, position_change);
        }


    }
}