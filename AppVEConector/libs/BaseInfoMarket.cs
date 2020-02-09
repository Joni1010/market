using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libs
{
    /// <summary>
    /// Предрасчитанные данные по инструменту
    /// </summary>
    public struct BaseInfoMarket
    {
        public int CountOrderBuy;
        public int CountVolumeBuy;
        public int CountOrderSell;
        public int CountVolumeSell;

        public int CountStopOrderBuy;
        public int CountStopVolumeBuy;
        public int CountStopOrderSell;
        public int CountStopVolumeSell;

        public int CountStopLimitOrderBuy;
        public int CountStopLimitVolumeBuy;
        public int CountStopLimitOrderSell;
        public int CountStopLimitVolumeSell;

        public int CountCurrentPosition;
        public decimal CurrentPrice;
        public decimal ForecastSum;
        public decimal VarMargin;
        public decimal Balanse;
        public decimal FreeBalanse;
    }
}
