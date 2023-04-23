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
        public abstract int GetRadiusOfTheBall();
        private class DataAPI : DataAbstractAPI
        {
            internal int mass;
            internal int radius;
            internal int boardHeight;
            internal int boardWidth;
            internal Random randomIntNumber = new Random();

            public DataAPI(int boardHeight, int boardWidth)
            {
                this.mass = 10;
                this.radius = 10;
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
            public override int GetRadiusOfTheBall()
            {
                return radius;
            }

            internal Position GetAppropriateVelocityVector()
            {
                int Velocity_XValue = randomIntNumber.Next(-5, 5);
                int Velocity_YValue = randomIntNumber.Next(-5, 5);
                return new Position(Velocity_XValue, Velocity_YValue);
            }

            public Position RandomBallCenterCoordinates()
            {
                int CoordinateX = randomIntNumber.Next(radius, boardWidth - radius);
                int CoordinateY = randomIntNumber.Next(radius, boardHeight - radius);
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