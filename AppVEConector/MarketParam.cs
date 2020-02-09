using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVEConector
{
	[Serializable]
	public class MarketParam
	{
		/// <summary> Закрывать все стоп-лосы при 0 позиции </summary>
		public bool CancelStopOrderByNull = false;
	}
}
