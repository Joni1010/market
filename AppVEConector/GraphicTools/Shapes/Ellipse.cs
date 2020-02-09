
using System.Drawing;

namespace GraphicTools.Shapes
{
    class Ellipse
    {
        /// <summary>
        /// Ширина линии
        /// </summary>
        public float Width = 1;
        /// <summary>
        /// Radius
        /// </summary>
        public float Radius = 1;
        /// <summary>
        /// Цвет линии
        /// </summary>
        public Color ColorLine = Color.Black;
        /// <summary>
        /// Цвет заливки
        /// </summary>
        public Color FillColor = Color.White;
        /// <summary>
        /// Залить цветом
        /// </summary>
        public bool Fill = false;
        /// <summary> Отрисовка линии </summary>
        /// <param name="g"></param>
        /// <param name="pointStart"></param>
        /// <param name="pointEnd"></param>
        /// <param name="color"></param>
        public void PaintCircle(Graphics g, PointF pointCenter)
        {
            g.DrawEllipse(new Pen(ColorLine, Width), pointCenter.X, pointCenter.Y, Radius * 2, Radius * 2);
            if (Fill)
            {
                g.FillEllipse(new SolidBrush(FillColor), pointCenter.X, pointCenter.Y, Radius * 2, Radius * 2);
            }
        }
    }
}
