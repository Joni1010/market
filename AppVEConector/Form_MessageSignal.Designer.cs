namespace AppVEConector
{
    partial class Form_MessageSignal
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.dataGridViewInfoSignal = new System.Windows.Forms.DataGridView();
            this.ColumnValueSign = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnOpenCharts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfoSignal)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonClose.Location = new System.Drawing.Point(0, 286);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(405, 41);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // dataGridViewInfoSignal
            // 
            this.dataGridViewInfoSignal.AllowUserToDeleteRows = false;
            this.dataGridViewInfoSignal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInfoSignal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnValueSign,
            this.ColumnOpenCharts});
            this.dataGridViewInfoSignal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewInfoSignal.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewInfoSignal.Name = "dataGridViewInfoSignal";
            this.dataGridViewInfoSignal.ReadOnly = true;
            this.dataGridViewInfoSignal.RowHeadersVisible = false;
            this.dataGridViewInfoSignal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewInfoSignal.Size = new System.Drawing.Size(405, 286);
            this.dataGridViewInfoSignal.TabIndex = 2;
            // 
            // ColumnValueSign
            // 
            this.ColumnValueSign.HeaderText = "Сигнал";
            this.ColumnValueSign.Name = "ColumnValueSign";
            this.ColumnValueSign.ReadOnly = true;
            this.ColumnValueSign.Width = 250;
            // 
            // ColumnOpenCharts
            // 
            this.ColumnOpenCharts.HeaderText = "График";
            this.ColumnOpenCharts.Name = "ColumnOpenCharts";
            this.ColumnOpenCharts.ReadOnly = true;
            // 
            // Form_MessageSignal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 327);
            this.Controls.Add(this.dataGridViewInfoSignal);
            this.Controls.Add(this.buttonClose);
            this.Name = "Form_MessageSignal";
            this.Text = "Сигналы";
            this.Load += new System.EventHandler(this.Form_MessageSignal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfoSignal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.DataGridView dataGridViewInfoSignal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValueSign;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnOpenCharts;
    }
}