using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logika
{
    internal class PoolTable : LogicAbstractAPI
    {
        internal int tableWidth { get; }
        internal int tableHeight { get; }
        internal int numberOfBalls { get; }
        internal static int radius = 10;
        internal static int mass = 10;
        internal List<Ball> listOfBalls = new List<Ball>();
        internal List<Task> listOfTasks = new List<Task>();

        internal PoolTable(int tableWidth, int tableHeight, int numberOfBalls)
        {
            this.tableWidth = tableWidth;
            this.tableHeight = tableHeight;
            this.numberOfBalls = numberOfBalls;
        }

        
        public override void AddSpecifiedNumerOfBalls(int numberOfBallsToAdd)
        {
            for (int i = 0; i < numberOfBalls; i++)
            {
                Ball newBall = new Ball(mass, GetRandomPosition(), radius, GetRandomPosition());
                listOfBalls.Add(newBall);
            }
        }
        public override void ClearPoolTable()
        {
            for (int i = 0; i < listOfTasks.Count; i++)
            {
                listOfTasks[i].Dispose();
            }
        }

        public override void MoveGeneratedBalls()
        {
            for (int i = 0; i < listOfBalls.Count; i++)
            {
                Task newTask = new Task(() =>
                {
                    while (true)
                    {
                        listOfBalls[i].Move(this.tableWidth, this.tableHeight);
                        listOfBalls[i].velocityVector = GetRandomPosition();
                        Thread.Sleep(10);
                    }
                });
                listOfTasks.Add(newTask);
            }
        }

        internal Position GetRandomPosition()
        {
            Random randomNumberGenerator = new Random();
            int minimumCoordinateValue = 0 + radius;
            int generatedX = randomNumberGenerator.Next(this.tableWidth - 2 * radius) + radius;
            int generatedY = randomNumberGenerator.Next(this.tableHeight - 2 * radius) + radius;
            return new Position(generatedX, generatedY);
        } 
    }
}
