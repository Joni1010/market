using Libs;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppVEConector
{
    public class AutoStopLoss : AutoControls<AutoStopLoss.Item>
    {
        const string FILENAME = "auto_stoploss.dat";

        public AutoStopLoss(string pathSaveFile) :
            base(pathSaveFile, FILENAME)
        {
            
        }
        [Serializable]
        public class Item
        {
            public string Comment = "";
            public decimal Price;
            public decimal Tiks;
            public string SecAndCode;
            public string SecName;
        }
    }
}
