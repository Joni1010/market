using System.Windows.Forms;

namespace AppVEConector
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подключитьсяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.дополнительноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSpeedOrders = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSign = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTestSign = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSignCall = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageStopOrders = new System.Windows.Forms.TabPage();
            this.splitContainerMainOrders = new System.Windows.Forms.SplitContainer();
            this.splitContainerTablesStopOrders = new System.Windows.Forms.SplitContainer();
            this.dataGridViewStopOrders = new System.Windows.Forms.DataGridView();
            this.StopOrdersNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrdersID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrdersSec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrderыType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrdersCondition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrdersStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrdersPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrdersVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrdersPriceStop1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrdersPriceStop2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrdersSpread = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopOrdersOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelConditionStopList = new System.Windows.Forms.Panel();
            this.groupBoxFiltersStopOrders = new System.Windows.Forms.GroupBox();
            this.checkBoxSOExec = new System.Windows.Forms.CheckBox();
            this.checkBoxSOClosed = new System.Windows.Forms.CheckBox();
            this.checkBoxSOActive = new System.Windows.Forms.CheckBox();
            this.tabPageOrders = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitContainerListOrders = new System.Windows.Forms.SplitContainer();
            this.groupBoxOrders = new System.Windows.Forms.GroupBox();
            this.dataGridViewOrders = new System.Windows.Forms.DataGridView();
            this.NumOrders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusOrders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IDOrders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrdersObjSec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecOrders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceOrders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VolumeOrders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BalanceOrders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DirectionOrders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NamesOrders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.PanelPortfolios = new System.Windows.Forms.GroupBox();
            this.dataGridPortfolios = new System.Windows.Forms.DataGridView();
            this.Account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LimitKind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PosBalanse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VarMargin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Commision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PanelPositions = new System.Windows.Forms.GroupBox();
            this.dataGridPositions = new System.Windows.Forms.DataGridView();
            this.NamePos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StepPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnGO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActPoss = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Orders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PosVarMargin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnGetDepth = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSearchSec = new System.Windows.Forms.TextBox();
            this.dataGridFoundSec = new System.Windows.Forms.DataGridView();
            this.CodeSec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameSec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonOpenFoundDepth = new System.Windows.Forms.Button();
            this.buttonSaveSecurity = new System.Windows.Forms.Button();
            this.labelDescription = new System.Windows.Forms.Label();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageSignals = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel19 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxSecSign = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonClearSignalLog = new System.Windows.Forms.Button();
            this.buttonSignTestCall = new System.Windows.Forms.Button();
            this.buttonSignTestDev = new System.Windows.Forms.Button();
            this.buttonRestartSignalPort = new System.Windows.Forms.Button();
            this.comboBoxPorts = new System.Windows.Forms.ComboBox();
            this.labelSignNameSec = new System.Windows.Forms.Label();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAddSignTime = new System.Windows.Forms.Button();
            this.dateTimePickerSign = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAddSignVolume = new System.Windows.Forms.Button();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSignAddVol1000 = new System.Windows.Forms.Button();
            this.numericUpDownSignVolume = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxSignTimeFrame = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelSign = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAddSign = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonLastPrice = new System.Windows.Forms.Button();
            this.numericUpDownPrice = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxCond = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelSettingsSign = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewListSign = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueSignal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConditionSignal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSignDown = new System.Windows.Forms.Button();
            this.buttonSignUp = new System.Windows.Forms.Button();
            this.buttonDelSign = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxLogSign = new System.Windows.Forms.TextBox();
            this.textBoxLogDev = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxSignComment = new System.Windows.Forms.TextBox();
            this.tabPageFastGaps = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDownFGTimeStep = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFGSizeGap = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxFGActivate = new System.Windows.Forms.CheckBox();
            this.comboBoxFGTimeFrame = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxFGLog = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel20 = new System.Windows.Forms.TableLayoutPanel();
            this.tabPageDescription = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDescSave = new System.Windows.Forms.Button();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.tabPageAutoOrders = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel24 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownAOVolume = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxAOSecurities = new System.Windows.Forms.ComboBox();
            this.numericUpDownAOPrice = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel22 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAOSell = new System.Windows.Forms.Button();
            this.buttonAOBuy = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxAOAccount = new System.Windows.Forms.ComboBox();
            this.labelAOInfo = new System.Windows.Forms.Label();
            this.labelAOLog = new System.Windows.Forms.Label();
            this.tableLayoutPanel25 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel23 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAODeleteSec = new System.Windows.Forms.Button();
            this.buttonAODelete = new System.Windows.Forms.Button();
            this.checkBoxAOSelectSec = new System.Windows.Forms.CheckBox();
            this.dataGridViewAOList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageAutoStops = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel26 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel29 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel30 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonASLDelete = new System.Windows.Forms.Button();
            this.checkBoxASLBySec = new System.Windows.Forms.CheckBox();
            this.dataGridViewASLList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel27 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel28 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonASLAdd = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.comboBoxASLSec = new System.Windows.Forms.ComboBox();
            this.numericUpDownASLTiks = new System.Windows.Forms.NumericUpDown();
            this.labelASLInfo = new System.Windows.Forms.Label();
            this.labelASLLog = new System.Windows.Forms.Label();
            this.comboBoxASLAccount = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabPageStopOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainOrders)).BeginInit();
            this.splitContainerMainOrders.Panel2.SuspendLayout();
            this.splitContainerMainOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTablesStopOrders)).BeginInit();
            this.splitContainerTablesStopOrders.Panel1.SuspendLayout();
            this.splitContainerTablesStopOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStopOrders)).BeginInit();
            this.panelConditionStopList.SuspendLayout();
            this.groupBoxFiltersStopOrders.SuspendLayout();
            this.tabPageOrders.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerListOrders)).BeginInit();
            this.splitContainerListOrders.Panel1.SuspendLayout();
            this.splitContainerListOrders.SuspendLayout();
            this.groupBoxOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.PanelPortfolios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPortfolios)).BeginInit();
            this.PanelPositions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPositions)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFoundSec)).BeginInit();
            this.tabControlMain.SuspendLayout();
            this.tabPageSignals.SuspendLayout();
            this.tableLayoutPanel19.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel17.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSignVolume)).BeginInit();
            this.tableLayoutPanel15.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanelSign.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanelSettingsSign.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewListSign)).BeginInit();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tabPageFastGaps.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFGTimeStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFGSizeGap)).BeginInit();
            this.tabPageDescription.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.tabPageAutoOrders.SuspendLayout();
            this.tableLayoutPanel24.SuspendLayout();
            this.tableLayoutPanel21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAOVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAOPrice)).BeginInit();
            this.tableLayoutPanel22.SuspendLayout();
            this.tableLayoutPanel25.SuspendLayout();
            this.tableLayoutPanel23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAOList)).BeginInit();
            this.tabPageAutoStops.SuspendLayout();
            this.tableLayoutPanel26.SuspendLayout();
            this.tableLayoutPanel29.SuspendLayout();
            this.tableLayoutPanel30.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewASLList)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel27.SuspendLayout();
            this.tableLayoutPanel28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownASLTiks)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 478);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(933, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(400, 17);
            this.toolStripStatusLabel1.Text = "Добро пожаловать в терминал.";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem,
            this.дополнительноToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(933, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.подключитьсяToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.настройкиToolStripMenuItem.Text = "Main";
            // 
            // подключитьсяToolStripMenuItem
            // 
            this.подключитьсяToolStripMenuItem.Name = "подключитьсяToolStripMenuItem";
            this.подключитьсяToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.подключитьсяToolStripMenuItem.Text = "Подключиться";
            this.подключитьсяToolStripMenuItem.Click += new System.EventHandler(this.ConnectToolStripMenuItem_Click);
            // 
            // дополнительноToolStripMenuItem
            // 
            this.дополнительноToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem11,
            this.ToolStripMenuItemSpeedOrders,
            this.ToolStripMenuItemSign});
            this.дополнительноToolStripMenuItem.Name = "дополнительноToolStripMenuItem";
            this.дополнительноToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.дополнительноToolStripMenuItem.Text = "Дополнительно";
            // 
            // ToolStripMenuItem11
            // 
            this.ToolStripMenuItem11.Name = "ToolStripMenuItem11";
            this.ToolStripMenuItem11.Size = new System.Drawing.Size(165, 22);
            this.ToolStripMenuItem11.Text = "Мульти окно";
            this.ToolStripMenuItem11.Click += new System.EventHandler(this.ToolStripMenuItem11_Click);
            // 
            // ToolStripMenuItemSpeedOrders
            // 
            this.ToolStripMenuItemSpeedOrders.Name = "ToolStripMenuItemSpeedOrders";
            this.ToolStripMenuItemSpeedOrders.Size = new System.Drawing.Size(165, 22);
            this.ToolStripMenuItemSpeedOrders.Text = "Быстрые ордера";
            this.ToolStripMenuItemSpeedOrders.Click += new System.EventHandler(this.ToolStripMenuItemSpeedOrders_Click);
            // 
            // ToolStripMenuItemSign
            // 
            this.ToolStripMenuItemSign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemTestSign,
            this.ToolStripMenuItemSignCall});
            this.ToolStripMenuItemSign.Name = "ToolStripMenuItemSign";
            this.ToolStripMenuItemSign.Size = new System.Drawing.Size(165, 22);
            this.ToolStripMenuItemSign.Text = "Сигналы";
            // 
            // ToolStripMenuItemTestSign
            // 
            this.ToolStripMenuItemTestSign.Name = "ToolStripMenuItemTestSign";
            this.ToolStripMenuItemTestSign.Size = new System.Drawing.Size(200, 22);
            this.ToolStripMenuItemTestSign.Text = "Тест передачи сигнала";
            this.ToolStripMenuItemTestSign.Click += new System.EventHandler(this.ToolStripMenuItemTestSign_Click_1);
            // 
            // ToolStripMenuItemSignCall
            // 
            this.ToolStripMenuItemSignCall.Name = "ToolStripMenuItemSignCall";
            this.ToolStripMenuItemSignCall.Size = new System.Drawing.Size(200, 22);
            this.ToolStripMenuItemSignCall.Text = "Тест вызова";
            this.ToolStripMenuItemSignCall.Click += new System.EventHandler(this.ToolStripMenuItemSignCall_Click);
            // 
            // tabPageStopOrders
            // 
            this.tabPageStopOrders.Controls.Add(this.splitContainerMainOrders);
            this.tabPageStopOrders.Location = new System.Drawing.Point(4, 22);
            this.tabPageStopOrders.Name = "tabPageStopOrders";
            this.tabPageStopOrders.Size = new System.Drawing.Size(925, 428);
            this.tabPageStopOrders.TabIndex = 3;
            this.tabPageStopOrders.Text = "Стоп заявки";
            this.tabPageStopOrders.UseVisualStyleBackColor = true;
            // 
            // splitContainerMainOrders
            // 
            this.splitContainerMainOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMainOrders.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMainOrders.Name = "splitContainerMainOrders";
            // 
            // splitContainerMainOrders.Panel2
            // 
            this.splitContainerMainOrders.Panel2.Controls.Add(this.splitContainerTablesStopOrders);
            this.splitContainerMainOrders.Panel2.Controls.Add(this.panelConditionStopList);
            this.splitContainerMainOrders.Size = new System.Drawing.Size(925, 428);
            this.splitContainerMainOrders.SplitterDistance = 162;
            this.splitContainerMainOrders.TabIndex = 1;
            // 
            // splitContainerTablesStopOrders
            // 
            this.splitContainerTablesStopOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTablesStopOrders.Location = new System.Drawing.Point(0, 40);
            this.splitContainerTablesStopOrders.Name = "splitContainerTablesStopOrders";
            this.splitContainerTablesStopOrders.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTablesStopOrders.Panel1
            // 
            this.splitContainerTablesStopOrders.Panel1.Controls.Add(this.dataGridViewStopOrders);
            this.splitContainerTablesStopOrders.Size = new System.Drawing.Size(759, 388);
            this.splitContainerTablesStopOrders.SplitterDistance = 242;
            this.splitContainerTablesStopOrders.TabIndex = 1;
            // 
            // dataGridViewStopOrders
            // 
            this.dataGridViewStopOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStopOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StopOrdersNum,
            this.StopOrdersID,
            this.StopOrdersSec,
            this.StopOrderыType,
            this.StopOrdersCondition,
            this.StopOrdersStatus,
            this.StopOrdersPrice,
            this.StopOrdersVolume,
            this.StopOrdersPriceStop1,
            this.StopOrdersPriceStop2,
            this.StopOrdersSpread,
            this.StopOrdersOffset});
            this.dataGridViewStopOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewStopOrders.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewStopOrders.Name = "dataGridViewStopOrders";
            this.dataGridViewStopOrders.ReadOnly = true;
            this.dataGridViewStopOrders.RowHeadersVisible = false;
            this.dataGridViewStopOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStopOrders.Size = new System.Drawing.Size(759, 242);
            this.dataGridViewStopOrders.TabIndex = 1;
            this.dataGridViewStopOrders.DoubleClick += new System.EventHandler(this.dataGridViewStopOrders_DoubleClick);
            // 
            // StopOrdersNum
            // 
            this.StopOrdersNum.HeaderText = "№";
            this.StopOrdersNum.Name = "StopOrdersNum";
            this.StopOrdersNum.ReadOnly = true;
            this.StopOrdersNum.Width = 50;
            // 
            // StopOrdersID
            // 
            this.StopOrdersID.HeaderText = "Номер";
            this.StopOrdersID.Name = "StopOrdersID";
            this.StopOrdersID.ReadOnly = true;
            this.StopOrdersID.Width = 80;
            // 
            // StopOrdersSec
            // 
            this.StopOrdersSec.HeaderText = "Инструмент";
            this.StopOrdersSec.Name = "StopOrdersSec";
            this.StopOrdersSec.ReadOnly = true;
            this.StopOrdersSec.Width = 80;
            // 
            // StopOrderыType
            // 
            this.StopOrderыType.HeaderText = "Тип";
            this.StopOrderыType.Name = "StopOrderыType";
            this.StopOrderыType.ReadOnly = true;
            // 
            // StopOrdersCondition
            // 
            this.StopOrdersCondition.HeaderText = "Направление";
            this.StopOrdersCondition.Name = "StopOrdersCondition";
            this.StopOrdersCondition.ReadOnly = true;
            this.StopOrdersCondition.Width = 90;
            // 
            // StopOrdersStatus
            // 
            this.StopOrdersStatus.HeaderText = "Статус";
            this.StopOrdersStatus.Name = "StopOrdersStatus";
            this.StopOrdersStatus.ReadOnly = true;
            this.StopOrdersStatus.Width = 80;
            // 
            // StopOrdersPrice
            // 
            this.StopOrdersPrice.HeaderText = "Цена";
            this.StopOrdersPrice.Name = "StopOrdersPrice";
            this.StopOrdersPrice.ReadOnly = true;
            this.StopOrdersPrice.Width = 70;
            // 
            // StopOrdersVolume
            // 
            this.StopOrdersVolume.HeaderText = "Объем";
            this.StopOrdersVolume.Name = "StopOrdersVolume";
            this.StopOrdersVolume.ReadOnly = true;
            this.StopOrdersVolume.Width = 60;
            // 
            // StopOrdersPriceStop1
            // 
            this.StopOrdersPriceStop1.HeaderText = "Стоп цена";
            this.StopOrdersPriceStop1.Name = "StopOrdersPriceStop1";
            this.StopOrdersPriceStop1.ReadOnly = true;
            this.StopOrdersPriceStop1.Width = 70;
            // 
            // StopOrdersPriceStop2
            // 
            this.StopOrdersPriceStop2.HeaderText = "Стоп цена 2";
            this.StopOrdersPriceStop2.Name = "StopOrdersPriceStop2";
            this.StopOrdersPriceStop2.ReadOnly = true;
            this.StopOrdersPriceStop2.Width = 70;
            // 
            // StopOrdersSpread
            // 
            this.StopOrdersSpread.HeaderText = "Спред";
            this.StopOrdersSpread.Name = "StopOrdersSpread";
            this.StopOrdersSpread.ReadOnly = true;
            this.StopOrdersSpread.Width = 70;
            // 
            // StopOrdersOffset
            // 
            this.StopOrdersOffset.HeaderText = "Защ. отступ";
            this.StopOrdersOffset.Name = "StopOrdersOffset";
            this.StopOrdersOffset.ReadOnly = true;
            this.StopOrdersOffset.Width = 50;
            // 
            // panelConditionStopList
            // 
            this.panelConditionStopList.Controls.Add(this.groupBoxFiltersStopOrders);
            this.panelConditionStopList.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelConditionStopList.Location = new System.Drawing.Point(0, 0);
            this.panelConditionStopList.Name = "panelConditionStopList";
            this.panelConditionStopList.Size = new System.Drawing.Size(759, 40);
            this.panelConditionStopList.TabIndex = 0;
            // 
            // groupBoxFiltersStopOrders
            // 
            this.groupBoxFiltersStopOrders.Controls.Add(this.checkBoxSOExec);
            this.groupBoxFiltersStopOrders.Controls.Add(this.checkBoxSOClosed);
            this.groupBoxFiltersStopOrders.Controls.Add(this.checkBoxSOActive);
            this.groupBoxFiltersStopOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFiltersStopOrders.Location = new System.Drawing.Point(0, 0);
            this.groupBoxFiltersStopOrders.Name = "groupBoxFiltersStopOrders";
            this.groupBoxFiltersStopOrders.Size = new System.Drawing.Size(759, 40);
            this.groupBoxFiltersStopOrders.TabIndex = 0;
            this.groupBoxFiltersStopOrders.TabStop = false;
            this.groupBoxFiltersStopOrders.Text = "Фильтр стоп-заявок";
            // 
            // checkBoxSOExec
            // 
            this.checkBoxSOExec.AutoSize = true;
            this.checkBoxSOExec.Location = new System.Drawing.Point(172, 17);
            this.checkBoxSOExec.Name = "checkBoxSOExec";
            this.checkBoxSOExec.Size = new System.Drawing.Size(97, 17);
            this.checkBoxSOExec.TabIndex = 2;
            this.checkBoxSOExec.Text = "Выполненные";
            this.checkBoxSOExec.UseVisualStyleBackColor = true;
            this.checkBoxSOExec.CheckedChanged += new System.EventHandler(this.checkBoxSOExec_CheckedChanged);
            // 
            // checkBoxSOClosed
            // 
            this.checkBoxSOClosed.AutoSize = true;
            this.checkBoxSOClosed.Location = new System.Drawing.Point(88, 17);
            this.checkBoxSOClosed.Name = "checkBoxSOClosed";
            this.checkBoxSOClosed.Size = new System.Drawing.Size(78, 17);
            this.checkBoxSOClosed.TabIndex = 1;
            this.checkBoxSOClosed.Text = "Закрытые";
            this.checkBoxSOClosed.UseVisualStyleBackColor = true;
            this.checkBoxSOClosed.CheckedChanged += new System.EventHandler(this.checkBoxSOClosed_CheckedChanged);
            // 
            // checkBoxSOActive
            // 
            this.checkBoxSOActive.AutoSize = true;
            this.checkBoxSOActive.Location = new System.Drawing.Point(6, 17);
            this.checkBoxSOActive.Name = "checkBoxSOActive";
            this.checkBoxSOActive.Size = new System.Drawing.Size(76, 17);
            this.checkBoxSOActive.TabIndex = 0;
            this.checkBoxSOActive.Text = "Активные";
            this.checkBoxSOActive.UseVisualStyleBackColor = true;
            this.checkBoxSOActive.CheckedChanged += new System.EventHandler(this.checkBoxSOActive_CheckedChanged);
            // 
            // tabPageOrders
            // 
            this.tabPageOrders.Controls.Add(this.panel4);
            this.tabPageOrders.Controls.Add(this.panel3);
            this.tabPageOrders.Location = new System.Drawing.Point(4, 22);
            this.tabPageOrders.Name = "tabPageOrders";
            this.tabPageOrders.Size = new System.Drawing.Size(925, 428);
            this.tabPageOrders.TabIndex = 2;
            this.tabPageOrders.Text = "Заявки";
            this.tabPageOrders.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.splitContainerListOrders);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(130, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(795, 428);
            this.panel4.TabIndex = 1;
            // 
            // splitContainerListOrders
            // 
            this.splitContainerListOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerListOrders.Location = new System.Drawing.Point(0, 0);
            this.splitContainerListOrders.Name = "splitContainerListOrders";
            this.splitContainerListOrders.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerListOrders.Panel1
            // 
            this.splitContainerListOrders.Panel1.Controls.Add(this.groupBoxOrders);
            this.splitContainerListOrders.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainerListOrders.Panel2
            // 
            this.splitContainerListOrders.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainerListOrders.Size = new System.Drawing.Size(795, 428);
            this.splitContainerListOrders.SplitterDistance = 267;
            this.splitContainerListOrders.TabIndex = 2;
            // 
            // groupBoxOrders
            // 
            this.groupBoxOrders.Controls.Add(this.dataGridViewOrders);
            this.groupBoxOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxOrders.Location = new System.Drawing.Point(0, 0);
            this.groupBoxOrders.Name = "groupBoxOrders";
            this.groupBoxOrders.Size = new System.Drawing.Size(795, 267);
            this.groupBoxOrders.TabIndex = 0;
            this.groupBoxOrders.TabStop = false;
            this.groupBoxOrders.Text = "Заявки";
            // 
            // dataGridViewOrders
            // 
            this.dataGridViewOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NumOrders,
            this.StatusOrders,
            this.IDOrders,
            this.OrdersObjSec,
            this.SecOrders,
            this.PriceOrders,
            this.VolumeOrders,
            this.BalanceOrders,
            this.DirectionOrders,
            this.NamesOrders});
            this.dataGridViewOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOrders.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewOrders.Name = "dataGridViewOrders";
            this.dataGridViewOrders.RowHeadersVisible = false;
            this.dataGridViewOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOrders.Size = new System.Drawing.Size(789, 248);
            this.dataGridViewOrders.TabIndex = 1;
            this.dataGridViewOrders.DoubleClick += new System.EventHandler(this.DataGridViewOrders_DoubleClick);
            // 
            // NumOrders
            // 
            this.NumOrders.Frozen = true;
            this.NumOrders.HeaderText = "№";
            this.NumOrders.Name = "NumOrders";
            this.NumOrders.ReadOnly = true;
            this.NumOrders.Width = 40;
            // 
            // StatusOrders
            // 
            this.StatusOrders.Frozen = true;
            this.StatusOrders.HeaderText = "Статус";
            this.StatusOrders.Name = "StatusOrders";
            this.StatusOrders.ReadOnly = true;
            this.StatusOrders.Width = 50;
            // 
            // IDOrders
            // 
            this.IDOrders.Frozen = true;
            this.IDOrders.HeaderText = "Номер заяв.";
            this.IDOrders.Name = "IDOrders";
            this.IDOrders.ReadOnly = true;
            // 
            // OrdersObjSec
            // 
            this.OrdersObjSec.Frozen = true;
            this.OrdersObjSec.HeaderText = "ObjSec";
            this.OrdersObjSec.Name = "OrdersObjSec";
            this.OrdersObjSec.ReadOnly = true;
            this.OrdersObjSec.Visible = false;
            // 
            // SecOrders
            // 
            this.SecOrders.Frozen = true;
            this.SecOrders.HeaderText = "Инструмент";
            this.SecOrders.Name = "SecOrders";
            this.SecOrders.ReadOnly = true;
            // 
            // PriceOrders
            // 
            this.PriceOrders.Frozen = true;
            this.PriceOrders.HeaderText = "Цена";
            this.PriceOrders.Name = "PriceOrders";
            this.PriceOrders.ReadOnly = true;
            this.PriceOrders.Width = 50;
            // 
            // VolumeOrders
            // 
            this.VolumeOrders.Frozen = true;
            this.VolumeOrders.HeaderText = "Объем";
            this.VolumeOrders.Name = "VolumeOrders";
            this.VolumeOrders.ReadOnly = true;
            this.VolumeOrders.Width = 50;
            // 
            // BalanceOrders
            // 
            this.BalanceOrders.Frozen = true;
            this.BalanceOrders.HeaderText = "Баланс";
            this.BalanceOrders.Name = "BalanceOrders";
            this.BalanceOrders.ReadOnly = true;
            this.BalanceOrders.Width = 50;
            // 
            // DirectionOrders
            // 
            this.DirectionOrders.Frozen = true;
            this.DirectionOrders.HeaderText = "Направление";
            this.DirectionOrders.Name = "DirectionOrders";
            this.DirectionOrders.ReadOnly = true;
            this.DirectionOrders.Width = 90;
            // 
            // NamesOrders
            // 
            this.NamesOrders.Frozen = true;
            this.NamesOrders.HeaderText = "Название";
            this.NamesOrders.Name = "NamesOrders";
            this.NamesOrders.ReadOnly = true;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(130, 428);
            this.panel3.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(925, 428);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Главная";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(338, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(582, 420);
            this.panel2.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.PanelPortfolios);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.PanelPositions);
            this.splitContainer1.Size = new System.Drawing.Size(582, 420);
            this.splitContainer1.SplitterDistance = 166;
            this.splitContainer1.TabIndex = 0;
            // 
            // PanelPortfolios
            // 
            this.PanelPortfolios.Controls.Add(this.dataGridPortfolios);
            this.PanelPortfolios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelPortfolios.Location = new System.Drawing.Point(0, 0);
            this.PanelPortfolios.Name = "PanelPortfolios";
            this.PanelPortfolios.Size = new System.Drawing.Size(582, 166);
            this.PanelPortfolios.TabIndex = 3;
            this.PanelPortfolios.TabStop = false;
            this.PanelPortfolios.Text = "Портфели";
            // 
            // dataGridPortfolios
            // 
            this.dataGridPortfolios.AllowUserToOrderColumns = true;
            this.dataGridPortfolios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPortfolios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Account,
            this.LimitKind,
            this.typeClient,
            this.Balance,
            this.CurBalance,
            this.PosBalanse,
            this.VarMargin,
            this.Commision});
            this.dataGridPortfolios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridPortfolios.Location = new System.Drawing.Point(3, 16);
            this.dataGridPortfolios.Name = "dataGridPortfolios";
            this.dataGridPortfolios.ReadOnly = true;
            this.dataGridPortfolios.RowHeadersVisible = false;
            this.dataGridPortfolios.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridPortfolios.RowTemplate.Height = 18;
            this.dataGridPortfolios.RowTemplate.ReadOnly = true;
            this.dataGridPortfolios.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridPortfolios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridPortfolios.Size = new System.Drawing.Size(576, 147);
            this.dataGridPortfolios.TabIndex = 0;
            // 
            // Account
            // 
            this.Account.Frozen = true;
            this.Account.HeaderText = "Счет";
            this.Account.Name = "Account";
            this.Account.ReadOnly = true;
            this.Account.Width = 80;
            // 
            // LimitKind
            // 
            this.LimitKind.Frozen = true;
            this.LimitKind.HeaderText = "Тип";
            this.LimitKind.Name = "LimitKind";
            this.LimitKind.ReadOnly = true;
            this.LimitKind.Width = 30;
            // 
            // typeClient
            // 
            this.typeClient.Frozen = true;
            this.typeClient.HeaderText = "Тип кл.";
            this.typeClient.Name = "typeClient";
            this.typeClient.ReadOnly = true;
            this.typeClient.Width = 50;
            // 
            // Balance
            // 
            this.Balance.Frozen = true;
            this.Balance.HeaderText = "Баланс";
            this.Balance.Name = "Balance";
            this.Balance.ReadOnly = true;
            this.Balance.Width = 90;
            // 
            // CurBalance
            // 
            this.CurBalance.Frozen = true;
            this.CurBalance.HeaderText = "Тек.Баланс";
            this.CurBalance.Name = "CurBalance";
            this.CurBalance.ReadOnly = true;
            this.CurBalance.Width = 90;
            // 
            // PosBalanse
            // 
            this.PosBalanse.Frozen = true;
            this.PosBalanse.HeaderText = "Баланс_поз";
            this.PosBalanse.Name = "PosBalanse";
            this.PosBalanse.ReadOnly = true;
            this.PosBalanse.Width = 90;
            // 
            // VarMargin
            // 
            this.VarMargin.Frozen = true;
            this.VarMargin.HeaderText = "Вар_маржа";
            this.VarMargin.Name = "VarMargin";
            this.VarMargin.ReadOnly = true;
            this.VarMargin.Width = 90;
            // 
            // Commision
            // 
            this.Commision.Frozen = true;
            this.Commision.HeaderText = "Бир_сбор";
            this.Commision.Name = "Commision";
            this.Commision.ReadOnly = true;
            this.Commision.Width = 70;
            // 
            // PanelPositions
            // 
            this.PanelPositions.Controls.Add(this.dataGridPositions);
            this.PanelPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelPositions.Location = new System.Drawing.Point(0, 0);
            this.PanelPositions.Name = "PanelPositions";
            this.PanelPositions.Size = new System.Drawing.Size(582, 250);
            this.PanelPositions.TabIndex = 5;
            this.PanelPositions.TabStop = false;
            this.PanelPositions.Text = "Позиции";
            // 
            // dataGridPositions
            // 
            this.dataGridPositions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPositions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NamePos,
            this.Code,
            this.ColumnLot,
            this.StepPrice,
            this.ColumnGO,
            this.ActPoss,
            this.Orders,
            this.PosVarMargin,
            this.BtnGetDepth});
            this.dataGridPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridPositions.Location = new System.Drawing.Point(3, 16);
            this.dataGridPositions.Name = "dataGridPositions";
            this.dataGridPositions.RowHeadersVisible = false;
            this.dataGridPositions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridPositions.Size = new System.Drawing.Size(576, 231);
            this.dataGridPositions.TabIndex = 1;
            this.dataGridPositions.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridPositions_CellContentClick);
            // 
            // NamePos
            // 
            this.NamePos.Frozen = true;
            this.NamePos.HeaderText = "Назв.";
            this.NamePos.Name = "NamePos";
            this.NamePos.ReadOnly = true;
            this.NamePos.Width = 130;
            // 
            // Code
            // 
            this.Code.Frozen = true;
            this.Code.HeaderText = "Код";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 70;
            // 
            // ColumnLot
            // 
            this.ColumnLot.Frozen = true;
            this.ColumnLot.HeaderText = "Лот";
            this.ColumnLot.Name = "ColumnLot";
            this.ColumnLot.ReadOnly = true;
            this.ColumnLot.Width = 50;
            // 
            // StepPrice
            // 
            this.StepPrice.Frozen = true;
            this.StepPrice.HeaderText = "Стоим. шага";
            this.StepPrice.Name = "StepPrice";
            this.StepPrice.ReadOnly = true;
            this.StepPrice.Width = 70;
            // 
            // ColumnGO
            // 
            this.ColumnGO.Frozen = true;
            this.ColumnGO.HeaderText = "ГО";
            this.ColumnGO.Name = "ColumnGO";
            this.ColumnGO.ReadOnly = true;
            this.ColumnGO.Width = 80;
            // 
            // ActPoss
            // 
            this.ActPoss.Frozen = true;
            this.ActPoss.HeaderText = "Кол. акт. поз.";
            this.ActPoss.Name = "ActPoss";
            this.ActPoss.ReadOnly = true;
            // 
            // Orders
            // 
            this.Orders.Frozen = true;
            this.Orders.HeaderText = "Заявки";
            this.Orders.Name = "Orders";
            this.Orders.ReadOnly = true;
            // 
            // PosVarMargin
            // 
            this.PosVarMargin.Frozen = true;
            this.PosVarMargin.HeaderText = "Вар_марж";
            this.PosVarMargin.Name = "PosVarMargin";
            this.PosVarMargin.ReadOnly = true;
            // 
            // BtnGetDepth
            // 
            this.BtnGetDepth.Frozen = true;
            this.BtnGetDepth.HeaderText = "Раб. окно";
            this.BtnGetDepth.Name = "BtnGetDepth";
            this.BtnGetDepth.ReadOnly = true;
            this.BtnGetDepth.Text = "Get win";
            this.BtnGetDepth.UseColumnTextForButtonValue = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 420);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Controls.Add(this.label6, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxSearchSec, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataGridFoundSec, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonOpenFoundDepth, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonSaveSecurity, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelDescription, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 251F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(335, 420);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(13, 13);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Поиск";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxSearchSec
            // 
            this.textBoxSearchSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSearchSec.Location = new System.Drawing.Point(128, 11);
            this.textBoxSearchSec.Margin = new System.Windows.Forms.Padding(1);
            this.textBoxSearchSec.Name = "textBoxSearchSec";
            this.textBoxSearchSec.Size = new System.Drawing.Size(196, 20);
            this.textBoxSearchSec.TabIndex = 2;
            this.textBoxSearchSec.TextChanged += new System.EventHandler(this.TextBoxSearchSec_TextChanged);
            // 
            // dataGridFoundSec
            // 
            this.dataGridFoundSec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridFoundSec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CodeSec,
            this.NameSec});
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridFoundSec, 2);
            this.dataGridFoundSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridFoundSec.Location = new System.Drawing.Point(11, 32);
            this.dataGridFoundSec.Margin = new System.Windows.Forms.Padding(1);
            this.dataGridFoundSec.Name = "dataGridFoundSec";
            this.dataGridFoundSec.RowHeadersVisible = false;
            this.dataGridFoundSec.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridFoundSec.ShowEditingIcon = false;
            this.dataGridFoundSec.Size = new System.Drawing.Size(313, 98);
            this.dataGridFoundSec.TabIndex = 3;
            // 
            // CodeSec
            // 
            this.CodeSec.HeaderText = "Код";
            this.CodeSec.Name = "CodeSec";
            this.CodeSec.ReadOnly = true;
            // 
            // NameSec
            // 
            this.NameSec.HeaderText = "Название";
            this.NameSec.Name = "NameSec";
            this.NameSec.ReadOnly = true;
            this.NameSec.Width = 210;
            // 
            // buttonOpenFoundDepth
            // 
            this.buttonOpenFoundDepth.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOpenFoundDepth.Location = new System.Drawing.Point(221, 134);
            this.buttonOpenFoundDepth.Name = "buttonOpenFoundDepth";
            this.buttonOpenFoundDepth.Size = new System.Drawing.Size(101, 32);
            this.buttonOpenFoundDepth.TabIndex = 4;
            this.buttonOpenFoundDepth.Text = "Торговое окно";
            this.buttonOpenFoundDepth.UseVisualStyleBackColor = true;
            this.buttonOpenFoundDepth.Click += new System.EventHandler(this.ButtonOpenFoundDepth_Click);
            // 
            // buttonSaveSecurity
            // 
            this.buttonSaveSecurity.Location = new System.Drawing.Point(13, 134);
            this.buttonSaveSecurity.Name = "buttonSaveSecurity";
            this.buttonSaveSecurity.Size = new System.Drawing.Size(103, 32);
            this.buttonSaveSecurity.TabIndex = 5;
            this.buttonSaveSecurity.Text = "Сохранить инстр.";
            this.buttonSaveSecurity.UseVisualStyleBackColor = true;
            this.buttonSaveSecurity.Click += new System.EventHandler(this.buttonSaveSecurity_Click);
            // 
            // labelDescription
            // 
            this.labelDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.labelDescription, 2);
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(15, 174);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(5);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(305, 241);
            this.labelDescription.TabIndex = 6;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPage1);
            this.tabControlMain.Controls.Add(this.tabPageOrders);
            this.tabControlMain.Controls.Add(this.tabPageStopOrders);
            this.tabControlMain.Controls.Add(this.tabPageSignals);
            this.tabControlMain.Controls.Add(this.tabPageFastGaps);
            this.tabControlMain.Controls.Add(this.tabPageDescription);
            this.tabControlMain.Controls.Add(this.tabPageAutoOrders);
            this.tabControlMain.Controls.Add(this.tabPageAutoStops);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 24);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(933, 454);
            this.tabControlMain.TabIndex = 3;
            // 
            // tabPageSignals
            // 
            this.tabPageSignals.Controls.Add(this.tableLayoutPanel19);
            this.tabPageSignals.Location = new System.Drawing.Point(4, 22);
            this.tabPageSignals.Name = "tabPageSignals";
            this.tabPageSignals.Size = new System.Drawing.Size(925, 428);
            this.tabPageSignals.TabIndex = 4;
            this.tabPageSignals.Text = "Сигналы";
            this.tabPageSignals.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel19
            // 
            this.tableLayoutPanel19.ColumnCount = 1;
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel19.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel19.Controls.Add(this.tableLayoutPanel14, 0, 1);
            this.tableLayoutPanel19.Controls.Add(this.splitContainer2, 0, 3);
            this.tableLayoutPanel19.Controls.Add(this.tableLayoutPanel8, 0, 2);
            this.tableLayoutPanel19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel19.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel19.Name = "tableLayoutPanel19";
            this.tableLayoutPanel19.RowCount = 4;
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel19.Size = new System.Drawing.Size(925, 428);
            this.tableLayoutPanel19.TabIndex = 2;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 9;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 272F));
            this.tableLayoutPanel6.Controls.Add(this.comboBoxSecSign, 7, 0);
            this.tableLayoutPanel6.Controls.Add(this.label3, 6, 0);
            this.tableLayoutPanel6.Controls.Add(this.buttonClearSignalLog, 4, 0);
            this.tableLayoutPanel6.Controls.Add(this.buttonSignTestCall, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.buttonSignTestDev, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.buttonRestartSignalPort, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.comboBoxPorts, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.labelSignNameSec, 8, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(920, 28);
            this.tableLayoutPanel6.TabIndex = 3;
            // 
            // comboBoxSecSign
            // 
            this.comboBoxSecSign.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboBoxSecSign.FormattingEnabled = true;
            this.comboBoxSecSign.Location = new System.Drawing.Point(474, 4);
            this.comboBoxSecSign.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxSecSign.Name = "comboBoxSecSign";
            this.comboBoxSecSign.Size = new System.Drawing.Size(170, 21);
            this.comboBoxSecSign.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(401, 1);
            this.label3.Margin = new System.Windows.Forms.Padding(1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 26);
            this.label3.TabIndex = 11;
            this.label3.Text = "Инструмент";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonClearSignalLog
            // 
            this.buttonClearSignalLog.Location = new System.Drawing.Point(251, 1);
            this.buttonClearSignalLog.Margin = new System.Windows.Forms.Padding(1);
            this.buttonClearSignalLog.Name = "buttonClearSignalLog";
            this.buttonClearSignalLog.Size = new System.Drawing.Size(90, 23);
            this.buttonClearSignalLog.TabIndex = 2;
            this.buttonClearSignalLog.Text = "Очистить лог";
            this.buttonClearSignalLog.UseVisualStyleBackColor = true;
            this.buttonClearSignalLog.Click += new System.EventHandler(this.buttonClearSignalLog_Click);
            // 
            // buttonSignTestCall
            // 
            this.buttonSignTestCall.Enabled = false;
            this.buttonSignTestCall.Location = new System.Drawing.Point(201, 1);
            this.buttonSignTestCall.Margin = new System.Windows.Forms.Padding(1);
            this.buttonSignTestCall.Name = "buttonSignTestCall";
            this.buttonSignTestCall.Size = new System.Drawing.Size(48, 23);
            this.buttonSignTestCall.TabIndex = 4;
            this.buttonSignTestCall.Text = "Вызов";
            this.buttonSignTestCall.UseVisualStyleBackColor = true;
            this.buttonSignTestCall.Click += new System.EventHandler(this.buttonSignTestCall_Click);
            // 
            // buttonSignTestDev
            // 
            this.buttonSignTestDev.Enabled = false;
            this.buttonSignTestDev.Location = new System.Drawing.Point(151, 1);
            this.buttonSignTestDev.Margin = new System.Windows.Forms.Padding(1);
            this.buttonSignTestDev.Name = "buttonSignTestDev";
            this.buttonSignTestDev.Size = new System.Drawing.Size(48, 23);
            this.buttonSignTestDev.TabIndex = 3;
            this.buttonSignTestDev.Text = "Тест устройства";
            this.buttonSignTestDev.UseVisualStyleBackColor = true;
            this.buttonSignTestDev.Click += new System.EventHandler(this.buttonSignTestDev_Click);
            // 
            // buttonRestartSignalPort
            // 
            this.buttonRestartSignalPort.Location = new System.Drawing.Point(61, 1);
            this.buttonRestartSignalPort.Margin = new System.Windows.Forms.Padding(1);
            this.buttonRestartSignalPort.Name = "buttonRestartSignalPort";
            this.buttonRestartSignalPort.Size = new System.Drawing.Size(86, 23);
            this.buttonRestartSignalPort.TabIndex = 2;
            this.buttonRestartSignalPort.Text = "Активировать ";
            this.buttonRestartSignalPort.UseVisualStyleBackColor = true;
            this.buttonRestartSignalPort.Click += new System.EventHandler(this.buttonRestartSignalPort_Click);
            // 
            // comboBoxPorts
            // 
            this.comboBoxPorts.FormattingEnabled = true;
            this.comboBoxPorts.Location = new System.Drawing.Point(3, 3);
            this.comboBoxPorts.Name = "comboBoxPorts";
            this.comboBoxPorts.Size = new System.Drawing.Size(54, 21);
            this.comboBoxPorts.TabIndex = 5;
            // 
            // labelSignNameSec
            // 
            this.labelSignNameSec.Location = new System.Drawing.Point(649, 1);
            this.labelSignNameSec.Margin = new System.Windows.Forms.Padding(1);
            this.labelSignNameSec.Name = "labelSignNameSec";
            this.labelSignNameSec.Size = new System.Drawing.Size(270, 26);
            this.labelSignNameSec.TabIndex = 12;
            this.labelSignNameSec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 4;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 175F));
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel17, 2, 0);
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel16, 1, 0);
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel15, 0, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(925, 110);
            this.tableLayoutPanel14.TabIndex = 4;
            // 
            // tableLayoutPanel17
            // 
            this.tableLayoutPanel17.ColumnCount = 1;
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel17.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel17.Location = new System.Drawing.Point(503, 3);
            this.tableLayoutPanel17.Name = "tableLayoutPanel17";
            this.tableLayoutPanel17.RowCount = 6;
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel17.Size = new System.Drawing.Size(244, 104);
            this.tableLayoutPanel17.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel9);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 98);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сигнал по времени";
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.60153F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.39847F));
            this.tableLayoutPanel9.Controls.Add(this.buttonAddSignTime, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.dateTimePickerSign, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 3;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(236, 79);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // buttonAddSignTime
            // 
            this.buttonAddSignTime.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonAddSignTime.Location = new System.Drawing.Point(150, 0);
            this.buttonAddSignTime.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddSignTime.Name = "buttonAddSignTime";
            this.buttonAddSignTime.Size = new System.Drawing.Size(73, 22);
            this.buttonAddSignTime.TabIndex = 8;
            this.buttonAddSignTime.Text = "Добавить";
            this.buttonAddSignTime.UseVisualStyleBackColor = true;
            this.buttonAddSignTime.Click += new System.EventHandler(this.buttonAddSignTime_Click);
            // 
            // dateTimePickerSign
            // 
            this.dateTimePickerSign.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerSign.Location = new System.Drawing.Point(1, 1);
            this.dateTimePickerSign.Margin = new System.Windows.Forms.Padding(1);
            this.dateTimePickerSign.Name = "dateTimePickerSign";
            this.dateTimePickerSign.Size = new System.Drawing.Size(148, 20);
            this.dateTimePickerSign.TabIndex = 9;
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.ColumnCount = 1;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(253, 3);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 6;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(244, 104);
            this.tableLayoutPanel16.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel5);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(244, 100);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Сигнал по объемам";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.93277F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.06723F));
            this.tableLayoutPanel5.Controls.Add(this.buttonAddSignVolume, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.comboBoxSignTimeFrame, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(238, 81);
            this.tableLayoutPanel5.TabIndex = 4;
            // 
            // buttonAddSignVolume
            // 
            this.buttonAddSignVolume.Location = new System.Drawing.Point(75, 44);
            this.buttonAddSignVolume.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddSignVolume.Name = "buttonAddSignVolume";
            this.buttonAddSignVolume.Size = new System.Drawing.Size(86, 22);
            this.buttonAddSignVolume.TabIndex = 4;
            this.buttonAddSignVolume.Text = "Добавить";
            this.buttonAddSignVolume.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.15094F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.84906F));
            this.tableLayoutPanel7.Controls.Add(this.buttonSignAddVol1000, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.numericUpDownSignVolume, 0, 0);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(75, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(140, 22);
            this.tableLayoutPanel7.TabIndex = 12;
            // 
            // buttonSignAddVol1000
            // 
            this.buttonSignAddVol1000.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSignAddVol1000.Location = new System.Drawing.Point(89, 0);
            this.buttonSignAddVol1000.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSignAddVol1000.Name = "buttonSignAddVol1000";
            this.buttonSignAddVol1000.Size = new System.Drawing.Size(51, 22);
            this.buttonSignAddVol1000.TabIndex = 8;
            this.buttonSignAddVol1000.Text = "+1000";
            this.buttonSignAddVol1000.UseVisualStyleBackColor = true;
            // 
            // numericUpDownSignVolume
            // 
            this.numericUpDownSignVolume.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownSignVolume.Location = new System.Drawing.Point(1, 1);
            this.numericUpDownSignVolume.Margin = new System.Windows.Forms.Padding(1);
            this.numericUpDownSignVolume.Name = "numericUpDownSignVolume";
            this.numericUpDownSignVolume.Size = new System.Drawing.Size(87, 20);
            this.numericUpDownSignVolume.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(1, 1);
            this.label4.Margin = new System.Windows.Forms.Padding(1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Объем";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxSignTimeFrame
            // 
            this.comboBoxSignTimeFrame.FormattingEnabled = true;
            this.comboBoxSignTimeFrame.Location = new System.Drawing.Point(76, 23);
            this.comboBoxSignTimeFrame.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxSignTimeFrame.Name = "comboBoxSignTimeFrame";
            this.comboBoxSignTimeFrame.Size = new System.Drawing.Size(87, 21);
            this.comboBoxSignTimeFrame.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(1, 23);
            this.label7.Margin = new System.Windows.Forms.Padding(1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 20);
            this.label7.TabIndex = 8;
            this.label7.Text = "Тайм фрейм";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 1;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 6;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(244, 104);
            this.tableLayoutPanel15.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanelSign);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 100);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сигнал по цене";
            // 
            // tableLayoutPanelSign
            // 
            this.tableLayoutPanelSign.ColumnCount = 2;
            this.tableLayoutPanelSign.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.05042F));
            this.tableLayoutPanelSign.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.94958F));
            this.tableLayoutPanelSign.Controls.Add(this.buttonAddSign, 1, 2);
            this.tableLayoutPanelSign.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanelSign.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelSign.Controls.Add(this.comboBoxCond, 1, 1);
            this.tableLayoutPanelSign.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanelSign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSign.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelSign.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelSign.Name = "tableLayoutPanelSign";
            this.tableLayoutPanelSign.RowCount = 4;
            this.tableLayoutPanelSign.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelSign.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelSign.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelSign.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelSign.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSign.Size = new System.Drawing.Size(238, 81);
            this.tableLayoutPanelSign.TabIndex = 4;
            // 
            // buttonAddSign
            // 
            this.buttonAddSign.Location = new System.Drawing.Point(62, 44);
            this.buttonAddSign.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddSign.Name = "buttonAddSign";
            this.buttonAddSign.Size = new System.Drawing.Size(86, 22);
            this.buttonAddSign.TabIndex = 4;
            this.buttonAddSign.Text = "Добавить";
            this.buttonAddSign.UseVisualStyleBackColor = true;
            this.buttonAddSign.Click += new System.EventHandler(this.buttonAddSign_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.15094F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.84906F));
            this.tableLayoutPanel4.Controls.Add(this.buttonLastPrice, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.numericUpDownPrice, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(62, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(140, 22);
            this.tableLayoutPanel4.TabIndex = 12;
            // 
            // buttonLastPrice
            // 
            this.buttonLastPrice.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonLastPrice.Font = new System.Drawing.Font("Courier New", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLastPrice.Location = new System.Drawing.Point(89, 0);
            this.buttonLastPrice.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLastPrice.Name = "buttonLastPrice";
            this.buttonLastPrice.Size = new System.Drawing.Size(30, 22);
            this.buttonLastPrice.TabIndex = 8;
            this.buttonLastPrice.Text = "◄";
            this.buttonLastPrice.UseVisualStyleBackColor = true;
            this.buttonLastPrice.Click += new System.EventHandler(this.buttonLastPrice_Click);
            // 
            // numericUpDownPrice
            // 
            this.numericUpDownPrice.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownPrice.Location = new System.Drawing.Point(1, 1);
            this.numericUpDownPrice.Margin = new System.Windows.Forms.Padding(1);
            this.numericUpDownPrice.Name = "numericUpDownPrice";
            this.numericUpDownPrice.Size = new System.Drawing.Size(87, 20);
            this.numericUpDownPrice.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Цена";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxCond
            // 
            this.comboBoxCond.FormattingEnabled = true;
            this.comboBoxCond.Location = new System.Drawing.Point(63, 23);
            this.comboBoxCond.Margin = new System.Windows.Forms.Padding(1);
            this.comboBoxCond.Name = "comboBoxCond";
            this.comboBoxCond.Size = new System.Drawing.Size(87, 21);
            this.comboBoxCond.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(1, 23);
            this.label2.Margin = new System.Windows.Forms.Padding(1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Условие";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 191);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanelSettingsSign);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer2.Size = new System.Drawing.Size(919, 234);
            this.splitContainer2.SplitterDistance = 468;
            this.splitContainer2.TabIndex = 0;
            // 
            // tableLayoutPanelSettingsSign
            // 
            this.tableLayoutPanelSettingsSign.ColumnCount = 1;
            this.tableLayoutPanelSettingsSign.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSettingsSign.Controls.Add(this.tableLayoutPanel11, 0, 0);
            this.tableLayoutPanelSettingsSign.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tableLayoutPanelSettingsSign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSettingsSign.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSettingsSign.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelSettingsSign.Name = "tableLayoutPanelSettingsSign";
            this.tableLayoutPanelSettingsSign.RowCount = 1;
            this.tableLayoutPanelSettingsSign.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSettingsSign.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 234F));
            this.tableLayoutPanelSettingsSign.Size = new System.Drawing.Size(468, 234);
            this.tableLayoutPanelSettingsSign.TabIndex = 5;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel11.Controls.Add(this.dataGridViewListSign, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.tableLayoutPanel12, 1, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(468, 234);
            this.tableLayoutPanel11.TabIndex = 8;
            // 
            // dataGridViewListSign
            // 
            this.dataGridViewListSign.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewListSign.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.ValueSignal,
            this.ConditionSignal});
            this.dataGridViewListSign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewListSign.Location = new System.Drawing.Point(1, 1);
            this.dataGridViewListSign.Margin = new System.Windows.Forms.Padding(1);
            this.dataGridViewListSign.MultiSelect = false;
            this.dataGridViewListSign.Name = "dataGridViewListSign";
            this.dataGridViewListSign.ReadOnly = true;
            this.dataGridViewListSign.RowHeadersVisible = false;
            this.dataGridViewListSign.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewListSign.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewListSign.Size = new System.Drawing.Size(406, 232);
            this.dataGridViewListSign.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Инструмент";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // ValueSignal
            // 
            this.ValueSignal.HeaderText = "Значение";
            this.ValueSignal.Name = "ValueSignal";
            this.ValueSignal.ReadOnly = true;
            this.ValueSignal.Width = 160;
            // 
            // ConditionSignal
            // 
            this.ConditionSignal.HeaderText = "Условие";
            this.ConditionSignal.Name = "ConditionSignal";
            this.ConditionSignal.ReadOnly = true;
            this.ConditionSignal.Width = 120;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 1;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Controls.Add(this.buttonSignDown, 0, 1);
            this.tableLayoutPanel12.Controls.Add(this.buttonSignUp, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.buttonDelSign, 0, 6);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(408, 0);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 7;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(60, 234);
            this.tableLayoutPanel12.TabIndex = 4;
            // 
            // buttonSignDown
            // 
            this.buttonSignDown.Location = new System.Drawing.Point(0, 22);
            this.buttonSignDown.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSignDown.Name = "buttonSignDown";
            this.buttonSignDown.Size = new System.Drawing.Size(60, 22);
            this.buttonSignDown.TabIndex = 9;
            this.buttonSignDown.Text = "▼";
            this.buttonSignDown.UseVisualStyleBackColor = true;
            // 
            // buttonSignUp
            // 
            this.buttonSignUp.Location = new System.Drawing.Point(0, 0);
            this.buttonSignUp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSignUp.Name = "buttonSignUp";
            this.buttonSignUp.Size = new System.Drawing.Size(60, 22);
            this.buttonSignUp.TabIndex = 8;
            this.buttonSignUp.Text = "▲";
            this.buttonSignUp.UseVisualStyleBackColor = true;
            // 
            // buttonDelSign
            // 
            this.buttonDelSign.Location = new System.Drawing.Point(0, 168);
            this.buttonDelSign.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDelSign.Name = "buttonDelSign";
            this.buttonDelSign.Size = new System.Drawing.Size(60, 20);
            this.buttonDelSign.TabIndex = 7;
            this.buttonDelSign.Text = "Удалить";
            this.buttonDelSign.UseVisualStyleBackColor = true;
            this.buttonDelSign.Click += new System.EventHandler(this.buttonDelSign_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxLogSign, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxLogDev, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 234F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(447, 234);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // textBoxLogSign
            // 
            this.textBoxLogSign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogSign.Location = new System.Drawing.Point(1, 1);
            this.textBoxLogSign.Margin = new System.Windows.Forms.Padding(1);
            this.textBoxLogSign.Multiline = true;
            this.textBoxLogSign.Name = "textBoxLogSign";
            this.textBoxLogSign.ReadOnly = true;
            this.textBoxLogSign.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLogSign.Size = new System.Drawing.Size(221, 232);
            this.textBoxLogSign.TabIndex = 2;
            // 
            // textBoxLogDev
            // 
            this.textBoxLogDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogDev.Location = new System.Drawing.Point(224, 1);
            this.textBoxLogDev.Margin = new System.Windows.Forms.Padding(1);
            this.textBoxLogDev.Multiline = true;
            this.textBoxLogDev.Name = "textBoxLogDev";
            this.textBoxLogDev.ReadOnly = true;
            this.textBoxLogDev.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLogDev.Size = new System.Drawing.Size(222, 232);
            this.textBoxLogDev.TabIndex = 1;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.667389F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 91.33261F));
            this.tableLayoutPanel8.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.textBoxSignComment, 1, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(1, 139);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(923, 48);
            this.tableLayoutPanel8.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(1, 1);
            this.label5.Margin = new System.Windows.Forms.Padding(1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Комментарий";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxSignComment
            // 
            this.textBoxSignComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSignComment.Location = new System.Drawing.Point(80, 1);
            this.textBoxSignComment.Margin = new System.Windows.Forms.Padding(1);
            this.textBoxSignComment.Multiline = true;
            this.textBoxSignComment.Name = "textBoxSignComment";
            this.textBoxSignComment.Size = new System.Drawing.Size(842, 46);
            this.textBoxSignComment.TabIndex = 17;
            // 
            // tabPageFastGaps
            // 
            this.tabPageFastGaps.Controls.Add(this.tableLayoutPanel3);
            this.tabPageFastGaps.Location = new System.Drawing.Point(4, 22);
            this.tabPageFastGaps.Name = "tabPageFastGaps";
            this.tabPageFastGaps.Size = new System.Drawing.Size(925, 428);
            this.tabPageFastGaps.TabIndex = 5;
            this.tabPageFastGaps.Text = "Поиск гэпов";
            this.tabPageFastGaps.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.75676F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.24324F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel10, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.textBoxFGLog, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel20, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(925, 428);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.89474F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.10526F));
            this.tableLayoutPanel10.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel10.Controls.Add(this.numericUpDownFGTimeStep, 1, 2);
            this.tableLayoutPanel10.Controls.Add(this.numericUpDownFGSizeGap, 1, 1);
            this.tableLayoutPanel10.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.checkBoxFGActivate, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.comboBoxFGTimeFrame, 1, 3);
            this.tableLayoutPanel10.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 8;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(297, 192);
            this.tableLayoutPanel10.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 25);
            this.label9.TabIndex = 1;
            this.label9.Text = "Интервал отсечки, сек";
            // 
            // numericUpDownFGTimeStep
            // 
            this.numericUpDownFGTimeStep.Location = new System.Drawing.Point(100, 53);
            this.numericUpDownFGTimeStep.Name = "numericUpDownFGTimeStep";
            this.numericUpDownFGTimeStep.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownFGTimeStep.TabIndex = 3;
            this.numericUpDownFGTimeStep.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericUpDownFGSizeGap
            // 
            this.numericUpDownFGSizeGap.Location = new System.Drawing.Point(100, 28);
            this.numericUpDownFGSizeGap.Name = "numericUpDownFGSizeGap";
            this.numericUpDownFGSizeGap.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownFGSizeGap.TabIndex = 2;
            this.numericUpDownFGSizeGap.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 25);
            this.label8.TabIndex = 0;
            this.label8.Text = "Размер гэпа, тиков";
            // 
            // checkBoxFGActivate
            // 
            this.checkBoxFGActivate.AutoSize = true;
            this.checkBoxFGActivate.Location = new System.Drawing.Point(100, 3);
            this.checkBoxFGActivate.Name = "checkBoxFGActivate";
            this.checkBoxFGActivate.Size = new System.Drawing.Size(153, 17);
            this.checkBoxFGActivate.TabIndex = 8;
            this.checkBoxFGActivate.Text = "Активировать стратегию";
            this.checkBoxFGActivate.UseVisualStyleBackColor = true;
            // 
            // comboBoxFGTimeFrame
            // 
            this.comboBoxFGTimeFrame.FormattingEnabled = true;
            this.comboBoxFGTimeFrame.Location = new System.Drawing.Point(100, 78);
            this.comboBoxFGTimeFrame.Name = "comboBoxFGTimeFrame";
            this.comboBoxFGTimeFrame.Size = new System.Drawing.Size(101, 21);
            this.comboBoxFGTimeFrame.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Тайм фрэйм";
            // 
            // textBoxFGLog
            // 
            this.textBoxFGLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFGLog.Location = new System.Drawing.Point(3, 201);
            this.textBoxFGLog.Multiline = true;
            this.textBoxFGLog.Name = "textBoxFGLog";
            this.tableLayoutPanel3.SetRowSpan(this.textBoxFGLog, 2);
            this.textBoxFGLog.Size = new System.Drawing.Size(297, 224);
            this.textBoxFGLog.TabIndex = 1;
            // 
            // tableLayoutPanel20
            // 
            this.tableLayoutPanel20.ColumnCount = 2;
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.83871F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.16129F));
            this.tableLayoutPanel20.Location = new System.Drawing.Point(304, 1);
            this.tableLayoutPanel20.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel20.Name = "tableLayoutPanel20";
            this.tableLayoutPanel20.RowCount = 1;
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 196F));
            this.tableLayoutPanel20.Size = new System.Drawing.Size(620, 196);
            this.tableLayoutPanel20.TabIndex = 2;
            // 
            // tabPageDescription
            // 
            this.tabPageDescription.Controls.Add(this.tableLayoutPanel13);
            this.tabPageDescription.Location = new System.Drawing.Point(4, 22);
            this.tabPageDescription.Name = "tabPageDescription";
            this.tabPageDescription.Size = new System.Drawing.Size(925, 428);
            this.tabPageDescription.TabIndex = 6;
            this.tabPageDescription.Text = "Описание";
            this.tabPageDescription.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 1;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel13.Controls.Add(this.tableLayoutPanel18, 0, 1);
            this.tableLayoutPanel13.Controls.Add(this.textBoxDescription, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 2;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.18691F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.813084F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(925, 428);
            this.tableLayoutPanel13.TabIndex = 0;
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 6;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 139F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 241F));
            this.tableLayoutPanel18.Controls.Add(this.buttonDescSave, 0, 0);
            this.tableLayoutPanel18.Location = new System.Drawing.Point(3, 388);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 1;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(723, 37);
            this.tableLayoutPanel18.TabIndex = 0;
            // 
            // buttonDescSave
            // 
            this.buttonDescSave.Location = new System.Drawing.Point(3, 3);
            this.buttonDescSave.Name = "buttonDescSave";
            this.buttonDescSave.Size = new System.Drawing.Size(94, 31);
            this.buttonDescSave.TabIndex = 0;
            this.buttonDescSave.Text = "Сохранить";
            this.buttonDescSave.UseVisualStyleBackColor = true;
            this.buttonDescSave.Click += new System.EventHandler(this.buttonDescSave_Click);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(3, 3);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDescription.Size = new System.Drawing.Size(919, 379);
            this.textBoxDescription.TabIndex = 1;
            // 
            // tabPageAutoOrders
            // 
            this.tabPageAutoOrders.Controls.Add(this.tableLayoutPanel24);
            this.tabPageAutoOrders.Location = new System.Drawing.Point(4, 22);
            this.tabPageAutoOrders.Name = "tabPageAutoOrders";
            this.tabPageAutoOrders.Size = new System.Drawing.Size(925, 428);
            this.tabPageAutoOrders.TabIndex = 7;
            this.tabPageAutoOrders.Text = "Авто-ордера";
            this.tabPageAutoOrders.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel24
            // 
            this.tableLayoutPanel24.ColumnCount = 2;
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.45946F));
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.54054F));
            this.tableLayoutPanel24.Controls.Add(this.tableLayoutPanel21, 0, 0);
            this.tableLayoutPanel24.Controls.Add(this.tableLayoutPanel25, 1, 0);
            this.tableLayoutPanel24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel24.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel24.Name = "tableLayoutPanel24";
            this.tableLayoutPanel24.RowCount = 1;
            this.tableLayoutPanel24.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel24.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 428F));
            this.tableLayoutPanel24.Size = new System.Drawing.Size(925, 428);
            this.tableLayoutPanel24.TabIndex = 0;
            // 
            // tableLayoutPanel21
            // 
            this.tableLayoutPanel21.ColumnCount = 2;
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.11842F));
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.88158F));
            this.tableLayoutPanel21.Controls.Add(this.numericUpDownAOVolume, 1, 2);
            this.tableLayoutPanel21.Controls.Add(this.label13, 0, 2);
            this.tableLayoutPanel21.Controls.Add(this.label12, 0, 1);
            this.tableLayoutPanel21.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel21.Controls.Add(this.comboBoxAOSecurities, 1, 0);
            this.tableLayoutPanel21.Controls.Add(this.numericUpDownAOPrice, 1, 1);
            this.tableLayoutPanel21.Controls.Add(this.tableLayoutPanel22, 1, 3);
            this.tableLayoutPanel21.Controls.Add(this.label14, 0, 4);
            this.tableLayoutPanel21.Controls.Add(this.comboBoxAOAccount, 1, 4);
            this.tableLayoutPanel21.Controls.Add(this.labelAOInfo, 1, 5);
            this.tableLayoutPanel21.Controls.Add(this.labelAOLog, 0, 8);
            this.tableLayoutPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel21.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel21.Name = "tableLayoutPanel21";
            this.tableLayoutPanel21.RowCount = 9;
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel21.Size = new System.Drawing.Size(322, 422);
            this.tableLayoutPanel21.TabIndex = 1;
            // 
            // numericUpDownAOVolume
            // 
            this.numericUpDownAOVolume.Location = new System.Drawing.Point(135, 53);
            this.numericUpDownAOVolume.Name = "numericUpDownAOVolume";
            this.numericUpDownAOVolume.Size = new System.Drawing.Size(99, 20);
            this.numericUpDownAOVolume.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(3, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(125, 25);
            this.label13.TabIndex = 4;
            this.label13.Text = "Объем";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(3, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(125, 25);
            this.label12.TabIndex = 3;
            this.label12.Text = "Цена";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 25);
            this.label11.TabIndex = 1;
            this.label11.Text = "Инструмент";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxAOSecurities
            // 
            this.comboBoxAOSecurities.FormattingEnabled = true;
            this.comboBoxAOSecurities.Location = new System.Drawing.Point(135, 3);
            this.comboBoxAOSecurities.Name = "comboBoxAOSecurities";
            this.comboBoxAOSecurities.Size = new System.Drawing.Size(183, 21);
            this.comboBoxAOSecurities.TabIndex = 0;
            // 
            // numericUpDownAOPrice
            // 
            this.numericUpDownAOPrice.Location = new System.Drawing.Point(135, 28);
            this.numericUpDownAOPrice.Name = "numericUpDownAOPrice";
            this.numericUpDownAOPrice.Size = new System.Drawing.Size(99, 20);
            this.numericUpDownAOPrice.TabIndex = 2;
            // 
            // tableLayoutPanel22
            // 
            this.tableLayoutPanel22.ColumnCount = 2;
            this.tableLayoutPanel22.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel22.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel22.Controls.Add(this.buttonAOSell, 1, 0);
            this.tableLayoutPanel22.Controls.Add(this.buttonAOBuy, 0, 0);
            this.tableLayoutPanel22.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel22.Location = new System.Drawing.Point(132, 75);
            this.tableLayoutPanel22.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel22.Name = "tableLayoutPanel22";
            this.tableLayoutPanel22.RowCount = 1;
            this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel22.Size = new System.Drawing.Size(126, 25);
            this.tableLayoutPanel22.TabIndex = 6;
            // 
            // buttonAOSell
            // 
            this.buttonAOSell.Location = new System.Drawing.Point(64, 1);
            this.buttonAOSell.Margin = new System.Windows.Forms.Padding(1);
            this.buttonAOSell.Name = "buttonAOSell";
            this.buttonAOSell.Size = new System.Drawing.Size(61, 23);
            this.buttonAOSell.TabIndex = 1;
            this.buttonAOSell.Text = "SELL";
            this.buttonAOSell.UseVisualStyleBackColor = true;
            // 
            // buttonAOBuy
            // 
            this.buttonAOBuy.Location = new System.Drawing.Point(1, 1);
            this.buttonAOBuy.Margin = new System.Windows.Forms.Padding(1);
            this.buttonAOBuy.Name = "buttonAOBuy";
            this.buttonAOBuy.Size = new System.Drawing.Size(61, 23);
            this.buttonAOBuy.TabIndex = 0;
            this.buttonAOBuy.Text = "BUY";
            this.buttonAOBuy.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(3, 100);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(125, 25);
            this.label14.TabIndex = 7;
            this.label14.Text = "Кл. счет";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxAOAccount
            // 
            this.comboBoxAOAccount.FormattingEnabled = true;
            this.comboBoxAOAccount.Location = new System.Drawing.Point(135, 103);
            this.comboBoxAOAccount.Name = "comboBoxAOAccount";
            this.comboBoxAOAccount.Size = new System.Drawing.Size(123, 21);
            this.comboBoxAOAccount.TabIndex = 8;
            // 
            // labelAOInfo
            // 
            this.labelAOInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAOInfo.Location = new System.Drawing.Point(135, 125);
            this.labelAOInfo.Name = "labelAOInfo";
            this.labelAOInfo.Size = new System.Drawing.Size(184, 128);
            this.labelAOInfo.TabIndex = 9;
            this.labelAOInfo.Text = "info";
            // 
            // labelAOLog
            // 
            this.labelAOLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel21.SetColumnSpan(this.labelAOLog, 2);
            this.labelAOLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAOLog.Location = new System.Drawing.Point(3, 272);
            this.labelAOLog.Name = "labelAOLog";
            this.labelAOLog.Size = new System.Drawing.Size(316, 150);
            this.labelAOLog.TabIndex = 10;
            // 
            // tableLayoutPanel25
            // 
            this.tableLayoutPanel25.ColumnCount = 1;
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel25.Controls.Add(this.tableLayoutPanel23, 0, 1);
            this.tableLayoutPanel25.Controls.Add(this.dataGridViewAOList, 0, 0);
            this.tableLayoutPanel25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel25.Location = new System.Drawing.Point(331, 3);
            this.tableLayoutPanel25.Name = "tableLayoutPanel25";
            this.tableLayoutPanel25.RowCount = 2;
            this.tableLayoutPanel25.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel25.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel25.Size = new System.Drawing.Size(591, 422);
            this.tableLayoutPanel25.TabIndex = 10;
            // 
            // tableLayoutPanel23
            // 
            this.tableLayoutPanel23.ColumnCount = 6;
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 203F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel23.Controls.Add(this.buttonAODeleteSec, 1, 0);
            this.tableLayoutPanel23.Controls.Add(this.buttonAODelete, 0, 0);
            this.tableLayoutPanel23.Controls.Add(this.checkBoxAOSelectSec, 3, 0);
            this.tableLayoutPanel23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel23.Location = new System.Drawing.Point(1, 393);
            this.tableLayoutPanel23.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel23.Name = "tableLayoutPanel23";
            this.tableLayoutPanel23.RowCount = 1;
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel23.Size = new System.Drawing.Size(589, 28);
            this.tableLayoutPanel23.TabIndex = 7;
            // 
            // buttonAODeleteSec
            // 
            this.buttonAODeleteSec.Location = new System.Drawing.Point(81, 1);
            this.buttonAODeleteSec.Margin = new System.Windows.Forms.Padding(1);
            this.buttonAODeleteSec.Name = "buttonAODeleteSec";
            this.buttonAODeleteSec.Size = new System.Drawing.Size(109, 26);
            this.buttonAODeleteSec.TabIndex = 1;
            this.buttonAODeleteSec.Text = "Удалить по инстр.";
            this.buttonAODeleteSec.UseVisualStyleBackColor = true;
            // 
            // buttonAODelete
            // 
            this.buttonAODelete.Location = new System.Drawing.Point(1, 1);
            this.buttonAODelete.Margin = new System.Windows.Forms.Padding(1);
            this.buttonAODelete.Name = "buttonAODelete";
            this.buttonAODelete.Size = new System.Drawing.Size(78, 26);
            this.buttonAODelete.TabIndex = 0;
            this.buttonAODelete.Text = "Удалить";
            this.buttonAODelete.UseVisualStyleBackColor = true;
            // 
            // checkBoxAOSelectSec
            // 
            this.checkBoxAOSelectSec.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxAOSelectSec.Location = new System.Drawing.Point(241, 1);
            this.checkBoxAOSelectSec.Margin = new System.Windows.Forms.Padding(1);
            this.checkBoxAOSelectSec.Name = "checkBoxAOSelectSec";
            this.checkBoxAOSelectSec.Size = new System.Drawing.Size(151, 26);
            this.checkBoxAOSelectSec.TabIndex = 2;
            this.checkBoxAOSelectSec.Text = "Выбрать по интрументу";
            this.checkBoxAOSelectSec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxAOSelectSec.UseVisualStyleBackColor = true;
            // 
            // dataGridViewAOList
            // 
            this.dataGridViewAOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.ColumnComment});
            this.dataGridViewAOList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAOList.Location = new System.Drawing.Point(1, 1);
            this.dataGridViewAOList.Margin = new System.Windows.Forms.Padding(1);
            this.dataGridViewAOList.Name = "dataGridViewAOList";
            this.dataGridViewAOList.ReadOnly = true;
            this.dataGridViewAOList.RowHeadersVisible = false;
            this.dataGridViewAOList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewAOList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAOList.Size = new System.Drawing.Size(589, 390);
            this.dataGridViewAOList.TabIndex = 6;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Инструмент";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 220;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Цена";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Направление";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 120;
            // 
            // ColumnComment
            // 
            this.ColumnComment.HeaderText = "Коментарий";
            this.ColumnComment.Name = "ColumnComment";
            this.ColumnComment.ReadOnly = true;
            this.ColumnComment.Width = 130;
            // 
            // tabPageAutoStops
            // 
            this.tabPageAutoStops.Controls.Add(this.tableLayoutPanel26);
            this.tabPageAutoStops.Location = new System.Drawing.Point(4, 22);
            this.tabPageAutoStops.Name = "tabPageAutoStops";
            this.tabPageAutoStops.Size = new System.Drawing.Size(925, 428);
            this.tabPageAutoStops.TabIndex = 8;
            this.tabPageAutoStops.Text = "Риск-стопы";
            this.tabPageAutoStops.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel26
            // 
            this.tableLayoutPanel26.ColumnCount = 2;
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.45946F));
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.54054F));
            this.tableLayoutPanel26.Controls.Add(this.tableLayoutPanel29, 1, 0);
            this.tableLayoutPanel26.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel26.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel26.Name = "tableLayoutPanel26";
            this.tableLayoutPanel26.RowCount = 1;
            this.tableLayoutPanel26.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel26.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel26.Size = new System.Drawing.Size(925, 428);
            this.tableLayoutPanel26.TabIndex = 1;
            // 
            // tableLayoutPanel29
            // 
            this.tableLayoutPanel29.ColumnCount = 1;
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel29.Controls.Add(this.tableLayoutPanel30, 0, 1);
            this.tableLayoutPanel29.Controls.Add(this.dataGridViewASLList, 0, 0);
            this.tableLayoutPanel29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel29.Location = new System.Drawing.Point(331, 3);
            this.tableLayoutPanel29.Name = "tableLayoutPanel29";
            this.tableLayoutPanel29.RowCount = 2;
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel29.Size = new System.Drawing.Size(591, 422);
            this.tableLayoutPanel29.TabIndex = 10;
            // 
            // tableLayoutPanel30
            // 
            this.tableLayoutPanel30.ColumnCount = 6;
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 203F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel30.Controls.Add(this.buttonASLDelete, 0, 0);
            this.tableLayoutPanel30.Controls.Add(this.checkBoxASLBySec, 3, 0);
            this.tableLayoutPanel30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel30.Location = new System.Drawing.Point(1, 393);
            this.tableLayoutPanel30.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel30.Name = "tableLayoutPanel30";
            this.tableLayoutPanel30.RowCount = 1;
            this.tableLayoutPanel30.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel30.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel30.Size = new System.Drawing.Size(589, 28);
            this.tableLayoutPanel30.TabIndex = 7;
            // 
            // buttonASLDelete
            // 
            this.buttonASLDelete.Location = new System.Drawing.Point(1, 1);
            this.buttonASLDelete.Margin = new System.Windows.Forms.Padding(1);
            this.buttonASLDelete.Name = "buttonASLDelete";
            this.buttonASLDelete.Size = new System.Drawing.Size(78, 26);
            this.buttonASLDelete.TabIndex = 0;
            this.buttonASLDelete.Text = "Удалить";
            this.buttonASLDelete.UseVisualStyleBackColor = true;
            // 
            // checkBoxASLBySec
            // 
            this.checkBoxASLBySec.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxASLBySec.Location = new System.Drawing.Point(241, 1);
            this.checkBoxASLBySec.Margin = new System.Windows.Forms.Padding(1);
            this.checkBoxASLBySec.Name = "checkBoxASLBySec";
            this.checkBoxASLBySec.Size = new System.Drawing.Size(151, 26);
            this.checkBoxASLBySec.TabIndex = 2;
            this.checkBoxASLBySec.Text = "Выбрать по интрументу";
            this.checkBoxASLBySec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxASLBySec.UseVisualStyleBackColor = true;
            // 
            // dataGridViewASLList
            // 
            this.dataGridViewASLList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewASLList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn8});
            this.dataGridViewASLList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewASLList.Location = new System.Drawing.Point(1, 1);
            this.dataGridViewASLList.Margin = new System.Windows.Forms.Padding(1);
            this.dataGridViewASLList.Name = "dataGridViewASLList";
            this.dataGridViewASLList.ReadOnly = true;
            this.dataGridViewASLList.RowHeadersVisible = false;
            this.dataGridViewASLList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewASLList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewASLList.Size = new System.Drawing.Size(589, 390);
            this.dataGridViewASLList.TabIndex = 6;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Инструмент";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 240;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Стоп, пунктов";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Коментарий";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 130;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel27);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(1, 5);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(1, 5, 1, 1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(326, 422);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Обязательные стоп-завки. Если другие не выствлены";
            // 
            // tableLayoutPanel27
            // 
            this.tableLayoutPanel27.ColumnCount = 2;
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.11842F));
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.88158F));
            this.tableLayoutPanel27.Controls.Add(this.tableLayoutPanel28, 1, 3);
            this.tableLayoutPanel27.Controls.Add(this.label16, 0, 1);
            this.tableLayoutPanel27.Controls.Add(this.label17, 0, 0);
            this.tableLayoutPanel27.Controls.Add(this.comboBoxASLSec, 1, 0);
            this.tableLayoutPanel27.Controls.Add(this.numericUpDownASLTiks, 1, 1);
            this.tableLayoutPanel27.Controls.Add(this.labelASLInfo, 1, 5);
            this.tableLayoutPanel27.Controls.Add(this.labelASLLog, 0, 8);
            this.tableLayoutPanel27.Controls.Add(this.comboBoxASLAccount, 1, 2);
            this.tableLayoutPanel27.Controls.Add(this.label18, 0, 2);
            this.tableLayoutPanel27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel27.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel27.Name = "tableLayoutPanel27";
            this.tableLayoutPanel27.RowCount = 9;
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel27.Size = new System.Drawing.Size(320, 403);
            this.tableLayoutPanel27.TabIndex = 2;
            // 
            // tableLayoutPanel28
            // 
            this.tableLayoutPanel28.ColumnCount = 2;
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel28.Controls.Add(this.buttonASLAdd, 0, 0);
            this.tableLayoutPanel28.Location = new System.Drawing.Point(131, 75);
            this.tableLayoutPanel28.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel28.Name = "tableLayoutPanel28";
            this.tableLayoutPanel28.RowCount = 1;
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel28.Size = new System.Drawing.Size(186, 25);
            this.tableLayoutPanel28.TabIndex = 12;
            // 
            // buttonASLAdd
            // 
            this.buttonASLAdd.Location = new System.Drawing.Point(1, 1);
            this.buttonASLAdd.Margin = new System.Windows.Forms.Padding(1);
            this.buttonASLAdd.Name = "buttonASLAdd";
            this.buttonASLAdd.Size = new System.Drawing.Size(91, 23);
            this.buttonASLAdd.TabIndex = 0;
            this.buttonASLAdd.Text = "Добавить";
            this.buttonASLAdd.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(3, 25);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(125, 25);
            this.label16.TabIndex = 3;
            this.label16.Text = "Величина стопа";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(3, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(125, 25);
            this.label17.TabIndex = 1;
            this.label17.Text = "Инструмент";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxASLSec
            // 
            this.comboBoxASLSec.FormattingEnabled = true;
            this.comboBoxASLSec.Location = new System.Drawing.Point(134, 3);
            this.comboBoxASLSec.Name = "comboBoxASLSec";
            this.comboBoxASLSec.Size = new System.Drawing.Size(183, 21);
            this.comboBoxASLSec.TabIndex = 0;
            // 
            // numericUpDownASLTiks
            // 
            this.numericUpDownASLTiks.Location = new System.Drawing.Point(134, 28);
            this.numericUpDownASLTiks.Name = "numericUpDownASLTiks";
            this.numericUpDownASLTiks.Size = new System.Drawing.Size(99, 20);
            this.numericUpDownASLTiks.TabIndex = 2;
            // 
            // labelASLInfo
            // 
            this.labelASLInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelASLInfo.Location = new System.Drawing.Point(134, 125);
            this.labelASLInfo.Name = "labelASLInfo";
            this.labelASLInfo.Size = new System.Drawing.Size(183, 128);
            this.labelASLInfo.TabIndex = 9;
            this.labelASLInfo.Text = "info";
            // 
            // labelASLLog
            // 
            this.labelASLLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel27.SetColumnSpan(this.labelASLLog, 2);
            this.labelASLLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelASLLog.Location = new System.Drawing.Point(3, 272);
            this.labelASLLog.Name = "labelASLLog";
            this.labelASLLog.Size = new System.Drawing.Size(314, 131);
            this.labelASLLog.TabIndex = 10;
            // 
            // comboBoxASLAccount
            // 
            this.comboBoxASLAccount.FormattingEnabled = true;
            this.comboBoxASLAccount.Location = new System.Drawing.Point(134, 53);
            this.comboBoxASLAccount.Name = "comboBoxASLAccount";
            this.comboBoxASLAccount.Size = new System.Drawing.Size(123, 21);
            this.comboBoxASLAccount.TabIndex = 8;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(3, 50);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(125, 25);
            this.label18.TabIndex = 7;
            this.label18.Text = "Кл. счет";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 500);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Торговый коннектор";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPageStopOrders.ResumeLayout(false);
            this.splitContainerMainOrders.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMainOrders)).EndInit();
            this.splitContainerMainOrders.ResumeLayout(false);
            this.splitContainerTablesStopOrders.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTablesStopOrders)).EndInit();
            this.splitContainerTablesStopOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStopOrders)).EndInit();
            this.panelConditionStopList.ResumeLayout(false);
            this.groupBoxFiltersStopOrders.ResumeLayout(false);
            this.groupBoxFiltersStopOrders.PerformLayout();
            this.tabPageOrders.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.splitContainerListOrders.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerListOrders)).EndInit();
            this.splitContainerListOrders.ResumeLayout(false);
            this.groupBoxOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.PanelPortfolios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPortfolios)).EndInit();
            this.PanelPositions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPositions)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFoundSec)).EndInit();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageSignals.ResumeLayout(false);
            this.tableLayoutPanel19.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel17.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel16.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSignVolume)).EndInit();
            this.tableLayoutPanel15.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanelSign.ResumeLayout(false);
            this.tableLayoutPanelSign.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrice)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanelSettingsSign.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewListSign)).EndInit();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tabPageFastGaps.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFGTimeStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFGSizeGap)).EndInit();
            this.tabPageDescription.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel13.PerformLayout();
            this.tableLayoutPanel18.ResumeLayout(false);
            this.tabPageAutoOrders.ResumeLayout(false);
            this.tableLayoutPanel24.ResumeLayout(false);
            this.tableLayoutPanel21.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAOVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAOPrice)).EndInit();
            this.tableLayoutPanel22.ResumeLayout(false);
            this.tableLayoutPanel25.ResumeLayout(false);
            this.tableLayoutPanel23.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAOList)).EndInit();
            this.tabPageAutoStops.ResumeLayout(false);
            this.tableLayoutPanel26.ResumeLayout(false);
            this.tableLayoutPanel29.ResumeLayout(false);
            this.tableLayoutPanel30.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewASLList)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel27.ResumeLayout(false);
            this.tableLayoutPanel28.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownASLTiks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem подключитьсяToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel1;
		private ToolStripMenuItem дополнительноToolStripMenuItem;
		private ToolStripMenuItem ToolStripMenuItem11;
		private TabPage tabPageStopOrders;
		private SplitContainer splitContainerMainOrders;
		private SplitContainer splitContainerTablesStopOrders;
		private DataGridView dataGridViewStopOrders;
		private DataGridViewTextBoxColumn StopOrdersNum;
		private DataGridViewTextBoxColumn StopOrdersID;
		private DataGridViewTextBoxColumn StopOrdersSec;
		private DataGridViewTextBoxColumn StopOrderыType;
		private DataGridViewTextBoxColumn StopOrdersCondition;
		private DataGridViewTextBoxColumn StopOrdersStatus;
		private DataGridViewTextBoxColumn StopOrdersPrice;
		private DataGridViewTextBoxColumn StopOrdersVolume;
		private DataGridViewTextBoxColumn StopOrdersPriceStop1;
		private DataGridViewTextBoxColumn StopOrdersPriceStop2;
		private DataGridViewTextBoxColumn StopOrdersSpread;
		private DataGridViewTextBoxColumn StopOrdersOffset;
		private Panel panelConditionStopList;
		private GroupBox groupBoxFiltersStopOrders;
		private CheckBox checkBoxSOExec;
		private CheckBox checkBoxSOClosed;
		private CheckBox checkBoxSOActive;
		private TabPage tabPageOrders;
		private Panel panel4;
		private SplitContainer splitContainerListOrders;
		private GroupBox groupBoxOrders;
		private DataGridView dataGridViewOrders;
		private DataGridViewTextBoxColumn NumOrders;
		private DataGridViewTextBoxColumn StatusOrders;
		private DataGridViewTextBoxColumn IDOrders;
		private DataGridViewTextBoxColumn OrdersObjSec;
		private DataGridViewTextBoxColumn SecOrders;
		private DataGridViewTextBoxColumn PriceOrders;
		private DataGridViewTextBoxColumn VolumeOrders;
		private DataGridViewTextBoxColumn BalanceOrders;
		private DataGridViewTextBoxColumn DirectionOrders;
		private DataGridViewTextBoxColumn NamesOrders;
		private Panel panel3;
		private TabPage tabPage1;
		private Panel panel2;
		private SplitContainer splitContainer1;
		private GroupBox PanelPortfolios;
		private DataGridView dataGridPortfolios;
		private GroupBox PanelPositions;
		private Panel panel1;
		private TableLayoutPanel tableLayoutPanel1;
		private Label label6;
		private TextBox textBoxSearchSec;
		private DataGridView dataGridFoundSec;
		private DataGridViewTextBoxColumn CodeSec;
		private DataGridViewTextBoxColumn NameSec;
		private Button buttonOpenFoundDepth;
		private TabControl tabControlMain;
		private ToolStripMenuItem ToolStripMenuItemSpeedOrders;
		private DataGridViewTextBoxColumn Account;
		private DataGridViewTextBoxColumn LimitKind;
		private DataGridViewTextBoxColumn typeClient;
		private DataGridViewTextBoxColumn Balance;
		private DataGridViewTextBoxColumn CurBalance;
		private DataGridViewTextBoxColumn PosBalanse;
		private DataGridViewTextBoxColumn VarMargin;
		private DataGridViewTextBoxColumn Commision;
		private ToolStripMenuItem ToolStripMenuItemSign;
		private ToolStripMenuItem ToolStripMenuItemTestSign;
		private ToolStripMenuItem ToolStripMenuItemSignCall;
		private TabPage tabPageSignals;
		private Button buttonClearSignalLog;
		private ComboBox comboBoxSecSign;
		private Label label3;
		private TableLayoutPanel tableLayoutPanel6;
		private Button buttonSignTestDev;
		private Button buttonRestartSignalPort;
		private Button buttonSignTestCall;
		private ComboBox comboBoxPorts;
		private TableLayoutPanel tableLayoutPanel19;
		private TableLayoutPanel tableLayoutPanel14;
		private TableLayoutPanel tableLayoutPanel17;
		private GroupBox groupBox1;
		private TableLayoutPanel tableLayoutPanel9;
		private Button buttonAddSignTime;
		private DateTimePicker dateTimePickerSign;
		private TableLayoutPanel tableLayoutPanel16;
		private GroupBox groupBox3;
		private TableLayoutPanel tableLayoutPanel5;
		private Button buttonAddSignVolume;
		private TableLayoutPanel tableLayoutPanel7;
		private Button buttonSignAddVol1000;
		private NumericUpDown numericUpDownSignVolume;
		private Label label4;
		private ComboBox comboBoxSignTimeFrame;
		private Label label7;
		private TableLayoutPanel tableLayoutPanel15;
		private GroupBox groupBox2;
		private TableLayoutPanel tableLayoutPanelSign;
		private Button buttonAddSign;
		private TableLayoutPanel tableLayoutPanel4;
		private Button buttonLastPrice;
		private NumericUpDown numericUpDownPrice;
		private Label label1;
		private ComboBox comboBoxCond;
		private Label label2;
		private SplitContainer splitContainer2;
		private TableLayoutPanel tableLayoutPanelSettingsSign;
		private TableLayoutPanel tableLayoutPanel11;
		private DataGridView dataGridViewListSign;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn ValueSignal;
		private DataGridViewTextBoxColumn ConditionSignal;
		private TableLayoutPanel tableLayoutPanel12;
		private Button buttonSignDown;
		private Button buttonSignUp;
		private Button buttonDelSign;
		private TableLayoutPanel tableLayoutPanel2;
		private TextBox textBoxLogSign;
		private TextBox textBoxLogDev;
		private TableLayoutPanel tableLayoutPanel8;
		private Label label5;
		private TextBox textBoxSignComment;
        private Button buttonSaveSecurity;
        private TabPage tabPageFastGaps;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel10;
        private NumericUpDown numericUpDownFGTimeStep;
        private Label label8;
        private Label label9;
        private NumericUpDown numericUpDownFGSizeGap;
        private Label label10;
        private ComboBox comboBoxFGTimeFrame;
        private CheckBox checkBoxFGActivate;
        private TextBox textBoxFGLog;
        private TabPage tabPageDescription;
        private TableLayoutPanel tableLayoutPanel13;
        private TableLayoutPanel tableLayoutPanel18;
        private TextBox textBoxDescription;
        private Button buttonDescSave;
        private Label labelDescription;
        private Label labelSignNameSec;
        private DataGridView dataGridPositions;
        private DataGridViewTextBoxColumn NamePos;
        private DataGridViewTextBoxColumn Code;
        private DataGridViewTextBoxColumn ColumnLot;
        private DataGridViewTextBoxColumn StepPrice;
        private DataGridViewTextBoxColumn ColumnGO;
        private DataGridViewTextBoxColumn ActPoss;
        private DataGridViewTextBoxColumn Orders;
        private DataGridViewTextBoxColumn PosVarMargin;
        private DataGridViewButtonColumn BtnGetDepth;
        private TableLayoutPanel tableLayoutPanel20;
        private TabPage tabPageAutoOrders;
        private TableLayoutPanel tableLayoutPanel24;
        private TableLayoutPanel tableLayoutPanel21;
        private NumericUpDown numericUpDownAOVolume;
        private Label label13;
        private Label label12;
        private Label label11;
        private ComboBox comboBoxAOSecurities;
        private NumericUpDown numericUpDownAOPrice;
        private TableLayoutPanel tableLayoutPanel22;
        private Button buttonAOSell;
        private Button buttonAOBuy;
        private Label label14;
        private ComboBox comboBoxAOAccount;
        private Label labelAOInfo;
        private TableLayoutPanel tableLayoutPanel25;
        private TableLayoutPanel tableLayoutPanel23;
        private Button buttonAODeleteSec;
        private Button buttonAODelete;
        private CheckBox checkBoxAOSelectSec;
        private DataGridView dataGridViewAOList;
        private Label labelAOLog;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn ColumnComment;
        private TabPage tabPageAutoStops;
        private TableLayoutPanel tableLayoutPanel26;
        private TableLayoutPanel tableLayoutPanel29;
        private TableLayoutPanel tableLayoutPanel30;
        private Button buttonASLDelete;
        private CheckBox checkBoxASLBySec;
        private DataGridView dataGridViewASLList;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel27;
        private TableLayoutPanel tableLayoutPanel28;
        private Button buttonASLAdd;
        private Label label16;
        private Label label17;
        private ComboBox comboBoxASLSec;
        private NumericUpDown numericUpDownASLTiks;
        private Label labelASLInfo;
        private Label labelASLLog;
        private ComboBox comboBoxASLAccount;
        private Label label18;
    }
}

