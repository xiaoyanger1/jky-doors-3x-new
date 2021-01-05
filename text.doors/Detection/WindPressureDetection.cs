using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Common;
using text.doors.dal;
using text.doors.Default;
using text.doors.Model;
using text.doors.Model.DataBase;
using text.doors.Service;
using Young.Core.Common;

namespace text.doors.Detection
{
    public partial class WindPressureDetection : Form
    {
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        private SerialPortClient _serialPortClient;
        //检验编号
        private string _tempCode = "";
        //当前樘号
        private string _tempTong = "";

        public DateTime dtnow { get; set; }


        private List<int> KFYPa = new List<int>();


        private static int _CYGXS = 0;
        //杆件长度
        private static int ganjianchangdu = 0;


        public List<WindPressureDGV> windPressureDGV = new List<WindPressureDGV>();
        /// <summary>
        /// 抗风压数据位置
        /// </summary>
        private PublicEnum.WindPressureTest? windPressureTest = null;

        public WindPressureDetection()
        { }

        public WindPressureDetection(SerialPortClient tcpClient, string tempCode, string tempTong)
        {
            InitializeComponent();
            this._serialPortClient = tcpClient;
            this._tempCode = tempCode;
            this._tempTong = tempTong;

            if (!DefaultBase.LockPoint)
            {
                rdb_DWDD1.Enabled = false;
                rdb_DWDD3.Enabled = false;
            }
            else
            {
                rdb_DWDD1.Enabled = true;
                rdb_DWDD3.Enabled = true;
            }

            txt_gbjc.Text = _serialPortClient.GetKFYjC().ToString();

            var dt = new DAL_dt_Settings().Getdt_SettingsByCode(_tempCode);

            ganjianchangdu = int.Parse(dt.Rows[0]["ganjianchangdu"].ToString());

            BindData();
            BindSetPressure();
            FYchartInit();


        }

        public void StopTimer()
        {
            this.tim_PainPic.Enabled = false;
            this.tim_wyData.Enabled = false;
            this.tim_btnType.Enabled = false;
        }
        public void InitTimer()
        {
            this.tim_PainPic.Enabled = true;
            this.tim_wyData.Enabled = true;
            this.tim_btnType.Enabled = true;
        }

        /// <summary>
        /// 绑定设定压力
        /// </summary>
        private void BindSetPressure()
        {
            lbl_title.Text = string.Format("门窗抗风压性能检测  第{0}号 {1}", this._tempCode, this._tempTong);
        }

        /// <summary>
        /// 风速图标
        /// </summary>
        private void FYchartInit()
        {
            dtnow = DateTime.Now;
            qm_Line.GetVertAxis.SetMinMax(-8000, 8000);
        }

        private void BindData()
        {
            #region 绑定
            List<WindPressureDGV> list = new List<WindPressureDGV>();
            if (windPressureDGV == null || windPressureDGV.Count == 0)
            {
                list = GetWindPressureDGV();
            }
            else
            {
                list = windPressureDGV;
            }

            dgv_WindPressure.DataSource = list;
            //dgv_WindPressure.Height = 215;
            dgv_WindPressure.RowHeadersVisible = false;
            dgv_WindPressure.AllowUserToResizeColumns = false;
            dgv_WindPressure.AllowUserToResizeRows = false;

            dgv_WindPressure.Columns[0].HeaderText = "国际检测";
            dgv_WindPressure.Columns[0].Width = 85;
            dgv_WindPressure.Columns[0].ReadOnly = true;
            dgv_WindPressure.Columns[0].DataPropertyName = "Pa";


            dgv_WindPressure.Columns[1].HeaderText = "位移1";
            dgv_WindPressure.Columns[1].Width = 75;
            dgv_WindPressure.Columns[1].DataPropertyName = "zwy1";


            dgv_WindPressure.Columns[2].HeaderText = "位移2";
            dgv_WindPressure.Columns[2].Width = 75;
            dgv_WindPressure.Columns[2].DataPropertyName = "zwy2";

            dgv_WindPressure.Columns[3].HeaderText = "位移3";
            dgv_WindPressure.Columns[3].Width = 75;
            dgv_WindPressure.Columns[3].DataPropertyName = "zwy3";

            dgv_WindPressure.Columns[4].HeaderText = "挠度";
            dgv_WindPressure.Columns[4].Width = 75;
            dgv_WindPressure.Columns[4].DataPropertyName = "zzd";

            dgv_WindPressure.Columns[5].HeaderText = "l/X";
            dgv_WindPressure.Columns[5].Width = 75;
            dgv_WindPressure.Columns[5].DataPropertyName = "zlx";



            dgv_WindPressure.Columns[6].HeaderText = "位移1";
            dgv_WindPressure.Columns[6].Width = 75;
            dgv_WindPressure.Columns[6].DataPropertyName = "fwy1";


            dgv_WindPressure.Columns[7].HeaderText = "位移2";
            dgv_WindPressure.Columns[7].Width = 74;
            dgv_WindPressure.Columns[7].DataPropertyName = "fwy2";

            dgv_WindPressure.Columns[8].HeaderText = "位移3";
            dgv_WindPressure.Columns[8].Width = 75;
            dgv_WindPressure.Columns[8].DataPropertyName = "fwy3";

            dgv_WindPressure.Columns[9].HeaderText = "挠度";
            dgv_WindPressure.Columns[9].Width = 75;
            dgv_WindPressure.Columns[9].DataPropertyName = "fzd";

            dgv_WindPressure.Columns[10].HeaderText = "l/X";
            dgv_WindPressure.Columns[10].Width = 75;
            dgv_WindPressure.Columns[10].DataPropertyName = "flx";


            dgv_WindPressure.Columns["zwy1"].DefaultCellStyle.Format = "N2";
            dgv_WindPressure.Columns["zwy2"].DefaultCellStyle.Format = "N2";
            dgv_WindPressure.Columns["zwy3"].DefaultCellStyle.Format = "N2";
            dgv_WindPressure.Columns["zzd"].DefaultCellStyle.Format = "N2";
            //dgv_WindPressure.Columns["zlx"].DefaultCellStyle.Format = "N";

            dgv_WindPressure.Columns["fwy1"].DefaultCellStyle.Format = "N2";
            dgv_WindPressure.Columns["fwy2"].DefaultCellStyle.Format = "N2";
            dgv_WindPressure.Columns["fwy3"].DefaultCellStyle.Format = "N2";
            dgv_WindPressure.Columns["fzd"].DefaultCellStyle.Format = "N2";
            //dgv_WindPressure.Columns["flx"].DefaultCellStyle.Format = "N";


            dgv_WindPressure.Refresh();
            #endregion
        }


