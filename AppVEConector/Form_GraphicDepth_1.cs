using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MarketObjects;
using System.Threading;
using TradingLib;
using AppVEConector.libs;
using Market.Candles;
using GraphicTools;
using Connector.Logs;
using Managers;
using libs;
using QuikConnector.libs;
using QuikConnector.MarketObjects;

namespace AppVEConector
{
    public partial class Form_GraphicDepth : Form
    {
        /// <summary>
        /// Портфель
        /// </summary>
        public Portfolio Portfolio = null;
        /// <summary>
        /// Position
        /// </summary>
        public Position Position = null;

        /// <summary>
        /// Обьект графика
        /// </summary>
        public Graphic GraphicStock = null;

        /// <summary> Кол-во свечей на графике (Масштаб) </summary>
        private int CountCandleInGraphic = 50;

        /// <summary> Значение текущего тайм-фрейма (в минутах) </summary>
        private int CurrentTimeFrameValue
        {
            get { return SettingsDepth.Data.CurrentTimeFrame == 0 ? 1 : SettingsDepth.Data.CurrentTimeFrame; }
            set { SettingsDepth.Data.CurrentTimeFrame = value; }
        }

        /// <summary> Текущего тайм-фрейм </summary>
        private CandleCollection CurrentTimeFrame
        {
            get
            {
                if (this.TrElement.CollectionTimeFrames.Count == 0) return null;
                return this.TrElement.CollectionTimeFrames.FirstOrDefault(tf => tf.TimeFrame == this.CurrentTimeFrameValue);
            }
        }

        /// <summary> Наполнитель стакана ПРОДАЖА. Сбрасывается при новом стакане.</summary>
        private DataGridViewRow[] ArraySell = null;
        /// <summary> Наполнитель стакана ПОКУПКА. Сбрасывается при новом стакане.</summary>
        private DataGridViewRow[] ArrayBuy = null;


        public Form_Strategy_morePrev FormStrategy = null;

        /// <summary>
        /// Все настройки, которые необходимо хранить.
        /// </summary>
        private Settings SettingsDepth = new Settings();

        class StructClickDepth
        {
            public string Flag = null;
            public decimal Price = -1;
            public decimal Volume = -1;
        }

        /// <summary>
        /// Обновляем сигналы для графика
        /// </summary>
        private void InitGraphicSignals()
        {
            Action updateSingnals = () =>
            {
                GraphicStock.LevelsSignal.SetLevels(
                        MainForm.GSMSignaler.ToArray().
                        Where(s => s.SecClass == Securities.ToString()).
                        Select(s => s.Price).ToArray()
                    );
                UpdateGraphic();
            };
            MainForm.GSMSignaler.OnAdd += (sig) =>
            {
                updateSingnals();
            };
            MainForm.GSMSignaler.OnRemove += (sig) =>
            {
                updateSingnals();
            };
            updateSingnals();
        }

        /// <summary> Инициализация панели очистки сделок за определенную дату </summary>
        private void InitPanelClearCharts()
        {
            DateMarket dateStart = new DateMarket();
            DateMarket dateEnd = new DateMarket();

            dateTimePickerClearChartsStart.ValueChanged += (s, e) =>
            {
                this.labelClearStart.Text = dateTimePickerClearChartsStart.Value.ToShortDateString() + " " +
                dateTimePickerClearChartsStart.Value.ToShortTimeString();
            };
            dateTimePickerClearChartsEnd.ValueChanged += (s, e) =>
            {
                this.labelClearEnd.Text = dateTimePickerClearChartsEnd.Value.ToShortDateString() + " " +
                dateTimePickerClearChartsEnd.Value.ToShortTimeString();
            };

            dateStart.SetDateTime(DateTime.Now.AddDays(-1));
            dateStart.SetHour(0).SetMinute(0).SetSecond(0);
            dateTimePickerClearChartsStart.Value = dateStart.GetDateTime();

            dateEnd.SetDateTime(DateTime.Now);
            dateEnd.SetHour(23).SetMinute(59).SetSecond(59);
            dateTimePickerClearChartsEnd.Value = dateEnd.GetDateTime();

            buttonClearCandle.Click += (s, e) =>
            {
                var dateClearStart = dateTimePickerClearChartsStart.Value;
                var dateClearEnd = dateTimePickerClearChartsEnd.Value;
                var result = MessageBox.Show(this, "Удалить сделки за " + dateClearStart.ToLongDateString() + " - " + dateClearEnd.ToLongDateString(), "Удаление котировок?",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    this.DeleteCharts(dateClearStart, dateClearEnd);
                }
            };

            ComboBox.ObjectCollection itemsField = new ComboBox.ObjectCollection(comboBoxLoadField);
            itemsField.Add("Price");
            itemsField.Add("Volume");
            comboBoxLoadField.DataSource = itemsField;

            ComboBox.ObjectCollection itemsOper = new ComboBox.ObjectCollection(comboBoxLoadOper);
            itemsOper.Add("*");
            itemsOper.Add("/");
            comboBoxLoadOper.DataSource = itemsOper;

        }

