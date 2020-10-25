using GraphicTools.Base;
using GraphicTools.Extension;
using GraphicTools.Shapes;
using Market.Volumes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static GraphicTools.GCandles;

namespace GraphicTools
{
    public class GHorVol
    {

        private static object _lockPaint = new object();
        /// <summary>
        /// Минимальная ширина блока, при котором рисуется фоновая панель
        /// </summary>
        const int MIN_WIDTH_FOR_LAYBORDER = 20;

        public ViewPanel Panel = null;
        public ViewPanel PanelVolume = null;

        public GHorVol(BaseParams param)
        {
            Panel = new ViewPanel(param);
            PanelVolume = new ViewPanel(param);
            Panel.OnResize += (rect) =>
            {
                Panel.Clear();
            };
            PanelVolume.OnResize += (rect) =>
            {
                PanelVolume.Clear();
            };
        }
        /// <summary>
        /// Класс результата по объединенным объемам
        /// </summary>
        public class PrepareHorVol
        {
            public HVolume HorVolumes = null;
            public GCandles.CandleInfo LastCandle = null;
            public MarketObjects.Chart MaxElem = new MarketObjects.Chart();
            public MarketObjects.Chart MinElem = new MarketObjects.Chart();
            public MarketObjects.Chart MaxDeltaElem = new MarketObjects.Chart();
            public MarketObjects.Chart MinDeltaElem = new MarketObjects.Chart();
            public long MaxVol
            {
                get
                {
                    return this.MaxElem.NotIsNull() ? this.MaxElem.Volume : 0;
                }
            }
            public long MinVol
            {
                get
                {
                    return this.MinElem.NotIsNull() ? this.MinElem.Volume : 0;
                }
            }
            public long MaxDeltaVol = 0;
            public long MinDeltaVol = 0;
            public long SumBuyVol = 0;
            public long SumSellVol = 0;
            public long SumDeltaBuy = 0;
            public long SumDeltaSell = 0;
            public RectangleF RectBlock = new RectangleF();
            /// <summary>
            /// Индекс левой свечи
            /// </summary>
            public int index1 = -1;
            /// <summary>
            /// Индекс правой свечи
            /// </summary>
            public int index2 = -1;
        }
        /// <summary>
        /// Цвет линий максимального обьема.
        /// </summary>
        public Color ColorLayVol = Color.FromArgb(40, Color.Violet);
        /// <summary>
        /// Цвет границы выделяющего слоя блока с обьемами.
        /// </summary>
        public Color ColorLayBorder = Color.FromArgb(10, Color.Black);
        /// <summary>
        /// Цвет линий обьема
        /// </summary>
        public Color ColorVol = Color.FromArgb(210, Color.DarkBlue);
        /// <summary>
        /// Цвет линий максимального обьема
        /// </summary>
        public Color ColorMaxVol = Color.FromArgb(250, Color.Red);
        /// <summary>
        /// Цвет дельты положительной
        /// </summary>
        public Color ColorDeltaPositive = Color.FromArgb(180, Color.Green);
        /// <summary>
        /// Цвет дельты отрицательной
        /// </summary>
        public Color ColorDeltaNegative = Color.FromArgb(180, Color.Orange);
        /// <summary>
        /// Прямоугольник отрисовки для дельты
        /// </summary>
        public RectangleF RectForDelta = new RectangleF();
        /// <summary>
        /// Данные по объединенным обьемам
        /// </summary>
        public PrepareHorVol PrepDataHorVol = null;
        /// <summary>
        /// Коллекция блоков с гор. обьемами
        /// </summary>
        public List<PrepareHorVol> CollectionBlocks = new List<PrepareHorVol>();
        public List<int> CollectionCandleInBlocks = new List<int>();
        /// <summary>
        /// Левая активная свеча
        /// </summary>
        public SelectCandle activeCandle1 = null;
        /// <summary>
        /// Правая активная свеча
        /// </summary>
        public SelectCandle activeCandle2 = null;

