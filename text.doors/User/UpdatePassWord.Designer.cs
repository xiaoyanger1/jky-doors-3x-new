﻿namespace text.doors
{
    partial class UpdatePassWord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdatePassWord));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_oldPassWord = new System.Windows.Forms.TextBox();
            this.txt_NewPassWord = new System.Windows.Forms.TextBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "原始密码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 98);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "新密码：";
            // 
            // txt_oldPassWord
            // 
            this.txt_oldPassWord.Location = new System.Drawing.Point(186, 39);
            this.txt_oldPassWord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_oldPassWord.Name = "txt_oldPassWord";
            this.txt_oldPassWord.PasswordChar = '*';
            this.txt_oldPassWord.Size = new System.Drawing.Size(148, 28);
            this.txt_oldPassWord.TabIndex = 2;
            // 
            // txt_NewPassWord
            // 
            this.txt_NewPassWord.Location = new System.Drawing.Point(186, 93);
            this.txt_NewPassWord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_NewPassWord.Name = "txt_NewPassWord";
            this.txt_NewPassWord.PasswordChar = '*';
            this.txt_NewPassWord.Size = new System.Drawing.Size(148, 28);
            this.txt_NewPassWord.TabIndex = 3;
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(224, 156);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(112, 34);
            this.btn_save.TabIndex = 4;
            this.btn_save.Text = "确定";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // UpdatePassWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 213);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.txt_NewPassWord);
            this.Controls.Add(this.txt_oldPassWord);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdatePassWord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改密码";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_oldPassWord;
        private System.Windows.Forms.TextBox txt_NewPassWord;
        private System.Windows.Forms.Button btn_save;
    }
}