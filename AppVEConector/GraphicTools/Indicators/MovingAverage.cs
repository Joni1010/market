using GraphicTools;
using GraphicTools.Base;
using GraphicTools.Shapes;
using Market.Candles;
using System;
using System.Drawing;
using static GraphicTools.GCandles;

namespace AppVEConector.GraphicTools.Indicators
{
    public class MovingAverage : Indicator
    {
        /// <summary>
        /// Тип цены по которой считаем значение средней
        /// </summary>
        public enum TYPE_PRICE : int
        {
            CLOSE = 1,
            OPEN = 2,
            HIGH = 3,
            LOW = 4
        };

        private CandleCollection Collection = null;

        /// <summary>
        /// Period
        /// </summary>
        private int Period = 30;

        /// <summary>
        /// Тип цены
        /// </summary>
        public TYPE_PRICE TypePrice = TYPE_PRICE.CLOSE;
        /// <summary>
        /// Значение расчитанной средней
        /// </summary>
        private decimal ValueMovingAverage = 0;

        public MovingAverage(ViewPanel mainPanel, bool enable = true) : 
            base(mainPanel)
        {
            Enable = enable;
        }
        /// <summary>
        /// Получить коллекцию
        /// </summary>
        /// <param name="collection"></param>
        public void GetCollectionCandles(CandleCollection collection)
        {
            Collection = collection;
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
        /// <summary>
        /// Получение значения средней
        /// </summary>
        /// <returns></returns>
        public decimal GetValue()
        {
            return ValueMovingAverage;
        }

        private struct MAValues
        {
            public decimal Value;
            public int Count;
        }
        /// <summary>
        /// Массив расчитанных данных для индикатора
        /// </summary>
        private MAValues[] allVal;
        private int index = 0;


        public override void EventInitStartIndicator()
        {
            Panel.Clear();
            allVal = new MAValues[this.CountVisibleCandle];
            index = 0;
        }

        public override void EventInitEndIndicator()
        {

        }

        private void funcCal(decimal value)
        {
            for (int i = index - Period - 1 >= 0 ? index - Period - 1 : 0; i <= index; i++)
            {
                if (allVal.Length > i && allVal[i].Count < Period)
                {
                    allVal[i].Value += value;
                    allVal[i].Count++;
                }
            }
            index++;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void EachCandle(int index, CandleData can, int count)
        {
            /*if (Period <= 0)
            {
                Panel.Clear();
                return;
            }*/
            if (this.TypePrice == TYPE_PRICE.CLOSE)
            {
                funcCal(can.Close);
            }
            else if (this.TypePrice == TYPE_PRICE.OPEN)
            {
                funcCal(can.Open);
            }
            else if (this.TypePrice == TYPE_PRICE.HIGH)
            {
                funcCal(can.High);
            }
            else if (this.TypePrice == TYPE_PRICE.LOW)
            {
                funcCal(can.Low);
            }

        }

        public override void EachFullCandle(CandleInfo candle)
        {
            Calculate(candle.Index, candle.TailCoord.High.X);
        }
        public override void NewCandleInTimeFrame(int timeFrame, CandleData candle)
        {

        }

        private PointF lastPoint = new PointF();
        /// <summary>
        /// Расчет средней
        /// </summary>
        /// <returns></returns>
        public void Calculate(int index, float lineCandle)
        {
            if (Period == 0) return;
            if (index == 0)
            {
                allVal[index].Value = Math.Round(allVal[index].Value / Period, Panel.Params.CountFloat);
                int y1 = GMath.GetCoordinate(Panel.Rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, allVal[index].Value);
                lastPoint = new PointF(lineCandle, y1);
                return;
            }
            if(index >= allVal.Length)
            {
                index = allVal.Length - 1;
            }
            if (allVal[index].Count == Period)
            {
                allVal[index].Value = Math.Round(allVal[index].Value / Period, Panel.Params.CountFloat);
                int y2 = GMath.GetCoordinate(Panel.Rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, allVal[index].Value);
                var p2 = new PointF(lineCandle, y2);
                Paint(lastPoint, p2, allVal[index].Value);
                lastPoint = p2;
            }
        }

        private void Paint(PointF p1, PointF p2, decimal price)
        {
            var canvas = Panel.GetGraphics;
            if (canvas.NotIsNull())
            {
                Line line = new Line();
                line.Width = 1;
                line.Paint(canvas, p1, p2, Color.Violet);
                /*
                HorLine lineH = new HorLine();
                lineH.ColorLine = Color.Violet;
                lineH.FillText = true;
                lineH.Paint(canvas, PaintPanel.Rect.Rectangle, price, PaintPanel.Params.MaxPrice, PaintPanel.Params.MinPrice);
                */
            }
        }
    }
}
