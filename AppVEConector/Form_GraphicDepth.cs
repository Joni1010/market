using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using Libs;
using System.Collections.Generic;

using MarketObjects;
using Connector.Logs;
using Managers;
using Market.Candles;
using System.Drawing.Drawing2D;
using AppVEConector.libs;
using QuikConnector.MarketObjects;
using AppVEConector.GraphicTools.Indicators;
using GraphicTools.Extension;
using System.Drawing;
using GraphicTools.Base;
using AppVEConector.libs.Signal;
using Market.AppTools;

namespace AppVEConector
{
    public partial class Form_GraphicDepth : Form
    {
        /// <summary>
        /// Trader
        /// </summary>
        public Connector.QuikConnector Trader = null;
        /// <summary>
        /// Торгуемый элемент
        /// </summary>
        public TElement TrElement = null;
        /// <summary>
        /// Родительское окно
        /// </summary>
        private new MainForm Parent = null;
        /// <summary>
        /// Текущий инструмент
        /// </summary>
        private Securities Securities
        {
            get
            {
                return TrElement.NotIsNull() ? TrElement.Security : null;
            }
        }
        /// <summary>
        /// Последняя стоп-заявка
        /// </summary>
        private StopOrder LastStopOrder
        {
            get
            {
                return Trader.Objects.StopOrders.ToArray().
                    Where(o => o.Sec == Securities).
                    OrderBy(o => o.OrderNumber).LastOrDefault();
            }
        }


        public Form_GraphicDepth(Connector.QuikConnector trader, TElement trElement, object parent)
        {
            InitializeComponent();
            this.Parent = (MainForm)parent;
            if (this.Parent.IsNull())
            {
                this.Close();
            }
            this.Trader = trader;
            if (this.Trader.IsNull())
            {
                this.Close();
            }
            this.TrElement = trElement;
            if (this.TrElement.IsNull())
            {
                this.Close();
            }
            SettingsDepth = SettingsFormSec.New(TrElement.Security);
        }

        private void Form_GraphicDepth_Load(object sender, EventArgs ev)
        {
            this.DoubleBuffered = true;
            numericUpDownVolume.InitWheelDecimal(0, 1000, 1);

            this.InitReset();

            PanelSettings_Init();
            ContextMenuGraphic_Init();

            this.Parent.OnNewReply += (reply) =>
            {
                var r = reply.Last();
                if (r.NotIsNull() && r.Account.NotIsNull())
                {
                    ShowTransReply(r.ResultMsg);
                }
            };

            //Запуск таймера обновляющий стакан
            this.FastUpdater();

            ///Событие новой свечи в любом тайм-фрейме
            this.TrElement.OnNewCandle += (tf, candle) =>
            {
                if (this.FormStrategy.NotIsNull())
                {
                    this.FormStrategy.ActivateStrategy(tf);
                }
                if (this.CurrentTimeFrame.TimeFrame == tf)
                {
                    this.UpdateGraphic();
                }
                GraphicStock.ProcessEventNewCandle(tf, candle);
            };

            comboBoxTimeFrame.InitDefault();
            comboBoxTimeFrame.DataSource = SelectorTimeFrame.GetAll();
            comboBoxTimeFrame.SelectedIndex = SelectorTimeFrame.GetIndex(SettingsDepth.Data.CurrentTimeFrame);
            comboBoxTimeFrame.SelectedIndexChanged += (s, e) =>
            {
                ComboBox sen = (ComboBox)s;
                if (sen.SelectedItem.NotIsNull() && sen.SelectedItem is SelectorTimeFrame && CurrentTimeFrameValue >= 0)
                {
                    var item = (SelectorTimeFrame)sen.SelectedItem;
                    SettingsDepth.Data.CurrentTimeFrame = item.TimeFrame;
                    GraphicStock.DataSourceTimeFrame = this.CurrentTimeFrame;
                    CountCandleInGraphic = SettingsDepth.Data.TimeFrame[CurrentTimeFrameValue].CountVisibleCandle;
                    UpdateGraphic();
                }
            };

            GraphicStock.OnCandleMove += (p, candle) =>
            {
                /*labelInfoGraphic.GuiAsync(() =>
                {
                    labelInfoGraphic.Text =
                        candle.Candle.Time.ToString() + " ; " +
                        "High " + candle.Candle.High + " ; " +
                        "Low " + candle.Candle.Low + " ; " +
                        "Open " + candle.Candle.Open + " ; " +
                        "Close " + candle.Candle.Close + " ; " +
                        "V " + candle.Candle.Volume + " ; ";
                    if (GraphicStock.GetTypeHorVolume() == 1)
                    {
                        labelInfoGraphic.Text += "HV " +
                            (candle.CurHorVolume.NotIsNull() ? (candle.CurHorVolume.VolBuy + candle.CurHorVolume.VolSell).ToString() : "0") +
                            " / " +
                            (candle.MaxHorVolume.NotIsNull() ? candle.MaxHorVolume.Volume.ToString() : "0");
                    }
                });*/
            };

            OnSelectTypeHorVol += (menu, type) =>
            {
                this.GraphicStock.SetTypeHorVolume(type);
                this.UpdateGraphic();
            };

            InitEvents();

            numericUpDownMAPeriod.InitWheelDecimal(0, 1000, 1);
            GraphicStock.Indicators.ForEach((ind) =>
            {
                if (ind is MovingAverage)
                {
                    numericUpDownMAPeriod.GuiAsync(() =>
                    {
                        numericUpDownMAPeriod.Value = ((MovingAverage)ind).GetPeriod();
                    });
                }
            });

            checkBoxStrechPrice.AddGroup();
            checkBoxMovePrice.AddGroup();
            checkBoxStrechPrice.Click += new System.EventHandler(this.checkBoxStrechPrice_Click);
            checkBoxMovePrice.CheckedChanged += new System.EventHandler(this.checkBoxMovePrice_CheckedChanged);


            hScrollGraphic.ValueChanged += (s, e) =>
            {
                //if (e.Type == ScrollEventType.EndScroll)
                {
                    UpdateGraphic();
                }
            };

            numericUpDownTradeVolControl.ValueChanged += (s, e) =>
            {
                GraphicStock.ActiveTrades.MinVolumeShow = (int)numericUpDownTradeVolControl.Value;
            };
            UpdateGraphic();
        }

