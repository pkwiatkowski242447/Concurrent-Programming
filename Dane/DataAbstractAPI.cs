using System;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public static DataAbstractAPI CreateDataAPIInstance()
        {
            return new DataAPI();
        }

        public abstract DataBallInterface CreateASingleBall(int idOfTheBall, double radiusOfTheBall);
        public abstract void CreateSerializerObject();
        public abstract void CreateBoard(int widthOfTheBoard, int heightOfTheBoard);
        public abstract double GetMassOfTheBall();
        public abstract int GetWidthOfTheBoard();
        public abstract int GetHeightOfTheBoard();

        private class DataAPI : DataAbstractAPI
        {
            internal double HardcodedMass = 10.0;
            internal Random randomNumber = new Random();
            internal DataBoardInterface? Board { get; set; }
            internal DataBallSerializer? ballSerializer = null;

            public override void CreateSerializerObject()
            {
                this.ballSerializer = DataBallSerializer.CreateJSONSerializer();
            }

            public override void CreateBoard(int widthOfTheBoard, int heightOfTheBoard)
            {
                this.Board = DataBoardInterface.CreateBoard(widthOfTheBoard, heightOfTheBoard);
                ballSerializer?.AddBoardDataToSerializationQueue(this.Board);
            }

            public override DataBallInterface CreateASingleBall(int idOfTheBall, double radiusOfTheBall)
            {
                DataPositionInterface centerOfTheBall = GetRandomPositionWithinTheMap(radiusOfTheBall);
                DataPositionInterface velocityVector;
                do
                {
                    velocityVector = GetAppropriateVelocityVector();
                }
                while (velocityVector.XCoordinate == 0 && velocityVector.YCoordinate == 0);
                DataBallInterface NewlyCreatedBall = DataBallInterface.CreateBall(idOfTheBall, HardcodedMass, centerOfTheBall, velocityVector, this.ballSerializer);
                return NewlyCreatedBall;
            }

            public override double GetMassOfTheBall()
            {
                return HardcodedMass;
            }

            public override int GetWidthOfTheBoard()
            {
                if (this.Board != null)
                {
                    return this.Board.WidthOfTheBoard;
                }
                return 0;
            }

            public override int GetHeightOfTheBoard()
            {
                if (this.Board != null)
                {
                    return this.Board.HeightOfTheBoard;
                }
                return 0;
            }
            internal DataPositionInterface GetRandomPositionWithinTheMap(double radiusOfTheBall)
            {
                double CoordinateX = randomNumber.NextDouble() * (GetWidthOfTheBoard() - 2 * radiusOfTheBall) + radiusOfTheBall;
                double CoordinateY = randomNumber.NextDouble() * (GetHeightOfTheBoard() - 2 * radiusOfTheBall) + radiusOfTheBall;
                return DataPositionInterface.CreatePosition(CoordinateX, CoordinateY);
            }

            internal DataPositionInterface GetAppropriateVelocityVector()
            {
                double Velocity_XValue = randomNumber.NextDouble() * 100 - 50;
                double Velocity_YValue = randomNumber.NextDouble() * 100 - 50;
                return DataPositionInterface.CreatePosition(Velocity_XValue, Velocity_YValue);
            }
        }
    }
}