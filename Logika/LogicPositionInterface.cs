using System;

namespace Logic
{
    public abstract class LogicPositionInterface
    {
        public abstract double XCoordinate { get; }
        public abstract double YCoordinate { get; }

        public static LogicPositionInterface CreateLogicPosition(double xCoordinate, double yCoordinate)
        {
            return new LogicPosition(xCoordinate, yCoordinate);
        }

        public abstract LogicPositionInterface Addition(LogicPositionInterface otherPosition);
        public abstract LogicPositionInterface Multiplication(double someDouble);
        public abstract LogicPositionInterface Subtraction(LogicPositionInterface otherPosition);
        public abstract double DotOperator(LogicPositionInterface otherPosition);
        public abstract double VectorLength();
        public abstract double EuclideanDistance(LogicPositionInterface otherPosition);

        private class LogicPosition : LogicPositionInterface
        {
            public override double XCoordinate { get; }
            public override double YCoordinate { get; }

            internal LogicPosition(double xCoordinate, double yCoordinate)
            {
                this.XCoordinate = xCoordinate;
                this.YCoordinate = yCoordinate;
            }

            public override double EuclideanDistance(LogicPositionInterface otherPosition)
            {
                return Math.Sqrt(Math.Pow(this.XCoordinate - otherPosition.XCoordinate, 2) + Math.Pow(this.YCoordinate - otherPosition.YCoordinate, 2));
            }

            public override LogicPositionInterface Addition(LogicPositionInterface otherPosition)
            {
                double XCoordinate = this.XCoordinate + otherPosition.XCoordinate;
                double YCoordinate = this.YCoordinate + otherPosition.YCoordinate;
                return CreateLogicPosition(XCoordinate, YCoordinate);
            }

            public override LogicPositionInterface Multiplication(double someDouble)
            {
                double XCoordinate = this.XCoordinate * someDouble;
                double YCoordinate = this.YCoordinate * someDouble;
                return CreateLogicPosition(XCoordinate, YCoordinate);
            }

            public override LogicPositionInterface Subtraction(LogicPositionInterface otherPosition)
            {
                double XCoordinate = this.XCoordinate - otherPosition.XCoordinate;
                double YCoordinate = this.YCoordinate - otherPosition.YCoordinate;
                return CreateLogicPosition(XCoordinate, YCoordinate);
            }

            public override double DotOperator(LogicPositionInterface otherPosition)
            {
                return this.XCoordinate * otherPosition.XCoordinate + this.YCoordinate * otherPosition.YCoordinate;
            }

            public override double VectorLength()
            {
                return Math.Sqrt(Math.Pow(this.XCoordinate, 2) + Math.Pow(this.YCoordinate, 2));
            }
        }
    }
}
