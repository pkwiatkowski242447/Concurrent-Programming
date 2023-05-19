using Logic;
using System;
using System.Collections.Generic;

namespace Model
{
    public abstract class ModelAbstractAPI : IObserver<int>, IObservable<ModelBallInterface>
    {
        public static ModelAbstractAPI CreateModelAPIInstance()
        {
            return new ModelAPI();
        }

        public abstract void CreateBalls(int SelectedNumberOfBalls);
        public abstract void ClearPoolTable();
        public abstract void StartBallMovement();
        public abstract ModelBallInterface GetModelBall(int indexValue);
        public abstract IDisposable Subscribe(IObserver<ModelBallInterface> ObserverObject);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int ballIndex);

        private class ModelAPI : ModelAbstractAPI
        {
            private readonly LogicAbstractAPI LogicAPI;
            private IObserver<ModelBallInterface>? BallObserver;
            private readonly IDisposable? ManagerOfObserver;
            private readonly List<ModelBallInterface> ListOfModelBalls = new List<ModelBallInterface>();
            public ModelAPI()
            {
                LogicAPI = LogicAbstractAPI.CreateLogicAPIInstance();
                ManagerOfObserver = LogicAPI.Subscribe(this);
            }

            public override void CreateBalls(int SelectedNumberOfBalls)
            {
                LogicAPI.CreatePlayingBoard();
                LogicAPI.CreateSpecifiedNumberOfBalls(SelectedNumberOfBalls);
                List<LogicBallInterface> ListOfLogicBalls = LogicAPI.GetListOfAllLogicBalls();
                for (int i = 0; i < ListOfLogicBalls.Count; i++)
                {
                    ListOfModelBalls.Add(ModelBallInterface.CreatModelBall(ListOfLogicBalls[i].BallCenter.YCoordinate, ListOfLogicBalls[i].BallCenter.XCoordinate, ListOfLogicBalls[i].BallRadius));
                }
            }

            public override void ClearPoolTable()
            {
                LogicAPI.ClearPoolTable();
                ListOfModelBalls.Clear();
                ManagerOfObserver?.Dispose();
            }

            public override void StartBallMovement()
            {
                LogicAPI.StartBallMovement();
            }

            public override ModelBallInterface GetModelBall(int indexValue)
            {
                return ListOfModelBalls[indexValue];
            }

            public override IDisposable Subscribe(IObserver<ModelBallInterface> Observer)
            {
                this.BallObserver = Observer;
                return new ObserverManager(Observer);
            }

            public override void OnCompleted()
            {
                ManagerOfObserver?.Dispose();
            }

            public override void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public override void OnNext(int ballIndex)
            {
                if (ballIndex < ListOfModelBalls.Count)
                {
                    ModelBallInterface ModelBall = ListOfModelBalls[ballIndex];
                    LogicBallInterface LogicBall = LogicAPI.GetCertainLogicBall(ballIndex);
                    ModelBall.Move(LogicBall.BallCenter.YCoordinate, LogicBall.BallCenter.XCoordinate);
                }
            }

            private class ObserverManager : IDisposable
            {
                public IObserver<ModelBallInterface>? ObserverToBeManaged;

                public ObserverManager(IObserver<ModelBallInterface> ObserverObject)
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