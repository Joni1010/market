using System.Drawing;


namespace GraphicTools
{
	static class RectangleFExtension
	{
		/// <summary>
		/// Добавить значение к текущему значению высоты
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static RectangleF AddHeight(this RectangleF obj, float value)
		{
			obj.Height += value;
			return obj;
		}

		public static RectangleF DecHeight(this RectangleF obj, float value)
		{
			obj.Height -= value;
			return obj;
		}
		public static RectangleF DecWidth(this RectangleF obj, float value)
		{
			obj.Width -= value;
			return obj;
		}

		public static RectangleF SetX(this RectangleF obj, float value)
		{
			obj.X = value;
			return obj;
		}
		public static RectangleF SetY(this RectangleF obj, float value)
		{
			obj.Y = value;
			return obj;
		}
		/// <summary>
		/// Установить значение к текущей высоты
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static RectangleF SetHeight(this RectangleF obj, int value)
		{
			obj.Height = value;
			return obj;
		}
		/// <summary>
		/// Установить значение текущей ширины
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static RectangleF SetWidth(this RectangleF obj, float value)
		{
			obj.Width = value;
			return obj;
		}
	}
	static class RectangleExtension
	{
		/// <summary>
		/// Добавить значение к текущему значению высоты
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Rectangle AddHeight(this Rectangle obj, int value)
		{
			obj.Height += value;
			return obj;
		}

		public static Rectangle DecHeight(this Rectangle obj, int value)
		{
			obj.Height -= value;
			return obj;
		}
		public static Rectangle DecWidth(this Rectangle obj, int value)
		{
			obj.Width -= value;
			return obj;
		}

		public static Rectangle SetX(this Rectangle obj, int value)
		{
			obj.X = value;
			return obj;
		}
		public static Rectangle SetY(this Rectangle obj, int value)
		{
			obj.Y = value;
			return obj;
		}

		/// <summary>
		/// Установить значение к текущей высоты
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Rectangle SetHeight(this Rectangle obj, int value)
		{
			obj.Height = value;
			return obj;
		}
		/// <summary>
		/// Установить значение текущей ширины
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Rectangle SetWidth(this Rectangle obj, int value)
		{
			obj.Width = value;
			return obj;
		}
	}
}
