namespace AppVEConector.Forms
{
    partial class Form_Arbitration
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxBaseSec = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelBaseSecLot = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelBaseSec = new System.Windows.Forms.Label();
            this.comboBoxBaseSec = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.labelBasePos = new System.Windows.Forms.Label();
            this.labelBaseSumPos = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBoxBaseAccount = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelBaseSecPrice = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.labelBaseCurrPos = new System.Windows.Forms.Label();
            this.groupBoxFutures = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.labelFutLot = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxFutSec = new System.Windows.Forms.ComboBox();
            this.labelFutPrice = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelFutGo = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelFutPriceStep = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelFutPos = new System.Windows.Forms.Label();
            this.numericUpDownFutPos = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownDiffPriceControll = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.checkBoxEnableControl = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDownDiffPriceClose = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.labelDiffPrice = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelFutSumPos = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxFutAccount = new System.Windows.Forms.ComboBox();
            this.checkBoxEnableTrade = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.labelFutCurrPos = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxBaseSec.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBoxFutures.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFutPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDiffPriceControll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDiffPriceClose)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxBaseSec, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxFutures, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 475);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBoxBaseSec
            // 
            this.groupBoxBaseSec.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxBaseSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxBaseSec.Location = new System.Drawing.Point(400, 0);
            this.groupBoxBaseSec.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxBaseSec.Name = "groupBoxBaseSec";
            this.groupBoxBaseSec.Size = new System.Drawing.Size(400, 475);
            this.groupBoxBaseSec.TabIndex = 1;
            this.groupBoxBaseSec.TabStop = false;
            this.groupBoxBaseSec.Text = "Базовый инструмент";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.labelBaseSecLot, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelBaseSec, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxBaseSec, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.labelBasePos, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.labelBaseSumPos, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.label15, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxBaseAccount, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.labelBaseSecPrice, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label17, 0, 17);
            this.tableLayoutPanel2.Controls.Add(this.labelBaseCurrPos, 1, 17);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 21;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(394, 456);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // labelBaseSecLot
            // 
            this.labelBaseSecLot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelBaseSecLot.Location = new System.Drawing.Point(198, 23);
            this.labelBaseSecLot.Margin = new System.Windows.Forms.Padding(1);
            this.labelBaseSecLot.Name = "labelBaseSecLot";
            this.labelBaseSecLot.Size = new System.Drawing.Size(195, 20);
            this.labelBaseSecLot.TabIndex = 3;
            this.labelBaseSecLot.Text = "0";
            this.labelBaseSecLot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(1, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Лот";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelBaseSec
            // 
            this.labelBaseSec.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelBaseSec.Location = new System.Drawing.Point(1, 1);
            this.labelBaseSec.Margin = new System.Windows.Forms.Padding(1);
            this.labelBaseSec.Name = "labelBaseSec";
            this.labelBaseSec.Size = new System.Drawing.Size(158, 20);
            this.labelBaseSec.TabIndex = 0;
            this.labelBaseSec.Text = "Инструмент";
            this.labelBaseSec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxBaseSec
            // 
            this.comboBoxBaseSec.FormattingEnabled = true;
            this.comboBoxBaseSec.Location = new System.Drawing.Point(198, 1);
            this.comboBoxBaseSec.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxBaseSec.Name = "comboBoxBaseSec";
            this.comboBoxBaseSec.Size = new System.Drawing.Size(194, 21);
            this.comboBoxBaseSec.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label9.Location = new System.Drawing.Point(1, 155);
            this.label9.Margin = new System.Windows.Forms.Padding(1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(195, 20);
            this.label9.TabIndex = 11;
            this.label9.Text = "Позиция базового актива";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelBasePos
            // 
            this.labelBasePos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelBasePos.Location = new System.Drawing.Point(198, 155);
            this.labelBasePos.Margin = new System.Windows.Forms.Padding(1);
            this.labelBasePos.Name = "labelBasePos";
            this.labelBasePos.Size = new System.Drawing.Size(195, 20);
            this.labelBasePos.TabIndex = 12;
            this.labelBasePos.Text = "0";
            this.labelBasePos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelBaseSumPos
            // 
            this.labelBaseSumPos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelBaseSumPos.Location = new System.Drawing.Point(198, 177);
            this.labelBaseSumPos.Margin = new System.Windows.Forms.Padding(1);
            this.labelBaseSumPos.Name = "labelBaseSumPos";
            this.labelBaseSumPos.Size = new System.Drawing.Size(195, 20);
            this.labelBaseSumPos.TabIndex = 16;
            this.labelBaseSumPos.Text = "0";
            this.labelBaseSumPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.Location = new System.Drawing.Point(1, 177);
            this.label12.Margin = new System.Windows.Forms.Padding(1);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(195, 20);
            this.label12.TabIndex = 15;
            this.label12.Text = "Сумма позиции";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label15.Location = new System.Drawing.Point(1, 111);
            this.label15.Margin = new System.Windows.Forms.Padding(1);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(195, 20);
            this.label15.TabIndex = 12;
            this.label15.Text = "Счет";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxBaseAccount
            // 
            this.comboBoxBaseAccount.FormattingEnabled = true;
            this.comboBoxBaseAccount.Location = new System.Drawing.Point(198, 111);
            this.comboBoxBaseAccount.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxBaseAccount.Name = "comboBoxBaseAccount";
            this.comboBoxBaseAccount.Size = new System.Drawing.Size(194, 21);
            this.comboBoxBaseAccount.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(1, 89);
            this.label2.Margin = new System.Windows.Forms.Padding(1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Текущая цена";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelBaseSecPrice
            // 
            this.labelBaseSecPrice.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelBaseSecPrice.Location = new System.Drawing.Point(198, 89);
            this.labelBaseSecPrice.Margin = new System.Windows.Forms.Padding(1);
            this.labelBaseSecPrice.Name = "labelBaseSecPrice";
            this.labelBaseSecPrice.Size = new System.Drawing.Size(195, 20);
            this.labelBaseSecPrice.TabIndex = 5;
            this.labelBaseSecPrice.Text = "0,0";
            this.labelBaseSecPrice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label17.Location = new System.Drawing.Point(1, 375);
            this.label17.Margin = new System.Windows.Forms.Padding(1);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(195, 20);
            this.label17.TabIndex = 19;
            this.label17.Text = "Текущая позиция";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelBaseCurrPos
            // 
            this.labelBaseCurrPos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelBaseCurrPos.Location = new System.Drawing.Point(198, 375);
            this.labelBaseCurrPos.Margin = new System.Windows.Forms.Padding(1);
            this.labelBaseCurrPos.Name = "labelBaseCurrPos";
            this.labelBaseCurrPos.Size = new System.Drawing.Size(195, 20);
            this.labelBaseCurrPos.TabIndex = 11;
            this.labelBaseCurrPos.Text = "0";
            this.labelBaseCurrPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxFutures
            // 
            this.groupBoxFutures.Controls.Add(this.tableLayoutPanel3);
            this.groupBoxFutures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFutures.Location = new System.Drawing.Point(0, 0);
            this.groupBoxFutures.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxFutures.Name = "groupBoxFutures";
            this.groupBoxFutures.Size = new System.Drawing.Size(400, 475);
            this.groupBoxFutures.TabIndex = 0;
            this.groupBoxFutures.TabStop = false;
            this.groupBoxFutures.Text = "Фьючерс";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.labelFutLot, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxFutSec, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelFutPrice, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.labelFutGo, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.labelFutPriceStep, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.labelFutPos, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.numericUpDownFutPos, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.numericUpDownDiffPriceControll, 1, 14);
            this.tableLayoutPanel3.Controls.Add(this.label11, 0, 14);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxEnableControl, 1, 16);
            this.tableLayoutPanel3.Controls.Add(this.label13, 0, 15);
            this.tableLayoutPanel3.Controls.Add(this.numericUpDownDiffPriceClose, 1, 15);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 11);
            this.tableLayoutPanel3.Controls.Add(this.labelDiffPrice, 1, 11);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.labelFutSumPos, 1, 8);
            this.tableLayoutPanel3.Controls.Add(this.label14, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxFutAccount, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxEnableTrade, 1, 19);
            this.tableLayoutPanel3.Controls.Add(this.label16, 0, 17);
            this.tableLayoutPanel3.Controls.Add(this.labelFutCurrPos, 1, 17);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 21;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(394, 456);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // labelFutLot
            // 
            this.labelFutLot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelFutLot.Location = new System.Drawing.Point(198, 23);
            this.labelFutLot.Margin = new System.Windows.Forms.Padding(1);
            this.labelFutLot.Name = "labelFutLot";
            this.labelFutLot.Size = new System.Drawing.Size(195, 20);
            this.labelFutLot.TabIndex = 3;
            this.labelFutLot.Text = "0";
            this.labelFutLot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(1, 23);
            this.label6.Margin = new System.Windows.Forms.Padding(1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(195, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "Лот";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Location = new System.Drawing.Point(1, 1);
            this.label7.Margin = new System.Windows.Forms.Padding(1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "Инструмент";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxFutSec
            // 
            this.comboBoxFutSec.FormattingEnabled = true;
            this.comboBoxFutSec.Location = new System.Drawing.Point(198, 1);
            this.comboBoxFutSec.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxFutSec.Name = "comboBoxFutSec";
            this.comboBoxFutSec.Size = new System.Drawing.Size(194, 21);
            this.comboBoxFutSec.TabIndex = 1;
            // 
            // labelFutPrice
            // 
            this.labelFutPrice.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelFutPrice.Location = new System.Drawing.Point(198, 89);
            this.labelFutPrice.Margin = new System.Windows.Forms.Padding(1);
            this.labelFutPrice.Name = "labelFutPrice";
            this.labelFutPrice.Size = new System.Drawing.Size(195, 20);
            this.labelFutPrice.TabIndex = 5;
            this.labelFutPrice.Text = "0,0";
            this.labelFutPrice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.Location = new System.Drawing.Point(1, 45);
            this.label8.Margin = new System.Windows.Forms.Padding(1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(195, 20);
            this.label8.TabIndex = 6;
            this.label8.Text = "ГО";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFutGo
            // 
            this.labelFutGo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelFutGo.Location = new System.Drawing.Point(198, 45);
            this.labelFutGo.Margin = new System.Windows.Forms.Padding(1);
            this.labelFutGo.Name = "labelFutGo";
            this.labelFutGo.Size = new System.Drawing.Size(195, 20);
            this.labelFutGo.TabIndex = 7;
            this.labelFutGo.Text = "0.0";
            this.labelFutGo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label10.Location = new System.Drawing.Point(1, 67);
            this.label10.Margin = new System.Windows.Forms.Padding(1);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(195, 20);
            this.label10.TabIndex = 8;
            this.label10.Text = "Стоимость шага";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFutPriceStep
            // 
            this.labelFutPriceStep.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelFutPriceStep.Location = new System.Drawing.Point(198, 67);
            this.labelFutPriceStep.Margin = new System.Windows.Forms.Padding(1);
            this.labelFutPriceStep.Name = "labelFutPriceStep";
            this.labelFutPriceStep.Size = new System.Drawing.Size(195, 20);
            this.labelFutPriceStep.TabIndex = 9;
            this.labelFutPriceStep.Text = "0.0";
            this.labelFutPriceStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(1, 89);
            this.label4.Margin = new System.Windows.Forms.Padding(1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(195, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Текущая цена";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFutPos
            // 
            this.labelFutPos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelFutPos.Location = new System.Drawing.Point(1, 155);
            this.labelFutPos.Margin = new System.Windows.Forms.Padding(1);
            this.labelFutPos.Name = "labelFutPos";
            this.labelFutPos.Size = new System.Drawing.Size(195, 20);
            this.labelFutPos.TabIndex = 12;
            this.labelFutPos.Text = "Позиция по фьюч.";
            this.labelFutPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDownFutPos
            // 
            this.numericUpDownFutPos.Location = new System.Drawing.Point(198, 155);
            this.numericUpDownFutPos.Margin = new System.Windows.Forms.Padding(1);
            this.numericUpDownFutPos.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownFutPos.Name = "numericUpDownFutPos";
            this.numericUpDownFutPos.Size = new System.Drawing.Size(130, 20);
            this.numericUpDownFutPos.TabIndex = 13;
            // 
            // numericUpDownDiffPriceControll
            // 
            this.numericUpDownDiffPriceControll.Location = new System.Drawing.Point(198, 309);
            this.numericUpDownDiffPriceControll.Margin = new System.Windows.Forms.Padding(1);
            this.numericUpDownDiffPriceControll.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownDiffPriceControll.Name = "numericUpDownDiffPriceControll";
            this.numericUpDownDiffPriceControll.Size = new System.Drawing.Size(130, 20);
            this.numericUpDownDiffPriceControll.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label11.Location = new System.Drawing.Point(1, 309);
            this.label11.Margin = new System.Windows.Forms.Padding(1);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(195, 20);
            this.label11.TabIndex = 17;
            this.label11.Text = "Открывать поз. если расх >";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxEnableControl
            // 
            this.checkBoxEnableControl.AutoSize = true;
            this.checkBoxEnableControl.Location = new System.Drawing.Point(200, 355);
            this.checkBoxEnableControl.Name = "checkBoxEnableControl";
            this.checkBoxEnableControl.Size = new System.Drawing.Size(125, 16);
            this.checkBoxEnableControl.TabIndex = 18;
            this.checkBoxEnableControl.Text = "Включить контроль";
            this.checkBoxEnableControl.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label13.Location = new System.Drawing.Point(1, 331);
            this.label13.Margin = new System.Windows.Forms.Padding(1);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(195, 20);
            this.label13.TabIndex = 19;
            this.label13.Text = "Закрывать поз. если расх <";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDownDiffPriceClose
            // 
            this.numericUpDownDiffPriceClose.Location = new System.Drawing.Point(198, 331);
            this.numericUpDownDiffPriceClose.Margin = new System.Windows.Forms.Padding(1);
            this.numericUpDownDiffPriceClose.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownDiffPriceClose.Name = "numericUpDownDiffPriceClose";
            this.numericUpDownDiffPriceClose.Size = new System.Drawing.Size(130, 20);
            this.numericUpDownDiffPriceClose.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(1, 243);
            this.label3.Margin = new System.Windows.Forms.Padding(1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Расхождение цен";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDiffPrice
            // 
            this.labelDiffPrice.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelDiffPrice.Location = new System.Drawing.Point(198, 243);
            this.labelDiffPrice.Margin = new System.Windows.Forms.Padding(1);
            this.labelDiffPrice.Name = "labelDiffPrice";
            this.labelDiffPrice.Size = new System.Drawing.Size(195, 20);
            this.labelDiffPrice.TabIndex = 11;
            this.labelDiffPrice.Text = "0";
            this.labelDiffPrice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(1, 177);
            this.label5.Margin = new System.Windows.Forms.Padding(1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(195, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Сумма позиции";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFutSumPos
            // 
            this.labelFutSumPos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelFutSumPos.Location = new System.Drawing.Point(198, 177);
            this.labelFutSumPos.Margin = new System.Windows.Forms.Padding(1);
            this.labelFutSumPos.Name = "labelFutSumPos";
            this.labelFutSumPos.Size = new System.Drawing.Size(195, 20);
            this.labelFutSumPos.TabIndex = 15;
            this.labelFutSumPos.Text = "0";
            this.labelFutSumPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label14.Location = new System.Drawing.Point(1, 111);
            this.label14.Margin = new System.Windows.Forms.Padding(1);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(195, 20);
            this.label14.TabIndex = 12;
            this.label14.Text = "Счет";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxFutAccount
            // 
            this.comboBoxFutAccount.FormattingEnabled = true;
            this.comboBoxFutAccount.Location = new System.Drawing.Point(198, 111);
            this.comboBoxFutAccount.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxFutAccount.Name = "comboBoxFutAccount";
            this.comboBoxFutAccount.Size = new System.Drawing.Size(194, 21);
            this.comboBoxFutAccount.TabIndex = 1;
            // 
            // checkBoxEnableTrade
            // 
            this.checkBoxEnableTrade.AutoSize = true;
            this.checkBoxEnableTrade.Location = new System.Drawing.Point(200, 421);
            this.checkBoxEnableTrade.Name = "checkBoxEnableTrade";
            this.checkBoxEnableTrade.Size = new System.Drawing.Size(121, 16);
            this.checkBoxEnableTrade.TabIndex = 18;
            this.checkBoxEnableTrade.Text = "Разрешить сделки";
            this.checkBoxEnableTrade.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label16.Location = new System.Drawing.Point(1, 375);
            this.label16.Margin = new System.Windows.Forms.Padding(1);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(195, 20);
            this.label16.TabIndex = 19;
            this.label16.Text = "Текущая позиция";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFutCurrPos
            // 
            this.labelFutCurrPos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelFutCurrPos.Location = new System.Drawing.Point(198, 375);
            this.labelFutCurrPos.Margin = new System.Windows.Forms.Padding(1);
            this.labelFutCurrPos.Name = "labelFutCurrPos";
            this.labelFutCurrPos.Size = new System.Drawing.Size(195, 20);
            this.labelFutCurrPos.TabIndex = 11;
            this.labelFutCurrPos.Text = "0";
            this.labelFutCurrPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Form_Arbitration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 475);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form_Arbitration";
            this.Text = "Form_Arbitration";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxBaseSec.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBoxFutures.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFutPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDiffPriceControll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDiffPriceClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxFutures;
        private System.Windows.Forms.GroupBox groupBoxBaseSec;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelBaseSec;
        private System.Windows.Forms.ComboBox comboBoxBaseSec;
        private System.Windows.Forms.Label labelBaseSecPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelBaseSecLot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label labelFutLot;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxFutSec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelFutPrice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelFutGo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelFutPriceStep;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelDiffPrice;
        private System.Windows.Forms.Label labelFutPos;
        private System.Windows.Forms.NumericUpDown numericUpDownFutPos;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelFutSumPos;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelBasePos;
        private System.Windows.Forms.NumericUpDown numericUpDownDiffPriceControll;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBoxEnableControl;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelBaseSumPos;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUpDownDiffPriceClose;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBoxBaseAccount;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxFutAccount;
        private System.Windows.Forms.CheckBox checkBoxEnableTrade;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label labelBaseCurrPos;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label labelFutCurrPos;
    }
}