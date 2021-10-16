using AppVEConector.libs;
using AppVEConector.libs.Signal;
using Connector.Logs;
using Managers;
using Market.AppTools;
using MarketObjects;
using QuikConnector.MarketObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

namespace AppVEConector.Forms.StopOrders
{
    /// <summary>
    /// Базовый класс для составления форм Стоп-ордеров
    /// </summary>
    public class ControlForFormStopOrders : Form
    {
        public new MainForm Parent = null;

        protected Action eventInitPanel = null;
        protected Action eventTimer1s = null;

        public class TrackOrder
        {
            /// <summary>
            /// Период
            /// </summary>
            public int Period = 0;
            /// <summary>
            /// Кол-во тиков прибавляемые/отнимаемые
            /// </summary>
            public int Tiks = 0;
            /// <summary>
            /// Время последнего трека отсечки
            /// </summary>
            public DateTime LastTrack = DateTime.Now;
        }

        public class PanelControl
        {
            public TElement TrElement = null;
            public Position Position = null;
            public int Index = -1;
            public int Volume = 1;

            public ComboBox ComboboxSecurity = null;
            public Label LabelPosition = null;
            public Label LabelStopValue = null;
            public Label LabelOrderBuySell = null;
            public NumericUpDown PriceStop = null;

            public Button ButtonStop = null;
            public Button ButtonCancelOrder = null;
            public Button ButtonCancelStop = null;

            public Button ButtonSettings = null;

            public TableLayoutPanel LayoutPanel = null;

            public TrackOrder TrackOrder = null;

            public DateTime LastSendStopOrder;
        }

        /// <summary> Список всех панелей  </summary>
        protected List<PanelControl> ListPanels = new List<PanelControl>();

