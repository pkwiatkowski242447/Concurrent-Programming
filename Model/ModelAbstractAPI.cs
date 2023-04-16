using System;

namespace Model
{
    public abstract class ModelAbstractAPI : IObserver<int>, IObservable<BallInterface>
    {
        public static ModelAPI CreateModelAPI(int heightOfTheBoard, int widthOfTheBoard)
        {
            return new ModelAPI(heightOfTheBoard, widthOfTheBoard);
        }
        public abstract void MoveBalls(int howManyBalls);
        public abstract void ClearBoard();

        public abstract void OnCompleted();

        public abstract ModelBall GetModelBall(int value);

        public abstract void OnError(Exception error);

        public abstract void OnNext(int value);

        public abstract IDisposable Subscribe(IObserver<BallInterface> observer);

    }
}
