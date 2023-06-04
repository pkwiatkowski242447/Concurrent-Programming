using Data;

namespace Logic
{
    public abstract class LogicBallInterface
    {
        public abstract double BallMass { get; }
        public abstract double BallRadius { get; }
        public abstract LogicPositionInterface BallCenter { get; }
        public abstract LogicPositionInterface BallVelocity { get; set; }
        public abstract double TimeToWait { get; set; }

        public static LogicBallInterface CreateLogicBall(DataBallInterface dataBall, double MassOfTheBall, double RadiusOfTheBall)
        {
            LogicPositionInterface ballCenter = LogicPositionInterface.CreateLogicPosition(dataBall.CenterOfTheBall.XCoordinate, dataBall.CenterOfTheBall.YCoordinate);
            LogicPositionInterface ballVelocity = LogicPositionInterface.CreateLogicPosition(dataBall.VelocityVectorOfTheBall.XCoordinate, dataBall.VelocityVectorOfTheBall.YCoordinate);
            return new LogicBall(MassOfTheBall, RadiusOfTheBall, ballCenter, ballVelocity, dataBall.TimeToWait);
        }

        public abstract void UpdateLogicBall(DataBallInterface dataBall);

        private class LogicBall : LogicBallInterface
        {
            public override double BallMass { get; }
            public override double BallRadius { get; }
            public override LogicPositionInterface BallCenter { get => ActualBallCenter; }
            public override LogicPositionInterface BallVelocity { get; set; }
            public override double TimeToWait { get; set; }

            private LogicPositionInterface ActualBallCenter;

            internal LogicBall(double ballMass, double ballRadius, LogicPositionInterface ballCenter, LogicPositionInterface ballVelocity, double timeToWait)
            {
                this.BallMass = ballMass;
                this.BallRadius = ballRadius;
                this.ActualBallCenter = ballCenter;
                this.BallVelocity = ballVelocity;
                this.TimeToWait = timeToWait;
            }

            public override void UpdateLogicBall(DataBallInterface dataBall)
            {
                LogicPositionInterface BallCenter = LogicPositionInterface.CreateLogicPosition(dataBall.CenterOfTheBall.XCoordinate, dataBall.CenterOfTheBall.YCoordinate);
                this.SetBallCenter(BallCenter);
                this.TimeToWait = dataBall.TimeToWait;
            }

            private void SetBallCenter(LogicPositionInterface someOtherPosition)
            {
                this.ActualBallCenter = someOtherPosition;
            }
        }
    }
}