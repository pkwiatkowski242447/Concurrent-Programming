using System.Collections.Generic;

namespace Logika
{
    public abstract class LogicAbstractAPI
    {
        public static LogicAbstractAPI CreateApi(int height, int width)
        {
            return new Board(height, width);
        }
        public abstract void CreateBalls(int howManyBalls);
        public abstract void MoveBalls();
        public abstract void ClearBoard();
        public abstract List<Ball> GetBalls();
    }
}
