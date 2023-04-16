using Logika;

namespace TestProject
{
    [TestClass]
    public class LogicTest
    {
        [TestMethod]
        public void creatingLogicAPITest()
        {
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstace(1000, 1000);
            Assert.IsNotNull(LogicAPI);
        }

        [TestMethod]
        public void AddSpecifiedNumberOfBallsTest()
        {
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstace(1000, 1000);
            LogicAPI.AddSpecifiedNumerOfBalls(10);
            List<Ball> listOfBalls = LogicAPI.GetBallsList();
            Assert.AreEqual(10, listOfBalls.Count);
            bool correct = true;
            for (int i = 0; i < listOfBalls.Count; i++)
            {
                if (listOfBalls[i] == null)
                {
                    correct = false;
                }
            }
            Assert.AreEqual(true, correct);
        }

        [TestMethod]
        public void MoveGeneratedBallsTest()
        {
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstace(500, 500);
            LogicAPI.AddSpecifiedNumerOfBalls(1);
            List<Ball> originalListOfBalls = LogicAPI.GetBallsList();
            List<Position> listOfStartingPositions = new List<Position>();
            foreach (Ball ball in originalListOfBalls)
            {
                listOfStartingPositions.Add(new Position(ball.centerOfTheBall.xCoordinate, ball.centerOfTheBall.yCoordinate));
            }
            LogicAPI.MoveGeneratedBalls();
            Thread.Sleep(25);
            originalListOfBalls = LogicAPI.GetBallsList();
            List<Position> listOfEndingPositions = new List<Position>();
            foreach (Ball ball in originalListOfBalls)
            {
                listOfEndingPositions.Add(new Position(ball.centerOfTheBall.xCoordinate, ball.centerOfTheBall.yCoordinate));
            }
            bool positionChanges = false;
            for (int i = 0; i < listOfStartingPositions.Count; i++)
            {
                if (listOfStartingPositions[i].xCoordinate != listOfEndingPositions[i].xCoordinate
                    || listOfStartingPositions[i].yCoordinate != listOfEndingPositions[i].yCoordinate)
                {
                    positionChanges = true;
                    break;
                }
            }
            Assert.AreEqual(true, positionChanges);
        }

        [TestMethod]
        public void ClearPoolTableTest()
        {
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstace(1000, 500);
            LogicAPI.AddSpecifiedNumerOfBalls(10);
            LogicAPI.MoveGeneratedBalls();
            List<Ball> listOfBalls = LogicAPI.GetBallsList();
            Assert.AreEqual(10, listOfBalls.Count);
            LogicAPI.ClearPoolTable();
            listOfBalls = LogicAPI.GetBallsList();
            Assert.AreEqual(0, listOfBalls.Count);
        }

        [TestMethod]
        public void GetTotalNumberOfBalls()
        {
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstace(1000, 750);
            LogicAPI.AddSpecifiedNumerOfBalls(15);
            List<Ball> listOfBalls = LogicAPI.GetBallsList();
            Assert.AreEqual(15, listOfBalls.Count);
        }
    }
}