using text.doors.Common;
using text.doors.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Model.DataBase;
using Young.Core.SQLite;
using System.Runtime.InteropServices.WindowsRuntime;
using NPOI.SS.Formula.Functions;
using System.Windows.Forms;
using text.doors.Detection;

namespace text.doors.dal
{
    public class DAL_dt_Settings
    {
        /// <summary>
        /// 添加基本编号设置
        /// </summary>
        public bool Add(Model_dt_Settings model, string tong)
        {
            var res = false;
            string delsql = "delete from dt_Settings where dt_Code='" + model.dt_Code + "'";
            SQLiteHelper.ExecuteNonQuery(delsql);

            #region 添加主表
            string sql = string.Format(@"
                                        insert into dt_Settings
                                        (
                                        weituobianhao,
                                        weituodanwei,
                                        dizhi,
                                        dianhua,
                                        chouyangriqi,
                                        chouyangdidian,
                                        gongchengmingcheng,
                                        gongchengdidian,
                                        shengchandanwei,
                                        jiancexiangmu,
                                        jiancedidian,
                                        jianceriqi,
                                        jianceshebei,
                                        jianceyiju,

                                        yangpinmingcheng,
                                        yangpinshangbiao,
                                        yangpinzhuangtai,
                                        guigexinghao,
                                        kaiqifangshi,
                                        mianbanpinzhong,
                                        zuidamianban,
                                        mianbanhoudu,
                                        anzhuangfangshi,
                                        mianbanxiangqian,
                                        kuangshanmifeng,
                                        wujinpeijian,
                                        jianceshuliang,
                                        dangqiandanghao,

                                        dangqianwendu,
                                        daqiyali,
                                        kaiqifengchang,
                                        shijianmianji,
                                        ganjianchangdu,
                                        penlinshuiliang,
                                        qimidanweifengchangshejizhi,
                                        qimidanweimianjishejizhi,
                                        shuimijingyashejizhi,
                                        shuimidongyashejizhi,
                                        kangfengyazhengyashejizhi,
                                        kangfengyafuyashejizhi,                           
                                        danshandansuodian,

                                        dt_Code,
                                        dt_Create,
                                        kangfengyazhengp3shejizhi,
                                        kangfengyazhengpmaxshejizhi
                                        )
                                        VALUES
                                        (
                                        '{0}','{1}','{2}' ,'{3}','{4}' , '{5}' , '{6}','{7}' ,'{8}' ,'{9}' , '{10}','{11}'  ,  '{12}' ,'{13}' ,'{14}' ,'{15}'  ,'{16}' ,'{17}','{18}' , '{19}' ,
                                        '{20}' , '{21}' ,'{22}' ,'{23}','{24}' ,'{25}' ,'{26}' ,'{27}','{28}' , '{29}' ,'{30}','{31}' ,'{32}','{33}','{34}' ,'{35}' ,'{36}' ,'{37}' ,'{38}' ,
                                        '{39}' ,'{40}','{41}' ,datetime('now'),'{42}','{43}')",
                                        model.weituobianhao,
                                        model.weituodanwei,
                                        model.dizhi,
                                        model.dianhua,
                                        model.chouyangriqi,
                                        model.chouyangdidian,
                                        model.gongchengmingcheng,
                                        model.gongchengdidian,
                                        model.shengchandanwei,
                                        model.jiancexiangmu,
                                        model.jiancedidian,
                                        model.jianceriqi,
                                        model.jianceshebei,
                                        model.jianceyiju,

                                        model.yangpinmingcheng,
                                        model.yangpinshangbiao,
                                        model.yangpinzhuangtai,
                                        model.guigexinghao,
                                        model.kaiqifangshi,
                                        model.mianbanpinzhong,
                                        model.zuidamianban,
                                        model.mianbanhoudu,
                                        model.anzhuangfangshi,
                                        model.mianbanxiangqian,
                                        model.kuangshanmifeng,
                                        model.wujinpeijian,
                                        model.jianceshuliang,
                                        model.dangqiandanghao,

                                        model.dangqianwendu,
                                        model.daqiyali,
                                        model.kaiqifengchang,
                                        model.shijianmianji,
                                        model.ganjianchangdu,
                                        model.penlinshuiliang,
                                        model.qimidanweifengchangshejizhi,
                                        model.qimidanweimianjishejizhi,
                                        model.shuimijingyashejizhi,
                                        model.shuimidongyashejizhi,
                                        model.kangfengyazhengyashejizhi,
                                        model.kangfengyafuyashejizhi,
                                        model.danshandansuodian,
                                        model.dt_Code,
                                        model.kangfengyazhengp3shejizhi,
                                        model.kangfengyazhengpmaxshejizhi
                                        );
            res = SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
            #endregion

            #region 添加实验樘号记录
            if (res)
            {
                var table = SQLiteHelper.ExecuteDataRow("select * from dt_Info where info_DangH = '" + tong + "' and dt_Code = '" + model.dt_Code + "'")?.Table;
                if (table == null || table.Rows.Count == 0)
                {
                    sql = string.Format("insert into dt_Info (info_DangH,info_Create,dt_Code,Airtight,Watertight,WindPressure) values('{0}',datetime('now'),'{1}',0,0,0)", tong, model.dt_Code);
                    return SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
                }
            }
            #endregion

            return true;
        }


