using GraphicTools.Base;
using GraphicTools.Shapes;
using GraphicTools.Extension;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static GraphicTools.GCandles;
using MarketObjects.Charts;

namespace GraphicTools
{
    ////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Вертикальные уровни. Расположенные ниже графика или сверху.
    /// </summary>
    public class GVerLevel : BaseLevels
    {
        /// <summary>
        /// Область отрисовки
        /// </summary>
        public ViewPanel Panel = null;
        /// <summary>
        /// Область отрисовки последнего значения
        /// </summary>
        public ViewPanel PanelLast = null;
        /// <summary> Область значений шкалы </summary>
        public GRightValue Values = null;
        /// <summary>
        /// Цвет положительных значений
        /// </summary>
        public Color ValuePositive = Color.DarkGreen;
        /// <summary>
        /// Цвет отрицательных значений
        /// </summary>
        public Color ValueNegative = Color.DarkRed;
        /// <summary>
        /// Максимальная ширина единицы индикатора
        /// </summary>
        const int MAX_WIDTH = 2;

        public class DataLevel
        {
            /// <summary> Прямоугольник отрисовки </summary>
            public Rectangle PaintRect;
            /// <summary> Координаты тела </summary>
            public RectangleF Body = new Rectangle();
            /// <summary> Данные отрисовки по уровню </summary>
            public RectDraw DataPaint;
            /// <summary> Индекс свечи </summary>
            public int Index = -1;

            public decimal Value = -1;
            public decimal MaxValue = -1;
            public decimal MinValue = -1;
            /// <summary> Описание </summary>
            public string Description = "";
        }

        /// <summary>
        /// Список данных уровней
        /// </summary>
        public List<DataLevel> AllDataLevels = new List<DataLevel>();

        /// <summary> Добытие достижения лимитов Max Min по значению </summary>
        public event System.Action<decimal, decimal, decimal> OnReachLimitValues;

        public GVerLevel(BaseParams param)
        {
            Panel = new ViewPanel(param);
            PanelLast = new ViewPanel(param);
            Values = new GRightValue(param);

            Values.ColorLines = Color.DarkGray;
            Values.ColorText = Color.Black;

            Panel.OnChangeRect += (rect) =>
            {
                Values.Panel.SetRect(rect);
                PanelLast.SetRect(rect);
            };
        }
        public Chart GetFirstLevel()
        {
            var elem = CollectionLevels.ElementAt(0);
            return elem.NotIsNull() ? elem : null;
        }
        

        /// <summary> Рисует уровни </summary>
        /// <param name="canvas"></param>
        public void PaintByCandle(CandleInfo candle)
        {
            //this.Panel.Clear();
            if (candle.Index >= CollectionLevels.Count) return;
            var canvas = this.Panel.GetGraphics;
            //this.AllDataLevels.Clear();

            if (this.CollectionLevels.IsNull()) return;
            if (candle.Index == 0)
            {
                Values.Paint(this.Max, this.Min);
                Values.Panel.Paint(canvas);
            }

            var level = CollectionLevels.ElementAt(candle.Index);
            if (level.NotIsNull())
            {
                this.PaintOneLevel(canvas, level, candle);
            }
            (new Line()).Paint(canvas, new Point(Panel.Rect.X, Panel.Rect.Y), new Point(Panel.Rect.X + Panel.Rect.Width, Panel.Rect.Y), Color.Black);
        }

        public bool PaintLast(CandleInfo candle)
        {
            PanelLast.Clear();
            if (CollectionLevels.IsNull() || this.CollectionLevels.Count == 0) return false;
            var canvas = PanelLast.GetGraphics;
            var level = this.CollectionLevels.ElementAt(0);
            if (level.NotIsNull())
            {
                if (level.Volume > this.Max || level.Volume < this.Min)
                {
                    if (this.OnReachLimitValues.NotIsNull())
                    {
                        this.OnReachLimitValues(level.Volume, this.Max, this.Min);
                    }
                    return false;
                }
                else
                {
                    PaintOneLevel(canvas, level, candle, true);
                    Values.PaintCurrentValue(level.Volume, this.Max, this.Min);
                    Values.PanelCurValue.Paint(canvas);
                }
            }
            return true;
        }
        /// <summary>
        /// Отрисовка одного уровня
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rectPaint"></param>
        /// <param name="Value"></param>
        /// <param name="index"></param>
        private void PaintOneLevel(Graphics canvas, Chart level, CandleInfo candle, bool paintLast = false)
        {
            int Y1 = GMath.GetCoordinate(Panel.Rect.Height, this.Max, this.Min, level.Volume > 0 ? level.Volume : 0);
            int Y2 = GMath.GetCoordinate(Panel.Rect.Height, this.Max, this.Min, level.Volume < 0 ? level.Volume : 0);
            
            float bodyWidth = candle.Body.Width < MAX_WIDTH ? candle.Body.Width : MAX_WIDTH;//Panel.Params.WidthCandle - Panel.Params.MarginCandle; //-2 чтобы свечки не слипались
            int bodyHeight = Y2 - Y1;
            
            int bodyX = (int)(candle.Body.X + (candle.Body.Width / 2) - (bodyWidth / 2));
            int bodyY = Y1;
            //bodyHeight = bodyHeight - bodyY;
            bodyHeight = bodyHeight == 0 ? bodyHeight + 1 : bodyHeight;

            var rect = new RectDraw();
            if (candle.Index != 0 || paintLast)
            {
                var Color = ValuePositive;
                if (candle.Candle.Close < candle.Candle.Open)
                {
                    Color = ValueNegative;
                }
                rect.Paint(canvas, bodyX, bodyY, bodyWidth, bodyHeight, Color, Color);
            }

            var data = new DataLevel()
            {
                PaintRect = Panel.Rect.Rectangle,
                Value = level.Volume,
                MaxValue = this.Max,
                MinValue = this.Min,
                Index = candle.Index,
                DataPaint = rect,
                Body = new RectangleF(bodyX, bodyY, bodyWidth, bodyHeight)
            };
            if (data.NotIsNull())
            {
                if (data.Index == 1)
                {
                    this.AllDataLevels.RemoveAll(l => l.Index == data.Index);
                    AllDataLevels.Insert(0, data);
                }
                else AllDataLevels.Add(data);
            }
        }
    }
}
