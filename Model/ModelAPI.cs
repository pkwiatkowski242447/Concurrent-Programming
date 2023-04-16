using Logika;
using System;
using System.Collections.Generic;

namespace Model { 
    public class ModelAPI : ModelAbstractAPI
    {
        private LogicAbstractAPI LogicAPI;
        private IObserver<BallInterface>? BallObserver;
        private IDisposable? ManagerOfObserver;
        private List<ModelBall> ListOfBalls = new List<ModelBall>();
        internal ModelAPI(int widthOfTheTable, int heightOfTheTable)
        {
            LogicAPI = LogicAbstractAPI.CreateLogicAPIInstace(widthOfTheTable, heightOfTheTable);
            ManagerOfObserver = LogicAPI.Subscribe(this);
        }

        public override void MoveGeneratedBalls(int SelectedNumberOfBalls)
        {
            LogicAPI.AddSpecifiedNumerOfBalls(SelectedNumberOfBalls);
            List<Ball> listFromLogic = LogicAPI.GetBallsList();
            for (int i = 0; i < listFromLogic.Count; i++)
            {
                Ball ballObject = listFromLogic[i];
                ListOfBalls.Add(new ModelBall(ballObject.centerOfTheBall.xCoordinate, ballObject.centerOfTheBall.yCoordinate, ballObject.ballRadius));
            }
            LogicAPI.MoveGeneratedBalls();
        }

        public override void ClearPoolTable()
        {
            LogicAPI.ClearPoolTable();
            ListOfBalls.Clear();
            ManagerOfObserver?.Dispose();
        }

        public override ModelBall GetModelBall(int value)
        {
            return ListOfBalls[value];
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
            if (value < ListOfBalls.Count)
            {
                ModelBall ball = ListOfBalls[value];
                Ball ballObject = LogicAPI.GetBallsList()[value];
                Position velocityVector = ballObject.velocityVector;
                Position currentCenter = ballObject.centerOfTheBall;
                ball.Move(velocityVector.yCoordinate + currentCenter.yCoordinate, velocityVector.xCoordinate + currentCenter.xCoordinate);
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
