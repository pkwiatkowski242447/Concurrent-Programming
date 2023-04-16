using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Logika
{
    public class PoolTable : LogicAbstractAPI
    {
        private bool stopTasks { get; set; }
        private int tableWidth { get; }
        private int tableHeight { get; }
        private static int radius = 10;
        private static double mass = 10;
        private IObserver<int>? observer = null;
        private List<Ball> listOfBalls = new List<Ball>();
        private readonly List<Task> listOfTasks = new List<Task>();
        private Random randomNumberGenerator = new Random();

        internal PoolTable(int tableWidth, int tableHeight)
        {
            this.tableWidth = tableWidth;
            this.tableHeight = tableHeight;
        }

        
        public override void AddSpecifiedNumerOfBalls(int numberOfBallsToAdd)
        {
            for (int i = 0; i < numberOfBallsToAdd; i++)
            {
                Position centerOfTheBall = GetRandomPosition();
                Position velocityVector = GetRandomPosition();
                Ball newBall = new Ball(mass, centerOfTheBall, radius, velocityVector);
                listOfBalls.Add(newBall);

                Task newTask = new Task(() =>
                {
                    while (!stopTasks)
                    {
                        int identifier = i;
                        newBall.Move(this.tableWidth, this.tableHeight);
                        Position previousPosition;
                        do
                        {
                            previousPosition = newBall.centerOfTheBall;
                            newBall.centerOfTheBall = GetRandomPosition();
                        } while (newBall.centerOfTheBall.xCoordinate == previousPosition.xCoordinate ||
                            newBall.centerOfTheBall.yCoordinate == previousPosition.yCoordinate);
                        if (observer != null)
                        {
                            observer.OnNext(i);
                        }
                        Thread.Sleep(50);
                    }
                });
                listOfTasks.Add(newTask);
            }
        }
        public override void ClearPoolTable()
        {
            stopTasks = true;
            bool isEveryTaskStopped = false;
            while (!isEveryTaskStopped)
            {
                isEveryTaskStopped = true;
                foreach (Task task in listOfTasks)
                {
                    if (!task.IsCompleted)
                    {
                        isEveryTaskStopped = false;
                        break;
                    }
                }
            }
            for (int i = 0; i < listOfTasks.Count; i++)
            {
                listOfTasks[i].Dispose();
            }
            listOfBalls.Clear();
            listOfTasks.Clear();
        }

        public override void MoveGeneratedBalls()
        {
            stopTasks = false;
            for (int i = 0; i < listOfBalls.Count; i++)
            {
                listOfTasks[i].Start();
            }
        }

        public override List<Ball> GetBallsList()
        {
            return listOfBalls;
        }

        public override IDisposable Subscribe(IObserver<int> observer)
        {
            this.observer = observer;
            return new ObserverManager(observer);
        }

        internal Position GetRandomPosition()
        {
            int minimumCoordinateValue = 0 + radius;
            int generatedX = randomNumberGenerator.Next(2 * (this.tableWidth - radius)) - this.tableWidth + radius;
            int generatedY = randomNumberGenerator.Next(2 * (this.tableHeight - radius)) - this.tableHeight + radius;
            return new Position(generatedX, generatedY);
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
