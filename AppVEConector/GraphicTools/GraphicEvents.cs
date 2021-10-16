
using GraphicTools.Extension;
using System;
using System.Drawing;
using System.Linq;

namespace GraphicTools
{
    public partial class Graphic
    {
        public delegate void eventCandleWithCoordinate(Point point, GCandles.CandleInfo candle);
        public delegate void eventCandle(GCandles.CandleInfo candle);
        /// <summary> Событие передвижения курсора мыши по свечке. </summary>
        public event eventCandleWithCoordinate OnCandleMove;
        /// <summary> Событие левого клика по свечке </summary>
        public event eventCandleWithCoordinate OnCandleLeftClick;
        /// <summary> Событие правого клика по свечке </summary>
        public event eventCandleWithCoordinate OnCandleRightClick;

        public event eventCandleWithCoordinate OnCandleLeftMouseDown;
        public event eventCandleWithCoordinate OnCandleLeftMouseUp;

        /// <summary>
        /// Инициализация событий
        /// </summary>
        public void InitEvents()
        {
            this.OnResize += (rect) =>
            {
                this.Paint();
            };
            System.Action<decimal, decimal, decimal> EventRefrech = (cur, max, min) =>
            {
                this.Paint();
            };
            this.OnReachLimitPrice += EventRefrech;

            this.OnCandleLeftMouseDown += (coordCl, cand) =>
            {
                ActiveCandles.startSel();
                ActiveCandles.ActiveCandle1 = new SelectCandle() { coordClick = coordCl, dataCandle = cand };
                ActiveCandles.ActiveCandle2 = null;
            };
            this.OnCandleLeftMouseUp += (coordCl, cand) =>
            {
                ActiveCandles.ActiveCandle2 = new SelectCandle() { coordClick = coordCl, dataCandle = cand };
                ActiveCandles.endSel();

                CheckAndCreateNewLevel(coordCl, cand);
            };

            this.OnCandleRightClick += (coord, cand) =>
            {
                ActiveCandles.ActiveCandle1 = new SelectCandle() { dataCandle = null };
                this.Paint();
            };

            this.OnCandleMove += (cross, candle) =>
            {
                this.Candels.MoveVerticalActiveCandle(candle);
            };
        }
        /// <summary>
        /// Инициализация внутренних событий
        /// </summary>
        protected void InitNativeEvents()
        {
            System.Action<decimal, decimal, decimal> EventRefrech = (cur, max, min) =>
            {
                this.Paint();
            };
            Candels.OnReachMinMax += EventRefrech;
            Volumes.OnReachLimitValues += EventRefrech;
            //DeltaVol.OnReachLimitValues += EventRefrech;

            Candels.OnPaintedCandle += (dCan) =>
            {

            };

            DragAndDrop.OnDrag += (first, second) =>
            {
                RightPrices.CheckDragAndDrop(DragAndDrop);
            };
            DragAndDrop.OnDrop += (first, second) =>
            {
                RightPrices.CheckDragAndDrop(DragAndDrop);
            };

            //Отрисовка свечей завершена
            /*PanelCandels.OnPaintedCandles += () =>
            {

            };*/
            //Отрисовка свечей завершена
            /*PanelCandels.OnPaintedCandles += () =>
            {

            };*/

            //Перед отрисовкой свечей
            /*PanelCandels.OnBeforePaintCandle += () =>
            {
                this.ActiveCandle = null;
            };

            //Отрисовка одной свечи
            /*PanelCandels.OnPaintCandle += (candle) =>
            {

            };*/

        }

