using System;
using System.Threading.Tasks;

namespace Data
{
    public abstract class DataBallInterface : IObservable<DataBallInterface>
    {
        public abstract double MassOfTheBall { get; }
        public abstract DataPositionInterface CenterOfTheBall { get; }
        public abstract DataPositionInterface VelocityVectorOfTheBall { get; set; }
        public abstract bool StopTask { get; set; }
        public abstract bool StartBallMovement { get; set; }
        public abstract bool DidBallCollide { get; set; }

        public static DataBallInterface CreateBall(double massOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall)
        {
            return new Ball(massOfTheBall, centerOfTheBall, velocityVectorOfTheBall);
        }
        public abstract IDisposable Subscribe(IObserver<DataBallInterface> observer);

        private class Ball : DataBallInterface
        {
            public override double MassOfTheBall { get; }
            public override DataPositionInterface VelocityVectorOfTheBall { get; set; }
            public override bool StopTask { get; set; }
            public override bool StartBallMovement { get; set; }
            public override bool DidBallCollide { get; set; }

            public override DataPositionInterface CenterOfTheBall
            {
                get { return this.ActualCenterOfTheBall; }
            }

            internal IObserver<DataBallInterface>? ObserverObject;
            private DataPositionInterface ActualCenterOfTheBall;

            internal Ball(double massOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall)
            {
                this.MassOfTheBall = massOfTheBall;
                this.ActualCenterOfTheBall = centerOfTheBall;
                this.VelocityVectorOfTheBall = velocityVectorOfTheBall;
                this.StopTask = false;
                this.StartBallMovement = false;
                this.DidBallCollide = false;
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
                    await Task.Delay(1);
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

                double newXCoordinate = this.CenterOfTheBall.XCoordinate + this.VelocityVectorOfTheBall.XCoordinate;
                double newYCoordinate = this.CenterOfTheBall.YCoordinate + this.VelocityVectorOfTheBall.YCoordinate;

                DataPositionInterface NewCenterOfTheBallPosition = DataPositionInterface.CreatePosition(newXCoordinate, newYCoordinate);
                this.SetCenterOfTheBall(NewCenterOfTheBallPosition);
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