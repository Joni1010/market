namespace AppVEConector
{
	partial class Form_ActivateStopOrders
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.labelNameSec = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.groupBoxLimitOrder = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.numericUpDownStopOrderVol = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.numericUpDownStopOrderPrice = new System.Windows.Forms.NumericUpDown();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.buttonStopOrderCancel = new System.Windows.Forms.Button();
			this.buttonStopOrderSell = new System.Windows.Forms.Button();
			this.buttonStopOrderBuy = new System.Windows.Forms.Button();
			this.dateTimePickerStopOrder = new System.Windows.Forms.DateTimePicker();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel2.SuspendLayout();
			this.groupBoxLimitOrder.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownStopOrderVol)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownStopOrderPrice)).BeginInit();
			this.tableLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tableLayoutPanel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(309, 26);
			this.panel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 219F));
			this.tableLayoutPanel2.Controls.Add(this.labelNameSec, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(309, 20);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// labelNameSec
			// 
			this.labelNameSec.AutoSize = true;
			this.labelNameSec.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelNameSec.Location = new System.Drawing.Point(91, 1);
			this.labelNameSec.Margin = new System.Windows.Forms.Padding(1);
			this.labelNameSec.Name = "labelNameSec";
			this.labelNameSec.Size = new System.Drawing.Size(54, 18);
			this.labelNameSec.TabIndex = 2;
			this.labelNameSec.Text = "NameSec";
			this.labelNameSec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Left;
			this.label4.Location = new System.Drawing.Point(1, 1);
			this.label4.Margin = new System.Windows.Forms.Padding(1);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(68, 18);
			this.label4.TabIndex = 1;
			this.label4.Text = "Инструмент";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.groupBoxLimitOrder);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 26);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(309, 122);
			this.panel2.TabIndex = 0;
			// 
			// groupBoxLimitOrder
			// 
			this.groupBoxLimitOrder.Controls.Add(this.tableLayoutPanel1);
			this.groupBoxLimitOrder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxLimitOrder.Location = new System.Drawing.Point(0, 0);
			this.groupBoxLimitOrder.Name = "groupBoxLimitOrder";
			this.groupBoxLimitOrder.Size = new System.Drawing.Size(309, 122);
			this.groupBoxLimitOrder.TabIndex = 1;
			this.groupBoxLimitOrder.TabStop = false;
			this.groupBoxLimitOrder.Text = "Стоп-лимит";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.numericUpDownStopOrderVol, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.numericUpDownStopOrderPrice, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.dateTimePickerStopOrder, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(303, 103);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// numericUpDownStopOrderVol
			// 
			this.numericUpDownStopOrderVol.Location = new System.Drawing.Point(91, 23);
			this.numericUpDownStopOrderVol.Margin = new System.Windows.Forms.Padding(1);
			this.numericUpDownStopOrderVol.Name = "numericUpDownStopOrderVol";
			this.numericUpDownStopOrderVol.Size = new System.Drawing.Size(70, 20);
			this.numericUpDownStopOrderVol.TabIndex = 4;
			this.numericUpDownStopOrderVol.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.Location = new System.Drawing.Point(1, 1);
			this.label1.Margin = new System.Windows.Forms.Padding(1);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Стоп цена";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Left;
			this.label2.Location = new System.Drawing.Point(1, 23);
			this.label2.Margin = new System.Windows.Forms.Padding(1);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Объем";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Left;
			this.label3.Location = new System.Drawing.Point(1, 45);
			this.label3.Margin = new System.Windows.Forms.Padding(1);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(68, 20);
			this.label3.TabIndex = 2;
			this.label3.Text = "Дата оконч.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numericUpDownStopOrderPrice
			// 
			this.numericUpDownStopOrderPrice.Location = new System.Drawing.Point(91, 1);
			this.numericUpDownStopOrderPrice.Margin = new System.Windows.Forms.Padding(1);
			this.numericUpDownStopOrderPrice.Name = "numericUpDownStopOrderPrice";
			this.numericUpDownStopOrderPrice.Size = new System.Drawing.Size(101, 20);
			this.numericUpDownStopOrderPrice.TabIndex = 3;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 3;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel3.Controls.Add(this.buttonStopOrderCancel, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.buttonStopOrderSell, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.buttonStopOrderBuy, 0, 0);
			this.tableLayoutPanel3.Location = new System.Drawing.Point(93, 69);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(207, 27);
			this.tableLayoutPanel3.TabIndex = 6;
			// 
			// buttonStopOrderCancel
			// 
			this.buttonStopOrderCancel.BackColor = System.Drawing.Color.LightSteelBlue;
			this.buttonStopOrderCancel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonStopOrderCancel.Location = new System.Drawing.Point(141, 3);
			this.buttonStopOrderCancel.Name = "buttonStopOrderCancel";
			this.buttonStopOrderCancel.Size = new System.Drawing.Size(63, 21);
			this.buttonStopOrderCancel.TabIndex = 7;
			this.buttonStopOrderCancel.Text = "Cancel All";
			this.buttonStopOrderCancel.UseVisualStyleBackColor = false;
			// 
			// buttonStopOrderSell
			// 
			this.buttonStopOrderSell.BackColor = System.Drawing.Color.Coral;
			this.buttonStopOrderSell.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonStopOrderSell.Location = new System.Drawing.Point(72, 3);
			this.buttonStopOrderSell.Name = "buttonStopOrderSell";
			this.buttonStopOrderSell.Size = new System.Drawing.Size(63, 21);
			this.buttonStopOrderSell.TabIndex = 6;
			this.buttonStopOrderSell.Text = "StopSell";
			this.buttonStopOrderSell.UseVisualStyleBackColor = false;
			// 
			// buttonStopOrderBuy
			// 
			this.buttonStopOrderBuy.BackColor = System.Drawing.Color.LightGreen;
			this.buttonStopOrderBuy.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonStopOrderBuy.Location = new System.Drawing.Point(3, 3);
			this.buttonStopOrderBuy.Name = "buttonStopOrderBuy";
			this.buttonStopOrderBuy.Size = new System.Drawing.Size(63, 21);
			this.buttonStopOrderBuy.TabIndex = 5;
			this.buttonStopOrderBuy.Text = "StopBuy";
			this.buttonStopOrderBuy.UseVisualStyleBackColor = false;
			// 
			// dateTimePickerStopOrder
			// 
			this.dateTimePickerStopOrder.Location = new System.Drawing.Point(91, 45);
			this.dateTimePickerStopOrder.Margin = new System.Windows.Forms.Padding(1);
			this.dateTimePickerStopOrder.Name = "dateTimePickerStopOrder";
			this.dateTimePickerStopOrder.Size = new System.Drawing.Size(135, 20);
			this.dateTimePickerStopOrder.TabIndex = 7;
			// 
			// Form_ActivateStopOrders
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(309, 418);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "Form_ActivateStopOrders";
			this.Text = "Форма стоп заявок";
			this.Load += new System.EventHandler(this.Form_ActivateStopOrders_Load);
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.groupBoxLimitOrder.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownStopOrderVol)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownStopOrderPrice)).EndInit();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label labelNameSec;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox groupBoxLimitOrder;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.NumericUpDown numericUpDownStopOrderVol;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numericUpDownStopOrderPrice;
		private System.Windows.Forms.Button buttonStopOrderBuy;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Button buttonStopOrderSell;
		private System.Windows.Forms.Button buttonStopOrderCancel;
		private System.Windows.Forms.DateTimePicker dateTimePickerStopOrder;
	}
}