
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public abstract class LogicAbstractAPI : IObservable<int>
    {
        public static LogicAbstractAPI CreateLogicApi()
        {
            return new LogicAPI();
        }
        public abstract void CreateSpecifiedNumerOfBalls(int numberOfBallsToAdd);
        public abstract void ClearPoolTable();
        public abstract void MoveBalls();
        public abstract List<List<double>> GetAllBallsCoordinates();
        public abstract IDisposable Subscribe(IObserver<int> observer);

        private class LogicAPI : LogicAbstractAPI
        {
            internal bool stopTasks = true;
            internal DataAbstractAPI DataAPI;
            internal int WidthOfTheTable = 740;
            internal int HeightOfTheTable = 690;
            internal Object LockObject = new object();
            internal IObserver<int>? ObserverObject;
            internal List<Ball> ListOfManagedBalls { get; set; }
            internal List<Task> ListOfManagedTasks { get; set; }

            public LogicAPI()
            {
                DataAPI = DataAbstractAPI.CreateDataApi(HeightOfTheTable, WidthOfTheTable);
                ListOfManagedBalls = new List<Ball>();
                ListOfManagedTasks = new List<Task>();
            }

            public override void CreateSpecifiedNumerOfBalls(int numberOfBallsToAdd)
            {
                for (int i = 0; i < numberOfBallsToAdd; i++)
                {
                    int taskIdentfier = i;
                    Ball currentBall = DataAPI.CreateBall();
                    ListOfManagedBalls.Add(currentBall);
                    Task ballMovement = new Task(() =>
                    {
                        while (!stopTasks)
                        {
                            ManageCollisions(currentBall);
                            currentBall.MoveBall();
                            if (ObserverObject != null)
                            {
                                ObserverObject.OnNext(taskIdentfier);
                            }
                            Task.Delay(10).Wait();
                        }
                    });
                    ListOfManagedTasks.Add(ballMovement);
                }
            }

            public override void ClearPoolTable()
            {
                stopTasks = true;
                bool isEveryTaskStopped = false;
                while (!isEveryTaskStopped)
                {
                    isEveryTaskStopped = true;
                    foreach (Task task in ListOfManagedTasks)
                    {
                        if (!task.IsCompleted)
                        {
                            isEveryTaskStopped = false;
                            break;
                        }
                    }
                }
                for (int i = 0; i < ListOfManagedTasks.Count; i++)
                {
                    ListOfManagedTasks[i].Dispose();
                }
                ListOfManagedTasks.Clear();
                ListOfManagedBalls.Clear();
            }
            public override void MoveBalls()
            {
                stopTasks = false;
                for (int i = 0; i < ListOfManagedTasks.Count; i++)
                {
                    ListOfManagedTasks[i].Start();
                }
            }

            public override List<List<double>> GetAllBallsCoordinates()
            {
                List<List<double>> ListOfBallsCoordinates = new List<List<double>>();
                for (int i = 0; i < ListOfManagedBalls.Count; i++)
                {
                    double XCoordinate = ListOfManagedBalls[i].CenterOfTheBall.XCoordinate;
                    double YCoordinate = ListOfManagedBalls[i].CenterOfTheBall.YCoordinate;
                    double BallRadius = ListOfManagedBalls[i].BallRadius;
                    double VelocityX = ListOfManagedBalls[i].VelocityVector.XCoordinate;
                    double VelocityY = ListOfManagedBalls[i].VelocityVector.YCoordinate;
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
            public override IDisposable Subscribe(IObserver<int> observer)
            {
                this.ObserverObject = observer;
                return new ObserverManager(observer);
            }

            private void ManageCollisions(Ball SomeBall)
            {
                lock (LockObject)
                {
                    List<Ball> ListOfCollidingBalls = new List<Ball>();
                    double distanceBetweenTheseBalls = 0;
                    for (int i = 0; i < ListOfManagedBalls.Count; i++)
                    {
                        distanceBetweenTheseBalls = CalculateEuclideanDistanceBetweenTwoPositions(SomeBall.CenterOfTheBall, ListOfManagedBalls[i].CenterOfTheBall);
                        if (ListOfManagedBalls[i] != SomeBall && distanceBetweenTheseBalls <= SomeBall.BallRadius + ListOfManagedBalls[i].BallRadius &&
                            (CalculateEuclideanDistanceBetweenTwoPositions(SomeBall.CenterOfTheBall, ListOfManagedBalls[i].CenterOfTheBall) -
                            CalculateEuclideanDistanceBetweenTwoPositions(SomeBall.CenterOfTheBall + SomeBall.VelocityVector, ListOfManagedBalls[i].CenterOfTheBall + ListOfManagedBalls[i].VelocityVector) > 0))
                        {
                            ListOfCollidingBalls.Add(ListOfManagedBalls[i]);
                        }
                    }

                    foreach (Ball collidingBall in ListOfCollidingBalls)
                    {
                        Position DiffBetweenCenters = SomeBall.CenterOfTheBall - collidingBall.CenterOfTheBall;
                        Position DiffBetweenVelocities = SomeBall.VelocityVector - collidingBall.VelocityVector;
                        Position secondPart = Position.DotOperator(DiffBetweenVelocities, DiffBetweenCenters) / Math.Pow(Position.VectorLength(DiffBetweenCenters), 2) * DiffBetweenCenters;
                        Position newVelocityVectorForSomeBall = SomeBall.VelocityVector - (2f * collidingBall.MassOfTheBall / (SomeBall.MassOfTheBall + collidingBall.MassOfTheBall) * secondPart);
                        DiffBetweenCenters = collidingBall.CenterOfTheBall - SomeBall.CenterOfTheBall;
                        DiffBetweenVelocities = collidingBall.VelocityVector - SomeBall.VelocityVector;
                        secondPart = Position.DotOperator(DiffBetweenVelocities, DiffBetweenCenters) / Math.Pow(Position.VectorLength(DiffBetweenCenters), 2) * DiffBetweenCenters;
                        Position newVelocityVectorForCollidingBall = collidingBall.VelocityVector - (2f * SomeBall.MassOfTheBall / (SomeBall.MassOfTheBall + collidingBall.MassOfTheBall) * secondPart);

                        SomeBall.VelocityVector = newVelocityVectorForSomeBall;
                        collidingBall.VelocityVector = newVelocityVectorForCollidingBall;
                    }
                }

                // Repairing ball coordinates since collisions can make them incorrect
                if (SomeBall.CenterOfTheBall.XCoordinate > WidthOfTheTable - SomeBall.BallRadius)
                {
                    SomeBall.CenterOfTheBall.XCoordinate = WidthOfTheTable - SomeBall.BallRadius;
                }
                else if (SomeBall.CenterOfTheBall.XCoordinate < SomeBall.BallRadius)
                {
                    SomeBall.CenterOfTheBall.XCoordinate = SomeBall.BallRadius;
                }

                if (SomeBall.CenterOfTheBall.YCoordinate > HeightOfTheTable - SomeBall.BallRadius)
                {
                    SomeBall.CenterOfTheBall.YCoordinate = HeightOfTheTable - SomeBall.BallRadius;
                }
                else if (SomeBall.CenterOfTheBall.YCoordinate < SomeBall.BallRadius)
                {
                    SomeBall.CenterOfTheBall.YCoordinate = SomeBall.BallRadius;
                }

                // Collisions with vertical walls.
                if (0 >= (SomeBall.CenterOfTheBall.XCoordinate - SomeBall.BallRadius) ||
                (SomeBall.CenterOfTheBall.XCoordinate + SomeBall.BallRadius) >= WidthOfTheTable)
                {
                    SomeBall.VelocityVector.XCoordinate = -SomeBall.VelocityVector.XCoordinate;
                }

                // Collisions with horizontal walls.
                if (0 >= (SomeBall.CenterOfTheBall.YCoordinate - SomeBall.BallRadius) ||
                    (SomeBall.CenterOfTheBall.YCoordinate + SomeBall.BallRadius) >= HeightOfTheTable)
                {
                    SomeBall.VelocityVector.YCoordinate = -SomeBall.VelocityVector.YCoordinate;
                }
            }

            private double CalculateEuclideanDistanceBetweenTwoPositions(Position posNo1, Position posNo2)
            {
                return Math.Sqrt(Math.Pow(posNo1.XCoordinate - posNo2.XCoordinate, 2) +
                Math.Pow(posNo1.YCoordinate - posNo2.YCoordinate, 2));
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