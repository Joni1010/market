using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using System.Windows.Threading;
using System.Threading;
using AppVEConector.libs;

using MarketObjects;
using Connector.Logs;
using Managers;
using AppVEConector.Forms.StopOrders;
using Libs;
using AppVEConector.libs.Signal;
using Market.AppTools;
using System.ComponentModel;
using libs;
using AppVEConector.Forms;
using QuikConnector.libs.json;
using System.Text;

namespace AppVEConector
{
    public partial class MainForm : Form
    {
        public Connector.QuikConnector Trader = new Connector.QuikConnector();
        /// <summary> Торгуемы набор </summary>
        public TCollection DataTrading = new TCollection();

        /// <summary> 100ms секундный таймер </summary>
        public event Action<DispatcherTimer> OnTimer100ms = null;
        /// <summary> 1 секундный таймер </summary>
        public event Action<DispatcherTimer> OnTimer1s = null;
        /// <summary> 3-х секундный таймер </summary>
        public event Action<DispatcherTimer> OnTimer3s = null;
        /// <summary> 5-х секундный таймер </summary>
        public event Action<DispatcherTimer> OnTimer5s = null;

        /// <summary> Событие новых сообщений в терминале </summary>
        public event Action<IEnumerable<Reply>> OnNewReply = null;

        /// <summary> Список откртых форм со стаканом </summary>
        List<Form_GraphicDepth> ListFormsDepth = new List<Form_GraphicDepth>();
        Mutex MutexListFormsDEpth = new Mutex();

        /// <summary> Последнее сообщение по рынку </summary>
        public Reply LastMarketReply = null;

        private Form_Arbitration FormArbitration = null;

        //private TradeController ControlTrade = new TradeController();
        /// <summary> Форма быстрых ордеров </summary>
        private Form_LightOrders FormSpeedOrders = null;
        /// <summary> Список создаваемы таймеров</summary>
        private List<DispatcherTimer> listTimers = new List<DispatcherTimer>();

        public Kliring KliringDay = null;
        public Kliring KliringEndDay = null;

        public MainForm()
        {
            var klirDay = Connector.QuikConnector.ConfSettings.GetParam("Kliring", "KlDay").Value;
            var klirEndDay = Connector.QuikConnector.ConfSettings.GetParam("Kliring", "KlEndDay").Value;
            this.KliringDay = new Kliring(klirDay);
            this.KliringEndDay = new Kliring(klirEndDay);
            InitializeComponent();
        }
        /// <summary>
        /// Поиск инструмента в обход демо и инфо инструментов.
        /// </summary>
        /// <param name="secCode"></param>
        /// <returns></returns>
        public Securities SearchSecurity(string secCode)
        {
            Qlog.CatchException(() =>
            {
                var sec = Trader.Objects.tSecurities.ToArray().FirstOrDefault(s => s.Code.Contains(secCode)
                            && !s.Class.Code.Contains("INFO") && !s.Class.Code.Contains("EMU"));
                return sec;
            });
            return null;
        }
        public Securities SearchSecurity(string secCode, string secClassCode)
        {
            Qlog.CatchException(() =>
            {
                var sec = Trader.Objects.tSecurities.ToArray().FirstOrDefault(s => s.Code.Contains(secCode)
                            && s.Class.Code.Contains(secClassCode) && !s.Class.Code.Contains("EMU"));
                return sec;
            });
            return null;
        }

        /// <summary> Получение формы со стаканом и графиком </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public Form_GraphicDepth ShowGraphicDepth(Securities sec)
        {
            Qlog.CatchException(() =>
            {
                var form = ListFormsDepth.FirstOrDefault(f => f.TrElement.Security == sec);
                if (form.IsNull())
                {
                    var elTr = this.DataTrading.AddOrFind(sec);
                    if (elTr.NotIsNull())
                    {
                        form = new Form_GraphicDepth(Trader, elTr, this);
                        ListFormsDepth.Add(form);
                    }
                    //Инициализация закрытия
                    form.FormClosed += (s, e) =>
                {
                    ListFormsDepth.Remove(form);
                };
                }
                if (form.NotIsNull())
                {
                    form.Show();
                    form.WindowState = FormWindowState.Maximized;
                }
                return form;
            });
            return null;
        }

