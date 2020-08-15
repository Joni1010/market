using AppVEConector.GraphicTools.Base;
using AppVEConector.GraphicTools.Indicators;
using GraphicTools.Base;
using GraphicTools.Shapes;
using Managers;
using Market.Candles;
using MarketObjects;
using QuikConnector.MarketObjects;
using QuikControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using static GraphicTools.GCandles;

namespace GraphicTools.Extension
{
    public class BGraphAdd : BGraphExt
    {
        /// <summary>
        /// Source timeframe
        /// </summary>
        public object DataSourceTimeFrame = null;

        /// <summary>
        /// Набор индикаторов
        /// </summary>
        public List<object> Indicators = new List<object>();
        /// <summary>
        /// Source levels
        /// </summary>
        public object DataSourceLevels
        {
            set
            {
                Levels.Collection = (List<LevelsFree.DoubleLevel>)value;
            }
        }

        /// <summary>
        /// Кол-во свечей на графике
        /// </summary>
        public int CountVisibleCandles = 25;
        /// <summary>
        /// Индекс свечи с которой начинается отображение
        /// </summary>
        public int IndexFirstCandle = 0;
        /// <summary> 
        /// Координаты перекрестья 
        /// </summary>
        protected Point CrossPoint = new Point();
        /// <summary> Объемы </summary>
        protected GVerLevel Volumes = null;
        /// <summary> Дельта объемов </summary>
        //protected GVerLevel DeltaVol = null;
        /// <summary>
        /// Индикатор настроения спроса/предложения
        /// </summary>
        //protected GVerLevel Interest = null;

        /// <summary> Данные по горизонтальным объемам </summary>
        public GHorVol GHorVolumes = null;
        /// <summary> Тип рисования горизонтального объема. </summary>
        protected int TypeHorVolume = 0;

        /// <summary> Уровни заявок </summary>
        protected LevelsOrders LevelsOrders = null;
        /// <summary> Свободные уровни </summary>
        public LevelsFree Levels = null;

        /// <summary> Линии перекрестья </summary>
        protected CrossLine Cross = null;

        /// <summary> Событие изменения размеров полотна </summary>
		public event System.Action<GRectangle> OnResize;

        /// <summary> Добытие достижения лимитов Max Min по цене </summary>
        protected event System.Action<decimal, decimal, decimal> OnReachLimitPrice;

        /// <summary>
        /// Активные свечи
        /// </summary>
        public PeriodActCandles ActiveCandles = new PeriodActCandles();

        protected DragAndDrop DragAndDrop = new DragAndDrop();

        private Thread ThreadPaintHotVol = null;

        /// <summary>
        /// Получить данные по перекрестью
        /// </summary>
        /// <returns></returns>
        public CrossLine.DataCross GetDataCross()
        {
            return Cross.GetDataCross();
        }

        public ActiveTrades ActiveTrades = null;
        public IndicatorATR ATR = null;
        public IndicatorPaintLevels LevelsSignal = null;

        protected void InitAdd()
        {
            InitExt();
            Cross = new CrossLine(MainPanel.Params);
            Volumes = new GVerLevel(MainPanel.Params);

            //Interest = new GVerLevel(MainPanel.Params);
            GHorVolumes = new GHorVol(MainPanel.Params);

            LevelsOrders = new LevelsOrders(MainPanel.Params);
            Levels = new LevelsFree(MainPanel.Params);

            Indicators.Clear();
            Indicators.Add(new MovingAverage(Candels.Panel));
            Indicators.Add(new IndicatorCTHV(Candels.Panel));
            Indicators.Add(new IndicatorHV(Candels.Panel));
            Indicators.Add(ActiveTrades = new ActiveTrades(Candels.Panel));
            Indicators.Add(ATR = new IndicatorATR(Candels.Panel));
            Indicators.Add(LevelsSignal = new IndicatorPaintLevels(Candels.Panel));

            Volumes.Values.CountLineValue = 5;
            //DeltaVol.Values.CountLineValue = 4;
            //Interest.Values.CountLineValue = 5;

            MainPanel.OnChangeRect += (rect) =>
            {
                //Расчет всех панелей
                GetRectsPanels();
                if (OnResize.NotIsNull())
                {
                    OnResize(rect);
                }
            };
            InitLoopsEachCandle();
        }

