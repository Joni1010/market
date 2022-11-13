using AppVEConector.Components;
using AppVEConector.libs.Signal;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace AppVEConector.Forms
{
    public partial class Form_Arbitration : Form
    {
        struct DataCalc
        {
            public decimal futPrice;
            public decimal sumFutPos;
            public decimal futCurPos;

            public decimal diffPrice;

            public decimal basePrice;
            public decimal basePos;
            public decimal baseCurPos;
            public decimal sumBasePos;
        }
        private MainForm PForm = null;

        private Securities SecFut = null;

        private Securities SecBase = null;

        private DataCalc DataArb = new DataCalc();

        private DateTime LastSignal = new DateTime();
        const int PERIOD_SIGNAL = 1;
        const int COUNT_AVERAGE = 100;

        private decimal[] AverageDiff = new decimal[COUNT_AVERAGE];

        private int CountEvent = 0;


        public Form_Arbitration(MainForm parent)
        {
            InitializeComponent();
            PForm = parent;
            Init();

            PForm.OnTimer1s += UpdateData;
        }
        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            LastSignal = DateTime.Now;
            comboBoxFutSec.TextChanged += (s, e) =>
            {
                var text = comboBoxFutSec.Text;
                if (text.Length >= 2)
                {
                    var listSec = Searcher.StockAsArray(Quik.Trader.Objects.tSecurities.ToArray(), text);
                    if (listSec.Count() > 0)
                    {
                        comboBoxFutSec.Clear();
                        comboBoxFutSec.SetListValues(listSec);
                        UpdateData(null);
                    }
                }
                comboBoxFutSec.SelectionStart = comboBoxFutSec.Text.Length;
                //comboBoxBaseSec.Select(text.Length, 0);
            };
            comboBoxBaseSec.TextChanged += (s, e) =>
            {
                var text = comboBoxBaseSec.Text;
                if (text.Length >= 2)
                {
                    var listSec = Searcher.StockAsArray(Quik.Trader.Objects.tSecurities.ToArray(), text);
                    if (listSec.Count() > 0)
                    {
                        comboBoxBaseSec.Clear();
                        comboBoxBaseSec.SetListValues(listSec);
                        UpdateData(null);
                    }
                }
                comboBoxBaseSec.SelectionStart = comboBoxBaseSec.Text.Length;
                //comboBoxBaseSec.Select(text.Length, 0);
            };
            comboBoxBaseSec.SelectedIndexChanged += (s, e) =>
            {
                SecBase = null;
                if (comboBoxBaseSec.SelectedItem.NotIsNull())
                {
                    SecBase = PForm.GetSecByCode(comboBoxBaseSec.SelectedItem.ToString());
                    if (SecBase.NotIsNull())
                    {
                        Quik.Trader.RegisterSecurities(SecBase);
                    }
                }
                UpdateData(null);
            };
            comboBoxFutSec.SelectedIndexChanged += (s, e) =>
            {
                SecFut = null;
                if (comboBoxFutSec.SelectedItem.NotIsNull())
                {
                    SecFut = PForm.GetSecByCode(comboBoxFutSec.SelectedItem.ToString());
                    if (SecFut.NotIsNull())
                    {
                        Quik.Trader.RegisterSecurities(SecFut);
                    }
                }
                UpdateData(null);
            };

            comboBoxFutAccount.SetListValues(Quik.Trader.Objects.tClients.ToArray().Select(c => c.Code).ToArray());
            comboBoxBaseAccount.SetListValues(Quik.Trader.Objects.tClients.ToArray().Select(c => c.Code).ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timer"></param>
        public void UpdateData(DispatcherTimer timer)
        {
            DataArb = new DataCalc();

            if (SecFut.NotIsNull())
            {
                labelFutLot.Text = SecFut.Lot.ToString();
                labelFutGo.Text = SecFut.Params.SellDepo.ToString();
                labelFutPriceStep.Text = SecFut.StepPrice.ToString();
                labelFutPrice.Text = SecFut.LastPrice.ToString() + " ( " + (SecFut.LastPrice / SecFut.Lot).ToString() + ")";
            }
            else
            {
                labelFutLot.Text = "0";
                labelFutGo.Text = "0.0";
                labelFutPriceStep.Text = "0.0";
                labelFutPrice.Text = "0.0";
            }

            if (SecBase.NotIsNull())
            {
                labelBaseSecLot.Text = SecBase.Lot.ToString();
                labelBaseSecPrice.Text = SecBase.LastPrice.ToString();
            }
            else
            {
                labelBaseSecLot.Text = "0";
                labelBaseSecPrice.Text = "0.0";
            }

            if (SecFut.NotIsNull() && SecBase.NotIsNull())
            {
                DataArb.futPrice = SecFut.LastPrice;
                DataArb.basePrice = SecBase.LastPrice;

                var priceFut = DataArb.futPrice / SecFut.Lot;
                DataArb.diffPrice = priceFut > DataArb.basePrice
                    ? priceFut - DataArb.basePrice
                    : DataArb.basePrice - priceFut;
                DataArb.diffPrice = DataArb.diffPrice * SecFut.Lot;

                DataArb.sumFutPos = SecFut.Params.SellDepo * numericUpDownFutPos.Value;
                DataArb.basePos = Math.Round(numericUpDownFutPos.Value * SecFut.Lot / SecBase.Lot, 1);
                DataArb.sumBasePos = DataArb.basePos * SecBase.Lot * DataArb.basePrice;


                var posFut = Quik.Trader.Objects.tPositions.SearchFirst(p => p.Sec == SecFut);
                if (posFut.NotIsNull())
                {
                    DataArb.futCurPos = posFut.Data.CurrentNet;
                }
                var posBase = Quik.Trader.Objects.tPositions.SearchFirst(p => p.Sec == SecBase && p.Data.Type == 2);
                if (posBase.NotIsNull())
                {
                    DataArb.baseCurPos = posBase.Data.CurrentNet;
                }

                ControlEvent();
            }
            else
            {
                DataArb.diffPrice = 0;
                DataArb.sumFutPos = 0;
                DataArb.basePos = 0;
                DataArb.sumBasePos = 0;
            }
            //fut
            labelDiffPrice.Text = DataArb.diffPrice.ToString();
            labelFutSumPos.Text = DataArb.sumFutPos.ToString();
            labelFutCurrPos.Text = DataArb.futCurPos.ToString();
            //base
            labelBasePos.Text = DataArb.basePos.ToString();
            labelBaseSumPos.Text = DataArb.sumBasePos.ToString();
            labelBaseCurrPos.Text = DataArb.baseCurPos.ToString();
        }

        private void ControlEvent()
        {
            //Контроль расхождения
            if (numericUpDownDiffPriceControll.Value < DataArb.diffPrice
                && DataArb.diffPrice > 0
                && numericUpDownDiffPriceControll.Value > 0
                && numericUpDownFutPos.Value > 0
                && checkBoxEnableControl.Checked
                )
            {
                CountEvent++;
                if (CountEvent >= 3)
                {
                    var now = DateTime.Now;
                    if (LastSignal.AddMinutes(PERIOD_SIGNAL) < now)
                    {
                        LastSignal = now;
                        if (checkBoxEnableTrade.Enabled && DataArb.baseCurPos == 0 && DataArb.futCurPos == 0)
                        {
                            activeTrade();
                        }
                        Form_MessageSignal.Show("Arbitration " + SecFut.ToString() + " <=> " + SecBase.ToString()
                            + " diff: " + DataArb.diffPrice.ToString(), SecFut.ToString(), true);

                    }
                }
            }
            else
            {
                //Если за 3 такта не сохранилось расхождение, то сбрасываем
                CountEvent = 0;
            }
        }
        private void activeTrade()
        {

        }
    }
}
