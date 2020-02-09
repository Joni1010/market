using System.Linq;
using System.Windows.Forms;
using System.Windows.Threading;
using TradingLib;
using System.Collections.Generic;
using System;
using MarketObjects;
using Market.Candles;
using GraphicTools.Extension;

namespace AppVEConector
{
	public partial class Form_Strategy_morePrev :Form
	{
		public Connector.QuikConnector Trader = null;

		const int IntervalTimer = 60;    //sec
										 /// <summary> Контролируемый тайм-фрейм </summary>
		private int TimeFrameBuy = 0;
		private int TimeFrameSell = 0;
		public TElement TrElement = null;

		//DispatcherTimer MainTimer = null;
		/// <summary>
		/// Список сигналов
		/// </summary>
		public List<Signal> ListSignals = new List<Signal>();
		/// <summary>
		/// Хранимые настройки стратегий BUY
		/// </summary>
		public StructStrategy StarategyBuy = new StructStrategy();
		/// <summary>
		/// Хранимые настройки стратегий SELL
		/// </summary>
		public StructStrategy StarategySell = new StructStrategy();
		/// <summary>
		/// Текущий инструмент
		/// </summary>
		public Securities Securities
		{
			get
			{
				return this.TrElement.Security.IsNull() ? null : this.TrElement.Security;
			}
		}
		/// <summary>
		/// Минимальный шаг цены
		/// </summary>
		public decimal MinStepPrice
		{
			get
			{
				return this.TrElement.Security.IsNull() ? 0 : this.TrElement.Security.Params.MinPriceStep;
			}
		}
		/// <summary>
		/// текущая цена по инструменту
		/// </summary>
		public decimal CurrentPrice
		{
			get
			{
				return this.TrElement.Security.IsNull() && this.TrElement.Security.LastTrade.NotIsNull() ?
					0 : this.TrElement.Security.LastTrade.Price;
			}
		}


		public Form_Strategy_morePrev(Connector.QuikConnector trader, TElement trEl)
		{
			this.Trader = trader;
			InitializeComponent();
			this.TrElement = trEl;
			this.Text = trEl.Security.ToString();
			this.Init();
		}

		public void Init()
		{
			this.comboBoxTFBuy.SelectedIndexChanged += (s, e) =>
			{
				var sender = (ComboBox)s;
				this.TimeFrameBuy = ((SelectorTimeFrame)sender.SelectedItem).TimeFrame;
			};
			this.comboBoxTFBuy.SelectedIndexChanged += (s, e) =>
			{
				var sender = (ComboBox)s;
				this.TimeFrameSell = ((SelectorTimeFrame)sender.SelectedItem).TimeFrame;
			};

			var tf = SelectorTimeFrame.GetAll();

			this.comboBoxTFBuy.DataSource = tf.ToArray();
			this.comboBoxTFSell.DataSource = tf.ToArray();
			this.comboBoxTFBuy.SelectedIndex = 4;
			this.comboBoxTFSell.SelectedIndex = 4;


			this.InitBorderPrices();

			button1.Click += (s, e) =>
			{
				this.EmulateSignal();
			};

			EventHandler eventLastPrice1s = (s, e) =>
			{
				this.CancelOldOrders();
			};
			DispatcherTimer MainTimer = new DispatcherTimer();
			MainTimer.Tick += new EventHandler(eventLastPrice1s);
			MainTimer.Interval = new TimeSpan(0, 0, IntervalTimer);
			MainTimer.Start();

			this.labelMinStepPrice.Text = this.TrElement.Security.Params.MinPriceStep.ToString();
			this.labelPriceTick.Text = this.TrElement.Security.Params.StepPrice.ToString();
		}

		private void InitBorderPrices()
		{
			EventHandler doubleClick = (s, e) =>
			{
				if (s is NumericUpDown)
				{
					var el = (NumericUpDown)s;
					el.Value = this.CurrentPrice;
				}
			};
			//buy
			this.numericUpDownBorderHighBuy.InitWheelDecimal();
			this.numericUpDownBorderHighBuy.InitSecurity(this.Securities);
			this.numericUpDownBorderHighBuy.DoubleClick += doubleClick;

			this.numericUpDownBorderLowBuy.InitWheelDecimal();
			this.numericUpDownBorderLowBuy.InitSecurity(this.Securities);
			this.numericUpDownBorderLowBuy.DoubleClick += doubleClick;

			//sell
			this.numericUpDownBorderHighSell.InitWheelDecimal();
			this.numericUpDownBorderHighSell.InitSecurity(this.Securities);
			this.numericUpDownBorderHighSell.DoubleClick += doubleClick;

			this.numericUpDownBorderLowSell.InitWheelDecimal();
			this.numericUpDownBorderLowSell.InitSecurity(this.Securities);
			this.numericUpDownBorderLowSell.DoubleClick += doubleClick;
		}

