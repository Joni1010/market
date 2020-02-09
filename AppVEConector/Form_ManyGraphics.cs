using GraphicTools;
using GraphicTools.Extension;
using MarketObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Threading;
using TradingLib;

namespace AppVEConector
{
	partial class Form_ManyGraphics :Form
	{
		public Form_ManyGraphics(MainForm parent)
		{
			InitializeComponent();
			this.Parent = parent;
			this.Trader = this.Parent.Trader;
		}

		private Connector.QuikConnector Trader = null;
		private new MainForm Parent = null;

		/// <summary> Набор тайм фреймов </summary>
		public class PanelGraph
		{
			public int Index = -1;
			public int CountCandleVisible = 45;
			/// <summary>
			/// Выбранный индекс тайм-фрейма в кобобоксе
			/// </summary>
			public int SelectIndexTimeFrame = 4;
			/// <summary> Редактируемые уровни по инструменту  </summary>
			public LevelTools Levels = null;

			public PictureBox Canvas = null;
			public TElement TrElement = null;
			public ComboBox ComboBoxTimeFrame = null;
			public Graphic Graphic = null;
			public Label LabelTop = null;
			public Button ButtonMore = null;
			public Button ButtonLess = null;

			public Button ButBuy = null;
			public Button ButSell = null;
			public NumericUpDown Price = null;
			public NumericUpDown Volume = null;
			public Button ButCloseOrder = null;
			public Button OpenDepth = null;
			public Label LabelOrders = null;

			public Label Position = null;
		}
		/// <summary>
		/// Список всех панелей
		/// </summary>
		private List<PanelGraph> ListGraph = new List<PanelGraph>();



		private void Init()
		{
			this.FindElemPanels(this);

			this.FirstStart();

			this.initTimers();
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
				var el = this.ListGraph.FirstOrDefault(p => p.Index == index);
				if (el.IsNull())
				{
					el = new PanelGraph();
					this.ListGraph.Add(el);
					el.Index = index;
				}
				if (control is ComboBox)
				{
					el.ComboBoxTimeFrame = (ComboBox)control;
				}
				if (control is PictureBox)
				{
					el.Canvas = (PictureBox)control;
				}
				if (control is Label)
				{
					if (strTag.Contains("Ord")) el.LabelOrders = (Label)control;
					if (strTag.Contains("LT")) el.LabelTop = (Label)control;
					if (strTag.Contains("Pos")) el.Position = (Label)control;
				}
				if (control is Button)
				{
					var b = (Button)control;
					if (strTag.Contains("CL")) el.ButCloseOrder = b;
					if (b.Text == "+") el.ButtonMore = b;
					if (b.Text == "-") el.ButtonLess = b;
					if (b.Text == "sell") el.ButSell = b;
					if (b.Text == "buy") el.ButBuy = b;
					if (strTag.Contains("Depth")) el.OpenDepth = b;
				}
				if (control is NumericUpDown)
				{
					if (strTag.Contains("V"))
					{
						el.Volume = (NumericUpDown)control;
						el.Volume.InitWheelDecimal();
					}
					if (strTag.Contains("P"))
					{
						el.Price = (NumericUpDown)control;
						el.Price.InitWheelDecimal();
						el.Price.Maximum = 100000000;
					}
				}
			}
		}
		/// <summary>
		/// Метод первоначальных настроек
		/// </summary>
		private void FirstStart()
		{
			var tf = SelectorTimeFrame.GetAll();

			foreach (var panel in this.ListGraph)
			{
				if (panel.ComboBoxTimeFrame.NotIsNull())
				{
					panel.ComboBoxTimeFrame.DataSource = tf.ToArray();
					panel.ComboBoxTimeFrame.SelectedIndex = panel.SelectIndexTimeFrame;
				}
				panel.Canvas.Paint += (s, e) =>
				{
					if (panel.Graphic.NotIsNull())
					{
						var g = e.Graphics;
						panel.Graphic.AllToCanvas(g);
					}
				};
                panel.Graphic.SetObjectPaint(panel.Canvas);
				//Клик по графику для указания цены
				panel.Canvas.Click += (s, e) =>
				{
					if (panel.TrElement.IsNull()) return;
					panel.Price.Value = panel.TrElement.Security.LastTrade.NotIsNull() ?
						panel.TrElement.Security.LastTrade.Price : 0;
				};
				//увеличить график
				panel.ButtonLess.Click += (s, e) =>
				{
					panel.CountCandleVisible += 3;
					if (panel.CountCandleVisible > 1000) panel.CountCandleVisible = 1000;
					this.UpdateGraphic(panel);
				};
				//уменьшить график
				panel.ButtonMore.Click += (s, e) =>
				{
					panel.CountCandleVisible -= 3;
					if (panel.CountCandleVisible < 5) panel.CountCandleVisible = 5;
					this.UpdateGraphic(panel);
				};
				panel.ButBuy.Click += (s, e) =>
				{
					this.CreateOrder(panel, true);
				};
				panel.ButSell.Click += (s, e) =>
				{
					this.CreateOrder(panel, false);
				};
				panel.ButCloseOrder.Click += (s, e) =>
				{
					if (panel.TrElement.IsNull()) return;
					this.Trader.CancelAllOrder(panel.TrElement.Security);
				};

				panel.OpenDepth.Click += (s, e) =>
				{
					if (this.Trader.IsNull()) return;
					if (panel.TrElement.IsNull()) return;
					this.Parent.ShowGraphicDepth(panel.TrElement.Security);
				};
			}
			int ind = 1;
			foreach (var sec in LoadListTradeSec())
			{
				SetSecurityInPanel(this.ListGraph.FirstOrDefault(p => p.Index == ind), sec);
				ind++;
			}

			//SetSecurityInPanel(this.ListGraph.FirstOrDefault(p => p.Index == 1), "SiZ7", "SPBFUT");
			//SetSecurityInPanel(this.ListGraph.FirstOrDefault(p => p.Index == 2), "RIZ7", "SPBFUT");
			//SetSecurityInPanel(this.ListGraph.FirstOrDefault(p => p.Index == 3), "BRF8", "SPBFUT");
		}

