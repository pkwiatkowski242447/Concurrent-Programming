using Logic;
using System;
using System.Collections.Generic;

namespace Model
{
    public abstract class ModelAbstractAPI : IObserver<int>, IObservable<BallInterface>
    {
        public static ModelAbstractAPI CreateModelAPIInstance()
        {
            return new ModelAPI();
        }

        public abstract void CreateBalls(int SelectedNumberOfBalls);
        public abstract void ClearPoolTable();
        public abstract void OnCompleted();
        public abstract ModelBall GetModelBall(int indexValue);
        public abstract void OnError(Exception error);
        public abstract void OnNext(int indexValue);
        public abstract void MoveBalls();
        public abstract IDisposable Subscribe(IObserver<BallInterface> ObserverObject);

        private class ModelAPI : ModelAbstractAPI
        {
            private readonly LogicAbstractAPI LogicAPI;
            private IObserver<BallInterface>? BallObserver;
            private readonly IDisposable? ManagerOfObserver;
            private readonly List<ModelBall> ListOfModelBalls = new List<ModelBall>();
            internal ModelAPI()
            {
                LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance();
                ManagerOfObserver = LogicAPI.Subscribe(this);
            }

            public override void CreateBalls(int SelectedNumberOfBalls)
            {
                LogicAPI.CreateSpecifiedNumerOfBalls(SelectedNumberOfBalls);
                List<List<int>> ListFromLogic = LogicAPI.GetAllBallsCoordinates();
                for (int i = 0; i < ListFromLogic.Count; i++)
                {
                    List<int> BallObjectCoordinates = ListFromLogic[i];
                    ListOfModelBalls.Add(new ModelBall(BallObjectCoordinates[1], BallObjectCoordinates[0], BallObjectCoordinates[2]));
                }
            }

            public override void MoveBalls()
            {
                LogicAPI.MoveGeneratedBalls();
            }

            public override void ClearPoolTable()
            {
                LogicAPI.ClearPoolTable();
                ListOfModelBalls.Clear();
                ManagerOfObserver?.Dispose();
            }

            public override ModelBall GetModelBall(int indexValue)
            {
                return ListOfModelBalls[indexValue];
            }

            public override void OnCompleted()
            {
                ManagerOfObserver?.Dispose();
            }

            public override void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public override void OnNext(int indexValue)
            {
                if (indexValue < ListOfModelBalls.Count)
                {
                    ModelBall ball = ListOfModelBalls[indexValue];
                    List<int> ballObjectCoordinates = LogicAPI.GetAllBallsCoordinates()[indexValue];
                    ball.Move(ballObjectCoordinates[1] + ballObjectCoordinates[4], ballObjectCoordinates[0] + ballObjectCoordinates[3]);
                }
            }

            public override IDisposable Subscribe(IObserver<BallInterface> Observer)
            {
                this.BallObserver = Observer;
                return new ObserverManager(Observer);
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