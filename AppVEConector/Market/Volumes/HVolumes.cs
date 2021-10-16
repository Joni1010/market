using MarketObjects;
using MarketObjects.Charts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.Volumes
{
    /// <summary> Класс горизонтальных объемов </summary>
    [Serializable]
    public class HVolume
    {
        /// <summary> Время свечи </summary>
        public DateTime Time;
        /// <summary> id первой сделки </summary>       
        public long OpenId = -1;
        /// <summary> Id последней сделки </summary>
        public long CloseId = -1;

        [Serializable]
        public class Attr
        {
            public Chart Buy = null;
            public Chart Sell = null;
            public Chart Volume = null;
            /// <summary> delta </summary>
            public Chart DVolume = null;
        }
        const long MAX = 1000000000;
        const long MIN = -1000000000;
        private readonly object syncLock = new object();
        /// <summary> Коллекция горизонтальных объемов </summary>
        private List<ChartFull> Collection = new List<ChartFull>();
        /// <summary> Максимальные значения </summary>
        public Attr Min = new Attr();
        /// <summary> Минимальные значения </summary>
        public Attr Max = new Attr();

        /// <summary> Кол-во элементов (цен) в коллекции  </summary>
        public int Count
        {
            get { lock (syncLock) { return Collection.Count; } }
        }
        public HVolume()
        {
            lock (syncLock)
            {
                Clear();
            }
        }
        /// <summary> Конструктор </summary>
        public HVolume(DateTime time)
        {
            lock (syncLock)
            {
                Time = time;
                Clear();
            }
        }
        /// <summary> Очистка коллекцию </summary>
        public void Clear()
        {
            lock (syncLock) { Collection.Clear(); }
        }
        /// <summary> Возвращает массив </summary>
        /// <returns></returns>
        public ChartFull[] ToArray()
        {
            lock (syncLock) { return Collection.ToArray(); }
        }
        /// <summary>
        /// Добавление сделки
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="addForce"></param>
        public void NewTrade(Trade trade, bool addForce=false)
        {
            if (OpenId < 0 || CloseId < 0)
            {
                OpenId = CloseId = trade.Number;
            }
            else
            {
                if (OpenId <= trade.Number && trade.Number <= CloseId && !addForce)
                {
                    return;
                }
            }
            //Open
            if (OpenId >= trade.Number)
            {
                OpenId = trade.Number;
            }
            //Close
            if (CloseId <= trade.Number)
            {
                CloseId = trade.Number;
            }
            Add(trade.Price, trade.Volume, trade.Direction == OrderDirection.Buy ? true : false);
        }

        private void initMinMax(ChartFull elem)
        {
            if (Min.IsNull())
            {
                Min = new Attr();
            }
            if (Max.IsNull())
            {
                Max = new Attr();
            }
            if (Max.Buy.IsNull())
            {
                Max.Buy = new Chart() { Price = elem.Price, Volume = elem.VolBuy };
            }
            if (Max.Sell.IsNull())
            {
                Max.Sell = new Chart() { Price = elem.Price, Volume = elem.VolSell };
            }
            if (Max.Volume.IsNull())
            {
                Max.Volume = new Chart() { Price = elem.Price, Volume = MIN };
            }
            if (Min.Volume.IsNull())
            {
                Min.Volume = new Chart() { Price = elem.Price, Volume = MAX };
            }
            if (Max.DVolume.IsNull())
            {
                Max.DVolume = new Chart() { Price = elem.Price, Volume = MIN };
            }
            if (Min.DVolume.IsNull())
            {
                Min.DVolume = new Chart() { Price = elem.Price, Volume = MAX };
            }
        }
        /// <summary> Добавляем цену и объем в коллекцию </summary>
        public void Add(decimal price, long volumeBuy, long volumeSell)
        {
            lock (syncLock)
            {
                var elem = Collection.FirstOrDefault(e => e.Price == price);
                if (elem.IsNull())
                {
                    elem = new ChartFull() { Price = price, VolBuy = 0, VolSell = 0 };
                    lock (syncLock)
                    {
                        this.Collection.Add(elem);
                    }
                }
                initMinMax(elem);
                if (elem.NotIsNull())
                {
                    elem.VolBuy += volumeBuy;
                    elem.VolSell += volumeSell;
                    elem.CountBuy += volumeBuy > 0 ? 1 : 0;
                    elem.CountSell += volumeSell > 0 ? 1 : 0;
                    if (Max.Buy.Volume < elem.VolBuy)
                    {
                        Max.Buy.Price = elem.Price;
                        Max.Buy.Volume = elem.VolBuy;
                    }
                    if (Max.Sell.Volume < elem.VolSell)
                    {
                        Max.Sell.Price = elem.Price;
                        Max.Sell.Volume = elem.VolSell;
                    }
                    if (Max.Volume.Volume < elem.VolBuy + elem.VolSell)
                    {
                        Max.Volume.Price = elem.Price;
                        Max.Volume.Volume = elem.VolBuy + elem.VolSell;
                    }
                    if (Min.Volume.Volume > elem.VolBuy + elem.VolSell)
                    {
                        Min.Volume.Price = elem.Price;
                        Min.Volume.Volume = elem.VolBuy + elem.VolSell;
                    }
                    if (Max.DVolume.Volume < elem.VolBuy - elem.VolSell)
                    {
                        Max.DVolume.Price = elem.Price;
                        Max.DVolume.Volume = elem.VolBuy - elem.VolSell;
                    }
                    if (Min.DVolume.Volume > elem.VolBuy - elem.VolSell)
                    {
                        Min.DVolume.Price = elem.Price;
                        Min.DVolume.Volume = elem.VolBuy - elem.VolSell;
                    }
                }
            }
        }

        /// <summary> Добавляем цену и объем в коллекцию </summary>
        /// <param name="price"></param>
        /// <param name="volume"></param>
        public void Add(decimal price, long volume, bool isBuy)
        {
            lock (syncLock)
            {
                var elem = Collection.FirstOrDefault(e => e.Price == price);
                if (elem.IsNull())
                {
                    elem = new ChartFull() { Price = price, VolBuy = 0, VolSell = 0 };
                    Collection.Add(elem);
                }
                initMinMax(elem);
                if (elem.NotIsNull())
                {
                    if (isBuy)
                    {
                        elem.VolBuy += volume;
                        elem.CountSell++;
                        if (Max.Buy.Volume < elem.VolBuy)
                        {
                            Max.Buy.Price = elem.Price;
                            Max.Buy.Volume = elem.VolBuy;
                        }
                    }
                    else
                    {
                        elem.VolSell += volume;
                        elem.CountSell++;
                        if (Max.Sell.Volume < elem.VolSell)
                        {
                            Max.Sell.Price = elem.Price;
                            Max.Sell.Volume = elem.VolSell;
                        }
                    }
                    if (Max.Volume.Volume < elem.VolBuy + elem.VolSell)
                    {
                        Max.Volume.Price = elem.Price;
                        Max.Volume.Volume = elem.VolBuy + elem.VolSell;
                    }
                    if (Min.Volume.Volume > elem.VolBuy + elem.VolSell)
                    {
                        Min.Volume.Price = elem.Price;
                        Min.Volume.Volume = elem.VolBuy + elem.VolSell;
                    }
                    if (Max.DVolume.Volume < elem.VolBuy - elem.VolSell)
                    {
                        Max.DVolume.Price = elem.Price;
                        Max.DVolume.Volume = elem.VolBuy - elem.VolSell;
                    }
                    if (Min.DVolume.Volume > elem.VolBuy - elem.VolSell)
                    {
                        Min.DVolume.Price = elem.Price;
                        Min.DVolume.Volume = elem.VolBuy - elem.VolSell;
                    }
                }
            }
        }
        /*
                public void RecalculateMaxMinVolumes()
                {
                    var list = this.ToArray();
                    foreach (var elem in list)
                    {
                        if (MaxBuy.IsNull()) MaxBuy = new Chart();
                        if (MaxSell.IsNull()) MaxSell = new Chart();
                        if (MaxVolume.IsNull()) MaxVolume = new Chart();
                        if (MinVolume.IsNull()) MinVolume = new Chart();
                        if (MaxDeltaVolume.IsNull()) MaxDeltaVolume = new Chart();
                        if (MinDeltaVolume.IsNull()) MinDeltaVolume = new Chart();

                        if (MaxBuy.Volume < elem.VolBuy)
                        {
                            MaxBuy.Price = elem.Price;
                            MaxBuy.Volume = elem.VolBuy;
                        }
                        if (MaxSell.Volume < elem.VolSell)
                        {
                            MaxSell.Price = elem.Price;
                            MaxSell.Volume = elem.VolSell;
                        }
                        if (MaxVolume.Volume < elem.VolBuy + elem.VolSell)
                        {
                            MaxVolume.Price = elem.Price;
                            MaxVolume.Volume = elem.VolBuy + elem.VolSell;
                        }
                        if (MinVolume.Volume > elem.VolBuy + elem.VolSell)
                        {
                            MinVolume.Price = elem.Price;
                            MinVolume.Volume = elem.VolBuy + elem.VolSell;
                        }
                        if (MaxDeltaVolume.Volume < elem.VolBuy - elem.VolSell)
                        {
                            MaxDeltaVolume.Price = elem.Price;
                            MaxDeltaVolume.Volume = elem.VolBuy - elem.VolSell;
                        }
                        if (MinDeltaVolume.Volume > elem.VolBuy - elem.VolSell)
                        {
                            MinDeltaVolume.Price = elem.Price;
                            MinDeltaVolume.Volume = elem.VolBuy - elem.VolSell;
                        }
                    }
                }*/

        /// <summary>
        /// Получить список элементов между двумя ценами
        /// </summary>
        /// <param name="priceHigh"></param>
        /// <param name="priceLow"></param>
        /// <returns></returns>
        public ChartFull[] Between(decimal priceHigh, decimal priceLow)
        {
            lock (syncLock)
            {
                return Collection.Where(e => e.Price <= priceHigh && e.Price >= priceLow).ToArray();
            }
        }

        /// <summary> Возвращает из коллекции по цене </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public ChartFull GetElement(decimal price)
        {
            lock (syncLock)
            {
                return Collection.FirstOrDefault(e => e.Price == price);
            }
        }

        /// <summary> Возвращает сумму всех объемов </summary>
        /// <returns></returns>
        public decimal Sum(bool isBuy)
        {
            lock (syncLock)
            {
                if (isBuy) return Collection.Sum(e => e.VolBuy);
                else return Collection.Sum(e => e.VolSell);
            }
        }
        /// <summary> Возвращает сумму всех объемов </summary>
        /// <returns></returns>
        public decimal Sum()
        {
            lock (syncLock)
            {
                return Collection.Sum(e => e.VolBuy + e.VolSell);
            }
        }
        /// <summary> Возвращает сумму объемов между 2мя ценами </summary>
        /// <returns></returns>
        public long SumBetween(decimal priceHigh, decimal priceLow, bool isBuy)
        {
            var list = Between(priceHigh, priceLow);
            if (list.NotIsNull() && list.Length > 0)
            {
                if (isBuy) return list.Sum(e => e.VolBuy);
                else return list.Sum(e => e.VolSell);
            }
            return 0;
        }
        public long SumBetween(decimal priceHigh, decimal priceLow)
        {
            var list = Between(priceHigh, priceLow);
            if (list.NotIsNull() && list.Length > 0)
            {
                return list.Sum(e => e.VolBuy + e.VolSell);
            }
            return 0;
        }
    }
}

