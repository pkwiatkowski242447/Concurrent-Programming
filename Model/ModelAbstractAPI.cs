using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public abstract class ModelAbstractAPI :  IDisposable
    {
        public static ModelAPI CreateModelAPI(int heightOfTheBoard, int widthOfTheBoard)
        {
            return new ModelAPI(heightOfTheBoard, widthOfTheBoard);
        }
        public abstract void CreateBalls(int howManyBalls);
        public abstract void MoveBalls();
        public abstract void Dispose();

    }
}