        private void InitEvents()
        {
            //При наведении скрывать сообщение
            textBoxMessage.MouseMove += (s, ev) =>
            {
                if (!((TextBox)s).Parent.Empty())
                    ((TextBox)s).Parent.Visible = false;
            };

            MouseEventHandler rightClick = (s, ee) =>
            {
                var MouseEvent = (MouseEventArgs)ee;
                if (MouseEvent.Button == MouseButtons.Right)
                {
                    if (Securities.LastPrice != 0)
                    {
                        var obj = (NumericUpDown)s;
                        obj.Value = Securities.LastPrice;
                    }
                }
            };
            numericUpDownPrice.MouseDown += rightClick;
            numericUpDownStopPrice.MouseDown += rightClick;

            Action eventCancelOrdersBuy = () =>
            {
                Qlog.CatchException(() =>
                {
                    var listOrders = Trader.Objects.Orders.Where(o => o.IsActive() && o.IsBuy());
                    foreach (var ord in listOrders)
                    {
                        Trader.CancelOrder(Securities, ord.OrderNumber);
                    }
                });
            };

            labelOrdersBuy.MouseDoubleClick += (s, e) => { eventCancelOrdersBuy(); };
            labelOrdersBuy.Click += (s, e) =>
            {
                var MouseEvent = (MouseEventArgs)e;
                if (MouseEvent.Button == MouseButtons.Right)
                {
                    eventCancelOrdersBuy();
                }
            };

            Action eventCancelOrdersSell = () =>
            {
                Qlog.CatchException(() =>
                {
                    var listOrders = Trader.Objects.Orders.Where(o => o.IsActive() && o.IsSell());
                    foreach (var ord in listOrders)
                    {
                        Trader.CancelOrder(Securities, ord.OrderNumber);
                    }
                });
            };

            labelOrdersSell.MouseDoubleClick += (s, e) => { eventCancelOrdersSell(); };
            labelOrdersSell.Click += (s, e) =>
            {
                var MouseEvent = (MouseEventArgs)e;
                if (MouseEvent.Button == MouseButtons.Right)
                {
                    eventCancelOrdersSell();
                }
            };

            numericUpDownStopPrice.MouseDoubleClick += (s, e) =>
            {
                if (numericUpDownStopPrice.IsMouseOnTextBox(e))
                {
                    numericUpDownStopPrice.Value = Securities.LastPrice;
                }
            };

            numericUpDownPrice.MouseDoubleClick += (s, e) =>
            {
                if (numericUpDownPrice.IsMouseOnTextBox(e))
                {
                    numericUpDownPrice.Value = Securities.LastPrice;
                }
            };

            buttonStopLimitBuy.MouseUp += (s, e) =>
            {
                var MouseEvent = (MouseEventArgs)e;
                if (MouseEvent.Button == MouseButtons.Left)
                {
                    this.SendStopOrder(true);
                }
                if (MouseEvent.Button == MouseButtons.Right)
                {
                    var ord = this.Trader.Objects.StopOrders.Where(o => o.Comment.Contains(Define.STOP_LIMIT)
                        && o.IsActive() && o.IsBuy());
                    this.Trader.CancelListStopOrders(ord);
                }
            };
            buttonStopLimitSell.MouseUp += (s, e) =>
            {
                var MouseEvent = (MouseEventArgs)e;
                if (MouseEvent.Button == MouseButtons.Left)
                {
                    this.SendStopOrder(false);
                }
                if (MouseEvent.Button == MouseButtons.Right)
                {
                    var ord = this.Trader.Objects.StopOrders.Where(o => o.Comment.Contains(Define.STOP_LIMIT)
                        && o.IsActive() && o.IsSell());
                    this.Trader.CancelListStopOrders(ord);
                }
            };

            checkBoxIndEnableCountTrades.CheckedChanged += (s, e) =>
            {
                foreach (var ind in GraphicStock.Indicators)
                {
                    if (ind is IndicatorCTHV)
                    {
                        ((IndicatorCTHV)ind).Enable = checkBoxIndEnableCountTrades.Checked;
                        UpdateGraphic();
                        break;
                    }
                }
            };

            checkBoxIndHVEnable.CheckedChanged += (s, e) =>
            {
                foreach (var ind in GraphicStock.Indicators)
                {
                    if (ind is IndicatorHV)
                    {
                        UpdateGraphic();
                        break;
                    }
                }
            };

        }