        private void MainForm_Load(object sender, EventArgs ev)
        {
            Form_MessageSignal.Parent = this;
            Qlog.CatchException(() =>
            {
                dataGridPortfolios.Rows[0].Resizable = DataGridViewTriState.False;
                dataGridPositions.Rows[0].Resizable = DataGridViewTriState.False;
                dataGridViewStopOrders.Rows[0].Resizable = DataGridViewTriState.False;

                this.Trader.RegisterAllParamSec();

                InitTimers();

                this.InitPanelSignal();
                this.InitPanelFastGap();
                InitAutoOrders();
                InitAutoStopLoss();


                Trader.Objects.OnRun += () =>
                {
                    this.RegisterAllSecurity();
                };
                //Открытие торгового окна по найденному инструменту, по двойному щелчку
                dataGridFoundSec.DoubleClick += (s, e) =>
            {
                foreach (DataGridViewRow row in dataGridFoundSec.SelectedRows)
                {
                    if (row.Tag.NotIsNull())
                    {
                        var sec = (Securities)row.Tag;
                        this.ShowGraphicDepth(sec);
                    }
                }
            };
                //Открытие торгового окна по позиции, по двойному щелчку
                dataGridPositions.DoubleClick += (s, e) =>
            {
                foreach (DataGridViewRow row in dataGridPositions.SelectedRows)
                {
                    if (row.Tag.NotIsNull())
                    {
                        var infoPos = ((string)row.Tag);
                        if (!infoPos.Empty())
                        {
                            var sec = GetSecCodeAndClass(infoPos);
                            if (sec.NotIsNull())
                            {
                                ShowGraphicDepth(sec);
                            }
                        }
                    }
                }
            };


                loadTextDescription();
                this.WindowState = FormWindowState.Maximized;
            }, "", () => { MessageBox.Show("Error load form!"); });
        }


        /// /////////////////////////////////////////////////////////////////////////
        List<DataGridViewRow> listRowsPortfolios = new List<DataGridViewRow>();
        private void UpdateInfoPortfolios()
        {
            int i = 0;
            var listPortf = Trader.Objects.tPortfolios.ToArray();//.Where(p => p.TypeClient == 2);
                                                                 //int count = listPortf.Count();
            foreach (var p in listPortf)
            {
                dataGridPortfolios.GuiAsync(() =>
                {
                    var row = listRowsPortfolios.FirstOrDefault(r => r.Tag.NotIsNull() && ((Portfolio)r.Tag) == p);//((Portfolio)r.Tag).Account == p.Account && ((Portfolio)r.Tag).Client == p.Client
                                                                                                                   //&& r.Cells[1].Value.ToString() == p.LimitKind.ToString());
                    if (row.IsNull())
                    {
                        var newRow = (DataGridViewRow)dataGridPortfolios.Rows[0].Clone();
                        newRow.Cells[0].Value = "";
                        listRowsPortfolios.Add(newRow);
                        dataGridPortfolios.Rows.Add(newRow);
                        row = newRow;
                    }

                    row.Cells[0].Value = p.Client.Code;
                    row.Cells[1].Value = p.LimitKind.ToString();
                    row.Cells[2].Value = p.TypeClient.ToString();
                    row.Cells[3].Value = p.Balance.ToString();
                    row.Cells[4].Value = p.CurrentBalance.ToString();
                    row.Cells[5].Value = p.PositionBalance.ToString();

                    row.Cells[6].Value = p.VarMargin.ToString();
                    if (p.VarMargin > 0) row.Cells[6].Style.BackColor = Color.LightGreen;
                    if (p.VarMargin == 0) row.Cells[6].Style.BackColor = Color.White;
                    if (p.VarMargin < 0) row.Cells[6].Style.BackColor = Color.LightCoral;

                    row.Cells[7].Value = p.Commission.ToString();
                    row.Tag = p;
                });
                i++;
            }
        }
        /// /////////////////////////////////////////////////////////////////////////
        List<DataGridViewRow> listRowsPositions = new List<DataGridViewRow>();