        private List<WindPressureDGV> GetWindPressureDGV()
        {
            windPressureDGV = new List<WindPressureDGV>();
            var dt = new DAL_dt_kfy_Info().GetkfyByCodeAndTong(_tempCode, _tempTong);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                //极差
                txt_gbjc.Text = dr["defJC"].ToString();

                var jcvalue = int.Parse(txt_gbjc.Text);
                KFYPa = new List<int>();
                for (int i = 1; i < 9; i++)
                {
                    KFYPa.Add((jcvalue * i));
                }

                //绑定锁点
                if (dr["CheckLock"].ToString() == "1")
                    rdb_DWDD1.Checked = true;
                if (dr["CheckLock"].ToString() == "3")
                    rdb_DWDD3.Checked = true;

                foreach (var value in KFYPa)
                {
                    windPressureDGV.Add(new WindPressureDGV()
                    {
                        Pa = value + "Pa",
                        PaValue = value,
                        zwy1 = string.IsNullOrWhiteSpace(dr["z_one_" + value].ToString()) ? 0 : double.Parse(dr["z_one_" + value].ToString()),
                        zwy2 = string.IsNullOrWhiteSpace(dr["z_two_" + value].ToString()) ? 0 : double.Parse(dr["z_two_" + value].ToString()),
                        zwy3 = string.IsNullOrWhiteSpace(dr["z_three_" + value].ToString()) ? 0 : double.Parse(dr["z_three_" + value].ToString()),
                        zzd = string.IsNullOrWhiteSpace(dr["z_nd_" + value].ToString()) ? 0 : double.Parse(dr["z_nd_" + value].ToString()),
                        zlx = string.IsNullOrWhiteSpace(dr["z_ix_" + value].ToString()) ? 0 : Convert.ToInt32(dr["z_ix_" + value].ToString()),

                        fwy1 = string.IsNullOrWhiteSpace(dr["f_one_" + value].ToString()) ? 0 : double.Parse(dr["f_one_" + value].ToString()),
                        fwy2 = string.IsNullOrWhiteSpace(dr["f_two_" + value].ToString()) ? 0 : double.Parse(dr["f_two_" + value].ToString()),
                        fwy3 = string.IsNullOrWhiteSpace(dr["f_three_" + value].ToString()) ? 0 : double.Parse(dr["f_three_" + value].ToString()),
                        fzd = string.IsNullOrWhiteSpace(dr["f_nd_" + value].ToString()) ? 0 : double.Parse(dr["f_nd_" + value].ToString()),
                        flx = string.IsNullOrWhiteSpace(dr["f_ix_" + value].ToString()) ? 0 : Convert.ToInt32(dr["f_ix_" + value].ToString()),
                    });
                }
                this.txt_p1.Text = dr["p1"] == null ? "0" : dr["p1"].ToString();
                this.txt_p2.Text = dr["p2"] == null ? "0" : dr["p2"].ToString();
                this.txt_p3.Text = dr["p3"] == null ? "0" : dr["p3"].ToString();
                this.txt_f_p1.Text = dr["_p1"] == null ? "0" : dr["_p1"].ToString();
                this.txt_f_p2.Text = dr["_p2"] == null ? "0" : dr["_p2"].ToString();
                this.txt_f_p3.Text = dr["_p3"] == null ? "0" : dr["_p3"].ToString();
            }
            else
            {
                #region 添加默认
                foreach (var value in KFYPa)
                {
                    windPressureDGV.Add(new WindPressureDGV()
                    {
                        Pa = value + "Pa",
                        PaValue = value,
                        zwy1 = 0,
                        zwy2 = 0,
                        zwy3 = 0,
                        zzd = 0,
                        zlx = 0,
                        fwy1 = 0,
                        fwy2 = 0,
                        fwy3 = 0,
                        fzd = 0,
                        flx = 0,
                    });
                }
                windPressureDGV.Add(new WindPressureDGV()
                {
                    Pa = "P3阶段",
                    PaValue = -1,
                    zwy1 = 0,
                    zwy2 = 0,
                    zwy3 = 0,
                    zzd = 0,
                    zlx = 0,
                    fwy1 = 0,
                    fwy2 = 0,
                    fwy3 = 0,
                    fzd = 0,
                    flx = 0,
                });
                windPressureDGV.Add(new WindPressureDGV()
                {
                    Pa = "P3残余变形",
                    PaValue = -2,
                    zwy1 = 0,
                    zwy2 = 0,
                    zwy3 = 0,
                    zzd = 0,
                    zlx = 0,
                    fwy1 = 0,
                    fwy2 = 0,
                    fwy3 = 0,
                    fzd = 0,
                    flx = 0,
                });
                windPressureDGV.Add(new WindPressureDGV()
                {
                    Pa = "PMax/残余变形",
                    PaValue = -3,
                    zwy1 = 0,
                    zwy2 = 0,
                    zwy3 = 0,
                    zzd = 0,
                    zlx = 0,
                    fwy1 = 0,
                    fwy2 = 0,
                    fwy3 = 0,
                    fzd = 0,
                    flx = 0,
                });
                #endregion
            }
            return windPressureDGV;
        }

