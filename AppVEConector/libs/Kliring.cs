using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVEConector.libs
{
	public class Kliring
	{
		public DateTime Time;
		public int Period = 0;

		public Kliring(string formatStrKlir)
		{
			this.getValueKlir(formatStrKlir);
		}

		private void getValueKlir(string klir)
		{
			var dataKlir = klir.Split('-');
			if (dataKlir.Length == 2)
			{
				var time = dataKlir[0];
				this.Period = dataKlir[1].ToInt32();
				//var varTime = TimeSpan.Parse(time);
				this.Time =  Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + time); 
			}
		}
	}
}
