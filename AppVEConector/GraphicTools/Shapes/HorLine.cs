using System.Drawing;


namespace GraphicTools.Shapes
{
	/// <summary>
	/// Класс отрисовки горизонтальных линий с надписью
	/// </summary>
	class HorLine :LineText
	{
		/// <summary> Нахождение текста по отношению к линии  </summary>
		public enum DirectionLine
		{
			Right = 1,
			Left = 2,
			Center = 3,
		};

		/// <summary>
		/// Нахождение текста относительно линии
		/// </summary>
		public DirectionLine TextHAlign = DirectionLine.Right;

		/// <summary>
		/// Рисует горизонтальную линию с надписью цены
		/// </summary>
		/// <param name="g"></param>
		/// <param name="rectPaint"></param>
		/// <param name="maxPrice"></param>
		/// <param name="minPrice"></param>
		public void Paint(Graphics g, Rectangle rectPaint, decimal valPrice, decimal maxPrice, decimal minPrice)
		{
			Paint(g, rectPaint, valPrice, valPrice.ToString(), maxPrice, minPrice);
		}
		/// <summary>
		/// Рисует горизонтальную линию с надписью цены
		/// </summary>
		/// <param name="g"></param>
		/// <param name="rectPaint"></param>
		/// <param name="valPrice"></param>
		/// <param name="Text"></param>
		/// <param name="maxPrice"></param>
		/// <param name="minPrice"></param>
		public void Paint(Graphics g, Rectangle rectPaint, decimal valPrice, string ValText, decimal maxPrice, decimal minPrice)
		{
			int Y = GMath.GetCoordinate(rectPaint.Height, maxPrice, minPrice, valPrice);
			//Если выходит за пределы, удаляем
			if (Y < 0 || Y > rectPaint.Y + rectPaint.Height) return;

			var font = new Font(this.FontFamily, this.FontSize, FontStyle.Regular, GraphicsUnit.Point);

			var dataText = g.MeasureString(ValText, font);
			int WidthText = (int)dataText.Width;
			int HeightText = (int)dataText.Height;

			Point pLine1 = new Point(rectPaint.X, rectPaint.Y + Y);
			Point pLine2 = new Point(rectPaint.X + rectPaint.Width - WidthText - 2, rectPaint.Y + Y);

			float x = 0;
			float y = rectPaint.Y + Y - (this.FontSize);

			//ВЫравнивание лейбла
			switch (this.TextHAlign)
			{
				case DirectionLine.Left:
					x = rectPaint.X;
					pLine1 = new Point(rectPaint.X + WidthText, rectPaint.Y + Y);
					pLine2 = new Point(rectPaint.X + rectPaint.Width, rectPaint.Y + Y);
					break;
				case DirectionLine.Center:
					//Canvas.SetLeft(this.TextLabel, PaintPanel.X + (PaintPanel.Width / 2 - this.TextLabel.ActualWidth));
					break;
				case DirectionLine.Right:
					x = rectPaint.X + rectPaint.Width - WidthText;					
					break;
			}

			if (this.FillText)
			{
				var rect = new RectDraw();
				rect.ColorBorder = rect.ColorFill = Color.White;
				rect.Paint(g, x, y, WidthText, HeightText);
			}

			g.DrawString(ValText, font, new SolidBrush(this.ColorText), x, y);

            PaintLine(g, pLine1, pLine2);
        }

        private void PaintLine(Graphics g, Point p1, Point p2)
        {
            var line = new Line();
            line.Width = this.WidthLine;
            line.Paint(g, p1, p2, this.ColorLine);
        }

        public void Paint(Graphics g, Rectangle rectPaint, int Y, string ValText, decimal maxPrice, decimal minPrice)
        {
            //Если выходит за пределы, удаляем
            if (Y < 0 || Y > rectPaint.Y + rectPaint.Height) return;

            var font = new Font(this.FontFamily, this.FontSize, FontStyle.Regular, GraphicsUnit.Point);

            var dataText = g.MeasureString(ValText, font);
            int WidthText = (int)dataText.Width;
            int HeightText = (int)dataText.Height;

            Point pLine1 = new Point(rectPaint.X, rectPaint.Y + Y);
            Point pLine2 = new Point(rectPaint.X + rectPaint.Width - WidthText - 2, rectPaint.Y + Y);

            float x = 0;
            float y = rectPaint.Y + Y - (this.FontSize);

            //ВЫравнивание лейбла
            switch (this.TextHAlign)
            {
                case DirectionLine.Left:
                    x = rectPaint.X;
                    pLine1 = new Point(rectPaint.X + WidthText, rectPaint.Y + Y);
                    pLine2 = new Point(rectPaint.X + rectPaint.Width, rectPaint.Y + Y);
                    break;
                case DirectionLine.Center:
                    //Canvas.SetLeft(this.TextLabel, PaintPanel.X + (PaintPanel.Width / 2 - this.TextLabel.ActualWidth));
                    break;
                case DirectionLine.Right:
                    x = rectPaint.X + rectPaint.Width - WidthText;
                    break;
            }

            if (this.FillText)
            {
                var rect = new RectDraw();
                rect.ColorBorder = rect.ColorFill = Color.White;
                rect.Paint(g, x, y, WidthText, HeightText);
            }

            g.DrawString(ValText, font, new SolidBrush(this.ColorText), x, y);

            PaintLine(g, pLine1, pLine2);
        }
    }
}