        private void AnimateSeries(Steema.TeeChart.TChart chart, int yl)
        {
            this.qm_Line.Add(DateTime.Now, yl);
            this.tChart_qm.Axes.Bottom.SetMinMax(dtnow, DateTime.Now.AddSeconds(20));
        }

        private void tim_PainPic_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            //var c = _serialPortClient.GetCYGXS();
            //int value = int.Parse(c.ToString());

            AnimateSeries(this.tChart_qm, _CYGXS);
        }


        private void btn_zyyb_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            var res = _serialPortClient.Send_FY_Btn(BFMCommand.风压正压预备);
            if (!res)
            {
                MessageBox.Show("正压预备异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
            //double yl = _serialPortClient.Read_FY_Btn_SetValue("ZYYB");

            //lbl_setYL.Text = yl.ToString();

            windPressureTest = PublicEnum.WindPressureTest.ZReady;
            DisableBtnType();

            this.tim_fy.Enabled = true;
        }

        private void btn_zyks_Click(object sender, EventArgs e)
        {
            //IsFirst = false;
            if (!_serialPortClient.sp.IsOpen)
                return;

            var res = _serialPortClient.Send_FY_Btn(BFMCommand.风压正压开始);
            if (!res)
            {
                MessageBox.Show("正压开始异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }

            //double yl = _serialPortClient.Read_FY_Btn_SetValue("ZYKS");
            //lbl_setYL.Text = yl.ToString();

            windPressureTest = PublicEnum.WindPressureTest.ZStart;
            DisableBtnType();
            complete = new List<int>();

            this.tim_fy.Enabled = true;
        }

        private void btn_fyyb_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            var res = _serialPortClient.Send_FY_Btn(BFMCommand.风压负压预备, false);
            if (!res)
            {
                MessageBox.Show("负压预备异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
            //double yl = _serialPortClient.Read_FY_Btn_SetValue("FYYB");

            //lbl_setYL.Text = yl.ToString();

            windPressureTest = PublicEnum.WindPressureTest.FReady;
            DisableBtnType();
            this.tim_fy.Enabled = true;
        }

        private void btn_fyks_Click(object sender, EventArgs e)
        {
            //IsFirst = false;
            if (!_serialPortClient.sp.IsOpen)
                return;

            var res = _serialPortClient.Send_FY_Btn(BFMCommand.风压负压开始, false);
            if (!res)
            {
                MessageBox.Show("负压开始异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
            //double yl = _serialPortClient.Read_FY_Btn_SetValue("FYKS");

            //lbl_setYL.Text = yl.ToString();

            windPressureTest = PublicEnum.WindPressureTest.FStart;
            DisableBtnType();
            this.tim_fy.Enabled = true;
            complete = new List<int>();
        }

        private void btn_datahandle_Click(object sender, EventArgs e)
        {

            var defPa = int.Parse(txt_gbjc.Text);

            foreach (var item in windPressureDGV)
            {
                item.zzd = Math.Round(item.zwy2 - (item.zwy1 + item.zwy3) / 2, 2);
                item.zlx = item.zzd == 0 ? 0 : Convert.ToInt32(DefaultBase.BarLength / item.zzd);

                item.fzd = Math.Round(item.fwy2 - (item.fwy1 + item.fwy3) / 2, 2);
                item.flx = item.fzd == 0 ? 0 : Convert.ToInt32(DefaultBase.BarLength / item.fzd);
            }

            BindData();


            #region new

            double lx = 0;
            double.TryParse(txt_lx.Text, out lx);
            double zy = 0;
            double fy = 0;

            Formula.GetKFY(windPressureDGV, ganjianchangdu, lx, ref zy, ref fy);
            //if (zy != -100 && fy != -100)
            //{
            txt_p1.Text = zy > 0 ? Math.Round(zy, 0).ToString() : "0";
            txt_f_p1.Text = fy > 0 ? Math.Round(fy, 0).ToString() : "0";
            //}
            #endregion

            #region  old
            //var data = windPressureDGV.FindAll(t => t.zwy1 > 0 || t.zwy2 > 0 || t.zwy3 > 0 || t.fwy3 > 0 || t.fwy2 > 0 || t.fwy1 > 0);
            ////初始化按最大等级计算。
            //var zdefPa = int.Parse(txt_gbjc.Text) * 8;//2000;
            //var fdefPa = int.Parse(txt_gbjc.Text) * 8;//2000
            //double lx = 0;
            //double.TryParse(txt_lx.Text, out lx);
            //foreach (var item in data)
            //{
            //    if (item.zlx <= lx)
            //    {
            //        zdefPa = int.Parse(item.Pa.Replace("Pa", ""));
            //        break;
            //    }
            //}

            //foreach (var item in data)
            //{
            //    if (item.flx <= lx)
            //    {
            //        fdefPa = int.Parse(item.Pa.Replace("Pa", ""));
            //        break;
            //    }
            //}

            //var zone = new WindPressureDGV();
            //var ztwo = new WindPressureDGV();
            //zone = windPressureDGV.Find(t => t.Pa == (zdefPa - defPa) + "Pa");
            //ztwo = windPressureDGV.Find(t => t.Pa == zdefPa + "Pa");
            //if (zone != null || ztwo != null)
            //{
            //    var x1 = float.Parse(zone.zzd.ToString());
            //    var x2 = float.Parse(ztwo.zzd.ToString());
            //    var y1 = zdefPa - defPa;
            //    var y2 = zdefPa;

            //    var p = Calculate(x1, x2, y1, y2);

            //    txt_p1.Text = Math.Round(p, 0).ToString();
            //    txt_p2.Text = Math.Round(p * 1.5, 0).ToString();
            //    txt_p3.Text = Math.Round(p * 2.5, 0).ToString();
            //}

            //var fone = new WindPressureDGV();
            //var ftwo = new WindPressureDGV();
            //fone = windPressureDGV.Find(t => t.Pa == (fdefPa - defPa) + "Pa");
            //ftwo = windPressureDGV.Find(t => t.Pa == fdefPa + "Pa");
            //if (zone != null || ztwo != null)
            //{
            //    var _x1 = float.Parse(fone.fzd.ToString());
            //    var _x2 = float.Parse(ftwo.fzd.ToString());
            //    var y1 = fdefPa - defPa;
            //    var y2 = fdefPa;
            //    var _p = Calculate(_x1, _x2, y1, y2);
            //    txt_f_p1.Text = Math.Round(_p, 0).ToString();
            //    txt_f_p2.Text = Math.Round(_p * 1.5, 0).ToString();
            //    txt_f_p3.Text = Math.Round(_p * 2.5, 0).ToString();
            //}
            #endregion

            currentkPa = 0;
        }

        private bool AddKfyInfo(int defJC)
        {
            DAL_dt_kfy_Info dal = new DAL_dt_kfy_Info();
            Model_dt_kfy_Info model = new Model_dt_kfy_Info();
            model.dt_Code = _tempCode;
            model.info_DangH = _tempTong;
            model.defJC = defJC;
            for (int i = 0; i < KFYPa.Count; i++)
            {
                #region 获取
                if (i == 0)
                {
                    model.z_one_250 = this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString();
                    model.z_two_250 = this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString();
                    model.z_three_250 = this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString();
                    model.z_nd_250 = this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString();
                    model.z_ix_250 = this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString();
                    model.f_one_250 = this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString();
                    model.f_two_250 = this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString();
                    model.f_three_250 = this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString();
                    model.f_nd_250 = this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString();
                    model.f_ix_250 = this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString();
                }
                if (i == 1)
                {
                    model.z_one_500 = this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString();
                    model.z_two_500 = this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString();
                    model.z_three_500 = this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString();
                    model.z_nd_500 = this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString();
                    model.z_ix_500 = this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString();
                    model.f_one_500 = this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString();
                    model.f_two_500 = this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString();
                    model.f_three_500 = this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString();
                    model.f_nd_500 = this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString();
                    model.f_ix_500 = this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString();
                }
                if (i == 2)
                {
                    model.z_one_750 = this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString();
                    model.z_two_750 = this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString();
                    model.z_three_750 = this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString();
                    model.z_nd_750 = this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString();
                    model.z_ix_750 = this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString();
                    model.f_one_750 = this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString();
                    model.f_two_750 = this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString();
                    model.f_three_750 = this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString();
                    model.f_nd_750 = this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString();
                    model.f_ix_750 = this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString();
                }
                if (i == 3)
                {
                    model.z_one_1000 = this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString();
                    model.z_two_1000 = this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString();
                    model.z_three_1000 = this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString();
                    model.z_nd_1000 = this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString();
                    model.z_ix_1000 = this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString();
                    model.f_one_1000 = this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString();
                    model.f_two_1000 = this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString();
                    model.f_three_1000 = this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString();
                    model.f_nd_1000 = this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString();
                    model.f_ix_1000 = this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString();
                }
                if (i == 4)
                {
                    model.z_one_1250 = this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString();
                    model.z_two_1250 = this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString();
                    model.z_three_1250 = this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString();
                    model.z_nd_1250 = this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString();
                    model.z_ix_1250 = this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString();
                    model.f_one_1250 = this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString();
                    model.f_two_1250 = this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString();
                    model.f_three_1250 = this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString();
                    model.f_nd_1250 = this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString();
                    model.f_ix_1250 = this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString();
                }
                if (i == 5)
                {
                    model.z_one_1500 = this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString();
                    model.z_two_1500 = this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString();
                    model.z_three_1500 = this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString();
                    model.z_nd_1500 = this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString();
                    model.z_ix_1500 = this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString();
                    model.f_one_1500 = this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString();
                    model.f_two_1500 = this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString();
                    model.f_three_1500 = this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString();
                    model.f_nd_1500 = this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString();
                    model.f_ix_1500 = this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString();
                }
                if (i == 6)
                {
                    model.z_one_1750 = this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString();
                    model.z_two_1750 = this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString();
                    model.z_three_1750 = this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString();
                    model.z_nd_1750 = this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString();
                    model.z_ix_1750 = this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString();
                    model.f_one_1750 = this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString();
                    model.f_two_1750 = this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString();
                    model.f_three_1750 = this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString();
                    model.f_nd_1750 = this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString();
                    model.f_ix_1750 = this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString();
                }
                if (i == 7)
                {
                    model.z_one_2000 = this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString();
                    model.z_two_2000 = this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString();
                    model.z_three_2000 = this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString();
                    model.z_nd_2000 = this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString();
                    model.z_ix_2000 = this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString();
                    model.f_one_2000 = this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString();
                    model.f_two_2000 = this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString();
                    model.f_three_2000 = this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString();
                    model.f_nd_2000 = this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString();
                    model.f_ix_2000 = this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString();
                }

                #endregion
            }
            model.p1 = txt_p1.Text;
            model.p2 = txt_p2.Text;
            model.p3 = txt_p3.Text;
            model._p1 = txt_f_p1.Text;
            model._p2 = txt_f_p2.Text;
            model._p3 = txt_f_p3.Text;
            model.CheckLock = rdb_DWDD1.Checked ? 1 : rdb_DWDD3.Checked ? 3 : 0;
            return dal.Add_kfy_Info(model);
        }

        private void btn_zff_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            this.tim_fy.Enabled = true;
            int value = 0;

            int.TryParse(txt_p2.Text, out value);

            if (value == 0)
                return;

            var res = _serialPortClient.Set_FY_Value(BFMCommand.正反复数值, BFMCommand.正反复, value);
            if (!res)
            {
                MessageBox.Show("正反复异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }

            windPressureTest = PublicEnum.WindPressureTest.ZRepeatedly;
            DisableBtnType();
        }

        private void btn_fff_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            this.tim_fy.Enabled = true;
            int value = 0;
            int.TryParse(txt_f_p2.Text, out value);

            if (value == 0)
                return;
            var res = _serialPortClient.Set_FY_Value(BFMCommand.负反复数值, BFMCommand.负反复, value, false);
            if (!res)
            {
                MessageBox.Show("负反复异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }

            windPressureTest = PublicEnum.WindPressureTest.FRepeatedly;
            DisableBtnType();
        }

        private void btn_zaq_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            this.tim_fy.Enabled = true;
            int value = 0;
            int.TryParse(txt_p3.Text, out value);

            if (value == 0)
                return;
            var res = _serialPortClient.Set_FY_Value(BFMCommand.正安全数值, BFMCommand.正安全, value);
            if (!res)
            {
                MessageBox.Show("正安全异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
            windPressureTest = PublicEnum.WindPressureTest.ZSafety;
            DisableBtnType();
        }

        private void btnfaq_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            this.tim_fy.Enabled = true;
            int value = 0;
            int.TryParse(txt_f_p3.Text, out value);

            if (value == 0)
                return;

            var res = _serialPortClient.Set_FY_Value(BFMCommand.负安全数值, BFMCommand.负安全, value, false);
            if (!res)
            {
                MessageBox.Show("负安全异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
            windPressureTest = PublicEnum.WindPressureTest.FSafety;
            DisableBtnType();
        }

        private void dgv_WindPressure_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void btn_wygl_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            _serialPortClient.SendWYGL();
        }

        private void tim_wyData_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            //抗风压
            var displace1 = _serialPortClient.GetDisplace1();
            var displace2 = _serialPortClient.GetDisplace2();
            var displace3 = _serialPortClient.GetDisplace3();

            txt_wy1.Text = displace1.ToString();
            txt_wy2.Text = displace2.ToString();
            txt_wy3.Text = displace3.ToString();
        }

        /// <summary>
        /// 记录当前锚点
        /// </summary>
        private int currentkPa = 0;



        private void tim_fy_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            _CYGXS = _serialPortClient.GetCYGXS();

            lbl_dqyl.Text = _CYGXS.ToString();

            if (!IsOk)
                return;

            if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (_CYGXS >= KFYPa[i] - 10 && _CYGXS <= (KFYPa[i] + 10) && !complete.Exists(t => t == KFYPa[i]))
                    {
                        complete.Add(KFYPa[i]);
                        currentkPa = KFYPa[i];
                        tim_static.Enabled = true;
                    }
                }
            }

            if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (_CYGXS >= (-KFYPa[i] - 10) && _CYGXS <= (-KFYPa[i] + 10) && !complete.Exists(t => t == KFYPa[i]))
                    {
                        complete.Add(KFYPa[i]);
                        currentkPa = KFYPa[i];
                        tim_static.Enabled = true;
                    }
                }

            }
        }
        //稳压次数
        private int staticIndex = 1;
        private List<int> complete = new List<int>();
        private bool IsOk = true;
        private List<Tuple<double, double, double>> average = new List<Tuple<double, double, double>>();
        /// <summary>
        /// 稳压5次取平均值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tim_static_Tick(object sender, EventArgs e)
        {
            var defPa = int.Parse(txt_gbjc.Text);

            var common = windPressureTest == PublicEnum.WindPressureTest.ZStart ? BFMCommand.风压_正压是否计时 : BFMCommand.风压_负压是否计时;
            var res = _serialPortClient.Read_FY_Static_IsStart(common);
            if (!res)
                return;

            IsOk = false;
            if (staticIndex < 5)
            {
                double displace1 = _serialPortClient.GetDisplace1();
                double displace2 = _serialPortClient.GetDisplace2();
                double displace3 = _serialPortClient.GetDisplace3();

                average.Add(new Tuple<double, double, double>(displace1, displace2, displace3));
            }
            else
            {
                double ave1 = 0, ave2 = 0, ave3 = 0;
                foreach (var item in average)
                {
                    ave1 += item.Item1;
                    ave2 += item.Item2;
                    ave3 += item.Item3;
                }
                var pa = windPressureDGV.Find(t => t.Pa == currentkPa + "Pa");
                if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
                {
                    pa.zwy1 = Math.Round(ave1 / average.Count, 2);
                    pa.zwy2 = Math.Round(ave2 / average.Count, 2);
                    pa.zwy3 = Math.Round(ave3 / average.Count, 2);
                    pa.zzd = Math.Round(pa.zwy2 - (pa.zwy1 + pa.zwy3) / 2, 2);
                    pa.zlx = pa.zzd == 0 ? 0 : Convert.ToInt32(DefaultBase.BarLength / pa.zzd);
                }
                else if (windPressureTest == PublicEnum.WindPressureTest.FStart)
                {
                    pa.fwy1 = Math.Round(ave1 / average.Count, 2);
                    pa.fwy2 = Math.Round(ave2 / average.Count, 2);
                    pa.fwy3 = Math.Round(ave3 / average.Count, 2);
                    pa.fzd = Math.Round(pa.fwy2 - (pa.fwy1 + pa.fwy3) / 2, 2);
                    pa.flx = pa.fzd == 0 ? 0 : Convert.ToInt32(DefaultBase.BarLength / pa.fzd);
                }
                //清空初始化

                var def_LX = 0;
                int.TryParse(txt_lx.Text, out def_LX);

                var lx = windPressureTest == PublicEnum.WindPressureTest.ZStart ? pa.zlx : pa.flx;
                if (lx < def_LX)
                {
                    //  BindData();
                    Stop();
                    OpenBtnType();

                    //todo:使用那个lx?
                    double lx2 = 0;
                    double.TryParse(txt_lx.Text, out lx2);
                    double zy = 0;
                    double fy = 0;

                    Formula.GetKFY(windPressureDGV, ganjianchangdu, lx2, ref zy, ref fy);
                    if (zy != -100 && fy != -100)
                    {
                        txt_p1.Text = Math.Round(zy, 0).ToString();
                        txt_f_p1.Text = Math.Round(fy, 0).ToString();
                    }


                    //var one = new WindPressureDGV();
                    //var two = new WindPressureDGV();

                    //one = windPressureDGV.Find(t => t.Pa == (currentkPa - defPa) + "Pa");
                    //two = windPressureDGV.Find(t => t.Pa == currentkPa + "Pa");
                    //if (one != null && two != null)
                    //{
                    //var x1 = windPressureTest == PublicEnum.WindPressureTest.ZStart ? float.Parse(one.zzd.ToString()) : float.Parse(one.fzd.ToString());
                    //var x2 = windPressureTest == PublicEnum.WindPressureTest.ZStart ? float.Parse(two.zzd.ToString()) : float.Parse(two.fzd.ToString());
                    //var y1 = currentkPa - defPa;
                    //var y2 = currentkPa;


                    //var p = Calculate(x1, x2, y1, y2);
                    //if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
                    //{
                    //    txt_p1.Text = Math.Round(p, 0).ToString();
                    //txt_p2.Text = Math.Round(p * 1.5, 0).ToString();
                    //txt_p3.Text = Math.Round(p * 2.5, 0).ToString();
                    //}
                    //else if (windPressureTest == PublicEnum.WindPressureTest.FStart)
                    //{
                    //    txt_f_p1.Text = Math.Round(p, 0).ToString();
                    //txt_f_p2.Text = Math.Round(p * 1.5, 0).ToString();
                    //txt_f_p3.Text = Math.Round(p * 2.5, 0).ToString();
                    //}
                    //}
                }
                this.tim_static.Enabled = false;
                //    BindData();
                average = new List<Tuple<double, double, double>>();
                staticIndex = 1;
                IsOk = true;

                // lbl_setYL.Text = "0";
            }
            staticIndex++;
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <returns></returns> 

        private double Calculate(float x1, float x2, int y1, int y2)
        {
            float k = 0, b = 0;

            Formula.Calculate(x1, x2, y1, y2, ref k, ref b);

            double x = 0;
            double.TryParse(txt_lx.Text, out x);

            x = DefaultBase.BarLength / x;

            if (k == 0 && b == 0)
                return Math.Round(x, 2);

            return Math.Round(k * x + b, 2);
        }

        /// <summary>
        /// 是否开始
        /// </summary>
        private bool IsStart = false;
        private void tim_btnType_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            if (windPressureTest == null)
                return;

            var IsSeccess = false;
            if (windPressureTest == PublicEnum.WindPressureTest.ZReady)
            {
                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.风压正压预备结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    MessageBox.Show("风压正压预备结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (value == 3)
                {
                    windPressureTest = PublicEnum.WindPressureTest.Stop;
                    //  lbl_setYL.Text = "0";
                    OpenBtnType();
                    this.tim_fy.Enabled = true;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                double value = _serialPortClient.Read_FY_BtnType(BFMCommand.风压正压开始结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    MessageBox.Show("风压正压开始结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (value >= 15)
                {
                    windPressureTest = PublicEnum.WindPressureTest.Stop;
                    IsStart = false;
                    // lbl_setYL.Text = "0";
                    OpenBtnType();
                    this.tim_fy.Enabled = true;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FReady)
            {
                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.风压负压预备结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    MessageBox.Show("风压负压预备结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (value == 3)
                {
                    windPressureTest = PublicEnum.WindPressureTest.Stop;
                    //   lbl_setYL.Text = "0";
                    OpenBtnType();
                    this.tim_fy.Enabled = true;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                double value = _serialPortClient.Read_FY_BtnType(BFMCommand.风压负压开始结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    MessageBox.Show("风压负压开始结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (value >= 15)
                {
                    IsStart = false;
                    // lbl_setYL.Text = "0";
                    OpenBtnType();
                    this.tim_fy.Enabled = true;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZRepeatedly)
            {
                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.正反复结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    MessageBox.Show("正反复结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (value == 5)
                {
                    //   lbl_setYL.Text = "0";
                    OpenBtnType();
                    this.tim_fy.Enabled = true;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FRepeatedly)
            {
                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.负反复结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    MessageBox.Show("负反复结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (value == 5)
                {
                    // lbl_setYL.Text = "0";
                    OpenBtnType();
                    this.tim_fy.Enabled = true;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZSafety)
            {
                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.正安全结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    MessageBox.Show("正安全结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (value == 1)
                {
                    //  lbl_setYL.Text = "0";
                    OpenBtnType();
                    this.tim_fy.Enabled = true;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FSafety)
            {
                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.负安全结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    MessageBox.Show("负安全结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
                if (value == 1)
                {
                    // lbl_setYL.Text = "0";
                    OpenBtnType();
                    this.tim_fy.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 开启按钮
        /// </summary>
        private void OpenBtnType()
        {
            this.btn_zyyb.Enabled = true;
            this.btn_zyks.Enabled = true;
            this.btn_fyyb.Enabled = true;
            this.btn_fyks.Enabled = true;
            this.btn_zff.Enabled = true;
            this.btn_fff.Enabled = true;
            this.btn_zaq.Enabled = true;
            this.btnfaq.Enabled = true;
            this.btn_datahandle.Enabled = true;
        }

        /// <summary>
        /// 禁用按钮
        /// </summary>
        private void DisableBtnType()
        {
            this.btn_zyyb.Enabled = false;
            this.btn_zyks.Enabled = false;
            this.btn_fyyb.Enabled = false;
            this.btn_fyks.Enabled = false;
            this.btn_zff.Enabled = false;
            this.btn_fff.Enabled = false;
            this.btn_zaq.Enabled = false;
            this.btnfaq.Enabled = false;
            this.btn_datahandle.Enabled = false;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Stop();
            OpenBtnType();
            // lbl_setYL.Text = "0";

            windPressureTest = PublicEnum.WindPressureTest.Stop;
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

        private void tim_view_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!_serialPortClient.sp.IsOpen)
                    return;
                BindData();

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.tChart_qm.Export.ShowExportDialog();
        }

        private void tChart_qm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.char_cms_click.Show(MousePosition.X, MousePosition.Y);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (DefaultBase.LockPoint)
            {
                if (!rdb_DWDD1.Checked && !rdb_DWDD3.Checked)
                {
                    MessageBox.Show("请选择位移！", "请选择位移！", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    return;
                }
            }
            var jc = int.Parse(txt_gbjc.Text);
            if (AddKfyInfo(jc))
            {
                MessageBox.Show("处理成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
            complete = new List<int>();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                              this.panel1.ClientRectangle,
                              Color.Black,//7f9db9
                              1,
                              ButtonBorderStyle.Solid,
                              Color.Black,
                              1,
                              ButtonBorderStyle.Solid,
                              Color.Black,
                              1,
                              ButtonBorderStyle.Solid,
                              Color.Black,
                              1,
                              ButtonBorderStyle.Solid);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                              this.panel2.ClientRectangle,
                             Color.Black,
                              1,
                              ButtonBorderStyle.Solid,
                              Color.Black,
                              1,
                              ButtonBorderStyle.Solid,
                              Color.Black,
                              1,
                              ButtonBorderStyle.Solid,
                              Color.Black,
                              1,
                              ButtonBorderStyle.Solid);
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                            this.panel2.ClientRectangle,
                           Color.Black,
                            1,
                            ButtonBorderStyle.Solid,
                            Color.Black,
                            1,
                            ButtonBorderStyle.Solid,
                            Color.Black,
                            1,
                            ButtonBorderStyle.Solid,
                            Color.Black,
                            1,
                            ButtonBorderStyle.Solid);
        }

        private void dgv_WindPressure_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (var item in windPressureDGV)
            {
                item.zzd = Math.Round(item.zwy2 - (item.zwy1 + item.zwy3) / 2, 2);
                item.zlx = item.zzd == 0 ? 0 : Convert.ToInt32(DefaultBase.BarLength / item.zzd);

                item.fzd = Math.Round(item.fwy2 - (item.fwy1 + item.fwy3) / 2, 2);
                item.flx = item.fzd == 0 ? 0 : Convert.ToInt32(DefaultBase.BarLength / item.fzd);
            }

            BindData();
        }

        private void btn_gbjc_Click(object sender, EventArgs e)
        {
            var value = int.Parse(txt_gbjc.Text);
            var res = _serialPortClient.SendGBJC(value);
            if (!res)
            {
                MessageBox.Show("改变级差异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }

            KFYPa = new List<int>();
            for (int i = 1; i < 9; i++)
            {
                KFYPa.Add((value * i));
            }
            windPressureDGV = new List<WindPressureDGV>();
            BindData();
        }

        private void btn_zpmax_Click(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(txt_zpmax.Text, out value);

            var res = _serialPortClient.Set_FY_Value(BFMCommand.正PMAX值, BFMCommand.正PMAX, value);
            if (!res)
            {
                MessageBox.Show("正pmax！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
        }

        private void btn_fpmax_Click(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(txt_fpmax.Text, out value);
            var res = _serialPortClient.Set_FY_Value(BFMCommand.负PMAX值, BFMCommand.负PMAX, value);
            if (!res)
            {
                MessageBox.Show("负pmax！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }
        }


    }
}
