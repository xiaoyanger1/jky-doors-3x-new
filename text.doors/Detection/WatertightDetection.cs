
using Modbus.Device;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using text.doors.Common;
using text.doors.dal;
using text.doors.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Young.Core.Common;
using text.doors.Model.DataBase;
using text.doors.Default;
using text.doors.Service;
using static text.doors.Default.PublicEnum;
using NPOI.SS.Formula.Functions;

namespace text.doors.Detection
{
    public partial class WatertightDetection : Form
    {
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        private SerialPortClient _serialPortClient;
        //检验编号
        private string _tempCode = "";
        //当前樘号
        private string _tempTong = "";


        /// <summary>
        /// 水密按钮位置
        /// </summary>
        private PublicEnum.WaterTightPropertyTest? waterTightPropertyTest = null;

        /// <summary>
        /// 操作状态
        /// </summary>
        private bool IsSeccess = false;


        public DateTime dtnow { get; set; }
        public WatertightDetection()
        {

        }

        public WatertightDetection(SerialPortClient serialPortClient, string tempCode, string tempTong)
        {
            InitializeComponent();
            this._serialPortClient = serialPortClient;
            this._tempCode = tempCode;
            this._tempTong = tempTong;
            Init();

        }

        private void Init()
        {
            if (this.tabControl1.SelectedTab.Name == "水密")
            {
                Initial(QM_TestCount.第一次);
            }
            else if (this.tabControl1.SelectedTab.Name == "重复水密")
            {
                Initial(QM_TestCount.第二次);
            }

            Title();
            SMchartInit();
        }
        #region 数据绑定

