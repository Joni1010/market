using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AppVEConector.libs;
using Managers;
using AppVEConector.libs.Signal;
using System.Windows.Threading;
using Connector.Logs;
using AppVEConector.settings;

namespace AppVEConector.Forms.StopOrders
{
    public partial class Form_LightOrders : ControlForFormStopOrders
    {
        private const char SPLITTER_INDEXDATA = '#';

        public Form_CommonSettingsStopOrders FormSettings = null;

        private int stepsHide = 0;

        public SettingsLightOrders Settings = null;

        

        public Form_LightOrders(MainForm parent)
        {
            InitializeComponent();
            var rootDir = Global.GetPathData();
            var filename = rootDir + "\\" + "lord_settings.dat";
            Settings = SettingsLightOrders.New(filename);
            Parent = parent;
            searchAllElementsPanels(this);
        }

        private void Form_LightOrders_Load(object sender, EventArgs e)
        {
            this.ChangeOrientation();
            eventInitPanel += initPanel;
            InitPanels();
        }

        private void initPanel()
        {
            this.buttonCloseWin.Click += (s, e) => { this.Close(); };

            buttonSaveStruct.Click += (s, e) =>
            {
                Qlog.CatchException(() =>
                {
                    SaveStructPanels();
                });
            };
            buttonLoadStruct.Click += (s, e) =>
            {
                Qlog.CatchException(() =>
                {
                    LoadStructPanels();
                });
            };
            buttonChangeScreen.Click += (s, e) =>
            {
                Qlog.CatchException(() =>
                {
                    var numPos = Settings.Get("numberPosition");
                    Settings.Set("numberPosition", numPos + 1);
                    ChangeOrientation();
                });
            };
            buttonCommonSettings.Click += (s, e) =>
            {
                Qlog.CatchException(() =>
                {
                    if (FormSettings.IsNull())
                    {
                        FormSettings = new Form_CommonSettingsStopOrders(this);
                        FormSettings.Show();
                        FormSettings.TopMost = true;
                        FormSettings.FormClosed += (ss, ee) =>
                        {
                            updateAllComboBoxSec();
                            FormSettings = null;
                        };
                    }
                });
            };
            Action<DispatcherTimer> event100ms = (timer) =>
            {
                if (this.WindowState != FormWindowState.Minimized)
                {
                    var cur = Cursor.Position;
                    if (cur.X > this.Location.X
                        && cur.X < this.Location.X + this.Size.Width)
                    {
                        stepsHide = 0;
                        if (!this.TopMost)
                        {
                            this.TopMost = true;
                        }
                    }
                }
            };
            Parent.OnTimer100ms += event100ms;

            Action<DispatcherTimer> event1s = (timer) =>
            {
                if (this.WindowState != FormWindowState.Minimized && this.TopMost)
                {
                    if (stepsHide > 20)
                    {
                        this.TopMost = false;
                    }
                    stepsHide++;
                }
            };
            Parent.OnTimer1s += event1s;
            this.FormClosed += (s, e) =>
            {
                if (FormSettings.NotIsNull())
                {
                    FormSettings.Close();
                }
                Parent.OnTimer100ms -= event100ms;
                Parent.OnTimer1s -= event1s;
            };
            updateAllComboBoxSec();
        }

        private void updateAllComboBoxSec()
        {
            Qlog.CatchException(() =>
            {
                foreach (var pan in ListPanels)
                {
                    if (pan.ComboboxSecurity.NotIsNull())
                    {
                        pan.ComboboxSecurity.DataSource = null;
                        pan.ComboboxSecurity.Clear();

                        ComboBox.ObjectCollection items = new ComboBox.ObjectCollection(pan.ComboboxSecurity);
                        items.Insert(0, " ");
                        foreach (var itemSec in Settings.Get("ItemsSec"))
                        {
                            items.Add(itemSec.SecAndClass);
                        }
                        pan.ComboboxSecurity.DataSource = items;
                    }
                }
                LoadStructPanels();
            });
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChangeOrientation()
        {
            this.TopMost = true;
            var point = getCoordinateForm();
            this.SetBounds(point.X, point.Y, this.Width, this.Height);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Point getCoordinateForm()
        {
            var numPos = Settings.Get("numberPosition");
            if (numPos > 1)
            {
                Settings.Set("numberPosition", 0);
                Settings.Set("indexScreen", Settings.Get("indexScreen") + 1);
            }
            var screen = GetScreen();
            var rectScreen = screen.Bounds;
            var widthScreen = rectScreen.Width;
            var heightScreen = rectScreen.Height;
            var top = 0;
            var left = 0;

            if (Settings.Get("numberPosition") == 0)
            {
                top = this.Location.Y + (int)(heightScreen / 2) - (int)(this.Height / 2);
                left = this.Location.X + widthScreen - this.Width;
            }
            else if (Settings.Get("numberPosition") == 1)
            {
                top = this.Location.Y + (int)(heightScreen / 2) - (int)(this.Height / 2);
                left = this.Location.X + 0;
            }
            return new Point(left, top);
        }

        private Screen GetScreen()
        {
            if (Screen.AllScreens.Length <= Settings.Get("indexScreen"))
            {
                Settings.Set("indexScreen", 0);
            }
            var screen = Screen.AllScreens[Settings.Get("indexScreen")];
            Location = screen.WorkingArea.Location;
            return screen;
        }

        private void Form_LightOrders_Move(object sender, EventArgs e)
        {
            this.Text = "Controller stop orders";
        }




        /// <summary>
        /// Сохраняет структуру панелей с инструментами
        /// </summary>
        private void SaveStructPanels()
        {
            foreach (var pan in this.ListPanels)
            {
                if (pan.ComboboxSecurity.Text.Length > 1)
                {
                    string[] list = Settings.Get("StructPanels");
                    list[pan.Index] = pan.Index.ToString() + SPLITTER_INDEXDATA + pan.ComboboxSecurity.Text;
                    Settings.Set("StructPanels", list);
                }
            }
            Settings.Save();
        }
        /// <summary>
        /// Загружает структуру панелей с инструментами
        /// </summary>
        private void LoadStructPanels()
        {
            string[] list = Settings.Get("StructPanels");
            foreach (var line in list)
            {
                if (!line.Empty())
                {
                    if (line.Contains(SPLITTER_INDEXDATA))
                    {
                        var data = line.Split(SPLITTER_INDEXDATA);
                        var panel = ListPanels.FirstOrDefault(p => p.Index == data[0].ToInt32());
                        if (panel.NotIsNull())
                        {
                            panel.ComboboxSecurity.Text = data[1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Удаляет сигнал по цене
        /// </summary>
        /// <param name="price"></param>
        private void RemoveSignal(decimal price)
        {
            var listSigDel = SignalView.GSMSignaler.ToArray().Where(s => s.Price == price);
            foreach (var sig in listSigDel)
            {
                SignalView.GSMSignaler.RemoveSignal(sig);
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

        /// <summary> Установка сообщения </summary>
        protected override void SetMessage(string text)
        {
            textBoxMsg.GuiAsync(() =>
            {
                textBoxMsg.Text = text;
            });
        }

        protected override void SetInfo(string text)
        {
            labelVarMargin.GuiAsync(() =>
            {
                labelVarMargin.Text = text;
            });
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
