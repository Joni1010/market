using AppVEConector.libs.Signal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppVEConector
{
    public partial class Form_MessageSignal : Form
    {
        private static string TextMsg = "";

        private static Form_MessageSignal form = null;

        public static void Show(string test, bool sendSignal = false)
        {
            if (form.IsNull() || form.IsDisposed)
            {
                form = new Form_MessageSignal();
            }
            form.TopMost = true;
            form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            form.CenterToScreen();
            form.Show();
            if (sendSignal)
            {
                SignalView.GSMSignaler.SendSignalCall();
            }
            TextMsg = test + "\r\n" + TextMsg;
            form.textBox1.Text = TextMsg;
        }

        public Form_MessageSignal()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form_MessageSignal_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = TextMsg;
        }
    }
}
