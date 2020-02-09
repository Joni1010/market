using GraphicTools.Base;
using GraphicTools.Shapes;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace GraphicTools
{
	public class LevelsOrders
	{
        public ViewPanel Panel = null;

        public LevelsOrders(BaseParams param)
        {
            Panel = new ViewPanel(param);
        }

        public IEnumerable<MarketObjects.Chart> CollectionOrders = null;

		public void Paint()
		{
            Panel.Clear();
			var canvas = Panel.GetGraphics;

			if (this.CollectionOrders.IsNull()) return;
			int count = this.CollectionOrders.Count();
			if (count == 0) return;

			foreach (var ord in this.CollectionOrders.ToArray())
			{
				var vol = ord.Volume < 0 ? ord.Volume * -1 : ord.Volume;
				var horLine = new HorLine();
				horLine.ColorLine = horLine.ColorText = ord.Volume > 0 ? Color.DarkGreen : Color.DarkRed;
				horLine.TextHAlign = HorLine.DirectionLine.Left;
				horLine.FillText = true;
				horLine.Paint(canvas, Panel.Rect.Rectangle, ord.Price, ord.Price.ToString(), Panel.Params.MaxPrice, Panel.Params.MinPrice);
			}
		}
	}
}
