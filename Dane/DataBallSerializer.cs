namespace Data
{
    public abstract class DataBallSerializer
    {
        public abstract void AddDataBallToSerializationQueue(DataBallInterface dataBall);
        public abstract void AddBoardDataToSerializationQueue(DataBoardInterface dataBoard);

        public static DataBallSerializer CreateJSONSerializer()
        {
            return new JSONSerializer();
        }
    }
}
