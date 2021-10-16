using GraphicTools;
using GraphicTools.Base;
using Market.Candles;
using System;
using System.Drawing;
using static GraphicTools.GCandles;

namespace AppVEConector.GraphicTools.Indicators
{
    /// <summary>
    /// Базовый класс индикаторов
    /// </summary>
    public abstract class Indicator
    {
        /// <summary>
        /// Панель
        /// </summary>
        protected ViewPanel Panel = null;
        /// <summary>
        /// Кол-во свечей видимых на графике
        /// </summary>
        public int CountVisibleCandle = 0;
        /// <summary>
        /// Кол-во всех свечей от текущей
        /// </summary>
        public int CountAllCandle = 0;
        /// <summary>
        /// Текуйщий тайм фрейм
        /// </summary>
        public int CurrentTimeFrame = 0;
        /// <summary>
        /// Включить индикатор
        /// </summary>
        public bool Enable = false;

        /// <summary>
        /// Быстрая перерисовка
        /// </summary>
        public bool FastRedraw = false;
        /// <summary>
        /// Флаг инициализации
        /// </summary>
        private bool beforeEachCandle = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainPanel"></param>
        public Indicator(ViewPanel mainPanel)
        {
            Panel = new ViewPanel(mainPanel.Params);
            Panel.Rect = mainPanel.Rect;
        }
        /// <summary>
        /// Возвращает активен ли индикатор(true) или нет(false)
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEnable()
        {
            return Enable;
        }

        /// <summary>
        /// Инициализация старта идикатора
        /// </summary>
        /// <returns></returns>
        public bool InitStartIndicator(Action actionBeforeInit)
        {
            if (!beforeEachCandle)
            {
                if (actionBeforeInit.NotIsNull())
                {
                    actionBeforeInit();
                }
                EventInitStartIndicator();
                beforeEachCandle = true;
                return true;
            }
            return false;
        }

        public bool InitEndIndicator(Action actionAfterInit)
        {
            if (beforeEachCandle)
            {
                if (actionAfterInit.NotIsNull())
                {
                    actionAfterInit();
                }
                EventInitEndIndicator();
                beforeEachCandle = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Отправит на отрисовку на главный холст
        /// </summary>
        /// <param name="canvas"></param>
        public void ToCanvas(Graphics canvas)
        {
            Panel.Paint(canvas);
        }
        /// <summary>
        /// Получение координаты Y по цене
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        protected decimal GetYByPrice(decimal price)
        {
            return GMath.GetCoordinate(Panel.Rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, price);
        }

        public virtual void FastUpdate()
        {

        }

        public abstract void EventInitStartIndicator();
        public abstract void EventInitEndIndicator();
        /// <summary>
        /// Обработка каждой свечки cо всеми данными
        /// </summary>
        public abstract void EachFullCandle(CandleInfo toolsCandle);
        /// <summary>
        /// Обработка по каждой свечке в текущем тайм-фрейме
        /// </summary>
        /// <param name="candle"></param>
        public abstract void EachCandle(int index, CandleData candle, int count);
        /// <summary>
        /// Новая свечка в тайм-фрейме
        /// </summary>
        /// <param name="timeFrame"></param>
        /// <param name="candle"></param>
        public abstract void NewCandleInTimeFrame(int timeFrame, CandleData candle);
    }

}
