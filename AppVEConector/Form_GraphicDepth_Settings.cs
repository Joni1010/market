using GraphicTools.Extension;
using libs;
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
        /// <summary>
        /// Все настройки, которые необходимо хранить.
        /// </summary>
        private SettingsFormSec SettingsDepth = null;

        /// <summary> Авто масштабирование графика </summary>
        protected VarSynch<bool> AutoSizeGraphic = new VarSynch<bool>();
        /// <summary>
        /// Код клиента
        /// </summary>
        protected VarSynch<string> ClientCode = new VarSynch<string>();
        /// <summary>
        /// Тип лимита
        /// </summary>
        protected VarSynch<int> TypeClientLimit = new VarSynch<int>();
        /// <summary>
        /// Используемый тайм фрейм (в минутах)
        /// </summary>
        protected VarSynch<int> TimeFrameUse = new VarSynch<int>();

        private void PanelSettings_Init()
        {
            TimeFrameUse.OnSet += (v) =>
            {
                SettingsDepth.Set("CurrentTimeFrame", v);
                GraphicStock.TimeFrame = CurrentTimeFrame;
                CountCandleInGraphic = SettingsDepth.GetTF(v, "CountVisibleCandle");
                numericUpDownMAPeriod.Value = SettingsDepth.GetTF(v, "MovingAverage");
                UpdateGraphic();
            };
            AutoSizeGraphic.OnSet += (v) =>
            {
                GraphicStock.AutoScaling(v);
                ToolStripMenuItemAutoSize.Checked = v;
                checkBoxAutoSizePrice.Checked = v;
            };
            ClientCode.OnSet += (v) =>
            {
                SettingsDepth.Set("CodeClient", v);
                comboBoxCodeClient.Text = v;
                PanelSettings_setTypeClientLimit();
            };
            TypeClientLimit.OnSet += (v) =>
            {
                SettingsDepth.Set("TypeClientLimit", v);
                comboBoxTypeClientLimit.Text = v.ToString();
            };

            ClientCode.Value = SettingsDepth.Get("CodeClient");
            TypeClientLimit.Value = SettingsDepth.Get("TypeClientLimit");
            TimeFrameUse.Value = SettingsDepth.Get("CurrentTimeFrame"); 
            TimeFrameUse.Value = TimeFrameUse.Value <= 0 ? 1 : TimeFrameUse.Value;

            //поле ввода лимита границ объема в дельте
            numericUpDownMinMacsHorVol.Value = SettingsDepth.Get("HVLimitsDeltaVolumes");
            GraphicStock.SetLimitBorderHorVolume((long)numericUpDownMinMacsHorVol.Value);
            numericUpDownMinMacsHorVol.ValueChanged += (s, e) =>
            {
                SettingsDepth.Set("HVLimitsDeltaVolumes", (long)numericUpDownMinMacsHorVol.Value);
                GraphicStock.SetLimitBorderHorVolume((long)numericUpDownMinMacsHorVol.Value);
                UpdateGraphic();
            };

            PanelSettings_InitObjectsControls();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PanelSettings_InitObjectsControls()
        {
            //Получаем коды клиента
            comboBoxCodeClient.SetListValues(Trader.Objects.tClients.ToArray().Select(c => c.Code).ToArray(),
                ClientCode.Value);
            comboBoxCodeClient.SelectedValueChanged += (ss, ee) =>
            {
                if (comboBoxCodeClient.SelectedItem.NotIsNull())
                {
                    ClientCode.Value = comboBoxCodeClient.SelectedItem.ToString();
                }
            };
            comboBoxTypeClientLimit.SelectedValueChanged += (ss, ee) =>
            {
                TypeClientLimit.Value = comboBoxTypeClientLimit.SelectedItem.ToString().ToInt32();
            };
            checkBoxAutoSizePrice.Click += (s, e) =>
            {
                AutoSizeGraphic.Value = checkBoxAutoSizePrice.Checked;
            };
            ToolStripMenuItemAutoSize.Click += (s, e) =>
            {
                AutoSizeGraphic.Value = !ToolStripMenuItemAutoSize.Checked;
            };

            comboBoxTimeFrame.InitDefault();
            comboBoxTimeFrame.DataSource = SelectorTimeFrame.GetAll();
            comboBoxTimeFrame.SelectedIndex = SelectorTimeFrame.GetIndex(TimeFrameUse.Value);
            comboBoxTimeFrame.SelectedIndexChanged += (s, e) =>
            {
                ComboBox sen = (ComboBox)s;
                if (sen.SelectedItem.NotIsNull() && sen.SelectedItem is SelectorTimeFrame)
                {
                    var item = (SelectorTimeFrame)sen.SelectedItem;
                    TimeFrameUse.Value = item.TimeFrame;
                }
            };

            //Кол-во видимых свечей
            CountCandleInGraphic = SettingsDepth.GetTF(TimeFrameUse.Value, "CountVisibleCandle");
        }
        /// <summary>
        /// 
        /// </summary>
        private void PanelSettings_setTypeClientLimit()
        {
            var listPortf = Trader.Objects.tPortfolios
                .SearchAll(p => p.Account.AccClasses.FirstOrDefault(c => c == Securities.Class).NotIsNull() &&
                p.Client.Code == ClientCode.Value);
            if (listPortf.Count() > 0)
            {
                comboBoxTypeClientLimit.Clear();
                comboBoxTypeClientLimit.SetListValues(listPortf.Select(p => p.LimitKind.ToString()).ToArray(),
                    TypeClientLimit.Value.ToString());
            }
            else
            {
                TypeClientLimit.Value = -1;
            }
        }
    }
}
