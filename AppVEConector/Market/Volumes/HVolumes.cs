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
        private readonly object syncLock = new object();
        /// <summary> Коллекция горизонтальных объемов </summary>
        private List<ChartFull> Collection = new List<ChartFull>();
        /// <summary> Максимальный объем Buy  </summary>
        private Chart MaxBuy = null;
        /// <summary> Максимальный объем Sell  </summary>
        private Chart MaxSell = null;
        /// <summary> Максимальный горизонтальный объем из коллекции </summary>
        public Chart MaxVolume = null;
        /// <summary> Минимальный горизонтальный объем из коллекции </summary>
		public Chart MinVolume = null;

        /// <summary> Максимальный горизонтальный объем из коллекции DELTA</summary>
		public Chart MaxDeltaVolume = null;
        /// <summary> Минимальный горизонтальный объем из коллекции DELTA</summary>
		public Chart MinDeltaVolume = null;

        /// <summary> Кол-во элементов (цен) в коллекции  </summary>
        public int Count
        {
            get { return this.Collection.Count; }
        }
        /// <summary> Конструктор </summary>
        public HVolume()
        {
            this.Clear();
        }
        /// <summary> Очистка коллекцию </summary>
        public void Clear()
        {
            lock (syncLock)
            {
                this.Collection.Clear();
            }
        }
        /// <summary> Возвращает массив </summary>
        /// <returns></returns>
        public ChartFull[] ToArray()
        {
            lock (syncLock)
            {
                var array = this.Collection.ToArray();
                return array;
            }
        }
        public void AddVolume(decimal price, long volumeBuy, long volumeSell)
        {
            var elem = this.ToArray().FirstOrDefault(e => e.Price == price);
            if (elem.IsNull())
            {
                elem = new ChartFull() { Price = price, VolBuy = 0, VolSell = 0 };
                lock (syncLock)
                {
                    this.Collection.Add(elem);
                }
            }

            if (MaxBuy.IsNull())
            {
                MaxBuy = new Chart() { Price = elem.Price, Volume = elem.VolBuy };
            }
            if (MaxSell.IsNull())
            {
                MaxSell = new Chart() { Price = elem.Price, Volume = elem.VolSell };
            }
            if (MaxVolume.IsNull())
            {
                MaxVolume = new Chart() { Price = elem.Price, Volume = -100000000 };
            }
            if (MinVolume.IsNull())
            {
                MinVolume = new Chart() { Price = elem.Price, Volume = 10000000 };
            }
            if (MaxDeltaVolume.IsNull())
            {
                MaxDeltaVolume = new Chart() { Price = elem.Price, Volume = -100000000 };
            }
            if (MinDeltaVolume.IsNull())
            {
                MinDeltaVolume = new Chart() { Price = elem.Price, Volume = 10000000 };
            }
            if (elem.NotIsNull())
            {
                elem.VolBuy += volumeBuy;
                elem.VolSell += volumeSell;
                elem.CountBuy++;
                elem.CountSell++;
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
        }
        /// <summary> Добавляем цену и объем в коллекцию </summary>
        /// <param name="price"></param>
        /// <param name="volume"></param>
        public void AddVolume(decimal price, long volume, bool isBuy)
        {
            var elem = this.ToArray().FirstOrDefault(e => e.Price == price);
            if (elem.IsNull())
            {
                elem = new ChartFull() { Price = price, VolBuy = 0, VolSell = 0 };
                lock (syncLock)
                {
                    this.Collection.Add(elem);
                }
            }

            if (MaxBuy.IsNull())
            {
                MaxBuy = new Chart() { Price = elem.Price, Volume = elem.VolBuy };
            }

            if (MaxSell.IsNull())
            {
                MaxSell = new Chart() { Price = elem.Price, Volume = elem.VolSell };
            }

            if (MaxVolume.IsNull())
            {
                MaxVolume = new Chart() { Price = elem.Price, Volume = elem.VolBuy + elem.VolSell };
            }
            if (MinVolume.IsNull())
            {
                MinVolume = new Chart() { Price = elem.Price, Volume = elem.VolBuy + elem.VolSell };
            }
            if (MaxDeltaVolume.IsNull())
            {
                MaxDeltaVolume = new Chart() { Price = elem.Price, Volume = elem.VolBuy - elem.VolSell };
            }
            if (MinDeltaVolume.IsNull())
            {
                MinDeltaVolume = new Chart() { Price = elem.Price, Volume = elem.VolBuy - elem.VolSell };
            }
            if (elem.NotIsNull())
            {
                if (isBuy)
                {
                    elem.VolBuy += volume;
                    elem.CountSell++;
                    if (MaxBuy.Volume < elem.VolBuy)
                    {
                        MaxBuy.Price = elem.Price;
                        MaxBuy.Volume = elem.VolBuy;
                    }
                }
                else
                {
                    elem.VolSell += volume;
                    elem.CountSell++;
                    if (MaxSell.Volume < elem.VolSell)
                    {
                        MaxSell.Price = elem.Price;
                        MaxSell.Volume = elem.VolSell;
                    }
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
                /*if (MaxDeltaVolume.Volume < elem.VolBuy - elem.VolSell)
                {
                    MaxDeltaVolume.Price = elem.Price;
                    MaxDeltaVolume.Volume = elem.VolBuy - elem.VolSell;
                }
                if (MinDeltaVolume.Volume > elem.VolBuy - elem.VolSell)
                {
                    MinDeltaVolume.Price = elem.Price;
                    MinDeltaVolume.Volume = elem.VolBuy - elem.VolSell;
                }*/
            }
        }

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
        }
        /// <summary>  Получить список элементов между двумя ценами </summary>
        /// <param name="price1"></param>
        /// <param name="price2"></param>
        /// <returns></returns>
        public IEnumerable<ChartFull> GetElementBetween(decimal price1, decimal price2)
        {
            decimal tmp = 0;
            if (price1 > price2)
            {
                tmp = price1;
                price1 = price2;
                price2 = tmp;
            }
            return this.ToArray().Where(e => e.Price >= price1 && e.Price <= price2);
        }

        /// <summary> Возвращает объем по текущей цене </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public ChartFull GetVolume(decimal price)
        {
            var elem = this.ToArray().FirstOrDefault(e => e.Price == price);
            if (elem != null) return elem;
            return null;
        }

        /// <summary> Возвращает сумму всех объемов </summary>
        /// <returns></returns>
        public decimal GetSumAllVolume(bool isBuy)
        {
            if (isBuy) return this.ToArray().Sum(e => e.VolBuy);
            else return this.ToArray().Sum(e => e.VolSell);
        }
        /// <summary> Возвращает сумму объемов между 2мя ценами </summary>
        /// <returns></returns>
        public decimal GetSumVolumeBetween(decimal price1, decimal price2, bool isBuy)
        {
            decimal tmp = 0;
            if (price1 > price2)
            {
                tmp = price1;
                price1 = price2;
                price2 = tmp;
            }
            var list = this.GetElementBetween(price1, price2);
            if (isBuy) return list != null ? list.Sum(e => e.VolBuy) : 0;
            else return list != null ? list.Sum(e => e.VolSell) : 0;
        }
    }
}

