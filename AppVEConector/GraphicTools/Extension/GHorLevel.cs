using GraphicTools.Base;
using GraphicTools.Shapes;
using System.Drawing;
using System.Linq;

namespace GraphicTools.Extension
{
	/// <summary>
	/// Горизонтальные уровни, обычно расположенные снизу от графика.
	/// </summary>
	public class GHorLevel :BaseLevels
	{
        /// <summary>
        /// 
        /// </summary>
        public ViewPanel Panel = null;
        /// <summary>
        /// 
        /// </summary>
        private ViewPanel PanelLast = null;
        /// <summary> Область значений шкалы </summary>
        public GRightValue Values = null;

		public GHorLevel(BaseParams param)
		{
            Panel = new ViewPanel(param);
            PanelLast = new ViewPanel(param);
            Values = new GRightValue(param);
            Values.ColorLines = Color.Gray;
            Panel.OnChangeRect += (rect) =>
            {
                PanelLast.SetRect(rect);
                Values.Panel.SetRect(rect);
            };

        }
		public void PaintLevels(Graphics canvas, decimal MaxPrice, decimal MinPrice)
		{
			if (this.CollectionLevels == null) return;
	
			foreach (var valLevel in this.CollectionLevels)
			{
				this.PaintOneLevel(canvas, Panel, valLevel, MaxPrice, MinPrice);
			}

			(new Line()).Paint(canvas, new Point(Panel.Rect.X, Panel.Rect.Y), new Point(Panel.Rect.X, Panel.Rect.Y + Panel.Rect.Height), Color.Black);

			(new TextDraw()).Paint(canvas, "min:" + this.Min.ToString(), Panel.Rect.X, Panel.Rect.Y + Panel.Rect.Height - 12, Color.Black);
			(new TextDraw()).Paint(canvas, "max:" + this.Max.ToString(), Panel.Rect.X, Panel.Rect.Y + 1, Color.Black);
		}

		private void PaintOneLevel(Graphics canvas, ViewPanel Panel, MarketObjects.Chart Value, decimal MaxPrice, decimal MinPrice)
		{
			int y = GMath.GetCoordinate(Panel.Rect.Height, MaxPrice, MinPrice, Value.Price);
			int x1 = Panel.Rect.X + GMath.GetCoordinate(Panel.Rect.Width, this.Max, this.Min, Value.Volume > 0 ? 0 : Value.Volume);
			int x2 = Panel.Rect.X + GMath.GetCoordinate(Panel.Rect.Width, this.Max, this.Min, Value.Volume < 0 ? 0 : Value.Volume);

			var lineLev = new Line();
			lineLev.Width = 2;
			lineLev.Paint(canvas, new Point(x1, y), new Point(x2, y), Value.Volume < 0 ? Color.Red : Color.Green);
		}
	}
}
