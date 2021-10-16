using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using GraphicTools.Base;
using GraphicTools.Shapes;
using Market.Candles;
using System;
using MarketObjects.Charts;

namespace GraphicTools
{
    ////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>  Класс отвечающий за свечки </summary>
    public class GCandles
    {
        /// <summary> Минимальное кол-во свечек на графике</summary>
        const int MinCountCandle = 5;
        /// <summary> отступ между свечами </summary>
        public float MarginCandle = 1.1f;
        /// <summary>
        /// Текущий тайм фрейм
        /// </summary>
        public int CurrentTimeFrame = 0;
        /// <summary>
        /// Панель отрисовки
        /// </summary>
        public ViewPanel Panel = null;
        public ViewPanel PanelLastCandle = null;

        private Color ColorGrowCandle = Color.DarkSeaGreen;
        private Color ColorFallCandle = Color.LightCoral;
        private Color ColorBorderCandle = Color.FromArgb(140, Color.Black);

        /// <summary>
        /// Координаты для хвостов свечи
        /// </summary>
        public struct TailCoord
        {
            public Point High;
            public Point Low;
        }
        /// <summary>  Данные по свечке  </summary>
        public class CandleInfo
        {
            /// <summary> Прямоугольник отрисовки </summary>
            public Rectangle PaintRect;
            /// <summary> Данные по свечке </summary>
            public CandleData Candle;
            /// <summary> Соординаты хвостов </summary>
            public TailCoord TailCoord;
            /// <summary> Координаты тела свечи </summary>
            public RectangleF Body;
            /// <summary>
            /// Тайм-фрей которому пренадлежит свеча
            /// </summary>
            public int TimeFrame = 0;
            /// <summary> Индекс свечи </summary>
            public int Index = -1;
            /// <summary> Текущий горизонтальный объем </summary>
            public ChartFull CurHorVolume = null;
            /// <summary> Максимальный горизонтальный объем </summary>
            public Chart MaxHorVolume = new Chart();
            /// <summary> Описание </summary>
            public string Description = "";
            /// <summary>
            /// Флаг, что свеча первая в коллекции
            /// </summary>
            public bool First = false;
            /// <summary>
            /// Флаг, что свеча последняя в коллекции
            /// </summary>
            public bool Last = false;
            /// <summary>
            /// Предыдущая свечка, справа на лево
            /// </summary>
            public CandleInfo PrevCandleInfo = null;
        }
        /// <summary>
        /// Класс максимальны и минимальных значений
        /// </summary>
        public class MaxData
        {
            public decimal MaxPrice = 0;
            public decimal MinPrice = 10000000;
            public CandleData MaxCandle = null;
            public CandleData MinCandle = null;
            public Chart MaxHorVolume = new Chart();
        }
        /// <summary>  Максимальные значения </summary>
        public MaxData MaxValues = new MaxData();
        /// <summary> Добытие достижения лимитов Max Min по цене </summary>
		public event System.Action<decimal, decimal, decimal> OnReachMinMax;

        public delegate void EventPointCandle(CandleInfo dataCandle);
        /// <summary> Событие рисования свечки </summary>
        public event EventPointCandle OnPaintedCandle;

        /// <summary> Событие нахождения указателя мыши на вертикале свечки </summary>
        public event EventPointCandle OnMoveVerticalCandle;

        public delegate void EventCandles(int index, CandleData candle, int count);
        public delegate void EventPaintedCandles(CandleInfo dCandle);
        /// <summary>
        /// Событие прохода каждой свечи из коллекции
        /// </summary>
        public EventCandles OnEachCandle;
        /// <summary>
        /// Событие отрисованной свечи
        /// </summary>
        public EventPaintedCandles OnEachPaintedCandle;

        /// <summary> Перед отрисовкой свечей </summary>
        public event Action OnBeforePaintCandle;
        /// <summary> Событие после отрисовки свечей </summary>
        public event Action OnPaintedCandles = null;

        public delegate void EventPointHorVolumes(int Y, decimal Price, long Volume);
        /// <summary> Событие рисования линии горизонтального объема </summary>
        //public event EventPointHorVolumes OnPaintHorVolume = null;

        /// <summary> Коллекция свечек для отображения </summary>
        public IEnumerable<CandleData> CollectionCandle = null;

        /// <summary> Кол-во рисуемых свечей (Масштаб) </summary>
        public int CountPaintCandle = 0;
        /// <summary>
        /// Кол-во свечей в коллекции от текущей.
        /// </summary>
        public int Count = 0;

        /// <summary> Ширина одной свечи </summary>
        public float WidthCandle { get { return Panel.Params.WidthCandle; } }

        /// <summary> Все данные по отрисованным свечкам  </summary>
        public List<CandleInfo> AllDataPaintedCandle = new List<CandleInfo>();
        /// <summary>
        /// Коллекция линий по свечкам
        /// </summary>
        public float[] VerticalLines = null;

