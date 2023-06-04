using System;

namespace Data
{
    public abstract class DataPositionInterface
    {
        public abstract double XCoordinate { get; }
        public abstract double YCoordinate { get; }

        public static DataPositionInterface CreatePosition(double xCoordinate, double yCoordinate)
        {
            return new Position(xCoordinate, yCoordinate);
        }

        public abstract double VectorLength();

        private class Position : DataPositionInterface
        {
            public override double XCoordinate { get; }
            public override double YCoordinate { get; }

            internal Position(double xCoordinate, double yCoordinate)
            {
                this.XCoordinate = xCoordinate;
                this.YCoordinate = yCoordinate;
            }

            public override double VectorLength()
            {
                return Math.Sqrt(Math.Pow(this.XCoordinate, 2) + Math.Pow(this.YCoordinate, 2));
            }
        }
    }
}
