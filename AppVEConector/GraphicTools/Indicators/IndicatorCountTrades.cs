using GraphicTools;
using GraphicTools.Base;
using GraphicTools.Shapes;
using Market.Candles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVEConector.GraphicTools.Indicators
{
    class IndicatorCountTrades : Indicator
    {
        public IndicatorCountTrades(ViewPanel mainPanel) : 
            base(mainPanel)
        {
        }
        public override void EventInitStartIndicator()
        {
            Panel.Clear();
        }

        public override void EventInitEndIndicator()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        public override void EachCandle(int index, CandleData can, int count)
        {
            
        }

        public override void EachFullCandle(GCandles.CandleInfo toolsCandle)
        {
            if (!Enable) return;
            var canvas = Panel.GetGraphics;
            var circleCountTrade = new Ellipse();
            circleCountTrade.Width = 1;
            circleCountTrade.ColorLine = Color.Black;
            circleCountTrade.Fill = true;
            circleCountTrade.FillColor = Color.Blue;
            circleCountTrade.Radius = toolsCandle.Candle.CountTrade / 100 / CurrentTimeFrame;
            circleCountTrade.PaintCircle(canvas, 
                new PointF(toolsCandle.TailCoord.High.X - circleCountTrade.Radius,
                toolsCandle.Body.Y + toolsCandle.Body.Height / 2 - circleCountTrade.Radius));
        }
        public override void NewCandleInTimeFrame(int timeFrame, CandleData candle)
        {
            
        }
    }
}
