namespace GraphicTools
{
	public class GMath
	{
		/// <summary>
		/// Получение координат по Y относительно полотна, по цене
		/// </summary>
		/// <param name="Height">100% величина высоты</param>
		/// <param name="maxValue">Макс значение</param>
		/// <param name="minValue">Мин значение</param>
		/// <param name="Value">Получаемое значение</param>
        /// <param name="controlBorder">Флаг контроля границ</param>
		/// <returns></returns>
		public static int GetCoordinate(double Height, decimal maxValue, decimal minValue, decimal Value, bool controlBorder = true)
		{
			decimal Interval = maxValue - minValue;
			if (Interval == 0) return 0;
			double percentValue = (double)((Value - minValue) * 100 / Interval);
			//percentValue = percentValue < 0 ? percentValue * -1 : percentValue;
			var res = (int)(Height - (Height * percentValue / 100));
            if (controlBorder)
            {
                if (res > Height) return (int)Height;
                if (res < 0) return 0;
            }
            return res;
		}
        /// <summary>
        /// Получение координат по Y относительно полотна, по цене
        /// </summary>
        /// <param name="Height"></param>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <param name="Value"></param>
        /// <param name="controlBorder"></param>
        /// <returns></returns>
        public static float GetCoordinateF(float Height, decimal maxValue, decimal minValue, decimal Value, bool controlBorder = true)
        {
            double Interval = (double)(maxValue - minValue);
            if (Interval == 0) return 0;
            double percentValue = (double)(Value - minValue) * 100 / Interval;
            var res = (float)(Height - (Height * percentValue / 100));
            if (controlBorder)
            {
                if (res > Height) return Height;
                if (res < 0) return 0;
            }
            return res;
        }

        /// <summary>
        /// Получение значения между МИН и МАКС, по значению координат
        /// </summary>
        /// <param name="Length"></param>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <param name="ValueCoordinate"></param>
        /// <returns></returns>
        public static decimal GetValueFromCoordinate(int Length, decimal maxValue, decimal minValue, decimal ValueCoordinate, int scale = 2)
		{
			if (ValueCoordinate > Length) return -1;
			if (ValueCoordinate < 0) return -1;

			decimal Interval = maxValue - minValue;

			if (Interval == 0) return 0;

			decimal percentValue = ValueCoordinate * 100 / Length;
			percentValue = percentValue < 0 ? percentValue * -1 : percentValue;
			percentValue = 100 - percentValue;

			return System.Math.Round(minValue + (Interval * percentValue / 100), scale);
		}
	}
}
