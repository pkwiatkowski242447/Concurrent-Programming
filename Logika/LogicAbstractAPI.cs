using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Logika
{
    public abstract class LogicAbstractAPI : IObservable<int>
    {
        public static LogicAbstractAPI CreateLogicAPIInstace(int widthOfTheTable, int heightOfTheTable)
        {
            return new PoolTable(widthOfTheTable, heightOfTheTable);
        }

        public abstract void AddSpecifiedNumerOfBalls(int numberOfBallsToAdd);
        public abstract void ClearPoolTable();
        public abstract void MoveGeneratedBalls();
        public abstract List<Ball> GetBallsList();
        public abstract IDisposable Subscribe(IObserver<int> observer);
    }
}
