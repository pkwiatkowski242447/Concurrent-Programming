using Dane;
using Logika;
using System;
using System.Collections.Generic;

namespace Model
{
    public abstract class ModelAbstractAPI : IObserver<int>, IObservable<BallInterface>
    {
        public static ModelAbstractAPI CreateModelAPI()
        {
            return new ModelAPI();
        }
        public abstract void MoveBalls();
        public abstract void CreateBalls(int howManyBalls);
        public abstract void ClearBoard();

        public abstract void OnCompleted();

        public abstract ModelBall GetModelBall(int value);

        public abstract void OnError(Exception error);

        public abstract void OnNext(int value);

        public abstract IDisposable Subscribe(IObserver<BallInterface> observer);
        private class ModelAPI : ModelAbstractAPI
        {
            private LogicAbstractAPI LogicAPI;
            private IObserver<BallInterface>? BallObserver;
            private IDisposable? ManagerOfObserver;
            private List<ModelBall> listOfBalls = new List<ModelBall>();
            internal ModelAPI()
            {
                LogicAPI = LogicAbstractAPI.CreateApi();
                ManagerOfObserver = LogicAPI.Subscribe(this);
            }

            public override void CreateBalls(int howManyBalls)
            {
                LogicAPI.CreateBalls(howManyBalls);
                List<List<int>> listFromLogic = LogicAPI.GetBalls();
                for (int i = 0; i < listFromLogic.Count; i++)
                {
                    List<int> ball = listFromLogic[i];
                    listOfBalls.Add(new ModelBall(ball[1], ball[0], ball[2]));
                }
                LogicAPI.MoveBalls();
            }

            public override void ClearBoard()
            {
                LogicAPI.ClearBoard();
                listOfBalls.Clear();
                ManagerOfObserver?.Dispose();
            }

            public override void OnCompleted()
            {
                ManagerOfObserver?.Dispose();
            }

            public override void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public override void OnNext(int value)
            {
                if (value < listOfBalls.Count)
                {
                    ModelBall ball_model = listOfBalls[value];
                    List<int> ball = LogicAPI.GetBalls()[value];
                    ball_model.Move(ball[1] + ball[4], ball[0] + ball[3]);
                }

            }

            public override IDisposable Subscribe(IObserver<BallInterface> Observer)
            {
                this.BallObserver = Observer;
                return new ObserverManager(Observer);
            }

            public override ModelBall GetModelBall(int value)
            {
                return listOfBalls[value];
            }

            public override void MoveBalls()
            {
                throw new NotImplementedException();
            }

            private class ObserverManager : IDisposable
            {
                public IObserver<BallInterface>? ObserverToBeManaged;

                public ObserverManager(IObserver<BallInterface> ObserverObject)
                {
                    ObserverToBeManaged = ObserverObject;
                }

                public void Dispose()
                {
                    ObserverToBeManaged = null;
                }
            }

        }
    }
}
