
using text.doors.Common;
using text.doors.Detection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Controls;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using Young.Core.Common;
using text.doors.Default;
using text.doors.Model;

namespace text.doors
{
    public partial class MainForm : Form
    {
        private static SerialPortClient _serialPortClient = new SerialPortClient();
        public static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();


        /// <summary>
        /// 当前温度
        /// </summary>
        private double _temperature = 0;
        /// <summary>
        /// 当前压力
        /// </summary>
        private double _temppressure = 0;

        /// <summary>
        /// 检验编号
        /// </summary>
        private string _tempCode = "";
        /// <summary>
        /// 当前樘号
        /// </summary>
        private string _tempTong = "";

        public static Thread bindLableThread;


        public static System.Timers.Timer register_tim;



        public static System.Timers.Timer bind_tim;

        public MainForm()
        {

            InitializeComponent();
            OpenSerialPortClient();

            ShowDetectionSet();
        }

        private void tim_getdate_Tick(object sender, EventArgs e)
        {
            if (_serialPortClient.sp.IsOpen)
            {
              
                _temperature = _serialPortClient.GetWDXS();
                _temppressure = _serialPortClient.GetDQYLXS();


                lbl_wdcgq.Text = _temperature.ToString();
                lbl_dqylcgq.Text = _temppressure.ToString();
                lbl_fscgq.Text = _serialPortClient.GetFSXS().ToString();
                lbl_cygcgq.Text = _serialPortClient.GetCY_High().ToString();
                lbl_cydcgq.Text = _serialPortClient.GetCY_Low().ToString();
            }
        }

    
        private void OpenSerialPortClient()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    using (BackgroundWorker bw = new BackgroundWorker())
                    {
                        bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SerialPort_RunWorkerCompleted);
                        bw.DoWork += new DoWorkEventHandler(SerialPort_DoWork);
                        bw.RunWorkerAsync();
                    }
                    if (!_serialPortClient.sp.IsOpen)
                    {
                        _serialPortClient.SerialPortOpen();
                    }