        /// <summary>
        /// Коллекция свечей
        /// </summary>
        public IEnumerable<GCandles.CandleInfo> CollectionCandles = null;
        /// <summary>
        /// Кол-во свечей объединеных в объем
        /// </summary>
        private int Count
        {
            get
            {
                return CollectionCandles.NotIsNull() ? CollectionCandles.Count() : 0;
            }
        }
        /// <summary>
        /// При блочной отрисовке обьемов, рисовать только первый блок а правой панели.
        /// </summary>
        private bool PaintOnlyFirstBlock = true;
        /// <summary>
        /// Рисование обьема по каждой свечке
        /// </summary>
        /// <param name="countInBlock"></param>
        public void PaintHorVolEachBlock(int countInBlock = 1, bool isCollection = false)
        {
            lock (_lockPaint)
            {
                Panel.Clear();
                PanelVolume.Clear();
                if (countInBlock <= 0)
                {
                    return;
                }
                List<PrepareHorVol> preDataHVol = new List<PrepareHorVol>();
                var CurrentHVol = new PrepareHorVol();
                CurrentHVol.HorVolumes = new HVolume();
                preDataHVol.Add(CurrentHVol);

                var list = this.CollectionCandles.ToArray();
                var maxAll = new MarketObjects.Chart();

                if (list.NotIsNull() && list.Count() > 0)
                {
                    int indexer = 0;
                    int steper = countInBlock;
                    if (isCollection)
                    {
                        if (CollectionCandleInBlocks.Count > 0)
                        {
                            steper = CollectionCandleInBlocks.ElementAt(indexer) + 1;
                        }
                        else
                        {
                            return;
                        }
                    }
                    foreach (var can in list)
                    {
                        if (can.NotIsNull())
                        {
                            foreach (var curVol in can.Candle.GetHorVolumes().HVolCollection.ToArray())
                            {
                                CurrentHVol.HorVolumes.AddVolume(curVol.Price, curVol.VolBuy, true);
                                CurrentHVol.HorVolumes.AddVolume(curVol.Price, curVol.VolSell, false);
                                CurrentHVol.SumBuyVol += curVol.VolBuy;
                                CurrentHVol.SumSellVol += curVol.VolSell;
                            }

                            CurrentHVol.RectBlock.X = CurrentHVol.RectBlock.X > can.Body.X || CurrentHVol.RectBlock.X == 0
                                ? can.Body.X : CurrentHVol.RectBlock.X;
                            CurrentHVol.RectBlock.Y = CurrentHVol.RectBlock.Y > can.TailCoord.High.Y || CurrentHVol.RectBlock.Y == 0
                                ? can.TailCoord.High.Y : CurrentHVol.RectBlock.Y;
                            CurrentHVol.RectBlock.Width = CurrentHVol.RectBlock.Width < (can.Body.X + can.Body.Width) || CurrentHVol.RectBlock.Width == 0
                                ? (can.Body.X + can.Body.Width) : CurrentHVol.RectBlock.Width;
                            CurrentHVol.RectBlock.Height = CurrentHVol.RectBlock.Height < can.TailCoord.Low.Y || CurrentHVol.RectBlock.Height == 0
                                ? can.TailCoord.Low.Y : CurrentHVol.RectBlock.Height;
                        }
                        steper--;
                        if (steper <= 0)
                        {
                            CurrentHVol.MaxElem = CurrentHVol.HorVolumes.MaxVolume;
                            CurrentHVol.MinElem = CurrentHVol.HorVolumes.MinVolume;

                            if (maxAll.Volume < CurrentHVol.MaxVol)
                            {
                                maxAll = CurrentHVol.MaxElem;
                            }
                            CurrentHVol.LastCandle = can;

                            indexer++;
                            steper = countInBlock;
                            if (isCollection)
                            {
                                if (indexer >= CollectionCandleInBlocks.Count)
                                {
                                    break;
                                }
                                steper = CollectionCandleInBlocks.ElementAt(indexer) - CollectionCandleInBlocks.ElementAt(indexer - 1);
                            }
                            CurrentHVol = new PrepareHorVol();
                            CurrentHVol.HorVolumes = new HVolume();
                            preDataHVol.Add(CurrentHVol);
                        }
                    }
                }
                PaintOnlyFirstBlock = true;
                foreach (var elVol in preDataHVol)
                {
                    RectangleF rectPaint = new RectangleF();
                    rectPaint.X = elVol.RectBlock.X;
                    rectPaint.Y = elVol.RectBlock.Y;
                    rectPaint.Width = elVol.RectBlock.Width - elVol.RectBlock.X;
                    rectPaint.Height = elVol.RectBlock.Height - elVol.RectBlock.Y;
                    elVol.RectBlock = rectPaint;

                    this.PaintOneBlock(elVol, maxAll, countInBlock);
                    PaintOnlyFirstBlock = false;
                }
            }
        }
        /// <summary>
        /// Рисует блок состоящий из нескольких свечей
        /// </summary>
        /// <param name="prepHorV"></param>
        /// <param name="max"></param>
        /// <param name="countInBlock"></param>
        private void PaintOneBlock(PrepareHorVol prepHorV, MarketObjects.Chart max, int countInBlock)
        {
            var canvas = Panel.GetGraphics;
            var canvasOnlyVol = PanelVolume.GetGraphics;
            if (prepHorV.RectBlock.Width >= MIN_WIDTH_FOR_LAYBORDER)
            {
                //рисует прямоугольник выделяющий период
                var rectVol = new RectDraw();
                rectVol.ColorFill = ColorLayVol;
                rectVol.ColorBorder = ColorLayBorder;
                rectVol.Paint(canvas, prepHorV.RectBlock.X, prepHorV.RectBlock.Y,
                    prepHorV.RectBlock.Width, prepHorV.RectBlock.Height);

                var textMax = new TextDraw();
                textMax.Color = Color.Black;
                var text = "max:" + prepHorV.MaxVol.ToString();
                var size = textMax.GetSizeText(canvas, text);
                textMax.Paint(canvas, text, prepHorV.RectBlock.X, prepHorV.RectBlock.Y - size.Height);
            }

            prepHorV.HorVolumes.ToArray().ForEach<MarketObjects.ChartVol>((hv) =>
            {
                this.PaintLineHVol(canvas, prepHorV.RectBlock, max, new MarketObjects.Chart(), hv);
                if (PaintOnlyFirstBlock)
                {
                    this.PaintLineHVol(canvasOnlyVol, PanelVolume.Rect.Rectangle, max, new MarketObjects.Chart(), hv, Color.Blue);
                }
            });
        }

