using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Logika
{
    public abstract class LogicAbstractAPI
    {
        public static LogicAbstractAPI CreateLogicAPIInstace(int width, int height, int numOfBalls)
        {
            return new PoolTable(width, height, numOfBalls);
        }

        public abstract void AddSpecifiedNumerOfBalls(int numberOfBallsToAdd);
        public abstract void ClearPoolTable();
        public abstract void MoveGeneratedBalls();
    }
}
