namespace Calculator
{
    public class Kalkulator
    {
        public int Add(int x, int y)
        {
            return x + y;
        }

        public int Sub(int x, int y)
        {
            return x - y;
        }

        public int Mul(int x, int y)
        {
            return x * y;
        }

        public int Div(int x, int y)
        {
            if (x == 0 && y == 0)
            {
                throw new NotImplementedException("Operacja dzielenia przez 0 nie jest zdefiniowana.");
            }
            else
            {
                return x / y;
            }
        }

    }
}