using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using AppVEConector.libs;
using MarketObjects;
using Market.Candles;
using AppVEConector.Strategy;
using QuikConnector.libs;
using Market.Base;

/// <summary> Библиотека торгуемых элеметов </summary>
namespace Market.AppTools
{
    /// <summary> Торгуемы активный элемент</summary>
    public class TElement
    {
        const string DIR_CHARTS = "charts";
        const string DIR_VOLUMES = "hvolumes";
        /// <summary>
        /// Тайм-фрейм в котором ведется учет сделок
        /// </summary>
        //static int TIME_FRAME_CONTROL = 5;
        /// <summary>
        /// Иструмент
        /// </summary>
        public Securities Security = null;

        public List<Trade> LastTrades = new List<Trade>();

        public StorageTimeFrames StorageTF = null;

        /// <summary> Последние данный по стакану </summary>
        //public LockObject<Quote> LastQuote = new LockObject<Quote>();

        /// <summary> Коллекция тайм-фреймов со свечками </summary>
        //public List<CandleCollection> CollectionTimeFrames = new List<CandleCollection>();
        /// <summary>
        /// Locker
        /// </summary>
        //private readonly object syncObj = new object();

        /// <summary> Событие новой свечи в любом тайм фрейме  </summary>
        //public CandleLib.CandleCollection.EventCandle OnNewCandle = null;
        /// <summary> Тайм-фрейм контроля сделок </summary>
       // private CandleCollection TFControllTrades = null;

        /// <summary>
        /// Список стратегий
        /// </summary>
        public List<object> ListStrategy = new List<object>();

        /// <summary> Событие новой свечи в любом тайм-фрейме </summary>
        public event ElementTF<CandlesBlock, CandleData>.eventElementTimeFrame OnNewCandle;
        /// <summary>
        /// Время сохранения 
        /// </summary>

        public TElement(Securities sec)
        {
            this.Security = sec;
            ListStrategy.Add(new FastGap());
            StorageTF = new StorageTimeFrames(GetDirCharts(DIR_CHARTS), GetDirCharts(DIR_VOLUMES));
        }

        public void Create()
        {
            //Добавление тайм-фреймов
            /*foreach (var timeFrame in TElement.TIME_FRAMES)
            {
                this.CollectionTimeFrames.Add(new CandleCollection(timeFrame));
            }*/
            //Определяем контролируемы тафм-фрейм
            /*this.TFControllTrades = this.CollectionTimeFrames.FirstOrDefault(f => f.TimeFrame == TIME_FRAME_CONTROL);
            this.TFControllTrades.ControlTrades = true;
            this.TFControllTrades.OnNewCandle += (tframe, candle) =>
            {
                this.TFControllTrades.ClearHistoryTrade(candle);
            };*/


            StorageTF.Each((tf) =>
            {
                tf.Candles.OnNew += (tframe, candle) =>
                {
                    if (!OnNewCandle.IsNull())
                    {
                        OnNewCandle(tframe, candle);
                    }
                };
            });
        }

        /// <summary> Сохранение всех котировок в файл </summary>
        public void Save()
        {
            StorageTF.SaveAllTimeFrames();
        }

        /// <summary> Загружает котировки из файла </summary>
        public void LoadCharts(bool load = false)
        {
            /* if (this.SaveHistory || load)
             {
                 this.CollectionTimeFrames.ForEach((tf) =>
                 {
                     this.LoadAllCollection(tf.TimeFrame);
                 });
             }*/
        }

        public void LoadHistoryCharts(int timeframe)
        {
            StorageTF.LoadHistory(timeframe);
        }

        /// <summary> Получает директорию хранения котировок </summary>
        /// <returns></returns>
        private string GetDirCharts(string nameDir)
        {
            var rootDir = Global.GetPathData();
            var dirCharts = rootDir != "" ? rootDir + "\\" + nameDir + "\\" : ".\\" + nameDir + "\\";
            string FullDir = dirCharts + this.Security.ClassCode + "\\";
            if (!Directory.Exists(FullDir)) Directory.CreateDirectory(FullDir);
            FullDir = FullDir + this.Security.Code + "\\";
            if (!Directory.Exists(FullDir)) Directory.CreateDirectory(FullDir);
            return FullDir;
        }

        /// <summary> Запись новой сделки </summary>
        /// <param name="trade"></param>
        public void NewTrade(Trade trade, bool history = false)
        {
            StorageTF.AddNewTrade(trade, history);
        }
        /// <summary> Очистка котировок за период </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        public void ClearCandles(DateTime dateStart, DateTime dateEnd)
        {
            StorageTF.DeleteCandles(dateStart, dateEnd);
        }
    }
}
