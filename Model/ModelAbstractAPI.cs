using Logika;
using System;
using System.ComponentModel;

namespace Model
{
    public abstract class ModelAbstractAPI : IObserver<int>, IObservable<BallInterface>
    {
        public static ModelAbstractAPI CreateModelAPIInstance(int widthOfTheTable, int heightOfTheTable)
        {
            return new ModelAPI(widthOfTheTable, heightOfTheTable);
        }

        public abstract void AddSpecifiedNumberOfBalls();
        public abstract void MoveGeneratedBalls();
        public abstract void ClearPoolTable();

        public abstract void OnCompleted();

        public abstract void OnError(Exception error);

        public abstract void OnNext(int value);

        public abstract IDisposable Subscribe(IObserver<BallInterface> observer);
    }
}