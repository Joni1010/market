using Market.AppTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AppVEConector
{
    public partial class Form_CopySecurity : Form
    {
        TElement TrElement = null;
        public Form_CopySecurity( TElement trElement)
        {
            InitializeComponent();
            this.TrElement = trElement;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var newObject = (Form_CopySecurity)this.MemberwiseClone();
            //newObject.TrElement.Security.Code = this.textBoxSecCode.Text;
            var sec = this.TrElement.Security.Clone();
            sec.Code = this.textBoxSecCode.Text; 
            sec.Shortname = this.textBoxSecCode.Text;

            Quik.Trader.Objects.tSecurities.Add(sec);
            this.Close();
        }
    }
}
