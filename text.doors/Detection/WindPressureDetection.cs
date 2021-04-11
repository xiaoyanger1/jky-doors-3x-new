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


        private static object obj = new object();
        public DateTime dtnow { get; set; }


        //默认
        private List<DefKFYPa> defKFYPa = new List<DefKFYPa>();

        public static double _displace1 = 0;
        public static double _displace2 = 0;
        public static double _displace3 = 0;

        public static bool IsGCJC = false;


        public List<WindPressureDGV> windPressureDGV = new List<WindPressureDGV>();
        /// <summary>
        /// 抗风压数据位置
        /// </summary>
        private PublicEnum.WindPressureTest? windPressureTest = null;


        public static System.Timers.Timer tim_fy1;
        public static System.Timers.Timer tim_static1;
        public static Thread td;

        public WindPressureDetection()
        {

        }

        public WindPressureDetection(SerialPortClient tcpClient, string tempCode, string tempTong)
        {
            InitializeComponent();

            //var lx2 = 375;

            //var zy = 0.00;
            //var fy = 0.00;
            //windPressureDGV.Add(new WindPressureDGV() { PaValue = 250, zwy1 = 0.04, zwy2 = 0.76, zwy3 = 0.03 });
            //windPressureDGV.Add(new WindPressureDGV() { PaValue = 500, zwy1 = 0.10, zwy2 = 1.42, zwy3 = 0.07 });
            //windPressureDGV.Add(new WindPressureDGV() { PaValue = 750, zwy1 = 0.17, zwy2 = 2.09, zwy3 = 0.14 });
            //windPressureDGV.Add(new WindPressureDGV() { PaValue = 1000, zwy1 = 0.29, zwy2 = 2.83, zwy3 = 0.23 });
            //windPressureDGV.Add(new WindPressureDGV() { PaValue = 1250, zwy1 = 0.44, zwy2 = 3.61, zwy3 = 3.22 });


            //Formula.GetKFY(windPressureDGV, 1240, lx2, ref zy, ref fy);

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

            BindFromInit();

            BindData(false);

            td = new Thread(BindFromInput);
            td.IsBackground = true;
            td.Start();

        }

        /// <summary>
        /// 实时绑定数据
        /// </summary>
        private void BindFromInput()
        {
            SetRealTimeData st1 = new SetRealTimeData(Update_wy1_Label);
            SetRealTimeData st2 = new SetRealTimeData(Update_wy2_Label);
            SetRealTimeData st3 = new SetRealTimeData(Update_wy3_Label);
            SetRealTimeData st4 = new SetRealTimeData(Update_lbl_dqyl_Label);

            while (true)
            {
                try
                {
                    if (lbl_wy1.InvokeRequired)
                        lbl_wy1.Invoke(st1, _serialPortClient.GetDisplace1().ToString());
                    else
                        lbl_wy1.Text = _serialPortClient.GetDisplace1().ToString();

                    if (lbl_wy2.InvokeRequired)
                        lbl_wy2.Invoke(st2, _serialPortClient.GetDisplace2().ToString());
                    else
                        lbl_wy2.Text = _serialPortClient.GetDisplace2().ToString();

                    if (lbl_wy3.InvokeRequired)
                        lbl_wy3.Invoke(st3, _serialPortClient.GetDisplace3().ToString());
                    else
                        lbl_wy3.Text = _serialPortClient.GetDisplace3().ToString();

                    if (lbl_dqyl.InvokeRequired)
                        lbl_dqyl.Invoke(st4, _serialPortClient.GetCY_High().ToString());
                    else
                        lbl_dqyl.Text = _serialPortClient.GetCY_High().ToString();
                }
                catch (Exception ex)
                {
                    td.Abort();
                    //Logger.Info(ex);
                }
            }
        }

        //委托
        public delegate void SetRealTimeData(string value);

        private void Update_wy1_Label(string value)
        {
            lbl_wy1.Text = value;
        }
        private void Update_wy2_Label(string value)
        {
            lbl_wy2.Text = value;
        }
        private void Update_wy3_Label(string value)
        {
            lbl_wy3.Text = value;
        }
        private void Update_lbl_dqyl_Label(string value)
        {
            lbl_dqyl.Text = value;
        }



        /// <summary>
        /// 基础设置
        /// </summary>
        private void BindFromInit()
        {
            // 绑定设定压力
            lbl_title.Text = string.Format("门窗抗风压性能检测  第{0}号 {1}", this._tempCode, this._tempTong);

            //改变极差默认
            var jcValue = _serialPortClient.GetKFYjC();
            txt_gbjc.Text = jcValue.ToString();


            dtnow = DateTime.Now;

            //风速图表
            qm_Line.GetVertAxis.SetMinMax(-8000, 8000);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="isUpdate">是否修改</param>
        private void BindData(bool isUpdate)
        {
            #region 绑定
            List<WindPressureDGV> dataSource = new List<WindPressureDGV>();
            if (isUpdate)
            {
                dataSource = windPressureDGV;
            }
            else
            {
                dataSource = GetWindPressureDGV();
            }
            dgv_WindPressure.DataSource = dataSource;
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
            //dgv_WindPressure.Columns["zlx"].DefaultCellStyle.Format = "N0";
            dgv_WindPressure.Columns["PaValue"].Visible = false;

            dgv_WindPressure.Columns["fwy1"].DefaultCellStyle.Format = "N2";
            dgv_WindPressure.Columns["fwy2"].DefaultCellStyle.Format = "N2";
            dgv_WindPressure.Columns["fwy3"].DefaultCellStyle.Format = "N2";
            dgv_WindPressure.Columns["fzd"].DefaultCellStyle.Format = "N2";
            //dgv_WindPressure.Columns["flx"].DefaultCellStyle.Format = "N0";
            dgv_WindPressure.Columns["PaValue"].Visible = false;


            dgv_WindPressure.Refresh();
            #endregion
        }


        private List<WindPressureDGV> GetWindPressureDGV()
        {
            windPressureDGV = new List<WindPressureDGV>();

            if (_serialPortClient.sp.IsOpen)
            {
                if (int.Parse(txt_gbjc.Text) == 0)
                {
                    return windPressureDGV;
                }
            }
            var dt = new DAL_dt_kfy_Info().GetkfyByCodeAndTong(_tempCode, _tempTong);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                if (!_serialPortClient.sp.IsOpen)
                {
                    //极差
                    txt_gbjc.Text = dr["defJC"].ToString();
                }

                IsGCJC = int.Parse(dr["testtype"].ToString()) == 2 ? true : false;

                if (IsGCJC)
                {
                    btn_gcjc.BackColor = Color.Green;
                }
                else
                {
                    btn_gcjc.BackColor = Color.Transparent;
                }


                txt_lx.Text = dr["lx"].ToString();
                txt_desc.Text = dr["desc"].ToString();
                //绑定锁点
                if (dr["CheckLock"].ToString() == "1")
                    rdb_DWDD1.Checked = true;
                if (dr["CheckLock"].ToString() == "3")
                    rdb_DWDD3.Checked = true;

                var jcvalue = int.Parse(txt_gbjc.Text);

                defKFYPa = new List<DefKFYPa>();
                for (int i = 1; i < 9; i++)
                {
                    defKFYPa.Add(new DefKFYPa() { Value = jcvalue * i });
                }
                foreach (var paInfo in defKFYPa)
                {
                    windPressureDGV.Add(new WindPressureDGV()
                    {
                        Pa = paInfo.Value + "Pa",
                        PaValue = paInfo.Value,
                        zwy1 = string.IsNullOrWhiteSpace(dr["z_one_" + paInfo.Value].ToString()) ? 0 : double.Parse(dr["z_one_" + paInfo.Value].ToString()),
                        zwy2 = string.IsNullOrWhiteSpace(dr["z_two_" + paInfo.Value].ToString()) ? 0 : double.Parse(dr["z_two_" + paInfo.Value].ToString()),
                        zwy3 = string.IsNullOrWhiteSpace(dr["z_three_" + paInfo.Value].ToString()) ? 0 : double.Parse(dr["z_three_" + paInfo.Value].ToString()),

                        fwy1 = string.IsNullOrWhiteSpace(dr["f_one_" + paInfo.Value].ToString()) ? 0 : double.Parse(dr["f_one_" + paInfo.Value].ToString()),
                        fwy2 = string.IsNullOrWhiteSpace(dr["f_two_" + paInfo.Value].ToString()) ? 0 : double.Parse(dr["f_two_" + paInfo.Value].ToString()),
                        fwy3 = string.IsNullOrWhiteSpace(dr["f_three_" + paInfo.Value].ToString()) ? 0 : double.Parse(dr["f_three_" + paInfo.Value].ToString()),
                    });
                }
                //极差
                for (int i = 0; i < 3; i++)
                {
                    var name = "";
                    var field = "";
                    if (i == 0)
                    {
                        name = "P3阶段";
                        field = "p3jieduan";
                    }
                    else if (i == 1)
                    {
                        name = "P3残余变形";
                        field = "p3canyubianxing";
                    }
                    else if (i == 2)
                    {
                        name = "PMax/残余变形";
                        field = "pMaxcanyubianxing";
                    }
                    windPressureDGV.Add(new WindPressureDGV()
                    {
                        Pa = name,
                        PaValue = -1,
                        zwy1 = string.IsNullOrWhiteSpace(dr["z_one_" + field].ToString()) ? 0 : double.Parse(dr["z_one_" + field].ToString()),
                        zwy2 = string.IsNullOrWhiteSpace(dr["z_two_" + field].ToString()) ? 0 : double.Parse(dr["z_two_" + field].ToString()),
                        zwy3 = string.IsNullOrWhiteSpace(dr["z_three_" + field].ToString()) ? 0 : double.Parse(dr["z_three_" + field].ToString()),

                        fwy1 = string.IsNullOrWhiteSpace(dr["f_one_" + field].ToString()) ? 0 : double.Parse(dr["f_one_" + field].ToString()),
                        fwy2 = string.IsNullOrWhiteSpace(dr["f_two_" + field].ToString()) ? 0 : double.Parse(dr["f_two_" + field].ToString()),
                        fwy3 = string.IsNullOrWhiteSpace(dr["f_three_" + field].ToString()) ? 0 : double.Parse(dr["f_three_" + field].ToString()),
                    });
                }

                this.txt_p1.Text = dr["p1"] == null ? "0" : dr["p1"].ToString();
                this.txt_p2.Text = dr["p2"] == null ? "0" : dr["p2"].ToString();
                this.txt_p3.Text = dr["p3"] == null ? "0" : dr["p3"].ToString();
                this.txt_f_p1.Text = dr["_p1"] == null ? "0" : dr["_p1"].ToString();
                this.txt_f_p2.Text = dr["_p2"] == null ? "0" : dr["_p2"].ToString();
                this.txt_f_p3.Text = dr["_p3"] == null ? "0" : dr["_p3"].ToString();
                this.txt_zpmax.Text = dr["z_pMax"] == null ? "0" : dr["z_pMax"].ToString();
                this.txt_fpmax.Text = dr["f_pMax"] == null ? "0" : dr["f_pMax"].ToString();


            }
            else
            {
                #region 添加默认

                var jcvalue = int.Parse(txt_gbjc.Text);

                defKFYPa = new List<DefKFYPa>();
                for (int i = 1; i < 9; i++)
                {
                    defKFYPa.Add(new DefKFYPa() { Value = jcvalue * i });
                }

                windPressureDGV = new List<WindPressureDGV>();

                foreach (var paInfo in defKFYPa)
                {
                    windPressureDGV.Add(new WindPressureDGV()
                    {
                        Pa = paInfo.Value + "Pa",
                        PaValue = paInfo.Value,
                        zwy1 = 0.00,
                        zwy2 = 0.00,
                        zwy3 = 0.00,
                        fwy1 = 0.00,
                        fwy2 = 0.00,
                        fwy3 = 0.00,
                    });
                }
                //极差
                for (int i = 0; i < 3; i++)
                {
                    var name = "";
                    var paValue = 0;
                    if (i == 0)
                    {
                        name = "P3阶段";
                        paValue = -1;
                    }
                    else if (i == 1)
                    {
                        name = "P3残余变形";
                        paValue = -2;
                    }

                    else if (i == 2)
                    {
                        name = "PMax/残余变形";
                        paValue = -3;
                    }
                    windPressureDGV.Add(new WindPressureDGV()
                    {
                        Pa = name,
                        PaValue = paValue,
                        zwy1 = 0.00,
                        zwy2 = 0.00,
                        zwy3 = 0.00,
                        fwy1 = 0.00,
                        fwy2 = 0.00,
                        fwy3 = 0.00,
                    });
                }
                #endregion
            }
            return windPressureDGV;
        }



        private void tim_PainPic_Tick(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            AnimateSeries(this.tChart_qm, _serialPortClient.GetCY_High());
            // AnimateSeries(this.tChart_qm, RegisterData.CY_High_Value);

        }
        private void AnimateSeries(Steema.TeeChart.TChart chart, int yl)
        {
            this.qm_Line.Add(DateTime.Now, yl);
            this.tChart_qm.Axes.Bottom.SetMinMax(dtnow, DateTime.Now.AddSeconds(20));
        }


        private void dgv_WindPressure_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }


        private bool AddKfyInfo(int defJC)
        {
            DAL_dt_kfy_Info dal = new DAL_dt_kfy_Info();
            Model_dt_kfy_Info model = new Model_dt_kfy_Info();
            model.dt_Code = _tempCode;
            model.info_DangH = _tempTong;
            model.defJC = defJC;
            model.lx = int.Parse(txt_lx.Text);
            for (int i = 0; i < 11; i++)
            {
                #region 获取
                if (i == 0)
                {
                    model.z_one_250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                    model.f_one_250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                if (i == 1)
                {
                    model.z_one_500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                    model.f_one_500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                if (i == 2)
                {
                    model.z_one_750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                    model.f_one_750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                if (i == 3)
                {
                    model.z_one_1000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_1000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_1000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_1000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_1000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                    model.f_one_1000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_1000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_1000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_1000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_1000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                if (i == 4)
                {
                    model.z_one_1250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_1250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_1250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_1250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_1250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                    model.f_one_1250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_1250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_1250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_1250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_1250 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                if (i == 5)
                {
                    model.z_one_1500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_1500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_1500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_1500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_1500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                    model.f_one_1500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_1500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_1500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_1500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_1500 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                if (i == 6)
                {
                    model.z_one_1750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_1750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_1750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_1750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_1750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                    model.f_one_1750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_1750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_1750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_1750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_1750 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                if (i == 7)
                {
                    model.z_one_2000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_2000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_2000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_2000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_2000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                    model.f_one_2000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_2000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_2000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_2000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_2000 = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                if (i == 8)
                {
                    model.z_one_p3jieduan = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_p3jieduan = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_p3jieduan = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_p3jieduan = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_p3jieduan = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");


                    model.f_one_p3jieduan = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_p3jieduan = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_p3jieduan = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_p3jieduan = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_p3jieduan = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                if (i == 9)
                {
                    model.z_one_p3canyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_p3canyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_p3canyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_p3canyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_p3canyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                    model.f_one_p3canyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_p3canyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_p3canyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_p3canyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_p3canyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                if (i == 10)
                {
                    model.z_one_pMaxcanyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy1"].Value.ToString()).ToString("f2");
                    model.z_two_pMaxcanyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy2"].Value.ToString()).ToString("f2");
                    model.z_three_pMaxcanyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zwy3"].Value.ToString()).ToString("f2");
                    model.z_nd_pMaxcanyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zzd"].Value.ToString()).ToString("f2");
                    model.z_ix_pMaxcanyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["zlx"].Value.ToString()).ToString("f2");
                    model.f_one_pMaxcanyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy1"].Value.ToString()).ToString("f2");
                    model.f_two_pMaxcanyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy2"].Value.ToString()).ToString("f2");
                    model.f_three_pMaxcanyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fwy3"].Value.ToString()).ToString("f2");
                    model.f_nd_pMaxcanyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["fzd"].Value.ToString()).ToString("f2");
                    model.f_ix_pMaxcanyubianxing = double.Parse(this.dgv_WindPressure.Rows[i].Cells["flx"].Value.ToString()).ToString("f2");
                }
                #endregion
            }
            model.p1 = txt_p1.Text;
            model.p2 = txt_p2.Text;
            model.p3 = txt_p3.Text;
            model._p1 = txt_f_p1.Text;
            model._p2 = txt_f_p2.Text;
            model._p3 = txt_f_p3.Text;

            model.testtype = IsGCJC == true ? 2 : 1;
            model.pMax = txt_zpmax.Text;
            model._pMax = txt_fpmax.Text;
            model.desc = txt_desc.Text;
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
                MessageBox.Show("正反复异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            windPressureTest = PublicEnum.WindPressureTest.ZRepeatedly;
            DisableBtnType();
            btn_zff.BackColor = Color.Green;
        }

        private void btn_zyyb_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            var res = _serialPortClient.Send_FY_Btn(BFMCommand.风压正压预备);
            if (!res)
            {
                MessageBox.Show("正压预备异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            windPressureTest = PublicEnum.WindPressureTest.ZReady;

            DisableBtnType();

            btn_zyyb.BackColor = Color.Green;

        }

        private void btn_zyks_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            var res = _serialPortClient.Send_FY_Btn(BFMCommand.风压正压开始);
            if (!res)
            {
                //MessageBox.Show("正压开始异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            windPressureTest = PublicEnum.WindPressureTest.ZStart;


            DisableBtnType();
            btn_zyks.BackColor = Color.Green;
            complete = new List<int>();


            tim_fy1 = new System.Timers.Timer(1000);
            tim_fy1.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer);  //到达时间的时候执行倒计时事件timeout；
            tim_fy1.Enabled = true;
        }

        private void fyTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            if (!IsOk)
                return;

            if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                var cyValue = _serialPortClient.GetCY_High();
                var val = defKFYPa.Find(t => cyValue >= t.MinValue && cyValue <= t.MaxValue);
                if (val != null && !complete.Exists(t => t == val.Value))
                {
                    complete.Add(val.Value);
                    currentkPa = val.Value;

                    tim_static1 = new System.Timers.Timer(500);
                    tim_static1.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer);  //到达时间的时候执行倒计时事件timeout；
                    tim_static1.Enabled = true;
                }
            }

            if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                var cyValue = _serialPortClient.GetCY_High();
                var val = defKFYPa.Find(t => cyValue >= t._MinValue && cyValue <= t._MaxValue);
                if (val != null && !complete.Exists(t => t == val.Value))
                {
                    complete.Add(val.Value);
                    currentkPa = val.Value;

                    tim_static1 = new System.Timers.Timer(500);
                    tim_static1.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer);  //到达时间的时候执行倒计时事件timeout；
                    tim_static1.Enabled = true;
                }
            }

            if (windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.FSafety)
            {
                if (!complete.Exists(t => t == -1))
                {
                    complete.Add(-1);
                    currentkPa = -1;

                    tim_static1 = new System.Timers.Timer(500);
                    tim_static1.Elapsed += new System.Timers.ElapsedEventHandler(staticTimer);  //到达时间的时候执行倒计时事件timeout；
                    tim_static1.Enabled = true;

                    //  tim_static.Enabled = true;
                }
            }
        }

        private void staticTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            IsOk = false;
            var maxIndex = 0;
            var common = "";
            if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                common = BFMCommand.风压_正压是否计时;
                maxIndex = 6;
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                common = BFMCommand.风压_负压是否计时;
                maxIndex = 6;
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZSafety)
            {
                common = BFMCommand.风压安全_正压是否计时;
                maxIndex = 4;
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FSafety)
            {
                common = BFMCommand.风压安全_负压是否计时;
                maxIndex = 4;
            }
            var res = _serialPortClient.Read_FY_Static_IsStart(common);
            if (!res)
            {
                return;
            }


            if (staticIndex < maxIndex)
            {
                var _displace1 = _serialPortClient.GetDisplace1();
                var _displace2 = _serialPortClient.GetDisplace2();
                var _displace3 = _serialPortClient.GetDisplace3();
                average.Add(new Tuple<double, double, double>(_displace1, _displace2, _displace3));
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
                var pa = windPressureDGV.Find(t => t.PaValue == currentkPa);

                if (windPressureTest == PublicEnum.WindPressureTest.ZStart || windPressureTest == PublicEnum.WindPressureTest.ZSafety)
                {
                    pa.zwy1 = Math.Round(ave1 / average.Count, 2, MidpointRounding.AwayFromZero);
                    pa.zwy2 = Math.Round(ave2 / average.Count, 2, MidpointRounding.AwayFromZero);
                    pa.zwy3 = Math.Round(ave3 / average.Count, 2, MidpointRounding.AwayFromZero);
                }
                else if (windPressureTest == PublicEnum.WindPressureTest.FStart || windPressureTest == PublicEnum.WindPressureTest.FSafety)
                {
                    pa.fwy1 = Math.Round(ave1 / average.Count, 2, MidpointRounding.AwayFromZero);
                    pa.fwy2 = Math.Round(ave2 / average.Count, 2, MidpointRounding.AwayFromZero);
                    pa.fwy3 = Math.Round(ave3 / average.Count, 2, MidpointRounding.AwayFromZero);
                }

                //清空初始化
                BindData(true);

                if (windPressureTest == PublicEnum.WindPressureTest.ZStart || windPressureTest == PublicEnum.WindPressureTest.FStart)
                {
                    var def_LX = 0;
                    int.TryParse(txt_lx.Text, out def_LX);

                    var lx = windPressureTest == PublicEnum.WindPressureTest.ZStart ? pa.zlx : pa.flx;
                    if (lx < def_LX)
                    {
                        Stop();
                        OpenBtnType();

                        //todo:使用那个lx?
                        double lx2 = 0;
                        double.TryParse(txt_lx.Text, out lx2);
                        double zy = 0;
                        double fy = 0;

                        Formula.GetKFY(windPressureDGV, DefaultBase.BarLength, lx2, ref zy, ref fy);
                        if (zy != -100)
                        {
                            txt_p1.Text = Math.Round(zy, 0).ToString();
                        }
                        if (fy != -100)
                        {
                            txt_f_p1.Text = Math.Round(fy, 0).ToString();
                        }
                    }
                }

                tim_static1.Enabled = false;
                //this.tim_static.Enabled = false;
                average = new List<Tuple<double, double, double>>();
                staticIndex = 0;
                IsOk = true;
            }
            staticIndex++;
        }
        private void btn_fyyb_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            var res = _serialPortClient.Send_FY_Btn(BFMCommand.风压负压预备, false);
            if (!res)
            {
                MessageBox.Show("负压预备异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            windPressureTest = PublicEnum.WindPressureTest.FReady;
            DisableBtnType();
            btn_fyyb.BackColor = Color.Green;
        }

        private void btn_fyks_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            var res = _serialPortClient.Send_FY_Btn(BFMCommand.风压负压开始, false);
            if (!res)
            {
                MessageBox.Show("负压开始异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            windPressureTest = PublicEnum.WindPressureTest.FStart;
            DisableBtnType();
            btn_fyks.BackColor = Color.Green;
            complete = new List<int>();

            tim_fy1 = new System.Timers.Timer(1000);
            tim_fy1.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer);  //到达时间的时候执行倒计时事件timeout；
            tim_fy1.Enabled = true;

        }

        private void btn_datahandle_Click(object sender, EventArgs e)
        {
            double lx = 0;
            double.TryParse(txt_lx.Text, out lx);
            double zy = 0;
            double fy = 0;

            Formula.GetKFY(windPressureDGV, DefaultBase.BarLength, lx, ref zy, ref fy);
            if (zy != -100)
            {
                txt_p1.Text = zy > 0 ? Math.Round(zy, 0).ToString() : "0";
            }
            if (fy != -100)
                txt_f_p1.Text = fy > 0 ? Math.Round(fy, 0).ToString() : "0";

            currentkPa = 0;
        }



        private void btn_fff_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;


            int value = 0;
            int.TryParse(txt_f_p2.Text, out value);

            if (value == 0)
                return;
            var res = _serialPortClient.Set_FY_Value(BFMCommand.负反复数值, BFMCommand.负反复, value, false);
            if (!res)
            {
                MessageBox.Show("负反复异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            windPressureTest = PublicEnum.WindPressureTest.FRepeatedly;
            this.tim_fy.Enabled = true;
            DisableBtnType();
            btn_fff.BackColor = Color.Green;
        }

        private void btn_zaq_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;

            int value = 0;
            int.TryParse(txt_p3.Text, out value);

            if (value == 0)
                return;

            var res = _serialPortClient.Set_FY_Value(BFMCommand.正安全数值, BFMCommand.正安全, value);
            if (!res)
            {
                MessageBox.Show("正安全异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            complete = new List<int>();
            windPressureTest = PublicEnum.WindPressureTest.ZSafety;
            //this.tim_fy.Enabled = true;
            DisableBtnType();
            btn_zaq.BackColor = Color.Green;
            tim_fy1 = new System.Timers.Timer(1000);
            tim_fy1.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer);  //到达时间的时候执行倒计时事件timeout；
            tim_fy1.Enabled = true;
        }


        /// <summary>
        /// 记录当前锚点
        /// </summary>
        private int currentkPa = 0;

        private void tim_fy_Tick(object sender, EventArgs e)
        {

            if (!IsOk)
                return;

            if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                var cyValue = _serialPortClient.GetCY_High();
                var val = defKFYPa.Find(t => cyValue >= t.MinValue && cyValue <= t.MaxValue);
                if (val != null && !complete.Exists(t => t == val.Value))
                {
                    complete.Add(val.Value);
                    currentkPa = val.Value;

                    tim_static.Enabled = true;
                }
            }

            if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                var cyValue = _serialPortClient.GetCY_High();
                var val = defKFYPa.Find(t => cyValue >= t._MinValue && cyValue <= t._MaxValue);
                if (val != null && !complete.Exists(t => t == val.Value))
                {
                    complete.Add(val.Value);
                    currentkPa = val.Value;

                    tim_static.Enabled = true;
                }
            }

            if (windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.FSafety)
            {
                if (!complete.Exists(t => t == -1))
                {
                    complete.Add(-1);
                    currentkPa = -1;

                    tim_static.Enabled = true;
                }
            }
        }
        //稳压次数
        private int staticIndex = 0;
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
            IsOk = false;
            var maxIndex = 0;
            var common = "";
            if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                common = BFMCommand.风压_正压是否计时;
                maxIndex = 6;
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                common = BFMCommand.风压_负压是否计时;
                maxIndex = 6;
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZSafety)
            {
                common = BFMCommand.风压安全_正压是否计时;
                maxIndex = 4;
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FSafety)
            {
                common = BFMCommand.风压安全_负压是否计时;
                maxIndex = 4;
            }
            var res = _serialPortClient.Read_FY_Static_IsStart(common);
            if (!res)
            {
                return;
            }


            if (staticIndex < maxIndex)
            {
                var _displace1 = _serialPortClient.GetDisplace1();
                var _displace2 = _serialPortClient.GetDisplace2();
                var _displace3 = _serialPortClient.GetDisplace3();
                average.Add(new Tuple<double, double, double>(_displace1, _displace2, _displace3));
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
                var pa = windPressureDGV.Find(t => t.PaValue == currentkPa);

                if (windPressureTest == PublicEnum.WindPressureTest.ZStart || windPressureTest == PublicEnum.WindPressureTest.ZSafety)
                {
                    pa.zwy1 = Math.Round(ave1 / average.Count, 2, MidpointRounding.AwayFromZero);
                    pa.zwy2 = Math.Round(ave2 / average.Count, 2, MidpointRounding.AwayFromZero);
                    pa.zwy3 = Math.Round(ave3 / average.Count, 2, MidpointRounding.AwayFromZero);
                }
                else if (windPressureTest == PublicEnum.WindPressureTest.FStart || windPressureTest == PublicEnum.WindPressureTest.FSafety)
                {
                    pa.fwy1 = Math.Round(ave1 / average.Count, 2, MidpointRounding.AwayFromZero);
                    pa.fwy2 = Math.Round(ave2 / average.Count, 2, MidpointRounding.AwayFromZero);
                    pa.fwy3 = Math.Round(ave3 / average.Count, 2, MidpointRounding.AwayFromZero);
                }

                //清空初始化
                BindData(true);

                if (windPressureTest == PublicEnum.WindPressureTest.ZStart || windPressureTest == PublicEnum.WindPressureTest.FStart)
                {
                    var def_LX = 0;
                    int.TryParse(txt_lx.Text, out def_LX);

                    var lx = windPressureTest == PublicEnum.WindPressureTest.ZStart ? pa.zlx : pa.flx;
                    if (lx < def_LX)
                    {
                        //  BindData(true);
                        Stop();
                        OpenBtnType();

                        //todo:使用那个lx?
                        double lx2 = 0;
                        double.TryParse(txt_lx.Text, out lx2);
                        double zy = 0;
                        double fy = 0;

                        Formula.GetKFY(windPressureDGV, DefaultBase.BarLength, lx2, ref zy, ref fy);
                        if (zy != -100)
                        {
                            txt_p1.Text = Math.Round(zy, 0).ToString();
                        }
                        if (fy != -100)
                        {
                            txt_f_p1.Text = Math.Round(fy, 0).ToString();
                        }
                    }
                }

                this.tim_static.Enabled = false;
                average = new List<Tuple<double, double, double>>();
                staticIndex = 0;
                IsOk = true;
            }
            staticIndex++;
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
                    return;
                }
                if (value == 3)
                {
                    windPressureTest = PublicEnum.WindPressureTest.Stop;
                    OpenBtnType();
                    this.tim_fy.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZStart)
            {
                double value = _serialPortClient.Read_FY_BtnType(BFMCommand.风压正压开始结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    MessageBox.Show("风压正压开始结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (value >= 15)
                {
                    windPressureTest = PublicEnum.WindPressureTest.Stop;
                    IsStart = false;
                    OpenBtnType();
                    this.tim_fy.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FReady)
            {
                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.风压负压预备结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    MessageBox.Show("风压负压预备结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (value == 3)
                {
                    windPressureTest = PublicEnum.WindPressureTest.Stop;
                    OpenBtnType();
                    this.tim_fy.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FStart)
            {
                double value = _serialPortClient.Read_FY_BtnType(BFMCommand.风压负压开始结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    MessageBox.Show("风压负压开始结束状态异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (value >= 15)
                {
                    IsStart = false;
                    OpenBtnType();
                    this.tim_fy.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZRepeatedly)
            {
                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.正反复结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    return;
                }
                if (value == 5)
                {
                    OpenBtnType();
                    this.tim_fy.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FRepeatedly)
            {
                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.负反复结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    return;
                }
                if (value == 5)
                {
                    OpenBtnType();
                    this.tim_fy.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.ZSafety || windPressureTest == PublicEnum.WindPressureTest.ZPmax)
            {

                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.正安全结束, ref IsSeccess);

                if (!IsSeccess)
                {
                    return;
                }
                if (value > 10)
                {
                    OpenBtnType();
                    this.tim_fy.Enabled = false;
                }
            }
            else if (windPressureTest == PublicEnum.WindPressureTest.FSafety || windPressureTest == PublicEnum.WindPressureTest.FPmax)
            {
                int value = _serialPortClient.Read_FY_BtnType(BFMCommand.负安全结束, ref IsSeccess);
                if (!IsSeccess)
                {
                    return;
                }
                if (value > 10)
                {
                    OpenBtnType();
                    this.tim_fy.Enabled = false;
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
            this.btn_zpmax.Enabled = true;
            this.btn_fpmax.Enabled = true;

            this.btn_zyyb.BackColor = Color.Transparent;
            this.btn_zyks.BackColor = Color.Transparent;
            this.btn_fyyb.BackColor = Color.Transparent;
            this.btn_fyks.BackColor = Color.Transparent;
            this.btn_zff.BackColor = Color.Transparent;
            this.btn_fff.BackColor = Color.Transparent;
            this.btn_zaq.BackColor = Color.Transparent;
            this.btnfaq.BackColor = Color.Transparent;
            this.btn_datahandle.BackColor = Color.Transparent;

            this.btn_zpmax.BackColor = Color.Transparent;
            this.btn_fpmax.BackColor = Color.Transparent;
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

            this.btn_zpmax.Enabled = false;

            this.btn_fpmax.Enabled = false;

            this.btn_zyyb.BackColor = Color.Transparent;
            this.btn_zyks.BackColor = Color.Transparent;
            this.btn_fyyb.BackColor = Color.Transparent;
            this.btn_fyks.BackColor = Color.Transparent;

            this.btn_zff.BackColor = Color.Transparent;
            this.btn_fff.BackColor = Color.Transparent;
            this.btn_zaq.BackColor = Color.Transparent;
            this.btnfaq.BackColor = Color.Transparent;
            this.btn_datahandle.BackColor = Color.Transparent;

            this.btn_zpmax.BackColor = Color.Transparent;

            this.btn_fpmax.BackColor = Color.Transparent;

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
                MessageBox.Show("负安全异常！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            complete = new List<int>();
            windPressureTest = PublicEnum.WindPressureTest.FSafety;
            DisableBtnType();

            btnfaq.BackColor = Color.Green;
            tim_fy1 = new System.Timers.Timer(1000);
            tim_fy1.Elapsed += new System.Timers.ElapsedEventHandler(fyTimer);  //到达时间的时候执行倒计时事件timeout；
            tim_fy1.Enabled = true;
        }

        private void btn_wygl_Click(object sender, EventArgs e)
        {
            if (!_serialPortClient.sp.IsOpen)
                return;
            _serialPortClient.SendWYGL();
        }

        /// <summary>
        /// 急停
        /// </summary>
        private void Stop()
        {
            var res = _serialPortClient.Stop();
            if (!res) { }
            //MessageBox.Show("急停异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            //获得当前选中的行   
            int rowindex = e.RowIndex;
            var name = dgv_WindPressure.Rows[rowindex].Cells[0].Value.ToString();

            var z_wy1 = dgv_WindPressure.Rows[rowindex].Cells[1].Value.ToString();
            var z_wy2 = dgv_WindPressure.Rows[rowindex].Cells[2].Value.ToString();
            var z_wy3 = dgv_WindPressure.Rows[rowindex].Cells[3].Value.ToString();

            var f_wy1 = dgv_WindPressure.Rows[rowindex].Cells[6].Value.ToString();
            var f_wy2 = dgv_WindPressure.Rows[rowindex].Cells[7].Value.ToString();
            var f_wy3 = dgv_WindPressure.Rows[rowindex].Cells[8].Value.ToString();


            var item = windPressureDGV.Find(t => t.Pa == name);
            item.zwy1 = double.Parse(z_wy1);
            item.zwy2 = double.Parse(z_wy2);
            item.zwy3 = double.Parse(z_wy3);
            item.fwy1 = double.Parse(f_wy1);
            item.fwy2 = double.Parse(f_wy2);
            item.fwy3 = double.Parse(f_wy3);

            BindData(true);
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Stop();
            OpenBtnType();
            // lbl_setYL.Text = "0";
            windPressureTest = PublicEnum.WindPressureTest.Stop;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (DefaultBase.LockPoint)
            {
                if (!rdb_DWDD1.Checked && !rdb_DWDD3.Checked)
                {
                    MessageBox.Show("请选择位移！", "请选择位移！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            var jc = int.Parse(txt_gbjc.Text);
            if (AddKfyInfo(jc))
            {
                MessageBox.Show("处理成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            complete = new List<int>();
        }

        private void btn_gbjc_Click(object sender, EventArgs e)
        {
            var value = int.Parse(txt_gbjc.Text);
            var res = _serialPortClient.SendGBJC(value);
            if (!res)
            {
                MessageBox.Show("改变级差异常", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            defKFYPa = new List<DefKFYPa>();
            for (int i = 1; i < 9; i++)
            {
                defKFYPa.Add(new DefKFYPa() { Value = value * i });
            }
            windPressureDGV = new List<WindPressureDGV>();

            #region 添加默认
            foreach (var paValue in defKFYPa)
            {
                windPressureDGV.Add(new WindPressureDGV()
                {
                    Pa = paValue.Value + "Pa",
                    PaValue = paValue.Value,
                    zwy1 = 0,
                    zwy2 = 0,
                    zwy3 = 0,
                    fwy1 = 0,
                    fwy2 = 0,
                    fwy3 = 0,
                });
            }
            //极差
            for (int i = 0; i < 3; i++)
            {
                var name = "";
                var paValue = 0;
                if (i == 0)
                {
                    name = "P3阶段";
                    paValue = -1;
                }
                else if (i == 1)
                {
                    name = "P3残余变形";
                    paValue = -2;
                }

                else if (i == 2)
                {
                    name = "PMax/残余变形";
                    paValue = -3;
                }
                windPressureDGV.Add(new WindPressureDGV()
                {
                    Pa = name,
                    PaValue = paValue,
                    zwy1 = 0,
                    zwy2 = 0,
                    zwy3 = 0,
                    fwy1 = 0,
                    fwy2 = 0,
                    fwy3 = 0,
                });
            }
            #endregion

            BindData(true);
        }

        private void btn_zpmax_Click(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(txt_zpmax.Text, out value);

            var res = _serialPortClient.Set_FY_Value(BFMCommand.正PMAX值, BFMCommand.正PMAX, value);
            if (!res)
            {
                MessageBox.Show("正pmax！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            windPressureTest = PublicEnum.WindPressureTest.ZPmax;

            DisableBtnType();

            btn_zpmax.BackColor = Color.Green;
        }

        private void btn_fpmax_Click(object sender, EventArgs e)
        {
            int value = 0;
            int.TryParse(txt_fpmax.Text, out value);
            var res = _serialPortClient.Set_FY_Value(BFMCommand.负PMAX值, BFMCommand.负PMAX, value);
            if (!res)
            {
                MessageBox.Show("负pmax！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            windPressureTest = PublicEnum.WindPressureTest.FPmax;

            DisableBtnType();

            btn_fpmax.BackColor = Color.Green;
        }

        private void btn_zp3cybx_Click(object sender, EventArgs e)
        {
            var info = windPressureDGV.Find(t => t.PaValue == -2);
            if (info != null)
            {
                info.zwy1 = _serialPortClient.GetDisplace1();
                info.zwy2 = _serialPortClient.GetDisplace2();
                info.zwy3 = _serialPortClient.GetDisplace3();
            }
            BindData(true);
        }

        private void btn_fp3cybx_Click(object sender, EventArgs e)
        {
            var info = windPressureDGV.Find(t => t.PaValue == -2);
            if (info != null)
            {
                info.fwy1 = _serialPortClient.GetDisplace1();
                info.fwy2 = _serialPortClient.GetDisplace2();
                info.fwy3 = _serialPortClient.GetDisplace3();
            }
            BindData(true);
        }

        private void btn_zpmax_cybx_Click(object sender, EventArgs e)
        {
            var info = windPressureDGV.Find(t => t.PaValue == -3);
            if (info != null)
            {
                info.zwy1 = _serialPortClient.GetDisplace1();
                info.zwy2 = _serialPortClient.GetDisplace2();
                info.zwy3 = _serialPortClient.GetDisplace3();
            }
            BindData(true);
        }

        private void btn_fpmax_cybx_Click(object sender, EventArgs e)
        {
            var info = windPressureDGV.Find(t => t.PaValue == -3);
            if (info != null)
            {
                info.fwy1 = _serialPortClient.GetDisplace1();
                info.fwy2 = _serialPortClient.GetDisplace2();
                info.fwy3 = _serialPortClient.GetDisplace3();
            }
            BindData(true);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btn_gcjc_Click(object sender, EventArgs e)
        {
            IsGCJC = !IsGCJC;

            if (IsGCJC)
            {
                btn_gcjc.BackColor = Color.Green;
            }
            else
            {
                btn_gcjc.BackColor = Color.Transparent;
            }
        }
    }
}

public class DefKFYPa
{

    public int Value { get; set; }

    public int MinValue
    {
        get
        {
            return this.Value - 10;
        }
    }

    public int MaxValue
    {
        get
        {
            return this.Value + 10;
        }
    }

    public int _MinValue
    {
        get
        {
            return -this.Value - 10;
        }
    }

    public int _MaxValue
    {
        get
        {
            return -this.Value + 10;
        }
    }
}