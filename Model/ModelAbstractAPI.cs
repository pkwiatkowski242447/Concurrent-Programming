using Logic;
using System;
using System.Collections.Generic;

namespace Model
{
    public abstract class ModelAbstractAPI : IObserver<int>, IObservable<BallInterface>
    {
        public static ModelAbstractAPI CreateModelApi()
        {
            return new ModelAPI();
        }
        public abstract void MoveBalls();
        public abstract void CreateBalls(int howManyBalls);
        public abstract void ClearPoolTable();

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
            private List<ModelBall> ListOfModelBalls = new List<ModelBall>();
            internal ModelAPI()
            {
                LogicAPI = LogicAbstractAPI.CreateLogicApi();
                ManagerOfObserver = LogicAPI.Subscribe(this);
            }

            public override void CreateBalls(int SelectedNumberOfBalls)
            {
                LogicAPI.CreateSpecifiedNumerOfBalls(SelectedNumberOfBalls);
                List<List<double>> ListFromLogic = LogicAPI.GetAllBallsCoordinates();
                for (int i = 0; i < ListFromLogic.Count; i++)
                {
                    List<double> BallObjectCoordinates = ListFromLogic[i];
                    ListOfModelBalls.Add(new ModelBall(BallObjectCoordinates[1], BallObjectCoordinates[0], BallObjectCoordinates[2]));
                }
            }

            public override void ClearPoolTable()
            {
                LogicAPI.ClearPoolTable();
                ListOfModelBalls.Clear();
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
                if (value < ListOfModelBalls.Count)
                {
                    ModelBall ball_model = ListOfModelBalls[value];
                    List<double> ball = LogicAPI.GetAllBallsCoordinates()[value];
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
                return ListOfModelBalls[value];
            }

            public override void MoveBalls()
            {
                LogicAPI.MoveBalls();
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