                    Thread.Sleep(500);
                }
            }));
            thread.Start();
        }

        void SerialPort_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = _serialPortClient.sp.IsOpen ? "串口连接：成功" : "串口连接：失败";
        }

        void SerialPort_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.tsl_tcpclient.Text = e.Result.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


        private void SelectDangHao(text.doors.Detection.DetectionSet.BottomType bt)
        {
            this.tssl_SetCode.Text = string.Format("{0}  {1}", bt.Code, bt.Tong);
            if (bt.ISOK == true)
            { this.tsl_type.Visible = false; }
            else { this.tsl_type.Visible = true; }

            _tempCode = bt.Code;
            _tempTong = bt.Tong;
            DefaultBase.IsSetTong = bt.ISOK;
            if (bt.ISOK)
            {
                ShowAirtightDetection();
            }
        }

        /// <summary>
        /// 参数设置
        /// </summary>
        private void ShowDetectionSet()
        {
            if (pl_showItem.Controls.Count > 0)
            {
                foreach (Control con in this.pl_showItem.Controls)
                {
                    if (con is DetectionSet)
                    {
                    }
                    else if (con is WindPressureDetection)
                    {
                        con.Dispose();
                        //WindPressureDetection.releaseThread();
                        ((Form)con).Close();
                    }
                    else
                    {
                        ((Form)con).Close();
                    }
                }

            }

            this.pl_showItem.Controls.Clear();
            DetectionSet ds = new DetectionSet(_serialPortClient, _temperature, _temppressure, _tempCode, _tempTong);
            ds.deleBottomTypeEvent += new DetectionSet.deleBottomType(SelectDangHao);
            ds.GetDangHaoTrigger();
            ds.TopLevel = false;
            ds.Parent = this.pl_showItem;
            ds.Show();
        }

        /// <summary>
        /// 水密监控
        /// </summary>
        private void ShowWatertightDetection()
        {
            if (pl_showItem.Controls.Count > 0)
            {
                foreach (Control con in this.pl_showItem.Controls)
                {
                    if (con is DetectionSet)
                    {
                    }
                    else if (con is WindPressureDetection)
                    {
                        //WindPressureDetection.releaseThread();
                        con.Dispose();
                        ((Form)con).Close();
                    }
                    else
                    {
                        ((Form)con).Close();
                    }
                }
            }
            this.pl_showItem.Controls.Clear();
            WatertightDetection rts = new WatertightDetection(_serialPortClient, _tempCode, _tempTong);

            rts.TopLevel = false;
            rts.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            rts.Parent = this.pl_showItem;
            rts.Show();
        }
        /// <summary>
        /// 气密监控
        /// </summary>
        private void ShowAirtightDetection()
        {
            if (pl_showItem.Controls.Count > 0)
            {
                foreach (Control con in this.pl_showItem.Controls)
                {
                    if (con is DetectionSet)
                    {
                    }
                    else if (con is WindPressureDetection)
                    {
                        con.Dispose();
                        ((Form)con).Close();
                    }
                    else
                    {
                        ((Form)con).Close();
                    }
                }
            }
            this.pl_showItem.Controls.Clear();

            AirtightDetection rts = new AirtightDetection(_serialPortClient, _tempCode, _tempTong);
            rts.TopLevel = false;
            rts.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            rts.Parent = this.pl_showItem;

            rts.Show();

        }
        /// <summary>
        /// 抗风压
        /// </summary>
        private void ShowWindPressure()
        {
            if (pl_showItem.Controls.Count > 0)
            {
                foreach (Control con in this.pl_showItem.Controls)
                {
                    if (con is DetectionSet)
                    {
                    }
                    else if (con is WindPressureDetection)
                    {
                        con.Dispose();
                        ((Form)con).Close();
                    }
                    else
                    {
                        ((Form)con).Close();
                    }
                }
            }
            this.pl_showItem.Controls.Clear();

            WindPressureDetection rts = new WindPressureDetection(_serialPortClient, _tempCode, _tempTong);
            this.pl_showItem.Controls.Clear();
            rts.TopLevel = false;
            rts.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            rts.Parent = this.pl_showItem;
            rts.Show();
        }

        private void hsb_WindControl_Scroll(object sender, ScrollEventArgs e)
        {
            if (hsb_WindControl.Value == 0)
                txt_hz.Text = "0.00";
            else
                txt_hz.Text = (hsb_WindControl.Value).ToString();

            double value = (hsb_WindControl.Value) * 640;

            var res = _serialPortClient.SendFJKZ(value);

            if (!res)
            {
                MessageBox.Show("风机控制异常,请确认服务器连接是否成功!", "风机", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //关闭
        private void tsm_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //收起
        private void tsb_fewer_Click(object sender, EventArgs e)
        {
            this.pl_set.Visible = false;
            this.tsb_fewer.Visible = false;
            this.tsb_open.Visible = true;
        }

        //打开
        private void tsb_open_Click(object sender, EventArgs e)
        {
            this.pl_set.Visible = true;
            this.tsb_open.Visible = false;
            this.tsb_fewer.Visible = true;
        }


        private void tsm_surveillance_Click(object sender, EventArgs e)
        {

        }

        //检测设定
        private void tsb_DetectionSet_Click(object sender, EventArgs e)
        {
            DefaultBase.IsSetTong = false;
            ShowDetectionSet();
        }

        private void tms_DetectionSet_Click(object sender, EventArgs e)
        {
            ShowDetectionSet();
        }

        private void tsm_UpdatePassWord_Click(object sender, EventArgs e)
        {
            UpdatePassWord up = new UpdatePassWord();
            up.Show();
            up.TopMost = true;
        }


        /// <summary>解决关闭按钮bug
        /// 
        /// </summary>
        /// <param name="msg"></param>
        protected override void WndProc(ref Message msg)
        {
            try
            {
                const int WM_SYSCOMMAND = 0x0112;
                const int SC_CLOSE = 0xF060;
                if (msg.Msg == WM_SYSCOMMAND && ((int)msg.WParam == SC_CLOSE))
                {
                    // 点击winform右上关闭按钮 
                    this.Dispose();
                    // 加入想要的逻辑处理
                    System.Environment.Exit(0);
                    return;
                }
                base.WndProc(ref msg);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


        private void tsb_生成报告_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
            {
                try
                {
                    FolderBrowserDialog path = new FolderBrowserDialog();
                    path.ShowDialog();

                    if (string.IsNullOrWhiteSpace(path.SelectedPath))
                    {
                        return;
                    }
                    string _name = "建筑外窗（门）气密、水密、抗风压性能检测报告" + "_" + _tempCode + ".xls";

                    var saveExcelUrl = path.SelectedPath + "\\" + _name;
                    ExportExcel exportExcel = new ExportExcel(_tempCode);
                    var res = exportExcel.ExportData(saveExcelUrl);
                    if (res)
                    {
                        MessageBox.Show("导出成功", "导出成功", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 高压归零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private bool _GaoYaGuiLing = false;
        private void btn_gyZero_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.SendGYBD(ref _GaoYaGuiLing))
            {
                MessageBox.Show("高压归零异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            btn_gyZero.BackColor = _GaoYaGuiLing ? Color.Green : Color.Transparent;
        }

        /// <summary>
        /// 低压归零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private bool _DiyaGuiLing = false;
        private void btn_dyZero_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.SendDYBD(ref _DiyaGuiLing))
            {
                MessageBox.Show("低压归零异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            btn_dyZero.BackColor = _DiyaGuiLing ? Color.Green : Color.Transparent;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.Show();
            a.TopMost = true;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (!DefaultBase.IsSetTong)
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            ComplexAssessment ca = new ComplexAssessment(_tempCode);
            if (DefaultBase.IsOpenComplexAssessment)
            {
                ca.Show();
                ca.TopMost = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!DefaultBase.IsSetTong)
            {
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            TakePhotos takePhotos = new TakePhotos(_tempCode);
            takePhotos.Show();
            takePhotos.TopMost = true;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SystemManager sm = new SystemManager();
            sm.Show();
            sm.TopMost = true;
        }

        private void 水密监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
                ShowWatertightDetection();
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void 气密监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
                ShowAirtightDetection();
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void 抗风压监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
            {
                ShowWindPressure();
            }
            else
            {
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsb_watertight_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
                ShowAirtightDetection();
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void tsbwatertight_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
                ShowWatertightDetection();
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void tsb_WindPressure_Click(object sender, EventArgs e)
        {
            if (DefaultBase.IsSetTong)
            {
                ShowWindPressure();
            }
            else
                MessageBox.Show("请先检测设定", "检测", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        void hsb_DoWork(object sender, DoWorkEventArgs e)
        {
            var IsSeccess = false;
            var diffPress = _serialPortClient.ReadFJSD(ref IsSeccess);
            if (!IsSeccess) return;

            //需要计算
            e.Result = diffPress;
        }

        void hsb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
                return;
            var value = int.Parse(e.Result.ToString()) / 640;
            txt_hz.Text = value.ToString();
        }


        private void pID设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }
        private void 系数设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void tim_panelValue_Tick(object sender, EventArgs e)
        {
            if (_serialPortClient.sp.IsOpen)
            {
                using (BackgroundWorker bw = new BackgroundWorker())
                {
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(hsb_RunWorkerCompleted);
                    bw.DoWork += new DoWorkEventHandler(hsb_DoWork);
                    bw.RunWorkerAsync();
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_kglkz_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.Sendkglkz())
            {
                MessageBox.Show("开关量控制异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private bool _FengJiQiDongStat = false;

        private void btn_fjqd_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendFengJiQiDong(ref _FengJiQiDongStat);
            if (!res)
            {
                MessageBox.Show("风机启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btn_fjqd.BackColor = _FengJiQiDongStat ? Color.Green : Color.Transparent;
        }
        private bool _ShuiBengQiDong = false;
        private void btn_sbqd_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendShuiBengQiDong(ref _ShuiBengQiDong);
            if (!res)
            {
                MessageBox.Show("水泵启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_sbqd.BackColor = _ShuiBengQiDong ? Color.Green : Color.Transparent;
        }

        private bool _BaoHuFaTong = false;
        private void btn_bhft_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendBaoHuFaTong(ref _BaoHuFaTong);
            if (!res)
            {
                MessageBox.Show("保护阀通启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_bhft.BackColor = _BaoHuFaTong ? Color.Green : Color.Transparent;
        }
        private bool _SiTongFaKai = false;
        private void btn_stfk_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendSiTongFaKai(ref _SiTongFaKai);
            if (!res)
            {
                MessageBox.Show("四通阀开异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_stfk.BackColor = _SiTongFaKai ? Color.Green : Color.Transparent;
        }


        private void btn_ddk_MouseUp(object sender, MouseEventArgs e)
        {

            var res = _serialPortClient.SendDianDongKai(false);
            if (!res)
            {
                MessageBox.Show("点动开异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_ddk.BackColor = Color.Transparent;

        }
        private void btn_ddk_MouseDown(object sender, MouseEventArgs e)
        {

            var res = _serialPortClient.SendDianDongKai(true);
            if (!res)
            {
                MessageBox.Show("点动开异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_ddk.BackColor = Color.Green;
        }

        private void btn_ddg_MouseDown(object sender, MouseEventArgs e)
        {
            var res = _serialPortClient.SendDianDongGuan(true);
            if (!res)
            {
                MessageBox.Show("水泵启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_ddg.BackColor = Color.Green;
        }

        private void btn_ddg_MouseUp(object sender, MouseEventArgs e)
        {
            var res = _serialPortClient.SendDianDongGuan(false);
            if (!res)
            {
                MessageBox.Show("水泵启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_ddg.BackColor = Color.Transparent;
        }

        private bool _GuanDaoTou = false;
        private void btn_gdt_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendGuanDaoTou(ref _GuanDaoTou);
            if (!res)
            {
                MessageBox.Show("关到头异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_gdt.BackColor = _GuanDaoTou ? Color.Green : Color.Transparent;
        }

        private void 传感器设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SensorSet ss = new SensorSet(_serialPortClient);
            ss.Show();
            ss.TopMost = true;
        }

        private void 系数设定ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CorrectionFactor correctionFactor = new CorrectionFactor();
            correctionFactor.Show();
        }

        private void pID设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PIDManager p = new PIDManager(_serialPortClient);
            p.Show();
        }
    }
}
