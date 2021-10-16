using AppVEConector.libs;
using AppVEConector.libs.Signal;
using Managers;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppVEConector
{
    public partial class MainForm : Form
    {
        public AutoOrders ObjAutoOrders = new AutoOrders(Global.GetPathData());

        public Securities AutoOrderSec = null;

        public void InitAutoOrders()
        {
            ObjAutoOrders.Load();

            comboBoxAOAccount.Click += (s, e) =>
            {
                if (comboBoxAOAccount.Items.Count == 0)
                {
                    comboBoxAOAccount.SetListValues(this.Trader.Objects.tClients.ToArray()
                        .Select(c => c.Code).ToArray());
                }
            };
            comboBoxAOSecurities.SetListValues(Global.GetWorkingListSec().ToArray());
            comboBoxAOSecurities.TextChanged += (s, e) =>
            {
                AutoOrderSec = null;
                var text = comboBoxAOSecurities.Text;
                if (text.Length >= 2)
                {
                    var listSec = Trader.Objects.tSecurities.SearchAll(el => el.Code.ToLower().Contains(text.ToLower()) ||
                        el.Name.ToLower().Contains(text.ToLower())).Select(el => el.ToString());
                    if (listSec.Count() > 0)
                    {
                        comboBoxAOSecurities.Clear();
                        comboBoxAOSecurities.SetListValues(listSec);
                        AutoOrdersUpdateGrid();
                    }
                }
                comboBoxAOSecurities.Select(text.Length, 0);
            };
            comboBoxAOSecurities.KeyPress += (s, e) =>
            {
                var k = e.KeyChar;
                if (e.KeyChar == 13)
                {
                    if (comboBoxAOSecurities.Items.Count > 0)
                    {
                        comboBoxAOSecurities.SelectedItem = comboBoxAOSecurities.Items[0];
                        AutoOrdersUpdatePanelOrder();
                        AutoOrdersUpdateGrid();
                    }
                }
            };
            comboBoxAOSecurities.SelectedIndexChanged += (s, e) =>
            {
                AutoOrdersUpdatePanelOrder();
            };


            buttonAOBuy.Click += (s, e) =>
            {
                AddAutoOrder(true);
            };
            buttonAOSell.Click += (s, e) =>
            {
                AddAutoOrder(false);
            };

            ObjAutoOrders.OnAdd += (condOrder) =>
            {
                AutoOrdersUpdateGrid();
            };
            ObjAutoOrders.OnDelete += (condOrder) =>
            {
                AutoOrdersUpdateGrid();
            };

            buttonAODelete.Click += (s, e) =>
            {
                if (dataGridViewAOList.SelectedRows.NotIsNull() && dataGridViewAOList.SelectedRows.Count > 0)
                {
                    foreach (var row in dataGridViewAOList.SelectedRows)
                    {
                        if (row is DataGridViewRow)
                        {
                            var rowEl = (DataGridViewRow)row;
                            if (rowEl.Tag.NotIsNull())
                            {
                                ObjAutoOrders.Delete((AutoOrders.ConditionOrder)rowEl.Tag);
                            }
                        }
                    }
                }
                AutoOrdersUpdateGrid();
            };

            buttonAODeleteSec.Click += (s, e) =>
            {
                if (AutoOrderSec.NotIsNull())
                {
                    ObjAutoOrders.Delete(AutoOrderSec);
                    AutoOrdersUpdateGrid();
                }
            };
            checkBoxAOSelectSec.CheckedChanged += (s, e) =>
            {
                AutoOrdersUpdateGrid();
            };

            AutoOrdersUpdateGrid();
        }

        private void AutoOrdersUpdateGrid()
        {
            dataGridViewAOList.GuiAsync(() =>
            {
                dataGridViewAOList.Rows.Clear();
                var list = ObjAutoOrders.ToArray;
                //ВЫбрать по инструменту
                if (checkBoxAOSelectSec.Checked)
                {
                    if (AutoOrderSec.NotIsNull())
                    {
                        list = list.Where(o => o.SecAndCode == AutoOrderSec.ToString()).ToArray();
                    }
                }
                foreach (var ord in list)
                {
                    if (ord.NotIsNull())
                    {
                        var newRow = (DataGridViewRow)dataGridViewAOList.Rows[0].Clone();
                        newRow.Cells[0].Value = ord.SecAndCode + " " + ord.SecName;
                        newRow.Cells[1].Value = ord.PriceCondition.ToString() + " (" + ord.Volume.ToString() + ")"; ;
                        newRow.Cells[2].Value = ord.IsBuy() ? "BUY" : "SELL";
                        newRow.Cells[3].Value = ord.Comment;
                        newRow.Tag = ord;
                        dataGridViewAOList.Rows.Add(newRow);
                    }
                }
            });
        }

        private void AutoOrdersUpdatePanelOrder()
        {
            if (comboBoxAOSecurities.SelectedItem.NotIsNull())
            {
                AutoOrderSec = GetSecCodeAndClass(comboBoxAOSecurities.SelectedItem.ToString());
                if (AutoOrderSec.NotIsNull())
                {
                    labelAOInfo.Text =
                        AutoOrderSec.Code + "\r\n" +
                        AutoOrderSec.Name + "\r\n" +
                        AutoOrderSec.Shortname + "\r\n" +
                        "Lot: " + AutoOrderSec.Lot + "\r\n" +
                        "MinPriceStep: " + AutoOrderSec.MinPriceStep + "\r\n" +
                        "LastPrice: " + AutoOrderSec.LastPrice + "\r\n";

                    numericUpDownAOPrice.InitSecurity(AutoOrderSec, 0);
                    numericUpDownAOVolume.InitWheelDecimal(0, 1000000, 1, 0);
                }
            }
        }

        private void AddAutoOrder(bool isBuy)
        {
            if (AutoOrderSec.IsNull())
            {
                AutoOrdersLog("Инструмент не определен.");
                return;
            }
            if (AutoOrderSec.LastPrice <= 0)
            {
                AutoOrdersLog("Некущая цена должна быть больше 0.");
                return;
            }
            if (numericUpDownAOPrice.Value <= 0)
            {
                AutoOrdersLog("Не корректная цена условия.");
                return;
            }
            if (numericUpDownAOVolume.Value <= 0)
            {
                AutoOrdersLog("Обьем долженбыть больше 0.");
                return;
            }
            if (comboBoxAOAccount.SelectedItem.IsNull())
            {
                AutoOrdersLog("Не выбран счет клиента.");
                return;
            }
            var getOrder = new AutoOrders.ConditionOrder()
            {
                PriceCondition = numericUpDownAOPrice.Value,
                SecAndCode = AutoOrderSec.ToString(),
                SecName = AutoOrderSec.Name,
                Comment = comboBoxAOAccount.SelectedItem.ToString(),
                Price = numericUpDownAOPrice.Value,
                Volume = (int)numericUpDownAOVolume.Value,
            };
            if (isBuy)
            {
                getOrder.Direction = OrderDirection.Buy;
                if (AutoOrderSec.LastPrice < getOrder.PriceCondition)
                {
                    getOrder.CondAutoOrder = AutoOrders.CondAutoOrder.MoreOrEquals;
                }
                else
                {
                    getOrder.CondAutoOrder = AutoOrders.CondAutoOrder.LessOrEquals;
                }
            }
            else
            {
                getOrder.Direction = OrderDirection.Sell;
                if (AutoOrderSec.LastPrice > getOrder.PriceCondition)
                {
                    getOrder.CondAutoOrder = AutoOrders.CondAutoOrder.LessOrEquals;
                }
                else
                {
                    getOrder.CondAutoOrder = AutoOrders.CondAutoOrder.MoreOrEquals;
                }
            }
            ObjAutoOrders.Add(getOrder);
            AutoOrdersUpdateGrid();
        }

        private void AutoOrdersLog(string text)
        {
            labelAOLog.Text = DateTime.Now.ToString() + ": " + text + "\r\n" + labelAOLog.Text;
        }

        public void AutoOrdersLoopControl()
        {
            MThread.InitThread(() =>
            {
                foreach (var ord in ObjAutoOrders.ToArray)
                {
                    var sec = GetSecCodeAndClass(ord.SecAndCode);
                    if (sec.NotIsNull())
                    {
                        Order order = null;
                        if (ord.CondAutoOrder == AutoOrders.CondAutoOrder.MoreOrEquals)
                        {
                            if (sec.LastPrice >= ord.PriceCondition)
                            {
                                order = new Order()
                                {
                                    Sec = sec,
                                    Direction = ord.Direction,
                                    Price = ord.Price,
                                    Volume = (int)ord.Volume,
                                    ClientCode = ord.Comment
                                };
                            }
                        }
                        else if (ord.CondAutoOrder == AutoOrders.CondAutoOrder.LessOrEquals)
                        {
                            if (sec.LastPrice <= ord.PriceCondition)
                            {
                                order = new Order()
                                {
                                    Sec = sec,
                                    Direction = ord.Direction,
                                    Price = ord.Price,
                                    Volume = (int)ord.Volume,
                                    ClientCode = ord.Comment
                                };
                            }
                        }
                        if (order.NotIsNull())
                        {
                            ObjAutoOrders.Delete(ord);

                            Trader.CreateOrder(order);
                            SignalView.GSMSignaler.SendSignalCall();

                            textBoxLogSign.GuiAsync(() =>
                            {
                                textBoxLogSign.Text = DateTime.Now.ToString() + " AutoOrder: " + order.Sec.Shortname + "(" + order.Sec.Code + ")" +
                                ", Price: " + order.Price + " (" + order.Volume + ")" +
                                ", " + (order.IsBuy() ? "BUY" : "SELL") +
                                ", " + order.ClientCode +
                                "\r\n" + textBoxLogSign.Text;
                            });
                        }
                    }
                }
            });
        }
    }
}
