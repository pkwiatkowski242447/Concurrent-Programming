using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
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
        private readonly object BufferLockObject = new object();
        private readonly JArray LoggedBallsArray;
        private bool StopTask;

        public JSONSerializer()
        {
            string PathToTempFolder = Path.GetTempPath();
            PathToLogFile = PathToTempFolder + "BallLogFile.json";
            SerializationQueue = new ConcurrentQueue<JObject>();
            
            FileStream LogFile = File.Create(PathToLogFile);
            LogFile.Close();

            LoggedBallsArray = new JArray();
            this.StopTask = false;
            Task.Run(WriteSerializedDataToFile);
        }

        public override void AddDataBallToSerializationQueue(DataBallInterface dataBall)
        {
            Monitor.Enter(BufferLockObject);
            try
            {
                JObject serilizedObject = JObject.FromObject(dataBall);
                serilizedObject["Time"] = DateTime.Now.ToString("HH:mm:ss");

                SerializationQueue.Enqueue(serilizedObject);
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
            }
            finally
            {
                Monitor.Exit(BufferLockObject);
            }
        }

        private async void WriteSerializedDataToFile()
        {
            string jsonString;
            while (!this.StopTask)
            {
                if (!SerializationQueue.IsEmpty)
                {
                    while (SerializationQueue.TryDequeue(out JObject serilizedObject))
                    {
                        LoggedBallsArray.Add(serilizedObject);
                    }

                    jsonString = JsonConvert.SerializeObject(LoggedBallsArray, Formatting.Indented);
                    LoggedBallsArray.Clear();

                    await File.AppendAllTextAsync(PathToLogFile, jsonString);
                }
            }
        }

        public override void Dispose()
        {
            this.StopTask = true;
        }

        ~JSONSerializer()
        {
            Monitor.Enter(BufferLockObject);
            Monitor.Exit(BufferLockObject);
            this.Dispose();
        }
    }
}