        private void InitAllPanels()
        {
            //Инициализация доп панелей
            InitPanelLevels();
            InitPanelClearCharts();
            InitPanelControl();

            //Уровни
            UpdatePanelLevels(true);
        }

        /// <summary> Инициализация нового инструмента в текущем окне. Сброс на новый инструмент.</summary>
        private void InitReset()
        {
            this.SetHead(Securities);
            //Загружаем настройки по инструменту
            SettingsDepth.ReloadSecurity(Securities.ToString());
            SettingsDepth.InitTimeFrame(TrElement.CollectionTimeFrames.Select(t => t.TimeFrame));

            //Инициализация всех панелей
            InitAllPanels();

            //Регистрация стакана
            Trader.RegisterDepth(Securities);


            //цена для заявки
            numericUpDownPrice.InitSecurity(Securities);
            numericUpDownPrice.Value = 0;
            //Стоп цена
            numericUpDownStopPrice.InitSecurity(Securities);
            numericUpDownStopPrice.Value = 0;
            //Дата эксписрациия стоп заявки
            dateTimePickerStopOrder.Value = DateTime.Now.AddDays(SettingsDepth.Data.CountDaysForStopOrder);
            //Получаем значение последней стоп-заявки
            if (LastStopOrder.NotIsNull())
            {
                numericUpDownStopPrice.Value = LastStopOrder.ConditionPrice;
                dateTimePickerStopOrder.Value = DateTime.Now.AddDays(SettingsDepth.Data.CountDaysForStopOrder);
            }

            //Настройки графика
            this.GraphicStock = new Graphic();
            this.GraphicStock.InitEvents();
            this.GraphicStock.SetObjectPaint(this.pictureBoxGraphic);

            this.GraphicStock.Init(Securities);
            this.GraphicStock.DataSourceTimeFrame = this.CurrentTimeFrame;
            this.GraphicStock.DataSourceLevels = this.Levels.ObjectCollection;
            this.GraphicStock.ResetActiveCandles();
            UpdateGraphic();

            InitGraphicSignals();

            this.GraphicStock.Levels.OnNewLevel += (newLevel) =>
            {
                this.Levels.Add(newLevel);
                this.UpdatePanelLevels();
            };

            this.GraphicStock.OnCandleLeftMouseUp += (point, candle) =>
            {
                ResetButtonLevels();
            };

            //Скрол для графика
            hScrollGraphic.Value = hScrollGraphic.Maximum;

            //Clear depth
            dataGridViewDepth.Rows.Clear();
            this.ArraySell = null;
            this.ArrayBuy = null;

            //Code client
            comboBoxCodeClient.SelectedIndex = comboBoxCodeClient.FindStringExact(SettingsDepth.Data.CodeClient);
            //Кол-во видимых свечей
            this.CountCandleInGraphic = SettingsDepth.Data.TimeFrame[CurrentTimeFrameValue].CountVisibleCandle;

            EventAnyOrder(null, true);

            pictureBoxGraphic.Refresh();
        }

