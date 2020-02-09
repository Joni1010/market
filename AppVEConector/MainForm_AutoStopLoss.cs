using AppVEConector.libs;
using Managers;
using MarketObjects;
using QuikConnector.MarketObjects;
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
        public AutoStopLoss ASLObject = new AutoStopLoss(Global.GetPathData());

        public Securities StopLossSec = null;
        /// <summary>
        /// Время повторений контроля
        /// </summary>
        public DateTime TimeControl = DateTime.Now;
        const int PERIOD_CONTROL_SEC = 30;

        public void InitAutoStopLoss()
        {
            ASLObject.Load();

            comboBoxASLAccount.Click += (s, e) =>
            {
                if (comboBoxASLAccount.Items.Count == 0)
                {
                    comboBoxASLAccount.SetListValues(this.Trader.Objects.Clients.Select(c => c.Code).ToArray());
                }
            };
            comboBoxASLSec.SetListValues(Global.GetWorkingListSec().ToArray());
            comboBoxASLSec.TextChanged += (s, e) =>
            {
                StopLossSec = null;
                var text = comboBoxASLSec.Text;
                if (text.Length >= 2)
                {
                    var listSec = Trader.Objects.Securities.Where(el => el.Code.ToLower().Contains(text.ToLower()) ||
                        el.Name.ToLower().Contains(text.ToLower())).Select(el => el.ToString());
                    if (listSec.Count() > 0)
                    {
                        comboBoxASLSec.Clear();
                        comboBoxASLSec.SetListValues(listSec);
                        AutoSLUpdateGrid();
                    }
                }
                comboBoxASLSec.Select(text.Length, 0);
            };
            comboBoxASLSec.KeyPress += (s, e) =>
            {
                var k = e.KeyChar;
                if (e.KeyChar == 13)
                {
                    if (comboBoxASLSec.Items.Count > 0)
                    {
                        comboBoxASLSec.SelectedItem = comboBoxASLSec.Items[0];
                        AutoSLUpdatePanel();
                        AutoSLUpdateGrid();
                    }
                }
            };
            comboBoxASLSec.SelectedIndexChanged += (s, e) =>
            {
                AutoSLUpdatePanel();
            };

            ASLObject.OnAdd += (condOrder) =>
            {
                AutoSLUpdateGrid();
            };
            ASLObject.OnDelete += (condOrder) =>
            {
                AutoSLUpdateGrid();
            };

            buttonASLDelete.Click += (s, e) =>
            {
                if (dataGridViewASLList.SelectedRows.NotIsNull() && dataGridViewASLList.SelectedRows.Count > 0)
                {
                    foreach (var row in dataGridViewASLList.SelectedRows)
                    {
                        if (row is DataGridViewRow)
                        {
                            var rowEl = (DataGridViewRow)row;
                            if (rowEl.Tag.NotIsNull())
                            {
                                ASLObject.Delete((AutoStopLoss.Item)rowEl.Tag);
                            }
                        }
                    }
                }
                AutoSLUpdateGrid();
            };

            checkBoxASLBySec.CheckedChanged += (s, e) =>
            {
                AutoSLUpdateGrid();
            };

            buttonASLAdd.Click += (s, e) =>
            {
                AddSLControl();
            };

            AutoSLUpdateGrid();
        }

        private void AutoSLUpdateGrid()
        {
            dataGridViewASLList.GuiAsync(() =>
            {
                dataGridViewASLList.Rows.Clear();
                var list = ASLObject.ToArray;
                //ВЫбрать по инструменту
                if (checkBoxASLBySec.Checked)
                {
                    if (StopLossSec.NotIsNull())
                    {
                        list = list.Where(o => o.SecAndCode == StopLossSec.ToString()).ToArray();
                    }
                }
                foreach (var itemControl in list)
                {
                    if (itemControl.NotIsNull())
                    {
                        var newRow = (DataGridViewRow)dataGridViewASLList.Rows[0].Clone();
                        newRow.Cells[0].Value = itemControl.SecAndCode + " " + itemControl.SecName;
                        newRow.Cells[1].Value = itemControl.Tiks.ToString();
                        newRow.Cells[2].Value = itemControl.Comment;
                        newRow.Tag = itemControl;
                        dataGridViewASLList.Rows.Add(newRow);
                    }
                }
            });
        }

        private void AutoSLUpdatePanel()
        {
            if (comboBoxASLSec.SelectedItem.NotIsNull())
            {
                StopLossSec = GetSecCodeAndClass(comboBoxASLSec.SelectedItem.ToString());
                if (StopLossSec.NotIsNull())
                {
                    labelASLInfo.Text =
                        StopLossSec.Code + "\r\n" +
                        StopLossSec.Name + "\r\n" +
                        StopLossSec.Shortname + "\r\n" +
                        "Lot: " + StopLossSec.Lot + "\r\n" +
                        "MinPriceStep: " + StopLossSec.MinPriceStep + "\r\n" +
                        "LastPrice: " + StopLossSec.LastPrice + "\r\n";

                    numericUpDownASLTiks.InitWheelDecimal(0, 1000000, 1, 0);
                }
            }
        }

        private void AddSLControl()
        {
            if (StopLossSec.IsNull())
            {
                AutoSLLog("Инструмент не определен.");
                return;
            }
            if (numericUpDownASLTiks.Value <= 0)
            {
                AutoSLLog("Кол-во пунктов должно быть больше 0.");
                return;
            }
            if (comboBoxASLAccount.SelectedItem.IsNull())
            {
                AutoSLLog("Не выбран счет клиента.");
                return;
            }
            var itemExists = ASLObject.GetItem((i) => { return i.SecAndCode == StopLossSec.ToString(); });
            if (itemExists.NotIsNull())
            {
                AutoSLLog("Стоп инструкция уже существует.");
                return;
            }
            var newControl = new AutoStopLoss.Item()
            {
                SecAndCode = StopLossSec.ToString(),
                SecName = StopLossSec.Name,
                Comment = comboBoxASLAccount.SelectedItem.ToString(),
                Tiks = numericUpDownASLTiks.Value
            };

            ASLObject.Add(newControl);
            AutoSLUpdateGrid();
        }

        private void AutoSLLog(string text)
        {
            labelASLLog.GuiAsync(() =>
            {
                labelASLLog.Text = DateTime.Now.ToString() + ": " + text + "\r\n" + labelASLLog.Text;
            });
        }

        public void AutoSLLoopControl()
        {
            if (TimeControl.AddSeconds(PERIOD_CONTROL_SEC) > DateTime.Now)
            {
                return;
            }
            TimeControl = DateTime.Now;
            MThread.InitThread(() =>
            {
                foreach (var objControl in ASLObject.ToArray)
                {
                    var sec = GetSecCodeAndClass(objControl.SecAndCode);
                    if (sec.NotIsNull())
                    {
                        var pos = Trader.Objects.Positions.FirstOrDefault(p => p.Sec == sec);
                        if (pos.NotIsNull())
                        {
                            var stopLoss = Trader.Objects.StopOrders.Where(o => o.Sec == sec
                               && o.IsActive() && o.Comment.Contains(Define.STOP_LOSS)
                            );
                            var volume = pos.CurrentVolume;
                            //Проверяем, не появились ли лишние стоп ордера
                            if (stopLoss.Count() > 1)
                            {
                                Trader.CancelAllStopOrder(sec);
                                continue;
                            }
                            //Проверяем не изменилась ли позиция
                            else if (stopLoss.Count() == 1)
                            {
                                var activeStop = stopLoss.ElementAt(0);
                                //Снимаем стоп если разные обьемы
                                if (activeStop.Volume != volume)
                                {
                                    Trader.CancelAllStopOrder(sec);
                                    continue;
                                }
                                //Снимаем стоп если он в другую сторону
                                if (activeStop.IsBuy() && pos.IsBuy())
                                {
                                    Trader.CancelAllStopOrder(sec);
                                    continue;
                                }
                            }
                            else if (stopLoss.Count() == 0 && pos.CurrentVolume > 0)
                            {
                                var allTrades = Trader.Objects.MyTrades.Where(t => t.Trade.Sec == sec).OrderByDescending(o => o.Trade.Number);
                                if (allTrades.NotIsNull())
                                {
                                    decimal Price = 0;
                                    if (allTrades.Count() > 0)
                                    {
                                        var lastTrade = allTrades.FirstOrDefault(o => o.Trade.Direction == pos.CurrentDirection);
                                        if (lastTrade.NotIsNull())
                                        {
                                            Price = lastTrade.Trade.Price;
                                        }
                                    }
                                    if (Price == 0)
                                    {
                                        Price = sec.LastPrice;
                                    }
                                    var tiks = objControl.Tiks;
                                    var stopOrder = new StopOrder()
                                    {
                                        Sec = sec,
                                        Price = Price,
                                        ClientCode = objControl.Comment,
                                        Comment = Define.STOP_LOSS,
                                        Volume = volume,
                                        DateExpiry = DateMarket.ExtractDateTime(DateTime.Now.AddDays(10))
                                    };
                                    if (pos.IsBuy())
                                    {
                                        stopOrder.Direction = OrderDirection.Sell;
                                        stopOrder.ConditionPrice = Price - sec.MinPriceStep * tiks;
                                        stopOrder.Price = stopOrder.ConditionPrice - sec.MinPriceStep * 10;
                                    }
                                    else if (pos.IsSell())
                                    {
                                        stopOrder.Direction = OrderDirection.Buy;
                                        stopOrder.ConditionPrice = Price + sec.MinPriceStep * tiks;
                                        stopOrder.Price = stopOrder.ConditionPrice + sec.MinPriceStep * 10;
                                    }
                                    Trader.CreateStopOrder(stopOrder, StopOrderType.StopLimit);
                                    AutoSLLog("Create order: " + sec.ToString() + " " +
                                        (stopOrder.IsBuy() ? "B" : "S") +
                                        " " + stopOrder.ConditionPrice);
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