        /// <summary>
        /// 绑定水密初始值
        /// </summary>
        private void Initial(QM_TestCount qm_TestCount)
        {
            int type = (int)qm_TestCount;
            List<Model_dt_sm_Info> sm_Info = new DAL_dt_Settings().GetSMListByCode(_tempCode);
            List<Pressure> pressureList = new List<Pressure>();
            if (sm_Info != null && sm_Info.Count > 0)
            {
                var sm = sm_Info.FindAll(t => t.info_DangH == _tempTong && t.testcount == type);
                if (sm != null && sm.Count() > 0)
                {
                    #region 绑定
                    var checkDesc = sm[0].sm_PaDesc;
                    var sm_pa = sm[0].sm_Pa;
                    var remark = sm[0].sm_Remark;
                    var method = sm[0].Method;



                    if (method == "稳定加压")
                    {
                        this.rdb_wdjy.Checked = true;
                    }
                    else if (method == "波动加压")
                    {
                        this.rdb_bdjy.Checked = true;

                        txt_maxValue.Text = sm[0].sxyl;
                        txt_minValue.Text = sm[0].xxyl;
                    }
                    else
                    {
                        this.rdb_wdjy.Checked = true;
                    }

                    txt_ycjy.Text = sm[0].gongchengjiance;
                    var flish = "";
                    var two = "";
                    string[] temp = null;
                    if (!string.IsNullOrWhiteSpace(checkDesc))
                    {
                        if (checkDesc.Contains("〇"))
                        {
                            temp = checkDesc.Split(new char[] { '〇' }, StringSplitOptions.RemoveEmptyEntries);
                            flish = temp[0];
                            two = "〇" + temp[1];
                        }
                        else if (checkDesc.Contains("□"))
                        {
                            temp = checkDesc.Split(new char[] { '□' }, StringSplitOptions.RemoveEmptyEntries);
                            flish = temp[0];
                            two = "□" + temp[1];
                        }
                        else if (checkDesc.Contains("△"))
                        {
                            temp = checkDesc.Split(new char[] { '△' }, StringSplitOptions.RemoveEmptyEntries);
                            flish = temp[0];
                            two = "△" + temp[1];
                        }
                        else if (checkDesc.Contains("▲"))
                        {
                            temp = checkDesc.Split(new char[] { '▲' }, StringSplitOptions.RemoveEmptyEntries);
                            flish = temp[0];
                            two = "▲" + temp[1];
                        }
                        else if (checkDesc.Contains("●"))
                        {
                            temp = checkDesc.Split(new char[] { '●' }, StringSplitOptions.RemoveEmptyEntries);
                            flish = temp[0];
                            two = "●" + temp[1];
                        }
                        else
                        {
                            temp = checkDesc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            flish = temp[0];
                            two = temp[1];
                        }

                        if (qm_TestCount == QM_TestCount.第一次)
                        {
                            if (checkDesc.Contains("●") || checkDesc.Contains("▲"))
                            {
                                if (sm_pa == "0")
                                {
                                    cbb_1_100Pa.Text = flish;
                                    cbb_2_100Pa.Text = two;
                                }
                                if (sm_pa == "100")
                                {
                                    cbb_1_150Pa.Text = flish;
                                    cbb_2_150Pa.Text = two;
                                }
                                if (sm_pa == "150")
                                {
                                    cbb_1_200Pa.Text = flish;
                                    cbb_2_200Pa.Text = two;
                                }
                                if (sm_pa == "200")
                                {
                                    cbb_1_250Pa.Text = flish;
                                    cbb_2_250Pa.Text = two;
                                }
                                if (sm_pa == "250")
                                {
                                    cbb_1_300Pa.Text = flish;
                                    cbb_2_300Pa.Text = two;
                                }
                                if (sm_pa == "300")
                                {
                                    cbb_1_350Pa.Text = flish;
                                    cbb_2_350Pa.Text = two;
                                }
                                if (sm_pa == "350")
                                {
                                    cbb_1_400Pa.Text = flish;
                                    cbb_2_400Pa.Text = two;
                                }
                                if (sm_pa == "400")
                                {
                                    cbb_1_500Pa.Text = flish;
                                    cbb_2_500Pa.Text = two;
                                }
                                if (sm_pa == "500")
                                {
                                    cbb_1_600Pa.Text = flish;
                                    cbb_2_600Pa.Text = two;
                                }
                                if (sm_pa == "600")
                                {
                                    cbb_1_700Pa.Text = flish;
                                    cbb_2_700Pa.Text = two;
                                }
                                if (sm_pa == "700")
                                {
                                    cbb_1_700Pa.Text = flish;
                                    cbb_2_700Pa.Text = two;
                                }
                            }
                            else
                            {
                                if (sm_pa == "0")
                                {
                                    cbb_1_0Pa.Text = flish;
                                    cbb_2_0Pa.Text = two;
                                }
                                if (sm_pa == "100")
                                {
                                    cbb_1_100Pa.Text = flish;
                                    cbb_2_100Pa.Text = two;
                                }
                                if (sm_pa == "150")
                                {
                                    cbb_1_150Pa.Text = flish;
                                    cbb_2_150Pa.Text = two;
                                }
                                if (sm_pa == "200")
                                {
                                    cbb_1_200Pa.Text = flish;
                                    cbb_2_200Pa.Text = two;
                                }
                                if (sm_pa == "250")
                                {
                                    cbb_1_250Pa.Text = flish;
                                    cbb_2_250Pa.Text = two;
                                }
                                if (sm_pa == "300")
                                {
                                    cbb_1_300Pa.Text = flish;
                                    cbb_2_300Pa.Text = two;
                                }
                                if (sm_pa == "350")
                                {
                                    cbb_1_350Pa.Text = flish;
                                    cbb_2_350Pa.Text = two;
                                }
                                if (sm_pa == "400")
                                {
                                    cbb_1_400Pa.Text = flish;
                                    cbb_2_400Pa.Text = two;
                                }
                                if (sm_pa == "500")
                                {
                                    cbb_1_500Pa.Text = flish;
                                    cbb_2_500Pa.Text = two;
                                }
                                if (sm_pa == "600")
                                {
                                    cbb_1_600Pa.Text = flish;
                                    cbb_2_600Pa.Text = two;
                                }
                                if (sm_pa == "700")
                                {
                                    cbb_1_700Pa.Text = flish;
                                    cbb_2_700Pa.Text = two;
                                }
                            }
                            txt_zgfy.Text = sm_pa;
                            txt_desc.Text = remark;
                        }
                        else if (qm_TestCount == QM_TestCount.第二次)
                        {
                            if (checkDesc.Contains("●") || checkDesc.Contains("▲"))
                            {
                                if (sm_pa == "0")
                                {
                                    cbb_1_100Pa_cf.Text = flish;
                                    cbb_2_100Pa_cf.Text = two;
                                }
                                if (sm_pa == "100")
                                {
                                    cbb_1_150Pa_cf.Text = flish;
                                    cbb_2_150Pa_cf.Text = two;
                                }
                                if (sm_pa == "150")
                                {
                                    cbb_1_200Pa_cf.Text = flish;
                                    cbb_2_200Pa_cf.Text = two;
                                }
                                if (sm_pa == "200")
                                {
                                    cbb_1_250Pa_cf.Text = flish;
                                    cbb_2_250Pa_cf.Text = two;
                                }
                                if (sm_pa == "250")
                                {
                                    cbb_1_300Pa_cf.Text = flish;
                                    cbb_2_300Pa_cf.Text = two;
                                }
                                if (sm_pa == "300")
                                {
                                    cbb_1_350Pa_cf.Text = flish;
                                    cbb_2_350Pa_cf.Text = two;
                                }
                                if (sm_pa == "350")
                                {
                                    cbb_1_400Pa_cf.Text = flish;
                                    cbb_2_400Pa_cf.Text = two;
                                }
                                if (sm_pa == "400")
                                {
                                    cbb_1_500Pa_cf.Text = flish;
                                    cbb_2_500Pa_cf.Text = two;
                                }
                                if (sm_pa == "500")
                                {
                                    cbb_1_600Pa_cf.Text = flish;
                                    cbb_2_600Pa_cf.Text = two;
                                }
                                if (sm_pa == "600")
                                {
                                    cbb_1_700Pa_cf.Text = flish;
                                    cbb_2_700Pa_cf.Text = two;
                                }
                                if (sm_pa == "700")
                                {
                                    cbb_1_700Pa_cf.Text = flish;
                                    cbb_2_700Pa_cf.Text = two;
                                }
                            }
                            else
                            {
                                if (sm_pa == "0")
                                {
                                    cbb_1_0Pa_cf.Text = flish;
                                    cbb_2_0Pa_cf.Text = two;
                                }
                                if (sm_pa == "100")
                                {
                                    cbb_1_100Pa_cf.Text = flish;
                                    cbb_2_100Pa_cf.Text = two;
                                }
                                if (sm_pa == "150")
                                {
                                    cbb_1_150Pa_cf.Text = flish;
                                    cbb_2_150Pa_cf.Text = two;
                                }
                                if (sm_pa == "200")
                                {
                                    cbb_1_200Pa_cf.Text = flish;
                                    cbb_2_200Pa_cf.Text = two;
                                }
                                if (sm_pa == "250")
                                {
                                    cbb_1_250Pa_cf.Text = flish;
                                    cbb_2_250Pa_cf.Text = two;
                                }
                                if (sm_pa == "300")
                                {
                                    cbb_1_300Pa_cf.Text = flish;
                                    cbb_2_300Pa_cf.Text = two;
                                }
                                if (sm_pa == "350")
                                {
                                    cbb_1_350Pa_cf.Text = flish;
                                    cbb_2_350Pa_cf.Text = two;
                                }
                                if (sm_pa == "400")
                                {
                                    cbb_1_400Pa_cf.Text = flish;
                                    cbb_2_400Pa_cf.Text = two;
                                }
                                if (sm_pa == "500")
                                {
                                    cbb_1_500Pa_cf.Text = flish;
                                    cbb_2_500Pa_cf.Text = two;
                                }
                                if (sm_pa == "600")
                                {
                                    cbb_1_600Pa_cf.Text = flish;
                                    cbb_2_600Pa_cf.Text = two;
                                }
                                if (sm_pa == "700")
                                {
                                    cbb_1_700Pa_cf.Text = flish;
                                    cbb_2_700Pa_cf.Text = two;
                                }
                            }
                        }
                        txt_zgfy_cf.Text = sm_pa;
                        txt_desc_cf.Text = remark;
                    }
                    #endregion
                }
            }
        }
        /// <summary>
        /// 绑定设定压力
        /// </summary>
        private void Title()
        {
            lbl_smjc.Text = string.Format("门窗水密性能检测  第{0}号 {1}", this._tempCode, this._tempTong);

            btn_ksbd.Enabled = false;
            btn_tzbd.Enabled = false;
            _serialPortClient.qiehuanTab(false);
        }

