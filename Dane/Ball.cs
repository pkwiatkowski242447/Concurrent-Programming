namespace Data
{
    public class Ball
    {
        public double MassOfTheBall { get; set; }
        public Position CenterOfTheBall { get; set; }
        public int BallRadius { get; set; }
        public Position VelocityVector { get; set; }

        public Ball(double massOfTheBall, Position centerOfTheBall, int ballRadius, Position velocityVector)
        {
            this.MassOfTheBall = massOfTheBall;
            this.CenterOfTheBall = centerOfTheBall;
            this.BallRadius = ballRadius;
            this.VelocityVector = velocityVector;
        }


        public void MoveBall()
        {
            /*
             * Za logikê poruszania siê odpowiedzialna bêdzie, jak sama nazwa wskazuje, Logika
             * czyli w warstwie wy¿ej dokonywaæ bêdziemy korekt VelocityVectora - w wyniku tego 
             * w tej metodzie wystarczy tylko zmieniaæ po³o¿enie.
             * 
             * Sam pomys³ polega na tym: Dla ka¿dej kulki generujemy losow¹ pozycjê na mapie, i nastêpnie
             * generowany jest wektor prêdkoœci, który sprawia, ¿e kulka po ruchu znajduje siê w mapie. I dalej
             * opieraæ siê to bêdzie na okresowym sprawdzaniu po³o¿enia i je¿eli pozycja kulki, a dok³adnie centrum
             * + promieñ = 0 lub równa siê wysokoœæ / szerokoœæ to trzeba odwróciæ jedynie odpowiedni¹ wspó³rzêdn¹
             * wektora prêdkoœci (w przypadku ektremalnym: obie - przy trafieniu w sam róg).
             */
            CenterOfTheBall.XCoordinate = VelocityVector.XCoordinate + CenterOfTheBall.XCoordinate;
            CenterOfTheBall.YCoordinate = VelocityVector.YCoordinate + CenterOfTheBall.YCoordinate;

        }
    }
}