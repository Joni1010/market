using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradingLib;

namespace AppVEConector
{
    public partial class Form_CopySecurity : Form
    {
        TElement TrElement = null;
        Connector.QuikConnector Trader = null;
        public Form_CopySecurity(Connector.QuikConnector trader, TElement trElement)
        {
            InitializeComponent();

            this.TrElement = trElement;
            this.Trader = trader;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var newObject = (Form_CopySecurity)this.MemberwiseClone();
            //newObject.TrElement.Security.Code = this.textBoxSecCode.Text;
            var sec = this.TrElement.Security.Clone();
            sec.Code = this.textBoxSecCode.Text; 
            sec.Shortname = this.textBoxSecCode.Text;

            this.Trader.Objects.tSecurities.Add(sec);
            this.Close();
        }
    }
}
