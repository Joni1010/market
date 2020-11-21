using Market.Volumes;
using MarketObjects;
using System;


namespace Market.Volumes
{
    [Serializable]
    public class TradeVolume : Volume
    {
        public TradeVolume() { }


        /// <summary>  Добавление данных  </summary>
        /// <param name="trade"></param>
        public void AddTrade(Trade trade)
        {
            this.AddVolumeToArray(trade);
        }

		/// <summary>
		/// Запись сделки в коллекцию объемов
		/// </summary>
		/// <param name="trade"></param>
		private void AddVolumeToArray(Trade trade)
        {
            if (trade.IsSell())
            {
                this.AddSell(trade.Price, (int)trade.Volume);
                this.AddBuy(trade.Price, 0);
            }
            else if (trade.IsBuy())
            {
                this.AddBuy(trade.Price, (int)trade.Volume);
                this.AddSell(trade.Price, 0);
            }
        }
    }
}
