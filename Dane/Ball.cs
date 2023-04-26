namespace Data
{
    public class Ball
    {
        public double MassOfTheBall { get; set; }
        public Position CenterOfTheBall { get; set; }
        public double BallRadius { get; set; }
        public Position VelocityVector { get; set; }

        public Ball(double massOfTheBall, Position centerOfTheBall, double ballRadius, Position velocityVector)
        {
            this.MassOfTheBall = massOfTheBall;
            this.CenterOfTheBall = centerOfTheBall;
            this.BallRadius = ballRadius;
            this.VelocityVector = velocityVector;
        }


        public void MoveBall()
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
            CenterOfTheBall.XCoordinate = VelocityVector.XCoordinate + CenterOfTheBall.XCoordinate;
            CenterOfTheBall.YCoordinate = VelocityVector.YCoordinate + CenterOfTheBall.YCoordinate;

        }
    }
}
