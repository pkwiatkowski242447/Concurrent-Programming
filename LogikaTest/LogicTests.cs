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
            private Random randomNumber = new Random();
            private DataBallSerializer? serializer = null;

            public override void CreateSerializerObject()
            {
                serializer = null;
            }

            public override DataBallInterface CreateASingleBall(int idOfTheBall, double radiusOfTheBall)
            {
                double XCoordinate = randomNumber.NextDouble() * (WidthOfTheBoard - 2 * radiusOfTheBall);
                double YCoordinate = randomNumber.NextDouble() * (HeightOfTheBoard - 2 * radiusOfTheBall);
                DataPositionInterface CenterOfTheBall = new Position(XCoordinate, YCoordinate);
                XCoordinate = randomNumber.NextDouble() * 10 - 5;
                YCoordinate = randomNumber.NextDouble() * 10 - 5;
                DataPositionInterface VelocityVectorOfTheBall = new Position(XCoordinate, YCoordinate);
                return new Ball(idOfTheBall, CenterOfTheBall, VelocityVectorOfTheBall, serializer);
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

            public override int GetWidthOfTheBoard()
            {
                return this.WidthOfTheBoard;
            }
        }

        internal class Ball : DataBallInterface
        {
            public override int IdOfTheBall { get; }
            public override DataPositionInterface CenterOfTheBall { get => ActualCenterOfTheBall; }
            public override DataPositionInterface VelocityVectorOfTheBall { get; set; }
            public override bool DidBallCollide { get; set; }
            public override bool StartBallMovement { get; set; }
            public override double TimeToWait { get; set; }
            public override CancellationTokenSource CancelDelay { get; set; }

            private IObserver<DataBallInterface>? ObserverObject;
            private DataPositionInterface ActualCenterOfTheBall;
            private DataBallSerializer? SerializerObject;
            private bool StopTask = false;
            private int BaseWaitTime = 5;

            public Ball(int id, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall, DataBallSerializer? serializer)
            {
                this.IdOfTheBall = id;
                this.ActualCenterOfTheBall = centerOfTheBall;
                this.VelocityVectorOfTheBall = velocityVectorOfTheBall;
                this.DidBallCollide = false;
                this.SerializerObject = serializer;
                this.CancelDelay = new CancellationTokenSource();
                Task.Run(BallMovement);
            }

            private async void BallMovement()
            {
                while (!this.StopTask)
                {
                    if (this.StartBallMovement)
                    {
                        this.TimeToWait = (double)(this.BaseWaitTime / this.VelocityVectorOfTheBall.VectorLength());
                        if (this.TimeToWait > 10)
                        {
                            this.TimeToWait = 10;
                        }

                        this.Move();
                        if (this.SerializerObject != null)
                        {
                            SerializerObject.AddDataBallToSerializationQueue(this);
                        }
                        if (this.ObserverObject != null)
                        {
                            this.ObserverObject.OnNext(this);
                        }
                        this.DidBallCollide = false;
                        await Task.Delay((int)this.TimeToWait, CancelDelay.Token).ContinueWith(_ => { });
                        if (CancelDelay.IsCancellationRequested)
                        {
                            CancelDelay.Dispose();
                            CancelDelay = new CancellationTokenSource();
                        }
                    }
                }
            }

            public void Move()
            {
                double XCoordinate = this.CenterOfTheBall.XCoordinate + this.VelocityVectorOfTheBall.XCoordinate;
                double YCoordinate = this.CenterOfTheBall.YCoordinate + this.VelocityVectorOfTheBall.YCoordinate;
                DataPositionInterface NewCenterOfTheBall = DataPositionInterface.CreatePosition(XCoordinate, YCoordinate);
                this.SetCenterOfTheBall(NewCenterOfTheBall);
            }

            public override void Dispose()
            {
                this.StopTask = true;
            }

            private void SetCenterOfTheBall(DataPositionInterface someOtherPosition)
            {
                this.ActualCenterOfTheBall = someOtherPosition;
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
            public override double XCoordinate { get; }
            public override double YCoordinate { get; }

            public Position(double xCoordinate, double yCoordinate)
            {
                this.XCoordinate = xCoordinate;
                this.YCoordinate = yCoordinate;
            }

            public override double VectorLength()
            {
                return Math.Sqrt(Math.Pow(this.XCoordinate, 2) + Math.Pow(this.YCoordinate, 2));
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
            List<LogicBallInterface> listOfBallsCoordinates = LogicAPI.GetListOfAllLogicBalls();
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
        public void ClearPoolTableTest()
        {
            FakeDataAPI FakeAPI = new FakeDataAPI();
            LogicAbstractAPI LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance(FakeAPI);
            LogicAPI.CreatePlayingBoard();
            LogicAPI.CreateSpecifiedNumberOfBalls(10);
            List<LogicBallInterface> listOfBalls = LogicAPI.GetListOfAllLogicBalls();
            Assert.AreEqual(10, listOfBalls.Count);
            LogicAPI.ClearPoolTable();
            listOfBalls = LogicAPI.GetListOfAllLogicBalls();
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
            List<LogicBallInterface> listOfLogicBalls = LogicAPI.GetListOfAllLogicBalls();
            Assert.AreEqual(NumberOfBallsToCreate, listOfLogicBalls.Count);
        }

        // LogicPosition tests

        [TestMethod]
        public void LogicPositionFacotryTest()
        {
            double XCoordinate = 127;
            double YCoordinate = 421;

            LogicPositionInterface Position = LogicPositionInterface.CreateLogicPosition(XCoordinate, YCoordinate);

            Assert.AreEqual(XCoordinate, Position.XCoordinate);
            Assert.AreEqual(YCoordinate, Position.YCoordinate);
        }

        [TestMethod]
        public void LogicPositionAdditionMethodTest()
        {
            double XCoordinateFirst = 123;
            double XCoordinateSecond = 678;
            double YCoordinateFirst = 321;
            double YCoordinateSecond = 896;
            LogicPositionInterface PositionNumber1 = LogicPositionInterface.CreateLogicPosition(XCoordinateFirst, YCoordinateFirst);
            LogicPositionInterface PositionNumber2 = LogicPositionInterface.CreateLogicPosition(XCoordinateSecond, YCoordinateSecond);
            LogicPositionInterface ResultPosition = PositionNumber1.Addition(PositionNumber2);

            Assert.AreEqual(XCoordinateFirst + XCoordinateSecond, ResultPosition.XCoordinate);
            Assert.AreEqual(YCoordinateFirst + YCoordinateSecond, ResultPosition.YCoordinate);
        }

        [TestMethod]
        public void LogicPositionMultiplicationMethodTest()
        {
            double someDouble = 2.1;
            double XCoordinate = 123;
            double YCoordinate = 678;
            LogicPositionInterface Position = LogicPositionInterface.CreateLogicPosition(XCoordinate, YCoordinate);
            LogicPositionInterface ResultPosition = Position.Multiplication(someDouble);

            Assert.AreEqual(XCoordinate * someDouble, ResultPosition.XCoordinate);
            Assert.AreEqual(YCoordinate * someDouble, ResultPosition.YCoordinate);
        }

        [TestMethod]
        public void LogicPositionSubtractionMethodTest()
        {
            double XCoordinateFirst = 123;
            double XCoordinateSecond = 678;
            double YCoordinateFirst = 321;
            double YCoordinateSecond = 896;
            LogicPositionInterface PositionNumber1 = LogicPositionInterface.CreateLogicPosition(XCoordinateFirst, YCoordinateFirst);
            LogicPositionInterface PositionNumber2 = LogicPositionInterface.CreateLogicPosition(XCoordinateSecond, YCoordinateSecond);
            LogicPositionInterface ResultPosition = PositionNumber1.Subtraction(PositionNumber2);

            Assert.AreEqual(XCoordinateFirst - XCoordinateSecond, ResultPosition.XCoordinate);
            Assert.AreEqual(YCoordinateFirst - YCoordinateSecond, ResultPosition.YCoordinate);
        }

        [TestMethod]
        public void LogicPositionDotOperationMethodTest()
        {
            double XCoordinateFirst = 123;
            double XCoordinateSecond = 678;
            double YCoordinateFirst = 321;
            double YCoordinateSecond = 896;
            LogicPositionInterface PositionNumber1 = LogicPositionInterface.CreateLogicPosition(XCoordinateFirst, YCoordinateFirst);
            LogicPositionInterface PositionNumber2 = LogicPositionInterface.CreateLogicPosition(XCoordinateSecond, YCoordinateSecond);
            double ResultOfDotOperation = PositionNumber1.DotOperator(PositionNumber2);
            double ExpectedResult = XCoordinateFirst * XCoordinateSecond + YCoordinateFirst * YCoordinateSecond;

            Assert.AreEqual(ExpectedResult, ResultOfDotOperation);
        }

        [TestMethod]
        public void LogicPositionVectorLengthMethodTest()
        {
            double XCoordinate = 123;
            double YCoordinate = 678;
            LogicPositionInterface Position = LogicPositionInterface.CreateLogicPosition(XCoordinate, YCoordinate);
            double ActualVectorLength = Position.VectorLength();
            double ExpectedVectorLength = Math.Sqrt(Math.Pow(XCoordinate, 2) + Math.Pow(YCoordinate, 2));

            Assert.AreEqual(ExpectedVectorLength, ActualVectorLength);
        }

        [TestMethod]
        public void LogicPositionEuclideanDistanceMethodTest()
        {
            double XCoordinateFirst = 123;
            double XCoordinateSecond = 678;
            double YCoordinateFirst = 321;
            double YCoordinateSecond = 896;
            LogicPositionInterface PositionNumber1 = LogicPositionInterface.CreateLogicPosition(XCoordinateFirst, YCoordinateFirst);
            LogicPositionInterface PositionNumber2 = LogicPositionInterface.CreateLogicPosition(XCoordinateSecond, YCoordinateSecond);
            double EuclideanDistance = PositionNumber1.EuclideanDistance(PositionNumber2);
            double ExpectedEuclideanDistance = Math.Sqrt(Math.Pow(PositionNumber1.XCoordinate - PositionNumber2.XCoordinate, 2) + Math.Pow(PositionNumber1.YCoordinate - PositionNumber2.YCoordinate, 2));

            Assert.AreEqual(ExpectedEuclideanDistance, EuclideanDistance);
        }
    }
}