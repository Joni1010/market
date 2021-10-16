using AppVEConector.libs;
using Market.AppTools;
using MarketObjects;
using QuikConnector.MarketObjects;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AppVEConector
{
	public partial class Form_ActivateStopOrders :Form
	{
		Connector.QuikConnector Trader;
		TElement TrElement;
		public Form_ActivateStopOrders(Connector.QuikConnector trader, TElement trElement)
		{
			InitializeComponent();

			this.Trader = trader;
			this.TrElement = trElement;
		}

		private void Form_ActivateStopOrders_Load(object sender, EventArgs e)
		{
			labelNameSec.Text = this.TrElement.Security.ToString();

			numericUpDownStopOrderPrice.InitWheelDecimal();
			numericUpDownStopOrderPrice.InitSecurity(this.TrElement.Security);


			numericUpDownStopOrderPrice.Value = this.TrElement.Security.LastTrade.NotIsNull() ?
				this.TrElement.Security.LastTrade.Price : 0;

			numericUpDownStopOrderVol.Maximum = 1000000;
			numericUpDownStopOrderVol.Minimum = 1;
			numericUpDownStopOrderVol.InitWheelDecimal();

			buttonStopOrderBuy.Click += this.buttonStopOrderBuy_Click;
			buttonStopOrderSell.Click += this.buttonStopOrderSell_Click;

			buttonStopOrderCancel.Click += (s, ee) =>
			{
				var sOrders = this.Trader.Objects.tStopOrders.SearchAll(o => o.Sec == this.TrElement.Security &&
					o.Comment.Contains(Define.STOP_LIMIT));
				if(sOrders.NotIsNull() && sOrders.Count() > 0)
				{
					foreach(var ord in sOrders)
						this.Trader.CancelStopOrder(ord);
				}
			};

			MouseEventHandler rightClick = (s, ee) =>
			{
				var MouseEvent = (MouseEventArgs)ee;
				if (MouseEvent.Button == MouseButtons.Right)
				{
					if (this.TrElement.Security.LastTrade.NotIsNull())
					{
						var obj = (NumericUpDown)s;
						obj.Value = this.TrElement.Security.LastTrade.Price;
					}
				}
			};
			numericUpDownStopOrderPrice.MouseDown += rightClick;
		}

		private void buttonStopOrderBuy_Click(object s, EventArgs e)
		{
			if (this.TrElement.Security.LastPrice == 0) return;
			if (this.TrElement.Security.LastPrice > numericUpDownStopOrderPrice.Value)
			{
				var sOrder = new StopOrder()
				{
					Sec = this.TrElement.Security,
					Price = this.numericUpDownStopOrderPrice.Value,
					Volume = Convert.ToInt32(this.numericUpDownStopOrderVol.Value),
					Direction = OrderDirection.Buy,
					Comment = Define.STOP_LIMIT,
					ConditionPrice = this.numericUpDownStopOrderPrice.Value + this.TrElement.Security.Params.MinPriceStep,
					Offset = this.TrElement.Security.Params.MinPriceStep,
					Spread = this.TrElement.Security.Params.MinPriceStep,
					DateExpiry = DateMarket.ExtractDateTime(dateTimePickerStopOrder.Value)
				};
				this.Trader.CreateStopOrder(sOrder, StopOrderType.TakeProfit);
			} else {
				var sOrder = new StopOrder()
				{
					Sec = this.TrElement.Security,
					Price = this.numericUpDownStopOrderPrice.Value,
					Volume = Convert.ToInt32(this.numericUpDownStopOrderVol.Value),
					Direction = OrderDirection.Buy,
					Comment = Define.STOP_LIMIT,
					ConditionPrice = this.numericUpDownStopOrderPrice.Value - this.TrElement.Security.Params.MinPriceStep,
					DateExpiry = DateMarket.ExtractDateTime(dateTimePickerStopOrder.Value)
				};
				this.Trader.CreateStopOrder(sOrder, StopOrderType.StopLimit);
			}
		}

		private void buttonStopOrderSell_Click(object s, EventArgs e)
		{
			if (this.TrElement.Security.LastPrice == 0) return;
			if (this.TrElement.Security.LastPrice < numericUpDownStopOrderPrice.Value)
			{
				var sOrder = new StopOrder()
				{
					Sec = this.TrElement.Security,
					Price = this.numericUpDownStopOrderPrice.Value,
					Volume = Convert.ToInt32(this.numericUpDownStopOrderVol.Value),
					Direction = OrderDirection.Sell,
					Comment = Define.STOP_LIMIT,
					ConditionPrice = this.numericUpDownStopOrderPrice.Value - this.TrElement.Security.Params.MinPriceStep,
					Offset = this.TrElement.Security.Params.MinPriceStep,
					Spread = this.TrElement.Security.Params.MinPriceStep,
					DateExpiry = DateMarket.ExtractDateTime(dateTimePickerStopOrder.Value)
				};
				this.Trader.CreateStopOrder(sOrder, StopOrderType.TakeProfit);
			}
			else
			{
				var sOrder = new StopOrder()
				{
					Sec = this.TrElement.Security,
					Price = this.numericUpDownStopOrderPrice.Value,
					Volume = Convert.ToInt32(this.numericUpDownStopOrderVol.Value),
					Direction = OrderDirection.Sell,
					Comment = Define.STOP_LIMIT,
					ConditionPrice = this.numericUpDownStopOrderPrice.Value + this.TrElement.Security.Params.MinPriceStep,
					DateExpiry = DateMarket.ExtractDateTime(dateTimePickerStopOrder.Value)
				};
				this.Trader.CreateStopOrder(sOrder, StopOrderType.StopLimit);
			}
		}
	}
}
