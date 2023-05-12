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

        private class Position : DataPositionInterface
        {
            public override double XCoordinate { get; set; }
            public override double YCoordinate { get; set; }

            public Position(double xCoordinate, double yCoordinate)
            {
                this.XCoordinate = xCoordinate;
                this.YCoordinate = yCoordinate;
            } 
        }
    }
}
