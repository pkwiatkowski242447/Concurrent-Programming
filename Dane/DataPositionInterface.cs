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
        }
    }
}
