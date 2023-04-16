using System;


namespace Logika
{
    public class Ball
    {
        public double massOfTheBall { get ; set ;  }
        public Position centerOfTheBall { get ; set ; }
        public int ballRadius { get ; set ; }
        public Position velocityVector { get ; set ; }

        public Ball(double massOfTheBall, Position centerOfTheBall, int ballRadius, Position velocityVector)
        {
            this.massOfTheBall = massOfTheBall;
            this.centerOfTheBall = centerOfTheBall;
            this.ballRadius = ballRadius;
            this.velocityVector = velocityVector;
        }

        internal void Move(int widthOfTheTable, int heightOfTheTable)
        {
            if (0 > (centerOfTheBall.xCoordinate + velocityVector.xCoordinate - ballRadius) || 
                widthOfTheTable < (centerOfTheBall.xCoordinate + velocityVector.xCoordinate + ballRadius))
            {
                velocityVector.xCoordinate = 0;
            }

            if (0 > (centerOfTheBall.yCoordinate + velocityVector.yCoordinate - ballRadius) ||
                heightOfTheTable < (centerOfTheBall.yCoordinate + velocityVector.yCoordinate + ballRadius))
            {
                velocityVector.yCoordinate = 0;
            }

            centerOfTheBall.xCoordinate = centerOfTheBall.xCoordinate + velocityVector.xCoordinate;
            centerOfTheBall.yCoordinate = centerOfTheBall.yCoordinate + velocityVector.yCoordinate;
        }
    }
}