        /// <summary>
        /// Удаление котировок за указанную дату
        /// </summary>
        private void DeleteCharts(DateTime dateStart, DateTime dateEnd)
        {
            this.TrElement.ClearCandles(dateStart, dateEnd);
        }

        /// <summary>
        /// Создание отложенных стоп заявок
        /// </summary>
        /// <param name="isBuy"></param>
        private void SendStopOrder(bool isBuy = true)
        {
            if (Securities.LastPrice == 0)
            {
                ShowTransReply("Значение последней цены равно 0.");
                return;
            }
            if (isBuy)
            {
                var sOrder = new StopOrder()
                {
                    Sec = Securities,
                    Price = this.numericUpDownStopPrice.Value,
                    Volume = Convert.ToInt32(this.numericUpDownVolume.Value),
                    Direction = OrderDirection.Buy,
                    Comment = Define.STOP_LIMIT,
                    ClientCode = SettingsDepth.Data.CodeClient,
                    DateExpiry = DateMarket.ExtractDateTime(dateTimePickerStopOrder.Value)
                };
                if (Securities.LastPrice > this.numericUpDownStopPrice.Value)
                {
                    sOrder.ConditionPrice = this.numericUpDownStopPrice.Value + Securities.Params.MinPriceStep;
                    sOrder.Offset = Securities.Params.MinPriceStep;
                    sOrder.Spread = Securities.Params.MinPriceStep;
                    this.Trader.CreateStopOrder(sOrder, StopOrderType.TakeProfit);
                }
                else
                {
                    sOrder.ConditionPrice = this.numericUpDownStopPrice.Value - Securities.Params.MinPriceStep;
                    this.Trader.CreateStopOrder(sOrder, StopOrderType.StopLimit);
                }
            }
            else
            {
                var sOrder = new StopOrder()
                {
                    Sec = Securities,
                    Price = this.numericUpDownStopPrice.Value,
                    Volume = Convert.ToInt32(this.numericUpDownVolume.Value),
                    Direction = OrderDirection.Sell,
                    Comment = Define.STOP_LIMIT,
                    ClientCode = SettingsDepth.Data.CodeClient,
                    DateExpiry = DateMarket.ExtractDateTime(dateTimePickerStopOrder.Value)
                };
                if (Securities.LastPrice < numericUpDownStopPrice.Value)
                {
                    sOrder.ConditionPrice = this.numericUpDownStopPrice.Value - Securities.Params.MinPriceStep;
                    sOrder.Offset = Securities.Params.MinPriceStep;
                    sOrder.Spread = Securities.Params.MinPriceStep;
                    this.Trader.CreateStopOrder(sOrder, StopOrderType.TakeProfit);
                }
                else
                {
                    sOrder.ConditionPrice = this.numericUpDownStopPrice.Value + Securities.Params.MinPriceStep;
                    this.Trader.CreateStopOrder(sOrder, StopOrderType.StopLimit);
                }
            }
        }

