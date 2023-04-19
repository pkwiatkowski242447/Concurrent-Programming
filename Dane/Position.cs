namespace Data
{
    public class Position
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }

        public Position(int xCoordinate, int yCoordinate)
        {
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
        }
    }
}
