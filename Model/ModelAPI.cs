using Logika;
using System;
using System.Collections.Generic;

namespace Model { 
    public class ModelAPI : ModelAbstractAPI
    {
        private int SelectedNumberOfBalls = 0;
        private LogicAbstractAPI LogicAPI;
        private IObserver<BallInterface>? BallObserver;
        private ObserverManager? ManagerOfObserver;
        private List<ModelBall> listOfBalls = new List<ModelBall>();
        internal ModelAPI(int widthOfTheTable, int heightOfTheTable)
        {
            LogicAPI = LogicAbstractAPI.CreateLogicAPIInstace(widthOfTheTable, heightOfTheTable);
            ManagerOfObserver = LogicAPI.Subscribe(this);
        }

        public override void AddSpecifiedNumberOfBalls()
        {
            LogicAPI.AddSpecifiedNumerOfBalls(SelectedNumberOfBalls);
        }

        public override void MoveGeneratedBalls()
        {
            LogicAPI.AddSpecifiedNumerOfBalls(SelectedNumberOfBalls);
            LogicAPI.MoveGeneratedBalls();
        }

        public override void ClearPoolTable()
        {
            LogicAPI.ClearPoolTable();
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
