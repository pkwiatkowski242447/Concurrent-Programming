using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane
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
    }
}
