using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicTools.Shapes
{
	class LineText
	{
		/// <summary>
		/// Цвет линии
		/// </summary>
		public Color ColorLine = Color.LightGray;
		/// <summary>
		/// Цвет текста
		/// </summary>
		public Color ColorText = Color.LightGray;
		/// <summary>
		/// Шрифт
		/// </summary>
		public FontFamily FontFamily = new FontFamily("Helvetica");
		/// <summary>
		/// Размер шрифта
		/// </summary>
		public int FontSize = 8;
		/// <summary>
		/// Ширина линии
		/// </summary>
		public float WidthLine = 1;
		/// <summary>
		/// Флаг определяющий закрашивать фон под текстом или нет.
		/// </summary>
		public bool FillText = false;
		/// <summary>
		/// Цвет заливки фона под текстом
		/// </summary>
		public Color ColorFillText = Color.White;
	}
}
