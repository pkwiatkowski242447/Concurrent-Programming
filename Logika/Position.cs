using System;

namespace Logika
{
    internal class Position
    {
        internal int xCoordinate { get => xCoordinate; set => xCoordinate = value; }
        internal int yCoordinate { get => yCoordinate; set => yCoordinate = value; }

        public Position(int xCoordinate, int yCoordinate)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;
        }
    }
}
