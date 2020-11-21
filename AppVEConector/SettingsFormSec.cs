using AppVEConector.libs;
using Connector.Logs;
using MarketObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVEConector
{
    public class SettingsFormSec : Settings
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
        /// Data
        /// </summary>
        //public SData Data = new SData();
        public SData Data
        {
            set { this.data = value; }
            get { return (SData)this.data; }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void init() {
            if (data.IsNull())
            {
                data = new SData();
            }
        }

        private SettingsFormSec(string fileName) : base(fileName)
        {

        }
        public static SettingsFormSec New(Securities sec)
        {
            var filename = getFilename(sec.ToString());
            return new SettingsFormSec(filename);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string getFilename(string secAndClass)
        {
            var rootDir = libs.Global.GetPathData();
            var dir = rootDir != "" ? rootDir + "\\settings\\" : ".\\settings\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filename = dir + "set_" + secAndClass.Replace(':', '_') + ".dat";
            return filename;
        }
        /// <summary>
        /// Инициализируем все time-frames
        /// </summary>
        /// <param name="timeFrames"></param>
        public void InitTimeFrame(IEnumerable<int> timeFrames)
        {
            timeFrames.ForEach<int>((tf) =>
            {
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
    }
}
