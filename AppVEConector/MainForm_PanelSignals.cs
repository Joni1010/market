using AppVEConector.libs;
using Connector.Logs;
using MarketObjects;

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


using AppVEConector.libs.Proceses;
using AppVEConector.libs.Signal;
using GraphicTools.Extension;
using Managers;
using QuikConnector.MarketObjects;

namespace AppVEConector
{
    public partial class MainForm : Form
    {
        private Securities LastSecSignal = null;
        /// <summary>
        /// Последний выбранный сигнал в гриде
        /// </summary>
        //private SignalMarket SelSignal = null;

        protected int LastIndexSelectRow = 0;

        /// <summary>
        /// 
        /// </summary>
        public void InitPanelSignal()
        {
            SignalView.GSMSignaler.Phone = Connector.QuikConnector.ConfSettings.GetParam("Main", "PhoneSignal").Value;

            comboBoxSecSign.SetListValues(Global.GetWorkingListSec().ToArray());
            comboBoxSecSign.TextChanged += (s, e) =>
            {
                var text = comboBoxSecSign.Text;
                if (text.Length >= 2)
                {
                    var listSec = Trader.Objects.Securities.Where(el => el.Code.ToLower().Contains(text.ToLower()) ||
                        el.Name.ToLower().Contains(text.ToLower())).Select(el => el.ToString());
                    if (listSec.Count() > 0)
                    {
                        comboBoxSecSign.Clear();
                        comboBoxSecSign.SetListValues(listSec);
                    }
                }
                comboBoxSecSign.Select(text.Length, 0);
                UpdateGridSignals();
            };
            comboBoxSecSign.KeyPress += (s, e) =>
            {
                var k = e.KeyChar;
                if (e.KeyChar == 13)
                {
                    if (comboBoxSecSign.Items.Count > 0)
                    {
                        comboBoxSecSign.Text = comboBoxSecSign.Items[0].ToString();
                    }
                }
            };

            tableLayoutPanel14.Enabled = false;
            tableLayoutPanel8.Enabled = false;
            splitContainer2.Enabled = false;

            SetCurrentPorts();

            numericUpDownPrice.InitWheelDecimal();
            numericUpDownPrice.Maximum = 1000000000;

            numericUpDownSignVolume.InitWheelDecimal();
            numericUpDownSignVolume.Maximum = 1000000000;
            numericUpDownSignVolume.Minimum = 0;
            numericUpDownSignVolume.Increment = 1;

            var tf = SelectorTimeFrame.GetAll();
            comboBoxSignTimeFrame.DataSource = tf.ToArray();
            comboBoxSignTimeFrame.SelectedIndex = 3;

            buttonSignAddVol1000.Click += (s, e) =>
            {
                numericUpDownSignVolume.Value += 1000;
            };
            buttonAddSignVolume.Click += buttonAddSignVolume_Click;

            comboBoxSecSign.SelectedValueChanged += (s, e) =>
            {

                if (comboBoxSecSign.SelectedItem is string)
                {
                    LastSecSignal = GetSecCodeAndClass(comboBoxSecSign.SelectedItem.ToString());
                    if (LastSecSignal.NotIsNull())
                    {
                        numericUpDownPrice.InitSecurity(LastSecSignal);
                        numericUpDownPrice.Value = LastSecSignal.LastPrice.NotIsNull() ? LastSecSignal.LastPrice : 0;
                        labelSignNameSec.Text = LastSecSignal.Name;
                    }
                }

                UpdateGridSignals();
            };

            comboBoxCond.InitDefault();
            comboBoxCond.SetListValues(new string[] { ">=", ">", "<=", "<", "==" });
            comboBoxCond.SelectedIndex = 0;

            SignalView.GSMSignaler.OnAdd += (s) =>
            {
                UpdateGridSignals();
            };
            SignalView.GSMSignaler.OnRemove += (s) =>
            {
                UpdateGridSignals();
            };

            /*GSMSignaler.OnLoad += () =>
			{
				UpdateGridSignals();
			};*/
            SignalView.GSMSignaler.Load();

            buttonSignUp.Click += (s, e) => { MoveSignal(true); };
            buttonSignDown.Click += (s, e) => { MoveSignal(false); };

            dataGridViewListSign.Click += (s, e) =>
            {
                if (s is DataGridView)
                {
                    var dataGrid = (DataGridView)s;
                    foreach (DataGridViewRow r in dataGrid.Rows)
                    {
                        if (r.Selected)
                        {
                            LastIndexSelectRow = r.Index;
                        }
                    }
                }
            };
            UpdateGridSignals();

            AutoFindPort();
        }
        /// <summary>
        /// Перемещение сигнала по списку
        /// </summary>
        /// <param name="isUp"></param>
        private void MoveSignal(bool isUp = true)
        {
            Qlog.CatchException(() =>
            {
                if (dataGridViewListSign.SelectedRows.NotIsNull())
                {
                    if (dataGridViewListSign.SelectedRows.Count > 0)
                    {
                        foreach (var row in dataGridViewListSign.SelectedRows)
                        {
                            if (row is DataGridViewRow)
                            {
                                var r = (DataGridViewRow)row;
                                if (r.Tag is SignalMarket)
                                {
                                    if (isUp)
                                    {
                                        SignalView.GSMSignaler.MoveUp((SignalMarket)r.Tag);
                                        LastIndexSelectRow--;
                                    }
                                    else
                                    {
                                        SignalView.GSMSignaler.MoveDown((SignalMarket)r.Tag);
                                        LastIndexSelectRow++;
                                    }
                                }
                            }
                        }
                    }
                }
                UpdateGridSignals();
            });
        }

