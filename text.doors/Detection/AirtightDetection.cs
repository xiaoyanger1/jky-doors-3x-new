
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
using static text.doors.Default.PublicEnum;
using NPOI.SS.Formula.Functions;

namespace text.doors.Detection
{
    public partial class AirtightDetection : Form
    {
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        private SerialPortClient _serialPortClient;
        //检验编号
        private string _tempCode = "";
        //当前樘号
        private string _tempTong = "";

        /// <summary>
        /// 气密数据载体
        /// </summary>
        Pressure pressure_One = new Pressure();

        /// <summary>
        /// 气密数据载体重复
        /// </summary>
        Pressure pressure_Two = new Pressure();

        /// <summary>
        /// 气密数据位置
        /// </summary>
        private PublicEnum.AirtightPropertyTest? airtightPropertyTest = null;

        /// <summary>
        /// 分级指标
        /// </summary>
        double zFc = 0, fFc = 0, zMj = 0, fMj = 0;

        public List<ReadT> _readT = new List<ReadT>();

        public DateTime dtnow { get; set; }

        private static List<Pressure> pressureList = new List<Pressure>();


        public AirtightDetection()
        {
            tc_RealTimeSurveillance.Anchor = AnchorStyles.Top;
        }

        public AirtightDetection(SerialPortClient tcpClient, string tempCode, string tempTong)
        {
            InitializeComponent();
            this._serialPortClient = tcpClient;
            this._tempCode = tempCode;
            this._tempTong = tempTong;

            pressure_One = new Pressure();
            pressure_Two = new Pressure();

            Init();
        }

        private void Init()
        {
            if (this.tabControl1.SelectedTab.Name == "流量原始数据")
            {
                GetPressureFlow(QM_TestCount.第一次);
                BindFlowBase();
                BindLevelIndex(QM_TestCount.第一次);
            }
            else if (this.tabControl1.SelectedTab.Name == "重复流量数据")
            {
                GetPressureFlow(QM_TestCount.第一次);
                BindFlowBase();
                BindLevelIndex(QM_TestCount.第二次);
            }

            BindSetPressure();
            QMchartInit();

            Clear();
        }

        /// <summary>
        /// 绑定设定压力
        /// </summary>
        private void BindSetPressure()
        {
            lbl_title.Text = string.Format("门窗气密性能检测  第{0}号 {1}", this._tempCode, this._tempTong);
        }

        #region 数据绑定

        /// <summary>
        /// 获取流量数据
        /// </summary>
        /// <returns></returns>
        public void GetPressureFlow(PublicEnum.QM_TestCount qm_TestCount)
        {
            GetWindSpeedBase((int)qm_TestCount);
        }

        /// <summary>
        /// 绑定流量
        /// </summary>
        private void BindFlowBase()
        {
            if (this.tabControl1.SelectedTab.Name == "流量原始数据")
            {
                dgv_ll.DataSource = null;
                dgv_ll.DataSource = pressureList;

                dgv_ll.RowHeadersVisible = false;
                dgv_ll.AllowUserToResizeColumns = false;
                dgv_ll.AllowUserToResizeRows = false;
                dgv_ll.Columns[0].HeaderText = "压力Pa";
                dgv_ll.Columns[0].Width = 50;
                dgv_ll.Columns[0].ReadOnly = true;
                dgv_ll.Columns[0].DataPropertyName = "PressurePa";
                dgv_ll.Columns[1].HeaderText = "正压附加";
                dgv_ll.Columns[1].Width = 54;
                dgv_ll.Columns[1].DataPropertyName = "Pressure_Z";
                dgv_ll.Columns[2].HeaderText = "正压总的";
                dgv_ll.Columns[2].Width = 54;
                dgv_ll.Columns[2].DataPropertyName = "Pressure_Z_Z";
                dgv_ll.Columns[3].HeaderText = "负压附加";
                dgv_ll.Columns[3].Width = 54;
                dgv_ll.Columns[3].DataPropertyName = "Pressure_F";
                dgv_ll.Columns[4].HeaderText = "负压总的";
                dgv_ll.Columns[4].Width = 54;
                dgv_ll.Columns[4].DataPropertyName = "Pressure_F_Z";

                dgv_ll.Columns["Pressure_Z"].DefaultCellStyle.Format = "N2";
                dgv_ll.Columns["Pressure_Z_Z"].DefaultCellStyle.Format = "N2";
                dgv_ll.Columns["Pressure_F"].DefaultCellStyle.Format = "N2";
                dgv_ll.Columns["Pressure_F_Z"].DefaultCellStyle.Format = "N2";
            }
            else
            {
                dgv_ll2.DataSource = null;
                dgv_ll2.DataSource = pressureList;

                dgv_ll2.RowHeadersVisible = false;
                dgv_ll2.AllowUserToResizeColumns = false;
                dgv_ll2.AllowUserToResizeRows = false;
                dgv_ll2.Columns[0].HeaderText = "压力Pa";
                dgv_ll2.Columns[0].Width = 50;
                dgv_ll2.Columns[0].ReadOnly = true;
                dgv_ll2.Columns[0].DataPropertyName = "PressurePa";
                dgv_ll2.Columns[1].HeaderText = "正压附加";
                dgv_ll2.Columns[1].Width = 54;
                dgv_ll2.Columns[1].DataPropertyName = "Pressure_Z";
                dgv_ll2.Columns[2].HeaderText = "正压总的";
                dgv_ll2.Columns[2].Width = 54;
                dgv_ll2.Columns[2].DataPropertyName = "Pressure_Z_Z";
                dgv_ll2.Columns[3].HeaderText = "负压附加";
                dgv_ll2.Columns[3].Width = 54;
                dgv_ll2.Columns[3].DataPropertyName = "Pressure_F";
                dgv_ll2.Columns[4].HeaderText = "负压总的";
                dgv_ll2.Columns[4].Width = 54;
                dgv_ll2.Columns[4].DataPropertyName = "Pressure_F_Z";

                dgv_ll2.Columns["Pressure_Z"].DefaultCellStyle.Format = "N2";
                dgv_ll2.Columns["Pressure_Z_Z"].DefaultCellStyle.Format = "N2";
                dgv_ll2.Columns["Pressure_F"].DefaultCellStyle.Format = "N2";
                dgv_ll2.Columns["Pressure_F_Z"].DefaultCellStyle.Format = "N2";
            }
        }


        /// <summary>
        /// 获取流量
        /// </summary>
        private List<Pressure> GetWindSpeedBase(int qm_TestCount)
        {
            pressureList = new List<Pressure>();
            List<Model_dt_qm_Info> qm_Info = new DAL_dt_Settings().GetQMListByCode(_tempCode);

            if (qm_Info != null && qm_Info.Count > 0)
            {
                var qm = qm_Info.FindAll(t => t.info_DangH == _tempTong && string.IsNullOrWhiteSpace(t.qm_j_f_zd100) == false && t.testcount == qm_TestCount).OrderBy(t => t.info_DangH).ToList();
                if ((qm != null && qm.Count() > 0))
                {
                    gv_list.Enabled = false;

                    //监控
                    if (qm[0].testtype == "1")
                    {
                        btn_ycjy_z.Enabled = false;
                        btn_ycjyf.Enabled = false;

                        //绑定风速
                        foreach (var item in qm)
                        {
                            #region 赋值
                            Pressure model4 = new Pressure();
                            model4.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj10) ? 0 : double.Parse(item.qm_s_f_fj10);
                            model4.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd10) ? 0 : double.Parse(item.qm_s_f_zd10);

                            model4.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj10) ? 0 : double.Parse(item.qm_s_z_fj10);
                            model4.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd10) ? 0 : double.Parse(item.qm_s_z_zd10);
                            model4.PressurePa = "10";
                            pressureList.Add(model4);

                            Pressure model5 = new Pressure();
                            model5.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj30) ? 0 : double.Parse(item.qm_s_f_fj30);
                            model5.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd30) ? 0 : double.Parse(item.qm_s_f_zd30);

