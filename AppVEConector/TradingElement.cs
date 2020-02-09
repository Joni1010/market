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

/// <summary> Библиотека торгуемых элеметов </summary>
namespace TradingLib
{
    /// <summary> Коллекция активных торгуемых инструментов </summary>
    public class TElementCollection
    {
        /// <summary>
        /// Collection all trade elements
        /// </summary>
        private List<TElement> _Collection = new List<TElement>();
        /// <summary>
        /// Locker
        /// </summary>
        private readonly object syncLock = new object();

        /// <summary>
        /// Последний найденный инструмент
        /// </summary>
        private TElement lastFoundElem = null;
        public TElement[] Collection
        {
            get
            {
                lock (syncLock)
                {
                    return this._Collection.ToArray();
                }
            }
        }
        /// <summary> Получает торговый элемент</summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public TElement GetTElement(Securities sec)
        {
            lock (syncLock)
            {
                return this._Collection.FirstOrDefault(t => t.Security == sec);
            }
        }
        /// <summary> Добавляет торговый элемент в коллекцию по указанному инструменту.
        /// Если элемент существует в коллекции, то он возвращается.</summary>
        /// <param name="newElem"></param>
        public TElement AddOrFind(Securities sec)
        {
            if (sec.IsNull())
            {
                return null;
            }
            lock (syncLock)
            {
                if (lastFoundElem.NotIsNull() && lastFoundElem.Security == sec)
                {
                    return lastFoundElem;
                }
                var el = this._Collection.FirstOrDefault(t => t.Security == sec);
                if (el.IsNull())
                {
                    el = new TElement(sec);
                    this._Collection.Add(el);
                    el.Create();
                }
                lastFoundElem = el;
                return el;
            }
        }
    }

    /// <summary> Торгуемы активный элемент</summary>
    public class TElement
    {
        /// <summary>
        /// Коллекция тайм-фреймов
        /// </summary>
        public static int[] TIME_FRAMES = { 1, 2, 3, 5, 15, 30, 60, 240, 1440 };
        /// <summary>
        /// Тайм-фрейм в котором ведется учет сделок
        /// </summary>
        static int TIME_FRAME_CONTROL = 5;
        /// <summary>
        /// Иструмент
        /// </summary>
        public Securities Security = null;

        /// <summary> Последние данный по стакану </summary>
        //public LockObject<Quote> LastQuote = new LockObject<Quote>();

        /// <summary> Коллекция тайм-фреймов со свечками </summary>
        public List<CandleCollection> CollectionTimeFrames = new List<CandleCollection>();
        /// <summary>
        /// Locker
        /// </summary>
        private readonly object syncObj = new object();

        /// <summary> Событие новой свечи в любом тайм фрейме  </summary>
        //public CandleLib.CandleCollection.EventCandle OnNewCandle = null;
        /// <summary> Тайм-фрейм контроля сделок </summary>
        private CandleCollection TFControllTrades = null;

        /// <summary>
        /// Список стратегий
        /// </summary>
        public List<object> ListStrategy = new List<object>();

        public delegate void EventCandle(int timeframe, CandleData candle);
        /// <summary> Событие новой свечи в любом тайм-фрейме </summary>
        public event EventCandle OnNewCandle;
        /// <summary>
        /// Время сохранения 
        /// </summary>
        public DateTime TimeSave = DateTime.Now;
        /// <summary>
        /// флаг сохранения истории инструмента
        /// </summary>
        private bool SaveHistory = false;
        /// <summary>
        /// Флаг определяющий, проверен ли инструмент на сохранение.
        /// </summary>
        private bool CheckOnSave = false;

        public TElement(Securities sec)
        {
            this.Security = sec;
            ListStrategy.Add(new FastGap());
        }
        /// <summary>
        /// Получает статус флага, был ли проверен елемент на сохранение
        /// </summary>
        /// <returns></returns>
        public bool GetFlagCheckSave()
        {
            return CheckOnSave;
        }
        /// <summary>
        /// Установить флаг сохранения елемента
        /// </summary>
        /// <param name="stateSave"></param>
        /// <returns></returns>
        public bool SetFlagSave(bool stateSave)
        {
            this.SaveHistory = stateSave;
            this.CheckOnSave = true;
            if (this.TFControllTrades.FirstCandle.IsNull())
            {
                this.LoadCharts();
            }
            return this.SaveHistory;
        }
        /// <summary>
        /// Получить флаг сохранения елемента
        /// </summary>
        /// <returns></returns>
        public bool GetFlagSave()
        {
            return this.SaveHistory;
        }
        public void Create()
        {
            lock (syncObj)
            {
                //Добавление тайм-фреймов
                foreach (var timeFrame in TElement.TIME_FRAMES)
                {
                    this.CollectionTimeFrames.Add(new CandleCollection(timeFrame));
                }
                //Определяем контролируемы тафм-фрейм
                this.TFControllTrades = this.CollectionTimeFrames.FirstOrDefault(f => f.TimeFrame == TIME_FRAME_CONTROL);
                this.TFControllTrades.ControlTrades = true;
                this.TFControllTrades.OnNewCandle += (tframe, candle) =>
                {
                    this.TFControllTrades.ClearHistoryTrade(candle);
                };

                this.CollectionTimeFrames.ForEach((oneTf) =>
                {
                    oneTf.OnNewCandle += (tframe, candle) =>
                    {
                        if (!this.OnNewCandle.IsNull())
                        {
                            this.OnNewCandle(tframe, candle);
                        }
                    };
                });
            }
        }