        /// <summary> 0 - свечка из коллекции </summary>
        public CandleData FirstCandle
        {
            get
            {
                if (CollectionCandle.IsNull() || Count == 0) return null;
                return CollectionCandle.First();
            }
        }
        public CandleData LastCandle
        {
            get
            {
                if (CollectionCandle.IsNull() || Count == 0) return null;
                return CollectionCandle.Last();
            }
        }

        public GCandles(BaseParams param)
        {
            Panel = new ViewPanel(param);
            PanelLastCandle = new ViewPanel(param);
            Panel.OnChangeRect += (rect) =>
            {
                PanelLastCandle.SetRect(rect);
            };

        }
        /// <summary> Проверка корректности и наполненнсоти коллекции. </summary>
        /// <returns>true - если коллекция существует и не пуста.</returns>
        public bool IssetCollection()
        {
            if (CollectionCandle.NotIsNull() &&
                CollectionCandle.ToArray().Length > 0) return true;
            return false;
        }

        /// <summary> Получить ширину одной свечки </summary>
        /// <returns></returns>
        public float GetWidthOne()
        {
            CountPaintCandle = CountPaintCandle < MinCountCandle ? MinCountCandle : CountPaintCandle;
            Panel.Params.WidthCandle = (float)Panel.Rect.Width / (CountPaintCandle + MarginCandle);

            VerticalLines = new float[CountPaintCandle];
            Array.Clear(VerticalLines, 0, CountPaintCandle);

            return Panel.Params.WidthCandle;
        }
        /// <summary> Вызываем событие активной свечки </summary>
        /// <param name="candle"></param>
        public void MoveVerticalActiveCandle(GCandles.CandleInfo candle)
        {
            if (OnMoveVerticalCandle.NotIsNull() && candle.NotIsNull())
                OnMoveVerticalCandle(candle);
        }

        /// <summary> Отрисовывет свечи</summary>
        /// <param name="canvas"></param>
        /// <param name="rectPaint"></param>
        public void PaintCandles()
        {
            if (CountPaintCandle == 0) return;
            Panel.Params.WidthCandle = GetWidthOne();

            Panel.Clear();
            var canvas = Panel.GetGraphics;

            AllDataPaintedCandle.Clear();

            MaxValues = new MaxData();

            //Событие перед отрисовкой
            if (!OnBeforePaintCandle.IsNull())
                OnBeforePaintCandle();

            List<Chart> HVolume = new List<Chart>();
            int index = 1;
            Count = CollectionCandle.Count();
            CollectionCandle.ForEach<CandleData>((candleData) =>
            {
                if (OnEachCandle.NotIsNull())
                {
                    OnEachCandle(index - 1, candleData, Count);
                }
                if (index <= CountPaintCandle)
                {
                    var lastPCan = PaintOneCandle(canvas, Panel, candleData, index, index > 1 ? true : false);
                    if (OnEachPaintedCandle.NotIsNull())
                    {
                        if(index == 1)
                        {
                            lastPCan.First = true;
                        }
                        if(index == CountPaintCandle)
                        {
                            lastPCan.Last = true;
                        }
                        OnEachPaintedCandle(lastPCan);
                    }
                }
                index++;
            });

            if (!OnPaintedCandles.IsNull())
                OnPaintedCandles();
        }