                            model5.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj30) ? 0 : double.Parse(item.qm_s_z_fj30);
                            model5.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd30) ? 0 : double.Parse(item.qm_s_z_zd30);
                            model5.PressurePa = "30";
                            pressureList.Add(model5);

                            Pressure model6 = new Pressure();
                            model6.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj50) ? 0 : double.Parse(item.qm_s_f_fj50);
                            model6.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd50) ? 0 : double.Parse(item.qm_s_f_zd50);

                            model6.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj50) ? 0 : double.Parse(item.qm_s_z_fj50);
                            model6.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd50) ? 0 : double.Parse(item.qm_s_z_zd50);
                            model6.PressurePa = "50";
                            pressureList.Add(model6);

                            Pressure model7 = new Pressure();
                            model7.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj70) ? 0 : double.Parse(item.qm_s_f_fj70);
                            model7.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd70) ? 0 : double.Parse(item.qm_s_f_zd70);

                            model7.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj70) ? 0 : double.Parse(item.qm_s_z_fj70);
                            model7.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd70) ? 0 : double.Parse(item.qm_s_z_zd70);
                            model7.PressurePa = "70";
                            pressureList.Add(model7);


                            Pressure model1 = new Pressure();
                            model1.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj100) ? 0 : double.Parse(item.qm_s_f_fj100);
                            model1.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd100) ? 0 : double.Parse(item.qm_s_f_zd100);

                            model1.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj100) ? 0 : double.Parse(item.qm_s_z_fj100);
                            model1.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd100) ? 0 : double.Parse(item.qm_s_z_zd100);
                            model1.PressurePa = "100";
                            pressureList.Add(model1);

                            Pressure model2 = new Pressure();
                            model2.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj150) ? 0 : double.Parse(item.qm_s_f_fj150);
                            model2.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd150) ? 0 : double.Parse(item.qm_s_f_zd150);

                            model2.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj150) ? 0 : double.Parse(item.qm_s_z_fj150);
                            model2.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd150) ? 0 : double.Parse(item.qm_s_z_zd150);
                            model2.PressurePa = "150";
                            pressureList.Add(model2);

                            Pressure model3 = new Pressure();
                            model3.Pressure_F = string.IsNullOrWhiteSpace(item.qm_j_f_fj100) ? 0 : double.Parse(item.qm_j_f_fj100);
                            model3.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_j_f_zd100) ? 0 : double.Parse(item.qm_j_f_zd100);

                            model3.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_j_z_fj100) ? 0 : double.Parse(item.qm_j_z_fj100);
                            model3.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_j_z_zd100) ? 0 : double.Parse(item.qm_j_z_zd100);
                            model3.PressurePa = "100";
                            pressureList.Add(model3);

                            Pressure model8 = new Pressure();
                            model8.Pressure_F = string.IsNullOrWhiteSpace(item.qm_j_f_fj70) ? 0 : double.Parse(item.qm_j_f_fj70);
                            model8.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_j_f_zd70) ? 0 : double.Parse(item.qm_j_f_zd70);

                            model8.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_j_z_fj70) ? 0 : double.Parse(item.qm_j_z_fj70);
                            model8.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_j_z_zd70) ? 0 : double.Parse(item.qm_j_z_zd70);
                            model8.PressurePa = "70";
                            pressureList.Add(model8);

                            Pressure model9 = new Pressure();
                            model9.Pressure_F = string.IsNullOrWhiteSpace(item.qm_j_f_fj50) ? 0 : double.Parse(item.qm_j_f_fj50);
                            model9.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_j_f_zd50) ? 0 : double.Parse(item.qm_j_f_zd50);

                            model9.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_j_z_fj50) ? 0 : double.Parse(item.qm_j_z_fj50);
                            model9.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_j_z_zd50) ? 0 : double.Parse(item.qm_j_z_zd50);
                            model9.PressurePa = "50";
                            pressureList.Add(model9);

                            Pressure model10 = new Pressure();
                            model10.Pressure_F = string.IsNullOrWhiteSpace(item.qm_j_f_fj30) ? 0 : double.Parse(item.qm_j_f_fj30);
                            model10.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_j_f_zd30) ? 0 : double.Parse(item.qm_j_f_zd30);

                            model10.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_j_z_fj30) ? 0 : double.Parse(item.qm_j_z_fj30);
                            model10.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_j_z_zd30) ? 0 : double.Parse(item.qm_j_z_zd30);
                            model10.PressurePa = "30";
                            pressureList.Add(model10);

                            Pressure model11 = new Pressure();
                            model11.Pressure_F = string.IsNullOrWhiteSpace(item.qm_j_f_fj10) ? 0 : double.Parse(item.qm_j_f_fj10);
                            model11.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_j_f_zd10) ? 0 : double.Parse(item.qm_j_f_zd10);

                            model11.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_j_z_fj10) ? 0 : double.Parse(item.qm_j_z_fj10);
                            model11.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_j_z_zd10) ? 0 : double.Parse(item.qm_j_z_zd10);
                            model11.PressurePa = "10";
                            pressureList.Add(model11);

                            Pressure model12 = new Pressure();
                            model12.Pressure_F = 0;
                            model12.Pressure_F_Z = 0;

                            model12.Pressure_Z = 0;
                            model12.Pressure_Z_Z = 0;
                            model12.PressurePa = "设计值";
                            pressureList.Add(model12);
                            #endregion
                        }

                    }
                    else if (qm[0].testtype == "2") //工程检测  不存在监控数据
                    {
                        this.btn_justready.Enabled = false;
                        this.btn_loseready.Enabled = false;
                        this.btn_losestart.Enabled = false;
                        this.btn_juststart.Enabled = false;

                        txt_ycjy_z.Text = qm[0].sjz_value;
                        txt_ycjy_f.Text = qm[0].sjz_value;

                        //绑定空监测数据
                        if (qm_TestCount == (int)QM_TestCount.第一次)
                        {
                            pressureList = pressure_One.GetPressure();
                        }
                        else if (qm_TestCount == (int)QM_TestCount.第二次)
                        {
                            pressureList = pressure_Two.GetPressure();
                        }
                        //绑定设计值
                        var sjz = pressureList.Find(t => t.PressurePa == "设计值");
                        if (sjz != null)
                        {
                            sjz.Pressure_F = double.Parse(qm[0].sjz_f_fj);
                            sjz.Pressure_F_Z = double.Parse(qm[0].sjz_f_zd);
                            sjz.Pressure_Z = double.Parse(qm[0].sjz_z_fj);
                            sjz.Pressure_Z_Z = double.Parse(qm[0].sjz_z_zd);
                        }
                    }
                }
                else
                {
                    if (qm_TestCount == (int)QM_TestCount.第一次)
                    {
                        pressureList = pressure_One.GetPressure();
                    }
                    else if (qm_TestCount == (int)QM_TestCount.第二次)
                    {
                        pressureList = pressure_Two.GetPressure();
                    }
                }
            }
            else
            {
                if (qm_TestCount == (int)QM_TestCount.第一次)
                {
                    pressureList = pressure_One.GetPressure();
                }
                else if (qm_TestCount == (int)QM_TestCount.第二次)
                {
                    pressureList = pressure_Two.GetPressure();
                }
            }

            return pressureList;
        }


        /// <summary>
        /// 绑定分级指标
        /// </summary>
        private void BindLevelIndex(QM_TestCount qm_TestCount)
        {
            GetDatabaseLevelIndex(qm_TestCount);
            dgv_levelIndex.DataSource = GetLevelIndex();
            dgv_levelIndex.RowHeadersVisible = false;
            dgv_levelIndex.AllowUserToResizeColumns = false;
            dgv_levelIndex.AllowUserToResizeRows = false;
            dgv_levelIndex.Columns[0].HeaderText = "渗透量";
            dgv_levelIndex.Columns[0].Width = 96;
            dgv_levelIndex.Columns[0].ReadOnly = true;
            dgv_levelIndex.Columns[0].DataPropertyName = "Quantity";
            dgv_levelIndex.Columns[1].HeaderText = "正压";
            dgv_levelIndex.Columns[1].Width = 87;
            dgv_levelIndex.Columns[1].DataPropertyName = "PressureZ";
            dgv_levelIndex.Columns[2].HeaderText = "负压";
            dgv_levelIndex.Columns[2].Width = 87;
            dgv_levelIndex.Columns[2].DataPropertyName = "PressureF";

            dgv_levelIndex.Columns["PressureZ"].DefaultCellStyle.Format = "N2";
            dgv_levelIndex.Columns["PressureF"].DefaultCellStyle.Format = "N2";
        }

        /// <summary>
        /// 获取分级指标
        /// </summary>
        /// <returns></returns>
        private List<LevelIndex> GetLevelIndex()
        {
            return new List<LevelIndex>()
            {
                new  LevelIndex(){ Quantity="单位缝长",  PressureZ =Math.Round(zFc,2), PressureF  =Math.Round(fFc,2)},
                new  LevelIndex(){ Quantity="单位面积",  PressureZ =Math.Round(zMj,2), PressureF  =Math.Round(fMj,2)}
            };
        }




        #endregion

        #region 图表控制
        /// <summary>
        /// 风速图标
        /// </summary>
        private void QMchartInit()
        {
            dtnow = DateTime.Now;
            qm_Line.GetVertAxis.SetMinMax(-600, 600);
        }



        private void AnimateSeries(Steema.TeeChart.TChart chart, int yl)
        {
            this.qm_Line.Add(DateTime.Now, yl);
            this.tChart_qm.Axes.Bottom.SetMinMax(dtnow, DateTime.Now.AddSeconds(40));
        }

        /// <summary>
        /// 确定当前读取的压力状态
        /// </summary>
        private PublicEnum.Kpa_Level? kpa_Level = null;

        /// <summary>
        /// 差压读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tim_qm_Tick(object sender, EventArgs e)
        {

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZStart || airtightPropertyTest == PublicEnum.AirtightPropertyTest.FStart)
            {
                if (this.tim_Top10.Enabled == false)
                    SetCurrType();
            }

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZYCJY)
            {
                if (this.tim_Top10.Enabled == false)
                {
                    var start = _serialPortClient.GetQiMiTimeStart(BFMCommand.正压依次加压);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.YCJY;
                        tim_Top10.Enabled = true;
                    }
                }
            }
            else if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.FYCJY)
            {
                if (this.tim_Top10.Enabled == false)
                {
                    var start = _serialPortClient.GetQiMiTimeStart(BFMCommand.负压依次加压);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.YCJY;
                        tim_Top10.Enabled = true;
                    }
                }
            }
        }

        private void tim_PainPic_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            int c = 0;
            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZReady || airtightPropertyTest == PublicEnum.AirtightPropertyTest.FReady)
            {
                c = _serialPortClient.GetCY_High();
            }
            else
            {
                c = _serialPortClient.GetCY_Low();
            }


            lbl_dqyl.Text = c.ToString();

            AnimateSeries(this.tChart_qm, c);
        }

        int index = 0;
        private void tim_Top10_Tick(object sender, EventArgs e)
        {
            index++;

            if (index < 4)
            {
                return;
            }

            if (index > 8)
            {
                this.tim_Top10.Enabled = false;

                if (airtightPropertyTest != PublicEnum.AirtightPropertyTest.ZYCJY && airtightPropertyTest != PublicEnum.AirtightPropertyTest.FYCJY)
                {
                    index = 0;
                }
                else
                {
                    btn_ycjy_z.Enabled = true;
                    btn_ycjyf.Enabled = true;
                    btn_ycjy_z.BackColor = Color.Transparent;
                    btn_ycjyf.BackColor = Color.Transparent;
                }
                gv_list.Enabled = false;
                return;
            }
            gv_list.Enabled = true;

            var cyvalue = _serialPortClient.GetCY_Low();

            //获取风速
            var fsvalue = _serialPortClient.GetFSXS();

            //if (rdb_zdstl.Checked)
            //{
            //    Logger.Info($"总的风速：{fsvalue}当前级别：{kpa_Level}");
            //}
            //else
            //{
            //    Logger.Info($"附加风速：{fsvalue}当前级别：{kpa_Level}");
            //}

            //转换流量
            fsvalue = Formula.MathFlow(fsvalue);

            //if (rdb_zdstl.Checked)
            //{
            //    Logger.Info($"总的风速：{fsvalue}当前级别：{kpa_Level}");
            //}
            //else
            //{
            //    Logger.Info($"附加风速：{fsvalue}当前级别：{kpa_Level}");
            //}
            if (this.tabControl1.SelectedTab.Name == "流量原始数据")
            {
                #region 第一次
                if (rdb_fjstl.Checked)
                {
                    if (kpa_Level == PublicEnum.Kpa_Level.liter10)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter10);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter10);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter30)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter30);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter30);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter50)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter50);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter50);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter70)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter70);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter70);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter100)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter100);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter100);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter150)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter150);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter150);

                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop100);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop100);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop70)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop70);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop70);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop50)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop50);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop50);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop30)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop30);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop30);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop10)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop10);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop10);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.YCJY)
                    {
                        //todo:设计值计算
                        fsvalue = _serialPortClient.GetFSXS();
                        fsvalue = Formula.MathFlow(fsvalue);
                        if (cyvalue > 0)
                            pressure_One.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.YCJY);
                        else
                            pressure_One.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.YCJY);
                    }
                }
                else if (rdb_zdstl.Checked)
                {
                    if (kpa_Level == PublicEnum.Kpa_Level.liter10)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter10);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter10);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter30)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter30);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter30);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter50)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter50);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter50);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter70)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter70);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter70);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter100)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter100);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter100);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter150)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter150);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter150);

                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop100);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop100);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop70)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop70);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop70);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop50)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop50);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop50);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop30)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop30);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop30);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop10)
                    {
                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop10);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop10);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.YCJY)
                    {
                        //todo:设计值计算
                        fsvalue = _serialPortClient.GetFSXS();
                        fsvalue = Formula.MathFlow(fsvalue);

                        if (cyvalue > 0)
                            pressure_One.AddZYZD(fsvalue, PublicEnum.Kpa_Level.YCJY);
                        else
                            pressure_One.AddFYZD(fsvalue, PublicEnum.Kpa_Level.YCJY);
                    }
                }
                #endregion
            }
            else if (this.tabControl1.SelectedTab.Name == "重复流量数据")
            {
                #region 第二次
                if (rdb_fjstl.Checked)
                {
                    if (kpa_Level == PublicEnum.Kpa_Level.liter10)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter10);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter10);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter30)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter30);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter30);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter50)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter50);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter50);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter70)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter70);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter70);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter100)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter100);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter100);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter150)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter150);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter150);

                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop100);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop100);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop70)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop70);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop70);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop50)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop50);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop50);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop30)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop30);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop30);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop10)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop10);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop10);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.YCJY)
                    {
                        //todo:设计值计算
                        fsvalue = _serialPortClient.GetFSXS();
                        fsvalue = Formula.MathFlow(fsvalue);
                        if (cyvalue > 0)
                            pressure_Two.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.YCJY);
                        else
                            pressure_Two.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.YCJY);
                    }
                }
                else if (rdb_zdstl.Checked)
                {
                    if (kpa_Level == PublicEnum.Kpa_Level.liter10)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter10);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter10);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter30)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter30);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter30);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter50)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter50);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter50);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter70)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter70);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter70);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter100)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter100);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter100);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.liter150)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter150);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter150);

                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop100);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop100);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop70)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop70);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop70);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop50)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop50);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop50);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop30)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop30);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop30);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.drop10)
                    {
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop10);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop10);
                    }
                    else if (kpa_Level == PublicEnum.Kpa_Level.YCJY)
                    {
                        //todo:设计值计算
                        fsvalue = _serialPortClient.GetFSXS();
                        fsvalue = Formula.MathFlow(fsvalue);
                        if (cyvalue > 0)
                            pressure_Two.AddZYZD(fsvalue, PublicEnum.Kpa_Level.YCJY);
                        else
                            pressure_Two.AddFYZD(fsvalue, PublicEnum.Kpa_Level.YCJY);
                    }
                }
                #endregion
            }
        }


        /// <summary>
        /// 设置添加数据状态
        /// </summary>
        /// <param name="value"></param>
        private void SetCurrType()
        {
            bool start = false;
            var notReadList = _readT.FindAll(t => t.IsRead == false);
            if (notReadList == null || notReadList.Count == 0)
            {
                return;
            }
            var notRead = notReadList?.OrderBy(t => t.Order)?.First();

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZStart)
            {
                if (notRead?.Key == BFMCommand.正压10TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter10;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压30TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter30;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压50TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter50;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压70TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter70;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压100TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter100;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压150TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter150;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压_100TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop100;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压_70TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop70;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压_50TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop50;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压_30TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop30;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.正压_10TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop10;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                //else if (notRead?.Key == BFMCommand.正压依次加压)
                //{
                //    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                //    if (start)
                //    {
                //        kpa_Level = PublicEnum.Kpa_Level.YCJY;
                //        tim_Top10.Enabled = true;
                //        notRead.IsRead = true;
                //    }
                //}
            }

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.FStart)
            {

                if (notRead?.Key == BFMCommand.负压10TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter10;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压30TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter30;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压50TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter50;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压70TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter70;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压100TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter100;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压150TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.liter150;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压_100TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop100;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压_70TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop70;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压_50TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop50;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压_30TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop30;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                else if (notRead?.Key == BFMCommand.负压_10TimeStart)
                {
                    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                    if (start)
                    {
                        kpa_Level = PublicEnum.Kpa_Level.drop10;
                        tim_Top10.Enabled = true;
                        notRead.IsRead = true;
                    }
                }
                //else if (notRead?.Key == BFMCommand.负压依次加压)
                //{
                //    start = _serialPortClient.GetQiMiTimeStart(notRead?.Key);
                //    if (start)
                //    {
                //        kpa_Level = PublicEnum.Kpa_Level.YCJY;
                //        tim_Top10.Enabled = true;
                //        notRead.IsRead = true;
                //    }
                //}
            }
        }

        #endregion


        #region 气密性能检测按钮事件

        /// <summary>
        /// 判断是否开启正压开始或负压开始
        /// </summary>
        //private bool IsStart = false;

        private void btn_justready_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            //double yl = _serialPortClient.GetZYYBYLZ(ref IsSeccess, "ZYYB");
            //if (!IsSeccess)
            //{
            //    MessageBox.Show("读取设定值异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //lbl_setYL.Text = yl.ToString();



            var res = _serialPortClient.SetZYYB();
            if (!res)
            {
                return;
            }
            DisableBtnType();
            airtightPropertyTest = PublicEnum.AirtightPropertyTest.ZReady;

            btn_justready.BackColor = Color.Green;

            //关闭依次加压
            btn_ycjy_z.Enabled = false;
            btn_ycjyf.Enabled = false;

            btn_ycjy_z.BackColor = Color.Transparent;
            btn_ycjyf.BackColor = Color.Transparent;
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

        /// <summary>
        /// 正压开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_juststart_Click(object sender, EventArgs e)
        {
            index = 0;

            var res = _serialPortClient.SendZYKS();
            if (!res)
                return;

            DisableBtnType();

            _readT = new List<ReadT>();
            _readT.Add(new ReadT() { Key = BFMCommand.正压10TimeStart, Order = 1, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压30TimeStart, Order = 2, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压50TimeStart, Order = 3, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压70TimeStart, Order = 4, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压100TimeStart, Order = 5, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压150TimeStart, Order = 6, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压_100TimeStart, Order = 7, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压_70TimeStart, Order = 8, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压_50TimeStart, Order = 9, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压_30TimeStart, Order = 10, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.正压_10TimeStart, Order = 11, IsRead = false });


            if (rdb_fjstl.Checked)
            {
                foreach (var item in pressureList)
                {
                    item.Pressure_Z = 0;
                }
            }
            else if (rdb_zdstl.Checked)
            {
                foreach (var item in pressureList)
                {
                    item.Pressure_Z_Z = 0;
                }

            }
            BindFlowBase();

            btn_juststart.BackColor = Color.Green;

            airtightPropertyTest = PublicEnum.AirtightPropertyTest.ZStart;


            //关闭依次加压
            btn_ycjy_z.Enabled = false;
            btn_ycjyf.Enabled = false;

            btn_ycjy_z.BackColor = Color.Transparent;
            btn_ycjyf.BackColor = Color.Transparent;
        }

        private void btn_loseready_Click(object sender, EventArgs e)
        {
            //double yl = _serialPortClient.GetZYYBYLZ(ref IsSeccess, "FYYB");
            //if (!IsSeccess)
            //{
            //    MessageBox.Show("读取设定值异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //lbl_setYL.Text = "-" + yl.ToString();

            var res = _serialPortClient.SendFYYB();
            if (!res)
            {
                return;
            }

            DisableBtnType();
            btn_loseready.BackColor = Color.Green;
            airtightPropertyTest = PublicEnum.AirtightPropertyTest.FReady;


            //关闭依次加压
            btn_ycjy_z.Enabled = false;
            btn_ycjyf.Enabled = false;

            btn_ycjy_z.BackColor = Color.Transparent;
            btn_ycjyf.BackColor = Color.Transparent;
        }
        /// <summary>
        /// 禁用按钮
        /// </summary>
        private void DisableBtnType()
        {
            this.btn_justready.Enabled = false;
            this.btn_loseready.Enabled = false;
            this.btn_losestart.Enabled = false;
            //this.btn_datadispose.Enabled = false;
            this.btn_juststart.Enabled = false;
            btn_ycjy_z.Enabled = false;
            btn_ycjyf.Enabled = false;



            btn_justready.BackColor = Color.Transparent;
            btn_loseready.BackColor = Color.Transparent;
            btn_losestart.BackColor = Color.Transparent;
            btn_juststart.BackColor = Color.Transparent;
            btn_ycjy_z.BackColor = Color.Transparent;
            btn_ycjyf.BackColor = Color.Transparent;
        }

        /// <summary>
        /// 开启按钮
        /// </summary>
        private void OpenBtnType()
        {
            this.btn_justready.Enabled = true;
            this.btn_loseready.Enabled = true;
            this.btn_losestart.Enabled = true;
            this.btn_datadispose.Enabled = true;
            this.btn_juststart.Enabled = true;
            //btn_ycjy_z.Enabled = true;
            //btn_ycjyf.Enabled = true;

            btn_justready.BackColor = Color.Transparent;
            btn_loseready.BackColor = Color.Transparent;
            btn_losestart.BackColor = Color.Transparent;
            btn_juststart.BackColor = Color.Transparent;
            btn_ycjy_z.BackColor = Color.Transparent;
            btn_ycjyf.BackColor = Color.Transparent;
        }


        /// <summary>
        /// 负压开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_losestart_Click(object sender, EventArgs e)
        {
            index = 0;

            var res = _serialPortClient.SendFYKS();
            if (!res)
                return;
            DisableBtnType();

            _readT = new List<ReadT>();

            _readT.Add(new ReadT() { Key = BFMCommand.负压10TimeStart, Order = 1, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压30TimeStart, Order = 2, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压50TimeStart, Order = 3, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压70TimeStart, Order = 4, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压100TimeStart, Order = 5, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压150TimeStart, Order = 6, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压_100TimeStart, Order = 7, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压_70TimeStart, Order = 8, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压_50TimeStart, Order = 9, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压_30TimeStart, Order = 10, IsRead = false });
            _readT.Add(new ReadT() { Key = BFMCommand.负压_10TimeStart, Order = 11, IsRead = false });



            if (rdb_fjstl.Checked)
            {
                foreach (var item in pressureList)
                {
                    item.Pressure_F = 0;
                }
            }
            else if (rdb_zdstl.Checked)
            {
                foreach (var item in pressureList)
                {
                    item.Pressure_F_Z = 0;
                }
            }
            BindFlowBase();


            btn_losestart.BackColor = Color.Green;
            airtightPropertyTest = PublicEnum.AirtightPropertyTest.FStart;


            //关闭依次加压
            btn_ycjy_z.Enabled = false;
            btn_ycjyf.Enabled = false;

            btn_ycjy_z.BackColor = Color.Transparent;
            btn_ycjyf.BackColor = Color.Transparent;
        }


        #endregion

        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.chart_cms_qm_click.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void export_image_qm_Click(object sender, EventArgs e)
        {

            this.tChart_qm.Export.ShowExportDialog();
        }

        private string group = Guid.NewGuid().ToString();

        private void btn_datadispose_Click(object sender, EventArgs e)
        {

            if (this.tabControl1.SelectedTab.Name == "流量原始数据")
            {
                GetDatabaseLevelIndex(QM_TestCount.第一次);
                BindLevelIndex(QM_TestCount.第一次);
                if (AddQMResult(QM_TestCount.第一次))
                {
                    MessageBox.Show("处理完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (this.tabControl1.SelectedTab.Name == "重复流量数据")
            {
                GetDatabaseLevelIndex(QM_TestCount.第二次);
                BindLevelIndex(QM_TestCount.第二次);
                if (AddQMResult(QM_TestCount.第二次))
                {
                    MessageBox.Show("处理完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 添加气密结果
        /// </summary>
        /// <returns></returns>
        private bool AddQMResult(QM_TestCount qm_TestCount)
        {

            var sjzValue = txt_ycjy_z.Text;
            DAL_dt_qm_Info dal = new DAL_dt_qm_Info();

            Model_dt_qm_Info model = new Model_dt_qm_Info();
            model.testcount = (int)qm_TestCount;
            model.testtype = int.Parse(sjzValue) > 0 ? "2" : "1";

            for (int i = 0; i < 2; i++)
            {
                var desc = this.dgv_levelIndex.Rows[i].Cells["Quantity"].Value.ToString();
                model.info_DangH = _tempTong;
                model.dt_Code = _tempCode;
                if (desc == "单位缝长")
                {
                    model.qm_Z_FC = double.Parse(this.dgv_levelIndex.Rows[i].Cells["PressureZ"].Value.ToString()).ToString("f2");
                    model.qm_F_FC = double.Parse(this.dgv_levelIndex.Rows[i].Cells["PressureF"].Value.ToString()).ToString("f2");
                }
                else if (desc == "单位面积")
                {
                    model.qm_Z_MJ = double.Parse(this.dgv_levelIndex.Rows[i].Cells["PressureZ"].Value.ToString()).ToString("f2");
                    model.qm_F_MJ = double.Parse(this.dgv_levelIndex.Rows[i].Cells["PressureF"].Value.ToString()).ToString("f2");
                }
            }

            for (int i = 0; i < 12; i++)
            {
                if (qm_TestCount == QM_TestCount.第一次)
                {
                    #region 赋值
                    if (i == 0)
                    {
                        model.qm_s_z_fj10 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd10 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj10 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd10 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 1)
                    {
                        model.qm_s_z_fj30 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd30 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj30 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd30 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 2)
                    {
                        model.qm_s_z_fj50 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd50 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj50 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd50 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 3)
                    {
                        model.qm_s_z_fj70 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd70 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj70 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd70 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 4)
                    {
                        model.qm_s_z_fj100 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd100 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj100 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd100 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 5)
                    {
                        model.qm_s_z_fj150 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd150 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj150 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd150 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 6)
                    {
                        model.qm_j_z_fj100 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_z_zd100 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_f_fj100 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_j_f_zd100 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 7)
                    {
                        model.qm_j_z_fj70 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_z_zd70 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_f_fj70 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_j_f_zd70 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 8)
                    {
                        model.qm_j_z_fj50 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_z_zd50 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_f_fj50 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_j_f_zd50 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 9)
                    {
                        model.qm_j_z_fj30 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_z_zd30 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_f_fj30 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_j_f_zd30 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 10)
                    {
                        model.qm_j_z_fj10 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_z_zd10 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_f_fj10 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_j_f_zd10 = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 11)
                    {
                        model.sjz_value = txt_ycjy_z.Text;
                        model.sjz_z_fj = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.sjz_z_zd = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.sjz_f_fj = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.sjz_f_zd = double.Parse(this.dgv_ll.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    #endregion
                }
                else if (qm_TestCount == QM_TestCount.第二次)
                {
                    #region 赋值
                    if (i == 0)
                    {
                        model.qm_s_z_fj10 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd10 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj10 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd10 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 1)
                    {
                        model.qm_s_z_fj30 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd30 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj30 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd30 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 2)
                    {
                        model.qm_s_z_fj50 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd50 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj50 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd50 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 3)
                    {
                        model.qm_s_z_fj70 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd70 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj70 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd70 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 4)
                    {
                        model.qm_s_z_fj100 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd100 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj100 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd100 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 5)
                    {
                        model.qm_s_z_fj150 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_z_zd150 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_s_f_fj150 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_s_f_zd150 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 6)
                    {
                        model.qm_j_z_fj100 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_z_zd100 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_f_fj100 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_j_f_zd100 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 7)
                    {
                        model.qm_j_z_fj70 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_z_zd70 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_f_fj70 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_j_f_zd70 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 8)
                    {
                        model.qm_j_z_fj50 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_z_zd50 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_f_fj50 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_j_f_zd50 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 9)
                    {
                        model.qm_j_z_fj30 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_z_zd30 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_f_fj30 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_j_f_zd30 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 10)
                    {
                        model.qm_j_z_fj10 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_z_zd10 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.qm_j_f_fj10 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.qm_j_f_zd10 = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    if (i == 11)
                    {
                        model.sjz_value = txt_ycjy_z.Text;
                        model.sjz_z_fj = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z"].Value.ToString()).ToString("f2");
                        model.sjz_z_zd = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()).ToString("f2");
                        model.sjz_f_fj = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F"].Value.ToString()).ToString("f2");
                        model.sjz_f_zd = double.Parse(this.dgv_ll2.Rows[i].Cells["Pressure_F_Z"].Value.ToString()).ToString("f2");
                    }
                    #endregion
                }
            }
            return dal.Add(model);
        }


        /// <summary>
        /// 获取分级指标
        /// </summary>
        /// <param name="zFc">正压缝长</param>
        /// <param name="fFc">负压缝长</param>
        /// <param name="zMj">正压面积</param>
        /// <param name="fMj">负压面积</param>
        private void GetDatabaseLevelIndex(QM_TestCount qm_TestCount)
        {
            double kPa = 0;
            double tempTemperature = 0;
            double stitchLength = 0;
            double sumArea = 0;

            DataTable dt = new DAL_dt_Settings().Getdt_SettingsByCode(_tempCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                kPa = double.Parse(dt.Rows[0]["DaQiYaLi"].ToString());
                tempTemperature = double.Parse(dt.Rows[0]["DangQianWenDu"].ToString());
                stitchLength = double.Parse(dt.Rows[0]["KaiQiFengChang"].ToString());
                sumArea = double.Parse(dt.Rows[0]["shijianmianji"].ToString());
            }
            List<AirtightCalculation> airtightCalculation = new List<AirtightCalculation>();
            if (qm_TestCount == QM_TestCount.第一次)
            {
                if (this.dgv_ll.Rows[11].Cells["Pressure_Z_Z"].Value.ToString().ToString() != "0" || this.dgv_ll.Rows[11].Cells["Pressure_F_Z"].Value.ToString().ToString() != "0")
                {
                    zFc = Formula.GetIndexStitchLengthAndArea(
                        double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_Z_Z"].Value.ToString()),
                        double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_Z"].Value.ToString()),
                        true, kPa, tempTemperature, stitchLength, sumArea);

                    fFc = Formula.GetIndexStitchLengthAndArea(
                        double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_F_Z"].Value.ToString()),
                        double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_F"].Value.ToString()),
                         true, kPa, tempTemperature, stitchLength, sumArea);

                    zMj = Formula.GetIndexStitchLengthAndArea(
                        double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_Z_Z"].Value.ToString()),
                        double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_Z"].Value.ToString()),
                        false, kPa, tempTemperature, stitchLength, sumArea);

                    fMj = Formula.GetIndexStitchLengthAndArea(
                        double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_F_Z"].Value.ToString()),
                        double.Parse(this.dgv_ll.Rows[11].Cells["Pressure_F"].Value.ToString()),
                        false, kPa, tempTemperature, stitchLength, sumArea);
                }
                else
                {
                    airtightCalculation.Add(new AirtightCalculation()
                    {
                        PaValue = 10,
                        Z_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[0].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[10].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_S_FJ_Value = double.Parse(this.dgv_ll.Rows[0].Cells["Pressure_Z"].Value.ToString()),
                        Z_J_FJ_Value = double.Parse(this.dgv_ll.Rows[10].Cells["Pressure_Z"].Value.ToString()),

                        F_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[0].Cells["Pressure_F_Z"].Value.ToString()),
                        F_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[10].Cells["Pressure_F_Z"].Value.ToString()),
                        F_S_FJ_Value = double.Parse(this.dgv_ll.Rows[0].Cells["Pressure_F"].Value.ToString()),
                        F_J_FJ_Value = double.Parse(this.dgv_ll.Rows[10].Cells["Pressure_F"].Value.ToString()),

                        // _Z_Q_SJ_P=17.478,
                        kPa = kPa,
                        CurrentTemperature = tempTemperature
                    });
                    airtightCalculation.Add(new AirtightCalculation()
                    {
                        PaValue = 30,
                        Z_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[1].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[9].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_S_FJ_Value = double.Parse(this.dgv_ll.Rows[1].Cells["Pressure_Z"].Value.ToString()),
                        Z_J_FJ_Value = double.Parse(this.dgv_ll.Rows[9].Cells["Pressure_Z"].Value.ToString()),

                        F_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[1].Cells["Pressure_F_Z"].Value.ToString()),
                        F_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[9].Cells["Pressure_F_Z"].Value.ToString()),
                        F_S_FJ_Value = double.Parse(this.dgv_ll.Rows[1].Cells["Pressure_F"].Value.ToString()),
                        F_J_FJ_Value = double.Parse(this.dgv_ll.Rows[9].Cells["Pressure_F"].Value.ToString()),
                        //  _Z_Q_SJ_P = 22.062,
                        kPa = kPa,
                        CurrentTemperature = tempTemperature
                    });
                    airtightCalculation.Add(new AirtightCalculation()
                    {
                        PaValue = 50,
                        Z_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[2].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[8].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_S_FJ_Value = double.Parse(this.dgv_ll.Rows[2].Cells["Pressure_Z"].Value.ToString()),
                        Z_J_FJ_Value = double.Parse(this.dgv_ll.Rows[8].Cells["Pressure_Z"].Value.ToString()),

                        F_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[2].Cells["Pressure_F_Z"].Value.ToString()),
                        F_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[8].Cells["Pressure_F_Z"].Value.ToString()),
                        F_S_FJ_Value = double.Parse(this.dgv_ll.Rows[2].Cells["Pressure_F"].Value.ToString()),
                        F_J_FJ_Value = double.Parse(this.dgv_ll.Rows[8].Cells["Pressure_F"].Value.ToString()),
                        kPa = kPa,
                        // _Z_Q_SJ_P = 25.786,
                        CurrentTemperature = tempTemperature
                    });
                    airtightCalculation.Add(new AirtightCalculation()
                    {
                        PaValue = 70,
                        Z_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[3].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[7].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_S_FJ_Value = double.Parse(this.dgv_ll.Rows[3].Cells["Pressure_Z"].Value.ToString()),
                        Z_J_FJ_Value = double.Parse(this.dgv_ll.Rows[7].Cells["Pressure_Z"].Value.ToString()),

                        F_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[3].Cells["Pressure_F_Z"].Value.ToString()),
                        F_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[7].Cells["Pressure_F_Z"].Value.ToString()),
                        F_S_FJ_Value = double.Parse(this.dgv_ll.Rows[3].Cells["Pressure_F"].Value.ToString()),
                        F_J_FJ_Value = double.Parse(this.dgv_ll.Rows[7].Cells["Pressure_F"].Value.ToString()),
                        kPa = kPa,
                        //  _Z_Q_SJ_P = 35.815,
                        CurrentTemperature = tempTemperature
                    });
                    airtightCalculation.Add(new AirtightCalculation()
                    {
                        PaValue = 100,
                        Z_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_S_FJ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_Z"].Value.ToString()),
                        Z_J_FJ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_Z"].Value.ToString()),

                        F_S_ZZ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_F_Z"].Value.ToString()),
                        F_J_ZZ_Value = double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_F_Z"].Value.ToString()),
                        F_S_FJ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_F"].Value.ToString()),
                        F_J_FJ_Value = double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_F"].Value.ToString()),
                        kPa = kPa,
                        // _Z_Q_SJ_P = 63.893,
                        CurrentTemperature = tempTemperature
                    });

                    //获取分级指标
                    var indexStitchLengthAndArea = Formula.GetJK_IndexStitchLengthAndArea(airtightCalculation, stitchLength, sumArea);
                    if (indexStitchLengthAndArea != null)
                    {
                        zFc = indexStitchLengthAndArea.ZY_FC;
                        fFc = indexStitchLengthAndArea.FY_FC;
                        zMj = indexStitchLengthAndArea.ZY_MJ;
                        fMj = indexStitchLengthAndArea.FY_MJ;
                    }
                }

            }
            else if (qm_TestCount == QM_TestCount.第二次)
            {
                if (this.dgv_ll2.Rows[11].Cells["Pressure_Z_Z"].Value.ToString().ToString() != "0" || this.dgv_ll2.Rows[11].Cells["Pressure_F_Z"].Value.ToString().ToString() != "0")
                {
                    zFc = Formula.GetIndexStitchLengthAndArea(
                         double.Parse(this.dgv_ll2.Rows[11].Cells["Pressure_Z_Z"].Value.ToString()),
                         double.Parse(this.dgv_ll2.Rows[11].Cells["Pressure_Z_F"].Value.ToString()),
                         true, kPa, tempTemperature, stitchLength, sumArea);

                    fFc = Formula.GetIndexStitchLengthAndArea(
                        double.Parse(this.dgv_ll2.Rows[11].Cells["Pressure_F_Z"].Value.ToString()),
                        double.Parse(this.dgv_ll2.Rows[11].Cells["Pressure_F_F"].Value.ToString()),
                         true, kPa, tempTemperature, stitchLength, sumArea);

                    zMj = Formula.GetIndexStitchLengthAndArea(
                        double.Parse(this.dgv_ll2.Rows[11].Cells["Pressure_Z_Z"].Value.ToString()),
                        double.Parse(this.dgv_ll2.Rows[11].Cells["Pressure_Z_F"].Value.ToString()),
                        false, kPa, tempTemperature, stitchLength, sumArea);

                    fMj = Formula.GetIndexStitchLengthAndArea(
                        double.Parse(this.dgv_ll2.Rows[11].Cells["Pressure_F_Z"].Value.ToString()),
                        double.Parse(this.dgv_ll2.Rows[11].Cells["Pressure_F_F"].Value.ToString()),
                        false, kPa, tempTemperature, stitchLength, sumArea);
                }
                else
                {
                    airtightCalculation.Add(new AirtightCalculation()
                    {
                        PaValue = 10,
                        Z_S_ZZ_Value = double.Parse(this.dgv_ll2.Rows[0].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_J_ZZ_Value = double.Parse(this.dgv_ll2.Rows[10].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_S_FJ_Value = double.Parse(this.dgv_ll2.Rows[0].Cells["Pressure_Z"].Value.ToString()),
                        Z_J_FJ_Value = double.Parse(this.dgv_ll2.Rows[10].Cells["Pressure_Z"].Value.ToString()),

                        F_S_ZZ_Value = double.Parse(this.dgv_ll2.Rows[0].Cells["Pressure_F_Z"].Value.ToString()),
                        F_J_ZZ_Value = double.Parse(this.dgv_ll2.Rows[10].Cells["Pressure_F_Z"].Value.ToString()),
                        F_S_FJ_Value = double.Parse(this.dgv_ll2.Rows[0].Cells["Pressure_F"].Value.ToString()),
                        F_J_FJ_Value = double.Parse(this.dgv_ll2.Rows[10].Cells["Pressure_F"].Value.ToString()),
                        kPa = kPa,
                        CurrentTemperature = tempTemperature
                    });
                    airtightCalculation.Add(new AirtightCalculation()
                    {
                        PaValue = 30,
                        Z_S_ZZ_Value = double.Parse(this.dgv_ll2.Rows[1].Cells["Pressure_F_Z"].Value.ToString()),
                        Z_J_ZZ_Value = double.Parse(this.dgv_ll2.Rows[9].Cells["Pressure_F_Z"].Value.ToString()),
                        Z_S_FJ_Value = double.Parse(this.dgv_ll2.Rows[1].Cells["Pressure_F"].Value.ToString()),
                        Z_J_FJ_Value = double.Parse(this.dgv_ll2.Rows[9].Cells["Pressure_F"].Value.ToString()),

                        F_S_ZZ_Value = double.Parse(this.dgv_ll2.Rows[1].Cells["Pressure_F_Z"].Value.ToString()),
                        F_J_ZZ_Value = double.Parse(this.dgv_ll2.Rows[9].Cells["Pressure_F_Z"].Value.ToString()),
                        F_S_FJ_Value = double.Parse(this.dgv_ll2.Rows[1].Cells["Pressure_F"].Value.ToString()),
                        F_J_FJ_Value = double.Parse(this.dgv_ll2.Rows[9].Cells["Pressure_F"].Value.ToString()),
                        kPa = kPa,
                        CurrentTemperature = tempTemperature
                    });
                    airtightCalculation.Add(new AirtightCalculation()
                    {
                        PaValue = 50,
                        Z_S_ZZ_Value = double.Parse(this.dgv_ll2.Rows[2].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_J_ZZ_Value = double.Parse(this.dgv_ll2.Rows[8].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_S_FJ_Value = double.Parse(this.dgv_ll2.Rows[2].Cells["Pressure_Z"].Value.ToString()),
                        Z_J_FJ_Value = double.Parse(this.dgv_ll2.Rows[8].Cells["Pressure_Z"].Value.ToString()),

                        F_S_ZZ_Value = double.Parse(this.dgv_ll2.Rows[2].Cells["Pressure_F_Z"].Value.ToString()),
                        F_J_ZZ_Value = double.Parse(this.dgv_ll2.Rows[8].Cells["Pressure_F_Z"].Value.ToString()),
                        F_S_FJ_Value = double.Parse(this.dgv_ll2.Rows[2].Cells["Pressure_F"].Value.ToString()),
                        F_J_FJ_Value = double.Parse(this.dgv_ll2.Rows[8].Cells["Pressure_F"].Value.ToString()),
                        kPa = kPa,
                        CurrentTemperature = tempTemperature
                    });
                    airtightCalculation.Add(new AirtightCalculation()
                    {
                        PaValue = 70,
                        Z_S_ZZ_Value = double.Parse(this.dgv_ll2.Rows[3].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_J_ZZ_Value = double.Parse(this.dgv_ll2.Rows[7].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_S_FJ_Value = double.Parse(this.dgv_ll2.Rows[3].Cells["Pressure_Z"].Value.ToString()),
                        Z_J_FJ_Value = double.Parse(this.dgv_ll2.Rows[7].Cells["Pressure_Z"].Value.ToString()),

                        F_S_ZZ_Value = double.Parse(this.dgv_ll2.Rows[3].Cells["Pressure_F_Z"].Value.ToString()),
                        F_J_ZZ_Value = double.Parse(this.dgv_ll2.Rows[7].Cells["Pressure_F_Z"].Value.ToString()),
                        F_S_FJ_Value = double.Parse(this.dgv_ll2.Rows[3].Cells["Pressure_F"].Value.ToString()),
                        F_J_FJ_Value = double.Parse(this.dgv_ll2.Rows[7].Cells["Pressure_F"].Value.ToString()),
                        kPa = kPa,
                        CurrentTemperature = tempTemperature
                    });
                    airtightCalculation.Add(new AirtightCalculation()
                    {
                        PaValue = 100,
                        Z_S_ZZ_Value = double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_J_ZZ_Value = double.Parse(this.dgv_ll2.Rows[6].Cells["Pressure_Z_Z"].Value.ToString()),
                        Z_S_FJ_Value = double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_Z"].Value.ToString()),
                        Z_J_FJ_Value = double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_Z"].Value.ToString()),

                        F_S_ZZ_Value = double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_F_Z"].Value.ToString()),
                        F_J_ZZ_Value = double.Parse(this.dgv_ll2.Rows[6].Cells["Pressure_F_Z"].Value.ToString()),
                        F_S_FJ_Value = double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_F"].Value.ToString()),
                        F_J_FJ_Value = double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_F"].Value.ToString()),
                        kPa = kPa,
                        CurrentTemperature = tempTemperature
                    });

                    //获取分级指标
                    var indexStitchLengthAndArea = Formula.GetJK_IndexStitchLengthAndArea(airtightCalculation, stitchLength, sumArea);
                    if (indexStitchLengthAndArea != null)
                    {
                        zFc = indexStitchLengthAndArea.ZY_FC;
                        fFc = indexStitchLengthAndArea.FY_FC;
                        zMj = indexStitchLengthAndArea.ZY_MJ;
                        fMj = indexStitchLengthAndArea.FY_MJ;
                    }
                }
            }


            #region  old
            //if (qm_TestCount == QM_TestCount.第一次)
            //{
            //    zFc = Formula.GetIndexStitchLengthAndArea(
            //        double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_Z_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_Z_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_Z"].Value.ToString()),
            //        true, kPa, tempTemperature, stitchLength, sumArea);

            //    fFc = Formula.GetIndexStitchLengthAndArea(
            //        double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_F_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_F"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_F_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_F"].Value.ToString()),
            //         true, kPa, tempTemperature, stitchLength, sumArea);

            //    zMj = Formula.GetIndexStitchLengthAndArea(
            //        double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_Z_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_Z_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_Z"].Value.ToString()),
            //         false, kPa, tempTemperature, stitchLength, sumArea);

            //    fMj = Formula.GetIndexStitchLengthAndArea(
            //        double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_F_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[4].Cells["Pressure_F"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_F_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll.Rows[6].Cells["Pressure_F"].Value.ToString()),
            //         false, kPa, tempTemperature, stitchLength, sumArea);
            //}
            //else if (qm_TestCount == QM_TestCount.第二次)
            //{
            //    zFc = Formula.GetIndexStitchLengthAndArea(
            //         double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_Z_Z"].Value.ToString()),
            //         double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_Z"].Value.ToString()),
            //         double.Parse(this.dgv_ll2.Rows[6].Cells["Pressure_Z_Z"].Value.ToString()),
            //         double.Parse(this.dgv_ll2.Rows[6].Cells["Pressure_Z"].Value.ToString()),
            //         true, kPa, tempTemperature, stitchLength, sumArea);

            //    fFc = Formula.GetIndexStitchLengthAndArea(
            //        double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_F_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_F"].Value.ToString()),
            //        double.Parse(this.dgv_ll2.Rows[6].Cells["Pressure_F_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll2.Rows[6].Cells["Pressure_F"].Value.ToString()),
            //         true, kPa, tempTemperature, stitchLength, sumArea);

            //    zMj = Formula.GetIndexStitchLengthAndArea(
            //        double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_Z_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll2.Rows[6].Cells["Pressure_Z_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll2.Rows[6].Cells["Pressure_Z"].Value.ToString()),
            //         false, kPa, tempTemperature, stitchLength, sumArea);

            //    fMj = Formula.GetIndexStitchLengthAndArea(
            //        double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_F_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll2.Rows[4].Cells["Pressure_F"].Value.ToString()),
            //        double.Parse(this.dgv_ll2.Rows[6].Cells["Pressure_F_Z"].Value.ToString()),
            //        double.Parse(this.dgv_ll2.Rows[6].Cells["Pressure_F"].Value.ToString()),
            //         false, kPa, tempTemperature, stitchLength, sumArea);
            //}
            #endregion
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.Stop();
            if (!res)
                return;
            this.btn_justready.Enabled = true;
            this.btn_loseready.Enabled = true;
            this.btn_losestart.Enabled = true;
            this.btn_datadispose.Enabled = true;
            this.btn_juststart.Enabled = true;

            btn_datadispose.BackColor = Color.Transparent;
            btn_justready.BackColor = Color.Transparent;
            btn_loseready.BackColor = Color.Transparent;
            btn_losestart.BackColor = Color.Transparent;
            btn_juststart.BackColor = Color.Transparent;

            this.tim_Top10.Enabled = false;
            BindFlowBase();
            airtightPropertyTest = PublicEnum.AirtightPropertyTest.Stop;
        }

        /// <summary>
        /// 控制气密性能检测按钮显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tim_getType_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            if (airtightPropertyTest == null) { return; }

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZReady)
            {
                int value = _serialPortClient.GetZYYBJS();
                if (value == 3)
                {
                    airtightPropertyTest = PublicEnum.AirtightPropertyTest.Stop;
                    //lbl_setYL.Text = "0";
                    OpenBtnType();
                }
            }
            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZStart)
            {
                double value = _serialPortClient.GetZYKSJS();

                if (value >= 15)
                {
                    airtightPropertyTest = PublicEnum.AirtightPropertyTest.Stop;
                    //IsStart = false;
                    //lbl_setYL.Text = "0";
                    OpenBtnType();
                }
            }

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.FReady)
            {
                int value = _serialPortClient.GetFYYBJS();

                if (value == 3)
                {
                    airtightPropertyTest = PublicEnum.AirtightPropertyTest.Stop;
                    //lbl_setYL.Text = "0";
                    OpenBtnType();
                }
            }

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.FStart)
            {
                double value = _serialPortClient.GetFYKSJS();

                if (value >= 15)
                {
                    airtightPropertyTest = PublicEnum.AirtightPropertyTest.Stop;
                    //IsStart = false;
                    //lbl_setYL.Text = "0";
                    OpenBtnType();
                }
            }
        }

        private void gv_list_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (this.tabControl1.SelectedTab.Name == "流量原始数据")
                //{
                //    BindFlowBase(QM_TestCount.第一次);
                //}
                //else if (this.tabControl1.SelectedTab.Name == "重复流量数据")
                //{
                //    BindFlowBase(QM_TestCount.第二次);
                //}

                if (this.tabControl1.SelectedTab.Name == "流量原始数据")
                {
                    var oneList = pressure_One.GetPressure();

                    for (int i = 0; i < oneList.Count; i++)
                    {
                        if (oneList[i].Pressure_F > 0)
                        {
                            pressureList[i].Pressure_F = oneList[i].Pressure_F;
                        }
                        if (oneList[i].Pressure_F_Z > 0)
                        {
                            pressureList[i].Pressure_F_Z = oneList[i].Pressure_F_Z;
                        }
                        if (oneList[i].Pressure_Z > 0)
                        {
                            pressureList[i].Pressure_Z = oneList[i].Pressure_Z;
                        }
                        if (oneList[i].Pressure_Z_Z > 0)
                        {
                            pressureList[i].Pressure_Z_Z = oneList[i].Pressure_Z_Z;
                        }
                    }
                }
                else if (this.tabControl1.SelectedTab.Name == "重复流量数据")
                {
                    var twoList = pressure_Two.GetPressure();
                    for (int i = 0; i < twoList.Count; i++)
                    {
                        if (twoList[i].Pressure_F > 0)
                        {
                            pressureList[i].Pressure_F = twoList[i].Pressure_F;
                        }
                        if (twoList[i].Pressure_F_Z > 0)
                        {
                            pressureList[i].Pressure_F_Z = twoList[i].Pressure_F_Z;
                        }
                        if (twoList[i].Pressure_Z > 0)
                        {
                            pressureList[i].Pressure_Z = twoList[i].Pressure_Z;
                        }
                        if (twoList[i].Pressure_Z_Z > 0)
                        {
                            pressureList[i].Pressure_Z_Z = twoList[i].Pressure_Z_Z;
                        }
                    }
                }
                BindFlowBase();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void Clear()
        {
            pressure_One = new Pressure();
            pressure_Two = new Pressure();
            airtightPropertyTest = null;
            tim_Top10.Enabled = false;
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.tabControl1.SelectedTab.Name == "流量原始数据")
            //{
            //    BindFlowBase(QM_TestCount.第一次);
            //    BindLevelIndex(QM_TestCount.第一次);
            //}
            //else if (this.tabControl1.SelectedTab.Name == "重复流量数据")
            //{
            //    BindFlowBase(QM_TestCount.第二次);
            //    BindLevelIndex(QM_TestCount.第二次);
            //}

            if (this.tabControl1.SelectedTab.Name == "流量原始数据")
            {
                GetPressureFlow(QM_TestCount.第一次);
            }
            else if (this.tabControl1.SelectedTab.Name == "重复流量数据")
            {
                GetPressureFlow(QM_TestCount.第二次);
            }
            BindFlowBase();
        }

        private void btn_ycjy_z_Click(object sender, EventArgs e)
        {
            index = 0;
            this.btn_ycjy_z.Enabled = false;
            int value = 0;
            int.TryParse(txt_ycjy_z.Text, out value);

            if (value == 0)
            {
                this.btn_ycjy_z.Enabled = true;
                return;
            }
            var res = _serialPortClient.Set_FY_Value(BFMCommand.正依次加压值, BFMCommand.正依次加压, value);
            if (!res)
            {
                MessageBox.Show("正依次加压！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //重复做
            if (rdb_fjstl.Checked)
            {
                foreach (var item in pressureList)
                {
                    item.Pressure_Z = 0;
                }
            }
            else if (rdb_zdstl.Checked)
            {
                foreach (var item in pressureList)
                {
                    item.Pressure_Z_Z = 0;
                }

            }
            BindFlowBase();


            //本程序控制
            btn_ycjy_z.BackColor = Color.Green;

            airtightPropertyTest = PublicEnum.AirtightPropertyTest.ZYCJY;


            //关闭监控按钮
            this.btn_justready.Enabled = false;
            this.btn_loseready.Enabled = false;
            this.btn_losestart.Enabled = false;
            this.btn_juststart.Enabled = false;


            btn_justready.BackColor = Color.Transparent;
            btn_loseready.BackColor = Color.Transparent;
            btn_losestart.BackColor = Color.Transparent;
            btn_juststart.BackColor = Color.Transparent;
        }

        private void btn_ycjyf_Click(object sender, EventArgs e)
        {
            index = 0;
            this.btn_ycjyf.Enabled = false;
            int value = 0;

            int.TryParse(txt_ycjy_f.Text, out value);

            if (value == 0)
            {
                this.btn_ycjyf.Enabled = true;
                return;
            }

            var res = _serialPortClient.Set_FY_Value(BFMCommand.负依次加压值, BFMCommand.负依次加压, value);
            if (!res)
            {
                MessageBox.Show("负依次加压异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rdb_fjstl.Checked)
            {
                foreach (var item in pressureList)
                {
                    item.Pressure_F = 0;
                }
            }
            else if (rdb_zdstl.Checked)
            {
                foreach (var item in pressureList)
                {
                    item.Pressure_F_Z = 0;
                }
            }
            BindFlowBase();

            //本程序控制
            btn_ycjyf.BackColor = Color.Green;
            airtightPropertyTest = PublicEnum.AirtightPropertyTest.FYCJY;

            //关闭监控按钮
            this.btn_justready.Enabled = false;
            this.btn_loseready.Enabled = false;
            this.btn_losestart.Enabled = false;
            this.btn_juststart.Enabled = false;

            btn_justready.BackColor = Color.Transparent;
            btn_loseready.BackColor = Color.Transparent;
            btn_losestart.BackColor = Color.Transparent;
            btn_juststart.BackColor = Color.Transparent;
        }

        private void tim_getsjz_Tick(object sender, EventArgs e)
        {

        }

        private void txt_ycjy_z_TextChanged(object sender, EventArgs e)
        {
            txt_ycjy_f.Text = txt_ycjy_z.Text;

        }

        private void txt_ycjy_f_TextChanged(object sender, EventArgs e)
        {
            txt_ycjy_z.Text = txt_ycjy_f.Text;
        }

        private void tChart_sm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.chart_cms_qm_click.Show(MousePosition.X, MousePosition.Y);
            }
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.tChart_qm.Export.ShowExportDialog();
        }


        private void btn_exit_Click(object sender, EventArgs e)
        {
            var res = _serialPortClient.Stop();

            // MessageBox.Show("急停异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //Stop();
            this.Close();
        }
    }


    public class ReadT
    {
        public string Key { get; set; }
        public int Order { get; set; }
        public bool IsRead { get; set; }
    }
}
