using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Default;

namespace text.doors.Detection
{
    public partial class CorrectionFactor : Form
    {
        public CorrectionFactor()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.txtzy.Text = DefaultBase.Z_Factor;
            this.txtfy.Text = DefaultBase.F_Factor;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {


            int zFactor = 0;
            int.TryParse(txtzy.Text, out zFactor);

            int fFactor = 0;
            int.TryParse(txtfy.Text, out fFactor);


            SaveConfig(this.txtzy.Text, "Z_Factor");
            SaveConfig(this.txtfy.Text, "F_Factor");


            MessageBox.Show("保存成功,请重启软件", "风系数设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.Dispose();
            // 加入想要的逻辑处理
            System.Environment.Exit(0);
        }

        private void SaveConfig(string value, string key)
        {

            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");

            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            System.Xml.XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");

            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", value);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", key);
                xElem2.SetAttribute("value", value);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