		public void CancelOldOrders()
		{
			var ordersForCancel = this.Trader.Objects.Orders.Where(o => o.Comment.Contains(StructStrategy.PrefixComment) &&
				o.Sec == this.TrElement.Security && o.IsActive());
			if (ordersForCancel.NotIsNull())
			{
				var ordBuy = ordersForCancel.Where(o => o.IsBuy());
				if (ordBuy.NotIsNull())
				{
					foreach (var ord in ordersForCancel)
					{
						if (ord.DateCreateOrder.GetDateTime().AddMinutes(this.TimeFrameBuy * StructStrategy.CountPeriodCancelOrder) < DateTime.Now)
						{
							this.Trader.CancelOrder(ord);
						}
					}
				}
				var ordSell = ordersForCancel.Where(o => o.IsSell());
				if (ordBuy.NotIsNull())
				{
					foreach (var ord in ordersForCancel)
					{
						if (ord.DateCreateOrder.GetDateTime().AddMinutes(this.TimeFrameSell * StructStrategy.CountPeriodCancelOrder) < DateTime.Now)
						{
							this.Trader.CancelOrder(ord);
						}
					}
				}
			}
		}
		/// <summary>
		/// Активирует стратегию
		/// </summary>
		public void ActivateStrategy(int timeFrame)
		{
			if (TimeFrameBuy == timeFrame || TimeFrameSell == timeFrame)
				this.PrepareStrategy();
		}

		public void PrepareStrategy()
		{
			if (this.TrElement.IsNull()) return;

			if (this.checkBoxBuy.Checked)
			{
				var collectionCandle = TrElement.CollectionTimeFrames.FirstOrDefault(t => t.TimeFrame == TimeFrameBuy);
				if (collectionCandle.NotIsNull()) this.forBuy(collectionCandle);
			}
			if (this.checkBoxSell.Checked)
			{
				var collectionCandle = TrElement.CollectionTimeFrames.FirstOrDefault(t => t.TimeFrame == TimeFrameSell);
				if (collectionCandle.NotIsNull()) this.forSell(collectionCandle);
			}
		}

		public void EmulateSignal()
		{
			var collectionCandle = TrElement.CollectionTimeFrames.FirstOrDefault(t => t.TimeFrame == TimeFrameBuy);

			var candlePrev = collectionCandle.GetElement(2);
			var candleCurr = collectionCandle.GetElement(1);

			if (checkBoxAccBuy.Checked)
			{
				this.TraderBuy(candlePrev, candleCurr);
			}
		}

		public void forBuy(CandleCollection collectionCandle)
		{
			this.GuiAsync(() =>
			{
				if (collectionCandle.Count > 3)
				{
					var candlePrev = collectionCandle.GetElement(2);
					var candleCurr = collectionCandle.GetElement(1);
					if (candlePrev.NotIsNull() && candleCurr.NotIsNull())
						StrategyBuy(candlePrev, candleCurr);
				}
			});
		}

		public void forSell(CandleCollection collectionCandle)
		{
			this.GuiAsync(() =>
			{
				if (collectionCandle.Count > 3)
				{
					var candlePrev = collectionCandle.GetElement(2);
					var candleCurr = collectionCandle.GetElement(1);
					if (candlePrev.NotIsNull() && candleCurr.NotIsNull())
						StrategySell(candlePrev, candleCurr);
				}
			});
		}

