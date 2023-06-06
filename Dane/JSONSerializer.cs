using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace Data
{
    internal class JSONSerializer : DataBallSerializer
    {
        private readonly string PathToLogFile;
        private readonly ConcurrentQueue<SerializationObject> SerializationQueue;
        private readonly JArray BufferToWrite = new JArray();
        private readonly int QueueSize = 50;
        private CancellationTokenSource StateChange = new CancellationTokenSource();
        private bool StopTask;

        public JSONSerializer()
        {
            string PathToTempFolder = Path.GetTempPath();
            PathToLogFile = PathToTempFolder + "SimulationLogFile.json";
            SerializationQueue = new ConcurrentQueue<SerializationObject>();

            using (FileStream LogFile = File.Create(PathToLogFile))
            {
                LogFile.Close();
            }

            this.StopTask = false;
            Task.Run(WriteSerializedDataToFile);
        }

        public override void AddDataBallToSerializationQueue(SerializationObject BallObject)
        {
            if (SerializationQueue.Count < this.QueueSize)
            {
                SerializationQueue.Enqueue(BallObject);
                StateChange.Cancel();
            }
        }

        public override void AddBoardDataToSerializationQueue(SerializationObject BoardObject)
        {
            if (SerializationQueue.Count < this.QueueSize)
            {
                SerializationQueue.Enqueue(BoardObject);
                StateChange.Cancel();
            }
        }

        private async void WriteSerializedDataToFile()
        {
            StringBuilder stringBuilder = new StringBuilder();
            while (!this.StopTask)
            {
                if (!SerializationQueue.IsEmpty)
                {
                    while (SerializationQueue.TryDequeue(out SerializationObject serilizedObject))
                    {
                        JObject jsonObject = JObject.FromObject(serilizedObject);
                        jsonObject["Time"] = DateTime.Now.ToString("HH:mm:ss");
                        BufferToWrite.Add(jsonObject);
                    }

                    stringBuilder.Append(JsonConvert.SerializeObject(BufferToWrite, Formatting.Indented));
                    BufferToWrite.Clear();
                    await File.AppendAllTextAsync(PathToLogFile, stringBuilder.ToString());
                    stringBuilder.Clear();
                }
                await Task.Delay(Timeout.Infinite, StateChange.Token).ContinueWith(_ => { });

                if (this.StateChange.IsCancellationRequested)
                {
                    this.StateChange = new CancellationTokenSource();
                }
            }
        }

        public override void Dispose()
        {
            this.StopTask = true;
        }

        ~JSONSerializer()
        {
            this.Dispose();
        }
    }
}
