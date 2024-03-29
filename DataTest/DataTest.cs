using Data;
namespace DataTest
{
    [TestClass]
    public class DataTest
    {
        // Ball tests

        [TestMethod]
        public void CreateASingleBallTest()
        {
            double RadiusOfTheBall = 10.0;

            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(1000, 1000);

            DataBallInterface NewlyCreatedBall = DataAPI.CreateASingleBall(0, RadiusOfTheBall);
            Assert.AreNotEqual(null, NewlyCreatedBall);
        }

        [TestMethod]
        public void CheckIfConstructorOfTheBallIsCorrect()
        {
            int XCoordinate = 678;
            int YCoordinate = 876;

            DataPositionInterface CenterOfTheBall = DataPositionInterface.CreatePosition(XCoordinate, YCoordinate);

            int VelocityX = 10;
            int VelocityY = 10;

            DataPositionInterface VelocityVectorOfTheBall = DataPositionInterface.CreatePosition(VelocityX, VelocityY);

            DataBallInterface NewBall = DataBallInterface.CreateBall(0, CenterOfTheBall, VelocityVectorOfTheBall, null);

            Assert.AreEqual(XCoordinate, NewBall.CenterOfTheBall.XCoordinate);
            Assert.AreEqual(YCoordinate, NewBall.CenterOfTheBall.YCoordinate);
            Assert.AreEqual(VelocityX, NewBall.VelocityVectorOfTheBall.YCoordinate);
            Assert.AreEqual(VelocityY, NewBall.VelocityVectorOfTheBall.YCoordinate);
        }

        [TestMethod]
        public void CheckIfCenterOfTheBallIsCorrect()
        {
            int WidthOfTheTable = 910;
            int HeightOfTheTable = 678;
            double RadiusOfTheBall = 10.0;

            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(WidthOfTheTable, HeightOfTheTable);

            DataBallInterface NewlyCreatedBall = DataAPI.CreateASingleBall(0, RadiusOfTheBall);

            Assert.AreNotEqual(null, NewlyCreatedBall);

            bool correct = true;

            if (NewlyCreatedBall.CenterOfTheBall.XCoordinate - RadiusOfTheBall < 0 ||
                NewlyCreatedBall.CenterOfTheBall.XCoordinate + RadiusOfTheBall > WidthOfTheTable)
            {
                correct = false;
            }
            if (NewlyCreatedBall.CenterOfTheBall.YCoordinate - RadiusOfTheBall < 0 ||
                NewlyCreatedBall.CenterOfTheBall.YCoordinate + RadiusOfTheBall > HeightOfTheTable)
            {
                correct = false;
            }
            Assert.AreEqual(true, correct);
        }

        [TestMethod]
        public void CenterOfTheBallValidityTestFor1000Balls()
        {
            double RadiusOfTheBall = 10.0;

            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(740, 690);
            List<DataBallInterface> ListOfBalls = new List<DataBallInterface>();
            for (int i = 0; i < 1000; i++)
            {
                ListOfBalls.Add(DataAPI.CreateASingleBall(i, RadiusOfTheBall));
            }
            bool correct = true;
            for (int i = 0; i < 1000; i++)
            {
                if (ListOfBalls[i].CenterOfTheBall.XCoordinate - RadiusOfTheBall < 0 ||
                    ListOfBalls[i].CenterOfTheBall.XCoordinate + RadiusOfTheBall > 740)
                {
                    correct = false;
                    break;
                }
                if (ListOfBalls[i].CenterOfTheBall.YCoordinate - RadiusOfTheBall < 0 ||
                    ListOfBalls[i].CenterOfTheBall.YCoordinate + RadiusOfTheBall > 690)
                {
                    correct = false;
                    break;
                }
            }
            Assert.AreEqual(true, correct);
        }

        // Position tests

        [TestMethod]
        public void CheckIfConstructorOfThePositionIsCorrect()
        {
            int XCoordinate = 700;
            int YCoordinate = 678;

            DataPositionInterface NewPosition = DataPositionInterface.CreatePosition(XCoordinate, YCoordinate);

            Assert.AreEqual(XCoordinate, NewPosition.XCoordinate);
            Assert.AreEqual(YCoordinate, NewPosition.YCoordinate);
        }

        // Board tests

        [TestMethod]
        public void CreateBoardTest()
        {
            int WidthOfTheBoard = 123;
            int HeightOfTheBoard = 456;
            DataBoardInterface Board = DataBoardInterface.CreateBoard(WidthOfTheBoard, HeightOfTheBoard);
            Assert.AreNotEqual(null, Board);
        }

        [TestMethod]
        public void GetWidthOfTheBoardTest()
        {
            int WidthOfTheBoard = 123;
            int HeightOfTheBoard = 456;
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(WidthOfTheBoard, HeightOfTheBoard);
            Assert.AreEqual(WidthOfTheBoard, DataAPI.GetWidthOfTheBoard());
        }

        [TestMethod]
        public void GetHeightOfTheBoardTest()
        {
            int WidthOfTheBoard = 123;
            int HeightOfTheBoard = 456;
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(WidthOfTheBoard, HeightOfTheBoard);
            Assert.AreEqual(HeightOfTheBoard, DataAPI.GetHeightOfTheBoard());
        }

        // DataPosition tests

        [TestMethod]
        public void DataPositionFactoryTest()
        {
            double XCoordinate = 127;
            double YCoordinate = 421;

            DataPositionInterface Position = DataPositionInterface.CreatePosition(XCoordinate, YCoordinate);

            Assert.AreEqual(XCoordinate, Position.XCoordinate);
            Assert.AreEqual(YCoordinate, Position.YCoordinate);
        }

        [TestMethod]
        public void VectorLengthTestMethod()
        {
            double XCoordinate = 127;
            double YCoordinate = 237;
            double ExpectedResult = Math.Sqrt(Math.Pow(XCoordinate, 2) + Math.Pow(YCoordinate, 2));
            DataPositionInterface position = DataPositionInterface.CreatePosition(XCoordinate, YCoordinate);
            double ActualResult = position.VectorLength();

            Assert.AreEqual(ExpectedResult, ActualResult);
        }
    }
}