        #endregion

        #region 图表控制

        /// <summary>
        /// 水密
        /// </summary>
        private void SMchartInit()
        {
            dtnow = DateTime.Now;
            sm_Line.GetVertAxis.SetMinMax(-1100, 1100);
        }

        private void AnimateSeries(Steema.TeeChart.TChart chart, int yl)
        {

            this.sm_Line.Add(DateTime.Now, yl);
            this.tChart_sm.Axes.Bottom.SetMinMax(dtnow, DateTime.Now.AddSeconds(20));
        }

        private void tim_PainPic_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            var c = _serialPortClient.GetCY_High();
            int value = int.Parse(c.ToString());

            AnimateSeries(this.tChart_sm, value);
            //  AnimateSeries(this.tChart_sm, RegisterData.CY_High_Value);
        }
        #endregion


        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.chart_cms_sm_click.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void 导出图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //todo
            //this.tChart_qm.Export.ShowExportDialog();
        }

        /// <summary>
        /// 急停
        /// </summary>
        private void Stop()
        {
            var res = _serialPortClient.Stop();
            if (!res)
                MessageBox.Show("急停异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }




        private void btn_next_Click(object sender, EventArgs e)
        {

            var yl = _serialPortClient.GetSMYBSDYL(ref IsSeccess, "XYJ");
            if (this.rdb_bdjy.Checked == true)
            {
                if (yl == 350)
                {
                    Stop();
                    this.btn_ready.Enabled = true;
                    this.btn_start.Enabled = true;
                    this.btn_next.Enabled = true;

                    btn_ready.BackColor = Color.Transparent;
                    btn_start.BackColor = Color.Transparent;
                    btn_next.BackColor = Color.Transparent;

                    waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Stop;
                    return;
                }
            }
            else
            {
                if (yl == 700)
                {
                    Stop();
                    this.btn_ready.Enabled = true;
                    this.btn_start.Enabled = true;
                    this.btn_next.Enabled = true;

                    btn_ready.BackColor = Color.Transparent;
                    btn_start.BackColor = Color.Transparent;
                    btn_next.BackColor = Color.Transparent;

                    waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Stop;
                    return;
                }
            }


            var res = _serialPortClient.SendSMXXYJ();
            if (!res)
            {
                MessageBox.Show("设置水密性下一级异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Next;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            //  lbl_sdyl.Text = "0";
            Stop();

            this.btn_ready.Enabled = true;
            this.btn_start.Enabled = true;
            this.btn_next.Enabled = true;
            this.btn_upKpa.Enabled = true;
            this.btn_shuibeng.Enabled = true;

            btn_ready.BackColor = Color.Transparent;
            btn_start.BackColor = Color.Transparent;
            btn_next.BackColor = Color.Transparent;

            btn_upKpa.BackColor = Color.Transparent;
            btn_shuibeng.BackColor = Color.Transparent;

            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Stop;
        }


        #region 水密性能检测按钮事件
        private void btn_ready_Click(object sender, EventArgs e)
        {
            // double yl = _serialPortClient.GetSMYBSDYL(ref IsSeccess, "SMYB");
            //if (!IsSeccess)
            //{
            //     return;
            //}
            //lbl_sdyl.Text = yl.ToString();

            var res = _serialPortClient.SetSMYB();
            if (!res)
            {
                MessageBox.Show("水密预备异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            this.btn_ready.Enabled = false;
            this.btn_start.Enabled = false;
            this.btn_next.Enabled = false;
            this.btn_next.Enabled = false;
            this.btn_shuibeng.Enabled = false;
            this.btn_upKpa.Enabled = false;



            btn_ready.BackColor = Color.Green;
            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Ready;

        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendSMXKS();
            if (!res)
            {
                MessageBox.Show("水密开始异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.btn_start.Enabled = false;
            this.btn_next.Enabled = true;
            this.tim_upNext.Enabled = true;
            this.btn_ready.Enabled = false;

            btn_start.BackColor = Color.Green;
            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Start;
        }
        /// <summary>
        /// 依次加压
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private bool ycjyType = true;

        private void btn_upKpa_Click(object sender, EventArgs e)
        {
            var value = int.Parse(txt_ycjy.Text);
            var res = _serialPortClient.SendSMYCJY(value);
            if (!res)
            {
                return;
            }

            ycjyType = (ycjyType ? false : true);
            if (!ycjyType)
            {
                btn_upKpa.Text = "停止";
            }
            else
            {
                btn_upKpa.Text = "依次加压";

                //lbl_sdyl.Text = "0";
                //  lbl_sdyl.Text = txt_ycjy.Text;

                Stop();
                this.btn_ready.Enabled = true;
                this.btn_start.Enabled = true;
                this.btn_next.Enabled = true;
                waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Stop;
                return;
            }

            tim_upNext.Enabled = false;


            //lbl_sdyl.Text = value.ToString();
            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.CycleLoading;

        }
        #endregion

        private void tim_upNext_Tick(object sender, EventArgs e)
        {
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Stop)
            {
                return;
            }

            if (!_serialPortClient.sp.IsOpen)
                return;

            string TEMP = "";
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Ready)
                TEMP = "SMYB";
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.CycleLoading)
                TEMP = "SMKS";
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Start)
                TEMP = "SMKS";
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Next)
                TEMP = "XYJ";

            //double yl = _serialPortClient.GetSMYBSDYL(ref IsSeccess, TEMP);

            //if (!IsSeccess)
            //{
            //    return;
            //}
            //lbl_sdyl.Text = yl.ToString();


            //if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Next ||
            // waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Start ||
            // waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Next ||
            // waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.SrartBD)
            //{
            //    if (this.rdb_bdjy.Checked == true)
            //    {
            //        lbl_max.Visible = true;

            //        var minVal = 0;
            //        var maxVal = 0;

            //        _serialPortClient.GetCYXS_BODONG(ref IsSeccess, ref minVal, ref maxVal);

            //        lbl_sdyl.Text = minVal.ToString();
            //        lbl_max.Text = maxVal.ToString();
            //    }
            //}
        }

        #region -- 水密选择

        /// <summary>
        /// 位置
        /// </summary>
        private string CheckPosition = "";

        /// <summary>
        /// 问题
        /// </summary>
        private string CheckProblem = "";

        /// <summary>
        /// 数值
        /// </summary>
        private int CheckValue = 0;

        private void cbb_2_0Pa_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(cbb_1_0Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_0Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }

            CheckPosition = cbb_1_0Pa.Text;
            CheckProblem = cbb_2_0Pa.Text;
            CheckValue = 0;

            txt_zgfy.Text = CheckValue.ToString();
        }

        private void cbb_2_100Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_100Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_100Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_100Pa.Text;
            CheckProblem = cbb_2_100Pa.Text;
            CheckValue = 100;

            if (cbb_2_100Pa.Text.Contains("▲") || cbb_2_100Pa.Text.Contains("●"))
            {
                CheckValue = 0;
            }

            txt_zgfy.Text = CheckValue.ToString();
        }

        private void cbb_2_150Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_150Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_150Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_150Pa.Text;
            CheckProblem = cbb_2_150Pa.Text;
            CheckValue = 150;

            if (cbb_2_150Pa.Text.Contains("▲") || cbb_2_150Pa.Text.Contains("●"))
            {
                CheckValue = 100;
            }
            txt_zgfy.Text = CheckValue.ToString();
        }

        private void cbb_2_200Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_200Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_200Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_200Pa.Text;
            CheckProblem = cbb_2_200Pa.Text;
            CheckValue = 200;

            if (cbb_2_200Pa.Text.Contains("▲") || cbb_2_200Pa.Text.Contains("●"))
            {
                CheckValue = 150;
            }
            txt_zgfy.Text = CheckValue.ToString();
        }

        private void cbb_2_250Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_250Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_250Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_250Pa.Text;
            CheckProblem = cbb_2_250Pa.Text;
            CheckValue = 250;

            if (cbb_2_250Pa.Text.Contains("▲") || cbb_2_250Pa.Text.Contains("●"))
            {
                CheckValue = 200;
            }
            txt_zgfy.Text = CheckValue.ToString();
        }

        private void cbb_2_300Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_300Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_300Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_300Pa.Text;
            CheckProblem = cbb_2_300Pa.Text;
            CheckValue = 300;

            if (cbb_2_300Pa.Text.Contains("▲") || cbb_2_300Pa.Text.Contains("●"))
            {
                CheckValue = 250;
            }
            txt_zgfy.Text = CheckValue.ToString();
        }

        private void cbb_2_350Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_350Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_350Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_350Pa.Text;
            CheckProblem = cbb_2_350Pa.Text;
            CheckValue = 350;

            if (cbb_2_350Pa.Text.Contains("▲") || cbb_2_350Pa.Text.Contains("●"))
            {
                CheckValue = 300;
            }
            txt_zgfy.Text = CheckValue.ToString();
        }

        private void cbb_2_400Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_400Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_400Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_400Pa.Text;
            CheckProblem = cbb_2_400Pa.Text;
            CheckValue = 400;

            if (cbb_2_400Pa.Text.Contains("▲") || cbb_2_400Pa.Text.Contains("●"))
            {
                CheckValue = 350;
            }
            txt_zgfy.Text = CheckValue.ToString();
        }

        private void cbb_2_500Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_500Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_500Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_500Pa.Text;
            CheckProblem = cbb_2_500Pa.Text;
            CheckValue = 500;

            if (cbb_2_500Pa.Text.Contains("▲") || cbb_2_500Pa.Text.Contains("●"))
            {
                CheckValue = 400;
            }
            txt_zgfy.Text = CheckValue.ToString();
        }

        private void cbb_2_600Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_600Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_600Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_600Pa.Text;
            CheckProblem = cbb_2_600Pa.Text;
            CheckValue = 600;

            if (cbb_2_600Pa.Text.Contains("▲") || cbb_2_600Pa.Text.Contains("●"))
            {
                CheckValue = 500;
            }
            txt_zgfy.Text = CheckValue.ToString();
        }

        private void cbb_2_700Pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_700Pa.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_700Pa.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_700Pa.Text;
            CheckProblem = cbb_2_700Pa.Text;
            CheckValue = 700;

            if (cbb_2_700Pa.Text.Contains("▲") || cbb_2_700Pa.Text.Contains("●"))
            {
                CheckValue = 600;
            }
            txt_zgfy.Text = CheckValue.ToString();
        }

        private void txt_zgfy_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                int zgfy = int.Parse(txt_zgfy.Text);
                CheckValue = zgfy;
            }
            catch
            {
                MessageBox.Show("请输入数字", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 水密数据处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_2sjcl_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CheckPosition) || string.IsNullOrWhiteSpace(CheckProblem))
            {
                MessageBox.Show("选择失去焦点，请重新选择检测记录！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Model_dt_sm_Info model = new Model_dt_sm_Info();
            model.dt_Code = _tempCode;
            model.info_DangH = _tempTong;
            model.sm_Pa = CheckValue.ToString();
            model.sm_PaDesc = CheckPosition + "," + CheckProblem;
            model.sm_Remark = txt_desc.Text;

            model.testcount = 1;
            model.gongchengjiance = txt_ycjy.Text;


            if (this.rdb_bdjy.Checked == true)
            {
                model.Method = "波动加压";
                model.sxyl = txt_maxValue.Text;
                model.xxyl = txt_minValue.Text;
            }
            else
            {
                model.Method = "稳定加压";
            }

            if (new DAL_dt_sm_Info().Add(model))
            {
                MessageBox.Show("处理成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        private void tChart_sm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.chart_cms_sm_click.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void export_image_sm_Click(object sender, EventArgs e)
        {
            this.tChart_sm.Export.ShowExportDialog();
        }

        private void tim_sm_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            //lbldqyl.Text = RegisterData.CY_High_Value.ToString();

            var value = _serialPortClient.GetCY_High();

            lbldqyl.Text = value.ToString();

            //if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Next ||
            //    waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Start ||
            //    waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Next ||
            //    waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.SrartBD)
            //{
            //    if (this.rdb_bdjy.Checked == true)
            //    {
            //        lbl_max.Visible = true;

            //        var minVal = 0;
            //        var maxVal = 0;

            //        _serialPortClient.GetCYXS_BODONG(ref IsSeccess, ref minVal, ref maxVal);

            //        lbl_sdyl.Text = minVal.ToString();
            //        lbl_max.Text = maxVal.ToString();
            //    }
            //}
        }

        private void btn_ksbd_Click(object sender, EventArgs e)
        {
            int minValue = -1;
            int maxValue = -1;

            int.TryParse(txt_minValue.Text, out maxValue);

            int.TryParse(txt_maxValue.Text, out minValue);
            if (minValue == 0 || maxValue == 0)
            {
                MessageBox.Show("上线-下线压力请设置大于零数字", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var res = _serialPortClient.SendBoDongksjy(maxValue, minValue);
            if (!res)
            {
                MessageBox.Show("水密波动开始异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            this.btn_ksbd.Enabled = false;
            this.btn_tzbd.Enabled = true;
            this.btn_ready.Enabled = false;
            this.btn_start.Enabled = false;
            this.btn_next.Enabled = false;

            tim_upNext.Enabled = false;



            btn_ksbd.BackColor = Color.Green;

            btn_tzbd.BackColor = Color.Transparent;
            btn_ready.BackColor = Color.Transparent;
            btn_start.BackColor = Color.Transparent;
            btn_next.BackColor = Color.Transparent;

            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.SrartBD;
        }

        private void btn_tzbd_Click(object sender, EventArgs e)
        {
            //  lbl_sdyl.Text = "0";
            var res = _serialPortClient.StopBoDong();
            if (!res)
            {
                MessageBox.Show("停止波动", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Stop();
            this.btn_tzbd.Enabled = false;

            Thread.Sleep(5000);

            this.btn_ksbd.Enabled = true;
            this.btn_ready.Enabled = true;
            this.btn_start.Enabled = true;
            this.btn_next.Enabled = true;


            btn_tzbd.BackColor = Color.Green;

            btn_ksbd.BackColor = Color.Transparent;
            btn_ready.BackColor = Color.Transparent;
            btn_start.BackColor = Color.Transparent;
            btn_next.BackColor = Color.Transparent;

            waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.StopBD;
        }

        private void tim_getType_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            //水密预备
            if (waterTightPropertyTest == PublicEnum.WaterTightPropertyTest.Ready)
            {
                int value = _serialPortClient.GetSMYBJS(ref IsSeccess);

                if (!IsSeccess)
                {
                    return;
                }
                if (value == 3)
                {
                    waterTightPropertyTest = PublicEnum.WaterTightPropertyTest.Stop;
                    //    lbl_sdyl.Text = "0";

                    this.btn_ready.Enabled = true;
                    this.btn_start.Enabled = true;
                    this.btn_next.Enabled = true;
                    this.btn_shuibeng.Enabled = true;
                    this.btn_upKpa.Enabled = true;
                    this.btn_shuibeng.Enabled = true;

                    btn_ready.BackColor = Color.Transparent;
                    btn_start.BackColor = Color.Transparent;
                    btn_next.BackColor = Color.Transparent;
                    btn_shuibeng.BackColor = Color.Transparent;
                    btn_upKpa.BackColor = Color.Transparent;
                    btn_shuibeng.BackColor = Color.Transparent;
                }
            }
        }

        private void rdb_bdjy_CheckedChanged(object sender, EventArgs e)
        {
            btn_upKpa.Enabled = false;
            btn_ksbd.Enabled = true;
            btn_tzbd.Enabled = true;

            if (this.rdb_bdjy.Checked == true)
            {
                _serialPortClient.qiehuanTab(true);
            }
            else
            {
                _serialPortClient.qiehuanTab(false);
            }
        }

        private void rdb_wdjy_CheckedChanged(object sender, EventArgs e)
        {
            btn_upKpa.Enabled = true;
            btn_ksbd.Enabled = false;
            btn_tzbd.Enabled = false;
            if (this.rdb_bdjy.Checked == true)
            {
                _serialPortClient.qiehuanTab(true);
            }
            else
            {
                _serialPortClient.qiehuanTab(false);
            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.tabControl1.SelectedTab.Name == "水密")
            {
                Initial(QM_TestCount.第一次);
            }
            else if (this.tabControl1.SelectedTab.Name == "重复水密")
            {
                Initial(QM_TestCount.第二次);
            }
        }

        private void cbb_2_0Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(cbb_1_0Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_0Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }

            CheckPosition = cbb_1_0Pa_cf.Text;
            CheckProblem = cbb_2_0Pa_cf.Text;
            CheckValue = 0;

            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void cbb_2_100Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_100Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_100Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_100Pa_cf.Text;
            CheckProblem = cbb_2_100Pa_cf.Text;
            CheckValue = 100;

            if (cbb_2_100Pa_cf.Text.Contains("▲") || cbb_2_100Pa_cf.Text.Contains("●"))
            {
                CheckValue = 0;
            }

            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void cbb_2_150Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_150Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_150Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_150Pa_cf.Text;
            CheckProblem = cbb_2_150Pa_cf.Text;
            CheckValue = 150;

            if (cbb_2_150Pa_cf.Text.Contains("▲") || cbb_2_150Pa_cf.Text.Contains("●"))
            {
                CheckValue = 100;
            }
            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void cbb_2_200Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_200Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_200Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_200Pa_cf.Text;
            CheckProblem = cbb_2_200Pa_cf.Text;
            CheckValue = 200;

            if (cbb_2_200Pa_cf.Text.Contains("▲") || cbb_2_200Pa_cf.Text.Contains("●"))
            {
                CheckValue = 150;
            }
            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void cbb_2_250Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_250Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_250Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_250Pa_cf.Text;
            CheckProblem = cbb_2_250Pa_cf.Text;
            CheckValue = 250;

            if (cbb_2_250Pa_cf.Text.Contains("▲") || cbb_2_250Pa_cf.Text.Contains("●"))
            {
                CheckValue = 200;
            }
            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void cbb_2_300Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_300Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_300Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_300Pa_cf.Text;
            CheckProblem = cbb_2_300Pa_cf.Text;
            CheckValue = 300;

            if (cbb_2_300Pa_cf.Text.Contains("▲") || cbb_2_300Pa_cf.Text.Contains("●"))
            {
                CheckValue = 250;
            }
            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void cbb_2_350Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_350Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_350Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_350Pa_cf.Text;
            CheckProblem = cbb_2_350Pa_cf.Text;
            CheckValue = 350;

            if (cbb_2_350Pa_cf.Text.Contains("▲") || cbb_2_350Pa_cf.Text.Contains("●"))
            {
                CheckValue = 300;
            }
            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void cbb_2_400Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_400Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_400Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_400Pa_cf.Text;
            CheckProblem = cbb_2_400Pa_cf.Text;
            CheckValue = 400;

            if (cbb_2_400Pa_cf.Text.Contains("▲") || cbb_2_400Pa_cf.Text.Contains("●"))
            {
                CheckValue = 350;
            }
            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void cbb_2_500Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_500Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_500Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_500Pa_cf.Text;
            CheckProblem = cbb_2_500Pa_cf.Text;
            CheckValue = 500;

            if (cbb_2_500Pa_cf.Text.Contains("▲") || cbb_2_500Pa_cf.Text.Contains("●"))
            {
                CheckValue = 400;
            }
            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void cbb_2_600Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_600Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_600Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_600Pa_cf.Text;
            CheckProblem = cbb_2_600Pa_cf.Text;
            CheckValue = 600;

            if (cbb_2_600Pa_cf.Text.Contains("▲") || cbb_2_600Pa_cf.Text.Contains("●"))
            {
                CheckValue = 500;
            }
            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void cbb_2_700Pa_cf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbb_1_700Pa_cf.Text))
            {
                MessageBox.Show("请选择位置", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbb_2_700Pa_cf.Text = "";
                CheckProblem = "";
                CheckPosition = "";
                CheckValue = 0;
                return;
            }
            CheckPosition = cbb_1_700Pa_cf.Text;
            CheckProblem = cbb_2_700Pa_cf.Text;
            CheckValue = 700;

            if (cbb_2_700Pa_cf.Text.Contains("▲") || cbb_2_700Pa_cf.Text.Contains("●"))
            {
                CheckValue = 600;
            }
            txt_zgfy_cf.Text = CheckValue.ToString();
        }

        private void btn_1sjcl_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CheckPosition) || string.IsNullOrWhiteSpace(CheckProblem))
            {
                MessageBox.Show("选择失去焦点，请重新选择检测记录！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Model_dt_sm_Info model = new Model_dt_sm_Info();
            model.dt_Code = _tempCode;
            model.info_DangH = _tempTong;
            model.sm_Pa = CheckValue.ToString();
            model.sm_PaDesc = CheckPosition + "," + CheckProblem;
            model.sm_Remark = txt_desc_cf.Text;
            model.testcount = 2;
            model.gongchengjiance = txt_ycjy.Text;

            if (this.rdb_bdjy.Checked == true)
            {
                model.Method = "波动加压";
                model.sxyl = txt_maxValue.Text;
                model.xxyl = txt_minValue.Text;
            }
            else
            {
                model.Method = "稳定加压";
            }

            if (new DAL_dt_sm_Info().Add(model))
            {
                MessageBox.Show("处理成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool _ShuiBengQiDong = false;
        private void btn_shuibeng_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.SendShuiBengQiDong(ref _ShuiBengQiDong);
            if (!res)
            {
                MessageBox.Show("水泵启动异常,请确认服务器连接是否成功!", "设置", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_shuibeng.BackColor = _ShuiBengQiDong ? Color.Green : Color.Transparent;
        }

        private void btn_next_MouseDown(object sender, MouseEventArgs e)
        {
            btn_next.BackColor = Color.Green;
        }

        private void btn_next_MouseUp(object sender, MouseEventArgs e)
        {
            btn_next.BackColor = Color.Transparent;
        }
    }
}