        /// <summary>
        /// Получает информацию по объемам текущей свечки
        /// </summary>
        /// <param name="crossLine"></param>
        /// <param name="activeCandle"></param>
        public void GetValueVolume(Point crossLine, GCandles.CandleInfo activeCandle)
        {
            decimal priceY = GMath.GetValueFromCoordinate(Panel.Rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, crossLine.Y, Panel.Params.CountFloat);
            var hv = activeCandle.Candle.GetHorVolumes().HVolCollection.ToArray().FirstOrDefault(v => v.Price == priceY);
            if (hv.NotIsNull())
            {
                if (priceY == hv.Price)
                {
                    if (hv.Price == priceY) activeCandle.Description = (hv.VolBuy + hv.VolSell).ToString();
                    activeCandle.CurHorVolume = new MarketObjects.ChartVol() { Price = priceY, VolBuy = hv.VolBuy, VolSell = hv.VolSell };
                }
            }
        }
        /// <summary>
        /// Обработчик каждой свечки
        /// </summary>
        /// <param name="dataCandle"></param>
        public void EachCandle(CandleInfo dataCandle)
        {
            if (dataCandle.Index == 0)
            {
                PrepDataHorVol = new PrepareHorVol();
                PrepDataHorVol.HorVolumes = new HVolume();

                if (activeCandle2.IsNull())
                {
                    activeCandle2 = new SelectCandle();
                    activeCandle2.dataCandle = dataCandle;
                }
            }
            if (activeCandle1.NotIsNull() && activeCandle2.NotIsNull())
            {
                if (dataCandle.Index <= activeCandle1.dataCandle.Index && dataCandle.Index >= activeCandle2.dataCandle.Index)
                {
                    foreach (var curVol in dataCandle.Candle.GetHorVolumes().HVolCollection.ToArray())
                    {
                        if (curVol.Price < Panel.Params.MaxPrice && curVol.Price > Panel.Params.MinPrice)
                        {
                            PrepDataHorVol.HorVolumes.AddVolume(curVol.Price, curVol.VolBuy, curVol.VolSell);
                        }
                    }
                }
            }
        }

