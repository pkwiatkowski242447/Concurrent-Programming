using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    internal class JSONSerializer : DataBallSerializer
    {
        private readonly string PathToLogFile;
        private Task? LoggingTask;
        private readonly ConcurrentQueue<JObject> SerializationQueue;
        private readonly object LogFileLockObject = new object();
        private readonly object BufferLockObject = new object();
        private readonly JArray LoggedBallsArray;

        public JSONSerializer()
        {
            string PathToTempFolder = Path.GetTempPath();
            PathToLogFile = PathToTempFolder + "BallLogFile.json";
            SerializationQueue = new ConcurrentQueue<JObject>();
            if (File.Exists(PathToLogFile))
            {
                try
                {
                    string inputLogFile = File.ReadAllText(PathToLogFile);
                    LoggedBallsArray = JArray.Parse(inputLogFile);
                    return;
                }
                catch (JsonReaderException)
                {
                    LoggedBallsArray = new JArray();
                }
            }

            LoggedBallsArray = new JArray();
            FileStream LogFile = File.Create(PathToLogFile);
            LogFile.Close();
        }

        public override void AddDataBallToSerializationQueue(DataBallInterface dataBall)
        {
            Monitor.Enter(BufferLockObject);
            try
            {
                JObject serilizedObject = JObject.FromObject(dataBall);
                serilizedObject["Time"] = DateTime.Now.ToString("HH:mm:ss");

                SerializationQueue.Enqueue(serilizedObject);

                if (LoggingTask == null || LoggingTask.IsCompleted)
                {
                    LoggingTask = Task.Factory.StartNew(WriteSerializedDataToFile);
                }
            }
            finally
            {
                Monitor.Exit(BufferLockObject);
            }
        }

        public override void AddBoardDataToSerializationQueue(DataBoardInterface dataBoard)
        {
            Monitor.Enter(BufferLockObject);
            try
            {
                JObject serializedBoard = JObject.FromObject(dataBoard);
                serializedBoard["Time"] = DateTime.Now.ToString("HH:mm:ss");

                SerializationQueue.Enqueue(serializedBoard);

                if (LoggingTask == null || LoggingTask.IsCompleted)
                {
                    LoggingTask = Task.Factory.StartNew(WriteSerializedDataToFile);
                }
            }
            finally
            {
                Monitor.Exit(BufferLockObject);
            }
        }

        private void WriteSerializedDataToFile()
        {
            while (SerializationQueue.TryDequeue(out JObject serilizedObject))
            {
                LoggedBallsArray.Add(serilizedObject);
            }

            string jsonString = JsonConvert.SerializeObject(LoggedBallsArray, Formatting.Indented);

            Monitor.Enter(LogFileLockObject);
            try
            {
                File.WriteAllText(PathToLogFile, jsonString);
            }
            finally
            {
                Monitor.Exit(LogFileLockObject);
            }
        }

        ~JSONSerializer()
        {
            Monitor.Enter(LogFileLockObject);
            Monitor.Exit(LogFileLockObject);
        }
    }
}