        /// <summary> Быстрый тафмерного-обновителя </summary>
        public void FastUpdater()
        {
            UpdateDepth();
            prepareInfoForForm();
            UpdateInfoForm();
            ControlsParams();
            UpdateGraphicFast();
        }
        /// <summary> Управление параметрами торгового окна</summary>
        private void ControlsParams()
        {
            if (this.checkBoxCancelStop.Checked)
            {
                var pos = this.Trader.Objects.Positions.FirstOrDefault(p => p.Sec.Code == Securities.Code);
                var stopLoss = this.Trader.Objects.StopOrders.FirstOrDefault(so => so.Sec.Code == Securities.Code && so.IsActive());
                if (pos.NotIsNull() && stopLoss.NotIsNull())
                {
                    if (pos.CurrentVolume == 0)
                    {
                        this.Trader.CancelAllStopOrder(Securities);
                    }
                }
            }
        }

        private Thread ThreadInfo = null;
        private BaseInfoMarket Info = new BaseInfoMarket();
        /// <summary>
        /// Подготовка данных иструмента для форм
        /// </summary>
        private void prepareInfoForForm()
        {
            if (ThreadInfo.NotIsNull())
            {
                ThreadInfo.Abort();
                ThreadInfo = null;
                return;
            }
            if (comboBoxCodeClient.SelectedItem.NotIsNull())
            {
                var listPortf = Trader.Objects.Portfolios.Where(p => p.Account.AccClasses.FirstOrDefault(c => c == Securities.Class).NotIsNull() &&
                        p.Client.Code == comboBoxCodeClient.SelectedItem.ToString());
                if (SettingsDepth.Data.TypeClientLimit >= 0)
                {
                    listPortf = listPortf.Where(p => p.LimitKind == SettingsDepth.Data.TypeClientLimit);
                }
                if (listPortf.Count() > 0)
                {
                    Portfolio = listPortf.First();
                }
            }
            if (SettingsDepth.Data.CodeClient.Empty())
            {
                Position = Trader.Objects.Positions.FirstOrDefault(s => s.Sec == Securities);
            }
            else
            {
                Position = Trader.Objects.Positions.FirstOrDefault(s => s.Sec == Securities
                    && s.Client.Code == SettingsDepth.Data.CodeClient);
            }
            ThreadInfo = MThread.InitThread(() =>
            {
                if (this.Position.NotIsNull())
                {
                    Info.CountCurrentPosition = this.Position.Data.CurrentNet;
                }
                if (Securities.LastPrice != 0)
                {
                    Info.CurrentPrice = Securities.LastPrice;
                }
                //orders
                var orders = this.Trader.Objects.Orders.Where(so => so.Sec.Code == Securities.Code && so.IsActive());
                var ordB = orders.Where(o => o.IsBuy());
                var ordS = orders.Where(o => o.IsSell());
                Info.CountOrderBuy = ordB.Count();
                Info.CountOrderSell = ordS.Count();
                Info.CountVolumeBuy = ordB.Sum(o => o.Balance);
                Info.CountVolumeSell = ordS.Sum(o => o.Balance);

                var stopOrdersActive = this.Trader.Objects.StopOrders.Where(so => so.Sec.Code == Securities.Code && so.IsActive());
                if (stopOrdersActive.NotIsNull())
                {
                    var soBuy = stopOrdersActive.Where(so => so.IsBuy());
                    var soSell = stopOrdersActive.Where(so => so.IsSell());
                    var soOrderBuy = soBuy.Where(so => so.Comment.Contains(Define.STOP_LIMIT));
                    var soOrderSell = soSell.Where(so => so.Comment.Contains(Define.STOP_LIMIT));

                    var soLimitBuy = soBuy.Where(so => so.Comment.Contains(Define.STOP_LOSS));
                    var soLimitSell = soSell.Where(so => so.Comment.Contains(Define.STOP_LOSS));

                    //sLimit
                    Info.CountStopLimitOrderBuy = soLimitBuy.Count();
                    Info.CountStopLimitOrderSell = soLimitSell.Count();
                    if (Info.CountStopLimitOrderBuy > 0 || Info.CountStopLimitOrderSell > 0)
                    {
                        Info.CountStopLimitVolumeBuy = soLimitBuy.Sum(so => so.Volume);
                        Info.CountStopLimitVolumeSell = soLimitSell.Sum(so => so.Volume);
                    }
                    //sOrder
                    Info.CountStopOrderBuy = soOrderBuy.Count();
                    Info.CountStopOrderSell = soOrderSell.Count();
                    if (Info.CountStopOrderBuy > 0 || Info.CountStopOrderSell > 0)
                    {
                        Info.CountStopVolumeBuy = soOrderBuy.Sum(so => so.Volume);
                        Info.CountStopVolumeSell = soOrderSell.Sum(so => so.Volume);
                    }

                    if (Info.CountCurrentPosition != 0)
                    {
                        var stopOrder = stopOrdersActive.FirstOrDefault(so => so.IsActive());
                        if (stopOrder.NotIsNull()
                            && Securities.LastTrade.NotIsNull()
                            && Securities.Params.MinPriceStep > 0)
                        {
                            var priceStop = stopOrder.ConditionPrice;
                            decimal gapPrice = priceStop > Info.CurrentPrice ? priceStop - Info.CurrentPrice : Info.CurrentPrice - priceStop;
                            decimal result = (gapPrice / Securities.Params.MinPriceStep) * Securities.Params.StepPrice;
                            decimal countLots = Position.CurrentVolume;
                            Info.ForecastSum = result * countLots;
                        } else
                        {
                            Info.ForecastSum = 0;
                        }
                    }
                }

                if (Portfolio.NotIsNull())
                {
                    Info.Balanse = this.Portfolio.Balance;
                    Info.FreeBalanse = this.Portfolio.CurrentBalance;
                    Info.VarMargin = this.Portfolio.VarMargin;
                }

                ThreadInfo = null;
            });
        }
        /// <summary> Обновляем формы с иформацией </summary>
        public void UpdateInfoForm()
        {
            Qlog.CatchException(() =>
            {
                labelStopOrders.Text = "0/0";
                labelStopLimitOrders.Text = "0/0";
                labelForecastStop.Text = "0.00";
                buttonExitPos.Text = "0";
                labelOrdersBuy.Text = "0";
                labelOrdersSell.Text = "0";
                //stop order
                if (Info.CountStopOrderBuy > 0 || Info.CountStopOrderSell > 0)
                {
                    labelStopOrders.Text = (Info.CountStopOrderBuy > 0 ? Info.CountStopOrderBuy + " (" + Info.CountStopVolumeBuy + ")" : "0") + "/"
                    + (Info.CountStopOrderSell > 0 ? Info.CountStopOrderSell + " (" + Info.CountStopVolumeSell + ")" : "0");
                }
                //stop limit
                if (Info.CountStopLimitOrderBuy > 0 || Info.CountStopLimitOrderSell > 0)
                {
                    labelStopLimitOrders.Text = (Info.CountStopLimitOrderBuy > 0 ? Info.CountStopLimitOrderBuy + " (" + Info.CountStopLimitVolumeBuy + ")" : "0") + "/"
                    + (Info.CountStopLimitOrderSell > 0 ? Info.CountStopLimitOrderSell + " (" + Info.CountStopLimitVolumeSell + ")" : "0");
                }
                //forecast
                if (Info.ForecastSum >= 0)
                {
                    labelForecastStop.Text = Info.ForecastSum.ToString();
                }
                //cur pos
                if (Info.CountCurrentPosition != 0)
                {
                    buttonExitPos.Text = Info.CountCurrentPosition.ToString();
                }
                //balanse buy
                if (Info.CountVolumeBuy > 0)
                {
                    labelOrdersBuy.Text = Info.CountVolumeBuy.ToString();
                }
                //balanse buy
                if (Info.CountVolumeSell > 0)
                {
                    labelOrdersSell.Text = Info.CountVolumeSell.ToString();
                }
                //balanses
                labelStatBalance.Text = Info.Balanse.ToString() + " / " + Info.FreeBalanse.ToString();
                //margin
                labelVarMargin.Text = Info.VarMargin.ToString();

                labelStatBidAsk.Text = Securities.Params.SumBidDepth.ToString() + " / " + Securities.Params.SumAskDepth.ToString();
                if (Securities.Params.SumBidDepth > Securities.Params.SumAskDepth)
                {
                    labelStatBidAsk.BackColor = Color.LightGreen;
                }
                else
                {
                    labelStatBidAsk.BackColor = Color.LightCoral;
                }

                labelPriceStep.Text = Securities.Params.StepPrice.ToString();
                labelSizeLots.Text = Securities.Lot.ToString();

                labelStatOrdBid.Text = Securities.Params.NumBid.ToString() + " / " + Securities.Params.NumAsk.ToString();
                if (Securities.Params.NumBid > Securities.Params.NumAsk)
                {
                    labelStatOrdBid.BackColor = Color.LightGreen;
                }
                else
                {
                    labelStatOrdBid.BackColor = Color.LightCoral;
                }
                labelGaranty.Text = Securities.Params.BuyDepo.ToString();

                if (GraphicStock.ATR.NotIsNull())
                {
                    labelATR.Text = Math.Round(GraphicStock.ATR.Value, Securities.Scale).ToString();
                }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Quote createQuote(decimal step = 1, int countView = 20)
        {
            var quote = new Quote();
            quote.Sec = Securities;
            quote.Ask = new Quote.QuoteRow[countView];
            quote.Bid = new Quote.QuoteRow[countView];
            for (int i = 0; i < countView; i++)
            {
                Quote.QuoteRow rowBid = new Quote.QuoteRow();
                Quote.QuoteRow rowAsk = new Quote.QuoteRow();
                rowBid.Price = Securities.LastPrice - Securities.MinPriceStep * i * step;
                rowBid.Volume = 1;
                rowAsk.Price = Securities.LastPrice + Securities.MinPriceStep * i * step;
                rowAsk.Volume = 1;
                quote.Bid[countView - i - 1] = rowBid;
                quote.Ask[i] = rowAsk;
            }
            return quote;
        }

        /// <summary> Сумма объема в стакане, 0 - bid, 1 - ask</summary>
        public long[] CountInDepth = new long[2];
        /// <summary> Обновление стакана </summary>
        public void UpdateDepth()
        {
            if (TrElement.NotIsNull())
            {
                var quote = Securities.GetQuote();
                if (quote.IsNull())
                {
                    quote = createQuote(numericUpDownStepDepth.Value);
                }
                int countS = 20;
                int countB = 20;
                var QuoteBid = quote.Bid.ToArray();
                var QuoteAsk = quote.Ask.ToArray();
                int countInDepthSell = QuoteAsk.Length;
                int countInDepthBuy = QuoteBid.Length;

                if (ArraySell.IsNull() && countInDepthSell > 0)
                {
                    ArraySell = new DataGridViewRow[countS];
                    for (int i = countS - 1; i >= 0; i--)
                    {
                        var k = dataGridViewDepth.Rows.Add();
                        ArraySell[i] = dataGridViewDepth.Rows[k];
                        ArraySell[i].Cells[2].Style.BackColor = Color.LightCoral;
                    }

                }
                if (ArrayBuy.IsNull() && countInDepthBuy > 0)
                {
                    ArrayBuy = new DataGridViewRow[countB];
                    for (int i = 0; i < countB; i++)
                    {
                        var k = dataGridViewDepth.Rows.Add();
                        ArrayBuy[i] = dataGridViewDepth.Rows[k];
                        ArrayBuy[i].Cells[2].Style.BackColor = Color.LightGreen;
                    }
                }
                CountInDepth[0] = 0;
                CountInDepth[1] = 0;
                MThread.InitThread(() =>
                {
                    //Синхронизуируем между потоками
                    dataGridViewDepth.GuiAsync(() =>
                    {
                        if (ArraySell.NotIsNull())
                        {
                            //Наполняем Sell
                            for (int i = 0; i < countS; i++)
                            {
                                if (countInDepthSell <= i && ArraySell[i].NotIsNull())
                                {
                                    ArraySell[i].Cells[0].Value = "";
                                    ArraySell[i].Cells[1].Value = "";
                                    ArraySell[i].Cells[2].Value = "";
                                    ArraySell[i].Cells[3].Value = "";
                                    ArraySell[i].Cells[1].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Regular);
                                }
                                else
                                {
                                    decimal Price = QuoteAsk[i].Price;
                                    int Volume = QuoteAsk[i].Volume;
                                    CountInDepth[1] += Volume;
                                    decimal SumVol = Trader.Objects.Orders.ToArray()
                                        .Where(o => o.Sec == Securities && o.Price == Price && o.IsActive())
                                        .Sum(o => o.Volume);

                                    ArraySell[i].Cells[2].Value = Price.ToString();
                                    if (SumVol > 0)
                                    {
                                        ArraySell[i].Cells[2].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
                                        ArraySell[i].Cells[1].Value = Volume.ToString() + " (" + SumVol.ToString() + ")";
                                    }
                                    else
                                    {
                                        ArraySell[i].Cells[2].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Regular);
                                        ArraySell[i].Cells[1].Value = Volume.ToString();
                                    }
                                    ArraySell[i].Cells[1].Tag = new StructClickDepth()
                                    {
                                        Flag = "sell",
                                        Price = Price,
                                        Volume = Volume,
                                    };
                                    ArraySell[i].Cells[3].Tag = new StructClickDepth()
                                    {
                                        Flag = "buy",
                                        Price = Price,
                                        Volume = Volume,
                                    };
                                }
                            }
                        }
                        if (ArrayBuy.NotIsNull())
                        {
                            //Наполняем Buy
                            for (int i = 0; i < countB; i++)
                            {
                                if (countInDepthBuy <= i && ArrayBuy[i].NotIsNull())
                                {
                                    ArrayBuy[i].Cells[0].Value = "";
                                    ArrayBuy[i].Cells[1].Value = "";
                                    ArrayBuy[i].Cells[2].Value = "";
                                    ArrayBuy[i].Cells[3].Value = "";
                                    ArrayBuy[i].Cells[3].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Regular);
                                }
                                else
                                {
                                    decimal Price = QuoteBid[countInDepthBuy - i - 1].Price;
                                    int Volume = QuoteBid[countInDepthBuy - i - 1].Volume;
                                    CountInDepth[0] += Volume;
                                    decimal VolSum = Trader.Objects.Orders.ToArray().Where(o => o.Sec == Securities && o.Price == Price && o.IsActive()).Sum(o => o.Volume);

                                    ArrayBuy[i].Cells[1].Tag = new StructClickDepth()
                                    {
                                        Flag = "sell",
                                        Price = Price,
                                        Volume = Volume,
                                    };
                                    ArrayBuy[i].Cells[2].Value = Price.ToString();

                                    if (VolSum > 0)
                                    {
                                        ArrayBuy[i].Cells[2].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);
                                        ArrayBuy[i].Cells[3].Value = Volume.ToString() + " (" + VolSum.ToString() + ")";
                                    }
                                    else
                                    {
                                        ArrayBuy[i].Cells[2].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Regular);
                                        ArrayBuy[i].Cells[3].Value = Volume.ToString();
                                    }
                                    ArrayBuy[i].Cells[1].Tag = new StructClickDepth()
                                    {
                                        Flag = "sell",
                                        Price = Price,
                                        Volume = Volume,
                                    };
                                    ArrayBuy[i].Cells[3].Tag = new StructClickDepth()
                                    {
                                        Flag = "buy",
                                        Price = Price,
                                        Volume = Volume,
                                    };
                                }
                            }
                        }
                    });
                });
            }
        }

        /// <summary> Обновление сообщения </summary>
        private Thread threadMsg = null;
        /// <summary>
        /// Показать рыночное сообщение 
        /// </summary>
        /// <param name="Msg"></param>
        public void ShowTransReply(string Msg)
        {
            if (this.TrElement.NotIsNull())
            {
                if (threadMsg.NotIsNull())
                {
                    threadMsg.Abort();
                    threadMsg = null;
                }
                panelMessage.GuiAsync(() =>
                {
                    panelMessage.Visible = true;
                    textBoxMessage.Text = Msg;
                    this.SetBottomMessage(Msg);
                });
                MThread.InitThread(() =>
                {
                    Thread.Sleep(4000);
                    panelMessage.GuiAsync(() =>
                    {
                        panelMessage.Visible = false;
                    });
                });
            }
        }
        /// <summary>
        /// Событие любой заявки(обычной или стоп)
        /// </summary>
        /// <param name="eventStopOrders"></param>
        public void EventAnyOrder(IEnumerable<object> orders, bool nativeOrders = false)
        {
            if (orders.NotIsNull())
            {
                var count = orders.Count();
                if (count > 0)
                {
                    foreach (var order in orders)
                    {
                        if (order is StopOrder)
                        {
                            var ord = (StopOrder)order;
                            if (ord.Sec == Securities)
                            {
                                nativeOrders = true;
                                break;
                            }
                        }
                        else
                        if (order is Order)
                        {
                            var ord = (Order)order;
                            if (ord.Sec == Securities)
                            {
                                nativeOrders = true;
                                break;
                            }
                        }
                    }
                }
            }
            if (nativeOrders)
            {
                var actOrders = Trader.Objects.Orders.ToArray().Where(o => o.Sec.Code == Securities.Code && o.IsActive());
                var actStOrd = Trader.Objects.StopOrders.ToArray().Where(o => o.Sec.Code == Securities.Code && o.IsActive());
                EventNewOrder(actOrders);
                EventNewStopOrder(actStOrd);
                ActionAllActiveOrders(actOrders, actStOrd);
            }
        }
        /// <summary>
        /// Обновляем заявки для панели
        /// </summary>
        /// <param name="orders"></param>
        private void UpdatePanelOrder(IEnumerable<Order> orders)
        {
            this.GuiAsync(() =>
            {
                dataGridOrders.Rows.Clear();
                orders.ToArray().ForEach<Order>((o) =>
                {
                    var newRow = (DataGridViewRow)dataGridOrders.Rows[0].Clone();
                    newRow.Cells[0].Value = o.OrderNumber;
                    newRow.Cells[1].Value = o.Price.ToString();
                    newRow.Cells[2].Value = o.Volume.ToString();
                    newRow.Cells[3].Value = o.Balance.ToString();
                    newRow.Cells[4].Value = o.IsBuy() ? 'B' : 'S';
                    newRow.Tag = o;
                    dataGridOrders.Rows.Add(newRow);
                });
            });
        }

        private void ActionAllActiveOrders(IEnumerable<Order> orders, IEnumerable<StopOrder> stopOrders)
        {
            //Orders для графика
            List<Chart> ordersForGraphic = new List<Chart>();
            orders.ToArray().ForEach<Order>((o) =>
            {
                var ch = ordersForGraphic.FirstOrDefault(c => c.Price == o.Price);
                var vol = o.IsSell() ? o.Volume * -1 : o.Volume;
                if (ch != null) ch.Volume += vol;
                else ordersForGraphic.Add(new Chart() { Price = o.Price, Volume = vol });
            });
            //Stop orders
            foreach (var o in stopOrders)
            {
                var ch = ordersForGraphic.FirstOrDefault(c => c.Price == o.Price);
                var vol = o.IsSell() ? o.Volume * -1 : o.Volume;
                if (ch != null) ch.Volume += vol;
                else ordersForGraphic.Add(new Chart() { Price = o.ConditionPrice, Volume = vol });
            }
            GraphicStock.SetOrders(ordersForGraphic);
            UpdateGraphic();
        }

        /// <summary> Событие новой заявки </summary>
        public void EventNewOrder(IEnumerable<Order> orders)
        {
            Qlog.CatchException(() =>
            {
                if (TrElement.IsNull()) return;
                //Обновялем панель заявок
                UpdatePanelOrder(orders);
            });
        }

        /// <summary>
        /// Событие новой стоп заявки
        /// </summary>
        private void EventNewStopOrder(IEnumerable<StopOrder> orders)
        {
            var allStopOrders = orders.Where(so => so.Comment.Contains(Define.STOP_LOSS));
            if (allStopOrders.NotIsNull())
            {
                int count = allStopOrders.Count();
                if (count > 1)
                {
                    var lastOrder = allStopOrders.Last();
                    if (lastOrder.NotIsNull())
                    {
                        var cancelOrders = allStopOrders.Where(so => so.Sec == Securities
                            && so.IsActive()
                            && so.OrderNumber != lastOrder.OrderNumber
                            && so.Comment.Contains(Define.STOP_LOSS));
                        if (cancelOrders.NotIsNull())
                        {
                            this.Trader.CancelListStopOrders(cancelOrders);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Быстрое обновление в графике
        /// </summary>
        public void UpdateGraphicFast()
        {
            if (GraphicStock.IsNull())
            {
                return;
            }
            pictureBoxGraphic.GuiAsync(() =>
            {
                if (CurrentTimeFrame.NotIsNull())
                {
                    if (GraphicStock.ActiveTrades.NotIsNull())
                    {
                        GraphicStock.ActiveTrades.ListTrades = TrElement.LastTrades
                        .OrderByDescending(t => t.Number)
                        .Take(80);
                    }
                    GraphicStock.RedrawActual();
                    pictureBoxGraphic.Refresh();
                }
            });
        }

        /// <summary>
        /// Обновление графика
        /// </summary>
        /// <param name="updateAll"></param>
        public void UpdateGraphic()
        {
            if (GraphicStock.IsNull())
            {
                return;
            }
            pictureBoxGraphic.GuiAsync(() =>
            {
                var timeFrame = this.CurrentTimeFrame;
                if (timeFrame.NotIsNull())
                {
                    int index = GetIndexFirstCandle(timeFrame);
                    GraphicStock.IndexFirstCandle = index;
                    GraphicStock.CountVisibleCandles = this.CountCandleInGraphic;

                    GraphicStock.Redraw();
                    pictureBoxGraphic.Refresh();
                }
            });
        }

        /// <summary>
        /// Проверка и выставление стоп-ордера
        /// </summary>
        /// <param name="Price"></param>
        /// <param name="volume"></param>
        public void SetStopOrder(decimal Price, int volume, bool checkSet = true)
        {
            if (Price == 0)
            {
                ShowTransReply("Некорректная цена стоп-ордера!");
                return;
            }
            if (volume == 0)
            {
                ShowTransReply("Не текущих позиций!");
                return;
            }
            decimal stepStop = Securities.Params.MinPriceStep * 20;
            if (volume < 0)
            {
                stepStop = stepStop * -1;
                volume = volume * -1;
            }
            var stopOrder = new StopOrder()
            {
                Sec = Securities,
                Price = Price - stepStop,
                Volume = volume,
                ConditionPrice = Price,
                DateExpiry = DateMarket.ExtractDateTime(dateTimePickerStopOrder.Value),
                ClientCode = SettingsDepth.Data.CodeClient,
                Comment = Define.STOP_LOSS
            };

            if (this.Position.IsBuy() && Price > 0)
            {
                stopOrder.Direction = OrderDirection.Sell;
                if (Price > Securities.LastPrice)
                {
                    ShowTransReply("Не корректная цена стоп заявки! Необходимо указать цену ниже текущей.");
                    return;
                }
            }
            if (this.Position.IsSell() && Price > 0)
            {
                stopOrder.Direction = OrderDirection.Buy;
                if (Price < Securities.LastPrice)
                {
                    ShowTransReply("Не корректная цена стоп заявки! Необходимо указать цену выше текущей.");
                    return;
                }
            }
            //Устанавливаем в форму значение стоп цены
            this.GuiAsync(() =>
            {
                numericUpDownStopPrice.Value = Price;
            });
            MThread.InitThread(() =>
            {
                var cancelOrders = this.Trader.Objects.StopOrders.Where(so => so.Sec == Securities
                            && so.IsActive() && so.Comment.Contains(Define.STOP_LOSS));
                if (cancelOrders.NotIsNull())
                {
                    this.Trader.CancelListStopOrders(cancelOrders);
                }
                Thread.Sleep(500);
                this.Trader.CreateStopOrder(stopOrder, StopOrderType.StopLimit);
                if (checkSet)
                {
                    Thread.Sleep(10000);
                    var allStopOrders = this.Trader.Objects.StopOrders.Where(so => so.Sec == Securities && so.IsActive());
                    if (allStopOrders.NotIsNull())
                    {
                        if (allStopOrders.Count() == 0)
                        {
                            this.SetStopOrder(Price, volume, false);
                        }
                    }
                }
            });
        }
        /// <summary>
        /// Обработка новых свои сделок
        /// </summary>
        /// <param name="mytrade"></param>
        public void EventNewMyTrade(MyTrade mytrade)
        {
            if (mytrade.Trade.Sec != Securities) return;

            if (this.FormStrategy.NotIsNull())
            {
                this.FormStrategy.CheckMyTrade(mytrade);
            }
        }
    }// end class
}