        private void dataGridViewDepth_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGrid = (DataGridView)sender;
        }

        /// <summary> Кликеры по таблице стакана </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewDepth_Click(object sender, EventArgs e)
        {
            if (Securities.IsNull()) return;
            var MouseEvent = (MouseEventArgs)e;
            var dataGrid = (DataGridView)sender;
            dataGrid.ClearSelection();
            var hti = dataGrid.HitTest(MouseEvent.X, MouseEvent.Y);
            int indexRow = hti.RowIndex;
            int indexCol = hti.ColumnIndex;
            if (indexRow < 0 || indexCol < 0) return;

            DataGridViewCell cell = dataGrid.Rows[indexRow].Cells[indexCol];
            if (MouseEvent.Button == MouseButtons.Left)
            {
                if (cell.Tag != null)
                {
                    int Volume = Convert.ToInt32(numericUpDownVolume.Value);
                    if (Volume == 0)
                    {
                        ShowTransReply("Не указан объем!");
                    }
                    if (cell.Tag.GetType().ToString().Contains("StructClickDepth"))
                    {
                        var data = (StructClickDepth)cell.Tag;

                        OrderDirection? direction = null;
                        if (data.Flag == "buy")
                        {
                            direction = OrderDirection.Buy;
                        }
                        if (data.Flag == "sell")
                        {
                            direction = OrderDirection.Sell;
                        }
                        var clientCode = comboBoxCodeClient.SelectedItem.NotIsNull() ? comboBoxCodeClient.SelectedItem.ToString() : "";
                        MThread.InitThread(() =>
                        {
                            Trader.CreateOrder(new Order()
                            {
                                Sec = Securities,
                                Direction = direction,
                                Price = data.Price,
                                Volume = Volume,
                                ClientCode = clientCode
                            });
                        });
                    }
                }
            }
            if (MouseEvent.Button == MouseButtons.Right)
            {
                if (cell.Tag != null)
                {
                    if (cell.Tag.GetType().ToString().Contains("StructClickDepth"))
                    {
                        var data = (StructClickDepth)cell.Tag;
                        if (data.Flag == "buy" || data.Flag == "sell")
                        {
                            MThread.InitThread(() =>
                            {
                                var ords = Trader.Objects.Orders.Where(o => o.Sec == Securities && o.Price == data.Price && o.IsActive());
                                if (ords.NotIsNull())
                                {
                                    foreach (var ord in ords)
                                        Trader.CancelOrder(ord.Sec, ord.OrderNumber);
                                }
                            });
                        }
                    }
                }
            }
        }