        /// <summary> Расчет мин и макс цены на всем графике </summary>
		protected void GetMinMax()
        {
            if (MainPanel.Params.AutoSize)
            {
                GetMinMaxBase();

                //Ищем макс и мин в заявках
                if (LevelsOrders.CollectionOrders.NotIsNull())
                {
                    if (LevelsOrders.CollectionOrders.Count() > 0)
                    {
                        foreach (var ord in LevelsOrders.CollectionOrders.ToArray())
                        {
                            if (MainPanel.Params.MaxPrice < ord.Price) MainPanel.Params.MaxPrice = ord.Price;
                            if (MainPanel.Params.MinPrice > ord.Price) MainPanel.Params.MinPrice = ord.Price;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Определяет масштабирование ( максимальная и минимальная цена)
        /// </summary>
        /// <param name="auto"></param>
        public void AutoScaling(bool auto)
        {
            MainPanel.Params.AutoSize = auto;
        }

        public void SetTypeScaling(BaseParams.TYPE_SCALING type)
        {
            MainPanel.Params.TypeScaling = type;
        }

        /// <summary> Установка параметров </summary>
		/// <param name="countCandles"></param>
		protected void ComputeParams(Rectangle RectCanvas)
        {
            MainPanel.SetRect(new GRectangle(RectCanvas));
        }

        /// <summary> Расчет панелей</summary>
		private void GetRectsPanels()
        {
            Candels.Panel.SetRect(MainPanel.ExtractBottom(0));

            Cross.Panel.SetRect(Candels.Panel.ExtractBottom(0));

            Times.Panel.SetRect(Candels.Panel.ExtractBottom(20));
            Times.Panel.SetRect(GRectangle.Join(Candels.Panel.RectScreen, Times.Panel.RectScreen));

            //Interest.Panel.SetRect(Candels.Panel.ExtractBottom(50));
            //DeltaVol.Panel.SetRect(Candels.Panel.ExtractBottom(50));
            Volumes.Panel.SetRect(Candels.Panel.ExtractBottom(60));

            ///////////////////////////
            /// Right panels
            ///////////////////////////
            RightPrices.Panel.SetRect(Candels.Panel.ExtractRight(RightPrices.WidthBorder));
            RightPrices.Panel.SetRect(GRectangle.Join(Candels.Panel.RectScreen, RightPrices.Panel.RectScreen));

            GHorVolumes.Panel.SetRect(Candels.Panel.RectScreen);
            GHorVolumes.PanelVolume.SetRect(Candels.Panel.ExtractRight(100));
            Candels.Panel.ExtractRight(15);

            //Ширина свечки
            float WidthOneCandle = Candels.GetWidthOne();
            Cross.HeightForPrice = Candels.Panel.RectScreen.Height;
            Times.WidthOneCandle = WidthOneCandle;
            //Ширина области значений в нижних панелях
            Volumes.Values.SetWidthValues(RightPrices.WidthBorder);
            //DeltaVol.Values.SetWidthValues(RightPrices.WidthBorder);
            //Interest.Values.SetWidthValues(RightPrices.WidthBorder);

            LevelsOrders.Panel.SetRect(Candels.Panel.RectScreen);
            Levels.Panel.SetRect(Candels.Panel.RectScreen);
        }

        private void ToCanvas()
        {
            MainPanel.Clear();
            var canvas = MainPanel.GetGraphics;
            canvas.Clear(Color.White);

            Times.Panel.Paint(canvas);
            RightPrices.Panel.Paint(canvas);

            Candels.Panel.Paint(canvas);

            Volumes.Values.Panel.Paint(canvas);
            Volumes.Panel.Paint(canvas);

            LevelsOrders.Panel.Paint(canvas);
            Levels.Panel.Paint(canvas);
        }

        /// <summary>
        /// Рисует слои на холсте
        /// </summary>
        public void AllToCanvas(Graphics graphic)
        {
            Canvas = graphic;
            MainPanel.Paint(Canvas);

            Candels.PanelLastCandle.Paint(Canvas);

            Volumes.Values.PanelCurValue.Paint(Canvas);
            Volumes.PanelLast.Paint(Canvas);


            RightPrices.PanelCurValue.Paint(Canvas);
            Cross.Panel.Paint(Canvas);

            GHorVolumes.Panel.Paint(Canvas);
            GHorVolumes.PanelVolume.Paint(Canvas);

            Indicators.ForEach((objIndicator) =>
            {
                if (objIndicator is Indicator)
                {
                    ((Indicator)objIndicator).ToCanvas(Canvas);
                }
            });

            PaintRectSelectPeriod();
        }

        /// <summary>
        /// Прорисовка прямоугольника выделенного периода
        /// </summary>
        private void PaintRectSelectPeriod()
        {
            if (CrossPoint.X > 0 && ActiveCandles.ActiveCandle1.NotIsNull() &&
                ActiveCandles.ActiveCandle2.IsNull() && ActiveCandles.statusSel())
            {
                //Режим указания уровня
                if (Levels.TypeLevel == LevelsFree.TYPE_LEVELS.Vector)
                {
                    PaintLevelHorVector();
                }
                else if (Levels.TypeLevel == LevelsFree.TYPE_LEVELS.Rectangle)
                {
                    PaintLevelRectangle();
                }
                else
                {
                    var rectSel = new RectDraw();
                    rectSel.ColorBorder = Color.FromArgb(100, Color.Black);
                    rectSel.ColorFill = Color.FromArgb(50, Color.Blue);
                    var x = ActiveCandles.ActiveCandle1.dataCandle.TailCoord.High.X > CrossPoint.X
                        ? CrossPoint.X
                        : ActiveCandles.ActiveCandle1.dataCandle.TailCoord.High.X;
                    var width = CrossPoint.X - ActiveCandles.ActiveCandle1.dataCandle.TailCoord.High.X;
                    int y1Line = 0, y2Line = 0;
                    if (width < 0)
                    {
                        width = width * -1;
                        y2Line = ActiveCandles.ActiveCandle1.coordClick.Y;
                        y1Line = CrossPoint.Y;
                    }
                    else
                    {
                        y1Line = ActiveCandles.ActiveCandle1.coordClick.Y;
                        y2Line = CrossPoint.Y;
                    }
                    rectSel.Paint(Canvas, x, Candels.Panel.Rect.Y, width, Candels.Panel.Rect.Height);

                    var line = new Line();
                    line.Width = 1;
                    line.Paint(Canvas,
                        new PointF(x, y1Line),
                        new PointF(x + width, y2Line),
                        Color.Red);
                }
            }
        }
        /// <summary>
        /// Отрисовка уровня прямоугольного
        /// </summary>
        private void PaintLevelHorVector()
        {
            var line = new HorLine();
            line.ColorLine = Color.Red;
            line.WidthLine = 2.0f;
            line.TextHAlign = HorLine.DirectionLine.Left;
            var Rect = Candels.Panel.Rect.Rectangle;
            Rect.X = CrossPoint.X;
            Rect.Width = Rect.Width - CrossPoint.X;

            line.Paint(Canvas, Rect,
                GMath.GetValueFromCoordinate(Candels.Panel.Rect.Height, Candels.Panel.Params.MaxPrice, Candels.Panel.Params.MinPrice, CrossPoint.Y, Candels.Panel.Params.CountFloat)
                , Candels.Panel.Params.MaxPrice, Candels.Panel.Params.MinPrice);
        }
        private void PaintLevelRectangle()
        {
            var rectSel = new RectDraw();
            rectSel.ColorBorder = Color.FromArgb(100, Color.Blue);
            rectSel.ColorFill = Color.FromArgb(50, Color.Red);
            rectSel.WidthBorder = 2.0f;
            int x1 = 0, y1 = 0, height = 0;
            height = CrossPoint.Y - ActiveCandles.ActiveCandle1.coordClick.Y;
            height = height < 0 ? height * -1 : height;

            if (ActiveCandles.ActiveCandle1.coordClick.X > CrossPoint.X)
            {
                x1 = CrossPoint.X;
            }
            else
            {
                x1 = ActiveCandles.ActiveCandle1.dataCandle.TailCoord.High.X;
            }
            if (ActiveCandles.ActiveCandle1.coordClick.Y > CrossPoint.Y)
            {
                y1 = CrossPoint.Y;
            }
            else
            {
                y1 = ActiveCandles.ActiveCandle1.coordClick.Y;
            }
            var width = CrossPoint.X - ActiveCandles.ActiveCandle1.dataCandle.TailCoord.High.X;
            width = width < 0 ? width * -1 : width;
            rectSel.Paint(Canvas, x1, y1, width, height);
        }
        /// <summary>
        /// Проверка и создание нового уровня
        /// </summary>
        /// <param name="point"></param>
        /// <param name="candle"></param>
        protected void CheckAndCreateNewLevel(Point point, GCandles.CandleInfo candle)
        {
            if (ActiveCandles.ActiveCandle1.NotIsNull() && ActiveCandles.ActiveCandle2.NotIsNull())
            {
                if (Levels.TypeLevel == LevelsFree.TYPE_LEVELS.Vector)
                {
                    Levels.CallEventNewLevel(new LevelsFree.DoubleLevel()
                    {
                        DateLeft = new DateMarket(ActiveCandles.ActiveCandle2.dataCandle.Candle.Time),
                        Top = GMath.GetValueFromCoordinate(Candels.Panel.Rect.Height, Candels.Panel.Params.MaxPrice, Candels.Panel.Params.MinPrice, point.Y, Candels.Panel.Params.CountFloat)
                    });
                }
                else if (Levels.TypeLevel == LevelsFree.TYPE_LEVELS.Rectangle)
                {
                    int y1 = 0, y2 = 0;
                    if (ActiveCandles.ActiveCandle1.coordClick.Y > ActiveCandles.ActiveCandle2.coordClick.Y)
                    {
                        y2 = ActiveCandles.ActiveCandle1.coordClick.Y;
                        y1 = ActiveCandles.ActiveCandle2.coordClick.Y;
                    }
                    else
                    {
                        y1 = ActiveCandles.ActiveCandle1.coordClick.Y;
                        y2 = ActiveCandles.ActiveCandle2.coordClick.Y;
                    }

                    Levels.CallEventNewLevel(new LevelsFree.DoubleLevel()
                    {
                        DateLeft = new DateMarket(ActiveCandles.ActiveCandle1.dataCandle.Candle.Time),
                        DateRight = new DateMarket(ActiveCandles.ActiveCandle2.dataCandle.Candle.Time),
                        Top = GMath.GetValueFromCoordinate(Candels.Panel.Rect.Height, Candels.Panel.Params.MaxPrice, Candels.Panel.Params.MinPrice, y1, Candels.Panel.Params.CountFloat),
                        Bottom = GMath.GetValueFromCoordinate(Candels.Panel.Rect.Height, Candels.Panel.Params.MaxPrice, Candels.Panel.Params.MinPrice, y2, Candels.Panel.Params.CountFloat)
                    });
                }
            }
        }
        /// <summary>
        /// Отрисовка активных частей
        /// </summary>
        /// <param name="dCan"></param>
        /// <returns></returns>
        protected bool PaintActual(CandleInfo dCan)
        {
            if (!Candels.PaintLastCandle(dCan))
            {
                return false;
            }
            //Проверка на перерисовку
            if (MainPanel.Params.Redraw())
            {
                PaintAll();
                return false;
            }
            //Обычные обьемы
            Volumes.GetFirstLevel().Volume = dCan.Candle.Volume;
            if (!Volumes.PaintLast(dCan)) return false;

            RightPrices.PaintCurrentValue(dCan.Candle.Close, MainPanel.Params.MaxPrice, MainPanel.Params.MinPrice);
            if (dCan.Candle.Close > MainPanel.Params.MaxPrice || dCan.Candle.Close < MainPanel.Params.MinPrice)
            {
                if (MainPanel.Params.AutoSize)
                {
                    if (OnReachLimitPrice.NotIsNull())
                    {
                        OnReachLimitPrice(dCan.Candle.Close, MainPanel.Params.MaxPrice, MainPanel.Params.MinPrice);
                    }
                }
            }

            Indicators.ForEach((objIndicator) =>
            {
                if (objIndicator is Indicator)
                {
                    var obj = (Indicator)objIndicator;
                    if (obj.FastRedraw)
                    {
                        obj.FastUpdate();
                    }
                }
            });
            return true;
        }

        protected void InitLoopsEachCandle()
        {
            Candels.OnEachCandle += (index, candle, count) =>
            {
                //Первая свеча(актуальная)
                if (index == 0)
                {
                    Volumes.CollectionLevels.Clear();
                    Volumes.Panel.Clear();
                    Volumes.ResetMinMax();
                    Volumes.Min = 0;
                }

                //Обработка свечей которые будут рисоваться
                if (index < Candels.CountPaintCandle)
                {
                    //Объемы
                    Volumes.CollectionLevels.Insert(index, new Chart() { Volume = candle.Volume });
                    if (Volumes.Max < candle.Volume) Volumes.Max = candle.Volume;

                    //Interest
                    long deltaInterest = 0;
                    if (candle.CountInterest > 0)
                    {
                        deltaInterest = (long)((candle.InterestBuy - candle.InterestSell) / candle.CountInterest);
                    }
                }

                Indicators.ForEach((ind) =>
                {
                    if (ind is Indicator)
                    {
                        var indicator = (Indicator)ind;
                        indicator.InitStartIndicator(() =>
                        {
                            indicator.CountVisibleCandle = CountVisibleCandles;
                            indicator.CountAllCandle = count;
                            indicator.CurrentTimeFrame = Candels.CurrentTimeFrame;
                        });
                        indicator.EachCandle(index, candle, count);
                        if (index == count - 1)
                        {
                            indicator.InitEndIndicator(() => { });
                        }
                    }
                });

                if (index == count - 1)
                {
                    var step = Volumes.Max * 10 / 100;
                    Volumes.Max += step;
                    Volumes.Min -= step;
                }
            };
            //После отрисованной свечи
            Candels.OnEachPaintedCandle += (dCandle) =>
            {
                if (dCandle.First)
                {
                    Times.BeforePaint();
                }
                //Рисуем временные отметки
                Times.Paint(dCandle);

                if (dCandle.Last)
                {

                }
            };
        }

        /// <summary>Отрисовка всего графика </summary>
        /// <param name="graphic">Полотно</param>
        protected void PaintAll()
        {
            GetCollectionCandles();
            if (!Candels.IssetCollection()) return;
            GetMinMax();
            //Отрисовка свечей в приоритете
            Candels.PaintCandles();

            RightPrices.Paint(RightPrices.Panel.Params.MaxPrice, RightPrices.Panel.Params.MinPrice);

            if (ActiveCandles.ActiveCandle1.NotIsNull() && ActiveCandles.ActiveCandle2.NotIsNull())
            {
                if (ActiveCandles.ActiveCandle1.dataCandle.Index > ActiveCandles.ActiveCandle2.dataCandle.Index)
                {
                    GHorVolumes.activeCandle1 = ActiveCandles.ActiveCandle1;
                    GHorVolumes.activeCandle2 = ActiveCandles.ActiveCandle2;
                }
                else
                {
                    GHorVolumes.activeCandle1 = ActiveCandles.ActiveCandle2;
                    GHorVolumes.activeCandle2 = ActiveCandles.ActiveCandle1;
                }
                if (TypeHorVolume == 3)
                {
                    GHorVolumes.activeCandle2 = null;
                }
            }

            CandleInfo LastCandle = null;
            Levels.Panel.Clear();
            var leftCandle = Candels.AllDataPaintedCandle.Last();
            var rightCandle = Candels.AllDataPaintedCandle.First();
            //Паинт
            foreach (var dCandle in Candels.AllDataPaintedCandle.ToArray())
            {
                dCandle.PrevCandleInfo = LastCandle;

                Volumes.PaintByCandle(dCandle);

                GHorVolumes.EachCandle(dCandle);

                Levels.PaintByCandle(dCandle, leftCandle, rightCandle, Candels.AllDataPaintedCandle.Count);

                Indicators.ForEach((ind) =>
                {
                    if (ind is Indicator)
                    {
                        ((Indicator)ind).EachFullCandle(dCandle);
                    }
                });
                LastCandle = dCandle;
            }

            LevelsOrders.Paint();
            Levels.Paint();

            ActualizeActiveCandle();
            if (ThreadPaintHotVol.NotIsNull())
            {
                ThreadPaintHotVol.Abort();
                ThreadPaintHotVol = null;
            }


            if (ThreadPaintHotVol.IsNull())
            {
                ThreadPaintHotVol = MThread.InitThread(() =>
                {
                    GHorVolumes.CollectionCandles = Candels.AllDataPaintedCandle;
                    if (TypeHorVolume == 1)
                    {
                        if (ActiveCandles.ActiveCandle1.NotIsNull())
                        {
                            GHorVolumes.PaintHorVolEachBlock(ActiveCandles.ActiveCandle1.dataCandle.Index + 1);
                        }
                    }
                    else if (TypeHorVolume == 2 || TypeHorVolume == 3)
                    {
                        GHorVolumes.PaintHorVolByPeriodCandleDelta();
                    } else if (TypeHorVolume == 4)
                    {
                        GHorVolumes.PaintCollectionHVol();
                    }
                    ThreadPaintHotVol = null;
                });
            }

            ToCanvas();
        }

        public void CrearHorVolumes()
        {
            GHorVolumes.CollectionCandleInBlocks.Clear();
            GHorVolumes.activeCandle1 = null;
            GHorVolumes.activeCandle2 = null;
            ClearActiveCandles();
            GHorVolumes.Panel.Clear();
            GHorVolumes.PanelVolume.Clear();
        }

        public void ClearActiveCandles()
        {
            ActiveCandles.ActiveCandle1 = null;
            ActiveCandles.ActiveCandle2 = null;
        }
        /// <summary>
        /// Сброс активных свечей
        /// </summary>
        public void ResetActiveCandles()
        {
            ActiveCandles.ActiveCandle1 = null;
            ActiveCandles.ActiveCandle2 = null;
            GHorVolumes.Panel.Clear();
        }

        /// <summary>
        /// Актуализируем активную свечу
        /// </summary>
        private void ActualizeActiveCandle()
        {
            if (ActiveCandles.ActiveCandle1.NotIsNull() && ActiveCandles.ActiveCandle1.dataCandle.NotIsNull())
            {
                var actCandle = Candels.AllDataPaintedCandle.FirstOrDefault(c =>
                    c.Candle.Time == ActiveCandles.ActiveCandle1.dataCandle.Candle.Time);
                if (actCandle.NotIsNull())
                {
                    //обновляем текущую активную свечку
                    ActiveCandles.ActiveCandle1.dataCandle = actCandle;
                }
                else
                {
                    ActiveCandles.ActiveCandle1 = null;
                }
            }
            if (ActiveCandles.ActiveCandle2.NotIsNull() && ActiveCandles.ActiveCandle2.dataCandle.NotIsNull())
            {
                var actCandle = Candels.AllDataPaintedCandle.FirstOrDefault(c =>
                    c.Candle.Time == ActiveCandles.ActiveCandle2.dataCandle.Candle.Time);
                if (actCandle.NotIsNull())
                {
                    //обновляем текущую активную свечку
                    ActiveCandles.ActiveCandle2.dataCandle = actCandle;
                }
                else
                {
                    ActiveCandles.ActiveCandle2 = null;
                }
            }
        }
        /// <summary>
        /// Получает список ордеров для отрисовки
        /// </summary>
        public void SetOrders(IEnumerable<Chart> orders)
        {
            LevelsOrders.Panel.Clear();
            LevelsOrders.CollectionOrders = orders.Where(o => o.Price < MainPanel.Params.MaxPrice && o.Price > MainPanel.Params.MinPrice);
        }
        /// <summary>
        /// Получает коолекцию свечек для отрисовки
        /// </summary>
        private void GetCollectionCandles()
        {
            if (DataSourceTimeFrame is CandleCollection)
            {
                var tFrame = (CandleCollection)DataSourceTimeFrame;
                if (tFrame.Count > 0)
                {
                    Candels.CurrentTimeFrame = tFrame.TimeFrame;
                    Candels.CollectionCandle = tFrame.CollectionArray.Skip(IndexFirstCandle);
                    Candels.CountPaintCandle = CountVisibleCandles;
                    Candels.Count = Candels.CollectionCandle.Count();
                }
            }
        }
        /// <summary>
        /// Обработчик новой свечи в тайм врейме
        /// </summary>
        /// <param name="taimFrame"></param>
        /// <param name="candle"></param>
        public void ProcessEventNewCandle(int timeFrame, CandleData candle)
        {
            //MAverage.Calculate(Candels.VerticalLines);
            Indicators.ForEach((ind) =>
            {
                ((Indicator)ind).NewCandleInTimeFrame(timeFrame, candle);
            });
        }

    }
}
