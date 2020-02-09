using Connector.Logs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVEConector
{
    public class Settings
    {
        [Serializable]
        public class SDataTF
        {
            /// <summary>
            /// Кол-во свеячей видимых на полотне
            /// </summary>
            public int CountVisibleCandle = 50;
            /// <summary>
            /// Период по средней
            /// </summary>
            public int MovingAverage = 30;
        }
        [Serializable]
        public class SData
        {
            /// <summary>
            /// Code client
            /// </summary>
            public string CodeClient = "";
            /// <summary>
            /// Вид клиентского лимита
            /// </summary>
            public int TypeClientLimit = 0;
            /// <summary>
            /// Кол-во свечей в обьеме
            /// </summary>
            public decimal CountCandleInVolume = 0;
            /// <summary>
            /// Кол-во дней на которые выставляется стоп-заявка
            /// </summary>
            public int CountDaysForStopOrder = 5;
            /// <summary>
            /// Последний используемы тайм-фрейм
            /// </summary>
            public int CurrentTimeFrame = 1;
            /// <summary>
            /// 
            /// </summary>
            public Dictionary<int, SDataTF> TimeFrame = new Dictionary<int, SDataTF>();

            /// <summary>
            /// Кол-во тиков контроля стопа
            /// </summary>
            public int TiksControlStop = 0;
            /// <summary>
            /// Кол-во тиков контроля тэйка
            /// </summary>
            public int TiksControlTake = 0;
            /// <summary>
            /// Процент выхода из позиции
            /// </summary>
            public int PercentTakeOut = 0;
        }
        /// <summary>
        /// Locker
        /// </summary>
        private readonly object syncLock = new object();
        /// <summary>
        /// Data
        /// </summary>
        public SData Data = new SData();
        /// <summary>
        /// SecurityCode:CodeClass
        /// </summary>
        private string SecCode = "";

        public Settings()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetFilename()
        {
            var rootDir = libs.Global.GetPathData();
            var dir = rootDir != "" ? rootDir + "\\settings\\" : ".\\settings\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filename = dir + "set_" + this.SecCode.Replace(':', '_') + ".dat";
            return filename;
        }
        /// <summary>
        /// Перезагрузить инструмент
        /// </summary>
        /// <param name="SecAndClass"></param>
        public void ReloadSecurity(string SecAndClass)
        {
            if (!this.SecCode.Empty())
            {
                this.Save();
            }
            this.SecCode = SecAndClass;
            this.Load();
        }
        /// <summary>
        /// Инициализируем все time-frames
        /// </summary>
        /// <param name="timeFrames"></param>
        public void InitTimeFrame(IEnumerable<int> timeFrames)
        {
            timeFrames.ForEach<int>((tf)=> {
                CreateTimeFrame(tf);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeFrame"></param>
        private void CreateTimeFrame(int timeFrame)
        {
            var tf = Data.TimeFrame.FirstOrDefault(t => t.Key == timeFrame);
            if (tf.IsNull() || tf.Value.IsNull())
            {
                Data.TimeFrame.Add(timeFrame, new SDataTF());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            if (!this.SecCode.Empty())
            {
                this.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Save()
        {
            Qlog.CatchException(() =>
            {
                Stream stream = File.Open(GetFilename(), FileMode.Create);
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                lock (syncLock)
                {
                    binaryFormatter.Serialize(stream, this.Data);
                }
                stream.Close();
                return true;
            }, "");
        }

        private void Load()
        {
            Qlog.CatchException(() =>
            {
                var filename = GetFilename();
                if (File.Exists(filename))
                {
                    Stream stream = File.Open(filename, FileMode.Open);
                    stream.Position = 0;
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    lock (syncLock)
                    {
                        this.Data = (SData)binaryFormatter.Deserialize(stream);
                    }
                    stream.Close();
                    return true;
                }
                return false;
            }, "");
        }
    }
}