        void UpdateInfoPositions()
        {
            if (Trader.Objects.tPositions.Count > 0)
            {
                dataGridPositions.GuiAsync(() =>
                {
                    var listPos = Trader.Objects.tPositions.ToArray().OrderBy(p => p.Sec.ToString());
                    string lastSec = null;
                    foreach (var p in listPos)
                    {
                        if (p.Sec.NotIsNull() && lastSec != p.ToString())
                        {
                            lastSec = p.ToString();
                            var row = listRowsPositions.FirstOrDefault(r => r.Tag.ToString() == lastSec);
                            if (row.IsNull())
                            {
                                var newRow = (DataGridViewRow)dataGridPositions.Rows[0].Clone();
                                newRow.Cells[0].Value = "";
                                listRowsPositions.Add(newRow);
                                row = newRow;
                                Trader.RegisterSecurities(p.Sec);
                            }
                            //Ищем все позиции данного инструмента
                            var allSecPos = listPos.Where(ps => ps.Sec == p.Sec && ps.Client.Code == p.Client.Code)
                            .OrderBy(ps => ps.Data.Type)
                            .ToArray();
                            if (allSecPos.NotIsNull() && allSecPos.Length > 0)
                            {
                                var types = "";
                                var curPos = "";
                                //dataGridPositions.Rows.Clear();
                                foreach (var itemPos in allSecPos)
                                {
                                    types += types.Empty()
                                        ? itemPos.Data.Type.ToString()
                                        : "/" + itemPos.Data.Type.ToString();
                                    curPos += curPos.Empty()
                                        ? itemPos.Data.CurrentNet.ToString()
                                        : "/" + itemPos.Data.CurrentNet.ToString();
                                }

                                row.Tag = lastSec;
                                row.Cells[0].Value = p.Sec.Name;
                                row.Cells[1].Value = p.Client.NotIsNull() ? p.Client.Code + " " + types : "";
                                row.Cells[2].Value = p.Sec.ToString();
                                row.Cells[3].Value = p.Sec.Lot.ToString();
                                row.Cells[4].Value = p.Sec.StepPrice.ToString();
                                row.Cells[5].Value = p.Sec.Params.BuyDepo.ToString();
                                row.Cells[6].Value = curPos;// p.Data.CurrentNet.ToString();
                                setColorRow(row.Cells[6], p.Data.CurrentNet);
                                //Orders
                                row.Cells[7].Value = p.Data.OrdersBuy.ToString() + " / " + p.Data.OrdersSell.ToString();
                                //Var margin
                                row.Cells[8].Value = p.Data.VarMargin.ToString();
                                setColorRow(row.Cells[8], p.Data.VarMargin);
                            }
                        }
                    }
                    if (dataGridPositions.Rows.Count - 1 != listRowsPositions.Count)
                    {
                        dataGridPositions.Rows.Clear();
                        dataGridPositions.Rows.AddRange(listRowsPositions.OrderBy(r => r.Cells[0].Value).ToArray());
                        //dataGridPositions.Sort(dataGridPositions.Columns["NamePos"], ListSortDirection.Ascending);
                        dataGridPositions.Update();
                    }
                });
            }
        }

        void setColorRow(DataGridViewCell cell, decimal value)
        {
            if (value > 0) cell.Style.BackColor = Color.LightGreen;
            if (value < 0) cell.Style.BackColor = Color.LightCoral;
            if (value == 0) cell.Style.BackColor = Color.White;
        }

        /// /////////////////////////////////////////////////////////////////////////
        List<DataGridViewRow> listRowsOrders = new List<DataGridViewRow>();
        void UpdateInfoOrders(IEnumerable<Order> orders)
        {
            int i = 0;
            var list = orders.ToArray();
            foreach (var el in list)
            {
                dataGridViewOrders.GuiAsync(() =>
                {
                    var row = listRowsOrders.FirstOrDefault(r => r.Cells[2].Value.ToString() == el.OrderNumber.ToString());
                    if (row == null)
                    {
                        var newRow = (DataGridViewRow)dataGridViewOrders.Rows[0].Clone();
                        newRow.Cells[0].Value = listRowsOrders.Count.ToString();
                        newRow.Cells[2].Value = el.OrderNumber.ToString();
                        newRow.Cells[3].Value = el.Sec;
                        newRow.Cells[4].Value = el.Sec.Code;
                        newRow.Cells[5].Value = el.Price.ToString();
                        newRow.Cells[8].Value = el.IsBuy() ? "Buy" : "Sell";
                        newRow.Cells[9].Value = el.Sec.Name;
                        listRowsOrders.Add(newRow);
                        dataGridViewOrders.Rows.Add(newRow);
                        //Устанавливаем скрол вниз
                        dataGridViewOrders.FirstDisplayedCell = dataGridViewOrders.Rows[dataGridViewOrders.Rows.Count - 1].Cells[0];
                        row = newRow;
                    }
                    row.DefaultCellStyle.BackColor = dataGridViewOrders.Rows[0].DefaultCellStyle.BackColor;
                    //Status
                    if (el.IsActive())
                    {
                        row.Cells[1].Value = "Activ";
                        row.DefaultCellStyle.ForeColor = Color.Red;
                        if (el.Volume != el.Balance)
                        {
                            row.DefaultCellStyle.BackColor = Color.Gold;
                        }
                    }
                    if (el.IsClosed())
                    {
                        row.Cells[1].Value = "Close";
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                        if (el.Volume != el.Balance)
                        {
                            row.DefaultCellStyle.BackColor = Color.Gold;
                        }
                    }
                    if (el.IsExecuted())
                    {
                        row.Cells[1].Value = "Complete";
                        row.DefaultCellStyle.ForeColor = Color.Blue;
                    }

                    //Volume
                    row.Cells[6].Value = el.Volume.ToString();
                    //Balance
                    row.Cells[7].Value = el.Balance.ToString();
                });
                i++;
            }
        }

