using Model;

namespace ViewModel
{
    public class UserActionHandler
    {
        internal int widthOfTheTable;
        internal int heightOfTheTable;
        internal ModelAbstractAPI? ModelAPI;
        internal int numberOfSelectedBalls;

        public UserActionHandler()
        {

        }

        public void CreateModelAPI(int widthOfTheTable, int heightOfTheTable)
        {
            ModelAPI = ModelAbstractAPI.CreateModelAPIInstance(widthOfTheTable, heightOfTheTable);
        }

        public void StartSimulation()
        {
            
        }

        public void EndSimulation()
        {

        }

        public void AddBalls()
        {

        }

        public void RemoveBalls()
        {

        }
    }
}