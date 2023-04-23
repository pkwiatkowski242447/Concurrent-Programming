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
             * Za logik� poruszania si� odpowiedzialna b�dzie, jak sama nazwa wskazuje, Logika
             * czyli w warstwie wy�ej dokonywa� b�dziemy korekt VelocityVectora - w wyniku tego 
             * w tej metodzie wystarczy tylko zmienia� po�o�enie.
             * 
             * Sam pomys� polega na tym: Dla ka�dej kulki generujemy losow� pozycj� na mapie, i nast�pnie
             * generowany jest wektor pr�dko�ci, kt�ry sprawia, �e kulka po ruchu znajduje si� w mapie. I dalej
             * opiera� si� to b�dzie na okresowym sprawdzaniu po�o�enia i je�eli pozycja kulki, a dok�adnie centrum
             * + promie� = 0 lub r�wna si� wysoko�� / szeroko�� to trzeba odwr�ci� jedynie odpowiedni� wsp�rz�dn�
             * wektora pr�dko�ci (w przypadku ektremalnym: obie - przy trafieniu w sam r�g).
             */
            CenterOfTheBall.XCoordinate = VelocityVector.XCoordinate + CenterOfTheBall.XCoordinate;
            CenterOfTheBall.YCoordinate = VelocityVector.YCoordinate + CenterOfTheBall.YCoordinate;

        }
    }
}