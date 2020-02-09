using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicTools.Extension
{
    public class SelectCandle
    {
        /// <summary>
        /// Точка выделения (клик)
        /// </summary>
        public Point coordClick;
        /// <summary>
        /// Данные по свечке
        /// </summary>
        public GCandles.CandleInfo dataCandle;
    }
}
