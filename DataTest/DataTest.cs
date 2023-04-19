using Data;

namespace DataTest
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void CreateASingleBallTest()
        {
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance(1000, 1000);
            Ball NewlyCreatedBall = DataAPI.CreateASingleBall();
            Assert.AreNotEqual(null, NewlyCreatedBall);
        }

        [TestMethod]
        public void CheckIfConstructorOfTheBallIsCorrect()
        {
            int XCoordinate = 678;
            int YCoordinate = 876;
            Position CenterOfTheBall = new Position(XCoordinate, YCoordinate);
            int VelocityX = 10;
            int VelocityY = 10;
            Position VelocityVector = new Position(VelocityX, VelocityY);
            int RadiusOfTheBall = 15;
            double MassOfTheBall = 17.2;
            Ball NewBall = new Ball(MassOfTheBall, CenterOfTheBall, RadiusOfTheBall, VelocityVector);
            Assert.AreEqual(XCoordinate, NewBall.CenterOfTheBall.XCoordinate);
            Assert.AreEqual(YCoordinate, NewBall.CenterOfTheBall.YCoordinate);
            Assert.AreEqual(VelocityX, NewBall.VelocityVector.YCoordinate);
            Assert.AreEqual(VelocityY, NewBall.VelocityVector.YCoordinate);
            Assert.AreEqual(RadiusOfTheBall, NewBall.BallRadius);
            Assert.AreEqual(MassOfTheBall, NewBall.MassOfTheBall);
        }

        [TestMethod]
        public void TestFor1000Balls()
        {
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance(740, 690);
            List<Ball> ListOfBalls = new List<Ball>();
            for(int i = 0; i < 1000; i++)
            {
                ListOfBalls.Add(DataAPI.CreateASingleBall());
            }
            bool correct = true;
            for (int i = 0; i < 1000; i++)
            {
                if (ListOfBalls[i].CenterOfTheBall.XCoordinate - ListOfBalls[i].BallRadius < 0 ||
                    ListOfBalls[i].CenterOfTheBall.XCoordinate + ListOfBalls[i].BallRadius > 740)
                {
                    correct = false;
                    break;
                }
                if (ListOfBalls[i].CenterOfTheBall.YCoordinate - ListOfBalls[i].BallRadius < 0 ||
                    ListOfBalls[i].CenterOfTheBall.YCoordinate + ListOfBalls[i].BallRadius > 690)
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
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance(WidthOfTheTable, HeightOfTheTable);
            Ball NewlyCreatedBall = DataAPI.CreateASingleBall();
            Assert.AreNotEqual(null, NewlyCreatedBall);
            bool correct = true;
            if (NewlyCreatedBall.CenterOfTheBall.XCoordinate - NewlyCreatedBall.BallRadius < 0 || 
                NewlyCreatedBall.CenterOfTheBall.XCoordinate + NewlyCreatedBall.BallRadius > WidthOfTheTable)
            {
                correct = false;
            }
            if (NewlyCreatedBall.CenterOfTheBall.YCoordinate - NewlyCreatedBall.BallRadius < 0 ||
                NewlyCreatedBall.CenterOfTheBall.YCoordinate + NewlyCreatedBall.BallRadius > HeightOfTheTable)
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
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance(WidthOfTheTable, HeightOfTheTable);
            Ball NewlyCreatedBall = DataAPI.CreateASingleBall();
            Assert.AreNotEqual(null, NewlyCreatedBall);
            bool correct = true;
            if (NewlyCreatedBall.CenterOfTheBall.XCoordinate - NewlyCreatedBall.BallRadius + NewlyCreatedBall.VelocityVector.XCoordinate < 0 ||
                NewlyCreatedBall.CenterOfTheBall.XCoordinate + NewlyCreatedBall.BallRadius + NewlyCreatedBall.VelocityVector.XCoordinate > WidthOfTheTable)
            {
                correct = false;
            }
            if (NewlyCreatedBall.CenterOfTheBall.YCoordinate - NewlyCreatedBall.BallRadius + NewlyCreatedBall.VelocityVector.YCoordinate < 0 ||
                NewlyCreatedBall.CenterOfTheBall.YCoordinate + NewlyCreatedBall.BallRadius + NewlyCreatedBall.VelocityVector.YCoordinate > HeightOfTheTable)
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
            Position NewPosition = new Position(XCoordinate, YCoordinate);
            Assert.AreEqual(XCoordinate, NewPosition.XCoordinate);
            Assert.AreEqual(YCoordinate, NewPosition.YCoordinate);
        }

        [TestMethod]
        public void GetMassOfTheBall()
        {
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance(700, 700);
            double MassOfTheBall = DataAPI.GetMassOfTheBall();
            Assert.AreEqual(10.0, MassOfTheBall);
        }

        [TestMethod]
        public void GetRadiusOfTheBall()
        {
            DataAbstractAPI DataAPI = DataAbstractAPI.CreateDataAPIInstance(500, 500);
            int RadiusOfTheBall = DataAPI.GetRadiusOfTheBall();
            Assert.AreEqual(10, RadiusOfTheBall);
        }
    }
}