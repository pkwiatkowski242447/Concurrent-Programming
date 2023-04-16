using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Logika
{
    public abstract class LogicAbstractAPI
    {
        public static LogicAbstractAPI CreateLogicAPIInstace(int width, int height)
        {
            return new PoolTable(width, height);
        }

        public abstract void AddSpecifiedNumerOfBalls(int numberOfBallsToAdd);
        public abstract void ClearPoolTable();
        public abstract void MoveGeneratedBalls();
        public abstract List<Ball> GetBallsList();
    }
}
