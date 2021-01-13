namespace text.doors.Detection
{
    partial class AirtightDetection
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
            this.components = new System.ComponentModel.Container();
            this.tc_RealTimeSurveillance = new System.Windows.Forms.TabControl();
            this.page_airtight = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_ycjy_z = new System.Windows.Forms.Button();
            this.txt_ycjy_z = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_ycjyf = new System.Windows.Forms.Button();
            this.txt_ycjy_f = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgv_levelIndex = new System.Windows.Forms.DataGridView();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.tChart_qm = new Steema.TeeChart.TChart();
            this.chart_cms_qm_click = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.export_image_qm = new System.Windows.Forms.ToolStripMenuItem();
            this.qm_Line = new Steema.TeeChart.Styles.FastLine();
            this.btn_exit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_dqyl = new System.Windows.Forms.Label();
            this.lbl_setYL = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.流量原始数据 = new System.Windows.Forms.TabPage();
            this.dgv_ll = new System.Windows.Forms.DataGridView();
            this.重复流量数据 = new System.Windows.Forms.TabPage();
            this.dgv_ll2 = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdb_zdstl = new System.Windows.Forms.RadioButton();
            this.btn_losestart = new System.Windows.Forms.Button();
            this.btn_juststart = new System.Windows.Forms.Button();
            this.btn_loseready = new System.Windows.Forms.Button();
            this.btn_justready = new System.Windows.Forms.Button();
            this.rdb_fjstl = new System.Windows.Forms.RadioButton();
            this.btn_datadispose = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.tim_qm = new System.Windows.Forms.Timer(this.components);
            this.tim_getType = new System.Windows.Forms.Timer(this.components);
            this.tim_Top10 = new System.Windows.Forms.Timer(this.components);
            this.gv_list = new System.Windows.Forms.Timer(this.components);
            this.tim_PainPic = new System.Windows.Forms.Timer(this.components);
            this.tc_RealTimeSurveillance.SuspendLayout();
            this.page_airtight.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_levelIndex)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.chart_cms_qm_click.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.流量原始数据.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ll)).BeginInit();
            this.重复流量数据.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ll2)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tc_RealTimeSurveillance
            // 
            this.tc_RealTimeSurveillance.Controls.Add(this.page_airtight);
            this.tc_RealTimeSurveillance.ItemSize = new System.Drawing.Size(120, 26);
            this.tc_RealTimeSurveillance.Location = new System.Drawing.Point(0, 0);
            this.tc_RealTimeSurveillance.Name = "tc_RealTimeSurveillance";
            this.tc_RealTimeSurveillance.SelectedIndex = 0;
            this.tc_RealTimeSurveillance.Size = new System.Drawing.Size(1151, 710);
            this.tc_RealTimeSurveillance.TabIndex = 0;
            // 
            // page_airtight
            // 
            this.page_airtight.BackColor = System.Drawing.Color.White;
            this.page_airtight.Controls.Add(this.groupBox4);
            this.page_airtight.Controls.Add(this.groupBox2);
            this.page_airtight.Controls.Add(this.groupBox9);
            this.page_airtight.Controls.Add(this.btn_exit);
            this.page_airtight.Controls.Add(this.groupBox1);
            this.page_airtight.Controls.Add(this.tabControl1);
            this.page_airtight.Controls.Add(this.groupBox3);
            this.page_airtight.Controls.Add(this.btn_datadispose);
            this.page_airtight.Controls.Add(this.btn_stop);
            this.page_airtight.Location = new System.Drawing.Point(4, 30);
            this.page_airtight.Name = "page_airtight";
            this.page_airtight.Padding = new System.Windows.Forms.Padding(3);
            this.page_airtight.Size = new System.Drawing.Size(1143, 676);
            this.page_airtight.TabIndex = 0;
            this.page_airtight.Text = "气密监控";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_ycjy_z);
            this.groupBox4.Controls.Add(this.txt_ycjy_z);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.btn_ycjyf);
            this.groupBox4.Controls.Add(this.txt_ycjy_f);
            this.groupBox4.Location = new System.Drawing.Point(852, 460);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(285, 73);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "工程检测";
            // 
            // btn_ycjy_z
            // 
            this.btn_ycjy_z.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ycjy_z.Location = new System.Drawing.Point(149, 16);
            this.btn_ycjy_z.Name = "btn_ycjy_z";
            this.btn_ycjy_z.Size = new System.Drawing.Size(95, 23);
            this.btn_ycjy_z.TabIndex = 22;
            this.btn_ycjy_z.Text = "正压以此加压";
            this.btn_ycjy_z.UseVisualStyleBackColor = true;
            this.btn_ycjy_z.Click += new System.EventHandler(this.btn_ycjy_z_Click);
            // 
            // txt_ycjy_z
            // 
            this.txt_ycjy_z.Location = new System.Drawing.Point(54, 16);
            this.txt_ycjy_z.Name = "txt_ycjy_z";
            this.txt_ycjy_z.Size = new System.Drawing.Size(52, 21);
            this.txt_ycjy_z.TabIndex = 21;
            this.txt_ycjy_z.Text = "0";
            this.txt_ycjy_z.TextChanged += new System.EventHandler(this.txt_ycjy_z_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "Pa";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(110, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "Pa";
            // 
            // btn_ycjyf
            // 
            this.btn_ycjyf.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ycjyf.Location = new System.Drawing.Point(149, 43);
            this.btn_ycjyf.Name = "btn_ycjyf";
            this.btn_ycjyf.Size = new System.Drawing.Size(95, 23);
            this.btn_ycjyf.TabIndex = 25;
            this.btn_ycjyf.Text = "负压以此加压";
            this.btn_ycjyf.UseVisualStyleBackColor = true;
            this.btn_ycjyf.Click += new System.EventHandler(this.btn_ycjyf_Click);
            // 
            // txt_ycjy_f
            // 
            this.txt_ycjy_f.Enabled = false;
            this.txt_ycjy_f.Location = new System.Drawing.Point(54, 43);
            this.txt_ycjy_f.Name = "txt_ycjy_f";
            this.txt_ycjy_f.Size = new System.Drawing.Size(52, 21);
            this.txt_ycjy_f.TabIndex = 24;
            this.txt_ycjy_f.Text = "0";
            this.txt_ycjy_f.TextChanged += new System.EventHandler(this.txt_ycjy_f_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgv_levelIndex);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(849, 577);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 90);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "分级指标";
            // 
            // dgv_levelIndex
            // 
            this.dgv_levelIndex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_levelIndex.Location = new System.Drawing.Point(7, 15);
            this.dgv_levelIndex.Name = "dgv_levelIndex";
            this.dgv_levelIndex.RowHeadersWidth = 62;
            this.dgv_levelIndex.RowTemplate.Height = 23;
            this.dgv_levelIndex.Size = new System.Drawing.Size(275, 65);
            this.dgv_levelIndex.TabIndex = 0;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.tChart_qm);
            this.groupBox9.Location = new System.Drawing.Point(8, 87);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(838, 565);
            this.groupBox9.TabIndex = 20;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "检测";
            // 
            // tChart_qm
            // 
            // 
            // 
            // 
            this.tChart_qm.Aspect.ZOffset = 0D;
            this.tChart_qm.BackColor = System.Drawing.Color.White;
            this.tChart_qm.ContextMenuStrip = this.chart_cms_qm_click;
            this.tChart_qm.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // 
            // 
            this.tChart_qm.Header.Lines = new string[] {
        "气密检测"};
            this.tChart_qm.Location = new System.Drawing.Point(6, 20);
            this.tChart_qm.Name = "tChart_qm";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart_qm.Panel.Brush.Color = System.Drawing.Color.White;
            // 
            // 
            // 
            this.tChart_qm.Panel.Brush.Gradient.Visible = false;
            this.tChart_qm.Panel.MarginLeft = 0D;
            this.tChart_qm.Panel.MarginRight = 2D;
            this.tChart_qm.Panel.MarginTop = 0D;
            this.tChart_qm.Series.Add(this.qm_Line);
            this.tChart_qm.Size = new System.Drawing.Size(817, 480);
            this.tChart_qm.TabIndex = 18;
            this.tChart_qm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tChart1_MouseDown);
            // 
            // chart_cms_qm_click
            // 
            this.chart_cms_qm_click.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.chart_cms_qm_click.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.export_image_qm});
            this.chart_cms_qm_click.Name = "contextMenuStrip1";
            this.chart_cms_qm_click.Size = new System.Drawing.Size(125, 26);
            // 
            // export_image_qm
            // 
            this.export_image_qm.Name = "export_image_qm";
            this.export_image_qm.Size = new System.Drawing.Size(124, 22);
            this.export_image_qm.Text = "导出图片";
            this.export_image_qm.Click += new System.EventHandler(this.export_image_qm_Click);
            // 
            // qm_Line
            // 
            this.qm_Line.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.qm_Line.ColorEach = false;
            this.qm_Line.Cursor = System.Windows.Forms.Cursors.Cross;
            // 
            // 
            // 
            this.qm_Line.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            // 
            // 
            // 
            // 
            // 
            // 
            this.qm_Line.Marks.Callout.ArrowHead = Steema.TeeChart.Styles.ArrowHeadStyles.None;
            this.qm_Line.Marks.Callout.ArrowHeadSize = 8;
            // 
            // 
            // 
            this.qm_Line.Marks.Callout.Brush.Color = System.Drawing.Color.Black;
            this.qm_Line.Marks.Callout.Distance = 0;
            this.qm_Line.Marks.Callout.Draw3D = false;
            this.qm_Line.Marks.Callout.Length = 10;
            this.qm_Line.Marks.Callout.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            this.qm_Line.ShowInLegend = false;
            this.qm_Line.Title = "fastLine1";
            this.qm_Line.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.Ignore;
            // 
            // 
            // 
            this.qm_Line.XValues.DataMember = "X";
            this.qm_Line.XValues.DateTime = true;
            this.qm_Line.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.qm_Line.YValues.DataMember = "Y";
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(1052, 538);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(75, 33);
            this.btn_exit.TabIndex = 19;
            this.btn_exit.Text = "退出";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_dqyl);
            this.groupBox1.Controls.Add(this.lbl_setYL);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lbl_title);
            this.groupBox1.Location = new System.Drawing.Point(9, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(837, 67);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "状态";
            // 
            // lbl_dqyl
            // 
            this.lbl_dqyl.AutoSize = true;
            this.lbl_dqyl.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_dqyl.Location = new System.Drawing.Point(673, 25);
            this.lbl_dqyl.Name = "lbl_dqyl";
            this.lbl_dqyl.Size = new System.Drawing.Size(17, 16);
            this.lbl_dqyl.TabIndex = 24;
            this.lbl_dqyl.Text = "0";
            // 
            // lbl_setYL
            // 
            this.lbl_setYL.AutoSize = true;
            this.lbl_setYL.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_setYL.Location = new System.Drawing.Point(672, 1);
            this.lbl_setYL.Name = "lbl_setYL";
            this.lbl_setYL.Size = new System.Drawing.Size(17, 16);
            this.lbl_setYL.TabIndex = 23;
            this.lbl_setYL.Text = "0";
            this.lbl_setYL.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(497, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 16);
            this.label7.TabIndex = 22;
            this.label7.Text = "当前压力（帕）：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(497, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 16);
            this.label6.TabIndex = 21;
            this.label6.Text = "设定压力（帕）：";
            this.label6.Visible = false;
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_title.Location = new System.Drawing.Point(77, 26);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(257, 16);
            this.lbl_title.TabIndex = 20;
            this.lbl_title.Text = "门窗气密性能检测  第0号 第0樘";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.流量原始数据);
            this.tabControl1.Controls.Add(this.重复流量数据);
            this.tabControl1.Location = new System.Drawing.Point(852, 7);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(289, 352);
            this.tabControl1.TabIndex = 16;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // 流量原始数据
            // 
            this.流量原始数据.Controls.Add(this.dgv_ll);
            this.流量原始数据.Location = new System.Drawing.Point(4, 22);
            this.流量原始数据.Name = "流量原始数据";
            this.流量原始数据.Padding = new System.Windows.Forms.Padding(3);
            this.流量原始数据.Size = new System.Drawing.Size(281, 326);
            this.流量原始数据.TabIndex = 1;
            this.流量原始数据.Text = "流量原始数据(m3/h)";
            this.流量原始数据.UseVisualStyleBackColor = true;
            // 
            // dgv_ll
            // 
            this.dgv_ll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ll.Location = new System.Drawing.Point(5, 3);
            this.dgv_ll.Name = "dgv_ll";
            this.dgv_ll.RowHeadersWidth = 62;
            this.dgv_ll.RowTemplate.Height = 23;
            this.dgv_ll.Size = new System.Drawing.Size(270, 320);
            this.dgv_ll.TabIndex = 0;
            // 
            // 重复流量数据
            // 
            this.重复流量数据.Controls.Add(this.dgv_ll2);
            this.重复流量数据.Location = new System.Drawing.Point(4, 22);
            this.重复流量数据.Name = "重复流量数据";
            this.重复流量数据.Size = new System.Drawing.Size(281, 326);
            this.重复流量数据.TabIndex = 2;
            this.重复流量数据.Text = "重复流量原始数据(m3/h)";
            this.重复流量数据.UseVisualStyleBackColor = true;
            // 
            // dgv_ll2
            // 
            this.dgv_ll2.AllowUserToResizeColumns = false;
            this.dgv_ll2.AllowUserToResizeRows = false;
            this.dgv_ll2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ll2.Location = new System.Drawing.Point(5, 3);
            this.dgv_ll2.Name = "dgv_ll2";
            this.dgv_ll2.RowHeadersWidth = 62;
            this.dgv_ll2.RowTemplate.Height = 23;
            this.dgv_ll2.Size = new System.Drawing.Size(270, 320);
            this.dgv_ll2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdb_zdstl);
            this.groupBox3.Controls.Add(this.btn_losestart);
            this.groupBox3.Controls.Add(this.btn_juststart);
            this.groupBox3.Controls.Add(this.btn_loseready);
            this.groupBox3.Controls.Add(this.btn_justready);
            this.groupBox3.Controls.Add(this.rdb_fjstl);
            this.groupBox3.Location = new System.Drawing.Point(853, 362);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(284, 93);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "监控";
            // 
            // rdb_zdstl
            // 
            this.rdb_zdstl.AutoSize = true;
            this.rdb_zdstl.Location = new System.Drawing.Point(160, 13);
            this.rdb_zdstl.Name = "rdb_zdstl";
            this.rdb_zdstl.Size = new System.Drawing.Size(83, 16);
            this.rdb_zdstl.TabIndex = 0;
            this.rdb_zdstl.Tag = "stl";
            this.rdb_zdstl.Text = "总的渗透量";
            this.rdb_zdstl.UseVisualStyleBackColor = true;
            // 
            // btn_losestart
            // 
            this.btn_losestart.Location = new System.Drawing.Point(148, 61);
            this.btn_losestart.Name = "btn_losestart";
            this.btn_losestart.Size = new System.Drawing.Size(95, 28);
            this.btn_losestart.TabIndex = 4;
            this.btn_losestart.Text = "负压开始";
            this.btn_losestart.UseVisualStyleBackColor = true;
            this.btn_losestart.Click += new System.EventHandler(this.btn_losestart_Click);
            // 
            // btn_juststart
            // 
            this.btn_juststart.Location = new System.Drawing.Point(38, 62);
            this.btn_juststart.Name = "btn_juststart";
            this.btn_juststart.Size = new System.Drawing.Size(95, 27);
            this.btn_juststart.TabIndex = 4;
            this.btn_juststart.Text = "正压开始";
            this.btn_juststart.UseVisualStyleBackColor = true;
            this.btn_juststart.Click += new System.EventHandler(this.btn_juststart_Click);
            // 
            // btn_loseready
            // 
            this.btn_loseready.Location = new System.Drawing.Point(148, 32);
            this.btn_loseready.Name = "btn_loseready";
            this.btn_loseready.Size = new System.Drawing.Size(95, 25);
            this.btn_loseready.TabIndex = 4;
            this.btn_loseready.Text = "负压预备";
            this.btn_loseready.UseVisualStyleBackColor = true;
            this.btn_loseready.Click += new System.EventHandler(this.btn_loseready_Click);
            // 
            // btn_justready
            // 
            this.btn_justready.Location = new System.Drawing.Point(38, 33);
            this.btn_justready.Name = "btn_justready";
            this.btn_justready.Size = new System.Drawing.Size(95, 24);
            this.btn_justready.TabIndex = 4;
            this.btn_justready.Text = "正压预备";
            this.btn_justready.UseVisualStyleBackColor = true;
            this.btn_justready.Click += new System.EventHandler(this.btn_justready_Click);
            // 
            // rdb_fjstl
            // 
            this.rdb_fjstl.AutoSize = true;
            this.rdb_fjstl.Checked = true;
            this.rdb_fjstl.Location = new System.Drawing.Point(38, 14);
            this.rdb_fjstl.Name = "rdb_fjstl";
            this.rdb_fjstl.Size = new System.Drawing.Size(83, 16);
            this.rdb_fjstl.TabIndex = 0;
            this.rdb_fjstl.TabStop = true;
            this.rdb_fjstl.Tag = "stl";
            this.rdb_fjstl.Text = "附加渗透量";
            this.rdb_fjstl.UseVisualStyleBackColor = true;
            // 
            // btn_datadispose
            // 
            this.btn_datadispose.Location = new System.Drawing.Point(855, 538);
            this.btn_datadispose.Name = "btn_datadispose";
            this.btn_datadispose.Size = new System.Drawing.Size(82, 33);
            this.btn_datadispose.TabIndex = 14;
            this.btn_datadispose.Text = "数据处理";
            this.btn_datadispose.UseVisualStyleBackColor = true;
            this.btn_datadispose.Click += new System.EventHandler(this.btn_datadispose_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(943, 538);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(103, 33);
            this.btn_stop.TabIndex = 12;
            this.btn_stop.Text = "停止";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // tim_qm
            // 
            this.tim_qm.Enabled = true;
            this.tim_qm.Interval = 2000;
            this.tim_qm.Tick += new System.EventHandler(this.tim_qm_Tick);
            // 
            // tim_getType
            // 
            this.tim_getType.Enabled = true;
            this.tim_getType.Interval = 500;
            this.tim_getType.Tick += new System.EventHandler(this.tim_getType_Tick);
            // 
            // tim_Top10
            // 
            this.tim_Top10.Interval = 1000;
            this.tim_Top10.Tick += new System.EventHandler(this.tim_Top10_Tick);
            // 
            // gv_list
            // 
            this.gv_list.Interval = 1000;
            this.gv_list.Tick += new System.EventHandler(this.gv_list_Tick);
            // 
            // tim_PainPic
            // 
            this.tim_PainPic.Enabled = true;
            this.tim_PainPic.Interval = 1000;
            this.tim_PainPic.Tick += new System.EventHandler(this.tim_PainPic_Tick);
            // 
            // AirtightDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 713);
            this.Controls.Add(this.tc_RealTimeSurveillance);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AirtightDetection";
            this.Text = "RealTimeSurveillance";
            this.tc_RealTimeSurveillance.ResumeLayout(false);
            this.page_airtight.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_levelIndex)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.chart_cms_qm_click.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.流量原始数据.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ll)).EndInit();
            this.重复流量数据.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ll2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tc_RealTimeSurveillance;
        private System.Windows.Forms.TabPage page_airtight;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdb_zdstl;
        private System.Windows.Forms.Button btn_losestart;
        private System.Windows.Forms.Button btn_juststart;
        private System.Windows.Forms.Button btn_loseready;
        private System.Windows.Forms.Button btn_justready;
        private System.Windows.Forms.RadioButton rdb_fjstl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_datadispose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 流量原始数据;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_title;
        private Steema.TeeChart.TChart tChart_qm;
        private System.Windows.Forms.DataGridView dgv_ll2;
        private System.Windows.Forms.DataGridView dgv_levelIndex;
        private System.Windows.Forms.DataGridView dgv_ll;
        private Steema.TeeChart.Styles.FastLine qm_Line;
        private System.Windows.Forms.ContextMenuStrip chart_cms_qm_click;
        private System.Windows.Forms.ToolStripMenuItem export_image_qm;
        private System.Windows.Forms.Label lbl_dqyl;
        private System.Windows.Forms.Timer tim_Top10;
        private System.Windows.Forms.Timer gv_list;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Timer tim_qm;
        private System.Windows.Forms.Timer tim_getType;
        private System.Windows.Forms.Timer tim_PainPic;
        private System.Windows.Forms.Label lbl_setYL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_ycjy_z;
        private System.Windows.Forms.TextBox txt_ycjy_z;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_ycjyf;
        private System.Windows.Forms.TextBox txt_ycjy_f;
        private System.Windows.Forms.TabPage 重复流量数据;
    }
}