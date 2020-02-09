using GraphicTools;
using GraphicTools.Base;
using GraphicTools.Shapes;
using Market.Candles;

using System.Drawing;


namespace AppVEConector.GraphicTools.Indicators
{
    class IndicatorHV : Indicator
    {
        /// <summary>
        /// Размер по которому смотреть гор. обьемы
        /// </summary>
        public int SizeHorVolumes = 0;
        /// <summary>
        /// 
        /// </summary>
        public long MaxHVolume = -1;

        public long MinLimitVolume = 10;
        public IndicatorHV(ViewPanel mainPanel) :
            base(mainPanel)
        {
            //Enable = true;
        }
        public override void EventInitStartIndicator()
        {
            Panel.Clear();
            MaxHVolume = -1;
        }
        public override void EventInitEndIndicator()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public override void EachCandle(int index, CandleData candle, int count)
        {
            if (!Enable) return;
            if (index < CountVisibleCandle)
            {
                foreach (var hv in candle.GetHorVolumes().HVolCollection.ToArray())
                {
                    if (hv.VolBuy + hv.VolSell > MinLimitVolume)
                    {
                        var value = hv.VolBuy + hv.VolSell;
                        if (MaxHVolume < value) MaxHVolume = value;
                    }
                }
            }
        }

        public override void EachFullCandle(GCandles.CandleInfo toolsCandle)
        {
            if (!Enable) return;

            foreach (var hv in toolsCandle.Candle.GetHorVolumes().HVolCollection.ToArray())
            {
                if (hv.VolBuy + hv.VolSell > MinLimitVolume)
                {
                    var canvas = Panel.GetGraphics;
                    var value = hv.VolBuy + hv.VolSell;
                    var radius = (int)(value / CurrentTimeFrame);
                    if (radius == 0) continue;
                    int y = GMath.GetCoordinate(Panel.Rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, hv.Price);
                    int x = GMath.GetCoordinate(toolsCandle.Body.Width, MaxHVolume, 0, value);

                    var line = new Line();
                    if (MaxHVolume / 2 < value)
                    {
                        line.Width = 2.0f;
                        line.Paint(canvas,
                            new PointF(toolsCandle.Body.X, y),
                            new PointF(toolsCandle.Body.X + toolsCandle.Body.Width - x, y), Color.Red);
                    } else {
                        line.Width = 2.0f;
                        line.Paint(canvas,
                            new PointF(toolsCandle.Body.X, y),
                            new PointF(toolsCandle.Body.X + toolsCandle.Body.Width - x, y), Color.FromArgb(220, Color.Blue));
                    }
                }
            }
        }
        public override void NewCandleInTimeFrame(int timeFrame, CandleData candle)
        {

        }
    }
}
