using text.doors.Common;
using text.doors.Model.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text.doors.Default;
using Young.Core.SQLite;
using text.doors.Service;
using NPOI.SS.Formula.Functions;

namespace text.doors.dal
{
    public class DAL_dt_qm_Info
    {
        /// <summary>
        /// 添加气密信息
        /// </summary>
        /// <param name="mode"></param>
        public bool Add(Model_dt_qm_Info model)
        {
            //删除结果
            SQLiteHelper.ExecuteNonQuery("delete from dt_qm_Info where testcount= " + model.testcount + "  and dt_Code='" + model.dt_Code + "' and info_DangH = '" + model.info_DangH + "'");

            var sql = string.Format(@"insert into dt_qm_Info (dt_Code,info_DangH,qm_Z_FC,qm_F_FC,qm_Z_MJ,qm_F_MJ,
                        qm_s_z_fj10  ,
                        qm_s_z_fj30  ,
                        qm_s_z_fj50  ,
                        qm_s_z_fj70  ,
                        qm_s_z_fj100 ,
                        qm_s_z_fj150 ,
                        qm_j_z_fj100 ,
                        qm_j_z_fj70  ,
                        qm_j_z_fj50  ,
                        qm_j_z_fj30  ,
                        qm_j_z_fj10  ,
             
                        qm_s_z_zd10  ,
                        qm_s_z_zd30  ,
                        qm_s_z_zd50  ,
                        qm_s_z_zd70  ,
                        qm_s_z_zd100 ,
                        qm_s_z_zd150 ,
                        qm_j_z_zd100 ,
                        qm_j_z_zd70  ,
                        qm_j_z_zd50  ,
                        qm_j_z_zd30  ,
                        qm_j_z_zd10  ,
             
                        qm_s_f_fj10  ,
                        qm_s_f_fj30  ,
                        qm_s_f_fj50  ,
                        qm_s_f_fj70  ,
                        qm_s_f_fj100 ,
                        qm_s_f_fj150 ,
                        qm_j_f_fj100 ,
                        qm_j_f_fj70  ,
                        qm_j_f_fj50  ,
                        qm_j_f_fj30  ,
                        qm_j_f_fj10  ,
             
                        qm_s_f_zd10  ,
                        qm_s_f_zd30  ,
                        qm_s_f_zd50  ,
                        qm_s_f_zd70  ,
                        qm_s_f_zd100 ,
                        qm_s_f_zd150 ,
                        qm_j_f_zd100 ,
                        qm_j_f_zd70  ,
                        qm_j_f_zd50  ,
                        qm_j_f_zd30  ,
                        qm_j_f_zd10  ,
                        testcount,
                        testtype,
                        sjz_value,
                        sjz_z_fj,
                        sjz_z_zd,
                        sjz_f_fj,
                        sjz_f_zd
                        ) 
                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}'
                      ,'{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}',{50},'{51}','{52}','{53}','{54}','{55}','{56}')",
                 model.dt_Code, model.info_DangH, model.qm_Z_FC, model.qm_F_FC, model.qm_Z_MJ, model.qm_F_MJ,
                 model.qm_s_z_fj10,
                 model.qm_s_z_fj30,
                 model.qm_s_z_fj50,
                 model.qm_s_z_fj70,
                 model.qm_s_z_fj100,
                 model.qm_s_z_fj150,
                 model.qm_j_z_fj100,
                 model.qm_j_z_fj70,
                 model.qm_j_z_fj50,
                 model.qm_j_z_fj30,
                 model.qm_j_z_fj10,

                 model.qm_s_z_zd10,
                 model.qm_s_z_zd30,
                 model.qm_s_z_zd50,
                 model.qm_s_z_zd70,
                 model.qm_s_z_zd100,
                 model.qm_s_z_zd150,
                 model.qm_j_z_zd100,
                 model.qm_j_z_zd70,
                 model.qm_j_z_zd50,
                 model.qm_j_z_zd30,
                 model.qm_j_z_zd10,

                 model.qm_s_f_fj10,
                 model.qm_s_f_fj30,
                 model.qm_s_f_fj50,
                 model.qm_s_f_fj70,
                 model.qm_s_f_fj100,
                 model.qm_s_f_fj150,
                 model.qm_j_f_fj100,
                 model.qm_j_f_fj70,
                 model.qm_j_f_fj50,
                 model.qm_j_f_fj30,
                 model.qm_j_f_fj10,

                 model.qm_s_f_zd10,
                 model.qm_s_f_zd30,
                 model.qm_s_f_zd50,
                 model.qm_s_f_zd70,
                 model.qm_s_f_zd100,
                 model.qm_s_f_zd150,
                 model.qm_j_f_zd100,
                 model.qm_j_f_zd70,
                 model.qm_j_f_zd50,
                 model.qm_j_f_zd30,
                 model.qm_j_f_zd10,
                 model.testcount,
                 model.testtype,
                 model.sjz_value,
                 model.sjz_z_fj,
                 model.sjz_z_zd,
                 model.sjz_f_fj,
                 model.sjz_f_zd
                 );
            var res = SQLiteHelper.ExecuteNonQuery(sql) > 0 ? true : false;
            if (res)
            {
                new DAL_dt_Info().UpdateTestType(model.dt_Code, model.info_DangH, PublicEnum.SystemItem.Airtight, 1);
            }
            return true;
        }



        public void UpdateResult(Model_dt_Settings settings)
        {
            if (settings.dt_sm_Info != null && settings.dt_sm_Info.Count > 0)
            {
                var sql = "";
                foreach (var item in settings.dt_sm_Info)
                {
                    sql += $"update  dt_sm_Info set sm_PaDesc = '{item.sm_PaDesc}', sm_Pa='{item.sm_Pa}', sm_Remark='{item.sm_Remark}' where dt_Code ='{item.dt_Code}' and info_DangH='{item.info_DangH}';";
                }
                SQLiteHelper.ExecuteNonQuery(sql);
            }
            if (settings.dt_qm_Info != null && settings.dt_qm_Info.Count > 0)
            {
                string sql = "";
                foreach (var item in settings.dt_qm_Info)
                {
                    sql += $"update dt_qm_Info  set qm_Z_FC	='{item.qm_Z_FC}',qm_F_FC ='{item.qm_F_FC}',qm_Z_MJ	='{item.qm_Z_MJ}',qm_F_MJ	='{item.qm_F_MJ}' where dt_Code = '{item.dt_Code}' and info_DangH='{item.info_DangH}' ;   ";
                }
                SQLiteHelper.ExecuteNonQuery(sql);
            }
            if (settings.dt_kfy_Info != null && settings.dt_kfy_Info.Count > 0)
            {
                string sql = "";
                foreach (var item in settings.dt_kfy_Info)
                {
                    sql += $"update dt_kfy_Info  set p1	='{item.p1}',p2 ='{item.p2}',p3='{item.p3}',_p1='{item._p1}' ,_p2='{item._p2}',_p3='{item._p3}' where dt_Code = '{item.dt_Code}' and info_DangH='{item.info_DangH}' ;   ";
                }
                SQLiteHelper.ExecuteNonQuery(sql);
            }
        }
    }
}