        /// /////////////////////////////////////////////////////////////////////////
        /// 
        List<DataGridViewRow> listRowsStopOrders = new List<DataGridViewRow>();
        void ResetTableStopOrders()
        {
            Qlog.CatchException(() =>
            {
                listRowsStopOrders.Clear();
                dataGridViewStopOrders.Rows.Clear();
                this.FilteringStopOrders(Trader.Objects.tStopOrders.ToArray());
            });
        }

        /// <summary> фильтр для стоп-заявок  </summary>
        /// <param name="stOrders"></param>
        void FilteringStopOrders(IEnumerable<StopOrder> stOrders)
        {
            IEnumerable<StopOrder> listFilter = new List<StopOrder>();
            bool changeFilter = false;
            if (checkBoxSOActive.Checked)
            {
                listFilter = listFilter.Concat(stOrders.Where(so => so.IsActive()));
                changeFilter = true;
            }
            if (checkBoxSOClosed.Checked)
            {
                listFilter = listFilter.Concat(stOrders.Where(so => so.IsClosed()));
                changeFilter = true;
            }
            if (checkBoxSOExec.Checked)
            {
                listFilter = listFilter.Concat(stOrders.Where(so => so.IsExecuted()));
                changeFilter = true;
            }

            if (changeFilter)
            {
                this.UpdateInfoStopOrders(listFilter);
            }
            else
            {
                this.UpdateInfoStopOrders(stOrders);
            }
        }

        void UpdateInfoStopOrders(IEnumerable<StopOrder> stOrders)
        {
            int i = 0;
            var list = stOrders.ToArray();
            foreach (var el in list)
            {
                dataGridViewStopOrders.GuiAsync(() =>
                {
                    var row = listRowsStopOrders.FirstOrDefault(r => r.Cells[1].Value.ToString() == el.OrderNumber.ToString());
                    if (row == null)
                    {
                        var newRow = (DataGridViewRow)dataGridViewStopOrders.Rows[0].Clone();
                        newRow.Cells[0].Value = listRowsStopOrders.Count.ToString();
                        newRow.Cells[1].Value = el.OrderNumber.ToString();
                        newRow.Cells[2].Value = el.Sec.Code;
                        switch (el.TypeStopOrder)
                        {
                            case StopOrderType.StopLimit:
                                newRow.Cells[3].Value = "Stop лимит";
                                break;
                            case StopOrderType.LinkOrder:
                                newRow.Cells[3].Value = "Со связанной заявкой";
                                break;
                            case StopOrderType.TakeProfit:
                                newRow.Cells[3].Value = "Take профит";
                                break;
                        }
                        newRow.Cells[4].Value = el.IsBuy() ? "Buy" : "Sell";

                        newRow.Cells[6].Value = el.Price.ToString();
                        newRow.Cells[7].Value = el.Volume.ToString();
                        newRow.Cells[8].Value = el.ConditionPrice.ToString();
                        newRow.Cells[9].Value = el.ConditionPrice2.ToString();
                        newRow.Cells[10].Value = el.Spread.ToString();
                        newRow.Cells[11].Value = el.Offset.ToString();

                        listRowsStopOrders.Add(newRow);
                        dataGridViewStopOrders.Rows.Add(newRow);
                        //Устанавливаем скрол вниз
                        dataGridViewStopOrders.FirstDisplayedCell = dataGridViewStopOrders.Rows[dataGridViewStopOrders.Rows.Count - 1].Cells[0];
                        row = newRow;
                    }
                    row.DefaultCellStyle.BackColor = dataGridViewStopOrders.Rows[0].DefaultCellStyle.BackColor;
                    if (el.IsActive())
                    {
                        row.Cells[5].Value = "Active";
                        row.DefaultCellStyle.ForeColor = Color.Red;
                        if (el.Volume != el.Balance)
                        {
                            row.DefaultCellStyle.BackColor = Color.Gold;
                        }
                    }
                    else if (el.IsClosed())
                    {
                        row.Cells[5].Value = "Close";
                        row.DefaultCellStyle.ForeColor = Color.Gray;
                        if (el.Volume != el.Balance)
                        {
                            row.DefaultCellStyle.BackColor = Color.Gold;
                        }
                    }
                    else if (el.IsExecuted())
                    {
                        row.Cells[5].Value = "Complete";
                        row.DefaultCellStyle.ForeColor = Color.Blue;
                    }
                });
                i++;
            }
        }
        /// /////////////////////////////////////////////////////////////////////////
        void UpdateInfoAllTrades(IEnumerable<Trade> trades)
        {
            foreach (var t in trades)
            {
                var el = DataTrading.AddOrFind(t.Sec);
                if (el.NotIsNull())
                {
                    el.NewTrade(t);
                }
            }
            if (trades.Count() > 0)
            {
                Trade lastT = trades.Last();
                ChangeTextMainStatusBar(
                    Trader.Objects.tTrades.CountNew().ToString() + " " +
                    lastT.DateTrade.GetDateTime().ToLongTimeString() +
                    " Trade " + lastT.Number + " " +
                    lastT.SecCode + ": " +
                    lastT.Price + " (" + lastT.Volume + ") " + lastT.Direction,
                    false);
            }
        }
        private void EventAllTrades(IEnumerable<Trade> trades)
        {
            Qlog.CatchException(() =>
            {
                UpdateInfoAllTrades(trades);
            });
        }


