namespace Data
{
    public abstract class DataBallSerializer
    {
        public abstract void AddDataBallToSerializationQueue(DataBallInterface dataBall);

        public static DataBallSerializer CreateJSONSerializer()
        {
            return new JSONSerializer();
        }
    }
}
