using System;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public static DataAbstractAPI CreateDataAPIInstance(int widthOfTheTable, int heightOfTheTable)
        {
            return new DataAPI(widthOfTheTable, heightOfTheTable);
        }

        public abstract Ball CreateASingleBall();
        public abstract double GetMassOfTheBall();
        public abstract int GetRadiusOfTheBall();

        private class DataAPI : DataAbstractAPI
        {
            internal double HardcodedMass = 10.0;
            internal int HardcodedRadius = 10;
            internal Random randomIntNumber = new Random();
            internal int WidthOfTheTable { get; }
            internal int HeightOfTheTable { get; }

            public DataAPI(int widthOfTheTable, int heightOfTheTable)
            {
                this.WidthOfTheTable = widthOfTheTable;
                this.HeightOfTheTable = heightOfTheTable;
            }

            public override Ball CreateASingleBall()
            {
                Position centerOfTheBall = GetRandomPositionWithinTheMap();
                Position velocityVector;
                do
                {
                    velocityVector = GetAppropriateVelocityVector();
                }
                while(velocityVector.XCoordinate == 0 && velocityVector.YCoordinate == 0);
                Ball NewlyCreatedBall = new Ball(HardcodedMass, centerOfTheBall, HardcodedRadius, velocityVector);
                return NewlyCreatedBall;
            }

            public override double GetMassOfTheBall()
            {
                return HardcodedMass;
            }
            public override int GetRadiusOfTheBall()
            {
                return HardcodedRadius;
            }

            internal Position GetRandomPositionWithinTheMap()
            {
                int CoordinateX = randomIntNumber.Next(HardcodedRadius, WidthOfTheTable - HardcodedRadius);
                int CoordinateY = randomIntNumber.Next(HardcodedRadius, HeightOfTheTable - HardcodedRadius);
                return new Position(CoordinateX, CoordinateY);
            }

            internal Position GetAppropriateVelocityVector()
            {
                int Velocity_XValue = randomIntNumber.Next(-5, 5);
                int Velocity_YValue = randomIntNumber.Next(-5, 5);
                return new Position(Velocity_XValue, Velocity_YValue);
            }

            internal bool CheckIfBallCenterPostionIsCorrect(Position centerOfTheBall)
            {
                if (0 > (centerOfTheBall.XCoordinate - HardcodedRadius) || (centerOfTheBall.XCoordinate + HardcodedRadius) > WidthOfTheTable)
                {
                    return false;
                }
                if (0 > (centerOfTheBall.YCoordinate - HardcodedRadius) || (centerOfTheBall.YCoordinate + HardcodedRadius) > HeightOfTheTable)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