        private void EventOldTrades(IEnumerable<Trade> trades)
        {
            Qlog.CatchException(() =>
            {
                foreach (var t in trades)
                {
                    var el = DataTrading.AddOrFind(t.Sec);
                    if (el.NotIsNull())
                    {
                        el.NewTrade(t);
                    }
                }
            });
        }
        private void EventOrders(IEnumerable<Order> orders)
        {
            Qlog.CatchException(() =>
            {
                this.ForEachWinDepth((formDepth) =>
                {
                    formDepth.EventAnyOrder(orders.Where(o => o.Sec == formDepth.TrElement.Security));
                });
                UpdateInfoOrders(orders);
            });
        }

        void EventStopOrders(IEnumerable<StopOrder> stOrders)
        {
            Qlog.CatchException(() =>
            {
                this.ForEachWinDepth((formDepth) =>
                {
                    formDepth.EventAnyOrder(stOrders.Where(o => o.Sec == formDepth.TrElement.Security));
                });

                this.FilteringStopOrders(stOrders);
                this.KilledSignalsByStopOrders(stOrders.ToArray());
            });
        }
        /// <summary>
        /// Снимает все сигналы по стоп ордерам
        /// </summary>
        /// <param name="stOrders"></param>
        private void KilledSignalsByStopOrders(StopOrder[] stOrders)
        {
            Qlog.CatchException(() =>
            {
                foreach (var ord in stOrders)
                {
                    if (ord.Status == OrderStatus.CLOSED)
                    {
                        var listSig = SignalView.GSMSignaler.GetSignalByOrder(ord.ConditionPrice, Define.STOP_LOSS);
                        if (listSig.NotIsNull() && listSig.Count() > 0)
                        {
                            foreach (var sig in listSig)
                            {
                                SignalView.GSMSignaler.RemoveSignal(sig);
                            };
                        }
                    }
                }
            });
        }
        void EventPositions(IEnumerable<Position> pos)
        {
            Qlog.CatchException(() =>
            {
                UpdateInfoPositions();
            });
        }
        void EventPortfolio(IEnumerable<Portfolio> portf)
        {
            Qlog.CatchException(() =>
            {
                UpdateInfoPortfolios();
            });
        }

        /// <summary>  События в стакане </summary>
        /// <param name="listQuote"></param>
        /*void EventDepth(IEnumerable<Quote> listQuote)
        {
            Qlog.CatchException(() =>
            {
                MThread.InitThread(() =>
                {
                    foreach (var q in listQuote)
                    {
                        var el = DataTrading.AddOrFind(q.Sec);
                        if (el.NotIsNull())
                        {
                            el.LastQuote.Object = q;
                        }
                    }
                });
            });
        }*/


        private void EventMyTrades(IEnumerable<MyTrade> myTrades)
        {
            Qlog.CatchException(() =>
            {
                foreach (var mTrade in myTrades)
                {
                    this.ForEachWinDepth((formDepth) =>
                    {
                        if (mTrade.Trade.Sec == formDepth.TrElement.Security)
                        {
                            formDepth.LoopControl(true);
                        }
                    });
                }
            });
        }


        private void EventNewSec(IEnumerable<Securities> listSec)
        {
            Qlog.CatchException(() =>
            {
                Securities last = null;
                foreach (var s in listSec)
                {
                    var el = DataTrading.AddOrFind(s);
                    last = s;
                }
                if (last.NotIsNull())
                {
                    ChangeTextMainStatusBar(Trader.Objects.tSecurities.Count + " " + last.ToString());
                }
            });
        }
        /// <summary>
        /// Изменение текста в главном статус баре.
        /// </summary>
        /// <param name="text"></param>
        public void ChangeTextMainStatusBar(string text, bool time = true)
        {
            statusStrip1.GuiAsync(() =>
            {
                toolStripStatusLabel1.Text = (time ? DateTime.Now.ToString() + ": " : "") + text;
            });
        }

        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Enabled = false;

            ChangeTextMainStatusBar("Синхронизация с терминалом...");

