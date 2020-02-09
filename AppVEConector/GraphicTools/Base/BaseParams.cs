using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicTools.Base
{
    public class BaseParams
    {
        /// <summary>
        /// Тип масштабирования
        /// </summary>
        public enum TYPE_SCALING
        {
            NONE = 0,
            STRECH = 1,
            MOVE = 2
        };

        /// <summary>
		/// Максимальная цена
		/// </summary>
		public decimal MaxPrice = -10000000;
        /// <summary>
        /// Минимальная цена
        /// </summary>
        public decimal MinPrice = 10000000;

        /// <summary>
		/// Старая Максимальная цена
		/// </summary>
		public decimal oldMaxPrice = -10000000;
        /// <summary>
        /// Старая Минимальная цена
        /// </summary>
        public decimal oldMinPrice = 10000000;
        /// <summary>
        /// Автоматический рассчет MaxPrice and MinPrice
        /// </summary>
        public bool AutoSize = true;
        /// <summary>
        /// Режим масштабирования MaxPrice and MinPrice (вверх или вниз)
        /// </summary>
        public TYPE_SCALING TypeScaling = TYPE_SCALING.NONE;
        /// <summary>
		/// Отступ между свечами
		/// </summary>
		public float MarginCandle = 0;
        /// <summary>
        /// Ширина свечи
        /// </summary>
        public float WidthCandle = 0;
        /// <summary>
        /// Кол-во знаков после запятой
        /// </summary>
        public int CountFloat = 0;
        /// <summary>
		/// Минимальный шаг цены
		/// </summary>
		public decimal MinStepPrice = 0;
        /// <summary>
        /// Код инструмента
        /// </summary>
        public string Code = "";
        /// <summary>
        /// Класс инструмента
        /// </summary>
        public string Class = "";
        /// <summary>
        /// флаг о перерисовке
        /// </summary>
        private bool redraw = false;
        /// <summary>
        /// Перерисовать все
        /// </summary>
        public void WillRedraw()
        {
            redraw = true;
        }

        public bool Redraw()
        {
            if (redraw)
            {
                redraw = false;
                return true;
            }
            return false;
        }

        public void SetParam(decimal maxPrice, decimal minPrice, float widthCandle, float marginCandle, int countFloat, decimal minStepPrice)
        {
            this.SetParam(maxPrice, minPrice, widthCandle, marginCandle);
            this.CountFloat = countFloat;
            this.MinStepPrice = minStepPrice;
        }
        /// <summary>
        /// Установить базовые данные
        /// </summary>
        /// <param name="maxPrice"></param>
        /// <param name="minPrice"></param>
        /// <param name="widhtCandle"></param>
        /// <param name="marginCandle"></param>
        /// <param name="countFloat"></param>
        public void SetParam(decimal maxPrice, decimal minPrice, float widthCandle, float marginCandle, int countFloat)
        {
            this.SetParam(maxPrice, minPrice, widthCandle, marginCandle);
            this.CountFloat = countFloat;
        }

        /// <summary>
        /// Установить базовые данные
        /// </summary>    
        /// <param name="maxPrice"></param>
        /// <param name="minPrice"></param>
        /// <param name="widthCandle"></param>
        /// <param name="marginCandle"></param>
        public void SetParam(decimal maxPrice, decimal minPrice, float widthCandle, float marginCandle)
        {
            /*if (rectPlace.X != this.Rect.X || rectPlace.Y != this.Rect.Y ||
                rectPlace.Width != this.Rect.Width || rectPlace.Height != this.Rect.Height)
            {
                this.RectPlace = rectPlace;
                this.Rect = rectPlace.SetX(0).SetY(0);
                this.Clear();
                if (this.OnResize.NotIsNull()) this.OnResize(this.Rect);
            }*/
            this.MaxPrice = maxPrice;
            this.MinPrice = minPrice;
            this.MarginCandle = marginCandle;
            this.WidthCandle = widthCandle;
        }
    }
}
