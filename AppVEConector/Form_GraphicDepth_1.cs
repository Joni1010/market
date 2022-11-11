using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MarketObjects;
using System.Threading;
using AppVEConector.libs;
using Market.Candles;
using GraphicTools;
using Connector.Logs;
using Managers;
using libs;
using QuikConnector.libs;
using QuikConnector.MarketObjects;
using AppVEConector.libs.Signal;
using MarketObjects.Charts;
using Market.Base;

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

        /// <summary> Текущий тайм-фрейм </summary>
        private TimeFrame CurrentTimeFrame
        {
            get
            {
                if (TrElement.StorageTF.Count == 0)
                {
                    return null;
                }
                return TrElement.StorageTF.GetTimeFrame(TimeFrameUse.Value);
            }
        }

        /// <summary> Наполнитель стакана ПРОДАЖА. Сбрасывается при новом стакане.</summary>
        private DataGridViewRow[] ArraySell = null;
        /// <summary> Наполнитель стакана ПОКУПКА. Сбрасывается при новом стакане.</summary>
        private DataGridViewRow[] ArrayBuy = null;

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
                        SignalView.GSMSignaler.ToArray().
                        Where(s => s.SecClass == Securities.ToString()).
                        Select(s => s.Price).ToArray()
                    );
                UpdateGraphic();
            };
            SignalView.GSMSignaler.OnAdd += (sig) =>
            {
                updateSingnals();
            };
            SignalView.GSMSignaler.OnRemove += (sig) =>
            {
                updateSingnals();
            };
            updateSingnals();
        }

        /// <summary> Инициализация панели очистки сделок за определенную дату </summary>
        private void InitPanelClearCharts()
        {
            dateTimePickerClearChartsStart.Value = DateTime.Now;
            dateTimePickerClearChartsEnd.Value = DateTime.Now;
            dateTimePickerClearChartsStart.ValueChanged += (s, e) =>
            {
                if (dateTimePickerClearChartsStart.Value > dateTimePickerClearChartsEnd.Value)
                {
                    dateTimePickerClearChartsStart.Value = dateTimePickerClearChartsEnd.Value;
                }
            };
            dateTimePickerClearChartsEnd.ValueChanged += (s, e) =>
            {
                if (dateTimePickerClearChartsStart.Value > dateTimePickerClearChartsEnd.Value)
                {
                    dateTimePickerClearChartsEnd.Value = dateTimePickerClearChartsStart.Value;
                }
            };

            buttonClearCandle.Click += (s, e) =>
            {

                var dateClearStart = dateTimePickerClearChartsStart.Value;
                var dateClearEnd = dateTimePickerClearChartsEnd.Value;

                var result = MessageBox.Show(this, "Удалить сделки за " + dateClearStart.ToLongDateString() + " - " + dateClearEnd.ToLongDateString(), "Удаление котировок?",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (result == DialogResult.OK)
                {
                    var dStart = new DateMarket(dateClearStart);
                    dStart.SetHour(0).SetMinute(0).SetSecond(0);
                    var dEnd = new DateMarket(dateClearEnd);
                    dEnd.SetHour(23).SetMinute(59).SetSecond(59);
                    DeleteCharts(dStart.GetDateTime(), dEnd.GetDateTime());
                }
            };
        }

        private void InitAllPanels()
        {
            //Инициализация доп панелей
            InitPanelLevels();
            InitPanelClearCharts();
            InitPanelControl();
            InitPanelSignals();
            //Уровни
            UpdatePanelLevels(true);
        }

        /// <summary> Инициализация нового инструмента в текущем окне. Сброс на новый инструмент.</summary>
        private void InitReset()
        {
            this.SetHead(Securities);
            //Загружаем настройки по инструменту
            //SettingsDepth.ReloadSecurity(Securities.ToString());
            SettingsDepth.InitTimeFrame(StorageTimeFrames.TIME_FRAMES);

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
            dateTimePickerStopOrder.Value = DateTime.Now.AddDays(SettingsDepth.Get("CountDaysForStopOrder"));
            //Получаем значение последней стоп-заявки
            if (LastStopOrder.NotIsNull())
            {
                numericUpDownStopPrice.Value = LastStopOrder.ConditionPrice;
                dateTimePickerStopOrder.Value = DateTime.Now.AddDays(SettingsDepth.Get("CountDaysForStopOrder"));
            }

            //Настройки графика
            this.GraphicStock = new Graphic();
            this.GraphicStock.InitEvents();
            this.GraphicStock.SetObjectPaint(this.pictureBoxGraphic);

            this.GraphicStock.Init(Securities);
            this.GraphicStock.TimeFrame = CurrentTimeFrame;
            this.GraphicStock.DataSourceLevels = this.Levels.ObjectCollection;
            this.GraphicStock.ResetActiveCandles();

            GraphicStock.OnNotFullGraphic += (curValue, all) =>
            {
                if (all - curValue > 5)
                {
                    CurrentTimeFrame.Candles.Load();
                }
            };
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
                var pos = this.Trader.Objects.tPositions.SearchFirst(p => p.Sec.Code == Securities.Code);
                var stopLoss = this.Trader.Objects.tStopOrders.SearchFirst(so => so.Sec.Code == Securities.Code && so.IsActive());
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
                var listPortf = Trader.Objects.tPortfolios
                    .SearchAll(p => p.Account.AccClasses.FirstOrDefault(c => c == Securities.Class).NotIsNull() &&
                        p.Client.Code == comboBoxCodeClient.SelectedItem.ToString());
                if (TypeClientLimit.Value >= 0)
                {
                    listPortf = listPortf.Where(p => p.LimitKind == TypeClientLimit.Value).ToArray();
                }
                if (listPortf.Count() > 0)
                {
                    Portfolio = listPortf.First();
                }
            }
            if (ClientCode.Value.Empty())
            {
                Position = Trader.Objects.tPositions.SearchFirst(s => s.Sec == Securities);
            }
            else
            {
                Position = Trader.Objects.tPositions.SearchFirst(s => s.Sec == Securities
                    && s.Client.Code == ClientCode.Value);
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
                var orders = this.Trader.Objects.tOrders.SearchAll(so => so.Sec.Code == Securities.Code && so.IsActive()).ToArray();
                var ordB = orders.Where(o => o.IsBuy()).ToArray();
                var ordS = orders.Where(o => o.IsSell()).ToArray();
                Info.CountOrderBuy = ordB.Count();
                Info.CountOrderSell = ordS.Count();
                Info.CountVolumeBuy = ordB.Sum(o => o.Balance);
                Info.CountVolumeSell = ordS.Sum(o => o.Balance);

                var stopOrdersActive = this.Trader.Objects.tStopOrders.SearchAll(so => so.Sec.Code == Securities.Code && so.IsActive());
                if (stopOrdersActive.NotIsNull())
                {
                    var soBuy = stopOrdersActive.Where(so => so.IsBuy()).ToArray();
                    var soSell = stopOrdersActive.Where(so => so.IsSell()).ToArray();
                    var soOrderBuy = soBuy.Where(so => so.Comment.Contains(Define.STOP_LIMIT)).ToArray();
                    var soOrderSell = soSell.Where(so => so.Comment.Contains(Define.STOP_LIMIT)).ToArray();

                    var soLimitBuy = soBuy.Where(so => so.Comment.Contains(Define.STOP_LOSS)).ToArray();
                    var soLimitSell = soSell.Where(so => so.Comment.Contains(Define.STOP_LOSS)).ToArray();

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
                            && Securities.Params.MinPriceStep > 0
                            && Position.NotIsNull())
                        {
                            var priceStop = stopOrder.ConditionPrice;
                            decimal gapPrice = priceStop > Info.CurrentPrice ? priceStop - Info.CurrentPrice : Info.CurrentPrice - priceStop;
                            decimal result = (gapPrice / Securities.Params.MinPriceStep) * Securities.Params.StepPrice;
                            decimal countLots = Position.CurrentVolume;
                            Info.ForecastSum = result * countLots;
                        }
                        else
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

        /// <summary> Обновление стакана </summary>
        public void UpdateDepth()
        {
            if (TrElement.IsNull())
            {
                return;
            }
            int count = 20;
            var orders = Trader.Objects.tOrders
                .SearchAll(o => o.Sec == TrElement.Security && o.Status == OrderStatus.ACTIVE);

            var ordersBuy = orders.Where(o => o.Direction == OrderDirection.Buy)
                .OrderByDescending(o => o.Price).ToArray();
            var ordersSell = orders.Where(o => o.Direction == OrderDirection.Sell)
                .OrderBy(o => o.Price).ToArray();

            var pricesBuy = ordersBuy.Select(o => o.Price).Take(count).ToArray();
            var pricesSell = ordersSell.Select(o => o.Price).Take(count).ToArray();

            if (ArraySell.IsNull())
            {
                ArraySell = new DataGridViewRow[count];
                for (int i = count - 1; i >= 0; i--)
                {
                    var k = dataGridViewDepth.Rows.Add();
                    ArraySell[i] = dataGridViewDepth.Rows[k];
                    ArraySell[i].Cells[0].Value = "";
                    ArraySell[i].Cells[1].Value = "";
                    ArraySell[i].Cells[2].Value = "";
                    ArraySell[i].Cells[3].Value = "";
                    ArraySell[i].Cells[2].Style.BackColor = Color.LightCoral;
                    ArraySell[i].Cells[1].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Regular);
                }

            }
            if (ArrayBuy.IsNull())
            {
                ArrayBuy = new DataGridViewRow[count];
                for (int i = 0; i < count; i++)
                {
                    var k = dataGridViewDepth.Rows.Add();
                    ArrayBuy[i] = dataGridViewDepth.Rows[k];
                    ArrayBuy[i].Cells[0].Value = "";
                    ArrayBuy[i].Cells[1].Value = "";
                    ArrayBuy[i].Cells[2].Value = "";
                    ArrayBuy[i].Cells[3].Value = "";
                    ArrayBuy[i].Cells[2].Style.BackColor = Color.LightGreen;
                    ArrayBuy[i].Cells[1].Style.Font = new Font(DataGridView.DefaultFont, FontStyle.Regular);
                }
            }
            MThread.InitThread(() =>
            {
                //Синхронизуируем между потоками
                dataGridViewDepth.GuiAsync(() =>
                {
                    if (ArraySell.NotIsNull())
                    {
                        //Наполняем Sell
                        for (int i = 0; i < ArraySell.Length; i++)
                        {
                            var row = ArraySell[i];
                            if (pricesSell.Length <= i)
                            {
                                ArraySell[i].Cells[2].Value = "";
                                ArraySell[i].Cells[1].Value = "";
                                ArraySell[i].Cells[1].Tag = null;
                                ArraySell[i].Cells[3].Tag = null;
                                continue;
                            }
                            var ordersByPrice = ordersSell.Where(o => o.Price == pricesSell[i]).ToArray();
                            var volOrders = ordersByPrice.Sum(o => o.Balance);
                            
                            decimal Price = pricesSell[i];
                            int Volume = volOrders;
                            int countOrd = ordersByPrice.Count();

                            ArraySell[i].Cells[2].Value = Price.ToString();
                            ArraySell[i].Cells[1].Value = countOrd.ToString() + " (" +Volume.ToString() + ")";
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
                    if (ArrayBuy.NotIsNull())
                    {
                        //Наполняем Buy
                        for (int i = 0; i < ArrayBuy.Length; i++)
                        {
                            var row = ArrayBuy[i];
                            if (pricesBuy.Length <= i)
                            {
                                ArrayBuy[i].Cells[2].Value = "";
                                ArrayBuy[i].Cells[3].Value = "";
                                ArrayBuy[i].Cells[1].Tag = null;
                                ArrayBuy[i].Cells[3].Tag = null;
                                continue;
                            }
                            var ordersByPrice = ordersBuy.Where(o => o.Price == pricesBuy[i]).ToArray();
                            var volOrders = ordersByPrice.Sum(o => o.Balance);

                            decimal Price = pricesBuy[i];
                            int Volume = volOrders;
                            int countOrd = ordersByPrice.Count();

                            ArrayBuy[i].Cells[2].Value = Price.ToString();
                            ArrayBuy[i].Cells[3].Value = countOrd.ToString() + " (" + Volume.ToString() + ")";
                          
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
                });
            });
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
                var actOrders = Trader.Objects.tOrders.SearchAll(o => o.Sec.Code == Securities.Code && o.IsActive());
                var actStOrd = Trader.Objects.tStopOrders.SearchAll(o => o.Sec.Code == Securities.Code && o.IsActive());
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
                    if (GraphicStock.ActiveTrades.NotIsNull() && GraphicStock.ActiveTrades.IsEnable())
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
                if (CurrentTimeFrame.NotIsNull())
                {
                    int index = GetIndexFirstCandle(CurrentTimeFrame);
                    GraphicStock.IndexFirstCandle = index;
                    GraphicStock.CountVisibleCandles = this.CountCandleInGraphic;

                    GraphicStock.Paint();
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
                ClientCode = ClientCode.Value,
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
                var cancelOrders = this.Trader.Objects.tStopOrders.SearchAll(so => so.Sec == Securities
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
                    var allStopOrders = this.Trader.Objects.tStopOrders.SearchAll(so => so.Sec == Securities && so.IsActive());
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
        }
    }// end class
}
