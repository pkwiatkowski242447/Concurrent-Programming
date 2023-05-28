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
        private readonly Mutex LogFileMutex = new Mutex();
        private readonly Mutex BallsMutex = new Mutex();
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
            BallsMutex.WaitOne();
            try
            {
                JObject serilizedObject = JObject.FromObject(dataBall);
                serilizedObject["Time"] = DateTime.Now.ToString("HH:mm:ss");

                SerializationQueue.Enqueue(serilizedObject);

                if (LoggingTask == null || LoggingTask.IsCompleted)
                {
                    LoggingTask = Task.Factory.StartNew(WriteBallDataToFile);
                }
            }
            finally
            {
                BallsMutex.ReleaseMutex();
            }
        }

        private void WriteBallDataToFile()
        {
            while (SerializationQueue.TryDequeue(out JObject serializedBall))
            {
                LoggedBallsArray.Add(serializedBall);
            }

            string jsonString = JsonConvert.SerializeObject(LoggedBallsArray, Formatting.Indented);

            LogFileMutex.WaitOne();
            try
            {
                File.WriteAllText(PathToLogFile, jsonString);
            }
            finally
            {
                LogFileMutex.ReleaseMutex();
            }
        }

        ~JSONSerializer()
        {
            LogFileMutex.WaitOne();
            LogFileMutex.ReleaseMutex();
        }
    }
}