            Trader.Connect();
            Trader.Objects.tTrades.OnNew += new Events.EventsBase<Trade>.eventElement(EventAllTrades);
            Trader.Objects.tOldTrades.OnNew += new Events.EventsBase<Trade>.eventElement(EventOldTrades);

            //Trader.Objects.tSecurities.OnNew += new Events.EventsBase<Securities>.eventElement(EventNewSec);

            Trader.Objects.tMyTrades.OnNew += new Events.EventsBase<MyTrade>.eventElement(EventMyTrades);
            Trader.Objects.tOrders.OnNew += new Events.EventsBase<Order>.eventElement(EventOrders);
            Trader.Objects.tOrders.OnChange += new Events.EventsBase<Order>.eventElement(EventOrders);

            Trader.Objects.tStopOrders.OnNew += new Events.EventsBase<StopOrder>.eventElement(EventStopOrders);
            Trader.Objects.tStopOrders.OnChange += new Events.EventsBase<StopOrder>.eventElement(EventStopOrders);

            Trader.Objects.tPortfolios.OnNew += new Events.EventsBase<Portfolio>.eventElement(EventPortfolio);
            //Trader.Objects.tPortfolios.OnChange += new Events.EventsBase<Portfolio>.eventElement(EventPortfolio);

            Trader.Objects.tPositions.OnNew += new Events.EventsBase<Position>.eventElement(EventPositions);
            Trader.Objects.tPositions.OnChange += new Events.EventsBase<Position>.eventElement(EventPositions);

            Trader.Objects.tTransaction.OnTransReply += new ToolsTrans.eventTrans((listReply) =>
            {
                Qlog.CatchException(() =>
                {
                    if (listReply.Count() > 0)
                    {
                        if (OnNewReply.NotIsNull())
                        {
                            OnNewReply(listReply);
                        }
                        Reply r = listReply.Last();
                        if (r.NotIsNull())
                        {
                            this.LastMarketReply = r;
                            ChangeTextMainStatusBar(r.ResultMsg);
                        }
                    }
                });
            });
        }

