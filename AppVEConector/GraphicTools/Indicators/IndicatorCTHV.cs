using GraphicTools;
using GraphicTools.Base;
using GraphicTools.Shapes;
using Market.Candles;
using System;
using System.Drawing;


namespace AppVEConector.GraphicTools.Indicators
{
    class IndicatorCTHV : Indicator
    {
        public long MaxCount = -1;
        public int MinLimitCount = 10;
        public IndicatorCTHV(ViewPanel mainPanel) :
            base(mainPanel)
        {
            //Enable = true;
        }
        public override void EventInitStartIndicator()
        {
            Panel.Clear();
            MaxCount = -1;
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
                    if (hv.CountBuy + hv.CountSell > MinLimitCount)
                    {
                        var value = hv.CountBuy + hv.CountSell;
                        if (MaxCount < value) MaxCount = value;
                    }
                }
            }
        }

        public override void EachFullCandle(GCandles.CandleInfo toolsCandle)
        {
            if (!Enable) return;

            foreach (var hv in toolsCandle.Candle.GetHorVolumes().HVolCollection.ToArray())
            {
                if (hv.CountBuy + hv.CountSell > MinLimitCount)
                {
                    var canvas = Panel.GetGraphics;
                    var value = hv.CountBuy + hv.CountSell;

                    int y = GMath.GetCoordinate(Panel.Rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, hv.Price);
                    int x = GMath.GetCoordinate(toolsCandle.Body.Width, MaxCount, 0, value);
                    float radius = (toolsCandle.Body.Width - x) / 2;
                    if (radius == 0) continue;

                    var line = new Line();
                    if (MaxCount / 2 < value)
                    {
                        line.Width = 2f;
                        line.Paint(canvas,
                            new PointF(toolsCandle.TailCoord.High.X - radius, y),
                            new PointF(toolsCandle.TailCoord.High.X + radius, y), Color.Red);
                    } else
                    {
                        line.Width = 2f;
                        line.Paint(canvas,
                            new PointF(toolsCandle.TailCoord.High.X - radius, y),
                            new PointF(toolsCandle.TailCoord.High.X + radius, y), Color.FromArgb(220, Color.Blue));
                    }
                }
            }
        }
        public override void NewCandleInTimeFrame(int timeFrame, CandleData candle)
        {

        }
    }
}
