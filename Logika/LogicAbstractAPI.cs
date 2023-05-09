using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public abstract class LogicAbstractAPI : IObservable<int>
    {
        public static LogicAbstractAPI CreateLogicAPIInstance(DataAbstractAPI? DataAPI = default)
        {
            return new LogicAPI(DataAPI == null ? DataAbstractAPI.CreateDataAPIInstance() : DataAPI);
        }

        public abstract void CreatePlayingBoard();
        public abstract void CreateSpecifiedNumerOfBalls(int numberOfBallsToAdd);
        public abstract void ClearPoolTable();
        public abstract void MoveGeneratedBalls();
        public abstract List<List<double>> GetAllBallsCoordinates();
        public abstract IDisposable Subscribe(IObserver<int> observer);

        private class LogicAPI : LogicAbstractAPI
        {
            internal bool stopTasks = true;
            internal DataAbstractAPI DataAPI;
            internal IObserver<int>? ObserverObject;
            internal int WidthOfTheBoard = 740;
            internal int HeightOfTheBoard = 690;
            internal List<DataBallInterface> ListOfManagedBalls { get; set; }
            internal List<Task> ListOfManagedTasks { get; set; }

            public LogicAPI(DataAbstractAPI DataAPI)
            {
                this.DataAPI = DataAPI;
                ListOfManagedBalls = new List<DataBallInterface>();
                ListOfManagedTasks = new List<Task>();
            }

            public override void CreatePlayingBoard()
            {
                DataAPI.CreateBoard(this.WidthOfTheBoard, this.HeightOfTheBoard);
            }

            public override void CreateSpecifiedNumerOfBalls(int numberOfBallsToAdd)
            {
                for (int i = 0; i < numberOfBallsToAdd; i++)
                {
                    int taskIdentfier = i;
                    DataBallInterface currentBall = DataAPI.CreateASingleBall();
                    ListOfManagedBalls.Add(currentBall);
                    Task ballMovement = new Task(() =>
                    {
                        while (!stopTasks)
                        {
                            ManageCollisions(currentBall);
                            currentBall.Move();
                            if (ObserverObject != null)
                            {
                                ObserverObject.OnNext(taskIdentfier);
                            }
                            Task.Delay(1).Wait();
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
            public override void MoveGeneratedBalls()
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
                    double BallRadius = ListOfManagedBalls[i].RadiusOfTheBall;
                    double VelocityX = ListOfManagedBalls[i].VelocityVectorOfTheBall.XCoordinate;
                    double VelocityY = ListOfManagedBalls[i].VelocityVectorOfTheBall.YCoordinate;

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
            private void ManageCollisions(DataBallInterface SomeBall)
            {
                if (0 >= (SomeBall.CenterOfTheBall.XCoordinate - SomeBall.RadiusOfTheBall) || 
                    (SomeBall.CenterOfTheBall.XCoordinate + SomeBall.RadiusOfTheBall) >= this.WidthOfTheBoard)
                {
                    SomeBall.VelocityVectorOfTheBall.XCoordinate = -SomeBall.VelocityVectorOfTheBall.XCoordinate;
                }

                if (0 >= (SomeBall.CenterOfTheBall.YCoordinate - SomeBall.RadiusOfTheBall) ||
                    (SomeBall.CenterOfTheBall.YCoordinate + SomeBall.RadiusOfTheBall) >= this.HeightOfTheBoard)
                {
                    SomeBall.VelocityVectorOfTheBall.YCoordinate = -SomeBall.VelocityVectorOfTheBall.YCoordinate;
                }
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
