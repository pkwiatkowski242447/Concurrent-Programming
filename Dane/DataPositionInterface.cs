using System;

namespace Data
{
    public abstract class DataPositionInterface
    {
        public abstract double XCoordinate { get; set; }
        public abstract double YCoordinate { get; set; }

        public static DataPositionInterface CreatePosition(double xCoordinate, double yCoordinate)
        {
            return new Position(xCoordinate, yCoordinate);
        }
        public abstract DataPositionInterface Addition(DataPositionInterface otherPosition);
        public abstract DataPositionInterface Multiplication(double someDouble);
        public abstract DataPositionInterface Subtraction(DataPositionInterface otherPosition);
        public abstract double DotOperator(DataPositionInterface otherPosition);
        public abstract double VectorLength();
        public abstract double EuclideanDistance(DataPositionInterface otherPosition);

        private class Position : DataPositionInterface
        {
            public override double XCoordinate { get; set; }
            public override double YCoordinate { get; set; }

            public Position(double xCoordinate, double yCoordinate)
            {
                this.XCoordinate = xCoordinate;
                this.YCoordinate = yCoordinate;
            }
            public override double EuclideanDistance(DataPositionInterface otherPosition)
            {
                return Math.Sqrt(Math.Pow(this.XCoordinate - otherPosition.XCoordinate, 2) + Math.Pow(this.YCoordinate - otherPosition.YCoordinate, 2));
            }

            public override DataPositionInterface Addition(DataPositionInterface otherPosition)
            {
                double XCoordinate = this.XCoordinate + otherPosition.XCoordinate;
                double YCoordinate = this.YCoordinate + otherPosition.YCoordinate;
                return CreatePosition(XCoordinate, YCoordinate);
            }

            public override DataPositionInterface Multiplication(double someDouble)
            {
                double XCoordinate = this.XCoordinate * someDouble;
                double YCoordinate = this.YCoordinate * someDouble;
                return CreatePosition(XCoordinate, YCoordinate);
            }

            public override DataPositionInterface Subtraction(DataPositionInterface otherPosition)
            {
                double XCoordinate = this.XCoordinate - otherPosition.XCoordinate;
                double YCoordinate = this.YCoordinate - otherPosition.YCoordinate;
                return CreatePosition(XCoordinate, YCoordinate);
            }

            public override double DotOperator(DataPositionInterface otherPosition)
            {
                return this.XCoordinate * otherPosition.XCoordinate + this.YCoordinate * otherPosition.YCoordinate;
            }

            public override double VectorLength()
            {
                return Math.Sqrt(Math.Pow(this.XCoordinate, 2) + Math.Pow(this.YCoordinate, 2));
            }
        }
    }
}
