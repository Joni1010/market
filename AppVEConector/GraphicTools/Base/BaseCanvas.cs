using GraphicTools.Shapes;
using System;
using System.Drawing;


namespace GraphicTools.Base
{
    public class BaseCanvas
    {
        private readonly object _lock = new object();

        /// <summary> Слой отрисовки </summary>
        protected Bitmap ClipBoardLayout = null;

        /// <summary>
        /// Размеры панели
        /// </summary>
        public GRectangle Rect = new GRectangle();

        /// <summary>
        /// Получить внутренний холст
        /// </summary>
        public Graphics GetGraphics
        {
            get
            {
                lock (_lock)
                {
                    if (this.ClipBoardLayout.IsNull()) return null;
                    return Graphics.FromImage(this.ClipBoardLayout);
                }
            }
        }
        /// <summary>
        /// Событие изменения размера полотна
        /// </summary>
        public event Action<Rectangle> OnResize;

        /// <summary>
        /// Очистка внутреннего полотна
        /// </summary>
        public void Clear()
        {
            if (Rect.Width <= 0 || Rect.Height <= 0)
            {
                return;
            }
            lock (_lock)
            {
                if (ClipBoardLayout.NotIsNull() &&
                    ClipBoardLayout.Size.Width == Rect.Width &&
                    ClipBoardLayout.Size.Height == Rect.Height)
                {
                    GetGraphics.Clear(Color.Transparent);
                    return;
                }
            }
            try
            {
                lock (_lock)
                {
                    this.ClipBoardLayout = new Bitmap(Rect.Width, Rect.Height);
                }
                this.Rect.Rectangle = this.Rect.SetWidth(Rect.Width);
                this.Rect.Rectangle = this.Rect.SetHeight(Rect.Height);
            }
            catch (Exception)
            {
                return;
            }
        }
        /// <summary>
        /// Перенести временное полотно на холст
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void LayoutToCanvas(Graphics canvas, int x, int y)
        {
            lock (_lock)
            {
                if (this.ClipBoardLayout.NotIsNull())
                {
                    canvas.DrawImageUnscaled(this.ClipBoardLayout, new Point(x, y));
                }
            }
        }
    }
}
