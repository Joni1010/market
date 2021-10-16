using Market.Base;
using Market.Candles;
using System;
using System.Linq;


namespace Market.Volumes
{
    /// <summary>
    /// Класс блоков свечей-горизонтальных обьемов
    /// </summary>
    [Serializable]
    public class HVolumeBlock : BaseBlock <HVolumeBlock, HVolume>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeFrame"></param>
        /// <param name="idTime"></param>
        public HVolumeBlock(int timeFrame, BlockTime idTime)
        {
            lock (syncLock)
            {
                periodTimeFrame = timeFrame;
                IdTime = idTime;
            }
        }
        
        /// <summary>
        /// Получить элемент
        /// </summary>
        /// <param name="time"></param>
        /// <param name="addNew"></param>
        /// <returns></returns>

        public override HVolume GetElement(DateTime time, bool addNew = false)
        {
            var timeElement = CandleData.GetTimeCandle(time, periodTimeFrame);
            lock (syncLock)
            {
                if (lastSearchedElement.NotIsNull())
                {
                    if (lastSearchedElement.Time == timeElement)
                    {
                        return lastSearchedElement;
                    }
                    lastSearchedElement = default;
                }
                var element = Collection.FirstOrDefault(c => c.Time == timeElement);
                if (element.NotIsNull())
                {
                    lastSearchedElement = element;
                    return lastSearchedElement;
                }
                else if (addNew)
                {
                    var newElement = new HVolume(timeElement);
                    Collection.Add(newElement);
                    Collection = Collection.OrderByDescending(c => c.Time).ToList();
                    lastSearchedElement = newElement;
                }
            }
            return lastSearchedElement;
        }       
    }
}
