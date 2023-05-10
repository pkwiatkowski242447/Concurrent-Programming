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
            public int WidthOfTheBoard { get; set; }
            public int HeightOfTheBoard { get; set; }
            public bool CreateBoardCalled { get; set; }
            public Random randomNumber = new Random();

            public override DataBallInterface CreateASingleBall(double radiusOfTheBall)
            {
                double XCoordinate = randomNumber.NextDouble() * (WidthOfTheBoard - 2 * radiusOfTheBall);
                double YCoordinate = randomNumber.NextDouble() * (HeightOfTheBoard - 2 * radiusOfTheBall);
                DataPositionInterface CenterOfTheBall = new Position(XCoordinate, YCoordinate);
                XCoordinate = randomNumber.NextDouble() * 10 - 5;
                YCoordinate = randomNumber.NextDouble() * 10 - 5;
                DataPositionInterface VelocityVectorOfTheBall = new Position(XCoordinate, YCoordinate);
                return new Ball(MassOfTheBall, CenterOfTheBall, VelocityVectorOfTheBall);
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

            public override int GetWidthOfTheBoard()
            {
                return this.WidthOfTheBoard;
            }
        }

        internal class Ball : DataBallInterface
        {
            public override double MassOfTheBall { get; set; }
            public override DataPositionInterface CenterOfTheBall { get; set; }
            public override DataPositionInterface VelocityVectorOfTheBall { get; set; }
            public override bool StopTask { get; set; }
            public override bool DidBallCollide { get; set; }
            public override bool StartBallMovement { get; set; }

            internal IObserver<DataBallInterface>? ObserverObject;

            public Ball(double massOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall)
            {
                this.MassOfTheBall = massOfTheBall;
                this.CenterOfTheBall = centerOfTheBall;
                this.VelocityVectorOfTheBall = velocityVectorOfTheBall;
                this.StopTask = false;
                this.DidBallCollide = false;
                Task.Run(BallMovement);
            }

            public void BallMovement()
            {
                while(!this.StopTask)
                {
                    if (this.ObserverObject != null)
                    {
                        this.ObserverObject.OnNext(this);
                    }
                    this.Move();
                }
            }

            public void Move()
            {
                this.CenterOfTheBall.XCoordinate += this.VelocityVectorOfTheBall.XCoordinate;
                this.CenterOfTheBall.YCoordinate += this.VelocityVectorOfTheBall.YCoordinate;
            }

            public override IDisposable Subscribe(IObserver<DataBallInterface> observerObject)
            {
                this.ObserverObject = observerObject;
                return new ObserverManager(observerObject);
            }

            private class ObserverManager : IDisposable
            {
                IObserver<DataBallInterface>? SomeObserver;

                public ObserverManager(IObserver<DataBallInterface> observerObject)
                {
                    this.SomeObserver = observerObject;
                }

                public void Dispose()
                {
                    this.SomeObserver = null;
                }
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

            public override DataPositionInterface Addition(DataPositionInterface otherPosition)
            {
                double XCoordinate = this.XCoordinate + otherPosition.XCoordinate;
                double YCoordinate = this.YCoordinate + otherPosition.YCoordinate;
                return CreatePosition(XCoordinate, YCoordinate);
            }

            public override DataPositionInterface Multiplication(double someDouble)
            {
                double XCoordinate = this.XCoordinate * someDouble;
                double YCoordinate = this.YCoordinate * someDouble;
                return CreatePosition(XCoordinate, YCoordinate);
            }

            public override DataPositionInterface Subtraction(DataPositionInterface otherPosition)
            {
                double XCoordinate = this.XCoordinate - otherPosition.XCoordinate;
                double YCoordinate = this.YCoordinate - otherPosition.YCoordinate;
                return CreatePosition(XCoordinate, YCoordinate);
            }

            public override double DotOperator(DataPositionInterface otherPosition)
            {
                return this.XCoordinate * otherPosition.XCoordinate + this.YCoordinate * otherPosition.YCoordinate;
            }

            public override double VectorLength()
            {
                return Math.Sqrt(Math.Pow(this.XCoordinate, 2) + Math.Pow(this.YCoordinate, 2));
            }

            public override double EuclideanDistance(DataPositionInterface otherPosition)
            {
                return Math.Sqrt(Math.Pow(this.XCoordinate - otherPosition.XCoordinate, 2) + Math.Pow(this.YCoordinate - otherPosition.YCoordinate, 2));
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
            LogicAPI.CreateSpecifiedNumberOfBalls(10);
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
            LogicAPI.CreateSpecifiedNumberOfBalls(1);
            LogicAPI.StartBallMovement();
            List<List<double>> originalListOfBallsCoordinatesNo1 = LogicAPI.GetAllBallsCoordinates();
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
            LogicAPI.CreateSpecifiedNumberOfBalls(10);
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
            LogicAPI.CreateSpecifiedNumberOfBalls(NumberOfBallsToCreate);
            List<List<double>> listOfBallsCoordinates = LogicAPI.GetAllBallsCoordinates();
            Assert.AreEqual(NumberOfBallsToCreate, listOfBallsCoordinates.Count);
        }
    }
}