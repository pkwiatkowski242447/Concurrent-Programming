using System;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Threading;

namespace Data
{
    public abstract class DataBallInterface : IObservable<DataBallInterface>, IDisposable
    {
        public abstract int IdOfTheBall { get; }
        [JsonIgnore] 
        public abstract double MassOfTheBall { get; }
        public abstract DataPositionInterface CenterOfTheBall { get; }
        [JsonIgnore] 
        public abstract DataPositionInterface VelocityVectorOfTheBall { get; set; }
        [JsonIgnore]
        public abstract bool StartBallMovement { get; set; }
        [JsonIgnore]
        public abstract bool DidBallCollide { get; set; }
        [JsonIgnore]
        public abstract CancellationTokenSource CancelDelay { get; set; }
        [JsonIgnore]
        public abstract double TimeToWait { get; set; }

        public static DataBallInterface CreateBall(int idOfTheBall, double massOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall, DataBallSerializer? serializer)
        {
            return new Ball(idOfTheBall, massOfTheBall, centerOfTheBall, velocityVectorOfTheBall, serializer);
        }

        public abstract void Dispose();

        public abstract IDisposable Subscribe(IObserver<DataBallInterface> observer);

        private class Ball : DataBallInterface
        {
            public override int IdOfTheBall { get; }
            public override double MassOfTheBall { get; }
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

            internal Ball(int idOfTheBall, double massOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall, DataBallSerializer? serializer)
            {
                this.IdOfTheBall = idOfTheBall;
                this.MassOfTheBall = massOfTheBall;
                this.ActualCenterOfTheBall = centerOfTheBall;
                this.VelocityVectorOfTheBall = velocityVectorOfTheBall;
                this.SerializerObject = serializer;
                this.StartBallMovement = false;
                this.DidBallCollide = false;
                this.TimeToWait = 1;
                this.CancelDelay = new CancellationTokenSource();
                Task.Run(BallMovement);
            }


            private async void BallMovement()
            {
                while (!this.StopTask)
                {
                    if (this.StartBallMovement)
                    {
                        this.Move();
                        if (this.ObserverObject != null)
                        {
                            this.ObserverObject.OnNext(this);
                        }
                        this.DidBallCollide = false;
                    }
                    if (this.SerializerObject != null)
                    {
                        SerializerObject.AddDataBallToSerializationQueue(this);
                    }
                    await Task.Delay((int)this.TimeToWait, CancelDelay.Token).ContinueWith(_ => { });
                }
            }

            private void Move()
            {
                /*
                 * Za logikę poruszania się odpowiedzialna będzie, jak sama nazwa wskazuje, Logika
                 * czyli w warstwie wyżej dokonywać będziemy korekt VelocityVectora - w wyniku tego 
                 * w tej metodzie wystarczy tylko zmieniać położenie.
                 * 
                 * Sam pomysł polega na tym: Dla każdej kulki generujemy losową pozycję na mapie, i następnie
                 * generowany jest wektor prędkości, który sprawia, że kulka po ruchu znajduje się w mapie. I dalej
                 * opierać się to będzie na okresowym sprawdzaniu położenia i jeżeli pozycja kulki, a dokładnie centrum
                 * + promień = 0 lub równa się wysokość / szerokość to trzeba odwrócić jedynie odpowiednią współrzędną
                 * wektora prędkości (w przypadku ektremalnym: obie - przy trafieniu w sam róg).
                 */

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