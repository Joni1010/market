using GraphicTools.Base;
using GraphicTools.Shapes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicTools.Extension
{
    /// <summary> Объект отрисовки фрейма значений, справа на графике </summary>
    public partial class GRightValue
    {
        /// <summary> Цвет линий </summary>
        public Color ColorLines = Color.FromArgb(230, 230, 230);
        /// <summary> Цвет текстового значения </summary>
        public Color ColorText = Color.Black;
        /// <summary> Ширина области для отображения цен </summary>
        public int WidthBorder = 55;
        /// <summary> Текущее значение для контрольной линии </summary>
        public decimal CurrentValue = -1;
        /// <summary> Цвет текущего значения для контрольной линии </summary>
        public Color ColorCurrentLine = Color.Red;

        /// <summary> Кол-во линий отметок значений </summary>
        public int CountLineValue = 10;

        public ViewPanel Panel = null;
        public ViewPanel PanelCurValue = null;

        /// <summary>
        /// Кол-во знаков после запятой
        /// </summary>
        public int Decimal = 0;
        /// <summary>  Конструктор </summary>
        /// <param name="countFloat"> Точность Значение (знаков после запятой)</param>
        public GRightValue(BaseParams param)
        {
            Panel = new ViewPanel(param);
            PanelCurValue = new ViewPanel(param);
            Panel.OnChangeRect += (rect) =>
            {
                PanelCurValue.SetRect(rect);
            };
        }

        /// <summary>
        /// Устанавливает ширину области значений
        /// </summary>
        /// <param name="width"></param>
        public void SetWidthValues(int width)
        {
            WidthBorder = width;
        }

        /// <summary> Отрисовка </summary>
        /// <param name="canvas">Полотно</param>
        /// <param name="MaxValue">MAX значение</param>
        /// <param name="MinValue">MIN значение</param>
        public void Paint(decimal MaxValue, decimal MinValue)
        {
            Panel.Clear();
            var canvas = this.Panel.GetGraphics;

            decimal Interval = MaxValue - MinValue;
            decimal stepValue = 0;
            if (Panel.Params.MinStepPrice >= 1)
                stepValue = Convert.ToInt32(Interval / this.CountLineValue);
            else stepValue = decimal.Round(Interval / this.CountLineValue, this.Panel.Params.CountFloat);
            var strStep = stepValue.ToString();
            strStep = strStep.Substring(0, strStep.Length - 1) + '0';
            stepValue = strStep.ToDecimal();

            //Рисуем линию границы
            Point pBorder1 = new Point(this.Panel.Rect.X + this.Panel.Rect.Width - WidthBorder, this.Panel.Rect.Y);
            Point pBorder2 = new Point(this.Panel.Rect.X + this.Panel.Rect.Width - WidthBorder, this.Panel.Rect.Y + this.Panel.RectScreen.Height);

            var line = new Line();
            line.Paint(canvas, pBorder1, pBorder2, Color.Black);

            var horLine = new HorLine();
            horLine.ColorLine = this.ColorLines;
            horLine.ColorText = this.ColorText;
            horLine.FillText = true;
            horLine.ColorFillText = Color.White;
            decimal Value = MinValue;
            //Значение сетки
            while (Value < MaxValue && stepValue > 0)
            {
                horLine.Paint(canvas, Panel.Rect.Rectangle, Value, MaxValue, MinValue);
                Value += stepValue;
            }
            horLine.Paint(canvas, Panel.Rect.Rectangle, 0, MaxValue, MinValue);
        }

        /// <summary>  Рисует контрольную линию текущего значения </summary>
        public void PaintCurrentValue(decimal curValue, decimal max, decimal min)
        {
            CurrentValue = curValue;
            PanelCurValue.Clear();
            var canvas = PanelCurValue.GetGraphics;

            var horLine = new HorLine();
            horLine.ColorLine = horLine.ColorText = this.ColorCurrentLine;
            horLine.FillText = true;
            horLine.Paint(canvas, Panel.Rect.Rectangle, this.CurrentValue, max, min);
        }
        /// <summary>
        /// Проверяет валидность драг&дропа в данной области
        /// </summary>
        /// <param name="eventDrag"></param>
        /// <returns></returns>
        private bool checkDragAndDropHit(DragAndDrop eventDrag)
        {
            if (eventDrag.PointFirst.NotIsNull() && eventDrag.PointSecond.NotIsNull())
            {
                var first = eventDrag.PointFirst.Value;
                var second = eventDrag.PointSecond.Value;
                var x1 = this.Panel.Rect.X + this.Panel.Rect.Width - WidthBorder;
                var x2 = this.Panel.Rect.X + this.Panel.Rect.Width;
                var y1 = this.Panel.Rect.Y;
                var y2 = this.Panel.Rect.Y + this.Panel.RectScreen.Height;
                if ((first.X > x1 && first.Y < x2 && first.Y > y1 && first.Y < y2) &&
                    (second.X > x1 && second.Y < x2 && second.Y > y1 && second.Y < y2))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Проверна на захват и перемещение мышью
        /// </summary>
        /// <param name="eventDrag"></param>
        /// <returns></returns>
        public bool CheckDragAndDrop(DragAndDrop eventDrag)
        {
            if (eventDrag.NotIsNull())
            {
                //Проверяем по координатам валидность координат
                if (eventDrag.statusDrag() && checkDragAndDropHit(eventDrag))
                {
                    var half = this.Panel.RectScreen.Height / 2;
                    var distance = eventDrag.PointFirst.Value.Y - eventDrag.PointSecond.Value.Y;
                    distance = distance < 0 ? distance * -1 : distance;
                    var val = (distance / 2) * Panel.Params.MinStepPrice;
                    //Динамическое масштабирование
                    if (!Panel.Params.AutoSize)
                    {
                        //Перемещение вверх-вниз
                        if (Panel.Params.TypeScaling == BaseParams.TYPE_SCALING.MOVE)
                        {
                            //top inc
                            if (eventDrag.PointFirst.Value.Y < eventDrag.PointSecond.Value.Y)
                            {
                                setDinamicMinMaxValue(val, true);
                                setDinamicMinMaxValue(val, false);
                                return true;
                            }
                            //bottom dec
                            if (eventDrag.PointFirst.Value.Y > eventDrag.PointSecond.Value.Y)
                            {
                                setDinamicMinMaxValue(val * -1, true);
                                setDinamicMinMaxValue(val * -1, false);
                                return true;
                            }
                            return false;
                        }
                        //Растягивание графика вверх-вниз
                        if (Panel.Params.TypeScaling == BaseParams.TYPE_SCALING.STRECH)
                        {
                            if (this.Panel.Rect.Y + half > eventDrag.PointFirst.Value.Y)
                            {
                                //top inc
                                if (eventDrag.PointFirst.Value.Y < eventDrag.PointSecond.Value.Y)
                                {
                                    return setDinamicMinMaxValue(val, true);
                                }
                                //top dec
                                if (eventDrag.PointFirst.Value.Y > eventDrag.PointSecond.Value.Y)
                                {
                                    return setDinamicMinMaxValue(val * -1, true);
                                }
                            }
                            else
                            {
                                //bottom inc
                                if (eventDrag.PointFirst.Value.Y < eventDrag.PointSecond.Value.Y)
                                {
                                    return setDinamicMinMaxValue(val, false);
                                }
                                //bottom dec
                                if (eventDrag.PointFirst.Value.Y > eventDrag.PointSecond.Value.Y)
                                {
                                    return setDinamicMinMaxValue(val * -1, false);
                                }
                            }
                            return false;
                        }
                    }
                }
            }
            //После завершения перемещения устанавливаем текущие значения
            Panel.Params.oldMaxPrice = Panel.Params.MaxPrice;
            Panel.Params.oldMinPrice = Panel.Params.MinPrice;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isMax"></param>
        /// <returns></returns>
        private bool setDinamicMinMaxValue(decimal value, bool isMax)
        {
            if (isMax)
            {
                Panel.Params.MaxPrice = Panel.Params.oldMaxPrice + value;
            }
            else
            {
                Panel.Params.MinPrice = Panel.Params.oldMinPrice + value;
            }
            Panel.Params.WillRedraw();
            return true;
        }

        //Получает граничную округленную цену
        /*public static decimal GetBorderValue(decimal Value, decimal StepTicks)
        {
            string strValue = Value.ToString();
            int posFloatPoint = strValue.IndexOf(',');
            strValue = strValue.Replace(",", "");
            long Value = Convert.ToInt32(strValue);

            long k = (long)(Value / StepTicks);
            k = (int)(StepTicks * k);
            k = Value - (Value - k);

            long r = k;
            string resValue = r.ToString();
            if (strValue.Length > resValue.Length) posFloatPoint--;
            if (posFloatPoint >= 0) resValue = resValue.Insert(posFloatPoint, ",");

            decimal resultValue = Convert.ToDecimal(resValue);
            return resultValue;
        }*/
    }
}
