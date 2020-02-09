using System.Drawing;


namespace GraphicTools.Shapes
{
    class TextDraw
    {
        /// <summary>
        /// Цвет текста
        /// </summary>
        public Color Color = Color.Black;
        /// <summary>
        /// Шрифт
        /// </summary>
        public FontFamily FontFamily = new FontFamily("Helvetica");
        /// <summary>
        /// ФОрмат текста
        /// </summary>
        public Font Font = null;
        /// <summary>
        /// Размер шрифта
        /// </summary>
        private int FontSize = 8;

        public TextDraw()
        {
            this.Font = new Font(this.FontFamily, this.FontSize, FontStyle.Regular, GraphicsUnit.Point);
        }

        public void SetFontSize(int size)
        {
            this.FontSize = size;
            this.Font = new Font(this.FontFamily, this.FontSize, FontStyle.Regular, GraphicsUnit.Point);
        }
        /// <summary>
        /// Рисует текст на полотне
        /// </summary>
        /// <param name="g"></param>
        /// <param name="text"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="color"></param>
        public void Paint(Graphics g, string text, float X, float Y, Color color)
        {
            this.Color = color;

            var dataText = this.GetSizeText(g, text);
            int WidthText = (int)dataText.Width;
            int HeightText = (int)dataText.Height;

            var rect = new RectDraw();
            rect.ColorBorder = rect.ColorFill = Color.FromArgb(200, Color.White);
            rect.Paint(g, X, Y, WidthText, HeightText);

            g.DrawString(text, this.Font,
                        new SolidBrush(this.Color), X, Y);
        }
        public void Paint(Graphics g, string text, float X, float Y)
        {
            this.Paint(g, text, X, Y, this.Color);
        }

        public SizeF GetSizeText(Graphics g, string text)
        {
            return g.MeasureString(text, this.Font);
        }
    }
}
