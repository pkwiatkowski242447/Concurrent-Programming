using Data;

namespace DataTest
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void CreateASingleBallTest()
        {
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(1000, 1000);
            DataBallInterface NewlyCreatedBall = DataAPI.CreateASingleBall();
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

            int RadiusOfTheBall = 15;
            double MassOfTheBall = 17.2;

            DataBallInterface NewBall = DataBallInterface.CreateBall(MassOfTheBall, RadiusOfTheBall, CenterOfTheBall, VelocityVectorOfTheBall);

            Assert.AreEqual(XCoordinate, NewBall.CenterOfTheBall.XCoordinate);
            Assert.AreEqual(YCoordinate, NewBall.CenterOfTheBall.YCoordinate);
            Assert.AreEqual(VelocityX, NewBall.VelocityVectorOfTheBall.YCoordinate);
            Assert.AreEqual(VelocityY, NewBall.VelocityVectorOfTheBall.YCoordinate);
            Assert.AreEqual(RadiusOfTheBall, NewBall.RadiusOfTheBall);
            Assert.AreEqual(MassOfTheBall, NewBall.MassOfTheBall);
        }

        [TestMethod]
        public void TestFor1000Balls()
        {
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(740, 690);
            List<DataBallInterface> ListOfBalls = new List<DataBallInterface>();
            for(int i = 0; i < 1000; i++)
            {
                ListOfBalls.Add(DataAPI.CreateASingleBall());
            }
            bool correct = true;
            for (int i = 0; i < 1000; i++)
            {
                if (ListOfBalls[i].CenterOfTheBall.XCoordinate - ListOfBalls[i].RadiusOfTheBall < 0 ||
                    ListOfBalls[i].CenterOfTheBall.XCoordinate + ListOfBalls[i].RadiusOfTheBall > 740)
                {
                    correct = false;
                    break;
                }
                if (ListOfBalls[i].CenterOfTheBall.YCoordinate - ListOfBalls[i].RadiusOfTheBall < 0 ||
                    ListOfBalls[i].CenterOfTheBall.YCoordinate + ListOfBalls[i].RadiusOfTheBall > 690)
                {
                    correct = false;
                    break;
                }
            }
            Assert.AreEqual(true, correct);
        }

        [TestMethod]
        public void CheckIfCenterOfTheBallIsCorrect()
        {
            int WidthOfTheTable = 910;
            int HeightOfTheTable = 678;

            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(WidthOfTheTable, HeightOfTheTable);

            DataBallInterface NewlyCreatedBall = DataAPI.CreateASingleBall();

            Assert.AreNotEqual(null, NewlyCreatedBall);

            bool correct = true;

            if (NewlyCreatedBall.CenterOfTheBall.XCoordinate - NewlyCreatedBall.RadiusOfTheBall < 0 || 
                NewlyCreatedBall.CenterOfTheBall.XCoordinate + NewlyCreatedBall.RadiusOfTheBall > WidthOfTheTable)
            {
                correct = false;
            }
            if (NewlyCreatedBall.CenterOfTheBall.YCoordinate - NewlyCreatedBall.RadiusOfTheBall < 0 ||
                NewlyCreatedBall.CenterOfTheBall.YCoordinate + NewlyCreatedBall.RadiusOfTheBall > HeightOfTheTable)
            {
                correct = false;
            }
            Assert.AreEqual(true, correct);
        }

        [TestMethod]
        public void CheckIfVelocityVectorIsCorrect()
        {
            int WidthOfTheTable = 910;
            int HeightOfTheTable = 678;

            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(WidthOfTheTable, HeightOfTheTable);

            DataBallInterface NewlyCreatedBall = DataAPI.CreateASingleBall();

            Assert.AreNotEqual(null, NewlyCreatedBall);

            bool correct = true;

            if (NewlyCreatedBall.CenterOfTheBall.XCoordinate - NewlyCreatedBall.RadiusOfTheBall + NewlyCreatedBall.VelocityVectorOfTheBall.XCoordinate < 0 ||
                NewlyCreatedBall.CenterOfTheBall.XCoordinate + NewlyCreatedBall.RadiusOfTheBall + NewlyCreatedBall.VelocityVectorOfTheBall.XCoordinate > WidthOfTheTable)
            {
                correct = false;
            }
            if (NewlyCreatedBall.CenterOfTheBall.YCoordinate - NewlyCreatedBall.RadiusOfTheBall + NewlyCreatedBall.VelocityVectorOfTheBall.YCoordinate < 0 ||
                NewlyCreatedBall.CenterOfTheBall.YCoordinate + NewlyCreatedBall.RadiusOfTheBall + NewlyCreatedBall.VelocityVectorOfTheBall.YCoordinate > HeightOfTheTable)
            {
                correct = false;
            }
            Assert.AreEqual(true, correct);
        }

        [TestMethod]
        public void CheckIfConstructorOfThePositionIsCorrect()
        {
            int XCoordinate = 700;
            int YCoordinate = 678;

            DataPositionInterface NewPosition = DataPositionInterface.CreatePosition(XCoordinate, YCoordinate);

            Assert.AreEqual(XCoordinate, NewPosition.XCoordinate);
            Assert.AreEqual(YCoordinate, NewPosition.YCoordinate);
        }

        [TestMethod]
        public void GetMassOfTheBall()
        {
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(700, 700);
            double MassOfTheBall = DataAPI.GetMassOfTheBall();
            Assert.AreEqual(10.0, MassOfTheBall);
        }

        [TestMethod]
        public void GetRadiusOfTheBall()
        {
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance();
            DataAPI.CreateBoard(500, 500);
            double RadiusOfTheBall = DataAPI.GetRadiusOfTheBall();
            Assert.AreEqual(10, RadiusOfTheBall);
        }
    }
}