namespace Logika
{
    public class Ball
    {
        private double massOfTheBall { get; set; }
        public Position centerOfTheBall { get; set; }
        public int ballRadius { get; set; }
        public Position velocityVector { get; set; }

        public Ball(double massOfTheBall, Position centerOfTheBall, int ballRadius, Position velocityVector)
        {
            this.massOfTheBall = massOfTheBall;
            this.centerOfTheBall = centerOfTheBall;
            this.ballRadius = ballRadius;
            this.velocityVector = velocityVector;
        }


        public void MoveBall(int height, int width)
        {
            if (centerOfTheBall.xCoordinate + velocityVector.xCoordinate > width - ballRadius || 0 > (centerOfTheBall.xCoordinate + velocityVector.xCoordinate - ballRadius))
            {
                velocityVector.xCoordinate = -velocityVector.xCoordinate;

            }
            if (centerOfTheBall.yCoordinate + velocityVector.yCoordinate > height - ballRadius || 0 > (centerOfTheBall.yCoordinate + velocityVector.yCoordinate - ballRadius))
            {
                velocityVector.yCoordinate = -velocityVector.yCoordinate;

            }
            centerOfTheBall.xCoordinate =  velocityVector.xCoordinate + centerOfTheBall.xCoordinate;
            centerOfTheBall.yCoordinate = velocityVector.yCoordinate + centerOfTheBall.yCoordinate;

        }
    }
}
