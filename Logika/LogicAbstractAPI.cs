using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public abstract class LogicAbstractAPI : IObserver<DataBallInterface>, IObservable<int>
    {
        public static LogicAbstractAPI CreateLogicAPIInstance(DataAbstractAPI? DataAPI = default)
        {
            return new LogicAPI(DataAPI == null ? DataAbstractAPI.CreateDataAPIInstance() : DataAPI);
        }

        public abstract void CreatePlayingBoard();
        public abstract void CreateSpecifiedNumberOfBalls(int numberOfBallsToAdd);
        public abstract void ClearPoolTable();
        public abstract void MoveGeneratedBalls();
        public abstract void OnNext(DataBallInterface currentBall);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract List<List<double>> GetAllBallsCoordinates();
        public abstract IDisposable Subscribe(IObserver<int> observer);

        private class LogicAPI : LogicAbstractAPI
        {
            internal DataAbstractAPI DataAPI;
            internal IObserver<int>? ObserverObject;
            internal int WidthOfTheBoard = 740;
            internal int HeightOfTheBoard = 690;
            internal double RadiusOfTheBall = 10.0;
            internal List<IDisposable>? ListOfDataBallObservers;
            internal List<DataBallInterface> ListOfManagedDataBalls { get; set; }
            internal object LockObject = new object();

            public LogicAPI(DataAbstractAPI DataAPI)
            {
                this.DataAPI = DataAPI;
                ListOfManagedDataBalls = new List<DataBallInterface>();
                ListOfDataBallObservers = new List<IDisposable>();
            }

            public override void CreatePlayingBoard()
            {
                DataAPI.CreateBoard(this.WidthOfTheBoard, this.HeightOfTheBoard);
            }

            public override void CreateSpecifiedNumberOfBalls(int numberOfBallsToAdd)
            {
                for (int i = 0; i < numberOfBallsToAdd; i++)
                {
                    DataBallInterface currentBall = DataAPI.CreateASingleBall(this.RadiusOfTheBall);
                    ListOfManagedDataBalls.Add(currentBall);
                }
                foreach (DataBallInterface DataBall in ListOfManagedDataBalls)
                {
                    IDisposable BallObserver = DataBall.Subscribe(this);
                    ListOfDataBallObservers?.Add(BallObserver);
                }
            }

            public override void ClearPoolTable()
            {
                foreach (DataBallInterface DataBall in ListOfManagedDataBalls)
                {
                    DataBall.StopTask = true;
                }
                ListOfManagedDataBalls.Clear();
            }
            public override void MoveGeneratedBalls()
            {
                for (int i = 0; i < ListOfManagedDataBalls.Count; i++)
                {
                    ListOfManagedDataBalls[i].StartMovement = true;
                }
            }
            public override List<List<double>> GetAllBallsCoordinates()
            {
                List<List<double>> ListOfBallsCoordinates = new List<List<double>>();
                for (int i = 0; i < ListOfManagedDataBalls.Count; i++)
                {
                    double XCoordinate = ListOfManagedDataBalls[i].CenterOfTheBall.XCoordinate;
                    double YCoordinate = ListOfManagedDataBalls[i].CenterOfTheBall.YCoordinate;
                    double BallRadius = this.RadiusOfTheBall;
                    double VelocityX = ListOfManagedDataBalls[i].VelocityVectorOfTheBall.XCoordinate;
                    double VelocityY = ListOfManagedDataBalls[i].VelocityVectorOfTheBall.YCoordinate;

                    List<double> Coords = new List<double>()
                    {
                        XCoordinate,
                        YCoordinate,
                        BallRadius,
                        VelocityX,
                        VelocityY
                    };
                    ListOfBallsCoordinates.Add(Coords);
                }
                return ListOfBallsCoordinates;
            }
            public override IDisposable Subscribe(IObserver<int> observerObject)
            {
                this.ObserverObject = observerObject;
                return new ObserverManager(observerObject);
            }

            private void ManageCollisionsWithWalls(DataBallInterface SomeBall)
            {
                if (0 >= (SomeBall.CenterOfTheBall.XCoordinate - this.RadiusOfTheBall) ||
                    (SomeBall.CenterOfTheBall.XCoordinate + this.RadiusOfTheBall) >= this.WidthOfTheBoard)
                {
                    SomeBall.VelocityVectorOfTheBall.XCoordinate = -SomeBall.VelocityVectorOfTheBall.XCoordinate;
                }

                if (0 >= (SomeBall.CenterOfTheBall.YCoordinate - this.RadiusOfTheBall) ||
                    (SomeBall.CenterOfTheBall.YCoordinate + this.RadiusOfTheBall) >= this.HeightOfTheBoard)
                {
                    SomeBall.VelocityVectorOfTheBall.YCoordinate = -SomeBall.VelocityVectorOfTheBall.YCoordinate;
                }
            }

            private void ManageCollisionsWithOtherBalls(DataBallInterface dataBall)
            {
                List<DataBallInterface> ListOfCollidingBalls = new List<DataBallInterface>();
                double distanceBetweenTheseBalls;
                for (int i = 0; i < ListOfManagedDataBalls.Count; i++)
                {
                    distanceBetweenTheseBalls = dataBall.CenterOfTheBall.EuclideanDistance(ListOfManagedDataBalls[i].CenterOfTheBall);
                    DataPositionInterface NextLogicBallPosition = dataBall.CenterOfTheBall.Addition(dataBall.VelocityVectorOfTheBall);
                    DataPositionInterface NextPositionOfOtherBall = ListOfManagedDataBalls[i].CenterOfTheBall.Addition(ListOfManagedDataBalls[i].VelocityVectorOfTheBall);
                    if (ListOfManagedDataBalls[i] != dataBall && distanceBetweenTheseBalls <= 2 * this.RadiusOfTheBall &&
                        distanceBetweenTheseBalls - NextLogicBallPosition.EuclideanDistance(NextPositionOfOtherBall) > 0)
                    {
                        ListOfCollidingBalls.Add(ListOfManagedDataBalls[i]);
                    }
                }

                foreach (DataBallInterface collidingBall in ListOfCollidingBalls)
                {
                    DataPositionInterface DiffBetweenCenters = dataBall.CenterOfTheBall.Subtraction(collidingBall.CenterOfTheBall);
                    DataPositionInterface DiffBetweenVelocities = dataBall.VelocityVectorOfTheBall.Subtraction(collidingBall.VelocityVectorOfTheBall);

                    DataPositionInterface secondPart = DiffBetweenCenters.Multiplication(DiffBetweenVelocities.DotOperator(DiffBetweenCenters) / Math.Pow(DiffBetweenCenters.VectorLength(), 2));
                    DataPositionInterface newVelocityVectorForLogicBall = dataBall.VelocityVectorOfTheBall.Subtraction(secondPart.Multiplication(2f * collidingBall.MassOfTheBall / (dataBall.MassOfTheBall + collidingBall.MassOfTheBall)));
                    DiffBetweenCenters = collidingBall.CenterOfTheBall.Subtraction(dataBall.CenterOfTheBall);
                    DiffBetweenVelocities = collidingBall.VelocityVectorOfTheBall.Subtraction(dataBall.VelocityVectorOfTheBall);
                    secondPart = DiffBetweenCenters.Multiplication(DiffBetweenVelocities.DotOperator(DiffBetweenCenters) / Math.Pow(DiffBetweenCenters.VectorLength(), 2));

                    DataPositionInterface newVelocityVectorForCollidingBall = collidingBall.VelocityVectorOfTheBall.Subtraction(secondPart.Multiplication(2f * dataBall.MassOfTheBall / (dataBall.MassOfTheBall + collidingBall.MassOfTheBall)));

                    dataBall.VelocityVectorOfTheBall = newVelocityVectorForLogicBall;
                    collidingBall.VelocityVectorOfTheBall = newVelocityVectorForCollidingBall;

                    dataBall.DidBallCollide = true;
                    collidingBall.DidBallCollide = true;
                }
            }

            public override void OnNext(DataBallInterface currentBall)
            {
                int index = ListOfManagedDataBalls.IndexOf(currentBall);
                lock (LockObject)
                {
                    if (!currentBall.DidBallCollide)
                    {
                        ManageCollisionsWithWalls(ListOfManagedDataBalls[index]);
                        ManageCollisionsWithOtherBalls(ListOfManagedDataBalls[index]);
                    }
                    this.RepairCoordinates(currentBall);
                }
                if (this.ObserverObject != null)
                {
                    this.ObserverObject.OnNext(index);
                }
            }

            public override void OnCompleted()
            {
                if (ListOfDataBallObservers != null)
                {
                    foreach (IDisposable ObserverObject in ListOfDataBallObservers)
                    {
                        ObserverObject.Dispose();
                    }
                }
            }

            public override void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            private class ObserverManager : IDisposable
            {
                IObserver<int>? SomeObserver;

                public ObserverManager(IObserver<int> observer)
                {
                    this.SomeObserver = observer;
                }

                public void Dispose()
                {
                    this.SomeObserver = null;
                }
            }
            private void RepairCoordinates(DataBallInterface dataBall)
            {
                if (dataBall.CenterOfTheBall.XCoordinate > WidthOfTheBoard - this.RadiusOfTheBall)
                {
                    dataBall.CenterOfTheBall.XCoordinate = WidthOfTheBoard - this.RadiusOfTheBall;
                }
                else if (dataBall.CenterOfTheBall.XCoordinate < this.RadiusOfTheBall)
                {
                    dataBall.CenterOfTheBall.XCoordinate = this.RadiusOfTheBall;
                }

                if (dataBall.CenterOfTheBall.YCoordinate > HeightOfTheBoard - this.RadiusOfTheBall)
                {
                    dataBall.CenterOfTheBall.YCoordinate = HeightOfTheBoard - this.RadiusOfTheBall;
                }
                else if (dataBall.CenterOfTheBall.YCoordinate < this.RadiusOfTheBall)
                {
                    dataBall.CenterOfTheBall.YCoordinate = this.RadiusOfTheBall;
                }
            }
        }
    }
}