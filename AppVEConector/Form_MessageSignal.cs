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
        private struct RowSignal
        {
            public string Signal;
            public string SecAndClass;
        }
        public static MainForm PForm = null;
        //private static string TextMsg = "";
        private static List<RowSignal> listSignals = new List<RowSignal>();

        private static Form_MessageSignal form = null;

        public static void Show(string text, string secAndClass, bool sendSignal = false)
        {
            if (form.IsNull() || form.IsDisposed)
            {
                form = new Form_MessageSignal();
            }
            form.TopMost = true;
            form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            
            if (sendSignal)
            {
                SignalView.GSMSignaler.SendSignalCall();
            }
            listSignals.Insert(0, new RowSignal() { Signal = text, SecAndClass = secAndClass });

            form.CenterToScreen();
            form.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        private void fillGridSignalls()
        {
            var rowForClone = (DataGridViewRow)dataGridViewInfoSignal.Rows[0].Clone();
            dataGridViewInfoSignal.Rows.Clear();
            var list = listSignals.ToArray();
            foreach (var sig in list)
            {
                var newRow = (DataGridViewRow)rowForClone.Clone();
                newRow.Cells[0].Value = sig.Signal;
                if (!sig.SecAndClass.Empty())
                {
                    DataGridViewButtonCell btn = new DataGridViewButtonCell();
                    newRow.Cells[1] = btn;
                    btn.Value = "График";
                    btn.Tag = sig.SecAndClass;
                    //btn.UseColumnTextForButtonValue = true;
                    /*
                    Button btn = new Button();
                    btn.Text = "График";
                    newRow.Cells[1].Value = btn;
                    btn.Click += (s, e) =>
                    {
                    };*/
                } else
                {
                    newRow.Cells[1].Value = "";
                }
                dataGridViewInfoSignal.Rows.Add(newRow);
            }
            dataGridViewInfoSignal.Update();
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
            dataGridViewInfoSignal.CellContentClick += (s, ee) =>
            {
                var cell = dataGridViewInfoSignal.Rows[ee.RowIndex].Cells[ee.ColumnIndex];
                if (cell.NotIsNull() && cell.Tag.NotIsNull())
                {
                    var sec = (string)cell.Tag;
                    if (PForm.NotIsNull())
                    {
                        PForm.ShowGraphicDepth(PForm.GetSecByCode(sec));
                    }
                }
            };
            fillGridSignalls();

        }
    }
}
