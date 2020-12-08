
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
        Pressure pressure = new Pressure();
        /// <summary>
        /// 气密数据位置
        /// </summary>
        private PublicEnum.AirtightPropertyTest? airtightPropertyTest = null;

        /// <summary>
        /// 分级指标
        /// </summary>
        double zFc = 0, fFc = 0, zMj = 0, fMj = 0;

        public List<ReadT> _readT = new List<ReadT>();
        /// <summary>
        /// 操作状态
        /// </summary>
        private bool IsSeccess = false;

        private bool IsFirst = true;


        public DateTime dtnow { get; set; }
        public AirtightDetection()
        {
            //dgv_ll.Height = 410;
        }

        public AirtightDetection(SerialPortClient tcpClient, string tempCode, string tempTong)
        {
            InitializeComponent();
            this._serialPortClient = tcpClient;
            this._tempCode = tempCode;
            this._tempTong = tempTong;

            pressure = new Pressure();
            Init();
        }

        private void Init()
        {
            BindWindSpeedBase();
            BindLevelIndex();
            BindFlowBase();
            BindSetPressure();
            QMchartInit();

            Clear();
        }

        public void StopTimer()
        {
            this.tim_PainPic.Enabled = false;
            this.tim_qm.Enabled = false;
            this.tim_getType.Enabled = false;
        }
        public void InitTimer()
        {
            this.tim_PainPic.Enabled = true;
            this.tim_qm.Enabled = true;
            this.tim_getType.Enabled = true;
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
        public List<Pressure> GetPressureFlow()
        {
            List<Pressure> pressureList = new List<Pressure>();
            Formula slopeCompute = new Formula();
            for (int i = 0; i < 11; i++)
            {
                Pressure model = new Pressure();
                model.PressurePa = int.Parse(this.dgv_WindSpeed.Rows[i].Cells["PressurePa"].Value.ToString());
                model.Pressure_Z = slopeCompute.MathFlow(double.Parse(this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString()));
                model.Pressure_Z_Z = slopeCompute.MathFlow(double.Parse(this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString()));
                model.Pressure_F = slopeCompute.MathFlow(double.Parse(this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString()));
                model.Pressure_F_Z = slopeCompute.MathFlow(double.Parse(this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString()));
                pressureList.Add(model);
            }
            return pressureList;
        }

        /// <summary>
        /// 绑定流量
        /// </summary>
        private void BindFlowBase()
        {
            dgv_ll.DataSource = GetPressureFlow();

            dgv_ll.RowHeadersVisible = false;
            dgv_ll.AllowUserToResizeColumns = false;
            dgv_ll.AllowUserToResizeRows = false;
            dgv_ll.Columns[0].HeaderText = "压力Pa";
            dgv_ll.Columns[0].Width = 37;
            dgv_ll.Columns[0].ReadOnly = true;
            dgv_ll.Columns[0].DataPropertyName = "PressurePa";
            dgv_ll.Columns[1].HeaderText = "正压附加";
            dgv_ll.Columns[1].Width = 55;
            dgv_ll.Columns[1].DataPropertyName = "Pressure_Z";
            dgv_ll.Columns[2].HeaderText = "正压总的";
            dgv_ll.Columns[2].Width = 55;
            dgv_ll.Columns[2].DataPropertyName = "Pressure_Z_Z";
            dgv_ll.Columns[3].HeaderText = "负压附加";
            dgv_ll.Columns[3].Width = 55;
            dgv_ll.Columns[3].DataPropertyName = "Pressure_F";
            dgv_ll.Columns[4].HeaderText = "负压总的";
            dgv_ll.Columns[4].Width = 55;
            dgv_ll.Columns[4].DataPropertyName = "Pressure_F_Z";

            dgv_ll.Columns["Pressure_Z"].DefaultCellStyle.Format = "N2";
            dgv_ll.Columns["Pressure_Z_Z"].DefaultCellStyle.Format = "N2";
            dgv_ll.Columns["Pressure_F"].DefaultCellStyle.Format = "N2";
            dgv_ll.Columns["Pressure_F_Z"].DefaultCellStyle.Format = "N2";
        }




        /// <summary>
        /// 绑定风速
        /// </summary>
        private void BindWindSpeedBase()
        {
            Model_dt_Settings dt_Settings = new DAL_dt_Settings().GetInfoByCode(_tempCode);
            List<Pressure> pressureList = new List<Pressure>();
            if (dt_Settings.dt_qm_Info != null && dt_Settings.dt_qm_Info.Count > 0)
            {
                var qm = dt_Settings.dt_qm_Info.FindAll(t => t.info_DangH == _tempTong && string.IsNullOrWhiteSpace(t.qm_j_f_zd100) == false).OrderBy(t => t.info_DangH);
                //是否首次加载
                if (IsFirst && (qm != null && qm.Count() > 0))
                {
                    gv_list.Enabled = false;
                    foreach (var item in qm)
                    {

                        Pressure model4 = new Pressure();
                        model4.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj10) ? 0 : double.Parse(item.qm_s_f_fj10);
                        model4.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd10) ? 0 : double.Parse(item.qm_s_f_zd10);

                        model4.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj10) ? 0 : double.Parse(item.qm_s_z_fj10);
                        model4.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd10) ? 0 : double.Parse(item.qm_s_z_zd10);
                        model4.PressurePa = 10;
                        pressureList.Add(model4);

                        Pressure model5 = new Pressure();
                        model5.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj30) ? 0 : double.Parse(item.qm_s_f_fj30);
                        model5.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd30) ? 0 : double.Parse(item.qm_s_f_zd30);

                        model5.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj30) ? 0 : double.Parse(item.qm_s_z_fj30);
                        model5.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd30) ? 0 : double.Parse(item.qm_s_z_zd30);
                        model5.PressurePa = 30;
                        pressureList.Add(model5);

                        Pressure model6 = new Pressure();
                        model6.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj50) ? 0 : double.Parse(item.qm_s_f_fj50);
                        model6.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd50) ? 0 : double.Parse(item.qm_s_f_zd50);

                        model6.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj50) ? 0 : double.Parse(item.qm_s_z_fj50);
                        model6.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd50) ? 0 : double.Parse(item.qm_s_z_zd50);
                        model6.PressurePa = 50;
                        pressureList.Add(model6);

                        Pressure model7 = new Pressure();
                        model7.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj70) ? 0 : double.Parse(item.qm_s_f_fj70);
                        model7.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd70) ? 0 : double.Parse(item.qm_s_f_zd70);

                        model7.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj70) ? 0 : double.Parse(item.qm_s_z_fj70);
                        model7.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd70) ? 0 : double.Parse(item.qm_s_z_zd70);
                        model7.PressurePa = 70;
                        pressureList.Add(model7);


                        Pressure model1 = new Pressure();
                        model1.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj100) ? 0 : double.Parse(item.qm_s_f_fj100);
                        model1.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd100) ? 0 : double.Parse(item.qm_s_f_zd100);

                        model1.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj100) ? 0 : double.Parse(item.qm_s_z_fj100);
                        model1.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd100) ? 0 : double.Parse(item.qm_s_z_zd100);
                        model1.PressurePa = 100;
                        pressureList.Add(model1);

                        Pressure model2 = new Pressure();
                        model2.Pressure_F = string.IsNullOrWhiteSpace(item.qm_s_f_fj150) ? 0 : double.Parse(item.qm_s_f_fj150);
                        model2.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_s_f_zd150) ? 0 : double.Parse(item.qm_s_f_zd150);

                        model2.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_s_z_fj150) ? 0 : double.Parse(item.qm_s_z_fj150);
                        model2.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_s_z_zd150) ? 0 : double.Parse(item.qm_s_z_zd150);
                        model2.PressurePa = 150;
                        pressureList.Add(model2);

                        Pressure model3 = new Pressure();
                        model3.Pressure_F = string.IsNullOrWhiteSpace(item.qm_j_f_fj100) ? 0 : double.Parse(item.qm_j_f_fj100);
                        model3.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_j_f_zd100) ? 0 : double.Parse(item.qm_j_f_zd100);

                        model3.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_j_z_fj100) ? 0 : double.Parse(item.qm_j_z_fj100);
                        model3.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_j_z_zd100) ? 0 : double.Parse(item.qm_j_z_zd100);
                        model3.PressurePa = 100;
                        pressureList.Add(model3);

                        Pressure model8 = new Pressure();
                        model8.Pressure_F = string.IsNullOrWhiteSpace(item.qm_j_f_fj70) ? 0 : double.Parse(item.qm_j_f_fj70);
                        model8.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_j_f_zd70) ? 0 : double.Parse(item.qm_j_f_zd70);

                        model8.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_j_z_fj70) ? 0 : double.Parse(item.qm_j_z_fj70);
                        model8.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_j_z_zd70) ? 0 : double.Parse(item.qm_j_z_zd70);
                        model8.PressurePa = 100;
                        pressureList.Add(model8);

                        Pressure model9 = new Pressure();
                        model9.Pressure_F = string.IsNullOrWhiteSpace(item.qm_j_f_fj50) ? 0 : double.Parse(item.qm_j_f_fj50);
                        model9.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_j_f_zd50) ? 0 : double.Parse(item.qm_j_f_zd50);

                        model9.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_j_z_fj50) ? 0 : double.Parse(item.qm_j_z_fj50);
                        model9.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_j_z_zd50) ? 0 : double.Parse(item.qm_j_z_zd50);
                        model9.PressurePa = 100;
                        pressureList.Add(model9);

                        Pressure model10 = new Pressure();
                        model10.Pressure_F = string.IsNullOrWhiteSpace(item.qm_j_f_fj30) ? 0 : double.Parse(item.qm_j_f_fj30);
                        model10.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_j_f_zd30) ? 0 : double.Parse(item.qm_j_f_zd30);

                        model10.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_j_z_fj30) ? 0 : double.Parse(item.qm_j_z_fj30);
                        model10.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_j_z_zd30) ? 0 : double.Parse(item.qm_j_z_zd30);
                        model10.PressurePa = 100;
                        pressureList.Add(model10);

                        Pressure model11 = new Pressure();
                        model11.Pressure_F = string.IsNullOrWhiteSpace(item.qm_j_f_fj10) ? 0 : double.Parse(item.qm_j_f_fj10);
                        model11.Pressure_F_Z = string.IsNullOrWhiteSpace(item.qm_j_f_zd10) ? 0 : double.Parse(item.qm_j_f_zd10);

                        model11.Pressure_Z = string.IsNullOrWhiteSpace(item.qm_j_z_fj10) ? 0 : double.Parse(item.qm_j_z_fj10);
                        model11.Pressure_Z_Z = string.IsNullOrWhiteSpace(item.qm_j_z_zd10) ? 0 : double.Parse(item.qm_j_z_zd10);
                        model11.PressurePa = 100;
                        pressureList.Add(model11);
                    }
                }
                else
                {
                    pressureList = pressure.GetPressure();
                }
            }
            else
            {
                pressureList = pressure.GetPressure();
            }

            dgv_WindSpeed.DataSource = pressureList;
            dgv_WindSpeed.RowHeadersVisible = false;
            dgv_WindSpeed.AllowUserToResizeColumns = false;
            dgv_WindSpeed.AllowUserToResizeRows = false;
            dgv_WindSpeed.Columns[0].HeaderText = "压力Pa";
            dgv_WindSpeed.Columns[0].Width = 37;
            dgv_WindSpeed.Columns[0].ReadOnly = true;
            dgv_WindSpeed.Columns[0].DataPropertyName = "PressurePa";
            dgv_WindSpeed.Columns[1].HeaderText = "正压附加";
            dgv_WindSpeed.Columns[1].Width = 55;
            dgv_WindSpeed.Columns[1].DataPropertyName = "Pressure_Z";
            dgv_WindSpeed.Columns[2].HeaderText = "正压总的";
            dgv_WindSpeed.Columns[2].Width = 55;
            dgv_WindSpeed.Columns[2].DataPropertyName = "Pressure_Z_Z";
            dgv_WindSpeed.Columns[3].HeaderText = "负压附加";
            dgv_WindSpeed.Columns[3].Width = 55;
            dgv_WindSpeed.Columns[3].DataPropertyName = "Pressure_F";
            dgv_WindSpeed.Columns[4].HeaderText = "负压总的";
            dgv_WindSpeed.Columns[4].Width = 55;
            dgv_WindSpeed.Columns[4].DataPropertyName = "Pressure_F_Z";


            dgv_WindSpeed.Columns["Pressure_Z"].DefaultCellStyle.Format = "N2";
            dgv_WindSpeed.Columns["Pressure_Z_Z"].DefaultCellStyle.Format = "N2";
            dgv_WindSpeed.Columns["Pressure_F"].DefaultCellStyle.Format = "N2";
            dgv_WindSpeed.Columns["Pressure_F_Z"].DefaultCellStyle.Format = "N2";
        }


        /// <summary>
        /// 绑定分级指标
        /// </summary>
        private void BindLevelIndex()
        {
            GetDatabaseLevelIndex();
            dgv_levelIndex.DataSource = GetLevelIndex();
            //dgv_levelIndex.Height = 69;
            dgv_levelIndex.RowHeadersVisible = false;
            dgv_levelIndex.AllowUserToResizeColumns = false;
            dgv_levelIndex.AllowUserToResizeRows = false;
            dgv_levelIndex.Columns[0].HeaderText = "渗透量";
            dgv_levelIndex.Columns[0].Width = 96;
            dgv_levelIndex.Columns[0].ReadOnly = true;
            dgv_levelIndex.Columns[0].DataPropertyName = "Quantity";
            dgv_levelIndex.Columns[1].HeaderText = "正压";
            dgv_levelIndex.Columns[1].Width = 80;
            dgv_levelIndex.Columns[1].DataPropertyName = "PressureZ";
            dgv_levelIndex.Columns[2].HeaderText = "负压";
            dgv_levelIndex.Columns[2].Width = 80;
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
            this.tChart_qm.Axes.Bottom.SetMinMax(dtnow, DateTime.Now.AddSeconds(20));
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
            if (IsStart)
            {
                if (this.tim_Top10.Enabled == false)
                    SetCurrType();
            }

            //读取设定值
            //if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.ZStart)
            //{
            //    double yl = _serialPortClient.GetZYYBYLZ(ref IsSeccess, "ZYKS");
            //    if (!IsSeccess)
            //    {
            //        return;
            //    }
            //    lbl_setYL.Text = yl.ToString();
            //}
            //else if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.FStart)
            //{
            //    double yl = _serialPortClient.GetZYYBYLZ(ref IsSeccess, "FYKS");
            //    if (!IsSeccess)
            //    {
            //        return;
            //    }
            //    lbl_setYL.Text = "-" + yl.ToString();
            //}
            //else if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.Stop)
            //{
            //    lbl_setYL.Text = "0";
            //}

        }

        private void tim_PainPic_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            var c = _serialPortClient.GetCYDXS();

            lbl_dqyl.Text = c.ToString();

            AnimateSeries(this.tChart_qm, c);
        }

        int index = 0;
        private void tim_Top10_Tick(object sender, EventArgs e)
        {
            gv_list.Enabled = true;

            var cyvalue = _serialPortClient.GetCYDXS();

            index++;
            if (index > 4)
            {
                this.tim_Top10.Enabled = false;
                index = 0;
                gv_list.Enabled = false;
                return;
            }


            //获取风速
            var fsvalue = _serialPortClient.GetFSXS();

            if (rdb_fjstl.Checked)
            {
                if (kpa_Level == PublicEnum.Kpa_Level.liter10)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter10);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter10);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter30)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter30);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter30);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter50)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter50);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter50);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter70)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter70);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter70);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter100)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter100);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter100);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter150)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.liter150);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.liter150);

                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop100);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop100);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop70);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop70);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop50)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop50);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop50);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop30)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop30);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop30);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop10)
                {
                    if (cyvalue > 0)
                        pressure.AddZYFJ(fsvalue, PublicEnum.Kpa_Level.drop10);
                    else
                        pressure.AddFYFJ(fsvalue, PublicEnum.Kpa_Level.drop10);
                }
            }
            else if (rdb_zdstl.Checked)
            {
                if (kpa_Level == PublicEnum.Kpa_Level.liter10)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter10);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter10);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter30)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter30);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter30);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter50)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter50);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter50);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter70)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter70);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter70);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter100)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter100);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter100);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.liter150)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.liter150);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.liter150);

                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop100)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop100);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop100);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop70)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop70);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop70);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop50)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop50);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop50);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop30)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop30);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop30);
                }
                else if (kpa_Level == PublicEnum.Kpa_Level.drop10)
                {
                    if (cyvalue > 0)
                        pressure.AddZYZD(fsvalue, PublicEnum.Kpa_Level.drop10);
                    else
                        pressure.AddFYZD(fsvalue, PublicEnum.Kpa_Level.drop10);
                }
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
            }
        }


        #endregion


        #region 气密性能检测按钮事件

        /// <summary>
        /// 判断是否开启正压预备或负压预备
        /// </summary>
        private bool IsYB = false;
        /// <summary>
        /// 判断是否开启正压开始或负压开始
        /// </summary>
        private bool IsStart = false;

        private void btn_justready_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            //double yl = _serialPortClient.GetZYYBYLZ(ref IsSeccess, "ZYYB");
            //if (!IsSeccess)
            //{
            //    MessageBox.Show("读取设定值异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            //    return;
            //}
            //lbl_setYL.Text = yl.ToString();

            IsYB = true;
            DisableBtnType();

            var res = _serialPortClient.SetZYYB();
            if (!res)
            {
                MessageBox.Show("正压预备异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }

            airtightPropertyTest = PublicEnum.AirtightPropertyTest.ZReady;
        }

        /// <summary>
        /// 急停
        /// </summary>
        private void Stop()
        {
            var res = _serialPortClient.Stop();
            if (!res)
                MessageBox.Show("急停异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }

        /// <summary>
        /// 正压开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_juststart_Click(object sender, EventArgs e)
        {


            //double yl = _serialPortClient.GetZYYBYLZ(ref IsSeccess, "ZYKS");
            //if (!IsSeccess)
            //{
            //    MessageBox.Show("读取设定值异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            //    return;
            //}
            //lbl_setYL.Text = yl.ToString();

            IsFirst = false;
            IsStart = true;
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
                new Pressure().ClearZ_F();
            }
            else if (rdb_zdstl.Checked)
            {
                new Pressure().ClearZ_Z();
            }

            _serialPortClient.SendZYKS();

            airtightPropertyTest = PublicEnum.AirtightPropertyTest.ZStart;
        }

        private void btn_loseready_Click(object sender, EventArgs e)
        {
            //double yl = _serialPortClient.GetZYYBYLZ(ref IsSeccess, "FYYB");
            //if (!IsSeccess)
            //{
            //    MessageBox.Show("读取设定值异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            //    return;
            //}
            //lbl_setYL.Text = "-" + yl.ToString();


            IsYB = true;
            DisableBtnType();
            var res = _serialPortClient.SendFYYB();
            if (!res)
            {
                MessageBox.Show("负压预备异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }

            airtightPropertyTest = PublicEnum.AirtightPropertyTest.FReady;
        }
        /// <summary>
        /// 禁用按钮
        /// </summary>
        private void DisableBtnType()
        {
            this.btn_justready.Enabled = false;
            this.btn_loseready.Enabled = false;
            this.btn_losestart.Enabled = false;
            this.btn_datadispose.Enabled = false;
            this.btn_juststart.Enabled = false;
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
        }

        /// <summary>
        /// 负压开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_losestart_Click(object sender, EventArgs e)
        {

            IsFirst = false;
            //double yl = _serialPortClient.GetZYYBYLZ(ref IsSeccess, "FYKS");
            //if (!IsSeccess)
            //{
            //    return;
            //}
            //lbl_setYL.Text = "-" + yl.ToString();

            IsStart = true;
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
                new Pressure().ClearF_F();
            }
            else if (rdb_zdstl.Checked)
            {
                new Pressure().ClearF_Z();
            }
            _serialPortClient.SendFYKS();

            airtightPropertyTest = PublicEnum.AirtightPropertyTest.FStart;
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
            BindFlowBase();
            GetDatabaseLevelIndex();
            BindLevelIndex();
            if (AddQMResult())
            {
                MessageBox.Show("处理完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        /// <summary>
        /// 添加气密结果
        /// </summary>
        /// <returns></returns>
        private bool AddQMResult()
        {
            DAL_dt_qm_Info dal = new DAL_dt_qm_Info();

            Model_dt_qm_Info model = new Model_dt_qm_Info();

            for (int i = 0; i < 2; i++)
            {
                var desc = this.dgv_levelIndex.Rows[i].Cells["Quantity"].Value.ToString();
                model.info_DangH = _tempTong;
                model.dt_Code = _tempCode;
                if (desc == "单位缝长")
                {
                    model.qm_Z_FC = this.dgv_levelIndex.Rows[i].Cells["PressureZ"].Value.ToString();
                    model.qm_F_FC = this.dgv_levelIndex.Rows[i].Cells["PressureF"].Value.ToString();
                }
                else if (desc == "单位面积")
                {
                    model.qm_Z_MJ = this.dgv_levelIndex.Rows[i].Cells["PressureZ"].Value.ToString();
                    model.qm_F_MJ = this.dgv_levelIndex.Rows[i].Cells["PressureF"].Value.ToString();
                }
            }

            for (int i = 0; i < 11; i++)
            {
                if (i == 0)
                {
                    model.qm_s_z_fj10 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_s_z_zd10 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_s_f_fj10 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_s_f_zd10 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }
                if (i == 1)
                {
                    model.qm_s_z_fj30 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_s_z_zd30 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_s_f_fj30 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_s_f_zd30 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }
                if (i == 2)
                {
                    model.qm_s_z_fj50 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_s_z_zd50 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_s_f_fj50 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_s_f_zd50 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }
                if (i == 3)
                {
                    model.qm_s_z_fj70 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_s_z_zd70 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_s_f_fj70 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_s_f_zd70 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }
                if (i == 4)
                {
                    model.qm_s_z_fj100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_s_z_zd100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_s_f_fj100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_s_f_zd100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }
                if (i == 5)
                {
                    model.qm_s_z_fj150 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_s_z_zd150 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_s_f_fj150 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_s_f_zd150 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }
                if (i == 6)
                {
                    model.qm_j_z_fj100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_j_z_zd100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_j_f_fj100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_j_f_zd100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }
                if (i == 7)
                {
                    model.qm_j_z_fj70 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_j_z_zd70 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_j_f_fj70 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_j_f_zd70 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }
                if (i == 8)
                {
                    model.qm_j_z_fj50 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_j_z_zd50 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_j_f_fj50 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_j_f_zd50 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }
                if (i == 9)
                {
                    model.qm_j_z_fj30 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_j_z_zd30 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_j_f_fj30 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_j_f_zd30 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }
                if (i == 10)
                {
                    model.qm_j_z_fj10 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                    model.qm_j_z_zd10 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                    model.qm_j_f_fj10 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                    model.qm_j_f_zd10 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                }

                //if (i == 0)
                //{
                //    model.qm_s_z_fj100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                //    model.qm_s_z_zd100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                //    model.qm_s_f_fj100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                //    model.qm_s_f_zd100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                //}
                //else if (i == 1)
                //{
                //    model.qm_s_z_fj150 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                //    model.qm_s_z_zd150 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                //    model.qm_s_f_fj150 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                //    model.qm_s_f_zd150 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                //}
                //else if (i == 2)
                //{
                //    model.qm_j_z_fj100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z"].Value.ToString();
                //    model.qm_j_z_zd100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_Z_Z"].Value.ToString();
                //    model.qm_j_f_fj100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F"].Value.ToString();
                //    model.qm_j_f_zd100 = this.dgv_WindSpeed.Rows[i].Cells["Pressure_F_Z"].Value.ToString();
                //}
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
        private void GetDatabaseLevelIndex()
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
            var pressureFlow = GetPressureFlow();
            zFc = Formula.GetIndexStitchLengthAndArea(pressureFlow[0].Pressure_Z_Z, pressureFlow[0].Pressure_Z, pressureFlow[2].Pressure_Z_Z, pressureFlow[2].Pressure_Z, true, kPa, tempTemperature, stitchLength, sumArea);

            fFc = Formula.GetIndexStitchLengthAndArea(pressureFlow[0].Pressure_F_Z, pressureFlow[0].Pressure_F, pressureFlow[2].Pressure_F_Z, pressureFlow[2].Pressure_F, true, kPa, tempTemperature, stitchLength, sumArea);

            zMj = Formula.GetIndexStitchLengthAndArea(pressureFlow[0].Pressure_Z_Z, pressureFlow[0].Pressure_Z, pressureFlow[2].Pressure_Z_Z, pressureFlow[2].Pressure_Z, false, kPa, tempTemperature, stitchLength, sumArea);

            fMj = Formula.GetIndexStitchLengthAndArea(pressureFlow[0].Pressure_F_Z, pressureFlow[0].Pressure_F, pressureFlow[2].Pressure_F_Z, pressureFlow[2].Pressure_F, false, kPa, tempTemperature, stitchLength, sumArea);
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Stop();
            this.btn_justready.Enabled = true;
            this.btn_loseready.Enabled = true;
            this.btn_losestart.Enabled = true;
            this.btn_datadispose.Enabled = true;
            this.btn_juststart.Enabled = true;
            this.tim_Top10.Enabled = false;
            //lbl_setYL.Text = "0";
            BindWindSpeedBase();
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
                int value = _serialPortClient.GetZYYBJS(ref IsSeccess);

                if (!IsSeccess)
                {
                    return;
                }
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
                    IsStart = false;
                    Thread.Sleep(1000);
                    //lbl_setYL.Text = "0";
                    OpenBtnType();
                }
            }

            if (airtightPropertyTest == PublicEnum.AirtightPropertyTest.FReady)
            {
                int value = _serialPortClient.GetFYYBJS(ref IsSeccess);

                if (!IsSeccess)
                {
                    return;
                }
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
                    IsStart = false;
                    Thread.Sleep(1000);
                    //lbl_setYL.Text = "0";
                    OpenBtnType();
                }
            }
        }

        private void gv_list_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!_serialPortClient.sp.IsOpen)
                    return;
                BindWindSpeedBase();
                BindFlowBase();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void Clear()
        {
            pressure = new Pressure();
            airtightPropertyTest = null;
            tim_Top10.Enabled = false;
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFlowBase();
        }

        private void tim_readcy_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            var value = _serialPortClient.GetCYDXS();

            lbl_dqyl.Text = value.ToString();
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
            Stop();
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
