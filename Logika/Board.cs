using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Logika
{
    public class Board : LogicAbstractAPI
    {
        public bool stop;
        private int heightOfTheBoard { get; }
        private int widthOfTheBoard { get; }
        private int radius;
        private int mass;
        private IObserver<int>? observer = null;
        public List<Ball> Balls = new List<Ball>();
        public List<Task> Tasks = new List<Task>();
        public Board(int heightOfTheBoard, int widthOfTheBoard)
        {
            this.heightOfTheBoard = heightOfTheBoard;
            this.widthOfTheBoard = widthOfTheBoard;
            this.radius = 10;
            this.mass = 10;
        }

        public override void CreateBalls(int howManyBalls)
        {
            for (int i = 0; i < howManyBalls; i++)
            {
                Position centerOfTheBall = randomXY_ball();
                Position velocityVector = randomXY();
                Ball ball = new Ball(mass, centerOfTheBall, radius, velocityVector);
                Balls.Add(ball);
                Task task = new Task(() =>
                {
                    while (!stop)
                    {
                        int id = i;
                        ball.MoveBall(this.heightOfTheBoard, this.widthOfTheBoard);
                        Position previousPosition;
                        do
                        {
                            previousPosition = ball.centerOfTheBall;
                            ball.centerOfTheBall = randomXY_ball();
                        } while (ball.centerOfTheBall.xCoordinate == previousPosition.xCoordinate || ball.centerOfTheBall.yCoordinate == previousPosition.yCoordinate);
                        if (observer != null)
                        {
                            observer.OnNext(i);
                        }
                        Thread.Sleep(50);
                    }
                });
                Tasks.Add(task);
            }
        }

        public Position randomXY()
        {
            var random = new Random();
            int x = random.Next(-10, 10);
            int y = random.Next(-10, 10);
            Position coordinates = new Position(x, y);
            return coordinates;
        }

        public Position randomXY_ball()
        {
            var random = new Random();
            int x = random.Next(radius , widthOfTheBoard - radius);
            int y = random.Next(radius, heightOfTheBoard - radius);
            Position coordinates = new Position(x, y);
            return coordinates;
        }


        public override void MoveBalls()
        {
            stop = false;
            for (int i = 0; i < Balls.Count; i++)
            {
                Tasks[i].Start();
            }
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

        public override List<Ball> GetBalls()
        {
            return Balls;
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
    }
}