        /// <summary>
        /// Получаем клик по холсту (левый)
        /// </summary>
        /// <param name="coordClick"></param>
        public void GetLeftClick(Point coordClick)
        {
            this.GetEventByCoord(coordClick, (dCan) =>
            {
                if (this.OnCandleLeftClick.NotIsNull())
                    this.OnCandleLeftClick(coordClick, dCan);
            });
        }
        /// <summary>
        /// Получаем клик по холсту (правый)
        /// </summary>
        /// <param name="coordClick"></param>
        public void GetRightClick(Point coordClick)
        {
            this.GetEventByCoord(coordClick, (dCan) =>
            {
                if (this.OnCandleRightClick.NotIsNull())
                    this.OnCandleRightClick(coordClick, dCan);
            });
        }
        /// <summary>
        /// Получаем key-down (левый)
        /// </summary>
        /// <param name="coordClick"></param>
        public void GetLeftDown(Point coordClick)
        {
            this.GetEventVertByCoord(coordClick, (dCan) =>
            {
                if (this.OnCandleLeftMouseDown.NotIsNull())
                    this.OnCandleLeftMouseDown(coordClick, dCan);
            });
        }
        /// <summary>
        /// Получаем key-up (левый)
        /// </summary>
        /// <param name="coordClick"></param>
        public void GetLeftUp(Point coordClick)
        {
            this.GetEventVertByCoord(coordClick, (dCan) =>
            {
                if (this.OnCandleLeftMouseUp.NotIsNull())
                    this.OnCandleLeftMouseUp(coordClick, dCan);
            });
        }

        /// <summary>
        /// Определение события по попаданию координат на свечу
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="onEvent"></param>
        /// <param name="refresh"></param>
        private void GetEventByCoord(Point coord, Action<GCandles.CandleInfo> onEvent, bool refresh = true)
        {
            foreach (var dCan in this.Candels.AllDataPaintedCandle.ToArray())
            {
                if (dCan.Body.X <= coord.X && dCan.Body.X + dCan.Body.Width >= coord.X
                && dCan.TailCoord.High.Y <= coord.Y && dCan.TailCoord.Low.Y >= coord.Y)
                {
                    if (dCan.NotIsNull())
                    {
                        if (onEvent.NotIsNull())
                        {
                            onEvent(dCan);
                        }                            
                    }
                }
            }
            if (refresh)
            {
                Paint();
            }
        }

        /// <summary>
        /// Определение события по вертикале свечи
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="onEvent"></param>
        /// <param name="refresh"></param>
        private void GetEventVertByCoord(Point coord, Action<GCandles.CandleInfo> onEvent, bool refresh = true)
        {
            if (this.Candels.AllDataPaintedCandle.Count == 0) return;
            //Проверяем первую
            var first = this.Candels.AllDataPaintedCandle[0];
            if (first.Body.X + first.Body.Width <= coord.X)
            {
                if (first.NotIsNull())
                {
                    if (onEvent.NotIsNull())
                        onEvent(first);
                    if (refresh) this.Paint();
                    return;
                }
            }
            //Проверяем последнюю
            var last = this.Candels.AllDataPaintedCandle.Last();
            if (last.Body.X + last.Body.Width >= coord.X)
            {
                if (last.NotIsNull())
                {
                    if (onEvent.NotIsNull())
                        onEvent(last);
                    if (refresh) this.Paint();
                    return;
                }
            }
            //По всем
            foreach (var dCan in this.Candels.AllDataPaintedCandle.ToArray())
            {
                if (dCan.Body.X <= coord.X && dCan.Body.X + dCan.Body.Width >= coord.X)
                {
                    if (dCan.NotIsNull())
                    {
                        if (onEvent.NotIsNull())
                        {
                            onEvent(dCan);
                        }
                        break;
                    }
                }
                else
                {
                    if (dCan.Body.X <= coord.X + 1 && dCan.Body.X + dCan.Body.Width >= coord.X - 1)
                    {
                        if (dCan.NotIsNull())
                        {
                            if (onEvent.NotIsNull())
                            {
                                onEvent(dCan);
                            }
                            break;
                        }
                    }
                }
            }
            if (refresh)
            {
                Paint();
            }
        }
    }
}
