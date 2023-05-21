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

        private class Position : DataPositionInterface
        {
            public override double XCoordinate { get; }
            public override double YCoordinate { get; }

            internal Position(double xCoordinate, double yCoordinate)
            {
                this.XCoordinate = xCoordinate;
                this.YCoordinate = yCoordinate;
            } 
        }
    }
}