        public List<DictName> GetCodeList()
        {
            List<DictName> lis = new List<DictName>();
            string sql = "select distinct dt_code from  dt_settings order by dt_Create desc";

            var dr = SQLiteHelper.ExecuteDataRow(sql);
            if (dr != null)
            {
                var dt = dr.Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lis.Add(new DictName()
                    {
                        name = dt.Rows[i]["dt_code"].ToString(),
                        id = i
                    });
                }
            }
            return lis;

        }

        /// <summary>
        /// 查询编号，如编号等于null，则查询最近一次数据
        /// </summary>
        /// <param name="code">编号</param>
        /// <returns></returns>
        public DataTable Getdt_SettingsByCode(string code)
        {
            string sql = "";
            if (string.IsNullOrWhiteSpace(code))
            {
                sql = @"select t.*,t1.info_DangH from (select * from dt_Settings order by  dt_Create desc  LIMIT(1) ) t
                        join dt_Info  t1 on t.dt_Code = t1.dt_Code";
            }
            else
            {
                sql = @"select t.*,t1.info_DangH from dt_Settings  t
                            join dt_Info  t1 on t.dt_Code = t1.dt_Code
                            where t.dt_Code ='" + code + "'";
            }
            DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
            if (dr == null)
                return null;
            return dr.Table;
        }

