using Model;

namespace ViewModel
{
    public class UserActionHandler
    {
        internal ModelAbstractAPI? ModelAPI;

        public UserActionHandler()
        {

        }

        public void CreateModelAPI(int widthOfTheTable, int heightOfTheTable)
        {
            ModelAPI = ModelAbstractAPI.CreateModelAPI(widthOfTheTable, heightOfTheTable);
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