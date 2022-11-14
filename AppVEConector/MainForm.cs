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
using AppVEConector.Forms.StopOrders;
using Libs;
using AppVEConector.libs.Signal;
using Market.AppTools;
using AppVEConector.Forms;
using QuikConnector.Components.Log;
using AppVEConector.Components;
using QuikConnector.Components.Controllers;

namespace AppVEConector
{
    public partial class MainForm : Form
    {
        /// <summary> Торгуемы набор </summary>
        public TCollection DataTrading = new TCollection();
        /// <summary>
        /// Активные инстурменты с которыми осуществляется работа, а так же те, что в портфеле.
        /// </summary>
        public ActiveStocks ActiveList = new ActiveStocks();

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
        readonly List<Form_GraphicDepth> ListFormsDepth = new List<Form_GraphicDepth>();

        /// <summary> Последнее сообщение по рынку </summary>
        public Reply LastMarketReply = null;

        private Form_Arbitration FormArbitration = null;

        //private TradeController ControlTrade = new TradeController();
        /// <summary> Форма быстрых ордеров </summary>
        private Form_LightOrders FormSpeedOrders = null;

        public Kliring KliringDay = null;
        public Kliring KliringEndDay = null;

        public MainForm()
        {
            var klirDay = QuikConnector.Connector.ConfSettings.GetParam("Kliring", "KlDay").Value;
            var klirEndDay = QuikConnector.Connector.ConfSettings.GetParam("Kliring", "KlEndDay").Value;
            KliringDay = new Kliring(klirDay);
            KliringEndDay = new Kliring(klirEndDay);
            InitializeComponent();
        }
        /// <summary>
        /// Поиск инструмента в обход демо и инфо инструментов.
        /// </summary>
        /// <param name="secCode"></param>
        /// <returns></returns>
        public Securities SearchSecurity(string secCode)
        {
            QLog.CatchException(() =>
            {
                var sec = Quik.Trader.Objects.tSecurities.ToArray().FirstOrDefault(s => s.Code.Contains(secCode)
                            && !s.Class.Code.Contains("INFO") && !s.Class.Code.Contains("EMU"));
                return sec;
            });
            return null;
        }
        public Securities SearchSecurity(string secCode, string secClassCode)
        {
            QLog.CatchException(() =>
            {
                var sec = Quik.Trader.Objects.tSecurities.ToArray().FirstOrDefault(s => s.Code.Contains(secCode)
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
            QLog.CatchException(() =>
            {
                var form = ListFormsDepth.FirstOrDefault(f => f.TrElement.Security == sec);
                if (form.IsNull())
                {
                    var elTr = DataTrading.AddOrFind(sec);
                    if (elTr.NotIsNull())
                    {
                        form = new Form_GraphicDepth(elTr, this);
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
            QLog.ActionError = (e) =>
            {
                return MessageBox.Show(e.ToString());
            };
            Form_MessageSignal.PForm = this;
            QLog.CatchException(() =>
            {
                dataGridPortfolios.Rows[0].Resizable = DataGridViewTriState.False;
                dataGridPositions.Rows[0].Resizable = DataGridViewTriState.False;
                dataGridViewStopOrders.Rows[0].Resizable = DataGridViewTriState.False;

                Quik.Trader.RegisterAllParamSec();

                InitTimers();

                InitPanelSignal();
                InitPanelFastGap();
                InitAutoOrders();
                InitAutoStopLoss();


                //Quik.Trader.Objects.OnRun += () =>
                //{
                //    RegisterAllSecurity();
                //};
                //Открытие торгового окна по найденному инструменту, по двойному щелчку
                dataGridFoundSec.DoubleClick += (s, e) =>
                {
                    foreach (DataGridViewRow row in dataGridFoundSec.SelectedRows)
                    {
                        if (row.Tag.NotIsNull())
                        {
                            var sec = (Securities)row.Tag;
                            ShowGraphicDepth(sec);
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
                                var sec = GetSecByCode(infoPos);
                                if (sec.NotIsNull())
                                {
                                    ShowGraphicDepth(sec);
                                }
                            }
                        }
                    }
                };
                LoadTextDescription();
                WindowState = FormWindowState.Maximized;
            }, "");
        }


        /// /////////////////////////////////////////////////////////////////////////
        readonly List<DataGridViewRow> listRowsPortfolios = new List<DataGridViewRow>();
        private void UpdateInfoPortfolios()
        {
            int i = 0;
            var listPortf = Quik.Trader.Objects.tPortfolios.ToArray();//.Where(p => p.TypeClient == 2);
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
        readonly List<DataGridViewRow> listRowsPositions = new List<DataGridViewRow>();

        void UpdateInfoPositions()
        {
            if (Quik.Trader.Objects.tPositions.Count > 0)
            {
                dataGridPositions.GuiAsync(() =>
                {
                    var listPos = Quik.Trader.Objects.tPositions.ToArray().OrderBy(p => p.Sec.ToString());
                    string lastSec = null;
                    foreach (var p in listPos)
                    {
                        if (p.Sec.NotIsNull() && lastSec != p.ToString())
                        {
                            lastSec = p.ToString();
                            var row = listRowsPositions.FirstOrDefault(r => r.Tag.ToString() == p.Sec.ToString());
                            if (row.IsNull())
                            {
                                var newRow = (DataGridViewRow)dataGridPositions.Rows[0].Clone();
                                newRow.Cells[0].Value = "";
                                listRowsPositions.Add(newRow);
                                row = newRow;
                                Quik.Trader.RegisterSecurities(p.Sec);
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

                                row.Tag = p.Sec.ToString();
                                row.Cells[0].Value = p.Sec.Name;
                                row.Cells[1].Value = p.Client.NotIsNull() ? p.Client.Code + " " + types : "";
                                row.Cells[2].Value = p.Sec.ToString();
                                row.Cells[3].Value = p.Sec.Lot.ToString();
                                row.Cells[4].Value = p.Sec.StepPrice.ToString();
                                row.Cells[5].Value = p.Sec.Params.BuyDepo.ToString();
                                row.Cells[6].Value = curPos;// p.Data.CurrentNet.ToString();
                                row.Cells[6].ColorMarket(p.Data.CurrentNet);
                                //Orders
                                row.Cells[7].Value = p.Data.OrdersBuy.ToString() + " / " + p.Data.OrdersSell.ToString();
                                //Var margin
                                row.Cells[8].Value = p.Data.VarMargin.ToString();
                                row.Cells[8].ColorMarket(p.Data.VarMargin);
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

        /// /////////////////////////////////////////////////////////////////////////
        readonly List<DataGridViewRow> listRowsOrders = new List<DataGridViewRow>();
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
        readonly List<DataGridViewRow> listRowsStopOrders = new List<DataGridViewRow>();
        void ResetTableStopOrders()
        {
            QLog.CatchException(() =>
            {
                listRowsStopOrders.Clear();
                dataGridViewStopOrders.Rows.Clear();
                FilteringStopOrders(Quik.Trader.Objects.tStopOrders.ToArray());
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
                UpdateInfoStopOrders(listFilter);
            }
            else
            {
                UpdateInfoStopOrders(stOrders);
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

            Trade lastT = trades.Last();
            this.GuiAsync(() =>
            {
                labelLastTrade.Text =
                lastT.DateTrade.GetDateTime().ToLongTimeString() + " " +
                lastT.SecCode + " " +
                lastT.Price + " (" + lastT.Volume + ") " + lastT.Direction;
            });
        }
        private void EventAllTrades(IEnumerable<Trade> trades)
        {
            QLog.CatchException(() =>
            {
                UpdateInfoAllTrades(trades);
            });
        }


        private void EventOldTrades(IEnumerable<Trade> trades)
        {
            QLog.CatchException(() =>
            {
                foreach (var t in trades)
                {
                    var el = DataTrading.AddOrFind(t.Sec);
                    if (el.NotIsNull())
                    {
                        el.NewTrade(t);
                    }
                }
                Trade lastT = trades.Last();
                this.GuiAsync(() =>
                {
                    labelLastOldTrade.Text =
                        lastT.DateTrade.GetDateTime().ToLongTimeString() + " " +
                        lastT.SecCode + " " +
                        lastT.Price + " (" + lastT.Volume + ") " + lastT.Direction;
                    labelCountOldTrade.Text = Quik.Trader.Objects.tOldTrades.Count.ToString();
                });
            });
        }
        private void EventOrders(IEnumerable<Order> orders)
        {
            QLog.CatchException(() =>
            {
                ForEachWinDepth((formDepth) =>
                {
                    formDepth.EventAnyOrder(orders.Where(o => o.Sec == formDepth.TrElement.Security));
                });
                UpdateInfoOrders(orders);
            });
        }

        void EventStopOrders(IEnumerable<StopOrder> stOrders)
        {
            QLog.CatchException(() =>
            {
                ForEachWinDepth((formDepth) =>
                {
                    formDepth.EventAnyOrder(stOrders.Where(o => o.Sec == formDepth.TrElement.Security));
                });

                FilteringStopOrders(stOrders);
                KilledSignalsByStopOrders(stOrders.ToArray());
            });
        }
        /// <summary>
        /// Снимает все сигналы по стоп ордерам
        /// </summary>
        /// <param name="stOrders"></param>
        private void KilledSignalsByStopOrders(StopOrder[] stOrders)
        {
            QLog.CatchException(() =>
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
            QLog.CatchException(() =>
            {
                foreach (var p in pos)
                {
                    ActiveList.Add(p.Sec);
                }
                UpdateInfoPositions();
            });
        }
        void EventPortfolio(IEnumerable<Portfolio> portf)
        {
            QLog.CatchException(() =>
            {
                UpdateInfoPortfolios();
            });
        }

        /// <summary>  События в стакане </summary>
        /// <param name="listQuote"></param>
        /*void EventDepth(IEnumerable<Quote> listQuote)
        {
            QLog.CatchException(() =>
            {
                ThreadsController.Thread(() =>
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
            QLog.CatchException(() =>
            {
                foreach (var mTrade in myTrades)
                {
                    ForEachWinDepth((formDepth) =>
                    {
                        if (mTrade.Trade.Sec == formDepth.TrElement.Security)
                        {
                            formDepth.LoopControl(true);
                        }
                    });
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

            Quik.Trader.Connect();
            Quik.Trader.Objects.tTrades.OnNew += EventAllTrades;
            Quik.Trader.Objects.tOldTrades.OnNew += EventOldTrades;

            //Quik.Trader.Objects.tSecurities.OnNew += new Events.EventsBase<Securities>.eventElement(EventNewSec);

            Quik.Trader.Objects.tMyTrades.OnNew += EventMyTrades;
            Quik.Trader.Objects.tOrders.OnNew += EventOrders;
            Quik.Trader.Objects.tOrders.OnChange += EventOrders;

            Quik.Trader.Objects.tStopOrders.OnNew += EventStopOrders;
            Quik.Trader.Objects.tStopOrders.OnChange += EventStopOrders;

            Quik.Trader.Objects.tPortfolios.OnNew += EventPortfolio;
            //Quik.Trader.Objects.tPortfolios.OnChange += new Events.EventsBase<Portfolio>.eventElement(EventPortfolio);

            Quik.Trader.Objects.tPositions.OnNew += EventPositions;
            Quik.Trader.Objects.tPositions.OnChange += EventPositions;

            Quik.Trader.Objects.tTransaction.OnTransReply += new ToolsTrans.eventTrans((listReply) =>
            {
                QLog.CatchException(() =>
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
                            LastMarketReply = r;
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
            QLog.CatchException(() =>
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


        private void InitTimers()
        {
            TimersController.Timer(new TimeSpan(100), (s, e) =>
            {
                if (OnTimer100ms.NotIsNull())
                {
                    OnTimer100ms((DispatcherTimer)s);
                }
            });
            TimersController.Timer(new TimeSpan(0, 0, 1), (s, e) =>
            {
                if (OnTimer1s.NotIsNull())
                {
                    OnTimer1s((DispatcherTimer)s);
                }
            });

            TimersController.Timer(new TimeSpan(0, 0, 3), (s, e) =>
            {
                if (OnTimer3s.NotIsNull())
                {
                    OnTimer3s((DispatcherTimer)s);
                }
            });

            TimersController.Timer(new TimeSpan(0, 0, 5), (s, e) =>
            {
                if (OnTimer5s.NotIsNull())
                {
                    OnTimer5s((DispatcherTimer)s);
                }
            });
            //Проверка на отключение терминала
            TimersController.Timer(new TimeSpan(0, 0, 30), (s, e) =>
            {
                if (Quik.Trader.Connected && !Quik.Trader.Objects.Terminal.Connect)
                {
                    SignalView.GSMSignaler.SendSignalCall();
                }
            });

            OnTimer100ms += (timer) =>
            {
                EventStrategy();
            };

            OnTimer1s += (timer) =>
            {
                AutoOrdersLoopControl();
                AutoSLLoopControl();
                CheckAllSignals();
                ForEachWinDepth((formDepth) =>
                {
                    formDepth.FastUpdater();
                });
            };
            OnTimer3s += (timer) =>
            {
                QLog.CatchException(() =>
                {
                    Quik.Trader.PingServer();
                    UpdateInfoPortfolios();
                    ForEachWinDepth((formDepth) =>
                    {
                        formDepth.LoopControl();
                    });
                });
            };
            OnTimer5s += (timer) =>
            {
                QLog.CatchException(() =>
                {
                    UpdateInfoPositions();
                    ChangeTextMainStatusBar(Quik.Trader.Objects.tOldTrades.Count.ToString());
                });
            };
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Quik.Trader.Disconnect();
            Thread.Sleep(1);
            SaveAll();
            TimersController.StopAll();
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
            QLog.CatchException(() =>
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
                var list = FindSecurity(obj.Text);
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
                    ShowGraphicDepth(sec);
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
                FormSpeedOrders.FormClosed += (s, ee) => { FormSpeedOrders = null; };
                FormSpeedOrders.Show();
            }
        }
        private void ToolStripMenuItemTestSign_Click_1(object sender, EventArgs e)
        {
            SignalView.GSMSignaler.SendTestSignal();
            ThreadsController.Thread(() =>
            {
                Thread.Sleep(1000);
                SignalView.GSMSignaler.SendTestSignal(false);
            });
        }

        private void ToolStripMenuItemSignCall_Click(object sender, EventArgs e)
        {
            SignalView.GSMSignaler.SendSignalCall();
            ThreadsController.Thread(() =>
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
            if (Quik.Trader.IsNull())
            {
                return null;
            }
            if (Quik.Trader.Objects.tSecurities.Count > 0)
            {
                return Searcher.Stock(Quik.Trader.Objects.tSecurities.ToArray(), codeOrName);
            }
            return null;
        }
        /// <summary> Поиск инструмета </summary>
        public Securities FindSecurity(string secCode, string classCode)
        {
            if (Quik.Trader.IsNull()) return null;
            if (Quik.Trader.Objects.tSecurities.Count > 0)
            {
                var sec = Quik.Trader.Objects.tSecurities.ToArray().FirstOrDefault(s => s.Code.ToUpper().Contains(secCode.ToUpper()) && s.Class.Code.ToUpper().Contains(classCode.ToUpper()));
                if (sec.NotIsNull()) return sec;
            }
            return null;
        }
        /// <summary> Возвращает торговый элемент, или null </summary>
        public TElement GetTradeElement(Securities sec)
        {
            return GetTradeElement(sec.Code, sec.Class.Code);
        }
        /// <summary> Возвращает торговый элемент, или null </summary>
        /// <param name="secCode"></param>
        /// <param name="classCode"></param>
        /// <returns></returns>
        public TElement GetTradeElement(string secCode, string classCode)
        {
            if (Quik.Trader.IsNull()) return null;
            if (DataTrading.IsNull()) return null;
            var TrElem = DataTrading.Collection.FirstOrDefault(tr => tr.Security.Code == secCode && tr.Security.Class.Code == classCode);
            return TrElem.NotIsNull() ? TrElem : null;
        }
        /// <summary> Получает объект позиций по инструменту</summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public Position GetPosition(Securities sec)
        {
            if (Quik.Trader.IsNull() || Quik.Trader.Objects.tPositions.Count == 0)
            {
                return null;
            }
            var pos = Quik.Trader.Objects.tPositions.ToArray().FirstOrDefault(p => p.Sec.Code == sec.Code && p.Sec.Class.Code == sec.Class.Code);
            return pos.NotIsNull() ? pos : null;
        }


        public Securities GetSecByCode(string codeAndClass)
        {
            if (codeAndClass.Empty())
            {
                return null;
            }
            return Searcher.StockByCode(Quik.Trader.Objects.tSecurities.ToArray(), codeAndClass);
        }

        /// <summary>
        /// Проводит регистрацию всех активных инструментов
        /// </summary>
        //public void RegisterAllSecurity()
        //{
        //    var res = QLog.CatchException(() =>
        //    {
        //        var list = Global.GetWorkingListSec();
        //        foreach (var el in list.ToArray())
        //        {
        //            var sec = GetSecCodeAndClass(el);
        //            if (sec.NotIsNull())
        //            {
        //                Trader.RegisterSecurities(sec);
        //            }
        //        }
        //        return false;
        //    });
        //}

        /// <summary>
        /// Проверка на клиринг. true - идет клиринг
        /// </summary>
        /// <returns></returns>
        public bool CheckKliring()
        {
            if (KliringDay.Time < DateTime.Now
                    && KliringDay.Time.AddMinutes(KliringDay.Period) > DateTime.Now) return true;
            if (KliringEndDay.Time < DateTime.Now
                    && KliringEndDay.Time.AddMinutes(KliringEndDay.Period) > DateTime.Now) return true;
            return false;
        }


        private void buttonSaveSecurity_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridFoundSec.SelectedRows)
            {
                if (row.Tag.NotIsNull())
                {
                    var sec = (Securities)row.Tag;
                    var el = DataTrading.AddOrFind(sec);
                    if (el.NotIsNull())
                    {
                        Quik.Trader.RegisterSecurities(el.Security);
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
        }

        private void LoadTextDescription()
        {
            var rootDir = Global.GetPathData();
            var filename = rootDir + "/description.txt";
            WFile file = new WFile(filename);
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

