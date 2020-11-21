using Market.Volumes;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>  Данные по свечкам.  </summary>
namespace Market.Candles
{
    [Serializable]
    public partial class CandleData
    {
        /// <summary> Время свечи </summary>
        public DateTime Time;
        public long Volume = 0;
        public long VolumeBuy = 0;
        public long VolumeSell = 0;

        public decimal High = 0;
        public decimal Low = 1000000;
        public decimal Open = -1;
        public decimal Close = 0;

        /// <summary> id первой сделки </summary>       
        public long FirstId = 0;
        /// <summary> Id последней сделки </summary>
        public long LastId = 0;

        /// <summary> Время первой сделки </summary>
        public DateTime FirstTime = new DateTime();
        /// <summary> Время последней сделки </summary>
        public DateTime LastTime = new DateTime();

        /// <summary> Горизонтальные объемы для свечи </summary>
        public TradeVolume HorVolumes = new TradeVolume();

        /// <summary> Время последнего обновления </summary>
        public DateTime _lastUpdate;
        /// <summary> флаг, была ли свеча записана в файл. </summary>
        public bool _write = false;

        /// <summary> </summary>
        private List<long> CollectionNumTrades = null;
        /// <summary>
        /// Кол-во сделок в свече
        /// </summary>
        public long CountTrade = 0;
        public decimal InterestBuy = 0;
        public decimal InterestSell = 0;
        public long CountInterest = 0;
        private DateTime _lastInterest;
    }


    /// <summary> Класс хранимых данных свечки. </summary>
    public partial class CandleData
    {
        private readonly object syncLock = new object();
        /// <summary> Возвращает кол-во сделок в коллекции по данной свечке </summary>
        public long CountKeepTrades { get { return this.CollectionNumTrades.IsNull() ? 0 : this.CollectionNumTrades.Count; } }
        /// <summary> Очищает хранилище сделок. </summary>
        /// <returns>true - если очистка произведена, иначе false</returns>
        public bool ClearKeepTrades()
        {
            if (this.CountKeepTrades > 0)
            {
                this.CollectionNumTrades.Clear();
                return true;
            }
            return false;
        }

        /// <summary> Проверка уже записанной сделки в данную свечу </summary>
        /// <param name="trade"></param>
        /// <returns>true - если сделка уже записана </returns>
        public bool ExistsTrade(Trade trade)
        {
            if (this.CollectionNumTrades.IsNull()) return false;
            var num = this.CollectionNumTrades.FirstOrDefault(n => n == trade.Number);
            if (num.NotIsNull() && num > 0) return true;
            return false;
        }
        /// <summary> Конструктор свечи</summary>
        /// <param name="time">Граничное время свечи</param>
        public CandleData(DateTime time)
        {
            this.Time = time;
        }
        /// <summary> Запись новой сделки в свечку. </summary>
        /// <param name="trade">Новая сделка</param>
        public void NewTrade(Trade trade, bool controlTrades = false)
        {
            if (syncLock.IsNull())
            {
                return;
            }
            lock (syncLock)
            {
                //Open
                if (this.FirstTime.Ticks > trade.DateTrade.GetDateTime().Ticks ||
                    this.FirstTime.Ticks == 0 ||
                    this.FirstTime.Ticks == trade.DateTrade.GetDateTime().Ticks)
                {
                    if (this.FirstTime.Ticks == trade.DateTrade.GetDateTime().Ticks)
                    {
                        if (this.FirstId > trade.Number)
                        {
                            this.FirstTime = trade.DateTrade.GetDateTime();
                            this.Open = trade.Price;
                            this.FirstId = trade.Number;
                        }
                    }
                    else
                    {
                        this.FirstTime = trade.DateTrade.GetDateTime();
                        this.Open = trade.Price;
                        this.FirstId = trade.Number;
                    }
                }
                //Close
                if (this.LastTime.Ticks < trade.DateTrade.GetDateTime().Ticks ||
                        this.LastTime.Ticks == 0 ||
                        this.LastTime.Ticks == trade.DateTrade.GetDateTime().Ticks)
                {
                    if (this.LastTime.Ticks == trade.DateTrade.GetDateTime().Ticks)
                    {
                        if (this.LastId < trade.Number)
                        {
                            this.LastTime = trade.DateTrade.GetDateTime();
                            this.Close = trade.Price;
                            this.LastId = trade.Number;
                        }
                    }
                    else
                    {
                        this.LastTime = trade.DateTrade.GetDateTime();
                        this.Close = trade.Price;
                        this.LastId = trade.Number;
                    }
                }

                if (this.High < trade.Price) this.High = trade.Price;
                if (this.Low > trade.Price) this.Low = trade.Price;

                this.Volume += trade.Volume;
                this.CountTrade++;

                if (this._lastInterest.AddSeconds(20) < DateTime.Now)
                {
                    this.InterestBuy += trade.Sec.Params.SumBidDepth;
                    this.InterestSell += trade.Sec.Params.SumAskDepth;
                    this.CountInterest++;
                    _lastInterest = DateTime.Now;
                }
                //Считаем объемы отдельно
                if (trade.IsSell()) this.VolumeSell += trade.Volume;
                else this.VolumeBuy += trade.Volume;

                AddHorVolumes(trade);

                if (controlTrades)
                {
                    if (CollectionNumTrades.IsNull()) CollectionNumTrades = new List<long>();
                    CollectionNumTrades.Add(trade.Number);
                }

                _lastUpdate = DateTime.Now;
            }
        }
        /// <summary>
        /// Возвращает коллекцию горизонтальных обьемов по величине собранных сделок
        /// </summary>
        /// <returns></returns>
        public TradeVolume GetHorVolumes()
        {
            return HorVolumes;
        }
        /// <summary>
        /// Добавление горизонтальных обьемов
        /// </summary>
        /// <param name="trade"></param>
        private void AddHorVolumes(Trade trade)
        {
            HorVolumes.AddTrade(trade);
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