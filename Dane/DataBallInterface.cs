using System;
using System.Threading.Tasks;

namespace Data
{
    public abstract class DataBallInterface : IObservable<DataBallInterface>
    {
        public abstract double MassOfTheBall { get; set; }
        public abstract DataPositionInterface CenterOfTheBall { get; }
        public abstract DataPositionInterface VelocityVectorOfTheBall { get; set; }
        public abstract bool StopTask { get; set; }
        public abstract bool StartBallMovement { get; set; }
        public abstract bool DidBallCollide { get; set; }

        public static DataBallInterface CreateBall(double massOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall)
        {
            return new Ball(massOfTheBall, centerOfTheBall, velocityVectorOfTheBall);
        }

        public abstract IDisposable Subscribe(IObserver<DataBallInterface> observerObject);

        private class Ball : DataBallInterface
        {
            public override double MassOfTheBall { get; set; }
            public override DataPositionInterface CenterOfTheBall { get; }
            public override DataPositionInterface VelocityVectorOfTheBall { get; set; }
            public override bool StopTask { get; set; }
            public override bool StartBallMovement { get; set; }
            public override bool DidBallCollide { get; set; }

            internal IObserver<DataBallInterface>? ObserverObject;

            public Ball(double massOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall)
            {
                this.MassOfTheBall = massOfTheBall;
                this.CenterOfTheBall = centerOfTheBall;
                this.VelocityVectorOfTheBall = velocityVectorOfTheBall;
                this.StopTask = false;
                this.StartBallMovement = false;
                this.DidBallCollide = false;
                Task.Run(BallMovement);
            }

            public async void BallMovement()
            {
                while (!this.StopTask)
                {
                    if (this.StartBallMovement)
                    {
                        this.Move();
                    }
                    if (this.ObserverObject != null)
                    {
                        this.ObserverObject.OnNext(this);
                    }
                    this.DidBallCollide = false;
                    await Task.Delay(1);
                }
            }

            public void Move()
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

                this.CenterOfTheBall.XCoordinate += this.VelocityVectorOfTheBall.XCoordinate;
                this.CenterOfTheBall.YCoordinate += this.VelocityVectorOfTheBall.YCoordinate;
            }

            public override IDisposable Subscribe(IObserver<DataBallInterface> observerObject)
            {
                this.ObserverObject = observerObject;
                return new ObserverManager(observerObject);
            }

            private class ObserverManager : IDisposable
            {
                IObserver<DataBallInterface>? SomeObserver;

                public ObserverManager(IObserver<DataBallInterface> observerObject)
                {
                    this.SomeObserver = observerObject;
                }

                public void Dispose()
                {
                    this.SomeObserver = null;
                }
            }
        }
    }
}
