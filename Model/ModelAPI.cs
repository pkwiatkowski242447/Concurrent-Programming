using Logika;
using System;
using System.Collections.Generic;

namespace Model
{
    public class ModelAPI : ModelAbstractAPI
    {
        private LogicAbstractAPI LogicAPI;
        private IObserver<BallInterface>? BallObserver;
        private IDisposable? ManagerOfObserver;
        private List<ModelBall> listOfBalls = new List<ModelBall>();
        internal ModelAPI(int widthOfTheTable, int heightOfTheTable)
        {
            LogicAPI = LogicAbstractAPI.CreateApi(widthOfTheTable, heightOfTheTable);
            ManagerOfObserver = LogicAPI.Subscribe(this);
        }

        public override void MoveBalls(int howManyBalls)
        {
            LogicAPI.CreateBalls(howManyBalls);
            List<Ball> listFromLogic = LogicAPI.GetBalls();
            for (int i = 0; i < listFromLogic.Count; i++)
            {
                Ball ball = listFromLogic[i];
                listOfBalls.Add(new ModelBall(ball.centerOfTheBall.xCoordinate,ball.centerOfTheBall.yCoordinate, ball.ballRadius));
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
            if (value < listOfBalls.Count) { 
                ModelBall ball_model = listOfBalls[value];
                Ball ball = LogicAPI.GetBalls()[value];
                Position position_center = ball.centerOfTheBall;
                Position position_velocity = ball.velocityVector;
                ball_model.Move(position_center.xCoordinate + position_velocity.xCoordinate, position_center.yCoordinate + position_velocity.yCoordinate);
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