		private void CreateOrder(PanelGraph panel, bool IsBuy)
		{
			if (panel.TrElement.IsNull()) return;
			if (panel.Price.Value == 0) return;
			if (panel.Volume.Value == 0) return;
			var ord = new Order()
			{
				Price = panel.Price.Value,
				Volume = Convert.ToInt32(panel.Volume.Value),
				Sec = panel.TrElement.Security,
				Direction = IsBuy ? OrderDirection.Buy : OrderDirection.Sell
			};
			this.Trader.CreateOrder(ord);
		}

		private void SetSecurityInPanel(PanelGraph panel, Securities sec)
		{
			this.SetSecurityInPanel(panel, sec.Code, sec.Class.Code);
		}
		private void SetSecurityInPanel(PanelGraph panel, string secCode, string secClass)
		{
			if (panel.IsNull()) return;

			var TrEl = this.Parent.GetTradeElement(secCode, secClass);
			if (TrEl.IsNull()) return;
			panel.TrElement = TrEl;
			//Уровни

			panel.Graphic = new Graphic();
            panel.Graphic.InitEvents();
            panel.Graphic.Init(TrEl.Security);
			panel.LabelTop.Text = TrEl.Security.ToString();
			panel.Price.InitSecurity(TrEl.Security);
			panel.Price.Value = 0;
			var pos = this.Parent.GetPosition(panel.TrElement.Security);
			panel.Position.Text = pos.NotIsNull() ? pos.Data.CurrentNet.ToString() : "0";

			this.UpdateGraphic(panel);

			if (panel.TrElement.IsNull()) return;
			if (panel.TrElement.Security.IsNull()) return;
		}

		private void initTimers()
		{
			EventHandler eventLastPrice1s = (s, e) =>
			{
				foreach (var panel in this.ListGraph)
				{
					this.UpdatePanel(panel);
					this.UpdateGraphic(panel);
				}
			};
			DispatcherTimer dispatcherTimer1s = new DispatcherTimer();
			dispatcherTimer1s.Tick += new EventHandler(eventLastPrice1s);
			dispatcherTimer1s.Interval = new TimeSpan(0, 0, 0, 0, 500);
			dispatcherTimer1s.Start();
		}
		/// <summary> Получает текущий тайм-фрем в панеле </summary>
		/// <param name="panel"></param>
		/// <returns></returns>
		private int GetCurrentTimeFrame(PanelGraph panel)
		{
			if (panel.ComboBoxTimeFrame.IsNull()) return SelectorTimeFrame.TimeFrameMinute[0];
			if (panel.ComboBoxTimeFrame.SelectedItem.IsNull()) return SelectorTimeFrame.TimeFrameMinute[0];
			return ((SelectorTimeFrame)panel.ComboBoxTimeFrame.SelectedItem).TimeFrame;
		}

