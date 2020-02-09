using System.Drawing;
using System.Linq;
using GraphicTools.Base;
using GraphicTools.Extension;

namespace GraphicTools
{
    public class BGraphExt : BGraph
    {
        protected GRightValue RightPrices = null;
        protected GCandles Candels = null;
        protected GTimeFrame Times = null;

        /// <summary> Прямоугольник отрисовки врменых отметок </summary>
        protected Rectangle BottomRectForTimeLine;

        protected void InitExt()
        {
            Candels = new GCandles(MainPanel.Params);
            RightPrices = new GRightValue(MainPanel.Params);
            Times = new GTimeFrame(MainPanel.Params);

            RightPrices.Decimal = MainPanel.Params.CountFloat;
        }

        /// <summary> Расчет мин и макс цены на всем графике </summary>
		protected void GetMinMaxBase()
        {
            var collection = this.Candels.CollectionCandle.Take(Candels.CountPaintCandle);
            this.MainPanel.Params.MaxPrice = collection.Max(c => c.High);
            this.MainPanel.Params.MinPrice = collection.Min(c => c.Low);

            this.CorrectMinMax();
        }
        /// <summary>
        /// корректировка мин и макс значения цены
        /// </summary>
        private void CorrectMinMax()
        {
            var period = this.MainPanel.Params.MaxPrice - this.MainPanel.Params.MinPrice;
            var step = period * 10 / 100;
            this.MainPanel.Params.MaxPrice += step;
            this.MainPanel.Params.MinPrice -= step;

            this.MainPanel.Params.MaxPrice = System.Math.Round(this.MainPanel.Params.MaxPrice, this.MainPanel.Params.CountFloat);
            this.MainPanel.Params.MinPrice = System.Math.Round(this.MainPanel.Params.MinPrice, this.MainPanel.Params.CountFloat);

            var strMin = this.MainPanel.Params.MinPrice.ToString();
            var getMin = strMin.Substring(strMin.Length - 1).ToDecimal() * MainPanel.Params.MinStepPrice;
            MainPanel.Params.MinPrice -= getMin;

            this.MainPanel.Params.oldMaxPrice = this.MainPanel.Params.MaxPrice;
            this.MainPanel.Params.oldMinPrice = this.MainPanel.Params.MinPrice;
        }
    }
}