        public List<Model_dt_qm_Info> GetQMListByCode(string code)
        {
            List<Model_dt_qm_Info> list = new List<Model_dt_qm_Info>();

            var dt_qm_Info = SQLiteHelper.ExecuteDataRow("select * from dt_qm_Info where dt_Code='" + code + "' order by  info_DangH")?.Table;
            if (dt_qm_Info != null)
            {
                foreach (DataRow item in dt_qm_Info.Rows)
                {
                    #region
                    Model_dt_qm_Info model = new Model_dt_qm_Info();
                    model.dt_Code = item["dt_Code"].ToString();
                    model.info_DangH = item["info_DangH"].ToString();
                    model.qm_Z_FC = item["qm_Z_FC"].ToString();
                    model.qm_F_FC = item["qm_F_FC"].ToString();
                    model.qm_Z_MJ = item["qm_Z_MJ"].ToString();
                    model.qm_F_MJ = item["qm_F_MJ"].ToString();

                    model.qm_s_z_fj10 = item["qm_s_z_fj10"].ToString();
                    model.qm_s_z_fj30 = item["qm_s_z_fj30"].ToString();
                    model.qm_s_z_fj50 = item["qm_s_z_fj50"].ToString();
                    model.qm_s_z_fj70 = item["qm_s_z_fj70"].ToString();
                    model.qm_s_z_fj100 = item["qm_s_z_fj100"].ToString();
                    model.qm_s_z_fj150 = item["qm_s_z_fj150"].ToString();
                    model.qm_j_z_fj100 = item["qm_j_z_fj100"].ToString();
                    model.qm_j_z_fj70 = item["qm_j_z_fj70"].ToString();
                    model.qm_j_z_fj50 = item["qm_j_z_fj50"].ToString();
                    model.qm_j_z_fj30 = item["qm_j_z_fj30"].ToString();
                    model.qm_j_z_fj10 = item["qm_j_z_fj10"].ToString();

                    model.qm_s_z_zd10 = item["qm_s_z_zd10"].ToString();
                    model.qm_s_z_zd30 = item["qm_s_z_zd30"].ToString();
                    model.qm_s_z_zd50 = item["qm_s_z_zd50"].ToString();
                    model.qm_s_z_zd70 = item["qm_s_z_zd70"].ToString();
                    model.qm_s_z_zd100 = item["qm_s_z_zd100"].ToString();
                    model.qm_s_z_zd150 = item["qm_s_z_zd150"].ToString();
                    model.qm_j_z_zd100 = item["qm_j_z_zd100"].ToString();
                    model.qm_j_z_zd70 = item["qm_j_z_zd70"].ToString();
                    model.qm_j_z_zd50 = item["qm_j_z_zd50"].ToString();
                    model.qm_j_z_zd30 = item["qm_j_z_zd30"].ToString();
                    model.qm_j_z_zd10 = item["qm_j_z_zd10"].ToString();


                    model.qm_s_f_fj10 = item["qm_s_f_fj10"].ToString();
                    model.qm_s_f_fj30 = item["qm_s_f_fj30"].ToString();
                    model.qm_s_f_fj50 = item["qm_s_f_fj50"].ToString();
                    model.qm_s_f_fj70 = item["qm_s_f_fj70"].ToString();
                    model.qm_s_f_fj100 = item["qm_s_f_fj100"].ToString();
                    model.qm_s_f_fj150 = item["qm_s_f_fj150"].ToString();
                    model.qm_j_f_fj100 = item["qm_j_f_fj100"].ToString();
                    model.qm_j_f_fj70 = item["qm_j_f_fj70"].ToString();
                    model.qm_j_f_fj50 = item["qm_j_f_fj50"].ToString();
                    model.qm_j_f_fj30 = item["qm_j_f_fj30"].ToString();
                    model.qm_j_f_fj10 = item["qm_j_f_fj10"].ToString();


                    model.qm_s_f_zd10 = item["qm_s_f_zd10"].ToString();
                    model.qm_s_f_zd30 = item["qm_s_f_zd30"].ToString();
                    model.qm_s_f_zd50 = item["qm_s_f_zd50"].ToString();
                    model.qm_s_f_zd70 = item["qm_s_f_zd70"].ToString();
                    model.qm_s_f_zd100 = item["qm_s_f_zd100"].ToString();
                    model.qm_s_f_zd150 = item["qm_s_f_zd150"].ToString();
                    model.qm_j_f_zd100 = item["qm_j_f_zd100"].ToString();
                    model.qm_j_f_zd70 = item["qm_j_f_zd70"].ToString();
                    model.qm_j_f_zd50 = item["qm_j_f_zd50"].ToString();
                    model.qm_j_f_zd30 = item["qm_j_f_zd30"].ToString();
                    model.qm_j_f_zd10 = item["qm_j_f_zd10"].ToString();
                    model.testcount = int.Parse(item["testcount"].ToString());


                    model.testtype = item["testtype"]?.ToString();
                    model.sjz_z_fj = item["sjz_z_fj"]?.ToString();
                    model.sjz_z_zd = item["sjz_z_zd"]?.ToString();
                    model.sjz_f_zd = item["sjz_f_zd"]?.ToString();
                    model.sjz_f_fj = item["sjz_f_fj"]?.ToString();
                    model.sjz_value = item["sjz_value"]?.ToString();


                    list.Add(model);
                    #endregion
                }
            }
            return list;
        }

