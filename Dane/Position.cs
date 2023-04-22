namespace Dane
{
    public class Position
    {
        public int xCoordinate { get; set; }
        public int yCoordinate { get; set; }

        public Position(int xCoordinate, int yCoordinate)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
        }
    }
}