        public void PaintCollectionHVol()
        {

            if (activeCandle1.IsNull())
            {
                return;
            }
            var maxIndex = 0;
            if (CollectionCandleInBlocks.Count > 0)
            {
                maxIndex = CollectionCandleInBlocks.Max();
            }
            if (maxIndex < activeCandle1.dataCandle.Index)
            {
                CollectionCandleInBlocks.Add(activeCandle1.dataCandle.Index);
            }
            //Отрисовка
            if (CollectionCandleInBlocks.Count > 0)
            {
                PaintHorVolEachBlock(1, true);
            }
        }

        private bool CreateBlockHorVolume(bool addInCollection = false)
        {
            PrepDataHorVol.index1 = activeCandle1.dataCandle.Index;
            PrepDataHorVol.index2 = activeCandle2.dataCandle.Index;

            PrepDataHorVol.MaxElem = new MarketObjects.Chart();
            if (PrepDataHorVol.NotIsNull())
            {
                PrepDataHorVol.MaxElem = PrepDataHorVol.HorVolumes.MaxVolume;
                PrepDataHorVol.MinElem = PrepDataHorVol.HorVolumes.MinVolume;
                PrepDataHorVol.MaxDeltaElem = PrepDataHorVol.HorVolumes.MaxDeltaVolume;
                PrepDataHorVol.MinDeltaElem = PrepDataHorVol.HorVolumes.MinDeltaVolume;
            }
            PrepDataHorVol.MaxElem = PrepDataHorVol.MaxElem.NotIsNull() ? PrepDataHorVol.MaxElem : new MarketObjects.Chart();
            PrepDataHorVol.MinElem = PrepDataHorVol.MinElem.NotIsNull() ? PrepDataHorVol.MinElem : new MarketObjects.Chart();
            PrepDataHorVol.MaxDeltaElem = PrepDataHorVol.MaxDeltaElem.NotIsNull() ? PrepDataHorVol.MaxDeltaElem : new MarketObjects.Chart();
            PrepDataHorVol.MinDeltaElem = PrepDataHorVol.MinDeltaElem.NotIsNull() ? PrepDataHorVol.MinDeltaElem : new MarketObjects.Chart();

            PrepDataHorVol.LastCandle = activeCandle1.dataCandle;
            return true;
        }
        /// <summary>
        /// Рисование обьемов за период
        /// </summary>
        /// <param name="alwaysUpdate"></param>
        public void PaintHorVolByPeriodCandle(bool alwaysUpdate = false)
        {
            lock (_lockPaint)
            {
                if (PrepDataHorVol.IsNull()) return;
                if (activeCandle1.IsNull() || activeCandle2.IsNull()) return;

                Panel.Clear();
                PanelVolume.Clear();
                if (!CreateBlockHorVolume())
                {
                    return;
                }

                var canvas = Panel.GetGraphics;
                var canvasOnlyVol = PanelVolume.GetGraphics;

                var countCandleInVol = PrepDataHorVol.index1 - PrepDataHorVol.index2 + 1;
                var xLineBorder = activeCandle2.dataCandle.Body.X + activeCandle2.dataCandle.Body.Width;

                RectangleF rectPaint = new RectangleF();
                rectPaint.X = activeCandle1.dataCandle.Body.X;
                rectPaint.Width = xLineBorder - activeCandle1.dataCandle.Body.X;
                rectPaint.Width = rectPaint.Width < 40 ? 40 : rectPaint.Width;

                rectPaint.Y = activeCandle1.dataCandle.TailCoord.Low.Y;
                rectPaint.Height = 0;

                rectPaint.Y = rectPaint.Y - 13;
                rectPaint.Height = rectPaint.Height + 13;
                //рисует прямоугольник выделяющий период
                var rectVol = new RectDraw();
                rectVol.ColorFill = ColorLayVol;
                rectVol.ColorBorder = ColorLayBorder;
                rectVol.Paint(canvas, rectPaint.X, Panel.Rect.Y, rectPaint.Width, Panel.Rect.Height);

                //Получяаем максимальный гор. объем в свече или наборе свечей
                PrepDataHorVol.HorVolumes.ToArray().ForEach<MarketObjects.ChartVol>((hv) =>
                {
                    this.PaintLineHVol(canvas, rectPaint, PrepDataHorVol.MaxElem, new MarketObjects.Chart(), hv);
                    this.PaintLineHVol(canvasOnlyVol, PanelVolume.Rect.Rectangle, PrepDataHorVol.MaxElem, new MarketObjects.Chart(), hv, Color.Blue);
                    //this.PaintLineHVol(canvasDelta, PanelDelta.Rect.Rectangle, PrepDataHorVol.MaxDeltaElem, PrepDataHorVol.MinDeltaElem, hv, true);
                    PrepDataHorVol.SumBuyVol += hv.VolBuy;
                    PrepDataHorVol.SumSellVol += hv.VolSell;
                });

                var textMax = new TextDraw();
                textMax.Color = Color.Black;
                textMax.Paint(canvas,
                    "V:" + (PrepDataHorVol.SumBuyVol + PrepDataHorVol.SumSellVol).ToString() + "\r\n"
                    + "D:" + (PrepDataHorVol.SumBuyVol - PrepDataHorVol.SumSellVol).ToString() + "\r\n"
                    + "max:" + PrepDataHorVol.MaxVol.ToString() + "\r\n"
                    + "p:" + PrepDataHorVol.MaxElem.Price.ToString(),
                    rectPaint.X, Panel.Rect.Y);

                textMax.Paint(canvasOnlyVol,
                    "max: " + PrepDataHorVol.MaxVol.ToString() + " p:" + PrepDataHorVol.MaxElem.Price.ToString(),
                    this.RectForDelta.X, this.RectForDelta.Y);
            }
        }

