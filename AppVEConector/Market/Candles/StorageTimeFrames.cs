using Market.Base;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.Candles
{
    public class StorageTimeFrames
    {
        /// <summary>
        /// Коллекция тайм-фреймов
        /// </summary>
        public static int[] TIME_FRAMES = { 1, 2, 3, 5, 15, 30, 60, 240, 1440 };
        /// <summary>
        /// Периоды хранения в днях
        /// </summary>
        public static int[] PERIOD_KEEP = { 7, 7, 7, 14, 30, 60, 365, 365, 1825 };

        private readonly object syncLock = new object();
        private List<TimeFrame> AllTimeFrames = new List<TimeFrame>();

        /// <summary> Событие новой свечи в любом тайм-фрейме </summary>
        public event ElementTF<CandlesBlock, CandleData>.eventElementTimeFrame OnNewCandleAnyTimeFrame;

        private string DirKeepCharts = null;
        private string DirKeepHVol = null;
        /// <summary>
        /// Тайм фрейм который контролирует сделки
        /// </summary>
        private TimeFrame tfControl = null;

        /// <summary>
        /// Возвращает коллекцию тайм-фреймов
        /// </summary>
        private TimeFrame[] Collection
        {
            get
            {
                lock (syncLock)
                {
                    return AllTimeFrames.ToArray();
                }
            }
        }
        public int Count
        {
            get
            {
                lock (syncLock)
                {
                    return AllTimeFrames.Count;
                }
            }
        }

        public StorageTimeFrames(string dirKeepCharts, string dirKeepVol)
        {
            DirKeepCharts = dirKeepCharts;
            DirKeepHVol = dirKeepVol;
            init();
        }

        private void init()
        {
            OnNewCandleAnyTimeFrame += eventNewCandleAnyTimeFrame;

            for (int i = 0; i < TIME_FRAMES.Length; i++)           
            {
                var tf = TIME_FRAMES[i];
                var keepDay = tf < 60 ? true : false;
                var newTf = new TimeFrame(tf, keepDay, DirKeepCharts, DirKeepHVol);
                newTf.Candles.SetPeriodKeep(PERIOD_KEEP[i]);
                newTf.HVolumes.SetPeriodKeep(PERIOD_KEEP[i]);
                if (OnNewCandleAnyTimeFrame.NotIsNull())
                {
                    newTf.Candles.OnNew += OnNewCandleAnyTimeFrame;
                }
                AllTimeFrames.Add(newTf);

                newTf.Candles.OnSave += (tframe) =>
                {
                    newTf.HVolumes.Save();
                };

                newTf.Candles.Load();
            }
        }

        private void eventNewCandleAnyTimeFrame(int timeFrame, CandleData candle)
        {
            /*if (timeFrame == 1)
            {
                SaveAllTimeFrames();
            }*/
        }
        /// <summary>
        /// 
        /// </summary>
        public void SaveAllTimeFrames()
        {
            Each((tf) =>
            {
                tf.Candles.Save();
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeFrame"></param>
        public void LoadHistory(int timeFrame)
        {
            var timeFr = Collection.FirstOrDefault(tf => tf.Period == timeFrame);
            if (timeFr.NotIsNull())
            {
                timeFr.Candles.Load();
            }
        }

        public void Each(Action<TimeFrame> action)
        {
            foreach (var tf in Collection)
            {
                lock (syncLock)
                {
                    action(tf);
                }
            }
        }

        public TimeFrame GetTimeFrame(int periodTimeFrame)
        {
            return Collection.FirstOrDefault(tf => tf.Period == periodTimeFrame);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        public void DeleteCandles(DateTime dateStart, DateTime dateEnd)
        {
            while (true)
            {
                Each((timeframe) =>
                {
                    timeframe.Candles.Delete(dateStart);
                    timeframe.HVolumes.Delete(dateStart);
                });
                dateStart = dateStart.AddMinutes(1);
                if (dateStart > dateEnd)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candle"></param>
        /// <param name="trade"></param>
        /// <returns></returns>
        private bool addTrade(CandleData candle, Trade trade)
        {
            if (!candle.NewTrade(trade))
            {
                //Если сделка не попала в диапазон min < num < max
                if (!tfControl.CheckTrade(trade))
                {
                    return candle.NewTrade(trade, true);
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeFrame"></param>
        /// <param name="trade"></param>
        /// <param name="history"></param>
        /// <param name="control"></param>
        private void processAddTrade(TimeFrame timeFrame, Trade trade, bool history, bool control = false)
        {
            var candle = timeFrame.Candles.GetCandle(trade.DateTrade.DateTime, true);
            if (control)
            {
                candle.InitControl();
            }
            if (addTrade(candle, trade))
            {
                //блок был изменен
                timeFrame.Candles.GetCurrentBlock().SetChanged();

                var candleHVol = timeFrame.HVolumes.GetCandle(trade.DateTrade.DateTime, true);
                candleHVol.NewTrade(trade, true);
                timeFrame.HVolumes.GetCurrentBlock().SetChanged();

                //Генерируем событие новой свечи
                if (candle.IsNewCandle() && !history)
                {
                    timeFrame.Candles.EventNewElement(candle, timeFrame.Candles.BeforeNewCandle);
                }
            }
        }

        public bool AddNewTrade(Trade trade, bool history = false)
        {
            if (trade.IsNull())
            {
                return false;
            }
            //Контрольный там фрейм
            if (tfControl.IsNull())
            {
                var minTFr = TIME_FRAMES.Min();
                tfControl = GetTimeFrame(minTFr);
            }
            Each((timeframe) =>
            {
                if (tfControl.Period == timeframe.Period) { return; }
                processAddTrade(timeframe, trade, history);
            });
            lock (syncLock)
            {
                //Добавляем сделку в контрольный тайм-фрейм
                processAddTrade(tfControl, trade, history, true);
            }
            return true;
        }
    }
}
