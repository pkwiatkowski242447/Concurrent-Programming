namespace Logika
{
    public class Ball
    {
        private double massOfTheBall;
        private Position centerOfTheBall;
        private int ballRadius;
        private Position velocityVector;

        public Ball(double massOfTheBall, Position centerOfTheBall, int ballRadius, Position velocityVector)
        {
            this.massOfTheBall = massOfTheBall;
            this.centerOfTheBall = centerOfTheBall;
            this.ballRadius = ballRadius;
            this.velocityVector = velocityVector;
        }

        /*
         * Getters
         */

        public int GetBallRadius()
        {
            return this.ballRadius;
        }

        public Position GetCenterOfTheBall()
        {
            return centerOfTheBall;
        }

        /*
         * Setter methods.
         */

        public void SetVelocityVector(Position newVelocity)
        {
            this.velocityVector = newVelocity;
        }

        public void SetCenterOfTheBall(Position newCenterOfTheBall)
        {
            this.centerOfTheBall = newCenterOfTheBall;
        }

        public Position GetVelocityVector()
        {
            return velocityVector;
        }

        public void MoveBall(int height, int width)
        {
            if (centerOfTheBall.getXCoordinate() + velocityVector.getXCoordinate() > width - GetBallRadius() || 0 > (centerOfTheBall.getXCoordinate() + velocityVector.getXCoordinate() - ballRadius))
            {
                velocityVector.setXCoordinate(0);

            }
            if (centerOfTheBall.getYCoordinate() + velocityVector.getYCoordinate() > height - GetBallRadius() || 0 > (centerOfTheBall.getYCoordinate() + velocityVector.getYCoordinate() - ballRadius))
            {
                velocityVector.setYCoordinate(0);

            }
            centerOfTheBall.setXCoordinate(velocityVector.getXCoordinate() + centerOfTheBall.getXCoordinate());
            centerOfTheBall.setYCoordinate(velocityVector.getYCoordinate() + centerOfTheBall.getYCoordinate());

        }
    }
}
