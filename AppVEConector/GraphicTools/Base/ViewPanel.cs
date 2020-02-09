
using GraphicTools.Shapes;
using System.Drawing;

namespace GraphicTools.Base
{
    public class ViewPanel : BaseCanvas
    {
        /// <summary>
        /// Прямоугольник позиционирования на полотне
        /// </summary>
        public GRectangle RectScreen = new GRectangle();

        public delegate void eventMouseInPanel(Point coordMouse);
        public delegate void changeRect(GRectangle newRect);
        /// <summary>
        /// Событие перемещения мыши по панели
        /// </summary>
        public eventMouseInPanel OnMouseMove;
        public eventMouseInPanel OnMouseUp;
        public eventMouseInPanel OnMouseDown;
        public changeRect OnChangeRect = null;


        public BaseParams Params = null;


        /// <summary>
        /// Ширина панели
        /// </summary>
        public int Width
        {
            get
            {
                return Rect.Width;
            }
        }
        /// <summary>
        /// Высота панели
        /// </summary>
        public int Height
        {
            get
            {
                return Rect.Height;
            }
        }

        public ViewPanel(BaseParams param)
        {
            Params = param;
        }

        public void SetRect(GRectangle rect)
        {
            if (!RectScreen.Equals(rect))
            {
                RectScreen.Rectangle = rect.Rectangle;
                Rect.X = 0;
                Rect.Y = 0;
                Rect.Width = rect.Width;// new GRectangle(0, 0, rect.Width, rect.Height);
                Rect.Height = rect.Height;// new GRectangle(0, 0, rect.Width, rect.Height);
                if (OnChangeRect.NotIsNull())
                {
                    OnChangeRect(rect);
                }
                Clear();
            }
        }
        /// <summary>
        /// Выделить панель справа от текущей
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public GRectangle ExtractRight(int width)
        {
            if (width == 0)
            {
                return RectScreen;
            }
            GRectangle res = new GRectangle();
            res.X = RectScreen.X + RectScreen.Width - width;
            res.Y = RectScreen.Y;
            res.Width = width;
            res.Height = RectScreen.Height;

            SetRect(new GRectangle() { X = RectScreen.X, Y = RectScreen.Y, Width = RectScreen.Width - width, Height = RectScreen.Height });
            Clear();
            return res;
        }

        public GRectangle ExtractLeft(int width)
        {
            if (width == 0)
            {
                return RectScreen;
            }
            GRectangle res = new GRectangle();
            res.X = RectScreen.X;
            res.Y = RectScreen.Y;
            res.Width = width;
            res.Height = RectScreen.Height;

            SetRect(new GRectangle() { X = RectScreen.X + width, Y = RectScreen.Y, Width = RectScreen.Width, Height = RectScreen.Height });
            Clear();
            return res;
        }

        /// <summary>
        /// Выделить панель снизу от текущей
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public GRectangle ExtractBottom(int height)
        {
            if (height == 0)
            {
                return RectScreen;
            }
            GRectangle res = new GRectangle();
            res.X = RectScreen.X;
            res.Y = RectScreen.Y + RectScreen.Height - height;
            res.Width = RectScreen.Width;
            res.Height = height;

            SetRect(new GRectangle() { X = RectScreen.X, Y = RectScreen.Y, Width = RectScreen.Width, Height = RectScreen.Height - height });
            Clear();
            return res;
        }

        public void Paint(Graphics canvas)
        {
            this.LayoutToCanvas(canvas, RectScreen.X, RectScreen.Y);
        }

        /// <summary>
        /// Проверяем попадают ли координаты в область панели
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool checkCoordHit(Point point)
        {
            var x1 = this.Rect.X + this.Rect.Width;
            var x2 = this.Rect.X + this.Rect.Width;
            var y1 = this.Rect.Y;
            var y2 = this.Rect.Y + this.RectScreen.Height;
            if (point.X > x1 && point.X < x2 && point.Y > y1 && point.Y < y2)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Проверка перемещения мыши по области значений
        /// </summary>
        /// <param name="eventMouse"></param>
        /// <returns></returns>
        public bool CheckMouseMove(Point point)
        {
            if (checkCoordHit(point))
            {
                if (OnMouseMove.NotIsNull())
                {
                    OnMouseMove(point);
                }
                return true;
            }
            return false;
        }
    }
}
