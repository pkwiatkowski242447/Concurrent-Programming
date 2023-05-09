using Logic;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {

        [TestMethod]
        public void CreatingLogicAPITest()
        {
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance();
            LogicAPI.CreatePlayingBoard();
            Assert.IsNotNull(LogicAPI);
        }

        [TestMethod]
        public void AddSpecifiedNumberOfBallsTest()
        {
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance();
            LogicAPI.CreatePlayingBoard();
            LogicAPI.CreateSpecifiedNumerOfBalls(10);
            List<List<double>> listOfBallsCoordinates = LogicAPI.GetAllBallsCoordinates();
            Assert.AreEqual(10, listOfBallsCoordinates.Count);
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
        public void MoveGeneratedBallsTest()
        {
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance();
            LogicAPI.CreatePlayingBoard();
            LogicAPI.CreateSpecifiedNumerOfBalls(1);
            List<List<double>> originalListOfBallsCoordinatesNo1 = LogicAPI.GetAllBallsCoordinates();
            LogicAPI.MoveGeneratedBalls();
            List<List<double>> originalListOfBallsCoordinatesNo2 = LogicAPI.GetAllBallsCoordinates();
            bool positionChanges = false;
            for (int i = 0; i < originalListOfBallsCoordinatesNo1.Count; i++)
            {
                if (originalListOfBallsCoordinatesNo1[i][0] == originalListOfBallsCoordinatesNo2[i][0] ||
                    originalListOfBallsCoordinatesNo1[i][1] == originalListOfBallsCoordinatesNo2[i][1])
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
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance();
            LogicAPI.CreatePlayingBoard();
            LogicAPI.CreateSpecifiedNumerOfBalls(10);
            LogicAPI.MoveGeneratedBalls();
            List<List<double>> listOfBalls = LogicAPI.GetAllBallsCoordinates();
            Assert.AreEqual(10, listOfBalls.Count);
            LogicAPI.ClearPoolTable();
            listOfBalls = LogicAPI.GetAllBallsCoordinates();
            Assert.AreEqual(0, listOfBalls.Count);
        }

        [TestMethod]
        public void GetTotalNumberOfBalls()
        {
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance();
            LogicAPI.CreatePlayingBoard();
            LogicAPI.CreateSpecifiedNumerOfBalls(15);
            List<List<double>> listOfBallsCoordinates = LogicAPI.GetAllBallsCoordinates();
            Assert.AreEqual(15, listOfBallsCoordinates.Count);
        }
    }
}