        /// <summary>  Отрисовка текущей свечи (самой правой) </summary>
        public bool PaintLastCandle(CandleInfo candle)
        {
            PanelLastCandle.Clear();
            if (CollectionCandle.IsNull())
            {
                return false;
            }            
            var canvas = PanelLastCandle.GetGraphics;
            if (candle.NotIsNull())
            {
                if (PanelLastCandle.Params.AutoSize)
                {
                    //контроль максимальной цены
                    if (candle.Candle.High > PanelLastCandle.Params.MaxPrice)
                    {
                        if (OnReachMinMax.NotIsNull())
                        {
                            OnReachMinMax(candle.Candle.High, PanelLastCandle.Params.MaxPrice, PanelLastCandle.Params.MinPrice);
                        }
                        return false;
                    }
                    //контроль минимальной цены
                    if (candle.Candle.Low < PanelLastCandle.Params.MinPrice)
                    {
                        if (OnReachMinMax.NotIsNull())
                        {
                            OnReachMinMax(candle.Candle.Low, PanelLastCandle.Params.MaxPrice, PanelLastCandle.Params.MinPrice);
                        }
                        return false;
                    }
                }
                PaintOneCandle(canvas, PanelLastCandle, candle.Candle, 1);
            }
            return true;
        }
        /// <summary>
        /// Перенос текущей отрисованной свечи на полотно
        /// </summary>
        /// <param name="canvas"></param>
        public void LayoutLastCandleToCanvas(Graphics canvas)
        {
            if (PanelLastCandle.NotIsNull())
            {
                PanelLastCandle.Paint(canvas);
            }
        }
        /// <summary> Рисует одну свечку </summary>
        /// <param name="canvas"></param>
        /// <param name="rectPaint"></param>
        /// <param name="candleData"></param>
        /// <param name="index"></param>
        /// <param name="maxPrice"></param>
        /// <param name="minPrice"></param>
        private CandleInfo PaintOneCandle(Graphics canvas, ViewPanel panel, CandleData candleData, int index, bool paint = true)
        {
            int tailY1 = GMath.GetCoordinate(panel.Rect.Height, panel.Params.MaxPrice, panel.Params.MinPrice, candleData.High);
            int tailY2 = GMath.GetCoordinate(panel.Rect.Height, panel.Params.MaxPrice, panel.Params.MinPrice, candleData.Low);
            int tailX1 = (int)((panel.Rect.Width - panel.Params.WidthCandle * index) + panel.Params.WidthCandle / 2);

            int bodyX = (int)(panel.Rect.Width - panel.Params.WidthCandle * index);
            int bodyY = GMath.GetCoordinate(panel.Rect.Height, panel.Params.MaxPrice, panel.Params.MinPrice, candleData.Open > candleData.Close ? candleData.Open : candleData.Close);

            float bodyWidth = panel.Params.WidthCandle - MarginCandle > 0.3f ? panel.Params.WidthCandle - MarginCandle : 0.3f; //- чтобы свечки не слипались
            int bodyHeight = GMath.GetCoordinate(panel.Rect.Height, panel.Params.MaxPrice, panel.Params.MinPrice, candleData.Open < candleData.Close ? candleData.Open : candleData.Close);
            bodyHeight = bodyHeight - bodyY;
            bodyHeight = bodyHeight == 0 ? bodyHeight + 1 : bodyHeight;

            //tail
            var line = new Line();
            line.Width = 1.5f;

            if (paint)
            {
                line.Paint(canvas, new Point(tailX1, tailY1), new Point(tailX1, tailY2), ColorBorderCandle);
            }
            //Body
            var rect = new RectDraw();
            var colorCandle = candleData.Open > candleData.Close ? ColorFallCandle : ColorGrowCandle;
            var colorBorder = ColorBorderCandle;

            /*if (candleData.Open < candleData.Close && candleData.VolumeBuy - candleData.VolumeSell > 0)
			{
				colorBorder = colorCandle = Color.DarkGreen;
			}
			if (candleData.Open > candleData.Close && candleData.VolumeBuy - candleData.VolumeSell < 0)
			{
				colorBorder = colorCandle = Color.DarkRed;
			}*/
            if (paint)
            {
                rect.Paint(canvas, bodyX, bodyY, bodyWidth, bodyHeight, colorBorder, colorCandle);
            }

            if (MaxValues.MinCandle.IsNull()) MaxValues.MinCandle = candleData;
            if (MaxValues.MaxCandle.IsNull()) MaxValues.MaxCandle = candleData;
            if (MaxValues.MaxPrice < candleData.High)
            {
                MaxValues.MaxPrice = candleData.High;
                MaxValues.MaxCandle = candleData;
            }
            if (MaxValues.MinPrice > candleData.Low)
            {
                MaxValues.MinPrice = candleData.Low;
                MaxValues.MinCandle = candleData;
            }
            /*if (candleData.HorVolumes.HVolCollection.MaxVolume.NotIsNull())
            {
                if (MaxValues.MaxHorVolume.Volume < candleData.HorVolumes.HVolCollection.MaxVolume.Volume)
                {
                    MaxValues.MaxHorVolume = candleData.HorVolumes.HVolCollection.MaxVolume;
                }
            }*/

            var dCandle = new CandleInfo()
            {
                PaintRect = panel.Rect.Rectangle,
                Candle = candleData,
                TailCoord = new TailCoord() { Low = new Point(tailX1, tailY2), High = new Point(tailX1, tailY1) },
                Body = new RectangleF() { X = bodyX, Y = bodyY, Width = bodyWidth, Height = bodyHeight },
                Index = index - 1,
                MaxHorVolume = MaxValues.MaxHorVolume,
                TimeFrame = CurrentTimeFrame
            };
            VerticalLines[index - 1] = dCandle.TailCoord.High.X;
            if (index == 1)
            {
                AllDataPaintedCandle.RemoveAll(c => c.Index == index - 1);
                AllDataPaintedCandle.Insert(0, dCandle);
            }
            else
            {
                AllDataPaintedCandle.Add(dCandle);
            }

            if (OnPaintedCandle.NotIsNull())
            {
                OnPaintedCandle(dCandle);
            }
            return dCandle;
        }
    }
}