        public List<Model_dt_sm_Info> GetSMListByCode(string code)
        {

            List<Model_dt_sm_Info> list = new List<Model_dt_sm_Info>();
            var dt_sm_Info = SQLiteHelper.ExecuteDataRow("select * from dt_sm_Info where dt_Code='" + code + "' order by  info_DangH")?.Table;
            if (dt_sm_Info != null)
            {
                foreach (DataRow item in dt_sm_Info.Rows)
                {
                    #region
                    Model_dt_sm_Info model = new Model_dt_sm_Info();
                    model.dt_Code = item["dt_Code"].ToString();
                    model.info_DangH = item["info_DangH"].ToString();
                    model.sm_PaDesc = item["sm_PaDesc"].ToString();
                    model.sm_Pa = item["sm_Pa"].ToString();
                    model.sm_Remark = item["sm_Remark"].ToString();
                    model.Method = item["Method"].ToString();
                    model.testcount = int.Parse(item["testcount"].ToString());
                    model.sxyl = item["sxyl"].ToString();
                    model.xxyl = item["xxyl"].ToString();
                    model.gongchengjiance = item["gongchengjiance"].ToString();
                    list.Add(model);
                    #endregion
                }
            }
            return list;
        }

        /// <summary>
        /// 根据编号获取本次检测信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Model_dt_Settings GetInfoByCode(string code)
        {
            Model_dt_Settings settings = new Model_dt_Settings();

            var dt_settings = SQLiteHelper.ExecuteDataRow("select * from dt_settings where dt_Code='" + code + "'")?.Table;

            settings.weituobianhao = dt_settings.Rows[0]["weituobianhao"].ToString();
            settings.weituodanwei = dt_settings.Rows[0]["weituodanwei"].ToString();
            settings.dizhi = dt_settings.Rows[0]["dizhi"].ToString();
            settings.dianhua = dt_settings.Rows[0]["dianhua"].ToString();
            settings.chouyangriqi = dt_settings.Rows[0]["chouyangriqi"].ToString();
            settings.chouyangdidian = dt_settings.Rows[0]["chouyangdidian"].ToString();
            settings.gongchengmingcheng = dt_settings.Rows[0]["gongchengmingcheng"].ToString();
            settings.gongchengdidian = dt_settings.Rows[0]["gongchengdidian"].ToString();
            settings.shengchandanwei = dt_settings.Rows[0]["shengchandanwei"].ToString();
            settings.jiancexiangmu = dt_settings.Rows[0]["jiancexiangmu"].ToString();
            settings.jiancedidian = dt_settings.Rows[0]["jiancedidian"].ToString();
            settings.jianceriqi = dt_settings.Rows[0]["jianceriqi"].ToString();
            settings.jianceshebei = dt_settings.Rows[0]["jianceshebei"].ToString();
            settings.jianceyiju = dt_settings.Rows[0]["jianceyiju"].ToString();

            settings.yangpinmingcheng = dt_settings.Rows[0]["yangpinmingcheng"].ToString();
            settings.yangpinshangbiao = dt_settings.Rows[0]["yangpinshangbiao"].ToString();
            settings.yangpinzhuangtai = dt_settings.Rows[0]["yangpinzhuangtai"].ToString();
            settings.guigexinghao = dt_settings.Rows[0]["guigexinghao"].ToString();
            settings.kaiqifangshi = dt_settings.Rows[0]["kaiqifangshi"].ToString();
            settings.mianbanpinzhong = dt_settings.Rows[0]["mianbanpinzhong"].ToString();
            settings.zuidamianban = dt_settings.Rows[0]["zuidamianban"].ToString();
            settings.mianbanhoudu = dt_settings.Rows[0]["mianbanhoudu"].ToString();
            settings.anzhuangfangshi = dt_settings.Rows[0]["anzhuangfangshi"].ToString();
            settings.mianbanxiangqian = dt_settings.Rows[0]["mianbanxiangqian"].ToString();
            settings.kuangshanmifeng = dt_settings.Rows[0]["kuangshanmifeng"].ToString();
            settings.wujinpeijian = dt_settings.Rows[0]["wujinpeijian"].ToString();
            settings.jianceshuliang = dt_settings.Rows[0]["jianceshuliang"].ToString();
            settings.dangqiandanghao = dt_settings.Rows[0]["dangqiandanghao"].ToString();

            settings.dangqianwendu = dt_settings.Rows[0]["dangqianwendu"].ToString();
            settings.daqiyali = dt_settings.Rows[0]["daqiyali"].ToString();
            settings.kaiqifengchang = dt_settings.Rows[0]["kaiqifengchang"].ToString();
            settings.shijianmianji = dt_settings.Rows[0]["shijianmianji"].ToString();
            settings.ganjianchangdu = dt_settings.Rows[0]["ganjianchangdu"].ToString();
            settings.penlinshuiliang = dt_settings.Rows[0]["penlinshuiliang"].ToString();
            settings.qimidanweifengchangshejizhi = dt_settings.Rows[0]["qimidanweifengchangshejizhi"].ToString();
            settings.qimidanweimianjishejizhi = dt_settings.Rows[0]["qimidanweimianjishejizhi"].ToString();
            settings.shuimijingyashejizhi = dt_settings.Rows[0]["shuimijingyashejizhi"].ToString();
            settings.shuimidongyashejizhi = dt_settings.Rows[0]["shuimidongyashejizhi"].ToString();
            settings.kangfengyazhengyashejizhi = dt_settings.Rows[0]["kangfengyazhengyashejizhi"].ToString();
            settings.kangfengyafuyashejizhi = dt_settings.Rows[0]["kangfengyafuyashejizhi"].ToString();
            settings.danshandansuodian = dt_settings.Rows[0]["danshandansuodian"].ToString();
            settings.dt_Code = dt_settings.Rows[0]["dt_Code"].ToString();

            var dt_Info = SQLiteHelper.ExecuteDataRow("select * from dt_Info where dt_Code='" + code + "'   order by  info_DangH")?.Table;
            if (dt_Info != null)
            {
                List<Model_dt_Info> list = new List<Model_dt_Info>();
                foreach (DataRow item in dt_Info.Rows)
                {
                    #region
                    Model_dt_Info model = new Model_dt_Info();
                    model.dt_Code = item["dt_Code"].ToString();
                    model.info_Create = item["info_Create"].ToString();
                    model.info_DangH = item["info_DangH"].ToString();
                    model.Watertight = Convert.ToInt32(item["Watertight"].ToString());
                    model.WindPressure = Convert.ToInt32(item["WindPressure"].ToString());
                    model.Airtight = Convert.ToInt32(item["Airtight"].ToString());
                    list.Add(model);
                    #endregion
                }

                if (list.Count > 0)
                    settings.dt_InfoList = list;
            }
            var dt_qm_Info = SQLiteHelper.ExecuteDataRow("select * from dt_qm_Info where dt_Code='" + code + "' order by  info_DangH")?.Table;
            if (dt_qm_Info != null)
            {
                List<Model_dt_qm_Info> list = new List<Model_dt_qm_Info>();
                foreach (DataRow item in dt_qm_Info.Rows)
                {
                    #region
                    Model_dt_qm_Info model = new Model_dt_qm_Info();
                    model.dt_Code = item["dt_Code"].ToString();
                    model.info_DangH = item["info_DangH"].ToString();
                    model.qm_Z_FC = item["qm_Z_FC"].ToString();
                    model.qm_F_FC = item["qm_F_FC"].ToString();
                    model.qm_Z_MJ = item["qm_Z_MJ"].ToString();
                    model.qm_F_MJ = item["qm_F_MJ"].ToString();
                    model.qm_s_z_fj100 = item["qm_s_z_fj100"].ToString();
                    model.qm_s_z_fj150 = item["qm_s_z_fj150"].ToString();
                    model.qm_j_z_fj100 = item["qm_j_z_fj100"].ToString();
                    model.qm_s_z_zd100 = item["qm_s_z_zd100"].ToString();
                    model.qm_s_z_zd150 = item["qm_s_z_zd150"].ToString();
                    model.qm_j_z_zd100 = item["qm_j_z_zd100"].ToString();
                    model.qm_s_f_fj100 = item["qm_s_f_fj100"].ToString();
                    model.qm_s_f_fj150 = item["qm_s_f_fj150"].ToString();
                    model.qm_j_f_fj100 = item["qm_j_f_fj100"].ToString();
                    model.qm_s_f_zd100 = item["qm_s_f_zd100"].ToString();
                    model.qm_s_f_zd150 = item["qm_s_f_zd150"].ToString();
                    model.qm_j_f_zd100 = item["qm_j_f_zd100"].ToString();
                    list.Add(model);
                    #endregion
                }

                if (list.Count > 0)
                    settings.dt_qm_Info = list;
            }
            var dt_sm_Info = SQLiteHelper.ExecuteDataRow("select * from dt_sm_Info where dt_Code='" + code + "' order by  info_DangH")?.Table;
            if (dt_sm_Info != null)
            {
                List<Model_dt_sm_Info> list = new List<Model_dt_sm_Info>();
                foreach (DataRow item in dt_sm_Info.Rows)
                {
                    #region
                    Model_dt_sm_Info model = new Model_dt_sm_Info();
                    model.dt_Code = item["dt_Code"].ToString();
                    model.info_DangH = item["info_DangH"].ToString();
                    model.sm_PaDesc = item["sm_PaDesc"].ToString();
                    model.sm_Pa = item["sm_Pa"].ToString();
                    model.sm_Remark = item["sm_Remark"].ToString();
                    model.Method = item["Method"].ToString();
                    list.Add(model);
                    #endregion
                }
                if (list.Count > 0)
                    settings.dt_sm_Info = list;

            }
            var dt_kfy_Info = SQLiteHelper.ExecuteDataRow("select * from dt_kfy_Info where dt_Code='" + code + "' order by  info_DangH")?.Table;
            if (dt_kfy_Info != null)
            {
                List<Model_dt_kfy_Info> list = new List<Model_dt_kfy_Info>();
                foreach (DataRow item in dt_kfy_Info.Rows)
                {
                    #region
                    Model_dt_kfy_Info model = new Model.DataBase.Model_dt_kfy_Info();
                    model.z_one_250 = item["z_one_250"].ToString();
                    model.z_two_250 = item["z_two_250"].ToString();
                    model.z_three_250 = item["z_three_250"].ToString();
                    model.z_nd_250 = item["z_nd_250"].ToString();
                    model.z_ix_250 = item["z_ix_250"].ToString();
                    model.f_one_250 = item["f_one_250"].ToString();
                    model.f_two_250 = item["f_two_250"].ToString();
                    model.f_three_250 = item["f_three_250"].ToString();
                    model.f_nd_250 = item["f_nd_250"].ToString();
                    model.f_ix_250 = item["f_ix_250"].ToString();
                    model.z_one_500 = item["z_one_500"].ToString();
                    model.z_two_500 = item["z_two_500"].ToString();
                    model.z_three_500 = item["z_three_500"].ToString();
                    model.z_nd_500 = item["z_nd_500"].ToString();
                    model.z_ix_500 = item["z_ix_500"].ToString();
                    model.f_one_500 = item["f_one_500"].ToString();
                    model.f_two_500 = item["f_two_500"].ToString();
                    model.f_three_500 = item["f_three_500"].ToString();
                    model.f_nd_500 = item["f_nd_500"].ToString();
                    model.f_ix_500 = item["f_ix_500"].ToString();
                    model.z_one_750 = item["z_one_750"].ToString();
                    model.z_two_750 = item["z_two_750"].ToString();
                    model.z_three_750 = item["z_three_750"].ToString();
                    model.z_nd_750 = item["z_nd_750"].ToString();
                    model.z_ix_750 = item["z_ix_750"].ToString();
                    model.f_one_750 = item["f_one_750"].ToString();
                    model.f_two_750 = item["f_two_750"].ToString();
                    model.f_three_750 = item["f_three_750"].ToString();
                    model.f_nd_750 = item["f_nd_750"].ToString();
                    model.f_ix_750 = item["f_ix_750"].ToString();
                    model.z_one_1000 = item["z_one_1000"].ToString();
                    model.z_two_1000 = item["z_two_1000"].ToString();
                    model.z_three_1000 = item["z_three_1000"].ToString();
                    model.z_nd_1000 = item["z_nd_1000"].ToString();
                    model.z_ix_1000 = item["z_ix_1000"].ToString();
                    model.f_one_1000 = item["f_one_1000"].ToString();
                    model.f_two_1000 = item["f_two_1000"].ToString();
                    model.f_three_1000 = item["f_three_1000"].ToString();
                    model.f_nd_1000 = item["f_nd_1000"].ToString();
                    model.f_ix_1000 = item["f_ix_1000"].ToString();
                    model.z_one_1250 = item["z_one_1250"].ToString();
                    model.z_two_1250 = item["z_two_1250"].ToString();
                    model.z_three_1250 = item["z_three_1250"].ToString();
                    model.z_nd_1250 = item["z_nd_1250"].ToString();
                    model.z_ix_1250 = item["z_ix_1250"].ToString();
                    model.f_one_1250 = item["f_one_1250"].ToString();
                    model.f_two_1250 = item["f_two_1250"].ToString();
                    model.f_three_1250 = item["f_three_1250"].ToString();
                    model.f_nd_1250 = item["f_nd_1250"].ToString();
                    model.f_ix_1250 = item["f_ix_1250"].ToString();
                    model.z_one_1500 = item["z_one_1500"].ToString();
                    model.z_two_1500 = item["z_two_1500"].ToString();
                    model.z_three_1500 = item["z_three_1500"].ToString();
                    model.z_nd_1500 = item["z_nd_1500"].ToString();
                    model.z_ix_1500 = item["z_ix_1500"].ToString();
                    model.f_one_1500 = item["f_one_1500"].ToString();
                    model.f_two_1500 = item["f_two_1500"].ToString();
                    model.f_three_1500 = item["f_three_1500"].ToString();
                    model.f_nd_1500 = item["f_nd_1500"].ToString();
                    model.f_ix_1500 = item["f_ix_1500"].ToString();
                    model.z_one_1750 = item["z_one_1750"].ToString();
                    model.z_two_1750 = item["z_two_1750"].ToString();
                    model.z_three_1750 = item["z_three_1750"].ToString();
                    model.z_nd_1750 = item["z_nd_1750"].ToString();
                    model.z_ix_1750 = item["z_ix_1750"].ToString();
                    model.f_one_1750 = item["f_one_1750"].ToString();
                    model.f_two_1750 = item["f_two_1750"].ToString();
                    model.f_three_1750 = item["f_three_1750"].ToString();
                    model.f_nd_1750 = item["f_nd_1750"].ToString();
                    model.f_ix_1750 = item["f_ix_1750"].ToString();
                    model.z_one_2000 = item["z_one_2000"].ToString();
                    model.z_two_2000 = item["z_two_2000"].ToString();
                    model.z_three_2000 = item["z_three_2000"].ToString();
                    model.z_nd_2000 = item["z_nd_2000"].ToString();
                    model.z_ix_2000 = item["z_ix_2000"].ToString();
                    model.f_one_2000 = item["f_one_2000"].ToString();
                    model.f_two_2000 = item["f_two_2000"].ToString();
                    model.f_three_2000 = item["f_three_2000"].ToString();
                    model.f_nd_2000 = item["f_nd_2000"].ToString();
                    model.f_ix_2000 = item["f_ix_2000"].ToString();



                    model.p1 = item["p1"].ToString();
                    model.p2 = item["p2"].ToString();
                    model.p3 = item["p3"].ToString();
                    model.pMax = item["z_pMax"].ToString();
                    model._p1 = item["_p1"].ToString();
                    model._p2 = item["_p2"].ToString();
                    model._p3 = item["_p3"].ToString();
                    model._pMax = item["f_pMax"].ToString();
                    model.CheckLock = int.Parse(item["CheckLock"].ToString());
                    model.info_DangH = item["info_DangH"].ToString();
                    model.testtype = int.Parse(item["testtype"].ToString());

                    list.Add(model);
                    #endregion
                }
                if (list.Count > 0)
                    settings.dt_kfy_Info = list;
            }
            return settings;
        }


        /// <summary>
        /// 查询编号是否存在
        /// </summary>
        public bool Getdt_SettingsByCodeIsExist(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }
            string sql = "select * from dt_Settings where  dt_Code = '" + code + "'";
            DataRow dr = SQLiteHelper.ExecuteDataRow(sql);
            if (dr == null)
                return false;
            return true;
        }

    }
}
