using Market.Candles;
using Market.Volumes;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Base
{
    public class TimeFrame
    {
        public class History
        {
            public int TimeFrame = 0;
            public string Filename = "";
            public BlockTime IdTime;
        }

        private readonly object syncLock = new object();
        /// <summary> Значение тайм-фрейма </summary>
        private int IdTimeFrame = 0;
        /// <summary> Текущий тайм-фрейм </summary>
        public int Period
        {
            get { lock (syncLock) { return IdTimeFrame; } }
        }

        public CandlesTF Candles = null;
        public HVolumesTF HVolumes = null;

        /// <summary> Конструктор </summary>
        /// <param name="timeFrame">Кол-во минут</param>
        public TimeFrame(int periodTimeFrame, bool keepDay, string dirCharts, string dirHVol)
        {
            lock (syncLock)
            {
                Candles = new CandlesTF(periodTimeFrame, keepDay, dirCharts);
                HVolumes = new HVolumesTF(periodTimeFrame, dirHVol);
                IdTimeFrame = periodTimeFrame;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public bool CheckTrade(Trade trade)
        {
            var candle = Candles.GetCandle(trade.DateTrade.GetDateTime());
            if (candle.NotIsNull())
            {
                return candle.CheckTrade(trade.Number);
            }
            return false;
        }
    }
}