        public void PaintHorVolByPeriodCandleDelta(bool alwaysUpdate = false, bool delta = false)
        {
            lock (_lockPaint)
            {
                if (PrepDataHorVol.IsNull()) return;
                if (activeCandle1.IsNull() || activeCandle2.IsNull()) return;

                Panel.Clear();
                PanelVolume.Clear();
                if (!CreateBlockHorVolume())
                {
                    return;
                }

                var canvas = Panel.GetGraphics;
                var canvasOnlyVol = PanelVolume.GetGraphics;

                var countCandleInVol = PrepDataHorVol.index1 - PrepDataHorVol.index2 + 1;
                var xLineBorder = activeCandle2.dataCandle.Body.X + activeCandle2.dataCandle.Body.Width;

                RectangleF rectPaint = new RectangleF();
                rectPaint.X = activeCandle1.dataCandle.Body.X;
                rectPaint.Width = xLineBorder - activeCandle1.dataCandle.Body.X;
                rectPaint.Width = rectPaint.Width < 40 ? 40 : rectPaint.Width;

                rectPaint.Y = activeCandle1.dataCandle.TailCoord.Low.Y;
                rectPaint.Height = 0;

                rectPaint.Y = rectPaint.Y - 13;
                rectPaint.Height = rectPaint.Height + 13;
                //рисует прямоугольник выделяющий период
                var rectVol = new RectDraw();
                rectVol.ColorFill = ColorLayVol;
                rectVol.ColorBorder = ColorLayBorder;
                rectVol.Paint(canvas, rectPaint.X, Panel.Rect.Y, rectPaint.Width, Panel.Rect.Height);

                //Получяаем максимальный гор. объем в свече или наборе свечей
                PrepDataHorVol.HorVolumes.ToArray().ForEach<MarketObjects.ChartVol>((hv) =>
                {
                    PaintLineHVol(canvas, rectPaint, PrepDataHorVol.MaxElem, new MarketObjects.Chart(), hv);
                    if (delta)
                    {
                        PaintLineHVol(canvasOnlyVol, PanelVolume.Rect.Rectangle, PrepDataHorVol.MaxDeltaElem, PrepDataHorVol.MinDeltaElem, hv, null, true);
                        var d = hv.VolBuy - hv.VolSell;
                        if (d > 0)
                        {
                            PrepDataHorVol.SumDeltaBuy += d;
                        }
                        else
                        {
                            PrepDataHorVol.SumDeltaSell += d * -1;
                        }
                    } else
                    {
                        PaintLineHVol(canvasOnlyVol, PanelVolume.Rect.Rectangle, PrepDataHorVol.MaxElem, new MarketObjects.Chart(), hv);
                    }
                    PrepDataHorVol.SumBuyVol += hv.VolBuy;
                    PrepDataHorVol.SumSellVol += hv.VolSell; 
                });

                var textMax = new TextDraw();
                textMax.Color = Color.Black;
                textMax.Paint(canvas,
                    "V:" + (PrepDataHorVol.SumBuyVol + PrepDataHorVol.SumSellVol).ToString() + "\r\n"
                    + "D:" + (PrepDataHorVol.SumBuyVol - PrepDataHorVol.SumSellVol).ToString() + "\r\n"
                    + "max:" + PrepDataHorVol.MaxVol.ToString() + "\r\n"
                    + "p:" + PrepDataHorVol.MaxElem.Price.ToString(),
                    rectPaint.X, Panel.Rect.Y);
                if (delta)
                {
                    textMax.Paint(canvasOnlyVol,
                    "max: " + PrepDataHorVol.MinDeltaElem.Volume.ToString() + "/" + PrepDataHorVol.MaxDeltaElem.Volume.ToString() + "\r\n" +
                    "sum: " + PrepDataHorVol.SumDeltaSell.ToString() + "/" + PrepDataHorVol.SumDeltaBuy.ToString() + "\r\n"
                    ,
                    this.RectForDelta.X, this.RectForDelta.Y);
                } else
                {
                    textMax.Paint(canvasOnlyVol,
                    "max: " + PrepDataHorVol.MaxElem.Volume.ToString() + "\r\n" +
                    "sum: " + (PrepDataHorVol.SumSellVol + PrepDataHorVol.SumBuyVol).ToString() + "\r\n" +
                    "sum S/B: " + PrepDataHorVol.SumSellVol.ToString() + "/" + PrepDataHorVol.SumBuyVol.ToString() + "\r\n"
                    ,
                    this.RectForDelta.X, this.RectForDelta.Y);
                }
            }
        }

