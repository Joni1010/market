using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Candles;

namespace AppVEConector.Strategy
{
    public class FastGap : Strategy
    {
        public override void BeforeAction(Action action)
        {

        }
        public override string ActionCollection(IEnumerable<CandleData> candleCollection)
        {
            CandleData first = null;
            var candles = candleCollection.Skip(IndexStartCandle).Take(2);
            bool wasGap = false;
            decimal gap = 0;
            if (candles.Count() > 0)
            {
                foreach (var can in candles)
                {
                    if (first.IsNull())
                    {
                        first = can;
                    }
                    else
                    {
                        gap = can.Close - first.Close;
                        gap = gap < 0 ? gap * -1 : gap;
                        if (gap >= (Security.MinPriceStep * Option_1))
                        {
                            wasGap = true;
                            first = can;
                        }
                    }
                }
            }

            if (wasGap)
            {
                //MainForm.GSMSignaler.SendSignalCall();
                string appendLog = DateTime.Now.ToLongTimeString() + "\t" +
                    "Sec: " + Security.ToString() + "; " +
                    "Tf:" + TimeFrame.ToString() + "; " +
                    "GAP: " + gap.ToString() + "; " +
                   "BIG GAP SECURITY" +
                    "\r\n";
                return appendLog;
            }
            return "";
        }
    }
}
