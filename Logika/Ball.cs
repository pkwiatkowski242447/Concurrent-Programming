using System;
using System.Collections.Generic;
using System.Text;


namespace Logika
{
    internal class Ball
    {
        internal double massOfTheBall { get => massOfTheBall; set => massOfTheBall = value;  }
        internal Position centerOfTheBall { get => centerOfTheBall; set => centerOfTheBall = value; }
        internal int ballRadius { get => ballRadius; set => ballRadius = value; }
        internal Position velocityVector { get => velocityVector; set => velocityVector = value; }

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
                velocityVector.xCoordinate = -velocityVector.xCoordinate;
            }

            if (0 > (centerOfTheBall.yCoordinate + velocityVector.yCoordinate - ballRadius) ||
                heightOfTheTable < (centerOfTheBall.yCoordinate + velocityVector.yCoordinate + ballRadius))
            {
                velocityVector.yCoordinate = -velocityVector.yCoordinate;
            }

            centerOfTheBall.xCoordinate = centerOfTheBall.xCoordinate + velocityVector.xCoordinate;
            centerOfTheBall.yCoordinate = centerOfTheBall.yCoordinate + velocityVector.yCoordinate;
        }
    }
}