using MarketObjects;
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

        private void PanelSettings_Init()
        {
            //Получаем коды клиента
            comboBoxCodeClient.SetListValues(this.Trader.Objects.Clients.Select(c => c.Code).ToArray(), SettingsDepth.Data.CodeClient);
            comboBoxCodeClient.SelectedValueChanged += (ss, ee) =>
            {
                if (comboBoxCodeClient.SelectedItem.NotIsNull())
                {
                    SettingsDepth.Data.CodeClient = comboBoxCodeClient.SelectedItem.ToString();
                }
                PanelSettings_setTypeClientLimit();
            };
            comboBoxTypeClientLimit.SelectedValueChanged += (ss, ee) =>
            {
                SettingsDepth.Data.TypeClientLimit = comboBoxTypeClientLimit.SelectedItem.ToString().ToInt32();
            };

            checkBoxAutoSizePrice.Click += (s, e) =>
            {
                GraphicStock.AutoScaling(checkBoxAutoSizePrice.Checked);
                ToolStripMenuItemAutoSize.Checked = checkBoxAutoSizePrice.Checked;
            };
        }

        private void PanelSettings_setTypeClientLimit()
        {
            var limit = SettingsDepth.Data.TypeClientLimit;
            var listPortf = Trader.Objects.Portfolios.Where(p => p.Account.AccClasses.FirstOrDefault(c => c == Securities.Class).NotIsNull() &&
                p.Client.Code == SettingsDepth.Data.CodeClient);
            if (listPortf.Count() > 0)
            {
                comboBoxTypeClientLimit.SetListValues(listPortf.Select(p => p.LimitKind.ToString()).ToArray(), limit.ToString());
            }
            else
            {
                comboBoxTypeClientLimit.Clear();
                SettingsDepth.Data.TypeClientLimit = -1;
                comboBoxTypeClientLimit.SelectedValue = "";
            }
        }
    }
}
