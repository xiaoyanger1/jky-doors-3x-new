using text.doors.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Young.Core.Common;
using Young.Core.SQLite;
using System.Xml;
using System.IO.Ports;

namespace text.doors.Detection
{
    public partial class SystemManager : Form
    {
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        public SystemManager()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //检查是否含有串口
            string[] str = SerialPort.GetPortNames();
            if (str == null)
            {
                MessageUtil.ShowError("本机没有串口!");
            }

            // 串口
            foreach (string s in str)
            {
                this.cb_ProtName.Items.Add(s);
            }

            this.cb_ProtName.Text = GetConfigSetting("ProtName");
            this.txt_BaudRate.Text = GetConfigSetting("BaudRate");
            this.txt_DataBits.Text = GetConfigSetting("DataBits");
            this.txt_Parity.Text = GetConfigSetting("Parity");
            this.txt_StopBits.Text = GetConfigSetting("StopBits");
            this.txt_PipeDiameter.Text = GetConfigSetting("PipeDiameter");
        }


        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveConfig(this.cb_ProtName.Text, "ProtName");
            SaveConfig(this.txt_BaudRate.Text, "BaudRate");
            SaveConfig(this.txt_DataBits.Text, "DataBits");
            SaveConfig(this.txt_Parity.Text, "Parity");
            SaveConfig(this.txt_StopBits.Text, "StopBits");
            SaveConfig(this.txt_PipeDiameter.Text, "PipeDiameter");


            MessageBox.Show("保存成功,请重启软件", "风机", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //Application.Exit();
            //System.Environment.Exit(0);
            this.Dispose();
            // 加入想要的逻辑处理
            System.Environment.Exit(0);
        }

        private string GetConfigSetting(string value)
        {
            return System.Configuration.ConfigurationSettings.AppSettings[value].ToString();
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

            //XmlDocument doc = new XmlDocument();
            ////获得配置文件的全路径  
            //string strFileName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            //// string  strFileName= AppDomain.CurrentDomain.BaseDirectory + "\\exe.config";  
            //doc.Load(strFileName);
            ////找出名称为“add”的所有元素  
            //XmlNodeList nodes = doc.GetElementsByTagName("add");
            //for (int i = 0; i < nodes.Count; i++)
            //{
            //    //获得将当前元素的key属性  
            //    XmlAttribute att = nodes[i].Attributes["key"];
            //    //根据元素的第一个属性来判断当前的元素是不是目标元素  
            //    if (att.Value == key)
            //    {
            //        //对目标元素中的第二个属性赋值  
            //        att = nodes[i].Attributes["value"];
            //        att.Value = value;
            //        break;
            //    }
            //}
            ////保存上面的修改  
            //doc.Save(strFileName);
            //System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