        /// <summary>
        /// Заполняем структуру панелей
        /// </summary>
        /// <param name="control"></param>
        protected void searchAllElementsPanels(object control)
        {
            if (control.IsNull()) return;
            var obj = (Control)control;
            if (obj.Controls.Count > 0)
            {
                foreach (var c in obj.Controls)
                {
                    searchAllElementsPanels(c);
                }
            }
            if (obj.Tag.NotIsNull())
            {
                string strTag = obj.Tag.ToString();
                Regex rgx = new Regex(@"[a-zA-Z]*");
                int index = rgx.Replace(strTag, "").ToInt32();
                var el = ListPanels.FirstOrDefault(p => p.Index == index);
                if (el.IsNull())
                {
                    el = new PanelControl();
                    ListPanels.Add(el);
                    el.Index = index;
                }
                if (control is ComboBox)
                {
                    if (strTag.Contains("SEC"))
                    {
                        el.ComboboxSecurity = (ComboBox)control;
                        el.ComboboxSecurity.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                }
                if (control is Label)
                {
                    if (strTag.Contains("ORBS")) el.LabelOrderBuySell = (Label)control;
                    if (strTag.Contains("POS")) el.LabelPosition = (Label)control;
                    if (strTag.Contains("LS")) el.LabelStopValue = (Label)control;
                }
                if (control is Button)
                {
                    var b = (Button)control;
                    if (strTag.Contains("BSL")) el.ButtonStop = b;
                    if (strTag.Contains("CanOr")) el.ButtonCancelOrder = b;
                    if (strTag.Contains("CanSt")) el.ButtonCancelStop = b;

                    if (strTag.Contains("BSET")) el.ButtonSettings = b;
                }
                if (control is TableLayoutPanel)
                {
                    if (strTag.Contains("LAY")) el.LayoutPanel = (TableLayoutPanel)control;
                }

                if (control is NumericUpDown)
                {
                    if (strTag.Contains("SP"))
                    {
                        el.PriceStop = (NumericUpDown)control;
                        el.PriceStop.InitWheelDecimal();
                    }
                }
            }
        }
        protected void InitPanels()
        {
            if (ListPanels.Count == 0)
            {
                return;
            }

            foreach (var pan in ListPanels)
            {
                InitOnePanel(pan);
            }

            Action<IEnumerable<Reply>> actionReply = (listReply) =>
            {
                var reply = listReply.Last();
                SetMessage(reply.ResultMsg);
            };

            Parent.OnTimer1s += UpdaterDataFirst;
            Parent.OnTimer3s += UpdaterDataSecond;

            Parent.OnNewReply += actionReply;

            FormClosed += (s, e) =>
            {
                Parent.OnTimer1s -= UpdaterDataFirst;
                Parent.OnTimer3s -= UpdaterDataSecond;
                Parent.OnNewReply -= actionReply;
            };

            if (eventInitPanel.NotIsNull())
            {
                eventInitPanel();
            }
            Parent.Trader.Objects.tStopOrders.OnNew += (stopOrder) =>
            {
                foreach (var sOrd in stopOrder)
                {
                    foreach (var pan in ListPanels)
                    {
                        if (pan.TrElement.NotIsNull() && pan.TrElement.Security == sOrd.Sec)
                        {
                            pan.PriceStop.GuiAsync(() =>
                            {
                                pan.PriceStop.Value = sOrd.ConditionPrice;
                            });
                            break;
                        }
                    }
                }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        private decimal? GetPriceLastStopOrder(Securities sec)
        {
            var stopOrders = Parent.Trader.Objects.tStopOrders.SearchFirst(so => so.IsActive()
                && so.Sec == sec && so.Comment.Contains(Define.STOP_LOSS));
            if (stopOrders.NotIsNull())
            {
                return stopOrders.ConditionPrice;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panel"></param>
        private void InitOnePanel(PanelControl panel)
        {
            if (panel.ComboboxSecurity.NotIsNull())
            {
                panel.ComboboxSecurity.TextChanged += (s, e) =>
                {
                    Qlog.CatchException(() =>
                    {
                        UpdateDataPanel(panel);
                    });
                };
                panel.ComboboxSecurity.SelectedValueChanged += (s, e) =>
                {
                    Qlog.CatchException(() =>
                    {
                        var thisComBox = (ComboBox)s;
                        var doubleText = thisComBox.Text;
                        if (doubleText != " ")
                        {
                            var panelsDouble = ListPanels.Where(p => p.ComboboxSecurity.Text == doubleText);
                            if (panelsDouble.Count() > 1)
                            {
                                thisComBox.SelectedIndex = 0;
                                MessageBox.Show("Данный инструмент уже используется в ячейке.");
                            }
                        }
                    });
                };
                panel.ComboboxSecurity.MouseMove += (s, e) =>
                {
                    Qlog.CatchException(() =>
                    {
                        var thisComBox = (ComboBox)s;
                        var sec = Parent.Trader.Objects.tSecurities.SearchFirst(ss => ss.ToString() == thisComBox.Text);
                        if (sec.NotIsNull())
                        {
                            SetMessage(
                                "Name: " + sec.Shortname + "\r\n" +
                                "Expire: " + sec.Params.MatDate + "\r\n" +
                                "Lot: " + sec.Lot + "\r\n" +
                                "StepPrice: " + sec.StepPrice + "\r\n"
                                );
                        }
                    });
                };
                panel.PriceStop.MouseDoubleClick += (s, e) =>
                {
                    Qlog.CatchException(() =>
                    {
                        if (panel.TrElement.IsNull()) return;
                        if (panel.PriceStop.IsMouseOnTextBox(e))
                        {
                            var price = GetPriceLastStopOrder(panel.TrElement.Security);
                            if (price.NotIsNull())
                            {
                                panel.PriceStop.Value = price.Value;
                            }
                            else
                            {
                                panel.PriceStop.Value = panel.TrElement.Security.LastPrice > 0 ? panel.TrElement.Security.LastPrice : 0;
                            }
                        }
                    });
                };

                panel.ButtonCancelOrder.MouseUp += (s, e) =>
                {
                    Qlog.CatchException(() =>
                    {
                        if (panel.TrElement.IsNull()) return;
                        MouseEventArgs ev = (MouseEventArgs)e;
                        if (ev.Button == MouseButtons.Left)
                        {
                            CloseAllOrders(panel);
                        }
                        if (ev.Button == MouseButtons.Right)
                        {
                            CloseAllOrders(panel, true);
                        }
                    });
                };
                panel.ButtonCancelStop.MouseUp += (s, e) =>
                {
                    Qlog.CatchException(() =>
                    {
                        if (panel.TrElement.IsNull()) return;
                        MouseEventArgs ev = (MouseEventArgs)e;
                        if (ev.Button == MouseButtons.Left)
                        {
                            CloseAllStopOrders(panel);
                        }
                        if (ev.Button == MouseButtons.Right)
                        {
                            CloseAllStopOrders(panel, true);
                        }
                    });
                };

                panel.ButtonStop.MouseUp += (s, e) =>
                {
                    Qlog.CatchException(() =>
                    {
                        if (panel.TrElement.IsNull()) return;

                        MouseEventArgs ev = (MouseEventArgs)e;
                        if (ev.Button == MouseButtons.Left)
                        {
                            SendStopOrder(panel, false);
                        }
                        if (ev.Button == MouseButtons.Right)
                        {
                            SendStopOrder(panel, false, true);
                        }
                    });
                };

                panel.LabelPosition.MouseUp += (s, e) =>
                {
                    Qlog.CatchException(() =>
                    {
                        if (panel.TrElement.IsNull()) return;
                        MouseEventArgs ev = (MouseEventArgs)e;
                        if (ev.Button == MouseButtons.Left)
                        {
                            ClosePos(panel);
                        }
                        if (ev.Button == MouseButtons.Right)
                        {
                            Parent.ShowGraphicDepth(panel.TrElement.Security);
                        }
                    });
                };

                panel.ButtonSettings.Click += (s, e) =>
                {
                    Form_SettingsStop formSettings = new Form_SettingsStop(this, panel);
                    formSettings.ShowDialog();
                };
            }
        }

        /*private void LoadListTradeSec(ComboBox comboSec)
        {
            Qlog.CatchException(() =>
            {
                var listWorkSec = Global.GetWorkingListSec();
                var list = listWorkSec.ToList();
                list.Insert(0, " ");
                ComboBox.ObjectCollection items = new ComboBox.ObjectCollection(comboSec);
                items.AddRange(list.ToArray());
                comboSec.DataSource = items;
                if (listWorkSec.Count() > 0)
                {
                    comboSec.SelectedIndex = 0;
                }
            });
        }*/

        /// <summary> Обновляет данные в текущей панели </summary>
        /// <param name="panel"></param>
        private void UpdateDataPanel(PanelControl panel)
        {
            if (panel.ComboboxSecurity.SelectedItem.IsNull())
            {
                return;
            }
            string strSec = panel.ComboboxSecurity.SelectedItem.ToString();
            var sec = Parent.GetSecCodeAndClass(strSec);
            if (sec.NotIsNull())
            {
                panel.TrElement = Parent.DataTrading.Collection.FirstOrDefault(t => t.Security == sec);
                if (panel.TrElement.NotIsNull())
                {
                    panel.Position = Parent.Trader.Objects.tPositions.SearchFirst(p => p.Sec == panel.TrElement.Security);
                    Parent.Trader.RegisterSecurities(sec);
                    panel.PriceStop.InitSecurity(sec);

                    //Установка последней стоп цены
                    panel.PriceStop.Value = 0;
                    var stopOrders = Parent.Trader.Objects.tStopOrders.SearchAll(o => o.Sec == panel.TrElement.Security);
                    if (stopOrders.NotIsNull())
                    {
                        if (stopOrders.Count() > 0)
                        {
                            var stopOrderlast = stopOrders.Last();
                            if (stopOrderlast.NotIsNull())
                            {
                                panel.PriceStop.Value = stopOrderlast.ConditionPrice;
                            }
                        }
                    }
                }
            }
            else
            {
                panel.TrElement = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panel"></param>
        private void UpdateTrackOrders(PanelControl panel)
        {
            if (panel.TrElement.IsNull() || panel.TrackOrder.IsNull())
            {
                return;
            }
            if (panel.TrackOrder.Period > 0 && panel.TrackOrder.Tiks > 0)
            {
                if (panel.TrackOrder.LastTrack < DateTime.Now.AddMinutes(panel.TrackOrder.Period * -1))
                {
                    var addValue = panel.TrackOrder.Tiks * panel.TrElement.Security.MinPriceStep;
                    if (panel.Position.IsBuy())
                    {
                        panel.PriceStop.Value += addValue;
                    }
                    else if (panel.Position.IsSell())
                    {
                        panel.PriceStop.Value -= addValue;
                    }
                    SendStopOrder(panel);
                    panel.TrackOrder.LastTrack = DateTime.Now;
                }
            }
        }

        protected void SendStopOrder(PanelControl panel, bool autoStop = false, bool showInfo = false)
        {
            if (autoStop)
            {
                if (panel.LastSendStopOrder > DateTime.Now.AddSeconds(-5))
                {
                    //Для избежания многократного выставления
                    return;
                }
            }
            var pos = Parent.Trader.Objects.tPositions.SearchFirst(p => p.Sec == panel.TrElement.Security);
            decimal stepStop = panel.TrElement.Security.Params.MinPriceStep * 20;
            int volume = pos.NotIsNull() ? pos.Data.CurrentNet : 0;
            if (volume < 0)
            {
                stepStop = stepStop * -1;
                volume = volume * -1;
            }
            //Просмотр инфо по будущему стопу
            if (showInfo)
            {
                string text = "Инфо создаваемой стоп заявки:\n";
                text += "Позиций: " + pos.Data.CurrentNet + "\n\n";
                text += "Стоп цена: " + panel.PriceStop.Value + "\n";
                text += "Цена исполнения: " + (panel.PriceStop.Value - stepStop) + "\n";
                text += "Объем: " + volume + "\n";
                text += "Дата завершения: " + DateTime.Now.AddDays(1).ToShortDateString() + "\n";
                MessageBox.Show(text);
                return;
            }

            if (panel.PriceStop.Value == 0)
            {
                SetMessage("Некорректная цена стоп-ордера!");
                return;
            }
            if (volume == 0)
            {
                SetMessage("Не текущих позиций!");
                return;
            }
            var stopOrder = new StopOrder()
            {
                Sec = panel.TrElement.Security,
                Price = panel.PriceStop.Value - stepStop,
                Volume = volume,
                Direction = volume > 0 ? OrderDirection.Sell : OrderDirection.Buy,
                ConditionPrice = panel.PriceStop.Value,
                DateExpiry = DateMarket.ExtractDateTime(DateTime.Now.AddDays(1)),
                Comment = Define.STOP_LOSS
            };
            var lastPrice = panel.TrElement.Security.LastPrice;
            if (lastPrice > 0)
            {
                if (pos.IsBuy() && panel.PriceStop.Value > 0)
                {
                    stopOrder.Direction = OrderDirection.Sell;
                    if (panel.PriceStop.Value > lastPrice)
                    {
                        SetMessage("Не корректная цена стоп заявки! Необходимо указать цену ниже текущей.");
                        return;
                    }
                }
                if (pos.IsSell() && panel.PriceStop.Value > 0)
                {
                    stopOrder.Direction = OrderDirection.Buy;
                    if (panel.PriceStop.Value < lastPrice)
                    {
                        SetMessage("Не корректная цена стоп заявки! Необходимо указать цену выше текущей.");
                        return;
                    }
                }
            }

            MThread.InitThread(ThreadPriority.Normal, () =>
            {
                Qlog.CatchException(() =>
                {
                    Thread.Sleep(1000);
                    var countClosed = CancelAllStopOrders(panel);
                    Thread.Sleep(1000);
                    Parent.Trader.CreateStopOrder(stopOrder, StopOrderType.StopLimit);
                    addSignalByStop(stopOrder);
                });
            });
            panel.LastSendStopOrder = DateTime.Now;
        }

        /// <summary>
        /// Сигнал по стопу
        /// </summary>
        /// <param name="stopOrder"></param>
        private void addSignalByStop(StopOrder stopOrder)
        {
            if (stopOrder.IsNull()) return;
            SignalMarket.CondSignal cond = SignalMarket.CondSignal.MoreOrEquals;
            if (stopOrder.Sec.LastPrice > stopOrder.ConditionPrice) cond = SignalMarket.CondSignal.LessOrEquals;
            var newSign = new SignalMarket()
            {
                Type = SignalMarket.TypeSignal.ByPrice,
                SecClass = stopOrder.Sec.ToString(),
                Price = stopOrder.ConditionPrice,
                Condition = cond,
                Comment = stopOrder.Comment
            };
            SignalView.GSMSignaler.AddSignal(newSign);
        }

        /// <summary>
        /// Закрытие всех ордеров или просмотр инфо по заявкам
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="showInfo"></param>
        private void CloseAllOrders(PanelControl panel, bool showInfo = false)
        {
            if (showInfo)
            {
                var pos = Parent.Trader.Objects.tPositions.SearchFirst(p => p.Sec == panel.TrElement.Security);
                var orders = Parent.Trader.Objects.tOrders.SearchAll(p => p.Sec == panel.TrElement.Security && p.IsActive());
                //panel.TrElement.Security.Orders.Where(p => p.Sec == panel.TrElement.Security && p.Status == OrderStatus.ACTIVE);
                //Parent.Trader.Objects.Orders.Where(p => p.Sec == panel.TrElement.Security && p.Status == OrderStatus.ACTIVE);
                var stop_orders = Parent.Trader.Objects.tStopOrders.SearchAll(p => p.Sec == panel.TrElement.Security && p.IsActive());

                string text = "Инфо по " + panel.TrElement.Security + ": \n";
                text += "Позиций: " + (pos.NotIsNull() ? pos.Data.CurrentNet : 0) + "\n";
                text += "Заявок BUY: " + (orders.Where(o => o.IsBuy()).Count())
                    + " (" + (orders.Where(o => o.IsBuy()).Sum(o => o.Balance)) + ")\n";
                text += "Заявок SELL: " + (orders.Where(o => o.IsSell()).Count())
                    + " (" + (orders.Where(o => o.IsSell()).Sum(o => o.Balance)) + ")\n";
                text += "Стоп заявок BUY: " + (stop_orders.Where(o => o.IsBuy()).Count())
                    + " (" + (stop_orders.Where(o => o.IsBuy()).Sum(o => o.Volume)) + ")\n";
                text += "Стоп заявок SELL: " + (stop_orders.Where(o => o.IsSell()).Count())
                    + " (" + (stop_orders.Where(o => o.IsSell()).Sum(o => o.Volume)) + ")\n";

                MessageBox.Show(text);
                return;
            }
            else
            {
                Parent.Trader.CancelAllOrder(panel.TrElement.Security);
            }
        }

        /// <summary>  Закрытие всех ордеров или просмотр инфо по стоп-заявкам </summary>
        /// <param name="panel"></param>
        /// <param name="showInfo"></param>
        protected void CloseAllStopOrders(PanelControl panel, bool showInfo = false)
        {
            if (showInfo)
            {
                var pos = Parent.Trader.Objects.tPositions.SearchFirst(p => p.Sec == panel.TrElement.Security);
                var orders = Parent.Trader.Objects.tOrders.SearchAll(p => p.Sec == panel.TrElement.Security && p.IsActive());
                var stop_orders = Parent.Trader.Objects.tStopOrders.SearchAll(p => p.Sec == panel.TrElement.Security && p.IsActive());

                string text = "Инфо по " + panel.TrElement.Security + ": \n";
                text += "Позиций: " + (pos.NotIsNull() ? pos.Data.CurrentNet : 0) + "\n";
                text += "Заявок BUY: " + (orders.Where(o => o.IsBuy()).Count())
                    + " (" + (orders.Where(o => o.IsBuy()).Sum(o => o.Balance)) + ")\n";
                text += "Заявок SELL: " + (orders.Where(o => o.IsSell()).Count())
                    + " (" + (orders.Where(o => o.IsSell()).Sum(o => o.Balance)) + ")\n";
                text += "Стоп заявок BUY: " + (stop_orders.Where(o => o.IsBuy()).Count())
                    + " (" + (stop_orders.Where(o => o.IsBuy()).Sum(o => o.Volume)) + ")\n";
                text += "Стоп заявок SELL: " + (stop_orders.Where(o => o.IsSell()).Count())
                    + " (" + (stop_orders.Where(o => o.IsSell()).Sum(o => o.Volume)) + ")\n";

                MessageBox.Show(text);
                return;
            }
            else
            {
                CancelAllStopOrders(panel);
            }
        }

        /// <summary>
        /// Выход из позиции (закрытие позиции)
        /// </summary>
        /// <param name="panel"></param>
        protected void ClosePos(PanelControl panel)
        {
            var pos = Parent.Trader.Objects.tPositions.SearchFirst(p => p.Sec == panel.TrElement.Security);
            if (pos.NotIsNull())
            {
                var realPos = pos.Data.CurrentNet;
                var countPos = pos.CurrentVolume;
                if (countPos > 0)
                {
                    var result = MessageBox.Show(this, "Закрыть позицию " + panel.TrElement.Security + " ?",
                        "Закрыть позицию " + panel.TrElement.Security + " ?",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        var lastPrice = panel.TrElement.Security.LastPrice > 0 ? panel.TrElement.Security.LastPrice : 0;
                        lastPrice = lastPrice == 0 ? panel.TrElement.Security.Params.LastPrice : lastPrice;
                        if (lastPrice > 0)
                        {
                            var order = new Order();
                            order.Sec = panel.TrElement.Security;
                            if (pos.IsBuy())
                            {
                                order.Price = lastPrice - panel.TrElement.Security.Params.MinPriceStep * 5;
                                order.Direction = OrderDirection.Sell;
                            }
                            else if (pos.IsSell())
                            {
                                order.Price = lastPrice + panel.TrElement.Security.Params.MinPriceStep * 5;
                                order.Direction = OrderDirection.Buy;
                            }
                            order.Volume = countPos;
                            Parent.Trader.CreateOrder(order);
                        }
                        else
                        {
                            SetMessage("Не удалось определить последнюю цену.");
                        }
                    }
                }
            }
        }

        /// <summary> Переодически обновитель данных </summary>
        public void UpdaterDataFirst(DispatcherTimer timer)
        {
            if (ListPanels.Count == 0 || Parent.CheckKliring())
            {
                return;
            }
            var stopOrders = Parent.Trader.Objects.tStopOrders.SearchAll(so =>
                so.IsActive() && so.Comment.Contains(Define.STOP_LOSS));
            foreach (var pan in ListPanels)
            {
                pan.LabelPosition.Text = "0";
                if (pan.TrElement.NotIsNull() && pan.Position.NotIsNull())
                {
                    decimal StopLossValue = 0;
                    pan.LabelPosition.Text = pan.Position.Data.CurrentNet.ToString();
                    var countPos = pan.Position.CurrentVolume;
                    if (pan.TrElement.Security.LastPrice > 0)
                    {
                        if (stopOrders.Count() > 0)
                        {
                            var stOrdSec = stopOrders.Where(o => o.Sec == pan.TrElement.Security);
                            if (stOrdSec.Count() == 1)
                            {
                                if (countPos != 0)
                                {
                                    var lastOrd = stOrdSec.Last();
                                    //Расчет предпологаемого убытка
                                    decimal StopValue = lastOrd.ConditionPrice - pan.TrElement.Security.LastPrice;
                                    StopValue = StopValue < 0 ? StopValue * -1 : StopValue;
                                    StopValue = (StopValue / pan.TrElement.Security.Params.MinPriceStep) *
                                        (pan.TrElement.Security.Params.StepPrice == 0 ? pan.TrElement.Security.Params.MinPriceStep : pan.TrElement.Security.Params.StepPrice);
                                    StopLossValue = Math.Round(countPos * StopValue, 2);
                                }
                                else
                                {
                                    //если позиция ликвидирована, а стоп-заявки остались, то снимаем стопы
                                    Parent.Trader.CancelAllStopOrder(pan.TrElement.Security);
                                }
                            }
                            else
                            {
                                //Перевыставляем стоп если больше одной заявки
                                if (stOrdSec.Count() > 1)
                                {
                                    //SendStopOrder(pan, true);
                                    Form_MessageSignal.Show("Double stop order " + pan.TrElement.Security,
                                        pan.TrElement.Security.ToString(), true);
                                    CancelAllStopOrders(pan);
                                }
                            }
                        }
                    }
                    pan.LabelStopValue.BackColor = Color.Transparent;
                    if (StopLossValue != 0)
                    {
                        pan.LabelStopValue.Text = "-" + StopLossValue.ToString();
                    }
                    else
                    {
                        //Помечаем цветом, что надо выставить стоп
                        pan.LabelStopValue.Text = "0";
                        if (countPos != 0)
                        {
                            pan.LabelStopValue.BackColor = Color.Red;
                        }
                    }
                }
            }
        }

        public void UpdaterDataSecond(DispatcherTimer timer)
        {
            if (ListPanels.Count == 0 || Parent.CheckKliring())
            {
                return;
            }

            var stopOrders = Parent.Trader.Objects.tStopOrders.SearchAll(so =>
            so.IsActive() && so.Comment.Contains(Define.STOP_LOSS));
            foreach (var pan in ListPanels)
            {
                pan.LayoutPanel.BackColor = Color.LightBlue;
                if (pan.TrElement.IsNull() && pan.ComboboxSecurity.Text.Length > 2)
                {
                    UpdateDataPanel(pan);
                }
                if (pan.TrElement.IsNull())
                {
                    continue;
                }
                if (pan.Position.NotIsNull())
                {
                    var countPos = pan.Position.CurrentVolume;
                    countPos = countPos / (pan.TrElement.Security.Params.DaysToMatDate > 0 ? 1 : pan.TrElement.Security.Lot);
                    if (countPos != 0)
                    {
                        var stOrder = stopOrders.Where(o => o.Sec == pan.TrElement.Security);
                        if (stopOrders.Count() > 0)
                        {
                            var lastOrd = stOrder.Count() > 0 ? stOrder.Last() : null;
                            if (lastOrd.NotIsNull())
                            {
                                if (lastOrd.Volume != countPos && lastOrd.ConditionPrice == pan.PriceStop.Value)
                                {
                                    //Если позиция изменилась перевыставляем стоп
                                    SendStopOrder(pan, true);
                                }
                            }
                            UpdateTrackOrders(pan);
                        }
                        //Закрываем стопы обратные
                        if (pan.Position.IsBuy())
                        {
                            var stOrd = stOrder.Where(o => o.Price > pan.TrElement.Security.LastPrice && pan.TrElement.Security.LastPrice > 0);
                            if (stOrd.Count() > 0) Parent.Trader.CancelListStopOrders(stOrd);
                            pan.LayoutPanel.BackColor = Color.LightGreen;
                        }
                        if (pan.Position.IsSell())
                        {
                            var stOrd = stOrder.Where(o => o.Price < pan.TrElement.Security.LastPrice && pan.TrElement.Security.LastPrice > 0);
                            if (stOrd.Count() > 0) Parent.Trader.CancelListStopOrders(stOrd);
                            pan.LayoutPanel.BackColor = Color.Coral;
                        }
                    }
                }

                //Получаем информацию по заявкам
                var orders = Parent.Trader.Objects.tOrders.SearchAll(o => o.Sec == pan.TrElement.Security && o.Status == OrderStatus.ACTIVE);
                var prefixOrdBuySell = "Or.B/S: ";
                if (orders.NotIsNull() && orders.Count() > 0)
                {
                    //получаем кол-во активных заявок
                    var sumOrdBuy = orders.Where(o => o.Direction == OrderDirection.Buy)
                        .Sum(o => o.Balance);
                    var sumOrdSell = orders.Where(o => o.Direction == OrderDirection.Sell)
                        .Sum(o => o.Balance);
                    pan.LabelOrderBuySell.Text = prefixOrdBuySell + sumOrdBuy + "/" + sumOrdSell;
                }
                else
                {
                    pan.LabelOrderBuySell.Text = prefixOrdBuySell + " 0/0";
                }

            }
            var listPortf = Parent.Trader.Objects.tPortfolios.SearchAll(p => p.VarMargin != 0 && p.LimitKind == 0);
            if (listPortf.NotIsNull())
            {
                string textVarMargin = "";
                foreach (var portf in listPortf)
                {
                    textVarMargin += portf.Account.AccID + ": " + portf.VarMargin + "\n";
                }
                SetInfo(textVarMargin);
            }
        }
        /// <summary>
        /// Закрывает все активные стоп ордера
        /// </summary>
        protected int CancelAllStopOrders(PanelControl panel)
        {
            var cancelOrders = Parent.Trader.Objects.tStopOrders.SearchAll(so =>
                    so.Sec == panel.TrElement.Security
                    && so.IsActive() && so.Comment.Contains(Define.STOP_LOSS));
            Parent.Trader.CancelListStopOrders(cancelOrders);
            //RemoveListSignal(cancelOrders);
            return cancelOrders.Count();
        }

        protected virtual void SetMessage(string text)
        {

        }
        protected virtual void SetInfo(string text)
        {

        }
    }
}
