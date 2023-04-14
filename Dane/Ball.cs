

namespace Dane
{
    public class Ball
    {
        private double massOfTheBall;
        private Position centerOfTheBall;
        private int ballRadius;
        private Position velocityVector;

        /*
         * Constructor used to create ball object.
         */

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

        public double getBallMass()
        {
            return this.massOfTheBall;
        }

        public Position getCenterOfTheBall()
        {
            return this.centerOfTheBall;
        }

        public int getBallRadius()
        {
            return this.ballRadius;
        }

        public Position getVelocityVector()
        {
            return this.velocityVector;
        }

        /*
         * Setter methods.
         */

        public void setCenterOfTheBall(Position newCenter)
        {
            this.centerOfTheBall = newCenter;
        }

        public void setVelocityVector(Position newVelocity)
        {
            this.velocityVector = newVelocity;
        }

    }
}