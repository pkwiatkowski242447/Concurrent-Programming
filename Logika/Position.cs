namespace Logika
{
    public class Position
    {
        public int xCoordinate;
        public int yCoordinate;

        public Position(int xCoordinate, int yCoordinate)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
        }

        public int getXCoordinate()
        {
            return this.xCoordinate;
        }

        public int getYCoordinate()
        {
            return this.yCoordinate;
        }

        public void setXCoordinate(int xCoordinate)
        {
            this.xCoordinate = xCoordinate;
        }

        public void setYCoordinate(int yCoordinate)
        {
            this.yCoordinate = yCoordinate;
        }
    }
}
