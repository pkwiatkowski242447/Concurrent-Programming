namespace Logika
{
    internal class Ball
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

        private int getBallRadius()
        {
            return this.ballRadius;
        }

        /*
         * Setter methods.
         */

        internal void setVelocityVector(Position newVelocity)
        {
            this.velocityVector = newVelocity;
        }

        internal void MoveBall(int height, int width)
        {
            if (centerOfTheBall.getXCoordinate() + velocityVector.getXCoordinate() > width - getBallRadius() || 0 > centerOfTheBall.getXCoordinate() + velocityVector.getXCoordinate())
            {
                velocityVector.setXCoordinate(-velocityVector.getXCoordinate());

            }
            if (centerOfTheBall.getYCoordinate() + velocityVector.getYCoordinate() > height - getBallRadius() || 0 > centerOfTheBall.getYCoordinate() + velocityVector.getYCoordinate())
            {
                velocityVector.setYCoordinate(-velocityVector.getYCoordinate());

            }
            centerOfTheBall.setXCoordinate(velocityVector.getXCoordinate());
            centerOfTheBall.setYCoordinate(velocityVector.getYCoordinate());

        }
    }
}
