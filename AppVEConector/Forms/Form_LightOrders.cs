using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using TradingLib;
using AppVEConector.libs;
using MarketObjects;
using Connector.Logs;
using Managers;
using AppVEConector.libs.Signal;
using QuikConnector.MarketObjects;

namespace AppVEConector.Forms
{
    public partial class Form_LightOrders : Form
    {
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
        public class PanelOrders
        {
            public TElement TrElement = null;
            public Position Position = null;
            public int Index = -1;
            public int Volume = 1;

            public ComboBox ComboboxSecurity = null;
            public Label LabelPosition = null;
            public Label LabelStopValue = null;
            public NumericUpDown PriceOrder = null;
            public NumericUpDown PriceStop = null;

            public Button ButtonBuy = null;
            public Button ButtonSell = null;
            public Button ButtonStop = null;
            public Button ButtonCancelOrder = null;
            public Button ButtonCancelStop = null;

            public Button ButtonPlusStop = null;
            public Button ButtonMinusStop = null;

            public Button ButtonSettings = null;

            public TableLayoutPanel LayoutPanel = null;

            public TrackOrder TrackOrder = null;

            public DateTime LastSendStopOrder;
        }

        /// <summary> Список всех панелей  </summary>
        private List<PanelOrders> ListPanels = new List<PanelOrders>();

        private new MainForm Parent = null;

        public Form_LightOrders(MainForm parent)
        {
            InitializeComponent();
            this.Parent = parent;
            this.FindElemPanels(this);
        }

        private void Form_LightOrders_Load(object sender, EventArgs e)
        {
            this.ChangeOrientation();
            InitPanels();
        }

        private void ChangeOrientation()
        {
            this.TopMost = true;
            var rectScreen = GetScreen();

            var widthScreen = rectScreen.Width;
            var heightScreen = rectScreen.Height;
            this.Top = (int)(heightScreen / 2) - (int)(this.Height / 2);
            this.Left = widthScreen - this.Width;

            this.Text = rectScreen.Width + " " + rectScreen.Height;
        }

        private Rectangle GetScreen()
        {
            return Screen.FromControl(this).Bounds;
        }

        private void Form_LightOrders_Move(object sender, EventArgs e)
        {
            var rectScreen = GetScreen();
            this.Text = rectScreen.Width + " " + rectScreen.Height;
        }

        private void FindElemPanels(object control)
        {
            if (control.IsNull()) return;
            var obj = (Control)control;
            if (obj.Controls.Count > 0)
            {
                foreach (var c in obj.Controls)
                {
                    this.FindElemPanels(c);
                }
            }
            if (obj.Tag.NotIsNull())
            {
                string strTag = obj.Tag.ToString();
                Regex rgx = new Regex(@"[a-zA-Z]*");
                int index = rgx.Replace(strTag, "").ToInt32();
                var el = this.ListPanels.FirstOrDefault(p => p.Index == index);
                if (el.IsNull())
                {
                    el = new PanelOrders();
                    this.ListPanels.Add(el);
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
                    if (strTag.Contains("POS")) el.LabelPosition = (Label)control;
                    if (strTag.Contains("LS")) el.LabelStopValue = (Label)control;
                }
                if (control is Button)
                {
                    var b = (Button)control;
                    if (strTag.Contains("BBuy")) el.ButtonBuy = b;
                    if (strTag.Contains("BSell")) el.ButtonSell = b;
                    if (strTag.Contains("BSL")) el.ButtonStop = b;
                    if (strTag.Contains("CanOr")) el.ButtonCancelOrder = b;
                    if (strTag.Contains("CanSt")) el.ButtonCancelStop = b;

                    if (strTag.Contains("PLSTOP")) el.ButtonPlusStop = b;
                    if (strTag.Contains("MISTOP")) el.ButtonMinusStop = b;

                    if (strTag.Contains("BSET")) el.ButtonSettings = b;
                }
                if (control is TableLayoutPanel)
                {
                    if (strTag.Contains("LAY")) el.LayoutPanel = (TableLayoutPanel)control;
                }

                if (control is NumericUpDown)
                {
                    if (strTag.Contains("OP"))
                    {
                        el.PriceOrder = (NumericUpDown)control;
                        el.PriceOrder.InitWheelDecimal();
                    }
                    if (strTag.Contains("SP"))
                    {
                        el.PriceStop = (NumericUpDown)control;
                        el.PriceStop.InitWheelDecimal();
                    }
                }
            }
        }

