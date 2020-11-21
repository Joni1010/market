namespace AppVEConector.Forms.StopOrders
{
    partial class Form_CommonSettingsStopOrders
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewListSec = new System.Windows.Forms.DataGridView();
            this.ColumnSecClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelHeaderGrid = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSearchSec = new System.Windows.Forms.ComboBox();
            this.buttonAddSecInList = new System.Windows.Forms.Button();
            this.buttonDelSec = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewListSec)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.07407F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.92593F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.79365F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.20635F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(540, 315);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.36364F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.63636F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxSearchSec, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonAddSecInList, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(231, 216);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.dataGridViewListSec, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.labelHeaderGrid, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonDelSec, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(240, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(297, 309);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // dataGridViewListSec
            // 
            this.dataGridViewListSec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewListSec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSecClass,
            this.ColumnName});
            this.tableLayoutPanel3.SetColumnSpan(this.dataGridViewListSec, 2);
            this.dataGridViewListSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewListSec.Location = new System.Drawing.Point(1, 23);
            this.dataGridViewListSec.Margin = new System.Windows.Forms.Padding(1);
            this.dataGridViewListSec.Name = "dataGridViewListSec";
            this.dataGridViewListSec.ReadOnly = true;
            this.dataGridViewListSec.RowHeadersVisible = false;
            this.dataGridViewListSec.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewListSec.Size = new System.Drawing.Size(295, 263);
            this.dataGridViewListSec.TabIndex = 0;
            // 
            // ColumnSecClass
            // 
            this.ColumnSecClass.HeaderText = "Код";
            this.ColumnSecClass.Name = "ColumnSecClass";
            this.ColumnSecClass.ReadOnly = true;
            this.ColumnSecClass.Width = 90;
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "Название";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.Width = 200;
            // 
            // labelHeaderGrid
            // 
            this.labelHeaderGrid.Location = new System.Drawing.Point(1, 1);
            this.labelHeaderGrid.Margin = new System.Windows.Forms.Padding(1);
            this.labelHeaderGrid.Name = "labelHeaderGrid";
            this.labelHeaderGrid.Size = new System.Drawing.Size(146, 20);
            this.labelHeaderGrid.TabIndex = 1;
            this.labelHeaderGrid.Text = "Список инструментов";
            this.labelHeaderGrid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Инструмент";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxSearchSec
            // 
            this.comboBoxSearchSec.FormattingEnabled = true;
            this.comboBoxSearchSec.Location = new System.Drawing.Point(84, 1);
            this.comboBoxSearchSec.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxSearchSec.Name = "comboBoxSearchSec";
            this.comboBoxSearchSec.Size = new System.Drawing.Size(146, 21);
            this.comboBoxSearchSec.TabIndex = 2;
            // 
            // buttonAddSecInList
            // 
            this.buttonAddSecInList.Location = new System.Drawing.Point(83, 22);
            this.buttonAddSecInList.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddSecInList.Name = "buttonAddSecInList";
            this.buttonAddSecInList.Size = new System.Drawing.Size(102, 22);
            this.buttonAddSecInList.TabIndex = 3;
            this.buttonAddSecInList.Text = "Добавить >>";
            this.buttonAddSecInList.UseVisualStyleBackColor = true;
            // 
            // buttonDelSec
            // 
            this.buttonDelSec.Location = new System.Drawing.Point(0, 287);
            this.buttonDelSec.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDelSec.Name = "buttonDelSec";
            this.buttonDelSec.Size = new System.Drawing.Size(80, 22);
            this.buttonDelSec.TabIndex = 3;
            this.buttonDelSec.Text = "<< Удалить";
            this.buttonDelSec.UseVisualStyleBackColor = true;
            // 
            // Form_CommonSettingsStopOrders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 315);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form_CommonSettingsStopOrders";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки формы Стоп-заявок";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CommonSettingsStopOrders_FormClosing);
            this.Load += new System.EventHandler(this.Form_CommonSettingsStopOrders_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewListSec)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView dataGridViewListSec;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSecClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSearchSec;
        private System.Windows.Forms.Button buttonAddSecInList;
        private System.Windows.Forms.Label labelHeaderGrid;
        private System.Windows.Forms.Button buttonDelSec;
    }
}