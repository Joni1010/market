using QuikConnector.MarketObjects;
using System;


namespace AppVEConector.libs.Signal
{
	[Serializable]
	public class SignalMarket
	{
		/// <summary> Условия выполнения сигналов </summary>
		public enum CondSignal
		{
			More = 1,
			MoreOrEquals = 2,
			Less = 3,
			LessOrEquals = 4,
			Equals = 5
		}
		/// <summary>
		/// Тип сигнала
		/// </summary>
		public enum TypeSignal
		{
			ByPrice = 0,
			ByTime = 1,
			ByVolume = 2,
		}
		/// <summary> Тип сигнала </summary>
		public TypeSignal Type = TypeSignal.ByPrice;

		/// <summary> Инструмент SEC:CLASS</summary>
		public string SecClass = null;
		/// <summary> Цена исполнения сигнала </summary>
		public decimal Price = -1;
		public long Volume = -1;
		/// <summary> Условие исполнения сигнала </summary>
		public CondSignal Condition;
		/// <summary> Дата сигнала  </summary>
		public DateMarket DateTime = null;
		/// <summary> Комментарий </summary>
		public string Comment = "";
		/// <summary>  Флаг что сигнал бесконечный. </summary>
		public bool Infinity = false;
		/// <summary> Тайм фрейм </summary>
		public int TimeFrame = -1;
		/// <summary> Время, анти повторения </summary>
		public DateMarket TimeAntiRepeat = null;
        /// <summary>
        /// Внутренний ID
        /// </summary>
        public long Id = 0;
        public object Tag = null;
	}
}
