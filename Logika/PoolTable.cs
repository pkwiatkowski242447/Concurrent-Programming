using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Logika
{
    public class PoolTable : LogicAbstractAPI
    {
        internal bool stopTasks { get; set; }
        internal int tableWidth { get; }
        internal int tableHeight { get; }
        internal static int radius = 10;
        internal static double mass = 10;
        internal List<Ball> listOfBalls = new List<Ball>();
        private readonly List<Task> listOfTasks = new List<Task>();
        internal Random randomNumberGenerator = new Random();

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
                        newBall.Move(this.tableWidth, this.tableHeight);
                        Position previousPosition;
                        do
                        {
                            previousPosition = newBall.centerOfTheBall;
                            newBall.centerOfTheBall = GetRandomPosition();
                        } while (newBall.centerOfTheBall.xCoordinate == previousPosition.xCoordinate ||
                            newBall.centerOfTheBall.yCoordinate == previousPosition.yCoordinate);
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

        internal Position GetRandomPosition()
        {
            int minimumCoordinateValue = 0 + radius;
            int generatedX = randomNumberGenerator.Next(this.tableWidth - 2 * radius) + radius;
            int generatedY = randomNumberGenerator.Next(this.tableHeight - 2 * radius) + radius;
            return new Position(generatedX, generatedY);
        } 
    }
}
