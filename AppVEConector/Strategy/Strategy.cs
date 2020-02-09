using Market.Candles;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVEConector.Strategy
{
    public abstract class Strategy
    {
        /// <summary>
        /// Инструмент
        /// </summary>
        public Securities Security = null;
        /// <summary>
        /// Период опроса (в секундах)
        /// </summary>
        public int StepTime = 2;

        /// <summary>
        /// Тайм-фрейм в котором ведется опрос
        /// </summary>
        public int TimeFrame = 30;

        /// <summary>
        /// Индекс свечи с которой начинать опрос
        /// </summary>
        public int IndexStartCandle = 0;
        /// <summary>
        /// Кол-во свечей от стартовой
        /// </summary>
        public int CountCandle = 10;

        /// <summary>
        /// Свеча на которой сработало событие
        /// </summary>
        public CandleData CandleEvent = null;

        /// <summary>
        /// Время последней активации
        /// </summary>
        public DateTime TimeLastAction = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public decimal  Option_1 = 0;
        public int      Option_2 = 0;
        public long     Option_3 = 0;
        public double   Option_4 = 0;
        public decimal  Option_5 = 0;

        /// <summary>
        /// Перед обработкой стратегии
        /// </summary>
        public abstract void BeforeAction(Action action);
        /// <summary>
        /// Обработчик выбранной коллекции
        /// </summary>
        /// <param name="candleCollection"></param>
        public abstract string ActionCollection(IEnumerable<CandleData> candleCollection);
    }
}
