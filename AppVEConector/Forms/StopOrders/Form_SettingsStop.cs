using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppVEConector.Forms.StopOrders
{
	public partial class Form_SettingsStop :Form
	{
		private new ControlForFormStopOrders Parent;
		private Form_LightOrders.PanelControl Panel;
		public Form_SettingsStop(ControlForFormStopOrders parent, Form_LightOrders.PanelControl panel)
		{
			this.Parent = parent;
			this.Panel = panel;

			InitializeComponent();

			this.InitControls();
		}

		private void InitControls()
		{
			numericUpDownPeriodTrack.InitWheelDecimal();
			numericUpDownPeriodTrack.Maximum = 1000000;
			numericUpDownPeriodTrack.Minimum = 0;

			numericUpDownStepTrack.InitWheelDecimal();
			numericUpDownStepTrack.Maximum = 1000000;
			numericUpDownStepTrack.Minimum = 0;

			this.updateTrackInfo();
		}

		private void buttonSetTrackStop_Click(object sender, EventArgs e)
		{
			if (this.Panel.IsNull()) return;

			this.Panel.TrackOrder = new Form_LightOrders.TrackOrder()
			{
				Period = Convert.ToInt32(numericUpDownPeriodTrack.Value),
				Tiks = Convert.ToInt32(numericUpDownStepTrack.Value)
			};
			this.updateTrackInfo();
		}

		private void buttonResetTrack_Click(object sender, EventArgs e)
		{
			if (this.Panel.IsNull()) return;
			this.Panel.TrackOrder = null;
			this.updateTrackInfo();
		}

		private void updateTrackInfo()
		{
			if (this.Panel.TrackOrder.NotIsNull())
			{
				numericUpDownPeriodTrack.Value = this.Panel.TrackOrder.Period;
				numericUpDownStepTrack.Value = this.Panel.TrackOrder.Tiks;
			}
			else
			{
				numericUpDownPeriodTrack.Value = 0;
				numericUpDownStepTrack.Value = 0;
			}
			Thread.Sleep(400);
		}
	}
}
