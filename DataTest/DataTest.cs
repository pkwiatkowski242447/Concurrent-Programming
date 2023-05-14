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

            DataBallInterface NewlyCreatedBall = DataAPI.CreateASingleBall(RadiusOfTheBall);
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

            double MassOfTheBall = 17.2;

            DataBallInterface NewBall = DataBallInterface.CreateBall(MassOfTheBall, CenterOfTheBall, VelocityVectorOfTheBall);

            Assert.AreEqual(XCoordinate, NewBall.CenterOfTheBall.XCoordinate);
            Assert.AreEqual(YCoordinate, NewBall.CenterOfTheBall.YCoordinate);
            Assert.AreEqual(VelocityX, NewBall.VelocityVectorOfTheBall.YCoordinate);
            Assert.AreEqual(VelocityY, NewBall.VelocityVectorOfTheBall.YCoordinate);
            Assert.AreEqual(MassOfTheBall, NewBall.MassOfTheBall);
        }

        [TestMethod]
        public void CheckIfCenterOfTheBallIsCorrect()
        {
            int WidthOfTheTable = 910;
            int HeightOfTheTable = 678;
            double RadiusOfTheBall = 10.0;

            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(WidthOfTheTable, HeightOfTheTable);

            DataBallInterface NewlyCreatedBall = DataAPI.CreateASingleBall(RadiusOfTheBall);

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
        public void CheckIfVelocityVectorOfTheBallIsCorrect()
        {
            int WidthOfTheTable = 910;
            int HeightOfTheTable = 678;
            double RadiusOfTheBall = 10.0;

            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(WidthOfTheTable, HeightOfTheTable);

            DataBallInterface NewlyCreatedBall = DataAPI.CreateASingleBall(RadiusOfTheBall);

            Assert.AreNotEqual(null, NewlyCreatedBall);

            bool correct = true;

            if (NewlyCreatedBall.CenterOfTheBall.XCoordinate - RadiusOfTheBall + NewlyCreatedBall.VelocityVectorOfTheBall.XCoordinate < 0 ||
                NewlyCreatedBall.CenterOfTheBall.XCoordinate + RadiusOfTheBall + NewlyCreatedBall.VelocityVectorOfTheBall.XCoordinate > WidthOfTheTable)
            {
                correct = false;
            }
            if (NewlyCreatedBall.CenterOfTheBall.YCoordinate - RadiusOfTheBall + NewlyCreatedBall.VelocityVectorOfTheBall.YCoordinate < 0 ||
                NewlyCreatedBall.CenterOfTheBall.YCoordinate + RadiusOfTheBall + NewlyCreatedBall.VelocityVectorOfTheBall.YCoordinate > HeightOfTheTable)
            {
                correct = false;
            }
            Assert.AreEqual(true, correct);
        }

        [TestMethod]
        public void GetMassOfTheBallTest()
        {
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(700, 700);
            double MassOfTheBall = DataAPI.GetMassOfTheBall();
            Assert.AreEqual(10.0, MassOfTheBall);
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
                ListOfBalls.Add(DataAPI.CreateASingleBall(RadiusOfTheBall));
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

        // LogicPosition tests

        [TestMethod]
        public void PositionAdditionMethodTest()
        {
            double XCoordinateFirst = 123;
            double XCoordinateSecond = 678;
            double YCoordinateFirst = 321;
            double YCoordinateSecond = 896;
            DataPositionInterface PositionNumber1 = DataPositionInterface.CreatePosition(XCoordinateFirst, YCoordinateFirst);
            DataPositionInterface PositionNumber2 = DataPositionInterface.CreatePosition(XCoordinateSecond, YCoordinateSecond);
            DataPositionInterface ResultPosition = PositionNumber1.Addition(PositionNumber2);

            Assert.AreEqual(XCoordinateFirst + XCoordinateSecond, ResultPosition.XCoordinate);
            Assert.AreEqual(YCoordinateFirst + YCoordinateSecond, ResultPosition.YCoordinate);
        }

        [TestMethod]
        public void PositionMultiplicationMethodTest()
        {
            double someDouble = 2.1;
            double XCoordinate = 123;
            double YCoordinate = 678;
            DataPositionInterface Position = DataPositionInterface.CreatePosition(XCoordinate, YCoordinate);
            DataPositionInterface ResultPosition = Position.Multiplication(someDouble);

            Assert.AreEqual(XCoordinate * someDouble, ResultPosition.XCoordinate);
            Assert.AreEqual(YCoordinate * someDouble, ResultPosition.YCoordinate);
        }

        [TestMethod]
        public void PositionSubtractionMethodTest()
        {
            double XCoordinateFirst = 123;
            double XCoordinateSecond = 678;
            double YCoordinateFirst = 321;
            double YCoordinateSecond = 896;
            DataPositionInterface PositionNumber1 = DataPositionInterface.CreatePosition(XCoordinateFirst, YCoordinateFirst);
            DataPositionInterface PositionNumber2 = DataPositionInterface.CreatePosition(XCoordinateSecond, YCoordinateSecond);
            DataPositionInterface ResultPosition = PositionNumber1.Subtraction(PositionNumber2);

            Assert.AreEqual(XCoordinateFirst - XCoordinateSecond, ResultPosition.XCoordinate);
            Assert.AreEqual(YCoordinateFirst - YCoordinateSecond, ResultPosition.YCoordinate);
        }

        [TestMethod]
        public void PositionDotOperationMethodTest()
        {
            double XCoordinateFirst = 123;
            double XCoordinateSecond = 678;
            double YCoordinateFirst = 321;
            double YCoordinateSecond = 896;
            DataPositionInterface PositionNumber1 = DataPositionInterface.CreatePosition(XCoordinateFirst, YCoordinateFirst);
            DataPositionInterface PositionNumber2 = DataPositionInterface.CreatePosition(XCoordinateSecond, YCoordinateSecond);
            double ResultOfDotOperation = PositionNumber1.DotOperator(PositionNumber2);
            double ExpectedResult = XCoordinateFirst * XCoordinateSecond + YCoordinateFirst * YCoordinateSecond;

            Assert.AreEqual(ExpectedResult, ResultOfDotOperation);
        }

        [TestMethod]
        public void PositionVectorLengthMethodTest()
        {
            double XCoordinate = 123;
            double YCoordinate = 678;
            DataPositionInterface Position = DataPositionInterface.CreatePosition(XCoordinate, YCoordinate);
            double ActualVectorLength = Position.VectorLength();
            double ExpectedVectorLength = Math.Sqrt(Math.Pow(XCoordinate, 2) + Math.Pow(YCoordinate, 2));

            Assert.AreEqual(ExpectedVectorLength, ActualVectorLength);
        }

        [TestMethod]
        public void PositionEuclideanDistanceMethodTest()
        {
            double XCoordinateFirst = 123;
            double XCoordinateSecond = 678;
            double YCoordinateFirst = 321;
            double YCoordinateSecond = 896;
            DataPositionInterface PositionNumber1 = DataPositionInterface.CreatePosition(XCoordinateFirst, YCoordinateFirst);
            DataPositionInterface PositionNumber2 = DataPositionInterface.CreatePosition(XCoordinateSecond, YCoordinateSecond);
            double EuclideanDistance = PositionNumber1.EuclideanDistance(PositionNumber2);
            double ExpectedEuclideanDistance = Math.Sqrt(Math.Pow(PositionNumber1.XCoordinate - PositionNumber2.XCoordinate, 2) + Math.Pow(PositionNumber1.YCoordinate - PositionNumber2.YCoordinate, 2));

            Assert.AreEqual(ExpectedEuclideanDistance, EuclideanDistance);
        }
    }
}