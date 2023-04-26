using System;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public static DataAbstractAPI CreateDataApi(int heightOfTheBoard, int widthOfTheBoard)
        {
            return new DataAPI(heightOfTheBoard, widthOfTheBoard);
        }
        public abstract Ball CreateBall();
        public abstract double GetMassOfTheBall();
        public abstract double GetRadiusOfTheBall();
        private class DataAPI : DataAbstractAPI
        {
            internal double mass;
            internal double radius;
            internal int boardHeight;
            internal int boardWidth;
            internal Random randomNumber = new Random();

            public DataAPI(int boardHeight, int boardWidth)
            {
                this.mass = 10.0;
                this.radius = 10.0;
                this.boardHeight = boardHeight;
                this.boardWidth = boardWidth;
            }

            public override Ball CreateBall()
            {
                Position ball_center_position = RandomBallCenterCoordinates();
                Position velocityVector;
                do
                {
                    velocityVector = GetAppropriateVelocityVector();
                }
                while (velocityVector.XCoordinate == 0 && velocityVector.YCoordinate == 0);
                Ball ball = new Ball(mass, ball_center_position, radius, velocityVector);
                return ball;
            }

            public override double GetMassOfTheBall()
            {
                return mass;
            }
            public override double GetRadiusOfTheBall()
            {
                return radius;
            }

            internal Position GetAppropriateVelocityVector()
            {
                double Velocity_XValue = (randomNumber.NextDouble() * 10) - 5;
                double Velocity_YValue = (randomNumber.NextDouble() * 10) - 5;
                return new Position(Velocity_XValue, Velocity_YValue);
            }

            public Position RandomBallCenterCoordinates()
            {
                double CoordinateX = randomNumber.NextDouble() * (boardWidth - 2 * radius) + radius;
                double CoordinateY = randomNumber.NextDouble() * (boardHeight - 2 * radius) + radius;
                Position coordinates = new Position(CoordinateX, CoordinateY);
                return coordinates;
            }
            internal bool CheckIfBallCenterPostionIsCorrect(Position centerOfTheBall)
            {
                if (0 > (centerOfTheBall.XCoordinate - radius) || (centerOfTheBall.XCoordinate + radius) > boardWidth)
                {
                    return false;
                }
                if (0 > (centerOfTheBall.YCoordinate - radius) || (centerOfTheBall.YCoordinate + radius) > boardHeight)
                {
                    return false;
                }
                return true;
            }
        }
    }
}