		public void UpdatePanel(PanelGraph panel)
		{
			if (panel.IsNull()) return;
			if (panel.TrElement.IsNull()) return;
			var pos = this.Parent.GetPosition(panel.TrElement.Security);
			panel.Position.Text = pos.NotIsNull() ? pos.Data.CurrentNet.ToString() : "0";
			panel.LabelOrders.Text = (pos.NotIsNull() ? pos.Data.OrdersBuy.ToString() : "0") + " / " +
				(pos.NotIsNull() ? pos.Data.OrdersSell.ToString() : "0");

			if (this.Parent.LastMarketReply.NotIsNull())
				if (this.Text != this.Parent.LastMarketReply.ResultMsg)
					this.Text = this.Parent.LastMarketReply.ResultMsg;
		}

		public void UpdateGraphic(PanelGraph panelGraph, bool updateAll = false)
		{
			if (panelGraph.TrElement.IsNull()) return;
			if (panelGraph.Graphic.IsNull()) return;
			//try
			var valTF = this.GetCurrentTimeFrame(panelGraph);
			var timeFrame = panelGraph.TrElement.CollectionTimeFrames.FirstOrDefault(tf => tf.TimeFrame == valTF);
			if (!timeFrame.IsNull())
			{
				int index = 0;// GetIndexFirstCandle(timeFrame);

				//Orders
				List<MarketObjects.Chart> orders = new List<MarketObjects.Chart>();
				var allOrd = this.Trader.Objects.Orders.ToArray().Where(o => o.Sec.Code == panelGraph.TrElement.Security.Code && o.IsActive());
				foreach (var o in allOrd)
				{
					var ch = orders.FirstOrDefault(c => c.Price == o.Price);
					var vol = o.IsSell() ? o.Volume * -1 : o.Volume;
					if (ch != null) ch.Volume += vol;
					else orders.Add(new MarketObjects.Chart() { Price = o.Price, Volume = vol });
				}
				var allStOrd = this.Trader.Objects.StopOrders.ToArray().Where(o => o.Sec.Code == panelGraph.TrElement.Security.Code && o.IsActive());
				foreach (var o in allStOrd)
				{
					var ch = orders.FirstOrDefault(c => c.Price == o.Price);
					var vol = o.IsSell() ? o.Volume * -1 : o.Volume;
					if (ch != null) ch.Volume += vol;
					else orders.Add(new MarketObjects.Chart() { Price = o.ConditionPrice, Volume = vol });
				}
				//panelGraph.Graphic.SetOrders(orders);
				//panelGraph.Graphic.SetLevels(panelGraph.Levels.Collection);

				panelGraph.Canvas.GuiAsync(() =>
				{
					int countCandles = panelGraph.CountCandleVisible;

                    panelGraph.Graphic.IndexFirstCandle = index;
                    panelGraph.Graphic.CountVisibleCandles = countCandles;
                    //panelGraph.Graphic.PanelCandels.CollectionCandle = timeFrame.MainCollection.ToArray().Skip(index).Take(countCandles).ToArray();

					panelGraph.Canvas.Refresh();
				});
			}
		}

		/// <summary> Загружаем торгуемые элементы из файла </summary>
		private IEnumerable<Securities> LoadListTradeSec()
		{
			System.IO.StreamReader openFile = new System.IO.StreamReader(@"market.list", true);
			List<Securities> list = new List<Securities>();
			while (!openFile.EndOfStream)
			{
				string line = openFile.ReadLine();
				if (!line.Empty())
				{
					string[] el = line.Split(':');
					if (el.Length > 0)
					{
						if (!el[0].Empty() && !el[1].Empty())
						{
							var sec = Trader.Objects.Securities.FirstOrDefault(s => s.Code == el[0] && s.Class.Code == el[1]);
							if (sec.NotIsNull()) list.Add(sec);

						}
					}
				}
			}
			return list;
		}


		private void Form_ManyGraphics_Load(object sender, EventArgs e)
		{
			this.Init();
		}
	}
}
