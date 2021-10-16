using System;

namespace Market.Candles
{
    [Serializable]
    public class Candle
    {
        /// <summary> Время свечи </summary>
        public DateTime Time;
        /// <summary> Общий обьем </summary>
        public long Volume
        {
            get { return VolumeBuy + VolumeSell; }
        }
        /// <summary> Общий обьем BUY </summary>
        public long VolumeBuy = 0;
        /// <summary> Общий обьем SELL </summary>
        public long VolumeSell = 0;

        public decimal High = -1000000;
        public decimal Low = 1000000;
        public decimal Open = -1000000;
        public decimal Close = 0;

        /// <summary> id первой сделки </summary>       
        public long OpenId = -1;
        /// <summary> Id последней сделки </summary>
        public long CloseId = -1;
    }
}
