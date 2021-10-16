using AppVEConector.libs;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AppVEConector
{
    public class SettingsFormSec 
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
        public class SData : Settings
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
            public Dictionary<int, SDataTF> timeFrame = new Dictionary<int, SDataTF>();

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
            /// <summary>
            /// Лимиты границ по объемам в дельте
            /// </summary>
            public long HVLimitsDeltaVolumes = 0;

            public SData(string filename) : base(filename)
            {
                data = this;
                load();
            }
        }
        /// <summary>
        /// Хранилище настроек
        /// </summary>
        protected SData Storage = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public dynamic Get(string varName)
        {
            return Storage.Get(varName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, object value)
        {
            Storage.Set(name, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeframe"></param>
        /// <param name="varName"></param>
        /// <param name="value"></param>
        public void SetTF(int timeframe, string varName, object value)
        {
            try
            {
                Storage.timeFrame[timeframe].GetType().GetField(varName).SetValue(Storage.timeFrame[timeframe], value);
                Storage.save();
                return;
            }
            catch (Exception)
            {
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeframe"></param>
        /// <param name="varName"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public dynamic GetTF(int timeframe, string varName)
        {
            try
            {
                return Storage.timeFrame[timeframe].GetType().GetField(varName).GetValue(Storage.timeFrame[timeframe]);
            }
            catch (Exception)
            {
                return null;
            }
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
        private void CreateTimeFrame(int timeframe)
        {
            var tf = Storage.timeFrame.FirstOrDefault(t => t.Key == timeframe);
            if (tf.IsNull() || tf.Value.IsNull())
            {
                Storage.timeFrame.Add(timeframe, new SDataTF());
            }
        }

        protected SettingsFormSec(string filenameSettings)
        {
            Storage = new SData(filenameSettings);
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
        
    }
}