        private void Form_GraphicDepth_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Trader.UnregisterDepth(Securities);
            SettingsDepth.Save();
        }

        private void buttonCancelAll_Click(object sender, EventArgs e)
        {
            if (Securities.NotIsNull())
            {
                Trader.CancelAllOrder(Securities);
            }
        }

        private void buttonExitPos_Click(object sender, EventArgs e)
        {
            if (Securities.NotIsNull())
            {
                var pos = Trader.Objects.Positions.FirstOrDefault(p => p.Sec == Securities);
                if (pos != null)
                {
                    if (pos.CurrentVolume != 0)
                    {
                        MThread.InitThread(() =>
                        {
                            OrderDirection? direction = pos.IsBuy()
                                ? OrderDirection.Sell : OrderDirection.Buy;

                            decimal LastPrice = Securities.Params.LastPrice;
                            decimal Price = direction == OrderDirection.Sell
                                ? Price = LastPrice - Securities.Params.MinPriceStep * 5
                                : LastPrice + Securities.Params.MinPriceStep * 5;
                            this.GuiAsync(() =>
                            {
                                var clientCode = comboBoxCodeClient.SelectedItem.NotIsNull() ? comboBoxCodeClient.SelectedItem.ToString() : "";
                                Trader.CreateOrder(new Order()
                                {
                                    Sec = Securities,
                                    Direction = direction,
                                    Price = Price,
                                    Volume = pos.CurrentVolume,
                                    ClientCode = clientCode
                                });
                            });
                        });
                    }
                    else
                    {
                        ShowTransReply("По данному инструменту нет позиций.");
                    }
                }
            }
        }

        private void buttonBuy_Click(object sender, EventArgs e)
        {
            if (Securities != null)
            {
                if (numericUpDownVolume.Value > 0)
                {
                    var clientCode = comboBoxCodeClient.SelectedItem.NotIsNull() ? comboBoxCodeClient.SelectedItem.ToString() : "";
                    Trader.CreateOrder(new Order()
                    {
                        Sec = Securities,
                        Direction = OrderDirection.Buy,
                        Price = numericUpDownPrice.Value,
                        Volume = Convert.ToInt32(numericUpDownVolume.Value),
                        ClientCode = clientCode
                    });
                }
                else
                {
                    ShowTransReply("Объем не может быть 0 или отрицательным значением.");
                }
            }
        }

        private void buttonSell_Click(object sender, EventArgs e)
        {
            if (Securities != null)
            {
                if (numericUpDownVolume.Value > 0)
                {
                    var clientCode = comboBoxCodeClient.SelectedItem.NotIsNull() ? comboBoxCodeClient.SelectedItem.ToString() : "";
                    Trader.CreateOrder(new Order()
                    {
                        Sec = Securities,
                        Direction = OrderDirection.Sell,
                        Price = numericUpDownPrice.Value,
                        Volume = Convert.ToInt32(numericUpDownVolume.Value),
                        ClientCode = clientCode
                    });
                }
                else
                {
                    ShowTransReply("Объем не может быть 0 или отрицательным значением.");
                }
            }
        }

        private void Form_GraphicDepth_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show(this, "Закрыть окно " + Securities.Code + "?", "Закрыть окно " + Securities.Code + "?",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (this.FormStrategy.NotIsNull())
            {
                MessageBox.Show("Закройте окно торгуемой стратегии!");
                e.Cancel = true;
                return;
            }

            if (result == DialogResult.OK)
            {
                this.TrElement.SaveCharts();
                e.Cancel = false;
            }
            else e.Cancel = true;
        }

        private void checkBoxLockDepth_CheckedChanged(object sender, EventArgs e)
        {
            var check = (CheckBox)sender;
            if (check.Checked)
            {
                if (this.TrElement.NotIsNull())
                {
                    this.Trader.RegisterDepth(Securities);
                }
                dataGridViewDepth.Enabled = false;
                panelLock.Visible = true;
            }
            else
            {
                if (this.TrElement.NotIsNull())
                {
                    this.Trader.UnregisterDepth(Securities);
                }
                dataGridViewDepth.Enabled = true;
                panelLock.Visible = false;
            }
        }

        private void buttonCancelStopOrders_Click(object sender, EventArgs e)
        {
            if (Securities != null)
            {
                Trader.CancelAllStopOrder(Securities);
            }
        }

        int WidthPanelDepth = -1;
        private void checkBoxShowHideDepth_CheckedChanged(object sender, EventArgs e)
        {
            if (WidthPanelDepth < 0) WidthPanelDepth = panelRight.Width;
            if (checkBoxShowHideDepth.Checked) panelRight.Width = 0;
            else panelRight.Width = WidthPanelDepth;
        }

        private void numericUpDownPrice_ValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Создает стоп-заявки по кнопке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateStopOrder_Click(object sender, EventArgs e)
        {
            Qlog.CatchException(() =>
            {
                decimal Price = numericUpDownStopPrice.Value;
                this.SetStopOrder(Price, Position.Data.CurrentNet);
            });
        }

        /// <summary> Обработчик нажатия на сообщение </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMessage_Click(object sender, EventArgs e)
        {
            var MouseEvent = (MouseEventArgs)e;
            if (MouseEvent.Button == MouseButtons.Right)
            {
                panelMessage.Visible = false;
                panelMessage.Visible = false;
            }
        }

        private void pictureBoxGraphic_Paint(object sender, PaintEventArgs e)
        {
            Qlog.CatchException(() =>
            {
                var g = e.Graphics;
                g.CompositingQuality = CompositingQuality.GammaCorrected;
                GraphicStock.AllToCanvas(g);
            });
        }

        /// <summary>
        /// Получает индекс первой свечи с учетом скрола.
        /// </summary>
        /// <param name="timeFrame">Тайм-фрейм</param>
        /// <returns></returns>
        private int GetIndexFirstCandle(CandleCollection timeFrame, int step = 5)
        {
            int index = -1;
            hScrollGraphic.GuiAsync(() =>
            {
                int countCandlesWithStep = (int)(timeFrame.Count / step);
                if (hScrollGraphic.Maximum != countCandlesWithStep)
                {
                    var toFirst = false;
                    if (hScrollGraphic.Value == hScrollGraphic.Maximum)
                    {
                        toFirst = true;
                    }
                    hScrollGraphic.Minimum = 0;
                    hScrollGraphic.Maximum = countCandlesWithStep;
                    if (toFirst)
                    {
                        hScrollGraphic.Value = hScrollGraphic.Maximum;
                    }
                }
                index = (hScrollGraphic.Maximum - hScrollGraphic.Value) * step;
            });
            return index;
        }

        private void pictureBoxGraphic_Click(object sender, EventArgs e)
        {

        }

        private void Form_GraphicDepth_Resize(object sender, EventArgs e)
        {
            //this.UpdateGraphic();
        }

        private void buttonInc_Click(object sender, EventArgs e)
        {
            var obj = (Control)sender;
            var ConstMin = 10;
            var val = 1;
            if (obj.Tag.NotIsNull())
            {
                val = (int)obj.Tag;
            }
            this.CountCandleInGraphic -= (int)(this.CountCandleInGraphic / ConstMin) + val;
            if (this.CountCandleInGraphic < ConstMin) this.CountCandleInGraphic = ConstMin;

            SettingsDepth.Data.TimeFrame[CurrentTimeFrameValue].CountVisibleCandle = CountCandleInGraphic;
            this.UpdateGraphic();
        }

        private void buttonDec_Click(object sender, EventArgs e)
        {
            var obj = (Control)sender;
            var ConstMin = 10;
            var val = 1;
            if (obj.Tag.NotIsNull())
            {
                val = (int)obj.Tag;
            }
            this.CountCandleInGraphic += (int)(this.CountCandleInGraphic / ConstMin) + val;
            if (this.CountCandleInGraphic > 1500) this.CountCandleInGraphic = 1500;

            SettingsDepth.Data.TimeFrame[CurrentTimeFrameValue].CountVisibleCandle = CountCandleInGraphic;
            this.UpdateGraphic();
        }


        private void dateTimePickerStopOrder_ValueChanged(object sender, EventArgs e)
        {
            if (sender is DateTimePicker)
            {
                var pikerDT = (DateTimePicker)sender;
            }
        }
        /// <summary>
        /// Set header
        /// </summary>
        /// <param name="securities"></param>
        private void SetHead(Securities securities)
        {
            Text = "(" + securities.Code + ":" + securities.Class.Code + ") " + securities.Name;
        }
        /// <summary> Установка сообщения в нижней панели</summary>
        /// <param name="text"></param>
        private void SetBottomMessage(string text)
        {
            labelLastMsg.Text = text;
        }


        private void buttonDelOrder_Click(object sender, EventArgs e)
        {
            dataGridOrders.SelectedRows.ForEach<DataGridViewRow>((row) =>
            {
                if (!row.Tag.IsNull())
                {
                    var ord = (Order)row.Tag;
                    this.Trader.CancelOrder(ord.Sec, ord.OrderNumber);
                }
            });
        }

        private void buttonCopyOrder_Click(object sender, EventArgs e)
        {
            dataGridOrders.SelectedRows.ForEach<DataGridViewRow>((row) =>
            {
                if (!row.Tag.IsNull())
                {
                    var clientCode = comboBoxCodeClient.SelectedItem.NotIsNull() ? comboBoxCodeClient.SelectedItem.ToString() : "";
                    var ord = (Order)row.Tag;
                    ord.ClientCode = clientCode;
                    this.Trader.CreateOrder(ord);
                }
            });
        }

        private void buttonLoadTicks_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect = true;
            file.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            file.FilterIndex = 2;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    labelStatusLoad.GuiAsync(() =>
                    {
                        labelStatusLoad.Text = "";
                    });
                    int CountFiles = file.FileNames.Length;
                    int CountStat = CountFiles;
                    //  Поле которое необходимо модифицировать 
                    var fieldModify = comboBoxLoadField.Text;
                    // Операция модификации
                    var operationModify = comboBoxLoadOper.Text;

                    Thread threadLoad = null;
                    MThread.InitThread(() =>
                    {
                        foreach (string f in file.FileNames)
                        {
                            if (threadLoad.NotIsNull())
                            {
                                threadLoad.Join();
                            }
                            WFile wf = new WFile(f);
                            var arrayAll = wf.ReadAllLines();
                            threadLoad = MThread.InitThread(() =>
                            {
                                int i = 0;
                                List<Trade> LoadTradeFromFile = new List<Trade>();
                                foreach (var str in arrayAll)
                                {
                                    if (i == 0)
                                    {
                                        i++;
                                        continue; //пропускаем шапку
                                    }
                                    var tr = Trade.GetConvertTrade(Securities, str);
                                    //Корректируем значения 
                                    var modifyTrade = this.ModifyLoadTrade(tr, fieldModify, operationModify);
                                    if (modifyTrade.NotIsNull())
                                    {
                                        LoadTradeFromFile.Add(modifyTrade);
                                    }
                                    i++;
                                }
                                labelStatusLoad.GuiAsync(() =>
                                {
                                    labelStatusLoad.Text = f + "\r\n" + labelStatusLoad.Text;
                                });
                                i = 100;
                                foreach (var tr in LoadTradeFromFile)
                                {
                                    this.TrElement.NewTrade(tr, true);
                                    if (i <= 0)
                                    {
                                        Thread.Sleep(1);
                                        i = 100;
                                    }
                                }
                                CountStat--;

                                labelStatusLoad.GuiAsync(() =>
                                {
                                    labelStatusLoad.Text = "Process: " + CountStat + "/" + CountFiles + "\r\n" + labelStatusLoad.Text;
                                });
                            });
                        }
                    });
                    labelStatusLoad.GuiAsync(() =>
                    {
                        labelStatusLoad.Text = "Files: " + CountFiles + "\r\n" + labelStatusLoad.Text;
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private Trade ModifyLoadTrade(Trade trade, string fieldMod, string operationMod)
        {
            if (trade.IsNull())
            {
                return null;
            }
            if (!fieldMod.Empty() && !operationMod.Empty())
            {
                if (fieldMod == "Volume" && operationMod == "/")
                {
                    trade.Volume = trade.Volume / textBoxLoadValue.Text.ToInt32();
                }
                else if (fieldMod == "Volume" && operationMod == "*")
                {
                    trade.Volume = trade.Volume * textBoxLoadValue.Text.ToInt32();
                }
            }
            return trade;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (this.FormStrategy.IsNull())
            {
                this.FormStrategy = new Form_Strategy_morePrev(this.Trader, this.TrElement);
                this.FormStrategy.Show();

                this.FormStrategy.FormClosed += (s, ecl) =>
                {
                    this.FormStrategy = null;
                };
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var formStops = new Form_ActivateStopOrders(this.Trader, this.TrElement);
            formStops.ShowDialog();
        }


        private void buttonSignals_Click(object sender, EventArgs e)
        {
            FormSignalGsm formSignal = new FormSignalGsm(this.TrElement, SignalView.GSMSignaler);
            formSignal.ShowDialog();
        }

        private void buttonRecalculVol_Click(object sender, EventArgs e)
        {
            foreach (var tf in this.TrElement.CollectionTimeFrames.ToArray())
            {
                foreach (var can in tf.MainCollection)
                {
                    can.InterestBuy = 0;
                    can.InterestSell = 0;
                    can.CountInterest = 0;
                    can.HorVolumes.HVolCollection.RecalculateMaxMinVolumes();
                }
                tf.TimeLastUpdateCollection = DateTime.Now;
            }
        }

        private void buttonOrder5Plus_Click(object sender, EventArgs e)
        {
            dataGridOrders.SelectedRows.ForEach<DataGridViewRow>((row) =>
            {
                if (!row.Tag.IsNull())
                {
                    var clientCode = comboBoxCodeClient.SelectedItem.NotIsNull() ? comboBoxCodeClient.SelectedItem.ToString() : "";
                    var ord = (Order)row.Tag;
                    var newOrd = ord.Clone();
                    newOrd.ClientCode = clientCode;
                    newOrd.Price = ord.Price + 5 * Securities.MinPriceStep;
                    this.Trader.CreateOrder(newOrd);
                }
            });
        }

        private void buttonOrder5Minus_Click(object sender, EventArgs e)
        {
            dataGridOrders.SelectedRows.ForEach<DataGridViewRow>((row) =>
            {
                if (!row.Tag.IsNull())
                {
                    var clientCode = comboBoxCodeClient.SelectedItem.NotIsNull() ? comboBoxCodeClient.SelectedItem.ToString() : "";
                    var ord = (Order)row.Tag;
                    var newOrd = ord.Clone();
                    newOrd.ClientCode = clientCode;
                    newOrd.Price = ord.Price - 5 * Securities.MinPriceStep;
                    this.Trader.CreateOrder(newOrd);
                }
            });
        }

        private void buttonOrder10Plus_Click(object sender, EventArgs e)
        {
            dataGridOrders.SelectedRows.ForEach<DataGridViewRow>((row) =>
            {
                if (!row.Tag.IsNull())
                {
                    var clientCode = comboBoxCodeClient.SelectedItem.NotIsNull() ? comboBoxCodeClient.SelectedItem.ToString() : "";
                    var ord = (Order)row.Tag;
                    var newOrd = ord.Clone();
                    newOrd.ClientCode = clientCode;
                    newOrd.Price = ord.Price + 10 * Securities.MinPriceStep;
                    this.Trader.CreateOrder(newOrd);
                }
            });
        }

        private void buttonOrder10Minus_Click(object sender, EventArgs e)
        {
            dataGridOrders.SelectedRows.ForEach<DataGridViewRow>((row) =>
            {
                if (!row.Tag.IsNull())
                {
                    var clientCode = comboBoxCodeClient.SelectedItem.NotIsNull() ? comboBoxCodeClient.SelectedItem.ToString() : "";
                    var ord = (Order)row.Tag;
                    var newOrd = ord.Clone();
                    newOrd.ClientCode = clientCode;
                    newOrd.Price = ord.Price - 10 * Securities.MinPriceStep;
                    this.Trader.CreateOrder(newOrd);
                }
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var formCopy = new Form_CopySecurity(this.Trader, this.TrElement);
            formCopy.ShowDialog();
        }

        private void numericUpDownMAPeriod_ValueChanged(object sender, EventArgs e)
        {
            var value = Convert.ToInt32(numericUpDownMAPeriod.Value);
            GraphicStock.Indicators.ForEach((ind) =>
            {
                if (ind is MovingAverage)
                {
                    ((MovingAverage)ind).SetPeriod(value);
                }
            });
            UpdateGraphic();
        }

        private void buttonSaveSec_Click(object sender, EventArgs e)
        {
            var head = "Сохранить данные по инструменту " + Securities.Code + "?";
            var result = MessageBox.Show(this, head, head,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                TrElement.SaveCharts(true);
                MessageBox.Show("Инструмент сохранен");
            }
        }

        private void buttonLoadSec_Click(object sender, EventArgs e)
        {
            var head = "Загрузить данные по инструменту " + Securities.Code + "?";
            var result = MessageBox.Show(this, head, head,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                TrElement.LoadCharts(true);
                UpdateGraphic();
                MessageBox.Show("Инструмент загружен");
            }
        }

        private void buttonClearAllCandle_Click(object sender, EventArgs e)
        {
            var head = "Подтвердите удаление всех сделок " + Securities.Code + "?";
            var result = MessageBox.Show(this, head, head,
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                this.DeleteCharts(DateTime.Now.AddYears(-20), DateTime.Now);
                MessageBox.Show("История удалена");
            }
        }
        /// <summary>
        /// Растягивание графика
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxStrechPrice_Click(object sender, EventArgs e)
        {
            if (checkBoxStrechPrice.Checked)
            {
                GraphicStock.SetTypeScaling(BaseParams.TYPE_SCALING.STRECH);
            }
            else
            {
                GraphicStock.SetTypeScaling(BaseParams.TYPE_SCALING.NONE);
            }
            this.UpdateGraphic();
        }
        /// <summary>
        /// Обработка перемещения графика вниз-вверх
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxMovePrice_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMovePrice.Checked)
            {
                GraphicStock.SetTypeScaling(BaseParams.TYPE_SCALING.MOVE);
            }
            else
            {
                GraphicStock.SetTypeScaling(BaseParams.TYPE_SCALING.NONE);
            }
            this.UpdateGraphic();
        }

        private void checkBoxClearHorVol_CheckedChanged(object sender, EventArgs e)
        {
            GraphicStock.CrearHorVolumes();
            UpdateGraphic();
        }
    }
}
