using AppVEConector.libs.Signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppVEConector
{
    public partial class Form_GraphicDepth : Form
    {
        private int LastIndexSelectRow = 0;
        /// <summary>
        /// 
        /// </summary>
        private void InitPanelSignals()
        {
            UpdateGridSignals();

            buttonSignalDelete.Click += (s, e) =>
            {
                deleteSignal();
            };
            buttonSignalUpdate.Click += (s, e) =>
            {
                UpdateGridSignals();
            };
            SignalView.GSMSignaler.OnRemove += (signal) => {
                UpdateGridSignals();
            };
            SignalView.GSMSignaler.OnAdd += (signal) => {
                UpdateGridSignals();
            };
        }
        /// <summary>
        /// 
        /// </summary>
        private void deleteSignal()
        {
            if (dataGridViewSignal.SelectedRows.NotIsNull() && dataGridViewSignal.SelectedRows.Count > 0)
            {
                dataGridViewSignal.SelectedRows.ForEach<DataGridViewRow>((row) =>
                {
                    if (row is DataGridViewRow)
                    {
                        if (row.Tag is SignalMarket)
                        {
                            LastIndexSelectRow = row.Index - 1;
                            SignalView.GSMSignaler.RemoveSignal((SignalMarket)row.Tag);
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateGridSignals()
        {
            SignalView.constructGridSignals(dataGridViewSignal, Securities.ToString(), () =>
            {
                LastIndexSelectRow = LastIndexSelectRow < 0 ? 0 : LastIndexSelectRow;
                LastIndexSelectRow = LastIndexSelectRow >= dataGridViewSignal.Rows.Count ? dataGridViewSignal.Rows.Count - 1 : LastIndexSelectRow;
                dataGridViewSignal.Rows.ForEach<DataGridViewRow>((row) =>
                {
                    row.Selected = false;
                    if (row.Index == LastIndexSelectRow)
                    {
                        row.Selected = true;
                        dataGridViewSignal.FirstDisplayedScrollingRowIndex = LastIndexSelectRow;
                    }
                });
                /*foreach (DataGridViewRow row in dataGridViewSignal.Rows)
                {
                    
                }*/
            });
        }

    }
}