        /// <summary>
        /// Автоматический поиск нужного порта и подключение к ниму.
        /// </summary>
        private void AutoFindPort()
        {
            foreach (var port in SignalPort.GetListPorts())
            {
                if (SignalView.GSMSignaler.PortDevice) break;
                if (SignalView.GSMSignaler.NotIsNull()) SignalView.GSMSignaler.Close();
                if (SignalView.GSMSignaler.Init(port))
                {
                    SignalView.GSMSignaler.SendTestSignal();
                    SignalView.GSMSignaler.sPort.OnReceived = (data, objPort, sd) =>
                    {
                        if (data.Length > 1)
                        {
                            SetPort(objPort.PortName);
                            comboBoxPorts.GuiAsync(() =>
                            {
                                comboBoxPorts.SelectedItem = objPort.PortName;
                            });
                            SignalView.GSMSignaler.PortDevice = true;
                            return;
                        }
                    };
                }
                Thread.Sleep(1000);
            }
        }

        private void SetCurrentPorts()
        {
            comboBoxPorts.DataSource = null;
            ComboBox.ObjectCollection itemsPorts = new ComboBox.ObjectCollection(comboBoxPorts);
            foreach (var port in SignalPort.GetListPorts())
            {
                itemsPorts.Add(port);
            }
            comboBoxPorts.DataSource = itemsPorts;
            comboBoxPorts.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void UpdateGridSignals()
        {
            SignalView.constructGridSignals(dataGridViewListSign, comboBoxSecSign.Text, () =>
            {
                LastIndexSelectRow = LastIndexSelectRow < 0 ? 0 : LastIndexSelectRow;
                LastIndexSelectRow = LastIndexSelectRow >= dataGridViewListSign.Rows.Count ? dataGridViewListSign.Rows.Count - 1 : LastIndexSelectRow;
                foreach (DataGridViewRow row in dataGridViewListSign.Rows)
                {
                    if (row.Index == LastIndexSelectRow)
                    {
                        row.Selected = true;
                        dataGridViewListSign.FirstDisplayedScrollingRowIndex = LastIndexSelectRow;
                    }
                }
            });
        }

        /// <summary>
        /// Проверка выполнения сигналов
        /// </summary>
        public void CheckAllSignals()
        {
            if (!SignalView.GSMSignaler.IsInit()) return;
            Qlog.CatchException(() =>
            {
                foreach (var sig in SignalView.GSMSignaler.ToArray())
                {
                    switch (sig.Type)
                    {
                        case SignalMarket.TypeSignal.ByVolume:
                            if (!sig.SecClass.Empty() && sig.Volume > 0)
                            {
                                var date = sig.TimeAntiRepeat.GetDateTime();
                                if (date <= DateTime.Now)
                                {
                                    var sec = GetSecCodeAndClass(sig.SecClass);
                                    if (sec.NotIsNull())
                                    {
                                        var trElem = DataTrading.Collection.FirstOrDefault(c => c.Security == sec);
                                        if (trElem.NotIsNull())
                                        {
                                            var timeFrameCol = trElem.CollectionTimeFrames.FirstOrDefault(tf => tf.TimeFrame == sig.TimeFrame);
                                            if (timeFrameCol.NotIsNull())
                                            {
                                                var candleCur = timeFrameCol.FirstCandle;
                                                if (candleCur.NotIsNull())
                                                {
                                                    if (candleCur.Volume >= sig.Volume && sig.TimeAntiRepeat.GetDateTime() < DateTime.Now)
                                                    {
                                                        sig.TimeAntiRepeat.SetDateTime(DateTime.Now.AddMinutes(sig.TimeFrame));
                                                        SignalView.GSMSignaler.SendSignalCall();
                                                        if (!sig.Infinity) SignalView.GSMSignaler.RemoveSignal(sig);
                                                        textBoxLogSign.Text = "Volume: " + sig.SecClass + " > " + sig.Volume.ToString() + " tf:" + sig.TimeFrame + " [" + DateTime.Now.ToString() + "] (" + sig.Comment + ")" +
                                                            "\r\n-------------------------------\r\n" +
                                                            textBoxLogSign.Text;
                                                        Form_MessageSignal.Show(textBoxLogSign.Text);
                                                        Thread.Sleep(1);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case SignalMarket.TypeSignal.ByTime:
                            if (sig.DateTime.NotIsNull())
                            {
                                if (sig.Condition == SignalMarket.CondSignal.Equals)
                                {
                                    var dateSign = sig.DateTime.GetDateTime();
                                    var d1 = DateTime.Now.AddSeconds(-1);
                                    var d2 = d1.AddSeconds(2);
                                    if (dateSign.Hour == d1.Hour && dateSign.Minute == d1.Minute
                                        && dateSign.Second > d1.Second && dateSign.Second < d2.Second)
                                    {
                                        if (!sig.Infinity)
                                        {
                                            SignalView.GSMSignaler.RemoveSignal(sig);
                                        }
                                        string msg = "Time: " + sig.DateTime.GetDateTime().ToShortTimeString() + " (" + sig.Comment + ")" +
                                            "\r\n-------------------------------\r\n";
                                        textBoxLogSign.Text = msg + textBoxLogSign.Text;
                                        Form_MessageSignal.Show(msg, true);
                                        Thread.Sleep(1);
                                    }
                                }
                            }
                            break;
                        case SignalMarket.TypeSignal.ByPrice:
                            if (!sig.SecClass.Empty())
                            {
                                var sec = GetSecCodeAndClass(sig.SecClass);

                                if (sec.NotIsNull() && sec.LastTrade.NotIsNull())
                                {
                                    bool signal = false;
                                    string cond = "";
                                    switch (sig.Condition)
                                    {
                                        case SignalMarket.CondSignal.MoreOrEquals:
                                            if (sec.LastPrice >= sig.Price)
                                            {
                                                SignalView.GSMSignaler.SendSignalCall();
                                                signal = true;
                                                cond = ">=";
                                            }
                                            break;
                                        case SignalMarket.CondSignal.More:
                                            if (sec.LastPrice > sig.Price)
                                            {
                                                SignalView.GSMSignaler.SendSignalCall();
                                                signal = true;
                                                cond = ">";
                                            }
                                            break;
                                        case SignalMarket.CondSignal.LessOrEquals:
                                            if (sec.LastPrice <= sig.Price)
                                            {
                                                SignalView.GSMSignaler.SendSignalCall();
                                                signal = true;
                                                cond = "<=";
                                            }
                                            break;
                                        case SignalMarket.CondSignal.Less:
                                            if (sec.LastPrice < sig.Price)
                                            {
                                                SignalView.GSMSignaler.SendSignalCall();
                                                signal = true;
                                                cond = "<";
                                            }
                                            break;
                                        case SignalMarket.CondSignal.Equals:
                                            if (sec.LastPrice < sig.Price)
                                            {
                                                SignalView.GSMSignaler.SendSignalCall();
                                                signal = true;
                                                cond = "==";
                                            }
                                            break;
                                    }
                                    if (signal)
                                    {
                                        var msg = DateTime.Now.ToLongTimeString() + " -> " +
                                            sec.Code + " " + cond + " " + sig.Price.ToString() + "(" + sig.Comment + ")" +
                                            "\r\n";
                                        textBoxLogSign.Text = msg + textBoxLogSign.Text;
                                        Form_MessageSignal.Show(msg);
                                        SignalView.GSMSignaler.RemoveSignal(sig);
                                        Thread.Sleep(1);
                                    }
                                }
                            }
                            break;
                    }
                }
            });
        }
        /// <summary>
        /// Обработчик получения данных из порта
        /// </summary>
        /// <param name="data"></param>
        /// <param name="e"></param>
        private void DataFromPorts(string data, SerialPort port, SerialDataReceivedEventArgs e)
        {
            textBoxLogDev.GuiAsync(() =>
            {
                ReceiveData(data);
                textBoxLogDev.Text = data + "\r\n" + textBoxLogDev.Text;
            });
        }

        /// <summary>
        /// Последний входящий вызов
        /// </summary>
        private DateTime LastIncomingCall = DateTime.Now.AddMinutes(-20);
        private int CounterCalls = 0;

        private void ReceiveData(string data)
        {
            if (data.Contains("RING") && data.Contains("CLIP"))
            {
                if (data.Contains(SignalView.GSMSignaler.Phone.Replace("+", "")))
                {
                    if (LastIncomingCall > DateTime.Now.AddSeconds(-60))
                    {
                        CounterCalls++;
                        if (CounterCalls == 8)
                        {
                            MProcess.CloseTerminal();
                            SignalView.GSMSignaler.SendSignalResetCall();
                        }
                    }
                    else
                    {
                        LastIncomingCall = DateTime.Now;
                        CounterCalls = 1;
                    }
                }
            }
        }


        private void buttonAddSign_Click(object sender, EventArgs e)
        {
            SignalMarket.CondSignal cond = SignalMarket.CondSignal.MoreOrEquals; ;
            var strCond = (string)comboBoxCond.SelectedItem;
            if (strCond == ">=")
                cond = SignalMarket.CondSignal.MoreOrEquals;
            else if (strCond == ">")
                cond = SignalMarket.CondSignal.More;
            else if (strCond == "<=")
                cond = SignalMarket.CondSignal.LessOrEquals;
            else if (strCond == "<")
                cond = SignalMarket.CondSignal.Less;
            else if (strCond == "==")
                cond = SignalMarket.CondSignal.Equals;

            var sec = LastSecSignal.NotIsNull() ? LastSecSignal.ToString() : null;
            var newSign = new SignalMarket()
            {
                Type = SignalMarket.TypeSignal.ByPrice,
                SecClass = sec,
                Price = numericUpDownPrice.Value,
                Condition = cond,
                Comment = textBoxSignComment.Text
            };
            SignalView.GSMSignaler.AddSignal(newSign);
            textBoxSignComment.Text = "";
        }

        private void buttonAddSignTime_Click(object sender, EventArgs e)
        {
            var newSign = new SignalMarket()
            {
                Type = SignalMarket.TypeSignal.ByTime,
                DateTime = new DateMarket(dateTimePickerSign.Value),
                Condition = SignalMarket.CondSignal.Equals,
                Comment = textBoxSignComment.Text,
                Infinity = true
            };
            SignalView.GSMSignaler.AddSignal(newSign);
            textBoxSignComment.Text = "";
        }

        private void buttonAddSignVolume_Click(object sender, EventArgs e)
        {
            Qlog.CatchException(() =>
            {
                //if (LastSecSignal.IsNull()) return;
                int timeFrame = 1;
                if (comboBoxSignTimeFrame.SelectedItem is SelectorTimeFrame)
                    timeFrame = ((SelectorTimeFrame)comboBoxSignTimeFrame.SelectedItem).TimeFrame;
                DateMarket date = new DateMarket(DateTime.Now);
                var newSign = new SignalMarket()
                {
                    Type = SignalMarket.TypeSignal.ByVolume,
                    SecClass = LastSecSignal.ToString(),
                    Volume = Convert.ToInt64(numericUpDownSignVolume.Value),
                    TimeFrame = timeFrame,
                    TimeAntiRepeat = date,
                    Comment = textBoxSignComment.Text,
                    Infinity = true
                };
                SignalView.GSMSignaler.AddSignal(newSign);
                textBoxSignComment.Text = "";
            });
        }

        private void buttonDelSign_Click(object sender, EventArgs e)
        {
            Qlog.CatchException(() =>
            {
                //if (LastSecSignal.IsNull()) return;
                if (dataGridViewListSign.SelectedRows.NotIsNull())
                {
                    if (dataGridViewListSign.SelectedRows.Count > 0)
                    {
                        foreach (var row in dataGridViewListSign.SelectedRows)
                        {
                            if (row is DataGridViewRow)
                            {
                                var r = (DataGridViewRow)row;
                                if (r.Tag is SignalMarket)
                                {
                                    LastIndexSelectRow = r.Index - 1;
                                    SignalView.GSMSignaler.RemoveSignal((SignalMarket)r.Tag);
                                }
                            }
                        }
                    }
                }
            });
        }
        private void buttonLastPrice_Click(object sender, EventArgs e)
        {
            if (LastSecSignal.IsNull()) return;
            numericUpDownPrice.Value = LastSecSignal.LastPrice.NotIsNull() ?
                LastSecSignal.LastPrice : 0;
        }

        private void buttonClearSignalLog_Click(object sender, EventArgs e)
        {
            textBoxLogSign.Text = "";
        }

        private void buttonRestartSignalPort_Click(object sender, EventArgs e)
        {
            Qlog.CatchException(() =>
            {
                if (comboBoxPorts.SelectedItem.NotIsNull())
                {
                    string port = (string)comboBoxPorts.SelectedItem;
                    SetPort(port);
                }
                SetCurrentPorts();
            });
        }

        private void SetPort(string namePort)
        {
            if (SignalView.GSMSignaler.NotIsNull()) SignalView.GSMSignaler.Close();
            if (SignalView.GSMSignaler.Init(namePort))
            {
                SignalView.GSMSignaler.sPort.OnReceived = null;
                SignalView.GSMSignaler.sPort.OnReceived = DataFromPorts;
            }
            this.GuiAsync(() =>
            {
                buttonRestartSignalPort.Enabled = true;
                buttonSignTestCall.Enabled = true;
                buttonSignTestDev.Enabled = true;

                textBoxLogDev.Text = "Init port " + namePort + "\r\n" + textBoxLogDev.Text;

                tableLayoutPanel14.Enabled = true;
                tableLayoutPanel8.Enabled = true;
                splitContainer2.Enabled = true;
            });
        }

        private void buttonSignTestCall_Click(object sender, EventArgs e)
        {
            SignalView.GSMSignaler.SendSignalCall();
            MThread.InitThread(() =>
            {
                Thread.Sleep(15000);
                SignalView.GSMSignaler.SendSignalResetCall();
            });
        }

        private void buttonSignTestDev_Click(object sender, EventArgs e)
        {
            SignalView.GSMSignaler.SendTestSignal();
            MThread.InitThread(() =>
            {
                Thread.Sleep(1000);
                SignalView.GSMSignaler.SendTestSignal(false);
            });
        }
    }
}
