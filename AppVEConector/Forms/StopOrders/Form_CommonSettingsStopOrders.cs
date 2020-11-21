using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AppVEConector.Forms.StopOrders
{
    public partial class Form_CommonSettingsStopOrders : Form
    {
        private new Form_LightOrders ParentForm = null;
        public Form_CommonSettingsStopOrders(Form_LightOrders formStopOrders)
        {
            InitializeComponent();
            ParentForm = formStopOrders;
        }
        private void Form_CommonSettingsStopOrders_Load(object sender, EventArgs e)
        {
            initAddSec();
            initDeleteSec();
            updateGridListSec();
        }

        private Connector.QuikConnector Trader
        {
            get { return ParentForm.Parent.Trader; }
        }

        private void initAddSec()
        {
            comboBoxSearchSec.TextChanged += (s, e) =>
            {
                var text = comboBoxSearchSec.Text;
                if (text.Length >= 2)
                {
                    var listSec = Trader.Objects.Securities.Where(el => el.Code.ToLower().Contains(text.ToLower()) ||
                        el.Name.ToLower().Contains(text.ToLower())).Select(el => el.ToString());
                    if (listSec.Count() > 0)
                    {
                        comboBoxSearchSec.Clear();
                        comboBoxSearchSec.SetListValues(listSec);
                    }
                }
                comboBoxSearchSec.Select(text.Length, 0);
            };
            buttonAddSecInList.Click += (s, e) =>
            {
                if (comboBoxSearchSec.SelectedItem.NotIsNull())
                {
                    var secAndClass = comboBoxSearchSec.SelectedItem.ToString();
                    var sec = Trader.Objects.Securities.FirstOrDefault(se => se.ToString() == secAndClass);
                    if (sec.NotIsNull())
                    {
                        ParentForm.Settings.Data.ItemsSec.Add(new Form_LightOrders.SettingsForm.PositionForm.ItemSec()
                        {
                            SecAndClass = sec.ToString(),
                            Name = sec.Shortname
                        });
                        updateGridListSec();
                    }
                }
            };
        }

        private void initDeleteSec()
        {
            buttonDelSec.Click += (s, e) =>
            {
                if(dataGridViewListSec.SelectedRows.NotIsNull() && dataGridViewListSec.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dataGridViewListSec.SelectedRows)
                    {
                        if (row.Tag.NotIsNull())
                        {
                            var itemSec = (Form_LightOrders.SettingsForm.PositionForm.ItemSec)row.Tag;
                            ParentForm.Settings.Data.ItemsSec.RemoveAll(i => i.SecAndClass == itemSec.SecAndClass);
                        }
                    }
                    updateGridListSec();
                }
            };
        }

        private void updateGridListSec()
        {
            dataGridViewListSec.ClearSelection();
            dataGridViewListSec.Rows.Clear();
            if (ParentForm.Settings.Data.ItemsSec.Count > 0)
            {
                foreach (var itemSec in ParentForm.Settings.Data.ItemsSec) {
                    var row = (DataGridViewRow)dataGridViewListSec.Rows[0].Clone();
                    row.Cells[0].Value = itemSec.SecAndClass;
                    row.Cells[1].Value = itemSec.Name;
                    row.Tag = itemSec;
                    dataGridViewListSec.Rows.Add(row);
                }
            }
        }

        private void Form_CommonSettingsStopOrders_FormClosing(object sender, FormClosingEventArgs e)
        {
            ParentForm.Settings.Save();
        }
    }
}
