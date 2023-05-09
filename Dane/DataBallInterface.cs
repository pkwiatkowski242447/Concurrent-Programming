namespace Data
{
    public abstract class DataBallInterface
    {
        public abstract double MassOfTheBall { get; set; }
        public abstract double RadiusOfTheBall { get; set; }
        public abstract DataPositionInterface CenterOfTheBall { get; set; }
        public abstract DataPositionInterface VelocityVectorOfTheBall { get; set; }

        public static DataBallInterface CreateBall(double massOfTheBall, double radiusOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall)
        {
            return new Ball(massOfTheBall, radiusOfTheBall, centerOfTheBall, velocityVectorOfTheBall);
        }

        public abstract void Move();

        private class Ball : DataBallInterface 
        {
            public override double MassOfTheBall { get; set; }
            public override double RadiusOfTheBall { get; set; }
            public override DataPositionInterface CenterOfTheBall { get; set; }
            public override DataPositionInterface VelocityVectorOfTheBall { get; set; }

            public Ball(double massOfTheBall, double radiusOfTheBall, DataPositionInterface centerOfTheBall, DataPositionInterface velocityVectorOfTheBall)
            {
                this.MassOfTheBall = massOfTheBall;
                this.RadiusOfTheBall = radiusOfTheBall;
                this.CenterOfTheBall = centerOfTheBall;
                this.VelocityVectorOfTheBall = velocityVectorOfTheBall;
            }

            public override void Move()
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
        }
    }
}
