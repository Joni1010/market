using GraphicTools.Extension;
using MarketObjects;
using System.Drawing;
using System.Linq;

using System.Windows.Forms;
using GraphicTools.Base;

namespace GraphicTools
{
    public partial class Graphic : BGraphAdd
    {
        /// <summary>
        /// Объект полотна
        /// </summary>
        private Control ObjectCanvas = null;

        /// <summary> Активная свеча, на которую наведена мышь </summary>
        private GCandles.CandleInfo FocusCandle = null;

        /// <summary> Создает объект графика </summary>
        /// <param name="mainRect"></param>
        /// <param name="scale">Кол-во десятичных знаков после запятой</param>
        /// <param name="minStepPrice"></param>
        public Graphic()
        {

        }        

        public void Init(Securities security)
        {
            if (security.IsNull()) return;
            var param = new BaseParams()
            {
                CountFloat = security.Scale,
                MarginCandle = 0.5f,
                Class = security.Class.Code,
                Code = security.Code,
                MinStepPrice = security.MinPriceStep
            };
            MainPanel = new ViewPanel(param);

            InitAdd();
            InitNativeEvents();
        }
        /// <summary>  Указать тип горизонтальных объемов </summary>
        /// <param name="type"></param>
        public void SetTypeHorVolume(int type)
        {
            TypeHorVolume = type;
            GHorVolumes.Panel.Clear();
        }
        public void SetLimitBorderHorVolume(long limit)
        {
            limitHorVol = limit;
            GHorVolumes.Panel.Clear();
        }

        
        /// <summary>
        /// Возвращает тип гор. обьема
        /// </summary>
        /// <returns></returns>
        public int GetTypeHorVolume()
        {
            return TypeHorVolume;
        }

        /// <summary> Перерисовать все объекты </summary>
        public void Paint()
        {
            ComputeParams(ObjectCanvas.ClientRectangle);
            //Получаем необходимые свечи
            GetCollectionCandles();
            
            PaintAll();
            RedrawActual();
        }

        /// <summary>
        /// Перерисовать только активные части
        /// </summary>
        /// <param name="comParams">Пересичтать параметры</param>
        public void RedrawActual()
        {
            if (Candels.AllDataPaintedCandle.Count == 0)
            {
                return;
            }
            FocusCandle = null;

            //Рисуем активную свечу справа
            PaintActual(Candels.AllDataPaintedCandle[0]);
            //Ищем активную(наведенную свечу)
            FocusCandle = Candels.AllDataPaintedCandle.FirstOrDefault(dCan => dCan.Body.X <= CrossPoint.X && dCan.Body.X + dCan.Body.Width >= CrossPoint.X);
            if (FocusCandle.NotIsNull())
            {
                Cross.PaintCrossLines(CrossPoint, FocusCandle);
                if (FocusCandle.TailCoord.High.Y <= CrossPoint.Y && FocusCandle.TailCoord.Low.Y >= CrossPoint.Y)
                {
                    //Событие передвижения по свечке
                    if (OnCandleMove.NotIsNull())
                    {
                        OnCandleMove(CrossPoint, FocusCandle);
                    }
                }
            }
            else
            {
                Cross.PaintCrossLines(CrossPoint, null);
            }
        }

        /// <summary>
        /// Устанавливает объект полотна
        /// </summary>
        /// <param name="obj"></param>
        public void SetObjectPaint(Control obj)
        {
            ObjectCanvas = obj;
            ObjectCanvas.Click += (s, e) =>
            {
                var MouseEvent = (MouseEventArgs)e;
                if (MouseEvent.Button == MouseButtons.Left)
                {
                    // GetLeftClick(new Point(MouseEvent.X, MouseEvent.Y));
                }
                if (MouseEvent.Button == MouseButtons.Right)
                {
                    // GetRightClick(new Point(MouseEvent.X, MouseEvent.Y));
                }
            };
            ObjectCanvas.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    var p = new Point(e.X, e.Y);
                    DragAndDrop.startDrag(p);
                    GetLeftDown(p);
                }
                ObjectCanvas.Refresh();
            };
            ObjectCanvas.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    var p = new Point(e.X, e.Y);
                    DragAndDrop.endDrag();
                    GetLeftUp(p);
                }
                ObjectCanvas.Refresh();
            };
            ObjectCanvas.Resize += (s, e) =>
            {
                Paint();
                ObjectCanvas.Refresh();
            };
            ObjectCanvas.MouseMove += (s, e) =>
            {
                if (CrossPoint.X != e.X || CrossPoint.Y != e.Y)
                {
                    CrossPoint = new Point(e.X, e.Y);
                    DragAndDrop.Check(CrossPoint);

                    RedrawActual();
                    ObjectCanvas.Refresh();
                }
            };
        }
    }
}
