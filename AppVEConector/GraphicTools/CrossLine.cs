using GraphicTools.Base;
using GraphicTools.Shapes;
using System;
using System.Drawing;

namespace GraphicTools
{
    public class CrossLine
    {
        /// <summary>
        /// Высота тайм линий
        /// </summary>
        public int HeightForPrice = 0;

        public ViewPanel Panel = null;

        public struct DataCross
        {
            public decimal Price;
            public DateTime Time;
            public Point Point;
            public GCandles.CandleInfo Candle;
        }

        private DataCross Data = new DataCross();

        public DataCross GetDataCross()
        {
            return Data;
        }

        public CrossLine(BaseParams param)
        {
            Panel = new ViewPanel(param);
        }
        /// <summary>Отрисовка перекрестья</summary>
        /// <param name="coord"></param>
        public void PaintCrossLines(Point coord, GCandles.CandleInfo candle)
        {
            Panel.Clear();
            var canvas = Panel.GetGraphics;
            string time = "";
            Data = new DataCross();
            if (candle.NotIsNull())
            {
                Data.Candle = candle;
                Data.Time = Data.Candle.Candle.Time;
                string d = Data.Time.Day.ToString();
                string m = Data.Time.Month.ToString();
                string y = Data.Time.Year.ToString();
                string min = Data.Time.Minute.ToString();
                string hour = Data.Time.Hour.ToString();
                time = (d.Length < 2 ? '0' + d : d) + "." + (m.Length < 2 ? '0' + m : m) + "." + y + " " +
                    (hour.Length < 2 ? '0' + hour : hour) + ":" + (min.Length < 2 ? '0' + min : min);
            }
            Data.Price = GMath.GetValueFromCoordinate(this.HeightForPrice, Panel.Params.MaxPrice, Panel.Params.MinPrice, coord.Y, Panel.Params.CountFloat);

            var lineVer = new VerLine();
            lineVer.ColorLine = lineVer.ColorText = Color.Black;
            lineVer.WidthLine = 1;
            lineVer.FillText = true;
            lineVer.Paint(canvas, time, new Point(coord.X, 0), new Point(coord.X, Panel.Rect.Height));
            Data.Point.X = coord.X;

            var horLine = new HorLine();
            horLine.ColorLine = horLine.ColorText = Color.Black;
            horLine.FillText = true;
            horLine.Paint(canvas, Panel.Rect.Rectangle, coord.Y, Data.Price.ToString(), Panel.Params.MaxPrice, Panel.Params.MinPrice);
            Data.Point.Y = coord.Y;

            var priceText = new TextDraw();
            var TextAppendByCandle = "";
            //Данные по свечи в углу
            if (Data.Candle.NotIsNull())
            {
                priceText.Color = Color.Black;
                priceText.SetFontSize(8);
                priceText.Paint(canvas, "T: " + time + "\r\n" +
                    "O: " + Data.Candle.Candle.Open.ToString() + "\r\n" +
                    "H: " + Data.Candle.Candle.High.ToString() + "\r\n" +
                    "L: " + Data.Candle.Candle.Low.ToString() + "\r\n" +
                    "C: " + Data.Candle.Candle.Close.ToString() + "\r\n" +
                    "V: " + Data.Candle.Candle.Volume.ToString() + "\r\n"
                    , 0, 0);
                TextAppendByCandle = "Vol:" + Data.Candle.Candle.Volume;
            }

            //Текщие данные по цене и времени
            priceText.Color = Color.Black;
            priceText.SetFontSize(8);
            priceText.Paint(canvas, Data.Price.ToString() + "\r\n"
                + time + "\r\n"
                + TextAppendByCandle
                , coord.X + 10, coord.Y + 20);


        }
    }
}
