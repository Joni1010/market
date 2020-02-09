
using AppVEConector.libs.Signal;
using System.Collections.Generic;

using System.Windows.Forms;
using TradingLib;

namespace AppVEConector
{
	public partial class FormSignalGsm :Form
	{
		private TElement TrElement = null;

		private SignalGSM Signaler = null;
		public FormSignalGsm(TElement trElem, SignalGSM signaler)
		{
			InitializeComponent();

			this.TrElement = trElem;
			this.Signaler = signaler;
			this.UpdateGridSignals();

			numericUpDownPrice.InitWheelDecimal();
			numericUpDownPrice.InitSecurity(this.TrElement.Security);

			numericUpDownPrice.Value = this.TrElement.Security.LastPrice.NotIsNull() ?
				this.TrElement.Security.LastPrice : 0;

			var list = new List<string>();
			list.Add(">=");
			list.Add(">");
			list.Add("<=");
			list.Add("<");
			list.Add("==");
			ComboBox.ObjectCollection items = new ComboBox.ObjectCollection(this.comboBoxCond);
			items.AddRange(list.ToArray());
			this.comboBoxCond.DataSource = items;
			if (list.Count > 0) this.comboBoxCond.SelectedIndex = 0;


			this.Signaler.OnAdd += (s) =>
			{
				this.UpdateGridSignals();
			};
			this.Signaler.OnRemove += (s) =>
			{
				this.UpdateGridSignals();
			};
		}

		public void UpdateGridSignals()
		{
			this.dataGridView1.GuiAsync(() =>
			{
				this.dataGridView1.Rows.Clear();
				if (this.Signaler.ToArray().Length > 0)
				{
					int i = 0;
					foreach (var sig in this.Signaler.ToArray())
					{
						var newRow = (DataGridViewRow)dataGridView1.Rows[0].Clone();
						newRow.Cells[0].Value = sig.SecClass;
						newRow.Cells[1].Value = sig.Price.ToString();
						if (sig.Condition == SignalMarket.CondSignal.MoreOrEquals)
							newRow.Cells[2].Value = ">=";
						else if (sig.Condition == SignalMarket.CondSignal.More)
							newRow.Cells[2].Value = ">";
						else if (sig.Condition == SignalMarket.CondSignal.LessOrEquals)
							newRow.Cells[2].Value = "<=";
						else if (sig.Condition == SignalMarket.CondSignal.Less)
							newRow.Cells[2].Value = "<";
						else if (sig.Condition == SignalMarket.CondSignal.Equals)
							newRow.Cells[2].Value = "==";
						dataGridView1.Rows.Add(newRow);

						newRow.Tag = sig;
						i++;
					}

				}
			});
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			SignalMarket.CondSignal cond = SignalMarket.CondSignal.MoreOrEquals; ;
			var strCond = (string)this.comboBoxCond.SelectedItem;
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
			var newSign = new SignalMarket()
			{
				SecClass = this.TrElement.Security.ToString(),
				Price = this.numericUpDownPrice.Value,
				Condition = cond
			};
			this.Signaler.AddSignal(newSign);
		}

		private void buttonDelSign_Click(object sender, System.EventArgs e)
		{
			if (this.dataGridView1.SelectedRows.NotIsNull())
			{
				if (this.dataGridView1.SelectedRows.Count > 0)
				{
					foreach (var row in this.dataGridView1.SelectedRows)
					{
						if (row is DataGridViewRow)
						{
							var r = (DataGridViewRow)row;
							if (r.Tag is SignalMarket)
							{
								this.Signaler.RemoveSignal((SignalMarket)r.Tag);
							}
						}
					}
				}
			}
		}

		private void buttonLastPrice_Click(object sender, System.EventArgs e)
		{
			numericUpDownPrice.Value = this.TrElement.Security.LastPrice.NotIsNull() ?
				this.TrElement.Security.LastPrice : 0;
		}
	}
}
