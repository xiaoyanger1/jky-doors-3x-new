using text.doors.Common;
using text.doors.dal;
using text.doors.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using text.doors.Model.DataBase;
using text.doors.Default;

namespace text.doors.Detection
{
    public partial class DetectionSet : Form
    {
        private static Young.Core.Logger.ILog Logger = Young.Core.Logger.LoggerManager.Current();
        //当前温度
        public double _temperature { set; get; }
        //当前压力
        public double _temppressure { set; get; }

        private string _tempCode = "";
        private string _tempTong = "";
        private SerialPortClient _serialPortClient;

        public DetectionSet() { }
        public DetectionSet(SerialPortClient serialPortClient, double temperature, double temppressure, string tempCode, string tempTong)
        {
            this._serialPortClient = serialPortClient;
            this._temperature = temperature;
            this._temppressure = temppressure;
            this._tempCode = tempCode;
            this._tempTong = tempTong;
            InitializeComponent();
            Init();
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            string code = btn_JianYanBianHao.Text;

            if (string.IsNullOrWhiteSpace(txt_jianceshuliang.Text))
            {
                MessageBox.Show("请输入规格数量！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(cb_DangQianDangHao.Text))
            {
                MessageBox.Show("请设置樘号！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("请设置当前编号！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DAL_dt_Settings dal = new DAL_dt_Settings();
            try
            {
                var setting = GetSettings();
                var tong = this.cb_DangQianDangHao.Text;
                if (dal.Add(setting, tong))
                {
                  //  MessageBox.Show("设定完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //获取樘号
                    deleBottomTypeEvent(GetBottomType(true));
                    DefaultBase.base_SpecCount = int.Parse(txt_jianceshuliang.Text);
                    DefaultBase.base_TestItem = cb_JianCeXiangMu.Text;
                    DefaultBase.LockPoint = cbb_danshandansuodian.Text == "是" ? true : false;

                    int def = 0;
                    int.TryParse(txt_ganjianchangdu.Text, out def);

                    DefaultBase.BarLength = def;
                    this.btn_add.Enabled = true;
                    this.btn_select.Enabled = true;
                    this.btn_delete.Enabled = true;
                    this.btn_Ok.Enabled = true;

                   // this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加异常！", "异常", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.Error(ex);
            }
        }

        public void GetDangHaoTrigger()
        {
            //获取樘号
            deleBottomTypeEvent(GetBottomType(false));
        }

        private void Init()
        {
            BindInfoText();
            BindDangQianDangHao();
        }
        private void SelectDangHao(object sender, text.doors.Detection.Select_Code.TransmitEventArgs e)
        {
            _tempCode = e.Code;
            _tempTong = e.Tong;
            Init();
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            Select_Code sc = new Select_Code();
            sc.Transmit += new Select_Code.TransmitHandler(SelectDangHao);
            sc.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {

            MessageBox.Show("您是否删除", " 永久性删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.ServiceNotification);

            if (new DAL_dt_Info().delete_dt_Info(btn_JianYanBianHao.Text))
            {
                _tempCode = "";
                Init();
            }
        }


        #region Info
        /// <summary>
        /// 绑定控件
        /// </summary>
        private void BindInfoText()
        {
            try
            {
                DataTable dt = new DAL_dt_Settings().Getdt_SettingsByCode(_tempCode);
                if (dt != null)
                {

                    this.txt_WeiTuoBianHao.Text = dt.Rows[0]["weituobianhao"].ToString();
                    this.txt_WeiTuoDanWei.Text = dt.Rows[0]["weituodanwei"].ToString();
                    this.txt_dizhi.Text = dt.Rows[0]["dizhi"].ToString();
                    this.txt_dianhua.Text = dt.Rows[0]["dianhua"].ToString();
                    this.txt_chouyangriqi.Text = dt.Rows[0]["chouyangriqi"].ToString();
                    this.txt_chouyangdidian.Text = dt.Rows[0]["chouyangdidian"].ToString();
                    this.txt_gongchengmingcheng.Text = dt.Rows[0]["gongchengmingcheng"].ToString();
                    this.txt_gongchengdidian.Text = dt.Rows[0]["gongchengdidian"].ToString();
                    this.txt_shengchandanwei.Text = dt.Rows[0]["shengchandanwei"].ToString();
                    this.cb_JianCeXiangMu.Text = dt.Rows[0]["jiancexiangmu"].ToString();
                    this.txt_jiancedidian.Text = dt.Rows[0]["jiancedidian"].ToString();
                    this.txt_JianCeRiQi.Text = dt.Rows[0]["jianceriqi"].ToString();
                    this.txt_jianceshebei.Text = dt.Rows[0]["jianceshebei"].ToString();
                    this.cb_jianceyiju.Text = dt.Rows[0]["jianceyiju"].ToString();

                    this.txt_YangPinMingCheng.Text = dt.Rows[0]["yangpinmingcheng"].ToString();
                    this.txt_yangpinshangbiao.Text = dt.Rows[0]["yangpinshangbiao"].ToString();
                    this.txt_yangpinzhuangtai.Text = dt.Rows[0]["yangpinzhuangtai"].ToString();
                    this.txt_GuiGeXingHao.Text = dt.Rows[0]["guigexinghao"].ToString();
                    this.cb_KaiQiFangShi.Text = dt.Rows[0]["kaiqifangshi"].ToString();
                    this.cb_mianbanpinzhong.Text = dt.Rows[0]["mianbanpinzhong"].ToString();
                    this.txt_zuidamianban.Text = dt.Rows[0]["zuidamianban"].ToString();
                    this.txt_mianbanhoudu.Text = dt.Rows[0]["mianbanhoudu"].ToString();
                    this.cb_anzhuangfangshi.Text = dt.Rows[0]["anzhuangfangshi"].ToString();
                    this.cb_mianbanxiangqian.Text = dt.Rows[0]["mianbanxiangqian"].ToString();
                    this.cb_KuangShanMiFang.Text = dt.Rows[0]["kuangshanmifeng"].ToString();
                    this.txt_wujinpeijian.Text = dt.Rows[0]["wujinpeijian"].ToString();
                    this.txt_jianceshuliang.Text = dt.Rows[0]["jianceshuliang"].ToString();
                    this.cb_DangQianDangHao.Text = dt.Rows[0]["dangqiandanghao"].ToString();

                    this.txt_DangQianWenDu.Text = dt.Rows[0]["dangqianwendu"].ToString();
                    this.txt_DaQiYaLi.Text = dt.Rows[0]["daqiyali"].ToString();
                    this.txt_KaiQiFengChang.Text = dt.Rows[0]["kaiqifengchang"].ToString();
                    this.txt_shijianmianji.Text = dt.Rows[0]["shijianmianji"].ToString();
                    this.txt_ganjianchangdu.Text = dt.Rows[0]["ganjianchangdu"].ToString();
                    this.txt_penlinshuiliang.Text = dt.Rows[0]["penlinshuiliang"].ToString();
                    this.txt_qimidangweifengchangshejizhi.Text = dt.Rows[0]["qimidanweifengchangshejizhi"].ToString();
                    this.txt_QiMiDanWeiMianJiSheJiZhi.Text = dt.Rows[0]["qimidanweimianjishejizhi"].ToString();
                    this.txt_shuimijingyashejizhi.Text = dt.Rows[0]["shuimijingyashejizhi"].ToString();
                    this.txt_shuimidongyashejizhi.Text = dt.Rows[0]["shuimidongyashejizhi"].ToString();
                    this.txt_kangfengyazhengyashejizhi.Text = dt.Rows[0]["kangfengyazhengyashejizhi"].ToString();
                    this.txt_kangfengyafuyashejizhi.Text = dt.Rows[0]["kangfengyafuyashejizhi"].ToString();
                    this.cbb_danshandansuodian.Text = dt.Rows[0]["danshandansuodian"].ToString();

                    this.txt_kangfengyazhengp3shejizhi.Text = dt.Rows[0]["kangfengyazhengp3shejizhi"].ToString();
                    this.txt_kangfengyazhengpmaxshejizhi.Text = dt.Rows[0]["kangfengyazhengpmaxshejizhi"].ToString();


                    _tempCode = dt.Rows[0]["dt_Code"].ToString();
                    if (_tempTong == "")
                        _tempTong = dt.Rows[0]["info_DangH"].ToString();
                    cb_DangQianDangHao.Text = dt.Rows[0]["info_DangH"].ToString();
                    btn_JianYanBianHao.Text = dt.Rows[0]["dt_Code"].ToString();
                    this.cb_JianCeXiangMu.Enabled = false;
                    this.txt_jianceshuliang.Enabled = false;
                }
                else
                {
                    btn_JianYanBianHao.Text = DateTime.Now.ToString("yyyyMMdd") + "-01";
                }

                if (string.IsNullOrEmpty(txt_jianceshuliang.Text))
                {
                    txt_jianceshuliang.Text = "3";
                }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logger.Error(ex);
            }
        }


        private Model_dt_Settings GetSettings()
        {
            Model_dt_Settings model = new Model_dt_Settings();
            model.dt_Create = DateTime.Now;
            model.dt_Code = this.btn_JianYanBianHao.Text;


            model.weituobianhao = this.txt_WeiTuoBianHao.Text;
            model.weituodanwei = this.txt_WeiTuoDanWei.Text;
            model.dizhi = this.txt_dizhi.Text;
            model.dianhua = this.txt_dianhua.Text;
            model.chouyangriqi = this.txt_chouyangriqi.Text;
            model.chouyangdidian = this.txt_chouyangdidian.Text;
            model.gongchengmingcheng = this.txt_gongchengmingcheng.Text;
            model.gongchengdidian = this.txt_gongchengdidian.Text;
            model.shengchandanwei = this.txt_shengchandanwei.Text;
            model.jiancexiangmu = this.cb_JianCeXiangMu.Text;
            model.jiancedidian = this.txt_jiancedidian.Text;
            model.jianceriqi = this.txt_JianCeRiQi.Text;
            model.jianceshebei = this.txt_jianceshebei.Text;
            model.jianceyiju = this.cb_jianceyiju.Text;

            model.yangpinmingcheng = this.txt_YangPinMingCheng.Text;
            model.yangpinshangbiao = this.txt_yangpinshangbiao.Text;
            model.yangpinzhuangtai = this.txt_yangpinzhuangtai.Text;
            model.guigexinghao = this.txt_GuiGeXingHao.Text;
            model.kaiqifangshi = this.cb_KaiQiFangShi.Text;
            model.mianbanpinzhong = this.cb_mianbanpinzhong.Text;
            model.zuidamianban = this.txt_zuidamianban.Text;
            model.mianbanhoudu = this.txt_mianbanhoudu.Text;
            model.anzhuangfangshi = this.cb_anzhuangfangshi.Text;
            model.mianbanxiangqian = this.cb_mianbanxiangqian.Text;
            model.kuangshanmifeng = this.cb_KuangShanMiFang.Text;
            model.wujinpeijian = this.txt_wujinpeijian.Text;
            model.jianceshuliang = this.txt_jianceshuliang.Text;
            model.dangqiandanghao = this.cb_DangQianDangHao.Text;

            model.dangqianwendu = this.txt_DangQianWenDu.Text;
            model.daqiyali = this.txt_DaQiYaLi.Text;
            model.kaiqifengchang = this.txt_KaiQiFengChang.Text;
            model.shijianmianji = this.txt_shijianmianji.Text;
            model.ganjianchangdu = this.txt_ganjianchangdu.Text;
            model.penlinshuiliang = this.txt_penlinshuiliang.Text;
            model.qimidanweifengchangshejizhi = this.txt_qimidangweifengchangshejizhi.Text;
            model.qimidanweimianjishejizhi = this.txt_QiMiDanWeiMianJiSheJiZhi.Text;
            model.shuimijingyashejizhi = this.txt_shuimijingyashejizhi.Text;
            model.shuimidongyashejizhi = this.txt_shuimidongyashejizhi.Text;
            model.kangfengyazhengyashejizhi = this.txt_kangfengyazhengyashejizhi.Text;
            model.kangfengyafuyashejizhi = this.txt_kangfengyafuyashejizhi.Text;
            model.danshandansuodian = this.cbb_danshandansuodian.Text;
            model.kangfengyazhengp3shejizhi = this.txt_kangfengyazhengp3shejizhi.Text;
            model.kangfengyazhengpmaxshejizhi = this.txt_kangfengyazhengpmaxshejizhi.Text;
            return model;
        }

        #endregion


        private void btn_add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(btn_JianYanBianHao.Text))
            {
                MessageBox.Show("请输入编号！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var arr = btn_JianYanBianHao.Text.Split('-');
            if (arr.Length == 1)
            {
                MessageBox.Show("编号格式有误，请输入如d-1格式！", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btn_JianYanBianHao.Text = arr[0] + "-" + (int.Parse(arr[1]) + 1).ToString();
            this.cb_JianCeXiangMu.Enabled = true;
            this.txt_jianceshuliang.Enabled = true;
            this.btn_add.Enabled = false;
            this.btn_select.Enabled = false;
            this.btn_delete.Enabled = false;
            this.btn_Ok.Enabled = true;
        
            txt_DaQiYaLi.Text = _serialPortClient.GetDQYLXS().ToString();

            txt_DangQianWenDu.Text = _serialPortClient.GetWDXS().ToString();
            BindDangQianDangHao();
        }


        private void BindDangQianDangHao()
        {
            try
            {
                cb_DangQianDangHao.Items.Clear();
                int count = int.Parse(txt_jianceshuliang.Text);
                for (int i = 1; i <= count; i++)
                {
                    cb_DangQianDangHao.Items.Add("第" + i + "樘");
                }

                if (_tempTong != "")
                {
                    cb_DangQianDangHao.Text = _tempTong;
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                MessageBox.Show("规格数量请输入数字", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btn_GuiGeShuLiang_KeyUp(object sender, KeyEventArgs e)
        {

            try
            {
                if (int.Parse(txt_jianceshuliang.Text) > 3)
                {
                    MessageBox.Show("最大只能输入三樘", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_jianceshuliang.Text = "";
                    return;
                }
            }
            catch
            {
                MessageBox.Show("最大只能输入三樘", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_jianceshuliang.Text))
            {
                MessageBox.Show("请填写规格数量", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            BindDangQianDangHao();
        }

        #region 底部状态栏赋值

        private BottomType GetBottomType(bool ISOK)
        {
            BottomType bt = new BottomType(cb_DangQianDangHao.Text, btn_JianYanBianHao.Text, cbb_danshandansuodian.Text == "是" ? true : false, ISOK);
            return bt;
        }

        //委托
        public delegate void deleBottomType(BottomType bottomType);
        public deleBottomType deleBottomTypeEvent;//委托事件

        /// <summary>
        /// 底部状态栏
        /// </summary>
        public class BottomType
        {
            private string _code;
            private string _tong;
            private bool _lockPoint;
            private bool _isok;

            public BottomType(string code, string tong, bool lockPoint, bool isOK)
            {
                this._code = tong;
                this._tong = code;
                this._isok = isOK;
                this._lockPoint = lockPoint;

            }
            public string Code { get { return _code; } }
            public string Tong { get { return _tong; } }
            public bool ISOK { get { return _isok; } }

            public bool LockPoint { get { return _isok; } }
        }
        #endregion
    }
}
