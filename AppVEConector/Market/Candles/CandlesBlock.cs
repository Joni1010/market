using Market.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market.Candles
{
    /// <summary>
    /// Класс блоков свечей
    /// </summary>
    [Serializable]
    public class CandlesBlock : BaseBlock<CandlesBlock, CandleData>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeFrame"></param>
        /// <param name="idTime"></param>
        public CandlesBlock(int timeFrame, BlockTime idTime)
        {
            lock (syncLock)
            {
                periodTimeFrame = timeFrame;
                IdTime = idTime;
            }
        }
        /// <summary>
        /// Получить свечку по времени
        /// </summary>
        /// <param name="time"></param>
        /// <param name="addNew"></param>
        /// <returns></returns>

        public override CandleData GetElement(DateTime time, bool addNew = false)
        {
            var timeCandle = CandleData.GetTimeCandle(time, periodTimeFrame);
            lock (syncLock)
            {
                if (lastSearchedElement.NotIsNull())
                {
                    if (lastSearchedElement.Time == timeCandle)
                    {
                        return lastSearchedElement;
                    }
                    lastSearchedElement = null;
                }
                var candle = Collection.FirstOrDefault(c => c.Time == timeCandle);
                if (candle.NotIsNull())
                {
                    lastSearchedElement = candle;
                    return lastSearchedElement;
                }
                else if (addNew)
                {
                    var newCandle = new CandleData(timeCandle);
                    Collection.Add(newCandle);
                    Collection = Collection.OrderByDescending(c => c.Time).ToList();
                    lastSearchedElement = newCandle;
                }
            }
            return lastSearchedElement;
        }
    }
}
