using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public abstract class LogicAbstractAPI : IObservable<int>
    {
        public static LogicAbstractAPI CreateLogicAPIInstance()
        {
            return new LogicAPI();
        }
        public abstract void CreateSpecifiedNumerOfBalls(int numberOfBallsToAdd);
        public abstract void ClearPoolTable();
        public abstract void MoveGeneratedBalls();
        public abstract List<List<int>> GetAllBallsCoordinates();
        public abstract IDisposable Subscribe(IObserver<int> observer);

        private class LogicAPI : LogicAbstractAPI
        {
            internal bool stopTasks = true;
            internal DataAbstractAPI DataAPI;
            internal int WidthOfTheTable = 740;
            internal int HeightOfTheTable = 690;
            internal IObserver<int>? ObserverObject;
            internal List<Ball> ListOfManagedBalls { get; set; }
            internal List<Task> ListOfManagedTasks { get; set; }

            public LogicAPI()
            {
                DataAPI = DataAbstractAPI.CreateDataAPIInstance(WidthOfTheTable, HeightOfTheTable);
                ListOfManagedBalls = new List<Ball>();
                ListOfManagedTasks = new List<Task>();
            }

            public override void CreateSpecifiedNumerOfBalls(int numberOfBallsToAdd)
            {
                for (int i = 0; i < numberOfBallsToAdd; i++)
                {
                    int taskIdentfier = i;
                    Ball currentBall = DataAPI.CreateASingleBall();
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
            public override void MoveGeneratedBalls()
            {
                stopTasks = false;
                // Tu powinna byæ ju¿ faktyczna implementacja, jako, ¿e ruch to element Logiki programu
                for (int i = 0; i < ListOfManagedTasks.Count; i++)
                {
                    ListOfManagedTasks[i].Start();
                }
            }
            public override List<List<int>> GetAllBallsCoordinates()
            {
                List<List<int>> ListOfBallsCoordinates = new List<List<int>>();
                for (int i = 0; i < ListOfManagedBalls.Count; i++)
                {
                    int XCoordinate = ListOfManagedBalls[i].CenterOfTheBall.XCoordinate;
                    int YCoordinate = ListOfManagedBalls[i].CenterOfTheBall.YCoordinate;
                    int BallRadius = ListOfManagedBalls[i].BallRadius;
                    int VelocityX = ListOfManagedBalls[i].VelocityVector.XCoordinate;
                    int VelocityY = ListOfManagedBalls[i].VelocityVector.YCoordinate;
                    List<int> Coords = new List<int>()
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
                if (0 >= (SomeBall.CenterOfTheBall.XCoordinate - SomeBall.BallRadius) || 
                    (SomeBall.CenterOfTheBall.XCoordinate + SomeBall.BallRadius) >= WidthOfTheTable)
                {
                    SomeBall.VelocityVector.XCoordinate = -SomeBall.VelocityVector.XCoordinate;
                }

                if (0 >= (SomeBall.CenterOfTheBall.YCoordinate - SomeBall.BallRadius) ||
                    (SomeBall.CenterOfTheBall.YCoordinate + SomeBall.BallRadius) >= HeightOfTheTable)
                {
                    SomeBall.VelocityVector.YCoordinate = -SomeBall.VelocityVector.YCoordinate;
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
