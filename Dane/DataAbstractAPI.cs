using System;

namespace Dane
{
    public abstract class DataAbstractAPI
    {
        public static DataAbstractAPI CreateApi(int heightOfTheBoard, int widthOfTheBoard)
        {
            return new DataAPI(heightOfTheBoard, widthOfTheBoard);
        }
        public abstract Ball CreateBall();
        public abstract int getMassOfTheBall();
        public abstract int getRadiusOfTheBall();
        private class DataAPI : DataAbstractAPI
        {
            internal int mass;
            internal int radius;
            internal int boardHeight;
            internal int boardWidth;

            public DataAPI(int boardHeight, int boardWidth)
            {
                this.mass = 10;
                this.radius = 10;
                this.boardHeight = boardHeight;
                this.boardWidth = boardWidth;
            }

            public override Ball CreateBall()
            {
                Position ball_center_position = randomXY_ball();
                Position velocityVector = new Position(0,0);
                while (velocityVector.xCoordinate ==0 && velocityVector.yCoordinate ==0)
                {
                    velocityVector = randomXY();
                }
                Ball ball = new Ball(mass, ball_center_position, radius, velocityVector);
                return ball;
            }

            public override int getMassOfTheBall()
            {
                return mass;
            }

            public override int getRadiusOfTheBall()
            {
                return radius;
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
                int x = random.Next(radius, boardWidth - radius);
                int y = random.Next(radius, boardHeight - radius);
                Position coordinates = new Position(x, y);
                return coordinates;
            }
        }
    }
}
