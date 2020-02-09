
using GraphicTools.Base;
using MarketObjects;
using System.Drawing;

namespace GraphicTools
{
    public class BGraph
    {
        /// <summary> Полотно для рисования  </summary>
		protected Graphics Canvas;
        /// <summary>
        /// Полотно как главная панель
        /// </summary>
        protected ViewPanel MainPanel;

        /// <summary>
        /// Максимальная цена на графике
        /// </summary>
        public decimal MaxPrice
        {
            get
            {
                return MainPanel.Params.MaxPrice;
            }
        }
        /// <summary>
        /// Минимальная цена на графике
        /// </summary>
        public decimal MinPrice
        {
            get
            {
                return MainPanel.Params.MinPrice;
            }
        }

    }
}
