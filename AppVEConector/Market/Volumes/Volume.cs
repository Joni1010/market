using System;

namespace Market.Volumes
{
    [Serializable]
    public class Volume
    {
		/// <summary> Сумма объемов на покупку </summary>
        public long SumBuy = 0;
		/// <summary> Сумма объемов на продажу </summary>
		public long SumSell = 0;
		public HVolume HVolCollection = new HVolume();

		/// <summary> Добавляет объем Buy</summary>
		/// <param name="price"></param>
		/// <param name="volume"></param>
        protected void AddBuy(decimal price, long volume)
        {
            this.HVolCollection.AddVolume(price, volume, true);
            this.SumBuy += volume;
        }

        /// <summary> Добавляет объем Sell</summary>
        protected void AddSell(decimal price, long volume)
        {
            this.HVolCollection.AddVolume(price, volume, false);
            this.SumSell += volume;
        }
    }
}