        private void InitPanels()
        {
            if (this.ListPanels.Count == 0) return;

            foreach (var pan in this.ListPanels)
            {
                this.InitOnePanel(pan);
            }

            Action<IEnumerable<Reply>> actionReply = (listReply) =>
            {
                var reply = listReply.Last();
                textBoxMsg.GuiAsync(() =>
                {
                    this.SetMessge(reply.ResultMsg);
                });
            };

            this.Parent.OnTimer1s += this.UpdaterAll;
            this.Parent.OnTimer3s += this.UpdaterInfoByOrders;

            this.Parent.OnNewReply += actionReply;

            this.FormClosed += (s, e) =>
            {
                this.Parent.OnTimer1s -= this.UpdaterAll;
                this.Parent.OnTimer3s -= this.UpdaterInfoByOrders;
                this.Parent.OnNewReply -= actionReply;
                this.Parent = null;
            };

            this.buttonCloseWin.Click += (s, e) => { this.Close(); };

        }

        private void InitOnePanel(PanelOrders panel)
        {
            if (panel.ComboboxSecurity.NotIsNull())
            {
                this.LoadListTradeSec(panel.ComboboxSecurity);
                panel.ComboboxSecurity.TextChanged += (s, e) =>
                {
                    this.UpdateDataPanel(panel);
                };

                panel.ButtonBuy.MouseUp += (s, e) =>
                {
                    MouseEventArgs ev = (MouseEventArgs)e;
                    if (ev.Button == MouseButtons.Left)
                    {
                        if (panel.TrElement.IsNull()) return;
                        var order = new Order();
                        order.Sec = panel.TrElement.Security;
                        order.Price = panel.PriceOrder.Value;
                        order.Volume = panel.Volume;
                        order.Direction = OrderDirection.Buy;
                        this.Parent.Trader.CreateOrder(order);
                    }
                    if (ev.Button == MouseButtons.Right)
                    {
                        panel.Volume = panel.Volume < 1000 ? panel.Volume + 1 : panel.Volume;
                    }
                };

                panel.ButtonSell.MouseUp += (s, e) =>
                {
                    MouseEventArgs ev = (MouseEventArgs)e;
                    if (ev.Button == MouseButtons.Left)
                    {
                        if (panel.TrElement.IsNull()) return;
                        var order = new Order();
                        order.Sec = panel.TrElement.Security;
                        order.Price = panel.PriceOrder.Value;
                        order.Volume = panel.Volume;
                        order.Direction = OrderDirection.Sell;
                        this.Parent.Trader.CreateOrder(order);
                    }
                    if (ev.Button == MouseButtons.Right)
                    {
                        panel.Volume = panel.Volume > 1 ? panel.Volume - 1 : panel.Volume;
                    }
                };

                panel.PriceOrder.MouseDoubleClick += (s, e) =>
                {
                    if (panel.TrElement.IsNull()) return;
                    if (panel.PriceOrder.IsMouseOnTextBox(e))
                    {
                        panel.PriceOrder.Value = panel.TrElement.Security.LastPrice > 0 ? panel.TrElement.Security.LastPrice : 0;
                    }
                };
                panel.PriceStop.MouseDoubleClick += (s, e) =>
                {
                    if (panel.TrElement.IsNull()) return;
                    if (panel.PriceStop.IsMouseOnTextBox(e))
                    {
                        var stopOrders = this.Parent.Trader.Objects.StopOrders.FirstOrDefault(so => so.IsActive()
                                    && so.Sec == panel.TrElement.Security && so.Comment.Contains(Define.STOP_LOSS));
                        if (stopOrders.NotIsNull())
                        {
                            panel.PriceStop.Value = stopOrders.ConditionPrice;
                        }
                        else
                        {
                            panel.PriceStop.Value = panel.TrElement.Security.LastPrice > 0 ? panel.TrElement.Security.LastPrice : 0;
                        }
                    }
                };


                panel.ButtonCancelOrder.MouseUp += (s, e) =>
                {
                    if (panel.TrElement.IsNull()) return;
                    MouseEventArgs ev = (MouseEventArgs)e;
                    if (ev.Button == MouseButtons.Left)
                    {
                        this.CloseAllOrders(panel);
                    }
                    if (ev.Button == MouseButtons.Right)
                    {
                        this.CloseAllOrders(panel, true);
                    }
                };
                panel.ButtonCancelStop.MouseUp += (s, e) =>
                {
                    if (panel.TrElement.IsNull()) return;
                    MouseEventArgs ev = (MouseEventArgs)e;
                    if (ev.Button == MouseButtons.Left)
                    {
                        this.CloseAllStopOrders(panel);
                    }
                    if (ev.Button == MouseButtons.Right)
                    {
                        this.CloseAllStopOrders(panel, true);
                    }
                };

                panel.ButtonStop.MouseUp += (s, e) =>
                {
                    if (panel.TrElement.IsNull()) return;

                    MouseEventArgs ev = (MouseEventArgs)e;
                    if (ev.Button == MouseButtons.Left)
                    {
                        this.SendStopOrder(panel, false);
                    }
                    if (ev.Button == MouseButtons.Right)
                    {
                        this.SendStopOrder(panel, false, true);
                    }
                };

                panel.LabelPosition.MouseUp += (s, e) =>
                {
                    if (panel.TrElement.IsNull()) return;
                    MouseEventArgs ev = (MouseEventArgs)e;
                    if (ev.Button == MouseButtons.Left)
                    {
                        this.ClosePos(panel);
                    }
                    if (ev.Button == MouseButtons.Right)
                    {
                        this.Parent.ShowGraphicDepth(panel.TrElement.Security);
                    }
                };

                panel.ButtonPlusStop.MouseUp += (s, e) =>
                {
                    if (panel.TrElement.IsNull()) return;
                    MouseEventArgs ev = (MouseEventArgs)e;
                    if (ev.Button == MouseButtons.Left)
                    {
                        panel.PriceStop.Value += panel.TrElement.Security.MinPriceStep * 10;
                    }
                };
                panel.ButtonMinusStop.MouseUp += (s, e) =>
                {
                    if (panel.TrElement.IsNull()) return;
                    MouseEventArgs ev = (MouseEventArgs)e;
                    if (ev.Button == MouseButtons.Left)
                    {
                        if (panel.PriceStop.Value - panel.TrElement.Security.MinPriceStep * 10 > 0)
                            panel.PriceStop.Value -= panel.TrElement.Security.MinPriceStep * 10;
                    }
                };

                panel.ButtonSettings.Click += (s, e) =>
                {
                    Form_SettingsStop formSettings = new Form_SettingsStop(this, panel);
                    formSettings.ShowDialog();
                };



            }
        }
        /// <summary>
        /// Закрытие всех ордеров или просмотр инфо по заявкам
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="showInfo"></param>
        private void CloseAllOrders(PanelOrders panel, bool showInfo = false)
        {
            if (showInfo)
            {
                var pos = this.Parent.Trader.Objects.Positions.FirstOrDefault(p => p.Sec == panel.TrElement.Security);
                var orders = this.Parent.Trader.Objects.Orders.Where(p => p.Sec == panel.TrElement.Security && p.IsActive());
                //panel.TrElement.Security.Orders.Where(p => p.Sec == panel.TrElement.Security && p.Status == OrderStatus.ACTIVE);
                //this.Parent.Trader.Objects.Orders.Where(p => p.Sec == panel.TrElement.Security && p.Status == OrderStatus.ACTIVE);
                var stop_orders = this.Parent.Trader.Objects.StopOrders.Where(p => p.Sec == panel.TrElement.Security && p.IsActive());

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
                this.Parent.Trader.CancelAllOrder(panel.TrElement.Security);
            }
        }