        /// <summary>
        /// Рисует линию горизонтального объема
        /// </summary>
        private int PaintLineHVol(Graphics canvas, RectangleF rect, MarketObjects.Chart maxElem, MarketObjects.Chart minElem, MarketObjects.ChartVol hv, Color? colorVol = null, bool isDelta = false)
        {
            var widthLayoutVolume = rect.Width;
            var value = hv.VolBuy + hv.VolSell;
            if (isDelta)
            {
                value = hv.VolBuy - hv.VolSell;
            }
            float y = GMath.GetCoordinate(Panel.Rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, hv.Price);
            var height = GMath.GetCoordinate(Panel.Rect.Height, Panel.Params.MaxPrice, Panel.Params.MinPrice, hv.Price - Panel.Params.MinStepPrice) - y;

            if (y == 0 || y == Panel.Rect.Height)
            {
                return -1;
            }
            int x1 = 0, x2 = 0;
            if (value < 0)
            {
                x1 = GMath.GetCoordinate(widthLayoutVolume, maxElem.Volume, minElem.Volume, value);
                x2 = GMath.GetCoordinate(widthLayoutVolume, maxElem.Volume, minElem.Volume, 0);
            }
            else
            {
                x1 = GMath.GetCoordinate(widthLayoutVolume, maxElem.Volume, minElem.Volume, 0);
                x2 = GMath.GetCoordinate(widthLayoutVolume, maxElem.Volume, minElem.Volume, value);
            }
            var p1 = new PointF(rect.X + widthLayoutVolume - x1, y);
            var p2 = new PointF(rect.X + widthLayoutVolume - x2, y);


            RectDraw rectVol = new RectDraw();

            var color = ColorVol;
            if (isDelta)
            {
                if (value < 0)
                {
                    color = this.ColorDeltaNegative;
                }
                else
                {
                    color = this.ColorDeltaPositive;
                }
            }
            if (maxElem.Price == hv.Price)
            {
                color = ColorMaxVol;
            }
            if (minElem.Price == hv.Price)
            {
                color = ColorMaxVol;
            }
            if (colorVol.NotIsNull())
            {
                color = (Color)colorVol;
            }
            rectVol.ColorBorder = color;
            rectVol.ColorFill = color;
            rectVol.Paint(canvas, p1.X, y - height / 2, p2.X - p1.X, height - 1 <= 0 ? 1 : height - 1);
            return 0;
        }
    }
}
