using AppVEConector;
using GraphicTools.Base;
using GraphicTools.Shapes;
using QuikConnector.MarketObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static GraphicTools.GCandles;

namespace GraphicTools
{
    public class LevelsFree
    {
        public ViewPanel Panel = null;

        /// <summary>
        /// Видимый на графике или нет
        /// </summary>
        public bool Visible = true;

        public delegate void eventLevel(DoubleLevel newLevel);

        /// <summary>
        /// Событие нового уровня
        /// </summary>
        public eventLevel OnNewLevel = null;
        /// <summary>
        /// Типы уровней
        /// </summary>
        public enum TYPE_LEVELS
        {
            Nothing = 0,
            Vector = 1,
            Rectangle = 2,
            HorizontLine = 3,
            FreeLine = 10,
        };
        /// <summary>
        /// Тип уровня
        /// </summary>
        public TYPE_LEVELS TypeLevel = TYPE_LEVELS.Nothing;

        public LevelsFree(BaseParams param)
        {
            Panel = new ViewPanel(param);
        }

        public void CallEventNewLevel(DoubleLevel newLevel)
        {
            if (OnNewLevel.NotIsNull())
            {
                OnNewLevel(newLevel);
            }
        }
        [Serializable]
        public class DoubleLevel
        {
            /// <summary>
            /// Дата для определения от какой свечи рисовать
            /// </summary>
            public decimal Top = 0;
            public decimal Bottom = 0;
            public DateMarket DateLeft = new DateMarket();
            public DateMarket DateRight = new DateMarket();
            public override string ToString()
            {
                return Top.ToString() + LevelTools.SPLITTER +
                    Bottom.ToString() + LevelTools.SPLITTER +
                    (DateLeft.IsNull() ? "" : DateLeft.ToString()) + LevelTools.SPLITTER +
                    (DateRight.IsNull() ? "" : DateRight.ToString())
                    ;
            }
        }
        public IEnumerable<DoubleLevel> Collection = null;
        /// <summary>
        /// Цвет уровней
        /// </summary>
        public Color Color = Color.Blue;

        private class LevelRect
        {
            public DoubleLevel level = null;
            public CandleInfo candleLeft = null;
            public CandleInfo candleRight = null;
        }

        private List<LevelRect> listTmpLevRect = new List<LevelRect>();

