using GraphicTools;
using GraphicTools.Base;
using GraphicTools.Shapes;
using Market.Candles;
using System;
using System.Drawing;
using static GraphicTools.GCandles;

namespace AppVEConector.GraphicTools.Indicators
{
    public class IndicatorATR : Indicator
    {
        /// <summary>
        /// Тип цены по которой считаем значение средней
        /// </summary>
        public decimal Value = 0;

        /// <summary>
        /// Period
        /// </summary>
        private int Period = 30;

        public IndicatorATR(ViewPanel mainPanel, bool enable = true) : 
            base(mainPanel)
        {
            Enable = enable;
        }

        /// <summary>
        /// Установить период
        /// </summary>
        /// <param name="period"></param>
        public void SetPeriod(int period)
        {
            Period = period;
        }
        public int GetPeriod()
        {
            return Period;
        }

        public override void EventInitStartIndicator()
        {
            Panel.Clear();
        }

        public override void EventInitEndIndicator()
        {

        }

        private decimal sumATR = 0;
        /// <summary>
        /// 
        /// </summary>
        public override void EachCandle(int index, CandleData can, int count)
        {
            if (index == 0)
            {
                sumATR = 0;
            }
            if (index < Period)
            {
                sumATR += can.High - can.Low;
                Value = sumATR / Period;
            } 
        }

        public override void EachFullCandle(CandleInfo candle)
        {
            
        }
        public override void NewCandleInTimeFrame(int timeFrame, CandleData candle)
        {

        }

        private void Paint(PointF p1, PointF p2, decimal price)
        {
            /*var canvas = Panel.GetGraphics;*/
        }
    }
}
