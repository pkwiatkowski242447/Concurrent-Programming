using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Logika
{
    internal class Board : LogicAbstractAPI
    {
        private int heightOfTheBoard;
        private int widthOfTheBoard;
        private int radius;
        private int mass;
        public List<Ball> Balls = new List<Ball>();
        public List<Task> Tasks = new List<Task>();
        public Board(int heightOfTheBoard, int widthOfTheBoard)
        {
            this.heightOfTheBoard = heightOfTheBoard;
            this.widthOfTheBoard = widthOfTheBoard;
            this.radius = 10;
            this.mass = 10;
        }
        private int getHeightOfTheBoard()
        {
            return heightOfTheBoard;
        }
        private int getWidthOfTheBoard()
        {
            return widthOfTheBoard;
        }

        public override void CreateBalls(int howManyBalls)
        {
            for (int i = 0; i < howManyBalls; i++)
            {
                Ball ball = new Ball(mass, randomXY(), radius, randomXY());
                Balls.Add(ball);
            }
        }

        internal Position randomXY()
        {
            var random = new Random();
            int x = random.Next(0 + radius, getWidthOfTheBoard());
            int y = random.Next(0 + radius, getHeightOfTheBoard());
            Position coordinates = new Position(x, y);
            return coordinates;
        }


        public override void MoveBalls()
        {
            for (int i = 0; i < Balls.Count; i++)
            {
                Task task = new Task(() =>
                {
                    while (true)
                    {
                        Balls[i].MoveBall(this.heightOfTheBoard, this.widthOfTheBoard);
                        Balls[i].setVelocityVector(randomXY());
                        Thread.Sleep(10);
                    }
                });
                Tasks.Add(task);
            }
        }

        public override void ClearBoard()
        {
            for (int i = 0; i < Balls.Count; i++)
            {
                Tasks[i].Dispose();
            }
        }
    }
}
