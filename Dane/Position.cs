using System;

namespace Data
{
    public class Position
    {
        public double XCoordinate { get; set; }
        public double YCoordinate { get; set; }

        public Position(double xCoordinate, double yCoordinate)
        {
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
        }

        public static Position operator +(Position posNo1, Position posNo2) => new Position(posNo1.XCoordinate + posNo2.XCoordinate,
            posNo1.YCoordinate + posNo2.XCoordinate);

        public static Position operator *(double number, Position somePosition) => new Position(number * somePosition.XCoordinate,
             number * somePosition.YCoordinate);

        public static Position operator -(Position posNo1, Position posNo2) => new Position(posNo1.XCoordinate - posNo2.XCoordinate,
            posNo1.YCoordinate - posNo2.YCoordinate);

        public static double DotOperator(Position posNo1, Position posNo2)
        {
            double DotProduct = posNo1.XCoordinate * posNo2.XCoordinate + posNo1.YCoordinate * posNo2.YCoordinate;
            return DotProduct;
        }

        public static double VectorLength(Position posNo1)
        {
            double VecotrLength = Math.Sqrt(Math.Pow(posNo1.XCoordinate, 2) + Math.Pow(posNo1.YCoordinate, 2));
            return VecotrLength;
        }
    }
}
