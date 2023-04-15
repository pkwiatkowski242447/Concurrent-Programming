namespace Logika
{
    internal class Position
    {
        public int xCoordinate;
        public int yCoordinate;

        internal Position(int xCoordinate, int yCoordinate)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
        }

        internal int getXCoordinate()
        {
            return this.xCoordinate;
        }

        internal int getYCoordinate()
        {
            return this.yCoordinate;
        }

        internal void setXCoordinate(int xCoordinate)
        {
            this.xCoordinate = xCoordinate;
        }

        internal void setYCoordinate(int yCoordinate)
        {
            this.yCoordinate = yCoordinate;
        }
    }
}
