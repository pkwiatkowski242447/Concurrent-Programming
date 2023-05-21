using System;
using System.Collections.Generic;
using System.Threading;
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
        public abstract void StartBallMovement();
        public abstract LogicBallInterface GetCertainLogicBall(int index);
        public abstract List<LogicBallInterface> GetListOfAllLogicBalls();
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(DataBallInterface dataBall);
        public abstract IDisposable Subscribe(IObserver<int> observer);

        private class LogicAPI : LogicAbstractAPI
        {
            internal DataAbstractAPI DataAPI;
            internal int WidthOfTheBoard = 740;
            internal int HeightOfTheBoard = 690;
            internal double RadiusOfTheBall = 10.0;
            internal List<IDisposable>? ListOfDataBallObservers;
            internal IObserver<int>? ObserverObject;
            internal List<DataBallInterface> ListOfManagedDataBalls { get; set; }
            internal List<LogicBallInterface> ListOfManagedLogicBalls { get; set; }
            internal object LockObject = new object();

            internal LogicAPI(DataAbstractAPI DataAPI)
            {
                this.DataAPI = DataAPI;
                ListOfManagedDataBalls = new List<DataBallInterface>();
                ListOfManagedLogicBalls = new List<LogicBallInterface>();
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
                    LogicBallInterface newLogicBall = LogicBallInterface.CreateLogicBall(currentBall, this.RadiusOfTheBall);
                    ListOfManagedLogicBalls.Add(newLogicBall);
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
                if (ListOfDataBallObservers != null)
                {
                    foreach (IDisposable BallObserver in ListOfDataBallObservers)
                    {
                        BallObserver.Dispose();
                    }
                    ListOfDataBallObservers.Clear();
                }
                ListOfManagedLogicBalls.Clear();
                ListOfManagedDataBalls.Clear();
            }

            public override void StartBallMovement()
            {
                foreach (DataBallInterface DataBall in ListOfManagedDataBalls)
                {
                    DataBall.StartBallMovement = true;
                }
            }

            public override LogicBallInterface GetCertainLogicBall(int index)
            {
                return ListOfManagedLogicBalls[index];
            }

            public override List<LogicBallInterface> GetListOfAllLogicBalls()
            {
                return ListOfManagedLogicBalls;
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

            public override void OnNext(DataBallInterface dataBall)
            {
                int index = ListOfManagedDataBalls.IndexOf(dataBall);

                try
                {
                    Monitor.Enter(LockObject);

                    LogicBallInterface logicBall = ListOfManagedLogicBalls[index];
                    logicBall.UpdateLogicBall(dataBall);

                    if (!dataBall.DidBallCollide)
                    {
                        ManageCollisionsWithOtherBalls(index);
                    }
                }
                finally
                {
                    Monitor.Exit(LockObject);
                }

                ManageCollisionsWithWalls(index);

                if (this.ObserverObject != null)
                {
                    this.ObserverObject.OnNext(index);
                }
            }

            private void ManageCollisionsWithWalls(int index)
            {
                LogicBallInterface logicBall = ListOfManagedLogicBalls[index];

                double XCoordinateVelocity = logicBall.BallVelocity.XCoordinate;
                double YCoordinateVelocity = logicBall.BallVelocity.YCoordinate;

                if (logicBall.BallCenter.XCoordinate - this.RadiusOfTheBall <= 0 && XCoordinateVelocity <= 0 ||
                    logicBall.BallCenter.XCoordinate + this.RadiusOfTheBall >= this.WidthOfTheBoard && XCoordinateVelocity >= 0)
                {
                    XCoordinateVelocity = -XCoordinateVelocity;
                }
                
                if(logicBall.BallCenter.YCoordinate - this.RadiusOfTheBall <= 0 && YCoordinateVelocity <= 0 ||
                   logicBall.BallCenter.YCoordinate + this.RadiusOfTheBall >= this.HeightOfTheBoard && YCoordinateVelocity >= 0)
                {
                    YCoordinateVelocity = -YCoordinateVelocity;
                }

                logicBall.BallVelocity = LogicPositionInterface.CreateLogicPosition(XCoordinateVelocity, YCoordinateVelocity);
                ListOfManagedDataBalls[index].VelocityVectorOfTheBall = DataPositionInterface.CreatePosition(XCoordinateVelocity, YCoordinateVelocity);
            }

            private void ManageCollisionsWithOtherBalls(int index)
            {
                LogicBallInterface logicBall = ListOfManagedLogicBalls[index];

                List<LogicBallInterface> ListOfCollidingBalls = new List<LogicBallInterface>();
                double distanceBetweenTheseBalls;
                for (int i = 0; i < ListOfManagedDataBalls.Count; i++)
                {
                    distanceBetweenTheseBalls = logicBall.BallCenter.EuclideanDistance(ListOfManagedLogicBalls[i].BallCenter);
                    LogicPositionInterface NextLogicBallPosition = logicBall.BallCenter.Addition(logicBall.BallVelocity);
                    LogicPositionInterface NextPositionOfOtherBall = ListOfManagedLogicBalls[i].BallCenter.Addition(ListOfManagedLogicBalls[i].BallVelocity);
                    if (ListOfManagedLogicBalls[i] != logicBall && distanceBetweenTheseBalls <= logicBall.BallRadius + ListOfManagedLogicBalls[i].BallRadius &&
                        distanceBetweenTheseBalls - NextLogicBallPosition.EuclideanDistance(NextPositionOfOtherBall) > 0)
                    {
                        ListOfCollidingBalls.Add(ListOfManagedLogicBalls[i]);
                    }
                }

                foreach (LogicBallInterface collidingBall in ListOfCollidingBalls)
                {
                    LogicPositionInterface DiffBetweenCenters = logicBall.BallCenter.Subtraction(collidingBall.BallCenter);
                    LogicPositionInterface DiffBetweenVelocities = logicBall.BallVelocity.Subtraction(collidingBall.BallVelocity);

                    LogicPositionInterface secondPart = DiffBetweenCenters.Multiplication(DiffBetweenVelocities.DotOperator(DiffBetweenCenters) / Math.Pow(DiffBetweenCenters.VectorLength(), 2));
                    LogicPositionInterface newVelocityVectorForLogicBall = logicBall.BallVelocity.Subtraction(secondPart.Multiplication(2f * collidingBall.BallMass / (logicBall.BallMass + collidingBall.BallMass)));
                    DiffBetweenCenters = collidingBall.BallCenter.Subtraction(logicBall.BallCenter);
                    DiffBetweenVelocities = collidingBall.BallVelocity.Subtraction(logicBall.BallVelocity);
                    secondPart = DiffBetweenCenters.Multiplication(DiffBetweenVelocities.DotOperator(DiffBetweenCenters) / Math.Pow(DiffBetweenCenters.VectorLength(), 2));

                    LogicPositionInterface newVelocityVectorForCollidingBall = collidingBall.BallVelocity.Subtraction(secondPart.Multiplication(2f * logicBall.BallMass / (logicBall.BallMass + collidingBall.BallMass)));

                    // Updating ballVelocity in LogicBalls

                    logicBall.BallVelocity = newVelocityVectorForLogicBall;
                    collidingBall.BallVelocity = newVelocityVectorForCollidingBall;

                    // Converting logic velocity vectors to one used in Data Layer

                    DataPositionInterface dataBallNewVelocity = DataPositionInterface.CreatePosition(newVelocityVectorForLogicBall.XCoordinate, newVelocityVectorForLogicBall.YCoordinate);
                    DataPositionInterface collidingBallNewVelocity = DataPositionInterface.CreatePosition(newVelocityVectorForCollidingBall.XCoordinate, newVelocityVectorForCollidingBall.YCoordinate);

                    // Getting both balls from Data (that is from the ListOfManagedDataBalls)

                    DataBallInterface dataBall = ListOfManagedDataBalls[index];
                    DataBallInterface collidingBallFromData = ListOfManagedDataBalls[ListOfManagedLogicBalls.IndexOf(collidingBall)];

                    // Updating VelocityOfTheBall in balls from Data Layer

                    dataBall.VelocityVectorOfTheBall = dataBallNewVelocity;
                    collidingBallFromData.VelocityVectorOfTheBall = collidingBallNewVelocity;

                    // Marking that the collision between balls occured

                    dataBall.DidBallCollide = true;
                    collidingBallFromData.DidBallCollide = true;
                }
            }

            public override IDisposable Subscribe(IObserver<int> observerObject)
            {
                this.ObserverObject = observerObject;
                return new ObserverManager(observerObject);
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

        }
    }
}