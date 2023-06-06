using System;

namespace Data
{
    public abstract class DataBallSerializer : IDisposable
    {
        public abstract void AddDataBallToSerializationQueue(SerializationObject BallObject);
        public abstract void AddBoardDataToSerializationQueue(SerializationObject BoardObject);

        public static DataBallSerializer CreateJSONSerializer()
        {
            return new JSONSerializer();
        }

        public abstract void Dispose();
    }
}
