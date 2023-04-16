using Logika;

namespace Model
{
    public abstract class ModelAbstractAPI
    {
        public static ModelAPI CreateModelAPI(int widthOfTheTable, int heightOfTheTable)
        {
            return new ModelAPI(widthOfTheTable, heightOfTheTable);
        }

        public abstract void AddSpecifiedNumberOfBalls(int numberOfBallsToAdd);
        public abstract void ClearPoolTable();
        public abstract void MoveGeneratedBalls();
    }
}