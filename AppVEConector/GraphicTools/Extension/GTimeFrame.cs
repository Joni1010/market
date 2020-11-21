using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System;
using GraphicTools.Base;
using GraphicTools.Shapes;

namespace GraphicTools.Extension
{
    public class SelectorTimeFrame
    {
        /// <summary> Набор названий тайм-фреймов </summary>
        public static string[] LabelsTimeFrame = { "1M", "2M", "3M", "5M", "15M", "30M", "60M", "4H", "1D" };
        /// <summary> Набор тайм-фреймов в минутном значении </summary>
        public static int[] TimeFrameMinute = { 1, 2, 3, 5, 15, 30, 60, 240, 1440 };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeframe"></param>
        /// <returns></returns>
        public static int GetIndex(int timeframe)
        {
            return Array.IndexOf(TimeFrameMinute, timeframe);
        }

        public SelectorTimeFrame(int timeFrame)
        {
            int i = Array.IndexOf(TimeFrameMinute, timeFrame);
            if (i >= 0)
            {
                this.TimeFrame = TimeFrameMinute[i];
                this.StrTimeFrame = LabelsTimeFrame[i];
            }
            else this.TimeFrame = -1;
        }
        /// <summary> Строковое представление тайм-фрейма </summary>
        public string StrTimeFrame = "";
        /// <summary> Значение тайм-фрейма </summary>
        public int TimeFrame = 0;
        /// <summary> Получить список всех тайм-фреймов</summary>
        /// <returns></returns>
        public static IEnumerable<SelectorTimeFrame> GetAll()
        {
            List<SelectorTimeFrame> tf = new List<SelectorTimeFrame>();
            foreach (var el in TimeFrameMinute)
                tf.Add(new SelectorTimeFrame(el));
            return tf;
        }

        public override string ToString()
        {
            return this.StrTimeFrame;
        }
    }

    public partial class GTimeFrame
    {
        /// <summary>
        /// Период через который рисуются линии в пикселях
        /// </summary>
        const int PERIOD_SPLIT_LINE = 80;
        /// <summary> Ширина одной свечки </summary>
        public float WidthOneCandle = 0;

        /// <summary> Цвет вертикальных линий отметок </summary>
        public Color ColorMarkLine = Color.FromArgb(238, 238, 238);

        public ViewPanel Panel = null;
        public GTimeFrame(BaseParams param)
        {
            Panel = new ViewPanel(param);
        }

        private Graphics PanelCanvas = null;
        private GCandles.CandleInfo LastCandle = null;
        private int LastX = 0;
        private int index = 1;

        public void BeforePaint()
        {
            this.Panel.Clear();
            PanelCanvas = Panel.GetGraphics;
            index = 1;
            LastCandle = null;
            LastX = 0;
        }
        /// <summary>
        /// Отрисовка временных линий и значений.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rectPaint"></param>
        public void Paint(GCandles.CandleInfo candleData)
        {
            var lineVer = new VerLine();
            //Рисуем линию разделяющую периоды или разрывы
            if (LastCandle.NotIsNull())
            {
                if(candleData.Candle.Time.AddMinutes(candleData.TimeFrame) < LastCandle.Candle.Time)
                {
                    var p1 = new PointF(LastCandle.Body.X, this.Panel.Rect.Y);
                    var p2 = new PointF(LastCandle.Body.X, this.Panel.Rect.Y + this.Panel.Rect.Height);
                    lineVer.ColorLine = Color.FromArgb(100, Color.Gray);
                    lineVer.WidthLine = 1;
                    lineVer.FillText = true;
                    lineVer.Paint(PanelCanvas, null, p1, p2);
                }
            }
            int x = candleData.TailCoord.High.X;
            if (index == 1)
            {
                LastX = x;
            }
            if (LastX - x > PERIOD_SPLIT_LINE || index == 1)
            {
                var p1 = new Point(x, this.Panel.Rect.Y);
                var p2 = new Point(x, this.Panel.Rect.Y + this.Panel.Rect.Height);
                string min = candleData.Candle.Time.Minute.ToString();
                string hour = candleData.Candle.Time.Hour.ToString();
                string time = candleData.Candle.Time.Day.ToString() + "." + candleData.Candle.Time.Month.ToString()
                    + "/" + (hour.Length < 2 ? '0' + hour : hour) + ":" + (min.Length < 2 ? '0' + min : min);
                //if (LastX - x > 70) time = candleData.Candle.Time.Day.ToString() + "/" + candleData.Candle.Time.Month.ToString() + " " + time;

                lineVer.ColorLine = this.ColorMarkLine;
                lineVer.ColorText = Color.Gray;
                lineVer.WidthLine = 1;
                lineVer.FillText = true;
                lineVer.Paint(PanelCanvas, time, p1, p2);
                LastX = x;
            }
            LastCandle = candleData;
            if (x < 0)
            {
                return;
            }
            index++;
        }
    }


}