        /// <summary> Сохранение всех котировок в файл </summary>
        public void SaveCharts(bool save = false)
        {
            if (this.SaveHistory || save)
            {
                this.CollectionTimeFrames.ForEach((tf) =>
                {
                    if (tf.TimeLastUpdateCollection.IsNull())
                    {
                        tf.TimeLastUpdateCollection = DateTime.Now;
                    }
                    if (tf.TimeLastUpdateCollection >= tf.TimeLastSave || save)
                    {
                        this.SaveAllCollection(tf.TimeFrame);
                    }
                });
                TimeSave = DateTime.Now;
            }
        }

        /// <summary> Загружает котировки из файла </summary>
        public void LoadCharts(bool load = false)
        {
            if (this.SaveHistory || load)
            {
                this.CollectionTimeFrames.ForEach((tf) =>
                {
                    this.LoadAllCollection(tf.TimeFrame);
                });
            }
        }

        /// <summary> Получает директорию хранения котировок </summary>
        /// <returns></returns>
        private string GetDirCharts()
        {
            var rootDir = Global.GetPathData();
            var dirCharts = rootDir != "" ? rootDir + "\\charts\\" : ".\\charts\\";
            string FullDir = dirCharts + this.Security.ClassCode + "\\";
            if (!Directory.Exists(FullDir)) Directory.CreateDirectory(FullDir);
            FullDir = FullDir + this.Security.Code + "\\";
            if (!Directory.Exists(FullDir)) Directory.CreateDirectory(FullDir);
            return FullDir;
        }
        /// <summary> Возвращает путь к файлу с котировками </summary>
        /// <param name="timeFrame">Тайм фрейм</param>
        /// <returns></returns>
        private string GetFileCharts(int timeFrame)
        {
            string filePath = GetDirCharts() + this.Security.Code + "_" + timeFrame + "_dump.dat";
            return filePath;
        }

        /// <summary> Запись всей коллекции тайм фреймов в файл </summary>
        private void SaveAllCollection(int timeframe)
        {
            lock (syncObj)
            {
                var tf = this.CollectionTimeFrames.ToArray().FirstOrDefault(t => t.TimeFrame == timeframe);
                if (tf.NotIsNull() && tf.FirstCandle.NotIsNull())
                {
                    tf.WriteCollectionInFile(this.GetFileCharts(tf.TimeFrame));
                }
            }
        }

        /// <summary> Загрузка тайм-фрейма из файла</summary>
        /// <param name="timeframe"></param>
        private void LoadAllCollection(int timeframe)
        {
            lock (syncObj)
            {
                var tf = this.CollectionTimeFrames.FirstOrDefault(t => t.TimeFrame == timeframe);
                if (!tf.IsNull())
                {
                    string filePath = GetDirCharts() + this.Security.Code + "_" + tf.TimeFrame + "_dump.dat";
                    tf.ReadCollectionFromFile(filePath);
                }
            }
        }


        /// <summary> Запись новой сделки </summary>
        /// <param name="trade"></param>
        public void NewTrade(Trade trade, bool history = false)
        {
            if (!this.TFControllTrades.ExistTradeInTF(trade))
            {
                lock (syncObj)
                {
                    this.CollectionTimeFrames.ForEach((tf) =>
                    {
                        //Проверяем наличие данной сделки в тайм фрейме
                        if (this.TFControllTrades.TimeFrame != tf.TimeFrame)
                        {
                            tf.AddNewTrade(trade, history);
                        }
                    });
                    this.TFControllTrades.AddNewTrade(trade);
                }
            }
        }
        /// <summary> Очистка котировок за период </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        public void ClearCandles(DateTime dateStart, DateTime dateEnd)
        {
            if (this.CollectionTimeFrames.IsNull()) return;
            lock (syncObj)
            {
                this.CollectionTimeFrames.ForEach((tf) =>
                {
                    var candlesClear = tf.MainCollection.Where(c => c.Time >= dateStart && c.Time <= dateEnd).ToArray();
                    if (candlesClear.NotIsNull())
                    {
                        int count = candlesClear.Count();
                        if (count > 0)
                        {
                            tf.RemoveCandles(candlesClear);
                        }
                    }
                });
            }
            this.SaveCharts();
        }
    }
}
