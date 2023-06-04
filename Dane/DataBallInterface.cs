using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;

namespace Data
{
    public abstract class DataBallInterface : IObservable<DataBallInterface>, IDisposable
    {
        public abstract int IdOfTheBall { get; }
        public abstract DataPositionInterface CenterOfTheBall { get; } 
        public abstract DataPositionInterface VelocityVectorOfTheBall { get; set; }
        [JsonIgnore]
        public abstract bool StartBallMovement { get; set; }
        [JsonIgnore]
        public abstract bool DidBallCollide { get; set; }
        [JsonIgnore]
        public abstract CancellationTokenSource CancelDelay { get; set; }
        [JsonIgnore]
        public abstract double TimeToWait { get; set; }

        public static DataBallInterface CreateBall(int idOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall, DataBallSerializer? serializer)
        {
            return new Ball(idOfTheBall, centerOfTheBall, velocityVectorOfTheBall, serializer);
        }

        public abstract void Dispose();

        public abstract IDisposable Subscribe(IObserver<DataBallInterface> observer);

        private class Ball : DataBallInterface
        {
            public override int IdOfTheBall { get; }
            public override DataPositionInterface VelocityVectorOfTheBall { get; set; }
            public override bool StartBallMovement { get; set; }
            public override bool DidBallCollide { get; set; }
            public override CancellationTokenSource CancelDelay { get; set; }
            public override double TimeToWait { get; set; }

            public override DataPositionInterface CenterOfTheBall
            {
                get { return this.ActualCenterOfTheBall; }
            }

            private IObserver<DataBallInterface>? ObserverObject;
            private DataPositionInterface ActualCenterOfTheBall;
            private DataBallSerializer? SerializerObject;
            private bool StopTask = false;
            private int BaseWaitTime = 5;

            internal Ball(int idOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall, DataBallSerializer? serializer)
            {
                this.IdOfTheBall = idOfTheBall;
                this.ActualCenterOfTheBall = centerOfTheBall;
                this.VelocityVectorOfTheBall = velocityVectorOfTheBall;
                this.SerializerObject = serializer;
                this.StartBallMovement = false;
                this.DidBallCollide = false;
                this.CancelDelay = new CancellationTokenSource();
                Task.Run(BallMovement);
            }


            private async void BallMovement()
            {
                while (!this.StopTask)
                {
                    if (this.StartBallMovement)
                    {
                        this.TimeToWait = (double)(this.BaseWaitTime / this.VelocityVectorOfTheBall.VectorLength());
                        if (this.TimeToWait > 10)
                        {
                            this.TimeToWait = 10;
                        }

                        this.Move();

                        if (this.SerializerObject != null)
                        {
                            SerializerObject.AddDataBallToSerializationQueue(this);
                        }
                        if (this.ObserverObject != null)
                        {
                            this.ObserverObject.OnNext(this);
                        }

                        this.DidBallCollide = false;
                        await Task.Delay((int)this.TimeToWait, CancelDelay.Token).ContinueWith(_ => { });

                        if (CancelDelay.IsCancellationRequested)
                        {
                            CancelDelay.Dispose();
                            CancelDelay = new CancellationTokenSource();
                        }
                    }
                }
            }

            private void Move()
            {
                double newXCoordinate = this.CenterOfTheBall.XCoordinate + (this.VelocityVectorOfTheBall.XCoordinate * this.TimeToWait);
                double newYCoordinate = this.CenterOfTheBall.YCoordinate + (this.VelocityVectorOfTheBall.YCoordinate * this.TimeToWait);

                DataPositionInterface NewCenterOfTheBallPosition = DataPositionInterface.CreatePosition(newXCoordinate, newYCoordinate);
                this.SetCenterOfTheBall(NewCenterOfTheBallPosition);
            }

            public override void Dispose()
            {
                this.StopTask = true;
            }

            private void SetCenterOfTheBall(DataPositionInterface someOtherPosition)
            {
                this.ActualCenterOfTheBall = someOtherPosition;
            }

            public override IDisposable Subscribe(IObserver<DataBallInterface> observer)
            {
                this.ObserverObject = observer;
                return new ObserverManager(observer);
            }

            private class ObserverManager : IDisposable
            {
                IObserver<DataBallInterface>? SomeObserver;

                public ObserverManager(IObserver<DataBallInterface> observer)
                {
                    this.SomeObserver = observer;
                }

                public void Dispose()
                {
                    this.SomeObserver = null;
                }
            }
        }
    }
}