namespace text.doors.Detection
{
    partial class SystemManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemManager));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_ProtName = new System.Windows.Forms.ComboBox();
            this.txt_Parity = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_StopBits = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_DataBits = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.txt_PipeDiameter = new System.Windows.Forms.TextBox();
            this.txt_BaudRate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_ProtName);
            this.groupBox1.Controls.Add(this.txt_Parity);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txt_StopBits);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txt_DataBits);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btn_save);
            this.groupBox1.Controls.Add(this.txt_PipeDiameter);
            this.groupBox1.Controls.Add(this.txt_BaudRate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(27, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 253);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统设置";
            // 
            // cb_ProtName
            // 
            this.cb_ProtName.FormattingEnabled = true;
            this.cb_ProtName.Location = new System.Drawing.Point(121, 20);
            this.cb_ProtName.Margin = new System.Windows.Forms.Padding(2);
            this.cb_ProtName.Name = "cb_ProtName";
            this.cb_ProtName.Size = new System.Drawing.Size(101, 20);
            this.cb_ProtName.TabIndex = 185;
            // 
            // txt_Parity
            // 
            this.txt_Parity.Enabled = false;
            this.txt_Parity.Location = new System.Drawing.Point(121, 130);
            this.txt_Parity.Name = "txt_Parity";
            this.txt_Parity.Size = new System.Drawing.Size(100, 21);
            this.txt_Parity.TabIndex = 8;
            this.txt_Parity.Text = "Even";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(61, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "校验位：";
            // 
            // txt_StopBits
            // 
            this.txt_StopBits.Enabled = false;
            this.txt_StopBits.Location = new System.Drawing.Point(121, 106);
            this.txt_StopBits.Name = "txt_StopBits";
            this.txt_StopBits.Size = new System.Drawing.Size(100, 21);
            this.txt_StopBits.TabIndex = 6;
            this.txt_StopBits.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(61, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "停止位：";
            // 
            // txt_DataBits
            // 
            this.txt_DataBits.Enabled = false;
            this.txt_DataBits.Location = new System.Drawing.Point(121, 79);
            this.txt_DataBits.Name = "txt_DataBits";
            this.txt_DataBits.Size = new System.Drawing.Size(100, 21);
            this.txt_DataBits.TabIndex = 4;
            this.txt_DataBits.Text = "7";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "数据位：";
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(121, 204);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 2;
            this.btn_save.Text = "保存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // txt_PipeDiameter
            // 
            this.txt_PipeDiameter.Location = new System.Drawing.Point(121, 157);
            this.txt_PipeDiameter.Name = "txt_PipeDiameter";
            this.txt_PipeDiameter.Size = new System.Drawing.Size(100, 21);
            this.txt_PipeDiameter.TabIndex = 1;
            this.txt_PipeDiameter.Text = "0.08";
            // 
            // txt_BaudRate
            // 
            this.txt_BaudRate.Enabled = false;
            this.txt_BaudRate.Location = new System.Drawing.Point(120, 48);
            this.txt_BaudRate.Name = "txt_BaudRate";
            this.txt_BaudRate.Size = new System.Drawing.Size(100, 21);
            this.txt_BaudRate.TabIndex = 1;
            this.txt_BaudRate.Text = "9600";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "集流管直径：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "波特率：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "串口号：";
            // 
            // SystemManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 291);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SystemManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统设置";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.TextBox txt_PipeDiameter;
        private System.Windows.Forms.TextBox txt_BaudRate;
        private System.Windows.Forms.TextBox txt_DataBits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_Parity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_StopBits;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.ComboBox cb_ProtName;
    }
}