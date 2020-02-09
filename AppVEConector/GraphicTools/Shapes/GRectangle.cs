using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicTools.Shapes
{
    /// <summary>
    /// Класс отрисовки прямоугольника
    /// </summary>
    public class RectDraw
    {
        /// <summary>
        /// Цвет заливки
        /// </summary>
        public Color ColorFill = Color.White;
        /// <summary>
        /// Цвет границы
        /// </summary>
        public Color ColorBorder = Color.Black;
        /// <summary>
        /// Ширина линии границы
        /// </summary>
        public float WidthBorder = .2f;
        /// <summary>
        /// Рисует прямоугольник
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public RectangleF Paint(Graphics g, float x, float y, float width, float height)
        {
            RectangleF rect = new RectangleF(x, y, width, height);
            g.FillRectangle(new SolidBrush(this.ColorFill), rect);
            g.DrawRectangle(new Pen(this.ColorBorder, this.WidthBorder), rect.X, rect.Y, rect.Width, rect.Height);
            return rect;
        }
        /// <summary>
        /// Рисует прямоугольник
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="colorBorder"></param>
        /// <param name="fillColor"></param>
        /// <returns></returns>
        public RectangleF Paint(Graphics g, float x, float y, float width, float height, Color colorBorder, Color fillColor)
        {
            this.ColorBorder = colorBorder;
            this.ColorFill = fillColor;
            RectangleF rect = new RectangleF(x, y, width, height);
            g.FillRectangle(new SolidBrush(fillColor), rect);
            g.DrawRectangle(new Pen(this.ColorBorder, this.WidthBorder), rect.X, rect.Y, rect.Width, rect.Height);
            return rect;
        }
    }

    /// <summary>
    /// Класс для работы с пряугольными координатами
    /// </summary>
    public class GRectangle
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        /// <summary>
        /// Получить структуру прамоугольника
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(X, Y, Width, Height);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Width = value.Width;
                Height = value.Height;
            }
        }


        public GRectangle(int x = 0, int y = 0, int width = 0, int height = 0)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public GRectangle(Rectangle rect)
        {
            X = rect.X;
            Y = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }

        public static GRectangle operator +(GRectangle r1, GRectangle r2)
        {
            GRectangle res = new GRectangle();
            res.X = r1.X < r2.X ? r1.X : r2.X;
            res.Y = r1.Y < r2.Y ? r1.Y : r2.Y;
            res.Width = r1.X + r1.Width > r2.X + r2.Width ? r1.X + r1.Width - res.X : r2.X + r2.Width - res.X;
            res.Height = r1.X + r1.Height > r2.Y + r2.Height ? r1.Y + r1.Height - res.Y : r2.Y + r2.Height - res.Y;
            return res;
        }
        /// <summary>
        /// Обьединяет две прямоугольные области
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static GRectangle Join(GRectangle r1, GRectangle r2)
        {
            GRectangle res = new GRectangle();
            res.X = r1.X < r2.X ? r1.X : r2.X;
            res.Y = r1.Y < r2.Y ? r1.Y : r2.Y;
            res.Width = r1.X + r1.Width > r2.X + r2.Width ? r1.X + r1.Width - res.X : r2.X + r2.Width - res.X;
            res.Height = r1.X + r1.Height > r2.Y + r2.Height ? r1.Y + r1.Height - res.Y : r2.Y + r2.Height - res.Y;
            return res;
        }

        /*public static bool operator ==(RectanglePaint r1, RectanglePaint r2)
		{
			if (r1.X != r2.X) return false;
			if (r1.Y != r2.Y) return false;
			if (r1.Height != r2.Height) return false;
			if (r1.Width != r2.Width) return false;
			return true;
		}*/

        /// <summary>
        /// Обрезает в r1 область шириной r2, справа.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static GRectangle DeleteRight(GRectangle r1, GRectangle r2)
        {
            GRectangle res = new GRectangle();
            res.X = r1.X;
            res.Y = r1.Y;
            res.Width = r1.Width - r2.Width;
            res.Height = r1.Height;
            return res;
        }


        /// <summary>
        /// Обрезает в r1 область высотой r2, снизу.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static GRectangle DeleteBottom(GRectangle r1, GRectangle r2)
        {
            GRectangle res = new GRectangle();
            res.X = r1.X;
            res.Y = r1.Y;
            res.Width = r1.Width;
            res.Height = r1.Height - r2.Height;
            return res;
        }
        /// <summary>
        /// Добавляет к области r1, область высотой r2, снизу.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static GRectangle AttachBottom(GRectangle r1, GRectangle r2)
        {
            GRectangle res = new GRectangle();
            res.X = r1.X;
            res.Y = r1.Y;
            res.Width = r1.Width;
            res.Height = r1.Height + r2.Height;

            return res;
        }

        /// <summary>
        /// Добавляет к области r1, область высотой r2, справа.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static GRectangle AttachRight(GRectangle r1, GRectangle r2)
        {
            GRectangle res = new GRectangle();
            res.X = r1.X;
            res.Y = r1.Y;
            res.Width = r1.Width + r2.Width;
            res.Height = r1.Height;

            return res;
        }
        public static GRectangle AttachRight(GRectangle r1, int Width)
        {
            GRectangle res = new GRectangle();
            res.X = r1.X;
            res.Y = r1.Y;
            res.Width = r1.Width + Width;
            res.Height = r1.Height;

            return res;
        }

        /// <summary>
        /// Выделить область прямоугольную, справа в текущем прямоугольнике, указанной ширины.
        /// </summary>
        /// <param name="mainRetc"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static GRectangle GetRight(GRectangle mainRect, int width)
        {
            GRectangle res = new GRectangle();
            res.X = mainRect.X + mainRect.Width - width;
            res.Y = mainRect.Y;
            res.Width = width;
            res.Height = mainRect.Height;

            mainRect.Width -= width;
            return res;
        }
        /// <summary>
        /// Выделить область прямоугольную, снизу в текущем прямоугольнике, указанной высоты.
        /// </summary>
        /// <param name="mainRect"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static GRectangle GetBottom(GRectangle mainRect, int height)
        {
            GRectangle res = new GRectangle();
            res.X = mainRect.X;
            res.Y = mainRect.Y + mainRect.Height - height;
            res.Width = mainRect.Width;
            res.Height = height;

            return res;
        }

        public override bool Equals(object obj)
        {
            var tools = obj as GRectangle;
            return tools != null &&
                   X == tools.X &&
                   Y == tools.Y &&
                   Width == tools.Width &&
                   Height == tools.Height;
        }

        public override int GetHashCode()
        {
            var hashCode = 466501756;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }

        public static bool operator !=(GRectangle r1, GRectangle r2)
        {
            if (Object.Equals(r1, null) && !Object.Equals(r2, null)) return true;
            if (Object.Equals(r2, null) && !Object.Equals(r1, null)) return true;
            if (Object.Equals(r1, null) && Object.Equals(r2, null)) return false;
            if (r1.Width != r2.Width) return true;
            if (r1.Height != r2.Height) return true;
            if (r1.X != r2.X) return true;
            if (r1.Y != r2.Y) return true;
            return false;
        }

        public static bool operator ==(GRectangle r1, GRectangle r2)
        {
            if (Object.Equals(r1, null) && !Object.Equals(r2, null)) return false;
            if (Object.Equals(r2, null) && !Object.Equals(r1, null)) return false;
            if (Object.Equals(r1, null) && Object.Equals(r2, null)) return true;
            if (r1.Width == r2.Width && r1.Height == r2.Height && r1.X == r2.X && r1.Y == r2.Y) return true;
            return false;
        }


        /// <summary>
		/// Добавить значение к текущему значению высоты
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public Rectangle AddHeight(int value)
        {
            Height += value;
            return this.Rectangle;
        }

        public Rectangle DecHeight(int value)
        {
            Height -= value;
            return this.Rectangle;
        }
        public Rectangle DecWidth(int value)
        {
            Width -= value;
            return this.Rectangle;
        }

        public Rectangle SetX(int value)
        {
            X = value;
            return this.Rectangle;
        }
        public Rectangle SetY(int value)
        {
            Y = value;
            return this.Rectangle;
        }

        /// <summary>
        /// Установить значение к текущей высоты
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Rectangle SetHeight(int value)
        {
            Height = value;
            return this.Rectangle;
        }
        /// <summary>
        /// Установить значение текущей ширины
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Rectangle SetWidth(int value)
        {
            Width = value;
            return this.Rectangle;
        }
    }
}
