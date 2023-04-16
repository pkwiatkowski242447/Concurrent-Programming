using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logika
{
    public class Board : LogicAbstractAPI
    {
        public bool stop;
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
        public int GetHeightOfTheBoard()
        {
            return heightOfTheBoard;
        }
        public int GetWidthOfTheBoard()
        {
            return widthOfTheBoard;
        }

        public override void CreateBalls(int howManyBalls)
        {
            for (int i = 0; i < howManyBalls; i++)
            {
                Position centerOfTheBall = randomXY();
                Position velocityVector = randomXY();
                Ball ball = new Ball(mass, centerOfTheBall, radius, velocityVector);
                Balls.Add(ball);
                Task task = new Task(() =>
                {
                    while (!stop)
                    {
                        ball.MoveBall(this.heightOfTheBoard, this.widthOfTheBoard);
                        Position previousPosition;
                        do
                        {
                            previousPosition = ball.GetCenterOfTheBall();
                            ball.SetCenterOfTheBall(randomXY());   
                        } while (ball.GetCenterOfTheBall().xCoordinate == previousPosition.xCoordinate || ball.GetCenterOfTheBall().yCoordinate == previousPosition.yCoordinate);
                        Thread.Sleep(50);
                    }
                });
                Tasks.Add(task);
            }
        }

        public Position randomXY()
        {
            var random = new Random();
            int x = random.Next(radius, GetWidthOfTheBoard() - 2 * radius);
            int y = random.Next(radius, GetHeightOfTheBoard() - 2 * radius);
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
    }
}