        public void PaintByCandle(CandleInfo candle, CandleInfo leftCandle, CandleInfo rightCandle, int count)
        {
            if (!Visible) return;
            if (this.Collection.IsNull()) return;
            if (candle.IsNull()) return;
            var canvas = Panel.GetGraphics;
            //Поиск всех уровней попадающий в окно видимости
            if (candle.Index == 0)
            {
                SearchRectLevel(leftCandle, rightCandle);
            }
            if (candle.PrevCandleInfo.NotIsNull())
            {
                //Ищем правые грани уровня
                listTmpLevRect.Where(l =>
                    l.level.DateRight.DateTime >= candle.Candle.Time
                    && l.level.DateRight.DateTime < candle.PrevCandleInfo.Candle.Time
                ).ForEach<LevelRect>((lev) => { lev.candleRight = candle; });
                // Ищем левые грани уровня
                listTmpLevRect.Where(l =>
                    l.level.DateLeft.DateTime >= candle.Candle.Time
                    && l.level.DateLeft.DateTime < candle.PrevCandleInfo.Candle.Time
                ).ForEach<LevelRect>((lev) => { lev.candleLeft = candle; });
            }
            //Обработка на последней свече
            if (count == candle.Index + 1)
            {
                //Рисуем прямоугольные уровни
                foreach (var levRect in listTmpLevRect.ToArray())
                {
                    bool paint = false;
                    Rectangle rect = new Rectangle();
                    //Прямоугольный уровень
                    if (levRect.candleLeft.NotIsNull() && levRect.candleRight.NotIsNull())
                    {
                        var width = Panel.Rect.Width - levRect.candleLeft.TailCoord.High.X;
                        rect = (GRectangle.GetRight(Panel.Rect.Clone(), width)).Rectangle;
                        levRect.candleRight.TailCoord.High.X = levRect.candleRight.TailCoord.High.X < 0 ? 0 : levRect.candleRight.TailCoord.High.X;
                        levRect.candleLeft.TailCoord.High.X = levRect.candleLeft.TailCoord.High.X < 0 ? 0 : levRect.candleLeft.TailCoord.High.X;
                        rect.Width = levRect.candleRight.TailCoord.High.X - levRect.candleLeft.TailCoord.High.X;
                        paint = true;
                    }
                    //Линейный уровень
                    else if (levRect.candleLeft.NotIsNull())
                    {
                        var time = levRect.level.DateLeft.GetDateTime();
                        if (time <= rightCandle.Candle.Time)
                        {
                            var width = Panel.Rect.Width - levRect.candleLeft.TailCoord.High.X;
                            rect = (GRectangle.GetRight(Panel.Rect.Clone(), width)).Rectangle;
                            levRect.candleLeft.TailCoord.High.X = levRect.candleLeft.TailCoord.High.X < 0 ? 0 : levRect.candleLeft.TailCoord.High.X;
                            paint = true;
                        }
                    }
                    if (paint)
                    {
                        rect.X = rect.X < 0 ? 0 : rect.X;
                        if (levRect.level.Bottom <= 0 && levRect.level.Top > 0)
                        {
                            PaintLineLevel(canvas, rect, levRect.level, 2f);
                        }
                        else
                        {
                            PaintRectangleLevel(canvas, rect, levRect.level, 1);
                        }
                    }
                    listTmpLevRect.Clear();
                }
            }
        }
        /// <summary>
        /// Поиск прямоугольных уровней
        /// </summary>
        /// <param name="canLeft"></param>
        /// <param name="canRight"></param>
        private void SearchRectLevel(CandleInfo canLeft, CandleInfo canRight)
        {
            //Ищем Прямоугольные уровни которые попадают в окно видимости
            var levels = new List<DoubleLevel>();
            levels.AddRange(this.Collection.Where(l =>
            l.DateLeft.NotIsNull() && l.DateRight.NotIsNull() && l.Bottom > 0 && (
                (l.DateLeft.GetDateTime() <= canLeft.Candle.Time && l.DateRight.GetDateTime() >= canRight.Candle.Time) ||
                (l.DateLeft.GetDateTime() >= canLeft.Candle.Time && l.DateRight.GetDateTime() <= canRight.Candle.Time) ||
                (l.DateLeft.GetDateTime() < canLeft.Candle.Time && l.DateRight.GetDateTime() < canRight.Candle.Time && l.DateRight.GetDateTime() > canLeft.Candle.Time) ||
                (l.DateLeft.GetDateTime() > canLeft.Candle.Time && l.DateRight.GetDateTime() > canRight.Candle.Time && l.DateLeft.GetDateTime() < canRight.Candle.Time)
            )));
            //Линейные уровни которые попадают в окно видимости
            levels.AddRange(this.Collection.Where(l =>
                l.DateLeft.NotIsNull() && l.Bottom <= 0 && l.DateLeft.GetDateTime() <= canRight.Candle.Time
            ));
            levels.ForEach<DoubleLevel>((level) =>
            {
                listTmpLevRect.Add(new LevelRect()
                {
                    level = level,
                    candleLeft = canLeft,
                    candleRight = (level.Bottom > 0 ? canRight : null),
                }); ;
            });
        }
        /// <summary>
        /// Рисует уровень в виде луча
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="lev"></param>
        /// <param name="widthLine"></param>
        private void PaintLineLevel(Graphics g, Rectangle rect, DoubleLevel lev, float widthLine = 1)
        {
            var horLine = new HorLine();
            horLine.ColorLine = horLine.ColorText = Color.Blue;
            horLine.TextHAlign = HorLine.DirectionLine.Left;
            horLine.WidthLine = widthLine;
            horLine.FillText = true;
            horLine.Paint(g, rect, lev.Top, lev.Top.ToString(), Panel.Params.MaxPrice, Panel.Params.MinPrice);
        }
        /// <summary>
        /// Рисует уровень ввиде свободного прямоугольника
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="lev"></param>
        /// <param name="widthLine"></param>
        private void PaintRectangleLevel(Graphics g, Rectangle rect, DoubleLevel lev, float widthLine = 1)
        {
            int yTop = GMath.GetCoordinate(rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, lev.Top);
            int yBot = GMath.GetCoordinate(rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, lev.Bottom);
            if (yBot < 0 && yTop > 0) yBot = rect.Y + rect.Height;
            if (yTop < 0 && yBot > 0) yTop = rect.Y;

            var height = yBot - yTop;
            if (height <= 0) return;
            var rectLev = new RectDraw();
            rectLev.ColorBorder = Color.FromArgb(100, Color.Black);
            rectLev.ColorFill = Color.FromArgb(40, Color.Blue);
            rectLev.WidthBorder = widthLine;
            rectLev.Paint(g, rect.X, yTop, rect.Width, height);

            var middleLine = new Line();
            middleLine.Width = 1;
            middleLine.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            middleLine.Paint(g, new PointF(rect.X, yTop + height / 2), new PointF(rect.X + rect.Width, yTop + height / 2),
                Color.FromArgb(100, Color.Black));

            var topText = new TextDraw();
            var dataText = topText.GetSizeText(g, lev.Top.ToString());
            int HeightText = (int)dataText.Height;
            if (height > HeightText * 3 && rect.Width > dataText.Width)
            {
                topText.Paint(g, lev.Top.ToString(), rect.X, yTop);
                topText.Paint(g, lev.Bottom.ToString(), rect.X, yBot - HeightText);
            }
        }

        public void Paint()
        {
            if (!Visible) return;
            if (this.Collection.IsNull()) return;
            int count = this.Collection.Count();
            if (count == 0) return;
            var canvas = Panel.GetGraphics;

            foreach (var lev in this.Collection.Where(l => l.DateLeft.IsNull() && l.DateRight.IsNull()))
            {
                if (lev.Bottom <= 0 && lev.Top > 0)
                {
                    PaintLineLevel(canvas, Panel.Rect.Rectangle, lev, 2f);
                }
                else if (lev.Top > 0 && lev.Bottom > 0)
                {
                    PaintRectangleLevel(canvas, Panel.Rect.Rectangle, lev);
                }
            }
        }
    }
}
