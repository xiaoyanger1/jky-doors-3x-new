namespace text.doors.Detection
{
    partial class CorrectionFactor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CorrectionFactor));
            this.btnsave = new System.Windows.Forms.Button();
            this.txtfy = new System.Windows.Forms.TextBox();
            this.txtzy = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnsave
            // 
            this.btnsave.Location = new System.Drawing.Point(216, 173);
            this.btnsave.Margin = new System.Windows.Forms.Padding(4);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(112, 34);
            this.btnsave.TabIndex = 22;
            this.btnsave.Text = "保存";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // txtfy
            // 
            this.txtfy.Location = new System.Drawing.Point(216, 101);
            this.txtfy.Margin = new System.Windows.Forms.Padding(4);
            this.txtfy.Name = "txtfy";
            this.txtfy.Size = new System.Drawing.Size(260, 28);
            this.txtfy.TabIndex = 20;
            // 
            // txtzy
            // 
            this.txtzy.Location = new System.Drawing.Point(216, 44);
            this.txtzy.Margin = new System.Windows.Forms.Padding(4);
            this.txtzy.Name = "txtzy";
            this.txtzy.Size = new System.Drawing.Size(260, 28);
            this.txtzy.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 104);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 18);
            this.label2.TabIndex = 18;
            this.label2.Text = "负压系数修正值：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 18);
            this.label1.TabIndex = 17;
            this.label1.Text = "正压系数修正值：";
            // 
            // CorrectionFactor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 236);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.txtfy);
            this.Controls.Add(this.txtzy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CorrectionFactor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系数修正";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtfy;
        private System.Windows.Forms.TextBox txtzy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}