using text.doors.Common;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Young.Core.SQLite;
using System.IO;

namespace text.doors
{
    public partial class Login : Form
    {
        public static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        public Login()
        {
            InitializeComponent();


            //var _tempCode = "20210114-4";
            //string _name = "建筑外窗（门）气密、水密、抗风压性能检测报告" + "_" + _tempCode + ".xls";

            //var saveExcelUrl = "E:\\" + _name;

            //ExportExcel exportExcel = new ExportExcel(_tempCode);
            //var res = exportExcel.ExportData(saveExcelUrl);

            Init();
        }

        private string administrator = "administrator";
        private void Init()
        {
            var file= System.Environment.CurrentDirectory+ "/tempImage";
            var path = Path.GetFullPath(file);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //WindowsIdentity identity = WindowsIdentity.GetCurrent();
            //WindowsPrincipal principal = new WindowsPrincipal(identity);
            //if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            //{
            txt_userID.Text = administrator;
            this.txt_passWord.Focus();
            //}
            //else
            //{
            //    lbl_LoginType.Text = "请使用管理员身份登录！";
            //    btn_login.Enabled = false;
            //}
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            if (txt_passWord.Text == "")
            {
                lbl_LoginType.Text = "密码不能为空";
                return;
            }

            string sql = "select * from  User where User_Name='" + administrator + "'";
            DataTable dt = SQLiteHelper.ExecuteDataRow(sql).Table;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("账户出现问题，请联系管理员", "账户", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                Logger.Error("登录:未发现" + administrator + "账户");
                return;
            }


            if (!string.IsNullOrWhiteSpace(dt.Rows[0]["User_PassWord"].ToString()))
            {
                if (txt_passWord.Text == dt.Rows[0]["User_PassWord"].ToString())
                {
                    this.DialogResult = DialogResult.OK;//关键:设置登陆成功状态  
                    this.Close();
                }
                else
                {
                    lbl_LoginType.Text = "密码输入错误！";
                    return;
                }
            }
        }
    }
}
