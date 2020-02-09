using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicTools.Shapes
{
	class VerLine :LineText
	{
		/// <summary> Нахождение текста по отношению к линии  </summary>
		public enum DirectionLine
		{
			Top = 1,
			Bottom = 2,
		};
		/// <summary>
		/// Нахождение текста относительно линии
		/// </summary>
		public DirectionLine TextVAlign = DirectionLine.Bottom;

		/// <summary>
		/// Рисует вертикальнцю линию с надписью
		/// </summary>
		/// <param name="g"></param>
		/// <param name="ValText"></param>
		/// <param name="pStart"></param>
		/// <param name="pEnd"></param>
		public void Paint(Graphics g, string ValText, PointF pStart, PointF pEnd)
		{
            int WidthText = 0;
            int HeightText = 0;
            var textP = new TextDraw();
            if (ValText.NotIsNull())
            {
                textP.SetFontSize(7);
                textP.Color = this.ColorText;
                var sizeText = textP.GetSizeText(g, ValText);
                WidthText = (int)sizeText.Width;
                HeightText = (int)sizeText.Height;
            }

			float x = pEnd.X - WidthText / 2;
			float y = 0;

			//Выравнивание лейбла
			switch (this.TextVAlign)
			{
				case DirectionLine.Bottom:
					pEnd.Y -= HeightText;
					y = pEnd.Y;
					break;
				case DirectionLine.Top:
					pStart.Y += HeightText;
					y = pStart.Y + HeightText;					
					break;
			}			
            if (ValText.NotIsNull())
            {
                if (this.FillText)
                {
                    var rect = new RectDraw();
                    rect.ColorBorder = rect.ColorFill = Color.White;
                    rect.Paint(g, x, y, WidthText, HeightText);
                }
                textP.Paint(g, ValText, x, y);
            }

			var line = new Line();
			line.Width = this.WidthLine;
			line.Paint(g, pStart, pEnd, this.ColorLine);
		}

	}
}
