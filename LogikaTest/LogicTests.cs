using Data;
using Logic;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {
        internal class FakeDataAPI : DataAbstractAPI
        {
            public double MassOfTheBall = 15.1;
            public double RadiusOfTheBall = 12.7;
            public int WidthOfTheBoard { get; set; }
            public int HeightOfTheBoard { get; set; }
            public bool CreateBoardCalled { get; set; }
            public Random randomNumber = new Random();

            public override DataBallInterface CreateASingleBall()
            {
                double XCoordinate = randomNumber.NextDouble() * (WidthOfTheBoard - 2 * RadiusOfTheBall);
                double YCoordinate = randomNumber.NextDouble() * (HeightOfTheBoard - 2 * RadiusOfTheBall);
                DataPositionInterface CenterOfTheBall = new Position(XCoordinate, YCoordinate);
                XCoordinate = randomNumber.NextDouble() * 10 - 5;
                YCoordinate = randomNumber.NextDouble() * 10 - 5;
                DataPositionInterface VelocityVectorOfTheBall = new Position(XCoordinate, YCoordinate);
                return new Ball(MassOfTheBall, RadiusOfTheBall, CenterOfTheBall, VelocityVectorOfTheBall);
            }

            public override void CreateBoard(int widthOfTheBoard, int heightOfTheBoard)
            {
                this.WidthOfTheBoard = widthOfTheBoard;
                this.HeightOfTheBoard = heightOfTheBoard;
                this.CreateBoardCalled = true;
            }

            public override int GetHeightOfTheBoard()
            {
                return this.HeightOfTheBoard;
            }

            public override double GetMassOfTheBall()
            {
                return this.MassOfTheBall;
            }

            public override double GetRadiusOfTheBall()
            {
                return this.RadiusOfTheBall;
            }

            public override int GetWidthOfTheBoard()
            {
                return this.WidthOfTheBoard;
            }
        }

        internal class Ball : DataBallInterface
        {
            public override double MassOfTheBall { get; set; }
            public override double RadiusOfTheBall { get; set; }
            public override DataPositionInterface CenterOfTheBall { get; set; }
            public override DataPositionInterface VelocityVectorOfTheBall { get; set; }

            public Ball(double massOfTheBall, double radiusOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall)
            {
                this.MassOfTheBall = massOfTheBall;
                this.RadiusOfTheBall = radiusOfTheBall;
                this.CenterOfTheBall = centerOfTheBall;
                this.VelocityVectorOfTheBall = velocityVectorOfTheBall;
            }

            public override void Move()
            {
                this.CenterOfTheBall.XCoordinate += this.VelocityVectorOfTheBall.XCoordinate;
                this.CenterOfTheBall.YCoordinate += this.VelocityVectorOfTheBall.YCoordinate;
            }
        }

        internal class Position : DataPositionInterface
        {
            public override double XCoordinate { get; set; }
            public override double YCoordinate { get; set; }

            public Position(double xCoordinate, double yCoordinate)
            {
                this.XCoordinate = xCoordinate;
                this.YCoordinate = yCoordinate;
            }
        }



        [TestMethod]
        public void CreatingLogicAPITest()
        {
            FakeDataAPI FakeAPI = new FakeDataAPI();
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance(FakeAPI);
            Assert.IsNotNull(LogicAPI);
        }

        [TestMethod]
        public void CreatingPlayingBoardTest()
        {
            FakeDataAPI FakeAPI = new FakeDataAPI();
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance(FakeAPI);
            LogicAPI.CreatePlayingBoard();

            Assert.AreEqual(FakeAPI.GetWidthOfTheBoard(), 740);
            Assert.AreEqual(FakeAPI.GetHeightOfTheBoard(), 690);
            Assert.AreEqual(FakeAPI.CreateBoardCalled, true);
        }

        [TestMethod]
        public void AddSpecifiedNumberOfBallsTest()
        {
            FakeDataAPI FakeAPI = new FakeDataAPI();
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance(FakeAPI);
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
            FakeDataAPI FakeAPI = new FakeDataAPI();
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance(FakeAPI);
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
            FakeDataAPI FakeAPI = new FakeDataAPI();
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance(FakeAPI);
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
            int NumberOfBallsToCreate = 10;
            FakeDataAPI FakeAPI = new FakeDataAPI();
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance(FakeAPI);
            LogicAPI.CreatePlayingBoard();
            LogicAPI.CreateSpecifiedNumerOfBalls(NumberOfBallsToCreate);
            List<List<double>> listOfBallsCoordinates = LogicAPI.GetAllBallsCoordinates();
            Assert.AreEqual(NumberOfBallsToCreate, listOfBallsCoordinates.Count);
        }
    }
}