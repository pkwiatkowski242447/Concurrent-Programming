using System;
using System.Runtime.Serialization;

namespace Data
{
    public abstract class DataPositionInterface : ISerializable
    {
        public abstract double XCoordinate { get; }
        public abstract double YCoordinate { get; }

        public static DataPositionInterface CreatePosition(double xCoordinate, double yCoordinate)
        {
            return new Position(xCoordinate, yCoordinate);
        }

        public abstract double VectorLength();

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X Coordinate value: ", XCoordinate);
            info.AddValue("Y Coordinate value: ", YCoordinate);
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

            public override double VectorLength()
            {
                return Math.Sqrt(Math.Pow(this.XCoordinate, 2) + Math.Pow(this.YCoordinate, 2));
            }
        }
    }
}
