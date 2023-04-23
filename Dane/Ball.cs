namespace Dane
{
    public class Ball
    {
        public double massOfTheBall { get; set; }
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


        public void MoveBall()
        {
            centerOfTheBall.xCoordinate = velocityVector.xCoordinate + centerOfTheBall.xCoordinate;
            centerOfTheBall.yCoordinate = velocityVector.yCoordinate + centerOfTheBall.yCoordinate;

        }
    }
}