        /// <summary>  Закрытие всех ордеров или просмотр инфо по стоп-заявкам </summary>
        /// <param name="panel"></param>
        /// <param name="showInfo"></param>
        private void CloseAllStopOrders(PanelOrders panel, bool showInfo = false)
        {
            if (showInfo)
            {
                var pos = this.Parent.Trader.Objects.Positions.FirstOrDefault(p => p.Sec == panel.TrElement.Security);
                var orders = this.Parent.Trader.Objects.Orders.Where(p => p.Sec == panel.TrElement.Security && p.IsActive());
                var stop_orders = this.Parent.Trader.Objects.StopOrders.Where(p => p.Sec == panel.TrElement.Security && p.IsActive());

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
                this.CancelAllStopOrders(panel);
            }
        }

        /// <summary>
        /// Выход из позиции (закрытие позиции)
        /// </summary>
        /// <param name="panel"></param>
        private void ClosePos(PanelOrders panel)
        {
            Qlog.CatchException(() =>
            {
                var pos = this.Parent.Trader.Objects.Positions.FirstOrDefault(p => p.Sec == panel.TrElement.Security);
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
                                this.Parent.Trader.CreateOrder(order);
                            }
                            else
                            {
                                this.SetMessge("Не удалось определить последнюю цену.");
                            }
                        }
                    }
                }
            });
        }
        private void SendStopOrder(PanelOrders panel, bool autoStop = false, bool showInfo = false)
        {
            if (autoStop)
            {
                if (panel.LastSendStopOrder > DateTime.Now.AddSeconds(-5))
                {
                    //Для избежания многократного выставления
                    return;
                }
            }
            var pos = this.Parent.Trader.Objects.Positions.FirstOrDefault(p => p.Sec == panel.TrElement.Security);
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
                this.SetMessge("Некорректная цена стоп-ордера!");
                return;
            }
            if (volume == 0)
            {
                this.SetMessge("Не текущих позиций!");
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
                        this.SetMessge("Не корректная цена стоп заявки! Необходимо указать цену ниже текущей.");
                        return;
                    }
                }
                if (pos.IsSell() && panel.PriceStop.Value > 0)
                {
                    stopOrder.Direction = OrderDirection.Buy;
                    if (panel.PriceStop.Value < lastPrice)
                    {
                        this.SetMessge("Не корректная цена стоп заявки! Необходимо указать цену выше текущей.");
                        return;
                    }
                }
            }

            MThread.InitThread(ThreadPriority.Normal, () =>
            {
                Thread.Sleep(1000);
                var countClosed = this.CancelAllStopOrders(panel);
                Thread.Sleep(3000);
                this.Parent.Trader.CreateStopOrder(stopOrder, StopOrderType.StopLimit);
                this.addSignalByStop(stopOrder);
            });
            panel.LastSendStopOrder = DateTime.Now;
        }
        /// <summary>
        /// Сигнал по стопу
        /// </summary>
        /// <param name="stopOrder"></param>
        private void addSignalByStop(StopOrder stopOrder)
        {
            Qlog.CatchException(() =>
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
                MainForm.GSMSignaler.AddSignal(newSign);
            });
        }
        /// <summary>
        /// Удаляет сигнал по цене
        /// </summary>
        /// <param name="price"></param>
        private void RemoveSignal(decimal price)
        {
            var listSigDel = MainForm.GSMSignaler.ToArray().Where(s => s.Price == price);
            foreach (var sig in listSigDel)
            {
                MainForm.GSMSignaler.RemoveSignal(sig);
            }
        }
        /// <summary>
        /// Снимает сигнал по стоп заявкам
        /// </summary>
        /// <param name="listOrder"></param>
        /*private void RemoveListSignal(IEnumerable<StopOrder> listOrder)
		{
			Qlog.CatchException(() =>
			{
				foreach (var sOrd in listOrder)
				{
					this.RemoveSignal(sOrd.ConditionPrice);
				}
			});
		}*/

        /// <summary>
        /// Закрывает все активные стоп ордера
        /// </summary>
        private int CancelAllStopOrders(PanelOrders panel)
        {
            var cancelOrders = this.Parent.Trader.Objects.StopOrders.Where(so =>
                    so.Sec == panel.TrElement.Security
                    && so.IsActive() && so.Comment.Contains(Define.STOP_LOSS));
            this.Parent.Trader.CancelListStopOrders(cancelOrders);
            //this.RemoveListSignal(cancelOrders);
            return cancelOrders.Count();
        }
        /// <summary> Обновляет данные в текущей панели </summary>
        /// <param name="panel"></param>
        private void UpdateDataPanel(PanelOrders panel)
        {
            Qlog.CatchException(() =>
            {
                if (panel.ComboboxSecurity.SelectedItem.IsNull()) return;
                string strSec = panel.ComboboxSecurity.SelectedItem.ToString();
                var sec = this.Parent.GetSecCodeAndClass(strSec);
                if (sec.NotIsNull())
                {
                    panel.TrElement = this.Parent.DataTrading.Collection.FirstOrDefault(t => t.Security == sec);
                    if (panel.TrElement.NotIsNull())
                    {
                        panel.Position = this.Parent.Trader.Objects.Positions.FirstOrDefault(p => p.Sec == panel.TrElement.Security);

                        this.Parent.Trader.RegisterSecurities(sec);
                        panel.PriceOrder.InitSecurity(sec);
                        panel.PriceOrder.Value = sec.LastPrice > 0 ? sec.LastPrice : 0;

                        panel.PriceStop.InitSecurity(sec);

                        //Установка последней стоп цены
                        panel.PriceStop.Value = 0;
                        var stopOrders = this.Parent.Trader.Objects.StopOrders.Where(o => o.Sec == panel.TrElement.Security);
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
            });
        }

        public void UpdaterInfoByOrders(DispatcherTimer timer)
        {
            if (this.ListPanels.Count == 0) return;
            Qlog.CatchException(() =>
            {
                if (this.Parent.CheckKliring()) return;

                var stopOrders = this.Parent.Trader.Objects.StopOrders.Where(so => so.IsActive()
                                    && so.Comment.Contains(Define.STOP_LOSS));
                foreach (var pan in this.ListPanels)
                {
                    pan.LayoutPanel.BackColor = System.Drawing.Color.LightBlue;
                    if (pan.TrElement.NotIsNull())
                    {
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
                                            this.SendStopOrder(pan, true);
                                        }
                                    }
                                    this.UpdateTrackOrders(pan);
                                }
                                //Закрываем стопы обратные
                                if (pan.Position.IsBuy())
                                {
                                    var stOrd = stOrder.Where(o => o.Price > pan.TrElement.Security.LastPrice && pan.TrElement.Security.LastPrice > 0);
                                    if (stOrd.Count() > 0) this.Parent.Trader.CancelListStopOrders(stOrd);
                                    pan.LayoutPanel.BackColor = Color.LightGreen;
                                }
                                if (pan.Position.IsSell())
                                {
                                    var stOrd = stOrder.Where(o => o.Price < pan.TrElement.Security.LastPrice && pan.TrElement.Security.LastPrice > 0);
                                    if (stOrd.Count() > 0) this.Parent.Trader.CancelListStopOrders(stOrd);
                                    pan.LayoutPanel.BackColor = Color.Coral;
                                }
                            }
                        }
                    }
                }
            });
        }

        private void UpdateTrackOrders(PanelOrders panel)
        {
            if (panel.TrElement.IsNull()) return;
            if (panel.TrackOrder.IsNull()) return;
            Qlog.CatchException(() =>
            {
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
                        this.SendStopOrder(panel);
                        panel.TrackOrder.LastTrack = DateTime.Now;
                    }
                }
            });
        }
        /// <summary> Переодически обновитель данных </summary>
        public void UpdaterAll()
        {
            if (this.ListPanels.Count == 0) return;
            Qlog.CatchException(() =>
            {
                //Проверка идет ли клиринг
                if (this.Parent.CheckKliring()) return;

                var stopOrders = this.Parent.Trader.Objects.StopOrders.Where(so =>
                    so.IsActive() && so.Comment.Contains(Define.STOP_LOSS));
                foreach (var pan in this.ListPanels)
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
                                }
                                else
                                {
                                    //Перевыставляем стоп если больше одной заявки
                                    if (stOrdSec.Count() > 1)
                                    {
                                        //this.SendStopOrder(pan, true);
                                        Form_MessageSignal.Show("Double stop order " + pan.TrElement.Security, true);
                                        this.CancelAllStopOrders(pan);
                                    }
                                }
                            }
                            if (pan.Position.Data.CurrentNet == 0)
                            {
                                this.Parent.Trader.CancelAllStopOrder(pan.TrElement.Security);
                            }
                        }
                        pan.LabelStopValue.Text = "-" + StopLossValue.ToString();
                    }
                    pan.ButtonBuy.Text = "▲" + pan.Volume;
                    pan.ButtonSell.Text = "▼" + pan.Volume;
                }
                var listPortf = this.Parent.Trader.Objects.Portfolios.Where(p => p.VarMargin != 0 && p.LimitKind == 0);
                if (listPortf.NotIsNull())
                {
                    string textVarMargin = "";
                    foreach (var portf in listPortf)
                    {
                        textVarMargin += portf.Account.AccID + ": " + portf.VarMargin + "\n";
                    }
                    labelVarMargin.GuiAsync(() =>
                    {
                        labelVarMargin.Text = textVarMargin;
                    });
                }
            });
        }

        /// <summary> Загружаем торгуемые элементы из файла </summary>
        private void LoadListTradeSec(ComboBox comboSec)
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
        }

        /// <summary> Установка сообщения </summary>
        private void SetMessge(string text)
        {
            this.textBoxMsg.Text = text;
        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonMimize20s_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            MThread.InitThread(() =>
            {
                Thread.Sleep(20000);
                this.GuiAsync(() =>
                {
                    this.WindowState = FormWindowState.Normal;
                });
            });
        }
    }
}
