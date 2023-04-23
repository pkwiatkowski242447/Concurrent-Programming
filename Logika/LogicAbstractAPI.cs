using Dane;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Logika
{
    public abstract class LogicAbstractAPI : IObservable<int>
    {
        public static LogicAbstractAPI CreateApi()
        {
            return new LogicAPI();
        }
        public abstract void CreateBalls(int howManyBalls);
        public abstract void MoveBalls();
        public abstract void ClearBoard();
        public abstract List<List<int>> GetBalls();
        public abstract IDisposable Subscribe(IObserver<int> observer);
        private class LogicAPI : LogicAbstractAPI
        {
            internal int boardHeight = 690;
            internal int boardWidth = 740;
            internal DataAbstractAPI DataAPI;
            internal List<Ball> Balls = new List<Ball>();
            internal List<Task> Tasks = new List<Task>();
            internal bool stop;
            private IObserver<int>? observer;

            internal LogicAPI()
            {
                DataAPI = DataAbstractAPI.CreateApi(boardWidth, boardHeight);
            }
            public override void ClearBoard()
            {
                stop = true;
                bool isEveryTaskStopped = false;
                while (!isEveryTaskStopped)
                {
                    isEveryTaskStopped = true;
                    foreach (Task task in Tasks)
                    {
                        if (!task.IsCompleted)
                        {
                            isEveryTaskStopped = false;
                            break;
                        }
                    }
                }
                for (int i = 0; i < Tasks.Count; i++)
                {
                    Tasks[i].Dispose();
                }
                Balls.Clear();
                Tasks.Clear();
            }

            public override void CreateBalls(int howManyBalls)
            {
                for (int i = 0; i < howManyBalls; i++)
                {
                    int id = i;
                    Ball ball = DataAPI.CreateBall();
                    Balls.Add(ball);
                    Task task = new Task(() =>
                    {
                        while (!stop)
                        {
                            check_coordinates(ball);
                            ball.MoveBall();
                            if (observer != null)
                            {
                                observer.OnNext(id);
                            }
                            Task.Delay(10).Wait();
                        }
                    });
                    Tasks.Add(task);
                }
            }

            public override List<List<int>> GetBalls()
            {
                List<List<int>> ListOfBallsCoordinates = new List<List<int>>();
                for (int i = 0; i < Balls.Count; i++)
                {
                    int XCoordinate = Balls[i].centerOfTheBall.xCoordinate;
                    int YCoordinate = Balls[i].centerOfTheBall.yCoordinate;
                    int BallRadius = Balls[i].ballRadius;
                    int VelocityX = Balls[i].velocityVector.xCoordinate;
                    int VelocityY = Balls[i].velocityVector.yCoordinate;
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

            public override void MoveBalls()
            {
                stop = false;
                for (int i = 0; i < Tasks.Count; i++)
                {
                    Tasks[i].Start();
                }
            }

            public override IDisposable Subscribe(IObserver<int> observer)
            {
                this.observer = observer;
                return new ObserverManager(observer);
            }
            private class ObserverManager : IDisposable
            {
                public IObserver<int>? ObserverToBeManaged;

                public ObserverManager(IObserver<int> observerObject)
                {
                    ObserverToBeManaged = observerObject;
                }

                public void Dispose()
                {
                    ObserverToBeManaged = null;
                }
            }
            private void check_coordinates(Ball ball_object)
            {
                if (ball_object.centerOfTheBall.xCoordinate + ball_object.velocityVector.xCoordinate > boardWidth || ball_object.centerOfTheBall.xCoordinate + ball_object.velocityVector.xCoordinate < 0)
                {
                    ball_object.velocityVector.xCoordinate = -ball_object.velocityVector.xCoordinate;
                }
                if (ball_object.centerOfTheBall.yCoordinate + ball_object.velocityVector.yCoordinate > boardHeight || ball_object.centerOfTheBall.yCoordinate + ball_object.velocityVector.yCoordinate < 0)
                {
                    ball_object.velocityVector.yCoordinate = -ball_object.velocityVector.yCoordinate;
                }

            }
        }
    }
}