        /// <summary>
        /// Выполняет какое либо действие для открытых окон
        /// </summary>
        /// <param name="action"></param>
        private void ForEachWinDepth(Action<Form_GraphicDepth> action)
        {
            Qlog.CatchException(() =>
            {
                if (ListFormsDepth.NotIsNull() && ListFormsDepth.Count > 0)
                {
                    foreach (var f in ListFormsDepth.ToArray())
                    {
                        if (f.TrElement.NotIsNull())
                        {
                            action(f);
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Инициализация сейвера
        /// </summary>
        private void SaveAll()
        {
            foreach (var el in DataTrading.Collection.ToArray())
            {
                if (el.NotIsNull()) { el.Save(); }
            }
        }

        DateTime Timer100ms = DateTime.Now;
        DateTime Timer1s = DateTime.Now;
        DateTime Timer3s = DateTime.Now;
        DateTime Timer5s = DateTime.Now;

        private void InitTimers()
        {
            EventHandler livingLoop = (s, e) =>
            {
                Qlog.CatchException(() =>
                {
                    var now = DateTime.Now;
                    //100ms
                    if (now > Timer100ms.AddMilliseconds(100))
                    {
                        if (OnTimer100ms.NotIsNull())
                        {
                            this.OnTimer100ms((DispatcherTimer)s);
                        }
                        Timer100ms = now;
                    }
                    //1 sec
                    if (now > Timer1s.AddSeconds(1))
                    {
                        if (OnTimer1s.NotIsNull())
                        {
                            this.OnTimer1s((DispatcherTimer)s);
                        }
                        Timer1s = now;
                    }
                    //3 sec
                    if (now > Timer3s.AddSeconds(3))
                    {
                        if (this.OnTimer3s.NotIsNull())
                        {
                            this.OnTimer3s((DispatcherTimer)s);
                        }
                        Timer3s = now;
                    }
                    // 5 sec
                    if (now > Timer5s.AddSeconds(5))
                    {
                        if (this.OnTimer5s.NotIsNull())
                        {
                            this.OnTimer5s((DispatcherTimer)s);
                        }
                        Timer5s = now;
                    }
                    EventStrategy();
                });
            };
            MTimer.InitTimer(new TimeSpan(100), livingLoop);

            OnTimer1s += (timer) =>
            {
                AutoOrdersLoopControl();
                AutoSLLoopControl();
                this.CheckAllSignals();
                this.ForEachWinDepth((formDepth) =>
                {
                    formDepth.FastUpdater();
                });
            };
            OnTimer3s += (timer) =>
            {
                Qlog.CatchException(() =>
                {
                    this.Trader.PingServer();
                    UpdateInfoPortfolios();
                    this.ForEachWinDepth((formDepth) =>
                    {
                        formDepth.LoopControl();
                    });
                });
            };
            OnTimer5s += (timer) =>
            {
                Qlog.CatchException(() =>
                {
                    UpdateInfoPositions();
                    ChangeTextMainStatusBar(Trader.Objects.tOldTrades.CountNew().ToString());
                });
            };
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Trader.Disconnect();
            Thread.Sleep(1);
            SaveAll();
            MTimer.StopAll();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ListFormsDepth.Count > 0)
            {
                MessageBox.Show("Закройте все открытые окна!");
                e.Cancel = true;
                return;
            }
            var result = MessageBox.Show(this, "Закрыть окно?", "Закрыть окно?",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK) e.Cancel = false;
            else
            {
                e.Cancel = true;
                return;
            }
        }

        private void DataGridPositions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Qlog.CatchException(() =>
            {
                var senderGrid = (DataGridView)sender;
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    var sec = senderGrid.Rows[e.RowIndex].Tag != null ? (Securities)senderGrid.Rows[e.RowIndex].Tag : null;
                    if (sec != null) ShowGraphicDepth(sec);
                }
            });
        }

        private void TextBoxSearchSec_TextChanged(object sender, EventArgs e)
        {
            var obj = (TextBox)sender;
            if (obj.Text.Length > 1)
            {
                var list = this.FindSecurity(obj.Text);
                if (list.NotIsNull() && list.Count() > 0)
                {
                    dataGridFoundSec.Rows.Clear();
                    list.ToList().ForEach((el) =>
                    {
                        var row = (DataGridViewRow)dataGridFoundSec.Rows[0].Clone();
                        row.Cells[0].Value = el.Code + ":" + el.Class.Code;
                        row.Cells[1].Value = el.Name;
                        row.Tag = el;
                        dataGridFoundSec.Rows.Add(row);
                    });
                }
                else dataGridFoundSec.Rows.Clear();
            }
            else
            {
                dataGridFoundSec.Rows.Clear();
            }
        }

        private void ButtonOpenFoundDepth_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridFoundSec.SelectedRows)
            {
                if (row.Tag.NotIsNull())
                {
                    var sec = (Securities)row.Tag;
                    this.ShowGraphicDepth(sec);
                }
            }
        }

        private void ToolStripMenuItem11_Click(object sender, EventArgs e)
        {
            var multiWindow = new Form_ManyGraphics(this);
            multiWindow.Show();
        }


        private void ToolStripMenuItemSpeedOrders_Click(object sender, EventArgs e)
        {
            if (FormSpeedOrders.NotIsNull())
            {
                FormSpeedOrders.Close();
                return;
            }
            FormSpeedOrders = new Form_LightOrders(this);
            if (FormSpeedOrders.NotIsNull())
            {
                FormSpeedOrders.FormClosed += (s, ee) => { this.FormSpeedOrders = null; };
                FormSpeedOrders.Show();
            }
        }
        private void ToolStripMenuItemTestSign_Click_1(object sender, EventArgs e)
        {
            SignalView.GSMSignaler.SendTestSignal();
            MThread.InitThread(() =>
            {
                Thread.Sleep(1000);
                SignalView.GSMSignaler.SendTestSignal(false);
            });
        }

        private void ToolStripMenuItemSignCall_Click(object sender, EventArgs e)
        {
            SignalView.GSMSignaler.SendSignalCall();
            MThread.InitThread(() =>
            {
                Thread.Sleep(15000);
                SignalView.GSMSignaler.SendSignalResetCall();
            });
        }

        /// <summary> Поиск инструметов </summary>
        /// <param name="codeOrName"></param>
        /// <returns></returns>
        public IEnumerable<Securities> FindSecurity(string codeOrName)
        {
            if (this.Trader.IsNull()) return null;
            if (this.Trader.Objects.tSecurities.Count > 0)
            {
                var list = this.Trader.Objects.tSecurities.ToArray().Where(s => s.Name.NotIsNull() && s.Code.NotIsNull() &&
                (s.Code.ToUpper().Contains(codeOrName.ToUpper()) || s.Name.ToUpper().Contains(codeOrName.ToUpper())));
                if (list.NotIsNull()) return list;
            }
            return null;
        }
        /// <summary> Поиск инструмета </summary>
        public Securities FindSecurity(string secCode, string classCode)
        {
            if (this.Trader.IsNull()) return null;
            if (this.Trader.Objects.tSecurities.Count > 0)
            {
                var sec = this.Trader.Objects.tSecurities.ToArray().FirstOrDefault(s => s.Code.ToUpper().Contains(secCode.ToUpper()) && s.Class.Code.ToUpper().Contains(classCode.ToUpper()));
                if (sec.NotIsNull()) return sec;
            }
            return null;
        }
        /// <summary> Возвращает торговый элемент, или null </summary>
        public TElement GetTradeElement(Securities sec)
        {
            return this.GetTradeElement(sec.Code, sec.Class.Code);
        }
        /// <summary> Возвращает торговый элемент, или null </summary>
        /// <param name="secCode"></param>
        /// <param name="classCode"></param>
        /// <returns></returns>
        public TElement GetTradeElement(string secCode, string classCode)
        {
            if (this.Trader.IsNull()) return null;
            if (this.DataTrading.IsNull()) return null;
            var TrElem = this.DataTrading.Collection.FirstOrDefault(tr => tr.Security.Code == secCode && tr.Security.Class.Code == classCode);
            return TrElem.NotIsNull() ? TrElem : null;
        }
        /// <summary> Получает объект позиций по инструменту</summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public Position GetPosition(Securities sec)
        {
            if (this.Trader.IsNull()) return null;
            if (this.Trader.Objects.tPositions.Count == 0) return null;

            var pos = this.Trader.Objects.tPositions.ToArray().FirstOrDefault(p => p.Sec.Code == sec.Code && p.Sec.Class.Code == sec.Class.Code);
            if (pos.NotIsNull()) return pos;
            return null;
        }



        /// <summary> Получает инструмент по коду инструмента и коду класса. </summary>
        /// <param name="codeSecAndClass">Формат строки SIH8:SPBFUT</param>
        public Securities GetSecCodeAndClass(string codeSecAndClass)
        {
            if (codeSecAndClass.Empty()) return null;
            string line = codeSecAndClass;
            if (!line.Empty())
            {
                string[] el = line.Split(':');
                if (el.Length > 1)
                {
                    if (!el[0].Empty() && !el[1].Empty())
                    {
                        var sec = this.Trader.Objects.tSecurities.ToArray().FirstOrDefault(s => s.Code == el[0] && s.Class.Code == el[1]);
                        if (sec.NotIsNull())
                        {
                            return sec;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Проверка, яв-ся рабочим данный инструмент.
        /// </summary>
        /// <param name="sec"></param>
        /// <param name="reloadSec"></param>
        /// <returns></returns>
        public bool CheckWorkingSec(Securities sec, bool reloadSec = false)
        {
            var list = Global.GetWorkingListSec(reloadSec);
            foreach (var el in list.ToArray())
            {
                if (el.ToUpper().Contains(sec.ToString().ToUpper()))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Проводит регистрацию всех активных инструментов
        /// </summary>
        public void RegisterAllSecurity()
        {
            var res = Qlog.CatchException(() =>
            {
                var list = Global.GetWorkingListSec();
                foreach (var el in list.ToArray())
                {
                    var sec = this.GetSecCodeAndClass(el);
                    if (sec.NotIsNull())
                    {
                        Trader.RegisterSecurities(sec);
                    }
                }
                return false;
            });
        }

        /// <summary>
        /// Проверка на клиринг. true - идет клиринг
        /// </summary>
        /// <returns></returns>
        public bool CheckKliring()
        {
            if (this.KliringDay.Time < DateTime.Now
                    && this.KliringDay.Time.AddMinutes(this.KliringDay.Period) > DateTime.Now) return true;
            if (this.KliringEndDay.Time < DateTime.Now
                    && this.KliringEndDay.Time.AddMinutes(this.KliringEndDay.Period) > DateTime.Now) return true;
            return false;
        }


        private void buttonSaveSecurity_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridFoundSec.SelectedRows)
            {
                if (row.Tag.NotIsNull())
                {
                    var sec = (Securities)row.Tag;
                    var el = this.DataTrading.AddOrFind(sec);
                    if (el.NotIsNull())
                    {
                        Trader.RegisterSecurities(el.Security);
                    }
                }
            }
        }

        private void buttonDescSave_Click(object sender, EventArgs e)
        {
            var rootDir = Global.GetPathData();
            var filename = rootDir + "/description.txt";
            WFile file = new WFile(filename);
            file.WriteFileNew(textBoxDescription.Text);
            labelDescription.Text = textBoxDescription.Text;
        }

        private void loadTextDescription()
        {
            var rootDir = Global.GetPathData();
            var filename = rootDir + "/description.txt";
            WFile file = new WFile(filename);
            labelDescription.Text = textBoxDescription.Text = file.ReadAll();
        }

        private void арбитражToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FormArbitration.IsNull())
            {
                FormArbitration = new Form_Arbitration(this);
                FormArbitration.Show();
                FormArbitration.FormClosed += (ss, ee) => { FormArbitration = null; };
            }
        }
    }
}

