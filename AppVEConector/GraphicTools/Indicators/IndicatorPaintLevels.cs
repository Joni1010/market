using GraphicTools;
using GraphicTools.Base;
using GraphicTools.Shapes;
using Market.Candles;
using System;
using System.Collections.Generic;
using System.Drawing;
using static GraphicTools.GCandles;

namespace AppVEConector.GraphicTools.Indicators
{
    public class IndicatorPaintLevels : Indicator
    {
        private readonly object syncObj = new object();
        /// <summary>
        /// Список уровней
        /// </summary>
        private List<decimal> Levels = new List<decimal>();

        private int countPainted = 0;

        public IndicatorPaintLevels(ViewPanel mainPanel, bool enable = true) :
            base(mainPanel)
        {
            Enable = enable;
        }

        /// <summary>
        /// Установить период
        /// </summary>
        /// <param name="period"></param>
        public void SetLevels(decimal[] levels)
        {
            lock (syncObj)
            {
                Levels.Clear();
                Levels.AddRange(levels);
            }
        }

        public override void EventInitStartIndicator()
        {
            Panel.Clear();
            countPainted = 0;
        }

        public override void EventInitEndIndicator()
        {
            while (countPainted < Levels.Count)
            {
                Paint();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void EachCandle(int index, CandleData can, int count)
        {
            if (countPainted < Levels.Count)
            {
                Paint();
            }
        }

        public override void EachFullCandle(CandleInfo candle)
        {

        }
        public override void NewCandleInTimeFrame(int timeFrame, CandleData candle)
        {

        }

        private void Paint()
        {
            var canvas = Panel.GetGraphics;
            decimal levSign = 0;
            lock (syncObj)
            {
                levSign = Levels[countPainted];
            }
            if (levSign < Panel.Params.MaxPrice && levSign > Panel.Params.MinPrice)
            {
                var y = GMath.GetCoordinate(this.Panel.Rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, levSign);
                Point p1 = new Point() { X = Panel.Rect.X + Panel.Rect.Width - 30, Y = y };
                Point p2 = new Point() { X = Panel.Rect.X + Panel.Rect.Width, Y = y };
                Line lineLevel = new Line();
                lineLevel.Paint(canvas, p1, p2, Color.Red);
            }
            countPainted++;
        }
    }
}
