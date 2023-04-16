using Logika;

namespace Model
{
    public class ModelAPI : ModelAbstractAPI
    {
        internal LogicAbstractAPI LogicAPI;
        internal ModelAPI(int widthOfTheTable, int heightOfTheTable)
        {
            LogicAPI = LogicAbstractAPI.CreateLogicAPIInstace(widthOfTheTable, heightOfTheTable);
        }

        public override void AddSpecifiedNumberOfBalls(int numberOfBallsToAdd)
        {
            LogicAPI.AddSpecifiedNumerOfBalls(numberOfBallsToAdd);
        }

        public override void ClearPoolTable()
        {
            LogicAPI.ClearPoolTable();
        }

        public override void MoveGeneratedBalls()
        {
            LogicAPI.MoveGeneratedBalls();
        }
    }
}
