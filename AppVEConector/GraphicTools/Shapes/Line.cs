using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicTools.Shapes
{
	class Line
	{
		/// <summary>
		/// Ширина линии
		/// </summary>
		public float Width = 1;
        /// <summary>
        /// Стиль линии
        /// </summary>
        public DashStyle Style = DashStyle.Solid;

        /// <summary> Отрисовка линии </summary>
        /// <param name="g"></param>
        /// <param name="pointStart"></param>
        /// <param name="pointEnd"></param>
        /// <param name="color"></param>
        public void Paint(Graphics g, PointF pointStart, PointF pointEnd, Color color)
		{
            Pen pen = new Pen(color, this.Width);
            pen.DashStyle = Style;
            g.DrawLine(pen, pointStart, pointEnd);
		}
        /// <summary> Отрисовка линии </summary>
        /// <param name="g"></param>
        /// <param name="pointStart"></param>
        /// <param name="pointEnd"></param>
        /// <param name="pen"></param>
        public void PaintTransp(Graphics g, PointF pointStart, PointF pointEnd, Pen pen)
        {
            g.DrawLine(pen, pointStart, pointEnd);
        }

    }
}
