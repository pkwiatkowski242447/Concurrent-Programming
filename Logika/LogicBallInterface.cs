using Data;

namespace Logic
{
    public abstract class LogicBallInterface
    {
        public abstract double BallMass { get; }
        public abstract double BallRadius { get; }
        public abstract LogicPositionInterface BallCenter { get; }
        public abstract LogicPositionInterface BallVelocity { get; set; }

        public static LogicBallInterface CreateLogicBall(DataBallInterface dataBall, double ballRadius)
        {
            LogicPositionInterface ballCenter = LogicPositionInterface.CreateLogicPosition(dataBall.CenterOfTheBall.XCoordinate, dataBall.CenterOfTheBall.YCoordinate);
            LogicPositionInterface ballVelocity = LogicPositionInterface.CreateLogicPosition(dataBall.VelocityVectorOfTheBall.XCoordinate, dataBall.VelocityVectorOfTheBall.YCoordinate);
            return new LogicBall(dataBall.MassOfTheBall, ballRadius, ballCenter, ballVelocity);
        }

        public abstract void UpdateLogicBall(DataBallInterface dataBall);

        private class LogicBall : LogicBallInterface
        {
            public override double BallMass { get; }
            public override double BallRadius { get; }
            public override LogicPositionInterface BallCenter { get => ActualBallCenter; }
            public override LogicPositionInterface BallVelocity { get; set; }

            private LogicPositionInterface ActualBallCenter;

            internal LogicBall(double ballMass, double ballRadius, LogicPositionInterface ballCenter, LogicPositionInterface ballVelocity)
            {
                this.BallMass = ballMass;
                this.BallRadius = ballRadius;
                this.ActualBallCenter = ballCenter;
                this.BallVelocity = ballVelocity;
            }

            public override void UpdateLogicBall(DataBallInterface dataBall)
            {
                LogicPositionInterface BallCenter = LogicPositionInterface.CreateLogicPosition(dataBall.CenterOfTheBall.XCoordinate, dataBall.CenterOfTheBall.YCoordinate);
                this.SetBallCenter(BallCenter);
            }

            private void SetBallCenter(LogicPositionInterface someOtherPosition)
            {
                this.ActualBallCenter = someOtherPosition;
            }
        }
    }
}