		public void StrategyBuy(CandleData candlePrev, CandleData candleCurr)
		{
			if (candlePrev.Open < candleCurr.Close)
			{
				//Попадаение в границы high
				if (checkBoxBorderHighBuy.Checked)
					if (this.numericUpDownBorderHighBuy.Value < this.CurrentPrice) return;
				//Попадаение в границы low
				if (checkBoxBorderLowBuy.Checked)
					if (this.numericUpDownBorderLowBuy.Value > this.CurrentPrice) return;

				//тело должно быть большим 3 единицы
				if (candlePrev.Open - candlePrev.Close >= this.MinStepPrice * StructStrategy.MinSizeBodyCandle)
				{
					if (candlePrev.Open > candlePrev.Close && candleCurr.Open < candleCurr.Close)
					{
						if (candlePrev.Volume < candleCurr.Volume)
						{
							if (candlePrev.Volume + candleCurr.Volume > this.numericUpDownBuy.Value)
							{
								var text = "";
								text += candlePrev.Time.ToShortTimeString() + " -> " + candleCurr.Time.ToShortTimeString() + " ";
								if (this.TrElement.Security.LastTrade.NotIsNull())
								{
									text += this.TrElement.Security.LastTrade.Price.ToString() + " B";
								}
								else text += "not lastPrice";
								text += " " + this.TimeFrameBuy.ToString() + "m\r\n";
								textBoxLogBuy.Text = text + textBoxLogBuy.Text;
								this.WriteSignalToFile(this.TrElement.Security, DateTime.Now.ToString() + " " + text);
								//Торговля
								if (checkBoxAccBuy.Checked)
								{
									this.TraderBuy(candlePrev, candleCurr);
								}
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Функция торгующая BUY
		/// </summary>
		public void TraderBuy(CandleData candlePrev, CandleData candleCurr)
		{
			try
			{
				if (this.Trader.IsNull()) return;

				//разрешаем если позиция 0
				if (this.isPosition()) return;

				//получаем цену ордера
				StarategyBuy.StepDigress = 2;
				//цена ордера
				decimal priceOrder = candlePrev.High;

				var order = new Order()
				{
					Price = priceOrder,
					Volume = Convert.ToInt32(this.numericUpDownBVol.Value),
					Direction = OrderDirection.Buy,
					Sec = this.TrElement.Security,
					SecCode = this.TrElement.Security.Code,
					Comment = StructStrategy.PrefixComment + this.Trader.Objects.MyTrades.Count().ToString()
				};
				ListSignals.Add(new Signal() { Order = order, CandlePrev = candlePrev, CandleCurr = candleCurr });
				this.Trader.CreateOrder(order);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		/// <summary>
		/// Установщик стопов buy
		/// </summary>
		/// <param name="trade"></param>
		public void SetStopOrderBuy(MyTrade trade)
		{
			try
			{
				var signal = this.ListSignals.FirstOrDefault(s => trade.Comment.Contains(s.Order.Comment)
					&& trade.Comment.Contains(StructStrategy.PrefixComment)
					&& s.Order.Sec == trade.Trade.Sec && s.Order.Direction == trade.Trade.Direction);
				if (signal.NotIsNull())
				{
					this.GuiAsync(() =>
					{
						if (this.checkBoxAutoStopBuy.Checked)
						{
							//стоп
							decimal priceStop = signal.CandlePrev.Low < signal.CandleCurr.Low ?
									signal.CandlePrev.Low : signal.CandleCurr.Low;
							//получаем цену отступа стопа
							priceStop -= StarategyBuy.StepDigress * this.TrElement.Security.Params.MinPriceStep;
							//профит 
							decimal priceProfit = trade.Trade.Price + this.numericUpDownProfitTickBuy.Value * this.TrElement.Security.Params.MinPriceStep;
							if (priceStop > 0 && priceProfit > 0)
							{
								var stop = new StopOrder()
								{
									LinkOrderPrice = priceProfit,
									ConditionPrice = priceStop,
									Price = priceStop - 20 * this.TrElement.Security.Params.MinPriceStep,
									Volume = trade.Trade.Volume,
									Direction = OrderDirection.Sell,
									Sec = this.TrElement.Security,
                                };
								this.Trader.CreateStopOrder(stop, StopOrderType.LinkOrder);
							}
						}
					});
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		/// <summary>
		/// Контроллер сделок
		/// </summary>
		/// <param name="trade"></param>
		public void CheckMyTrade(MyTrade trade)
		{
			if (trade.IsNull() || trade.Trade.Sec.IsNull()) return;

			if (trade.Trade.Sec == this.TrElement.Security && trade.Comment.Contains(StructStrategy.PrefixComment))
			{
				if (trade.Trade.IsBuy())
				{
					this.SetStopOrderBuy(trade);
				}
				else if (trade.Trade.IsSell())
				{
					this.SetStopOrderSell(trade);
				}
			}
		}

		public void StrategySell(CandleData candlePrev, CandleData candleCurr)
		{
			if (candlePrev.Open > candleCurr.Close)
			{
				//Попадаение в границы high
				if (checkBoxBorderHighSell.Checked)
					if (this.numericUpDownBorderHighSell.Value < this.CurrentPrice) return;
				//Попадаение в границы low
				if (checkBoxBorderLowSell.Checked)
					if (this.numericUpDownBorderLowSell.Value > this.CurrentPrice) return;

				//тело должно быть большим 3 единицы
				if (candlePrev.Close - candlePrev.Open >= this.MinStepPrice * StructStrategy.MinSizeBodyCandle)
				{
					if (candlePrev.Open < candlePrev.Close && candleCurr.Open > candleCurr.Close)
					{
						if (candlePrev.Volume < candleCurr.Volume)
						{
							if (candlePrev.Volume + candleCurr.Volume > this.numericUpDownBuy.Value)
							{
								var text = "";
								text += candlePrev.Time.ToShortTimeString() + " -> " + candleCurr.Time.ToShortTimeString() + " ";
								if (this.TrElement.Security.LastTrade.NotIsNull())
								{
									text += this.TrElement.Security.LastTrade.Price.ToString() + " S";
								}
								else text += "not lastPrice";
								text += " " + this.TimeFrameBuy.ToString() + "m\r\n";
								textBoxLogSell.Text = text + textBoxLogSell.Text;
								this.WriteSignalToFile(this.TrElement.Security, DateTime.Now.ToString() + " " + text);

								//Торговля
								if (checkBoxAccBuy.Checked)
								{
									this.TraderSell(candlePrev, candleCurr);
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Функция торгующая SELL
		/// </summary>
		public void TraderSell(CandleData candlePrev, CandleData candleCurr)
		{
			try
			{
				if (this.Trader.IsNull()) return;

				//разрешаем если позиция 0
				if (this.isPosition()) return;

				//получаем цену ордера
				StarategySell.StepDigress = 2;
				//цена ордера
				decimal priceOrder = candlePrev.Low;

				var order = new Order()
				{
					Price = priceOrder,
					Volume = Convert.ToInt32(this.numericUpDownSVol.Value),
					Direction = OrderDirection.Sell,
					Sec = this.TrElement.Security,
					SecCode = this.TrElement.Security.Code,
					Comment = StructStrategy.PrefixComment + this.Trader.Objects.MyTrades.Count().ToString()
				};
				ListSignals.Add(new Signal() { Order = order, CandlePrev = candlePrev, CandleCurr = candleCurr });
				this.Trader.CreateOrder(order);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}
		/// <summary>
		/// Проверка наличия позиции
		/// </summary>
		/// <returns></returns>
		private bool isPosition()
		{
			try
			{
				var pos = this.Trader.Objects.Positions.FirstOrDefault(p => p.Sec == this.TrElement.Security);
				if (pos.IsNull()) return false;
				if (pos.CurrentVolume > 0) return true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
			return false;
		}

		/// <summary>
		/// Установщик стопов sell
		/// </summary>
		/// <param name="trade"></param>
		public void SetStopOrderSell(MyTrade trade)
		{
			try
			{
				var signal = this.ListSignals.FirstOrDefault(s => trade.Comment.Contains(s.Order.Comment)
					&& trade.Comment.Contains(StructStrategy.PrefixComment)
					&& s.Order.Sec == trade.Trade.Sec && s.Order.Direction == trade.Trade.Direction);
				if (signal.NotIsNull())
				{
					this.GuiAsync(() =>
					{
						if (this.checkBoxAutoStopSell.Checked)
						{
							//стоп
							decimal priceStop = signal.CandlePrev.High > signal.CandleCurr.High ?
									signal.CandlePrev.High : signal.CandleCurr.High;
							//получаем цену отступа стопа
							priceStop += StarategySell.StepDigress * this.TrElement.Security.Params.MinPriceStep;
							//профит 
							decimal priceProfit = trade.Trade.Price - this.numericUpDownProfitTickSell.Value * this.TrElement.Security.Params.MinPriceStep;
							if (priceStop > 0 && priceProfit > 0)
							{
								var stop = new StopOrder()
								{
									LinkOrderPrice = priceProfit,
									ConditionPrice = priceStop,
									Price = priceStop + 20 * this.TrElement.Security.Params.MinPriceStep,
									Volume = trade.Trade.Volume,
									Direction = OrderDirection.Buy,
									Sec = this.TrElement.Security,
								};
								this.Trader.CreateStopOrder(stop, StopOrderType.LinkOrder);
							}
						}
					});
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		public void WriteSignalToFile(Securities sec, string textSignal)
		{
			var nameFile = "st_signal_" + sec.Code + "_" + sec.ClassCode + ".log";
			System.IO.StreamWriter file = new System.IO.StreamWriter(nameFile, true);
			file.WriteLine(textSignal);
			file.Close();
		}
	}
	/// <summary>
	/// Хранитель сигнала
	/// </summary>
	public class Signal
	{
		/// <summary>
		/// Заявка
		/// </summary>
		public Order Order = null;
		/// <summary>
		/// Предыдущая сделка
		/// </summary>
		public CandleData CandlePrev = null;
		/// <summary>
		/// Текущая сделка
		/// </summary>
		public CandleData CandleCurr = null;
	}

	/// <summary>
	/// СТруктура стратегии
	/// </summary>
	public class StructStrategy
	{
		public static string PrefixComment = "robot";
		/// <summary>
		/// Минимальный размер тела свечки в тиках.
		/// </summary>
		public static int MinSizeBodyCandle = 3;
		/// <summary>
		/// Кол-во периодов, по истечению которых снимать актиные заявки
		/// </summary>
		public static int CountPeriodCancelOrder = 3;
		/// <summary>
		/// Погрешность отступа от авто-стопа
		/// </summary>
		public decimal StepDigress = 0;
	}
}
