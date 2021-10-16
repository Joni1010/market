using Market.Volumes;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>  Данные по свечкам.  </summary>
namespace Market.Candles
{
    /// <summary> Класс хранимых данных свечки. </summary>
    [Serializable]
    public class CandleData : Candle
    {
        /// <summary> Кол-во сделок в свече </summary>
        private long CountTrades = 0;
        /// <summary>
        /// Список num сделок для контроля
        /// </summary>
        private List<long> controlTrades = null;
        public delegate void EventCandle(CandleData candle);

        private readonly object syncLock = new object();

        /// <summary> Конструктор свечи</summary>
        /// <param name="time">Граничное время свечи</param>
        public CandleData(DateTime time)
        {
            Time = time;
        }
        /// <summary> Кол-во сделок в свече </summary>
        /// <returns></returns>
        public long GetCountTrades()
        {
            lock (syncLock)
            {
                return CountTrades;
            }
        }

        /// <summary>
        /// Запись новой сделки в свечку.
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="addForce">LДобавить сделку принудительно </param>
        /// <returns></returns>
        public bool NewTrade(Trade trade, bool addForce = false)
        {
            if (syncLock.IsNull())
            {
                return false;
            }
            lock (syncLock)
            {
                if (OpenId < 0 || CloseId < 0)
                {
                    OpenId = CloseId = trade.Number;
                }
                else
                {
                    if (OpenId <= trade.Number && trade.Number <= CloseId && !addForce)
                    {
                        return false;
                    }
                }
                //Open
                if (OpenId >= trade.Number)
                {
                    Open = trade.Price;
                    OpenId = trade.Number;
                }
                //Close
                if (CloseId <= trade.Number)
                {
                    Close = trade.Price;
                    CloseId = trade.Number;
                }
                //High
                if (High < trade.Price) { High = trade.Price; }
                //Low
                if (Low > trade.Price) { Low = trade.Price; }

                CountTrades++;

                //Считаем объемы отдельно
                if (trade.IsSell())
                {
                    VolumeSell += trade.Volume;
                }
                else
                {
                    VolumeBuy += trade.Volume;
                }
                if (controlTrades.NotIsNull())
                {
                    controlTrades.Add(trade.Number);
                }
                return true;
            }
        }
        /// <summary>
        /// Проверяет событие новой свечи
        /// </summary>
        /// <returns></returns>
        public bool IsNewCandle()
        {
            return GetCountTrades() == 1 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool CheckTrade(long num)
        {
            var found = controlTrades.FirstOrDefault(n => n == num);
            return found > 0 ? true : false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void InitControl()
        {
            if (controlTrades.IsNull())
            {
                controlTrades = new List<long>();
            }
        }

        /// <summary>  Расчет времени для свечи (граничной), по текущему времени.  </summary>
        /// <param name="time">Время сделки</param>
        /// <returns></returns>
        public static DateTime GetTimeCandle(DateTime time, int TimeFrame)
        {
            time = time.AddMilliseconds(time.Millisecond * -1);
            time = time.AddSeconds(time.Second * -1);
            int k = (int)(TimeFrame / 60);
            if (k > 0)
            {
                double r = (double)(time.Hour % k);
                if (r >= 1)
                {
                    k = 60 * (time.Hour - (int)(time.Hour / k) * k);
                }
                else
                {
                    k = 0;
                }
            }
            else
            {
                k = (int)(time.Minute / TimeFrame);
                if (time.Minute == k * TimeFrame) k = time.Minute;
                else k = (k * TimeFrame);
                k *= -1;
            }
            k = k + time.Minute;

            time = time.AddMinutes(k * -1);
            time = time.AddMilliseconds(time.Millisecond * -1);
            return time;
        }
    }
}