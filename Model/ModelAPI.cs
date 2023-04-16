using Logika;
using System;

namespace Model
{
    public class ModelAPI : ModelAbstractAPI
    {
        public LogicAbstractAPI LogicAPI;
        public ModelAPI(int heightOfTheBoard, int widthOfTheBoard)
        {
            LogicAPI = LogicAbstractAPI.CreateApi(heightOfTheBoard, widthOfTheBoard);
        }


        public override void CreateBalls(int howManyBalls)
        {
            LogicAPI.CreateBalls(howManyBalls);
        }

        public override void Dispose()
        {
            LogicAPI.ClearBoard();
        }

        public override void MoveBalls()
        {
            LogicAPI.MoveBalls();
        }

    }
}